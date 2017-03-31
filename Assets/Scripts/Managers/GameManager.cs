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
    
    private int numPlayers = 0;
    private Player[] players = new Player[(int)Player.Index.Count];
    public int NumPlayers { get { return numPlayers; } }

    private string playerStartInput = "Start_";
    
    [HideInInspector]
    public Event onGravityChanged = new Event();

    protected override void OnAwake()
    {
        Application.targetFrameRate = targetFps;
    }

    private void Update()
    {
        CheckSpawnNewPlayer();
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
        player.onKilled.AddListener(OnPlayerKilled);

    }

    private void OnPlayerKilled(Player p)
    {
        p.Stats.deaths++;
        p.Stats.lostPoints += options.DeathPenality;
    }

    [System.Serializable]
    public class Options
    {
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
}
