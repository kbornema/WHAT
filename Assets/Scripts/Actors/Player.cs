using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour 
{
    public class Event : UnityEvent<Player> { }

    [SerializeField]
    private Actor actor;
    public Actor TheActor { get { return actor; } }
    

    [SerializeField]
    private AWeapon weaponLeft;
    public AWeapon WeaponLeft { get { return weaponLeft; } set { weaponLeft = value; } }

    [SerializeField]
    private AWeapon weaponRight;
    public AWeapon WeaponRight { get { return weaponRight; } set { weaponRight = value; } }

    [SerializeField]
    private Health health;
    public Health TheHealth { get { return health; } }

    private bool isInvi = false;
    public bool IsInvi { get { return isInvi; } }

    public enum Index { One = 0, Two = 1, Three = 2, Four = 3, Count}

    [SerializeField]
    private Index index;
    public Index PlayerIndex { get { return index; } set { index = value; } }

    [SerializeField]
    private float padDeadzone = 0.5f;

    [SerializeField, ReadOnly]
    private Statistics statistics;
    public Statistics Stats { get { return statistics; } }

    [HideInInspector]
    public Event onKilled = new Event();
    [HideInInspector]
    public Event onRespawn = new Event();
    
    private string moveXAxisInput = "MoveX_";
    private string jumpInput = "Jump_";
    private string lookXInput = "LookX_";
    private string lookYInput = "LookY_";
    private string fire0Input = "Fire0_";
    private string fire1Input = "Fire1_";

    private bool inputBlocked = false;

    public bool IsDead { get; private set; }

    private SpriteRenderer[] spriteRenderers;
    private Color[] normalSpriteColors;


    [SerializeField]
    GameObject gam;
    SetHeadDing setHeadDing;

	// Use this for initialization
	void Start () 
    {
        setHeadDing = gam.GetComponent<SetHeadDing>();
        setHeadDing.AciveHeadDing(PlayerIndex);
        //setHeadDing.CurBomb(GetBomb());
        string indexIdString = ((int)index).ToString();

        moveXAxisInput += indexIdString;
        jumpInput += indexIdString;
        lookXInput += indexIdString;
        lookYInput += indexIdString;
        fire0Input += indexIdString;
        fire1Input += indexIdString;

        health.onZeroHealth.AddListener(OnZeroHealth);
        health.onHealthChanged.AddListener(OnHealthChanged);

        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        normalSpriteColors = new Color[spriteRenderers.Length];

        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            normalSpriteColors[i] = spriteRenderers[i].color;
        }
	}
    public int GetBomb()
    {
        return (weaponRight as SimpleProjectileWeapon).CurAmmo;
    }

    private void OnHealthChanged(Health h, Health.EventInfo info)
    {
        if(info.delta < 0.0f)
            GameManager.Instance.GameCam.Shake(0.25f, 0.25f);
    }

    private void OnZeroHealth(Health h, Health.EventInfo info)
    {
        StartCoroutine(Respawn());
    }

    private IEnumerator Respawn()
    {
        IsDead = true;
        inputBlocked = true;
        actor.ResetMovement();

        health.gameObject.layer = LayerUtil.NoneNumber;
        setHeadDing.DeactiveHeadDing(PlayerIndex);
        onKilled.Invoke(this);
        //aktiv

        

        ApplyColors(0.0f);
        //headDing.color = Color.white;
        
        yield return new WaitForSeconds(GameManager.Instance.GameOptions.RespawnCooldown);

        health.Refill();

        inputBlocked = false;

        ApplyColors(1.0f);
        //deaktivieren
        
        onRespawn.Invoke(this);

        actor.gameObject.transform.position = GameManager.Instance.GetPlayerSpawnPos();

        yield return Invi(GameManager.Instance.GameOptions.RespawnInviTime, true);

        IsDead = false;
    }

    private IEnumerator Invi(float inviTime, bool speedup)
    {
        isInvi = true;

        float time = 0.0f;

        float blinkCd = 0.0f;
        float blinkMaxCd = 0.25f;
        float respawnT = 0.0f;

        bool ping = false;

        while (time < inviTime)
        {
            time += Time.deltaTime;

            blinkCd += Time.deltaTime;

            respawnT = time / inviTime;

            if (speedup)
                blinkMaxCd = Mathf.Lerp(0.25f, 0.025f, respawnT);

            if(blinkCd >= blinkMaxCd)
            {
                blinkCd = 0.0f;

                if (ping)
                    ApplyColors(0.5f);
                else 
                    ApplyColors(1.0f);

                ping = !ping;
            }

            yield return new WaitForEndOfFrame();
        }
        
        ApplyColors(1.0f);
        health.gameObject.layer = LayerUtil.DamagableNumber;
        setHeadDing.AciveHeadDing(PlayerIndex);
        isInvi = false;

    }


	// Update is called once per frame
	private void Update () 
    {
        setHeadDing.CurBomb(GetBomb());
        //setHeadDing.UpdatePosition(transform.position);
        if (inputBlocked)
            return;

        float xAxis = Input.GetAxis(moveXAxisInput);

        actor.SetMoveX(xAxis);

        if (Input.GetButtonDown(jumpInput))
            actor.Jump();

        Vector2 lookDir = new Vector2(Input.GetAxis(lookXInput), Input.GetAxis(lookYInput));

        float lookVal = lookDir.magnitude;
        
        if (lookVal > padDeadzone)
        {
            actor.LookDirection = (lookDir / lookVal);
        }
        
        HandleWeapon();
        
	}

    private void HandleWeapon()
    {
        if(weaponLeft)
        {
            if (Input.GetButtonDown(fire0Input) && weaponLeft.Mode == AWeapon.FireMode.Click)
            {
                weaponLeft.TryShoot(actor, actor.LookDirection.normalized);
            }

            else if (Input.GetButton(fire0Input) && weaponLeft.Mode == AWeapon.FireMode.Press)
            {
                weaponLeft.TryShoot(actor, actor.LookDirection.normalized);
            }

            else if (Input.GetButtonUp(fire0Input) && weaponLeft.Mode == AWeapon.FireMode.Release)
            {
                weaponLeft.TryShoot(actor, actor.LookDirection.normalized);
            }
        }
    
        if(weaponRight)
        {
            if (Input.GetButtonDown(fire1Input) && weaponRight.Mode == AWeapon.FireMode.Click)
            {
                weaponRight.TryShoot(actor, actor.LookDirection.normalized);
            }

            else if (Input.GetButton(fire1Input) && weaponRight.Mode == AWeapon.FireMode.Press)
            {
                weaponRight.TryShoot(actor, actor.LookDirection.normalized);
            }

            else if (Input.GetButtonUp(fire1Input) && weaponRight.Mode == AWeapon.FireMode.Release)
            {
                weaponRight.TryShoot(actor, actor.LookDirection.normalized);
            }
        }
    }

    private void ApplyColors(float alpha)
    {
        for (int i = 0; i < normalSpriteColors.Length; i++)
        {
            Color c = normalSpriteColors[i];

            c.a *= alpha;

            spriteRenderers[i].color = c;
        }
    }

    public void Kill()
    {
        health.ApplyHealth(new Health.EventInfo(-(health.MaxHitpoints + 0.5f), null));
    }

    [System.Serializable]
    public class Statistics
    {
        public int deaths = 0;
        public int kills = 0;

        public int gainedPoints = 0;
        public int lostPoints = 0;
    }
}
