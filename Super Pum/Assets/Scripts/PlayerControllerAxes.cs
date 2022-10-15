using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerAxes : PlayerController
{
    float distanceDestination=0f;
    private BoxCollider2D bCol2d;
    [Tooltip("Usar Time.deltaTime en Update")] [SerializeField] private bool useAxes = false;
    [Tooltip("Usar Time.deltaTime en Update")] [SerializeField] private bool GetAxisRaw = false;

    new private void Awake()
    {
        base.Awake();
        bCol2d = gameObject.GetComponent<BoxCollider2D>();        
    }

    new protected void Update()
    {
        
        bool flagMove=CanMove(currentDirection, distanceDestination);
        
        if (distanceDestination < 0.02f || !flagMove)
        {
            if (useAxes)
            {
                if (!GetAxisRaw)
                {
                    if (Input.GetAxis("Horizontal") < 0)
                    {
                        if (!flagMove && currentDirection == Direction.left)
                            return;
                        destination = (Vector2)transform.position + (Vector2.left * spaceSpeed);
                        if (currentDirection != Direction.left)
                            RotateSprite();
                    }
                    else if (Input.GetAxis("Horizontal") > 0)
                    {
                        if (!flagMove && currentDirection == Direction.right)
                            return;
                        destination = (Vector2)transform.position + (Vector2.right * spaceSpeed);
                        if (currentDirection != Direction.right)
                            RotateSprite();
                    }
                }
                else
                {

                    if (Input.GetAxisRaw("Horizontal") < 0)
                    {
                        if (!flagMove && currentDirection == Direction.left)
                            return;
                        destination = (Vector2)transform.position + (Vector2.left * spaceSpeed);
                        if (currentDirection != Direction.left)
                            RotateSprite();
                    }
                    else if (Input.GetAxisRaw("Horizontal") > 0)
                    {
                        if (!flagMove && currentDirection == Direction.right)
                            return;
                        destination = (Vector2)transform.position + (Vector2.right * spaceSpeed);
                        if (currentDirection != Direction.right)
                            RotateSprite();
                    }
                }

            }
            else
            {
                if (Input.GetKey(KeyCode.LeftArrow))
                {
                    if (!flagMove && currentDirection == Direction.left)
                        return;
                    destination = (Vector2)transform.position + (Vector2.left * spaceSpeed);
                    if (currentDirection != Direction.left)
                        RotateSprite();
                }
                else if (Input.GetKey(KeyCode.RightArrow))
                {
                    if (!flagMove && currentDirection == Direction.right)
                        return;
                    destination = (Vector2)transform.position + (Vector2.right * spaceSpeed);
                    if (currentDirection != Direction.right)
                        RotateSprite();
                }
            }
        }
    }

    private new void FixedUpdate()  
    {
        Vector2 newPosition = Vector2.MoveTowards(gameObject.transform.position, destination, speed);
        _rigidbody.MovePosition(newPosition);

        distanceDestination = Vector2.Distance((Vector2)transform.position, destination);
    }


    /// <summary>
    /// Este método se utiliza para averiguar si el jugador puede mover a una posición.
    /// El jugardor mueve  
    /// <para>Direction currentDirection la dirección a la que queremos comprobar el movimiento</para>
    /// </summary>
    protected bool CanMove(Direction currentDirection, float distanceDestination)
    {
        Vector2 startPosition, Dir;
        
        if (currentDirection == Direction.left)
        {
            float left = transform.position.x - (bCol2d.bounds.extents.x); 
            startPosition = new Vector2(left, transform.position.y);
            Dir = Vector2.left;
        }
        else
        {
            float right = transform.position.x + (bCol2d.bounds.extents.x);            
            startPosition = new Vector2(right, transform.position.y);
            Dir = Vector2.right;
        }
        RaycastHit2D hit = Physics2D.Raycast(startPosition, Dir, distanceDestination, wallLayer);

#if UNITY_EDITOR
        if (hit.collider)
            Debug.DrawRay(startPosition, Dir * distanceDestination, Color.red);
        else
            Debug.DrawRay(startPosition, Dir * distanceDestination, Color.green);
#endif

        if (hit.collider)
        {
            print(hit.collider.name);
            return false;
        }
        else
            return true;
    }
}