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
    private int lives;

    public static GameManager SharedInstance { get; private set; }
    public GameStates currentGameState { get; private set; }

    [Tooltip("Imagen para la vida 1 del jugador")] [SerializeField] Image life1;
    [Tooltip("Imagen para la vida 2 del jugador")] [SerializeField] Image life2;

    [HideInInspector] public int PlayerLives {
        get => lives;
        private set {
            lives = value;

            if (lives == 3)
            {
                life2.enabled = true;
                life1.enabled = true;
            }
            else if (lives == 2)
            {
                life1.enabled = true;
                life2.enabled = false;
            }
            else 
            {
                life1.enabled = false;
                life2.enabled = false;
            }           
        }
    }

    [Tooltip("El GameObject que contiene el texto a mostrar en el Game Over")] [SerializeField] GameObject gameOverGameObject;
    [Tooltip("El Text usado para mostrar el tiempo")] [SerializeField] Text timeText;
    [Tooltip("Sonido al impactar en un enemigo mal")] [SerializeField] AudioClip pop;
    [Tooltip("Música del juego")] [SerializeField] AudioClip music;

    private AudioSource[] _audioSources;
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

        _audioSources = gameObject.GetComponents<AudioSource>();
    }



    private void Start()
    {
        NewGame();
    }

    public void NewGame() 
    {
        if (gameElements==null)
            gameElements = GameObject.FindObjectsOfType<GameElement>();        
        
        //PlayMusic();
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
        if (_audioSources[1] && pop != null)
        {
            if (!_audioSources[1].isPlaying)
                _audioSources[1].PlayOneShot(pop);
        }
    }

    void PlayMusic()
    {
        if (_audioSources[0] && pop != music)
        {
            if (!_audioSources[0].isPlaying)
            {
                _audioSources[0].loop = true;
                _audioSources[0].clip = music;
                _audioSources[0].Play();
            }
        }
    }
    void StopMusic()
    {
        if (_audioSources[0] && pop != music)
        {
            if (_audioSources[0].isPlaying)
                _audioSources[0].Stop();
        }
    }





    private void SetGameState(GameStates newGameState)
    {
        switch (newGameState)
        {
            case GameStates.inNewGame:
                
                if (gameOverGameObject)
                    gameOverGameObject.SetActive(false);
                PlayMusic();
                PlayerLives = 3;
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
                StopMusic();
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
