using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Propiedades y   
    
    protected SpriteRenderer sprite;
    protected Rigidbody2D _rigidbody;
    protected Direction currentDirection;
    protected Vector2 destination;   

    
    [Tooltip("Velocidad a la que se mueve el jugador")] [SerializeField] protected int speed = 2;
    [Range(0, 1)]
    [Tooltip("La cantidad en unidades que se mueve cuando pulsamos una tecla de mover")] [SerializeField] protected float spaceSpeed =1f;
    [Tooltip("Capa que identifica los muros del escenario")] [SerializeField] protected LayerMask wallLayer;
    [Space]
    [Header("Para Experimentar")]
    [Tooltip("Diferencias entre Update vs FixedUpdate")] [SerializeField] private bool useFixedUpdate =false;
    [Tooltip("Usar Time.deltaTime en Update")] [SerializeField] private bool deltaTime = false;

    #endregion

    protected void Awake()
    {
        sprite = gameObject.GetComponent<SpriteRenderer>();
        _rigidbody = gameObject.GetComponent<Rigidbody2D>();
        currentDirection = Direction.right;
    }



    // Start is called before the first frame update
    protected void Start()
    {
        destination = transform.position;
    }

    
    protected void Update()
    {
        if (useFixedUpdate)
            return;

        /* Espacio = Velocidad * Tiempo*/
        /* step es la cantidad de espacio a mover */
        float step;

        if (deltaTime)
            step = speed * Time.deltaTime;
        else
            step = speed;

        Debug.Log("Time.deltaTime: " + Time.deltaTime);
        Debug.Log("step: " + step);

        Vector2 newPosition = Vector2.MoveTowards(gameObject.transform.position, destination, step);
        _rigidbody.MovePosition(newPosition);        

        float distanceDestination= Vector2.Distance((Vector2)transform.position, destination);        

        if (distanceDestination < 0.02f || !CanMove(currentDirection))
        {
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                destination = (Vector2)transform.position + (Vector2.left*spaceSpeed);
                if (currentDirection != Direction.left)
                    RotateSprite();
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                destination = (Vector2)transform.position + (Vector2.right*spaceSpeed);
                if (currentDirection != Direction.right)
                    RotateSprite();
            }
        }
    }

    protected void FixedUpdate()
    {
        if (!useFixedUpdate)
            return;

        /* Espacio = Velocidad * Tiempo*/

        float step = speed;
        Debug.Log("Time.deltaTime: " + Time.deltaTime);
        Debug.Log("step: " + step);


        Vector2 newPosition = Vector2.MoveTowards(gameObject.transform.position, destination, step);
        _rigidbody.MovePosition(newPosition);        

        float distanceDestination= Vector2.Distance((Vector2)transform.position, destination);        

        if (distanceDestination < 0.02f || !CanMove(currentDirection))
        {
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                destination = (Vector2)transform.position + (Vector2.left*spaceSpeed);
                if (currentDirection != Direction.left)
                    RotateSprite();
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                destination = (Vector2)transform.position + (Vector2.right*spaceSpeed);
                if (currentDirection != Direction.right)
                    RotateSprite();
            }
        }
    }

    /// <summary>
    /// Este método se utiliza para averiguar si el jugador puede mover a una posición.
    /// El jugardor mueve  
    /// <para>Direction currentDirection la dirección a la que queremos comprobar el movimiento</para>
    /// </summary>
    protected bool CanMove(Direction currentDirection)
    {
        Vector2 Dir = currentDirection == Direction.left ? Vector2.left : Vector2.right;        
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Dir, 1.5f, wallLayer);

#if UNITY_EDITOR
        if (hit.collider)
            Debug.DrawRay(transform.position, Dir * 0.5f, Color.red);
        else
            Debug.DrawRay(transform.position, Dir * 0.5f, Color.green);
#endif

        if (hit.collider)
        {
            print(hit.collider.name);
            return false;
        }
        else
            return true;
    }

    protected void RotateSprite()
    {
        if (currentDirection == Direction.left)
            currentDirection = Direction.right;
        else
            currentDirection = Direction.left;

        sprite.flipX = !sprite.flipX;
    }
}
