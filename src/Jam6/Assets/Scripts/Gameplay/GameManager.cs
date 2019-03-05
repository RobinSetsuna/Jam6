/**
 * @author SerapH
 */

using UnityEngine;

/// <summary>
/// The state of the game
/// </summary>
public enum GameState : int
{
    Start = 1,
    LoadLevel = 2,
    Level = 3,
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

    private float playTime;

    bool temp = false;

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
                //switch (currentGameState)
                //{
                //}

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
                        CurrentGameState = GameState.LoadLevel;
                        break;

                    case GameState.LoadLevel:
                        Instantiate(ResourceUtility.GetPrefab("Level"));
                        CurrentGameState = GameState.Level;
                        break;

                    case GameState.Level:
                        playTime = 0;
                        break;
                }
            }
        }
    }

    private GameManager() { }

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

                    Hover hoverEnemy = ObjectRecycler.Singleton.GetObject<Hover>(id);

                    float x = Random.Range(-5f, 5f);
                    float y = Random.Range(-2f, 8f);

                    hoverEnemy.initialPosition = new Vector3(x + Mathf.Sign(x) * Random.Range(7f, 10f), y + Mathf.Sign(Random.Range(-5f, 5f)), 0);
                    hoverEnemy.targetPosition = new Vector3(x, y, 0);

                    hoverEnemy.gameObject.SetActive(true);

                    Debug.Log(111);
                }
                if(playTime > 15f && temp == false)
                {
                    var boss = Instantiate(ResourceUtility.GetPrefab("FinalBoss"));
                    boss.SetActive(true);
                    temp = true;
                }
                break;
        }
    }
}

