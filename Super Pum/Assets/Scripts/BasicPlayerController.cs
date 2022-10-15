using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicPlayerController : GameElement
{
    #region Propiedades y   
    
    protected SpriteRenderer sprite;
    protected Rigidbody2D _rigidbody;
    protected BoxCollider2D _boxcollider;
    protected Direction currentDirection;
    protected Vector2 destination;   
    
    //Probar con 100000
    [Range(1,100)]
    [Tooltip("Velocidad a la que se mueve el jugador")] [SerializeField] protected int speed = 2;
    [Space]
    [Header("Para Experimentar")]
    [Tooltip("Usar Time.deltaTime en Update")] [SerializeField] private bool deltaTime = false;
    [Tooltip("Mostar deltaTime")] [SerializeField] private bool showDeltaTime = false;
    [Tooltip("Diferencias entre Update vs FixedUpdate")] [SerializeField] private bool useFixedUpdate =false;


    override public void StartGame() { }
    override public void StopGame() { }

    #endregion

    protected void Awake()
    {
        sprite = gameObject.GetComponent<SpriteRenderer>();
        _rigidbody = gameObject.GetComponent<Rigidbody2D>();
        _boxcollider = gameObject.GetComponent<BoxCollider2D>();
        currentDirection = Direction.right;
    }

    
    protected void Update()
    {
        if (useFixedUpdate)
            return;

        float value = Input.GetAxisRaw("Horizontal");

        if (deltaTime)
            _rigidbody.velocity = (speed * 500f) * Time.deltaTime * value * transform.right;
        else
            _rigidbody.velocity = speed * value * transform.right;

        if ((value <0 && currentDirection == Direction.right)
            || 
           (value > 0 && currentDirection == Direction.left))
            RotateSprite();

        if (showDeltaTime)
            print("deltaTime: " + Time.deltaTime);
    }

    protected void FixedUpdate()
    {
        if (!useFixedUpdate)
            return;

        float value = Input.GetAxisRaw("Horizontal");

        _rigidbody.velocity = value * speed * transform.right;
        
        if ((value < 0 && currentDirection == Direction.right)
           ||
          (value > 0 && currentDirection == Direction.left))
            RotateSprite();

        if (showDeltaTime)
            print("deltaTime: " + Time.deltaTime);
    }

    

    protected void RotateSprite()
    {
        if (currentDirection == Direction.left)
        {
            currentDirection = Direction.right;
            _boxcollider.offset += new Vector2(-0.05f, 0f);
        }
        else
        {
            currentDirection = Direction.left;
            _boxcollider.offset += new Vector2(+0.05f, 0f);
        }
        sprite.flipX = !sprite.flipX;
    }
}
