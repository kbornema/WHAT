using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour 
{
    public class Event : UnityEngine.Events.UnityEvent<Actor> { }

    [SerializeField]
    private ActorAnimator actorAnimator;
    public ActorAnimator TheAnimator { get { return actorAnimator; } }

    [SerializeField]
    private Player player;
    public Player ThePlayer { get { return player; } }

    [SerializeField]
    private Health health;
    public Health TheHealth { get { return health; } }

    [SerializeField]
    private GameObject _center;
    public GameObject Center { get { return _center; } }

    [SerializeField]
    private GameObject _weaponRoot;
    public GameObject WeaponRoot { get { return _weaponRoot; } }

    [SerializeField]
    private GameObject _feet;
    public GameObject Feet { get { return _feet; } }

    [SerializeField]
    private ContactDetector groundDetector;
    public ContactDetector GroundDetector { get { return groundDetector; } }

    [SerializeField]
    private float jumpPower = 10.0f;

    [SerializeField] 
    private Rigidbody2D _myRigidbody;
    [SerializeField] 
    private Collider2D _physicalCollider;

    [SerializeField]
    private int maxJumpCount = 3;
    [SerializeField, ReadOnly]
    private int jumpCount = 0;

    [SerializeField]
    private int pointsOnKill;
    public int PointsOnKill { get { return pointsOnKill; } }

    /// <summary>Is called when the movingSpeed changes or when the moveDirection changes.</summary>
    [HideInInspector] 
    public Event onMovementChanged = new Event();
    /// <summary>Is called when the lookDirection changes.</summary>
    [HideInInspector] 
    public Event onLookDirChanged = new Event();

    private List<AWeapon> weapons = new List<AWeapon>();

    private bool isGrounded = false;

    private bool checkinUngrounded = true;

    [SerializeField]
    private float maxVeloX = 10.0f;
    [SerializeField]
    private float maxVeloY = 15.0f;

    public List<AWeapon> GetWeapons()
    {
        return weapons;
    }

    public void AddWeapon(AWeapon weapon)
    {
        Debug.Assert(!weapons.Contains(weapon));
        weapons.Add(weapon);

        weapon.gameObject.transform.SetParent(_weaponRoot.transform);
        weapon.gameObject.transform.localPosition = Vector3.zero;
    }

    public void Remove(AWeapon weapon)
    {
        Debug.Assert(weapons.Contains(weapon));
        weapons.Remove(weapon);

        Destroy(weapon.gameObject);
    }

    private Vector2 _lookDirection = new Vector2(0, 1.0f);
    public Vector2 LookDirection { get { return _lookDirection; } set { SetLookDir(value); } }
    public void SetLookDir(Vector2 lookDir)
    {
        if (lookDir == this._lookDirection)
            return;

        this._lookDirection = lookDir;
        this.onLookDirChanged.Invoke(this);
    }

    private Vector2 _moveDirection;
    public Vector2 MoveDirection { get { return _moveDirection; } set { SetMoveDir(value); } }
    public void SetMoveDir(Vector2 moveDir)
    {
        if (moveDir == this._moveDirection)
            return;

        this._moveDirection = moveDir;
        UpdateIsMoving();
        this.onMovementChanged.Invoke(this);
    }

    public void SetMoveX(float x)
    {
        if (this._moveDirection.x == x)
            return;

        else
        {
            this._moveDirection.x = x;
            UpdateIsMoving();
            this.onMovementChanged.Invoke(this);
        }
    }

    [SerializeField] 
    private float _moveSpeed = 1.0f;
    public float MoveSpeed { get { return _moveSpeed; } set { SetMoveSpeed(value); } }
    public void SetMoveSpeed(float speed)
    {
        if (speed == this._moveSpeed)
            return;

        this._moveSpeed = speed;
        UpdateIsMoving();
        this.onMovementChanged.Invoke(this);
    }

    private void UpdateIsMoving()
    {
        this._isMoving = this._moveSpeed != 0.0f && (this._moveDirection.x != 0.0f || this._moveDirection.y != 0.0f);

        if(actorAnimator)
            actorAnimator.SetMove(this._isMoving);
    }

    private bool _isMoving;
    public bool IsMoving { get { return _isMoving; } }

    public void AddForce(Vector2 dir, float force, ForceMode2D mode = ForceMode2D.Impulse)
    {
        AddForce(dir * force, mode);
    }

    public void AddForce(Vector2 force, ForceMode2D mode = ForceMode2D.Impulse)
    {
        this._myRigidbody.AddForce(force, mode);

        Vector2 velo = this._myRigidbody.velocity;

        velo.x = Mathf.Clamp(velo.x, -maxVeloX, maxVeloX);
        velo.y = Mathf.Clamp(velo.y, -maxVeloY, maxVeloY);

        this._myRigidbody.velocity = velo;
    }

    //might be changed to virtual and protected:
    private void Reset()
    {
        if (_myRigidbody == null)
            _myRigidbody = GetComponent<Rigidbody2D>();

        if (_physicalCollider == null)
            _physicalCollider = GetComponent<Collider2D>();

        if (_feet == null)
            _feet = gameObject;

        if (_center == null)
            _center = gameObject;
    }

    protected virtual void Start()
    {
        if(actorAnimator)
            StartCoroutine(SetUngroundedIn());

        UpdateIsMoving();

        groundDetector.onGrounded.AddListener(OnGrounded);
        groundDetector.onUngrounded.AddListener(OnUngrounded);
    }

    private void OnUngrounded(ContactDetector arg0)
    {
        if (actorAnimator && !checkinUngrounded)
        {
            if(!arg0.IsGrounded)
            {
                this.isGrounded = false;
            }
        }
            
    }

    private IEnumerator SetUngroundedIn()
    {
        while (checkinUngrounded)
        {
            while (this.isGrounded)
            {
                yield return new WaitForEndOfFrame();
            }

            yield return new WaitForSeconds(0.25f);

            if (!this.isGrounded)
            {
                actorAnimator.SetGrounded(false);
            }
        }
    }

    private void OnGrounded(ContactDetector arg0)
    {
        isGrounded = true;

        if (actorAnimator)
        {
            actorAnimator.SetGrounded(true);
        }

        jumpCount = 0;
    }

    protected virtual void FixedUpdate()
    {
        if(_isMoving)
        {
            _myRigidbody.position += _moveDirection * (_moveSpeed * Time.fixedDeltaTime);
        }
    }

    public void Jump()
    {
        if (jumpCount >= maxJumpCount)
            return;

        _myRigidbody.velocity = new Vector2(_myRigidbody.velocity.x, jumpPower);

        jumpCount++;
        if(player != null)
            SoundManager.Instance.StartSingleSoundRandomPitch(SoundManager.Sound.Jump, 0.8f);

        if (actorAnimator)
        {
            actorAnimator.TriggerJump();
            actorAnimator.SetGrounded(false);
        }
    }

    public void ResetMovement()
    {
        this._moveDirection = Vector2.zero;
        this._myRigidbody.velocity = Vector2.zero;
    }
}
