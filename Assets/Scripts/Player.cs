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

    [SerializeField]
    private AProjectile projectilePrefab;

    private bool inputBlocked = false;

    private SpriteRenderer[] spriteRenderers;
    private Color[] normalSpriteColors;

	// Use this for initialization
	void Start () 
    {
        string indexIdString = ((int)index).ToString();

        moveXAxisInput += indexIdString;
        jumpInput += indexIdString;
        lookXInput += indexIdString;
        lookYInput += indexIdString;
        fire0Input += indexIdString;

        health.onZeroHealth.AddListener(OnZeroHealth);

        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        normalSpriteColors = new Color[spriteRenderers.Length];

        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            normalSpriteColors[i] = spriteRenderers[i].color;
        }
	}

    private void OnZeroHealth(Health h, Health.EventInfo info)
    {
        StartCoroutine(Respawn());
    }

    private IEnumerator Respawn()
    {
        inputBlocked = true;
        actor.ResetMovement();

        int oldLayer = health.gameObject.layer;

        health.gameObject.layer = LayerUtil.NoneNumber;

        onKilled.Invoke(this);

        ApplyColors(0.5f);

        yield return new WaitForSeconds(GameManager.Instance.GameOptions.RespawnCooldown);

        health.Refill();

        health.gameObject.layer = oldLayer;

        inputBlocked = false;

        ApplyColors(1.0f);

        onRespawn.Invoke(this);

        yield return Invi(GameManager.Instance.GameOptions.RespawnInviTime, true);
    }

    private IEnumerator Invi(float inviTime, bool speedup)
    {
        int oldLayer = health.gameObject.layer;
        isInvi = true;

        health.gameObject.layer = LayerUtil.NoneNumber;

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
        health.gameObject.layer = oldLayer;
        isInvi = false;
    }

	// Update is called once per frame
	private void Update () 
    {
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

        if(Input.GetButtonDown(fire0Input))
        {
            AProjectile instance = Instantiate(projectilePrefab);
            
            instance.gameObject.transform.position = actor.Center.transform.position;
            instance.InitProjectile(actor, actor.LookDirection.normalized);
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

    [System.Serializable]
    public class Statistics
    {
        public int deaths = 0;
        public int kills = 0;

        public int gainedPoints = 0;
        public int lostPoints = 0;
    }
}
