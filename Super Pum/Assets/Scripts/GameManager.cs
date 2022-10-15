using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GameStates 
{
    inNewGame,
    inGame,
    inPlayerHit,
    inGoal,
    inGameOver
}


public class GameManager : MonoBehaviour
{
    public static GameManager SharedInstance { get; private set; }
    public GameStates currentGameState { get; private set; }

    [HideInInspector]public int PlayerLives { get; private set; }

    [Tooltip("El GameObject que contiene el texto a mostrar en el Game Over")] [SerializeField] GameObject gameOverGameObject;
    [Tooltip("El Text usado para mostrar el tiempo")] [SerializeField] Text timeText;
    [Tooltip("Sonido al impactar en un enemigo mal")] [SerializeField] AudioClip pop;

    private AudioSource _audioSource;
    private int enemies;

    public int NumberOfEnemies {
        get => enemies;
        set
        {
            enemies = value;
            if (enemies<=0 && currentGameState==GameStates.inGame)
                SetGameState(GameStates.inGoal);
        }
    }

    private GameElement[] gameElements;
    private int time;
    private Coroutine timeCorutine;

    // Start is called before the first frame update
    private void Awake()
    {
        if (SharedInstance)
            Destroy(this);
        else
            SharedInstance = this;

        _audioSource = gameObject.GetComponent<AudioSource>();
    }



    private void Start()
    {
        NewGame();
    }

    public void NewGame() 
    {
        if (gameElements==null)
            gameElements = GameObject.FindObjectsOfType<GameElement>();        
        PlayerLives = 3;
        SetGameState(GameStates.inNewGame);
    }


    private void StartGame()
    {
        foreach (GameElement _ in gameElements)
        {
            _.gameObject.SetActive(true);
            _.StartGame();
        }
        
        time = 99;
        timeText.text = string.Format("Time: 0{0}", time);
        SetGameState(GameStates.inGame);
        timeCorutine = StartCoroutine(TimeDown());
    }

    private IEnumerator TimeDown()
    {
        while (time > 0)
        {
            yield return new WaitForSeconds(1f);
            time--;
            if(time<10)
                timeText.text = string.Format("Time: 00{0}", time);
            else
                timeText.text = string.Format("Time: 0{0}", time);
        }
        PlayerLosesLife();
    }
    
    public void PlayerLosesLife()
    {
        if (timeCorutine != null)
            StopCoroutine(timeCorutine);
        
        SetGameState(GameStates.inPlayerHit);
        NumberOfEnemies = 0;
        PlayerLives--;
    }

    public void CanGameContinue()
    {
        if (PlayerLives > 0)
        {
            StartGame();
            SetGameState(GameStates.inGame);
        }
        else
            SetGameState(GameStates.inGameOver);
    }

    public void PlayPop()
    {
        if (_audioSource && pop != null)
        {
            if (!_audioSource.isPlaying)
                _audioSource.PlayOneShot(pop);
        }
    }


    private void SetGameState(GameStates newGameState)
    {
        switch (newGameState)
        {
            case GameStates.inNewGame:
                if (gameOverGameObject)
                    gameOverGameObject.SetActive(false);
                StartGame();
                currentGameState = GameStates.inGame;
                break;

            case GameStates.inGame:
                currentGameState = GameStates.inGame;
                break;

            case GameStates.inPlayerHit:
                currentGameState = GameStates.inPlayerHit;
                break;

            case GameStates.inGoal:
                
                if (timeCorutine != null)
                    StopCoroutine(timeCorutine);

                foreach (GameElement _ in gameElements)
                    _.StopGame();

                if (gameOverGameObject)
                {
                    gameOverGameObject.GetComponent<Text>().text = "WELL DONE";
                    gameOverGameObject.SetActive(true);
                }
                currentGameState = GameStates.inGoal;
                break;

            case GameStates.inGameOver:

                if (gameOverGameObject)
                {
                    gameOverGameObject.GetComponent<Text>().text="GAME OVER";
                    gameOverGameObject.SetActive(true); 
                }
                currentGameState = GameStates.inGameOver;
                break;

            default:
                break;
        }
    }   
}
