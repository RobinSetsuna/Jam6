/**
 * @author SerapH
 */

using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The state of the game
/// </summary>
public enum GameState : int
{
    Start = 1,
    LoadLevel = 2,
    Level = 3,
    BeforeBoss = 4,
    Boss = 5,
}

public struct SpawnData
{
    public int id;
    public float spawnTime;
    public Vector3 initialPosition;
    public Vector3 targetPosition;

    public SpawnData(int id, float spawnTime, Vector3 initialPosition, Vector3 targetPosition)
    {
        this.id = id;
        this.spawnTime = spawnTime;
        this.initialPosition = initialPosition;
        this.targetPosition = targetPosition;
    }
}

/// <summary>
/// A FSM for the whole game at the highest level
/// </summary>
public class GameManager : MonoBehaviour
{
    /// <summary>
    /// The unique instance
    /// </summary>
    public static GameManager Singleton { get; private set; }

    /// <summary>
    /// An event triggered whenever the state of the game changes
    /// </summary>
    public EventOnDataChange<GameState> OnCurrentGameStateChange { get; private set; }

    [SerializeField] private GameState initialState = GameState.Start;

    private GameState currentGameState;

    private LinearMovement level;
    private Queue<SpawnData> spawnQueue = new Queue<SpawnData>();
    private float playTime;

    /// <summary>
    /// The current state of the game
    /// </summary>
    public GameState CurrentGameState
    {
        get
        {
            return currentGameState;
        }

        private set
        {
            // Reset the current state
            if (value == currentGameState)
            {
#if UNITY_EDITOR
                Debug.Log(LogUtility.MakeLogStringFormat("GameManager", "Reset {0}.", value));
#endif

                //switch (currentGameState)
                //{
                //}
            }
            else
            {
                // Before leaving the previous state
                switch (currentGameState)
                {
                    case GameState.Start:
                        GUIManager.Singleton.Close("MainMenu");
                        break;
                }

#if UNITY_EDITOR
                Debug.Log(LogUtility.MakeLogStringFormat("GameManager", "Made a transition to {0}.", value));
#endif

                GameState previousGameState = CurrentGameState;
                currentGameState = value;

                OnCurrentGameStateChange.Invoke(previousGameState, currentGameState);

                // After entering the new state
                switch (currentGameState)
                {
                    case GameState.Start:
                        MathUtility.Initialize();
                        GUIManager.Singleton.Open("MainMenu");
                        break;


                    case GameState.LoadLevel:
                        level = Instantiate(ResourceUtility.GetPrefab<LinearMovement>("Level"));
                        Instantiate(ResourceUtility.GetPrefab("Player")).SetActive(true);
                        spawnQueue.Enqueue(new SpawnData(12, 10, new Vector3(-4, 15, 0), new Vector3(-4, 5, 0)));
                        GUIManager.Singleton.Open("HUD");
                        CurrentGameState = GameState.Level;
                        break;


                    case GameState.Level:
                        playTime = 0;
                        break;


                    case GameState.BeforeBoss:
                        playTime = 0;
                        break;


                    case GameState.Boss:
                        level.enabled = false;
                        Instantiate(ResourceUtility.GetPrefab("FinalBoss")).SetActive(true);
                        break;
                }
            }
        }
    }

    private GameManager() { }

    public void StartLevel()
    {
        CurrentGameState = GameState.LoadLevel;
    }

    /// <summary>
    /// Quit the game
    /// </summary>
    public void QuitGame()
    {
        Application.Quit();
    }

    private void Awake()
    {
        if (!Singleton)
        {
            Singleton = this;
            DontDestroyOnLoad(gameObject);

            OnCurrentGameStateChange = new EventOnDataChange<GameState>();
        }
        else if (this != Singleton)
            Destroy(gameObject);
    }

    private void Start()
    {
        CurrentGameState = initialState;
    }

    private void Update()
    {
        switch (currentGameState)
        {
            case GameState.Level:
                playTime += Time.deltaTime;

                if (Random.Range(0f, 1000f) < 10)
                {
                    int r = Random.Range(0, 100);

                    int id = 9;
                    if (r < 80)
                        id = 8;

                    HoverEnemy hoverEnemy = ObjectRecycler.Singleton.GetObject<HoverEnemy>(id);

                    float x = Random.Range(-5f, 5f);
                    float y = Random.Range(-2f, 8f);

                    hoverEnemy.initialPosition = new Vector3(x + Mathf.Sign(x) * Random.Range(7f, 10f), y + Mathf.Sign(Random.Range(-5f, 5f)), 0);
                    hoverEnemy.targetPosition = new Vector3(x, y, 0);

                    hoverEnemy.gameObject.SetActive(true);
                }

                while (spawnQueue.Count > 0 && playTime >= spawnQueue.Peek().spawnTime)
                {
                    SpawnData spawnData = spawnQueue.Dequeue();

                    ChasingEnemy enemy = ObjectRecycler.Singleton.GetObject<ChasingEnemy>(spawnData.id);
                    enemy.initialPosition = spawnData.initialPosition;
                    enemy.targetPosition = spawnData.targetPosition;

                    enemy.gameObject.SetActive(true);
                }

                if (level.transform.position.y < -50)
                    CurrentGameState = GameState.BeforeBoss;
                break;


            case GameState.BeforeBoss:
                if (level.transform.position.y < -57)
                    CurrentGameState = GameState.Boss;
                break;
        }
    }
}

