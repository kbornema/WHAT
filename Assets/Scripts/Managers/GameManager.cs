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
    private GameCamera gameCam;
    public GameCamera GameCam { get { return gameCam; } }

    [SerializeField]
    private Player playerPrefab;

    [SerializeField]
    private Options options;
    public Options GameOptions { get { return options; } }

    [SerializeField]
    private EnemyPrefabs enemyPrefabs;
    
    private int numPlayers = 0;
    private Player[] players = new Player[(int)Player.Index.Count];
    public int NumPlayers { get { return numPlayers; } }

    private string playerStartInput = "Start_";
    
    [HideInInspector]
    public Event onGravityChanged = new Event();

    private List<EnemySpawner> enemySpawners = new List<EnemySpawner>();

    protected override void OnAwake()
    {
        Application.targetFrameRate = targetFps;
    }

    private void Update()
    {
        CheckSpawnNewPlayer();

        if (Input.GetKeyDown(KeyCode.P))
            SpawnEnemyRandom(enemyPrefabs.normalTurtle);
    }

    private void CheckSpawnNewPlayer()
    {
        for (int i = 0; i < (int)Player.Index.Count; i++)
        {
            if (players[i] == null && Input.GetButtonDown(playerStartInput + i.ToString()))
            {
                Player instance = Instantiate(playerPrefab);

                instance.PlayerIndex = (Player.Index)i;
                players[i] = instance;
                numPlayers++;
                OnNewPlayerSpawned(instance);
            }
        }
    }

    private void OnNewPlayerSpawned(Player player)
    {
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

    private void SpawnEnemyRandom(Actor prefab)
    {
        SpawnEnemy(prefab, Random.Range(0, enemySpawners.Count));
    }

    private void SpawnEnemy(Actor prefab, int spawnId)
    {
        Actor instance = enemySpawners[spawnId].Spawn(prefab);

        instance.TheHealth.onZeroHealth.AddListener(OnEnemyKilled);
        
    }

    private void OnEnemyKilled(Health enemyHealth, Health.EventInfo info)
    {
        if(info.source && info.source.ThePlayer)
        {
            info.source.ThePlayer.Stats.kills++;

            if(enemyHealth.RootActor)
                info.source.ThePlayer.Stats.gainedPoints += enemyHealth.RootActor.PointsOnKill;
        }
    }

    public void RegisterEnemySpawner(EnemySpawner spawner)
    {
        enemySpawners.Add(spawner);
    }

    [System.Serializable]
    public class Options
    {
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
    }

    [System.Serializable]
    public class EnemyPrefabs
    {
        public Actor bombTurtle;
        public Actor normalTurtle;
    
        
    }
}
