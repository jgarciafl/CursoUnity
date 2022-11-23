using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    left,
    right
}

public enum Size
{
    small,
    medium,
    big
}


public class BallController : GameElement
{

    public Direction initialDirection = Direction.left;
    [Range(0, 1)]
    [SerializeField] float force = 0.25f;

    [Tooltip("Si la bola creara bolas hijas cuando sea impactada")] [SerializeField] bool isParent = false;
    [Tooltip("El tipo de bola que se creara cuando se ha impactada")] [SerializeField] GameObject childBalls;
    [Tooltip("El número de bolas hijas a crear")] [SerializeField] int numbreOfChildBalls = 0;
    [Tooltip("El tamaño de la bola")] [SerializeField] Size sizeOfBall;

    private Rigidbody2D _rigidbody;
    public bool isOriginalEnemy = false;

    Direction currentDirection;
    Vector2 currentPosition;

    const float smallSize = 0.2f;
    const float mediumSize = 1f;
    const float bigSize = 2f;

    bool waitAMoment = false;

    private void Awake()
    {
        _rigidbody = gameObject.GetComponent<Rigidbody2D>();

        currentDirection = initialDirection;
        currentPosition = gameObject.transform.position;
    }

    override public void StartGame()
    {
        GameManager.SharedInstance.NumberOfEnemies += 1;

        gameObject.transform.position = currentPosition;
        currentDirection = initialDirection;
        _rigidbody.gravityScale = 1f;

        Vector2 appliedForce = Vector2.zero;

        if (initialDirection == Direction.left)
            appliedForce = Vector2.left;
        else
            appliedForce = Vector2.right;

        if (!isOriginalEnemy)
            appliedForce += (Vector2.up * 2.2f);

        _rigidbody.AddForce(appliedForce, ForceMode2D.Impulse);
    }

    override public void StopGame()
    {
        if (!isOriginalEnemy)
            Destroy(gameObject);

        _rigidbody.gravityScale = 0f;
        _rigidbody.velocity = Vector2.zero;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (GameManager.SharedInstance.currentGameState != GameStates.inGame)
        {
            if (!isOriginalEnemy)
                Destroy(gameObject);
            return;
        }

        if (!waitAMoment)
        {
            if (sizeOfBall==Size.small && transform.position.y>4f && _rigidbody.velocity.y>0)
                _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _rigidbody.velocity.y*0.7f);

            if (_rigidbody.velocity.x < force)
            {
                if (currentDirection == Direction.left)
                    _rigidbody.AddForce(Vector2.left * force);
                else
                    _rigidbody.AddForce(Vector2.right * force);
            }
            _rigidbody.velocity = Vector2.ClampMagnitude(_rigidbody.velocity, 12f);            
        }
    }


    private GameObject SetBallChildPosition(int offtsetX)
    {
        Vector2 ballPosition = transform.position;
        GameObject _;

        if (ballPosition.y < 4f)
        {
            ballPosition = new Vector2(ballPosition.x, 4f);
            ballPosition += sizeOfBall switch
            {
                Size.medium => new Vector2(0, 1f),
                Size.big => new Vector2(0, 2f),
                _ => new Vector2(0, 0),
            };
        }

        ballPosition += sizeOfBall switch
        {
            Size.small => new Vector2(-smallSize * offtsetX, 0),
            Size.medium => new Vector2(-mediumSize * offtsetX, 0),
            Size.big => new Vector2(-bigSize * offtsetX, 0),
            _ => new Vector2(-mediumSize * offtsetX, 0),
        };


        if (ballPosition.x > 8 || ballPosition.x < -8)
        {
            ballPosition = transform.position;
            if (offtsetX % 2 == 0)
                ballPosition += new Vector2(-Random.value / 2, 0);
            else
                ballPosition += new Vector2(+Random.value / 2, 0);
        }

        _ = Instantiate(childBalls, ballPosition, Quaternion.identity);
        if (offtsetX % 2 == 0)
            _.GetComponent<BallController>().initialDirection = Direction.right; 
        else
            _.GetComponent<BallController>().initialDirection = Direction.left;

        return _;
    }



    private void CreateChildBalls()
    {
        for(int i=0; i < numbreOfChildBalls; i++)
        {
            GameObject _ = SetBallChildPosition(i);
            _.GetComponent<BallController>().isOriginalEnemy = false;
            _.GetComponent<BallController>().StartGame();
        }
    }



    public void Kill()
    {
        GameManager.SharedInstance.PlayPop();

        if (isParent && childBalls)
            CreateChildBalls();


        GameManager.SharedInstance.NumberOfEnemies--;

        if (isOriginalEnemy)
            gameObject.SetActive(false);
        else
            Destroy(gameObject);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (GameManager.SharedInstance.currentGameState != GameStates.inGame)
            return;

        if (collision.gameObject.layer == LayerMask.NameToLayer("Walls"))
        {
            currentDirection = currentDirection == Direction.left ? Direction.right : Direction.left;           
        }
        else if (collision.collider.CompareTag("Player"))
        {
            GameManager.SharedInstance.PlayerLosesLife();
            _rigidbody.gravityScale = 0f;
            _rigidbody.velocity = Vector2.zero;
        }
    }
}
