using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : AManager<GameManager>
{
    public class Event : UnityEvent<GameManager> { }

    [SerializeField]
    private float worldGravityScale = 1.0f;
    public float WorldGravityScale { get { return worldGravityScale; } set { worldGravityScale = value; onGravityChanged.Invoke(this); } }

    [SerializeField]
    private int targetFps = 60;

    [SerializeField]
    private float normalTimeScale = 1.5f;
    public float NormalTimeScale { get { return normalTimeScale; } }

    [SerializeField]
    private GameCamera gameCam;
    public GameCamera GameCam { get { return gameCam; } }

    [SerializeField]
    private Player playerPrefab;
    [SerializeField]
    private RuntimeAnimatorController[] playerControllers;

    [SerializeField]
    private GameObject acidRoot;
    public GameObject AcidRoot { get { return acidRoot; } }

    [SerializeField]
    private Options options;
    public Options GameOptions { get { return options; } }

    [SerializeField]
    private EnemyPrefabs enemyPrefabs;

    [SerializeField]
    private DifficultySettings settings;
    public DifficultySettings Settings { get { return settings; } }
    
    private int numPlayers = 0;
    private Player[] players = new Player[(int)Player.Index.Count];
    public int NumPlayers { get { return numPlayers; } }

    private string playerStartInput = "Start_";
    
    [HideInInspector]
    public Event onGravityChanged = new Event();

    private List<EnemySpawner> enemySpawners = new List<EnemySpawner>();
    private List<PlayerSpawn> playerSpawner = new List<PlayerSpawn>();

    private bool isSpawning = true;
    
    private float difficulty = 0.0f;
    public float Difficulty { get { return difficulty; } }

    private float time = 0.0f;

    private int enemyCount = 0;

    protected override void OnAwake()
    {
        Application.targetFrameRate = targetFps;
        Time.timeScale = normalTimeScale;
        StartCoroutine(SpawnRoutine());
    }

    private void Update()
    {
        CheckSpawnNewPlayer();

        if (Input.GetKeyDown(KeyCode.P))
            SpawnEnemyRandomPlace(enemyPrefabs.normalTurtle);

        time += Time.deltaTime;
        
        if(difficulty < 1.0f)
        {
            difficulty = time / options.TimeToFullDifficulty;

            if (difficulty >= 1.0f)
                difficulty = 1.0f;
        }
    }

    private IEnumerator SpawnRoutine()
    {
        while(isSpawning)
        {
            float cooldown = settings.GetSpawnCooldown(difficulty);
            yield return new WaitForSeconds(cooldown);

            int numSpawns = settings.GetNumSpawns(difficulty);

            for (int i = 0; i < numSpawns; i++)
            {
                yield return new WaitForSeconds(Random.Range(0.0f, 0.25f));
                SpawnEnemyRandom();  
            }
        }

    }

    private void CheckSpawnNewPlayer()
    {
        for (int i = 0; i < (int)Player.Index.Count; i++)
        {
            if (players[i] == null && Input.GetButtonDown(playerStartInput + i.ToString()))
            {
                Player instance = Instantiate(playerPrefab);

                instance.gameObject.transform.position = GetPlayerSpawnPos();
                instance.PlayerIndex = (Player.Index)i;
                players[i] = instance;
                numPlayers++;
                OnNewPlayerSpawned(instance, i);
            }
        }
    }

    private void OnNewPlayerSpawned(Player player, int i)
    {
        player.TheActor.TheAnimator.TheAnimator.runtimeAnimatorController = playerControllers[i];

        player.onKilled.AddListener(OnPlayerDeath);
        
        AWeapon weaponInstance = Instantiate(options.PlayerStartWeaponPrefab);
        player.TheActor.AddWeapon(weaponInstance);
        player.Weapon = weaponInstance;
    }

    private void OnPlayerDeath(Player p)
    {
        p.Stats.deaths++;
        p.Stats.lostPoints += options.DeathPenality;

        if(CheckAllPlayerDead())
        {
            Debug.Log("all dead!");
        }
    }

    private bool CheckAllPlayerDead()
    {
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i] != null && !players[i].IsDead)
            {
                return false;
            }
        }

        return true;
    }

    private void SpawnEnemyRandomPlace(Actor prefab)
    {
        SpawnEnemy(prefab, Random.Range(0, enemySpawners.Count));
    }

    public void SpawnEnemyRandom()
    {
        Actor prefab = enemyPrefabs.GetRandom(difficulty);
        SpawnEnemy(prefab, Random.Range(0, enemySpawners.Count));
    }

    public void SpawnNormalTurtle()
    {
        SpawnEnemy(enemyPrefabs.normalTurtle, Random.Range(0, enemySpawners.Count));
    }

    private void SpawnEnemy(Actor prefab, int spawnId)
    {
        if(enemyCount < options.MaxEnemies)
        {
            Actor instance = enemySpawners[spawnId].Spawn(prefab);

            Vector3 scale = instance.gameObject.transform.localScale;

            float scaleScale = Random.Range(0.80f, 1.10f);
            instance.gameObject.transform.localScale = scale * scaleScale;

            enemyCount++;
            instance.TheHealth.onZeroHealth.AddListener(OnEnemyKilled);
        }
    }

    private void OnEnemyKilled(Health enemyHealth, Health.EventInfo info)
    {
        if(info.source && info.source.ThePlayer)
        {
            info.source.ThePlayer.Stats.kills++;

            if(enemyHealth.RootActor)
                info.source.ThePlayer.Stats.gainedPoints += enemyHealth.RootActor.PointsOnKill;
        }

        enemyCount--;
    }
    
    public void RegisterPlayerSpawn(PlayerSpawn playerSpawn)
    {
        playerSpawner.Add(playerSpawn);
    }

    public Vector3 GetPlayerSpawnPos()
    {
        return playerSpawner[Random.Range(0, playerSpawner.Count)].transform.position;
    }

    public void RegisterEnemySpawner(EnemySpawner spawner)
    {
        enemySpawners.Add(spawner);
    }

    [System.Serializable]
    public class Options
    {
        [SerializeField]
        private int maxEnemies = 25;
        public int MaxEnemies { get { return maxEnemies; } }

        [SerializeField]
        private AWeapon playerStartWeaponPrefab;
        public AWeapon PlayerStartWeaponPrefab { get { return playerStartWeaponPrefab; } }

        [SerializeField]
        private int deathPenality = -5;
        public int DeathPenality { get { return deathPenality; } }

        [SerializeField]
        private float respawnCooldown = 5.0f;
        public float RespawnCooldown { get { return respawnCooldown; } }

        [SerializeField]
        private float respawnInviTime = 5.0f;
        public float RespawnInviTime { get { return respawnInviTime; } }

        [SerializeField]
        private float timeToFullDifficulty = 300.0f;
        public float TimeToFullDifficulty { get { return timeToFullDifficulty; } }
    }

    [System.Serializable]
    public class DifficultySettings
    {
        [Header("Start")]
        [SerializeField]
        private int spawnsMinStart = 1;
        [SerializeField]
        private int spawnsMaxStart = 1;

        [SerializeField]
        private float spawnCdMinStart = 3.0f;
        [SerializeField]
        private float spawnCdMaxStart = 6.0f;

        [Header("End")]
        [SerializeField]
        private int spawnsMinEnd = 1;
        [SerializeField]
        private int spawnsMaxEnd = 1;

        [SerializeField]
        private float spawnCdMinEnd = 3.0f;
        [SerializeField]
        private float spawnCdMaxEnd = 6.0f;

        [SerializeField]
        private float timeBetweenEventsStart = 5.0f;
        [SerializeField]
        private float timeBetweenEventsEnd = -5.0f;

        public float GetSpawnCooldown(float difficulty)
        {
            float min = Mathf.Lerp(spawnCdMinStart, spawnCdMinEnd, difficulty);
            float max = Mathf.Lerp(spawnCdMaxStart, spawnCdMaxEnd, difficulty);

            return Random.Range(min, max);
        }

        public int GetNumSpawns(float difficulty)
        {
            int min = (int)Mathf.Lerp(spawnsMinStart, spawnsMinEnd, difficulty);
            int max = (int)Mathf.Lerp(spawnsMaxStart, spawnsMaxEnd, difficulty);

            return Random.Range(min, max + 1);
        }

        public float GetTimeBetweenEvents(float difficulty)
        {
            return Mathf.Lerp(timeBetweenEventsStart, timeBetweenEventsEnd, difficulty);
        }
    }

    [System.Serializable]
    public class EnemyPrefabs
    {
        public float bombChance = 0.15f;
        public float bombIncrease = 0.25f;

        public Actor bombTurtle;
        public Actor normalTurtle;
        
        public Actor GetRandom(float difficulty)
        {
            float rand = Random.value;

            if (rand < bombChance + difficulty * bombIncrease)
                return bombTurtle;

            return normalTurtle;    
        }
    }
}
