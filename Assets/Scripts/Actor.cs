using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour 
{
    public class Event : UnityEngine.Events.UnityEvent<Actor> { }

    [SerializeField]
    private GameObject _center;
    public GameObject Center { get { return _center; } }

    [SerializeField]
    private GameObject _feet;
    public GameObject Feet { get { return _feet; } }

    [SerializeField]
    private ContactDetector groundDetector;
    public ContactDetector GroundDetector { get { return groundDetector; } }

    public bool IsGrounded { get { return groundDetector.IsGrounded; } }

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

    /// <summary>Is called when the movingSpeed changes or when the moveDirection changes.</summary>
    [HideInInspector] 
    public Event onMovementChanged = new Event();
    /// <summary>Is called when the lookDirection changes.</summary>
    [HideInInspector] 
    public Event onLookDirChanged = new Event();

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
        UpdateIsMoving();

        groundDetector.onGrounded.AddListener(OnGrounded);
    }

    private void OnGrounded(ContactDetector arg0)
    {
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

        AddForce(new Vector2(0, jumpPower), ForceMode2D.Impulse);
        jumpCount++;
    }

    public void ResetMovement()
    {
        this._moveDirection = Vector2.zero;
        this._myRigidbody.velocity = Vector2.zero;
    }
}
