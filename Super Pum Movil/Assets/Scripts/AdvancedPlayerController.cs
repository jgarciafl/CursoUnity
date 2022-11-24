using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdvancedPlayerController : BasicPlayerController
{
    [Tooltip("Usar deteccion continua de colisiones")] [SerializeField] private bool continuousCollisionDetection = true;
    [Tooltip("El Gameobject que se usa como proyectil")] [SerializeField] GameObject harpoon;
    [Tooltip("Simular moviemiento sobre hielo")] [SerializeField] bool movementOnIce = false;
    [Tooltip("Sonido disparo bien")] [SerializeField] AudioClip bang;
    [Tooltip("Sonido disparo mal")] [SerializeField] AudioClip nop;

    [Tooltip("Boton para mover el jugador a la izquierda")] [SerializeField] ButtonController buttonLeft;
    [Tooltip("Boton para mover el jugador a la derecha")] [SerializeField] ButtonController buttonRight;
    [Tooltip("Boton que el jugador pueda disparar")] [SerializeField] ButtonController buttonShoot;


    private float directionValue;
    private bool isShooting, waitShoot, isPlayerDeath = false;
    private Vector2 currentPosition;
    private Animator _animator;
    private BoxCollider2D bCol2d;
    private AudioSource _audioSource;
    GameObject harpoonInstance;

    // animation IDs
    private int _animIDWalking;
    private int _animIDShooting;
    private int _animIDDyingLeft;
    private int _animIDDyingRight;


    new private void Awake()
    {
        base.Awake();
        _animator= gameObject.GetComponent<Animator>();
        _audioSource = gameObject.GetComponent<AudioSource>();
        bCol2d = gameObject.GetComponent<BoxCollider2D>();
        AssignAnimationIDs();
        currentPosition = gameObject.transform.position;
    }


    private void AssignAnimationIDs()
    {
        _animIDWalking = Animator.StringToHash("Walking");
        _animIDShooting = Animator.StringToHash("Shooting");
        _animIDDyingLeft = Animator.StringToHash("DyingLeft");
        _animIDDyingRight = Animator.StringToHash("DyingRight");        
    }



    /// <summary>
    /// Este método se llama cada vez que comienza el juego o matan al personaje.
    /// Lo que hace es llevarlo a su posición inicial, le activa la gravedad, y le habilita el collider.
    /// </summary>
    override public void StartGame()
    {
        _rigidbody.gravityScale = 1;
        bCol2d.enabled = true;
        gameObject.transform.position = currentPosition;
        isPlayerDeath = false;
        waitShoot = false;
        isShooting = false;
    }

    private void Start()
    {
        base.Start();
        Vector2 harpoonPosition = transform.position;
        harpoonInstance = Instantiate(harpoon, harpoonPosition, Quaternion.identity);
        harpoonInstance.SetActive(false);

        //Ajustamos el modo de collisionDetectionMode al valor establecido en continuousCollisionDetection
        if (continuousCollisionDetection)
            _rigidbody.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        else
            _rigidbody.collisionDetectionMode = CollisionDetectionMode2D.Discrete;
        
        isShooting = false;
    }
               

    private IEnumerator WaitDeath()
    {
        yield return new WaitForSeconds(1f);
        _rigidbody.gravityScale = 100;
        bCol2d.enabled = false;
        yield return new WaitForSeconds(0.4f);
        _animator.Play("Idle");
        GameManager.SharedInstance.CanGameContinue();
    }


    //public IEnumerator CheckAnimationCompleted(string CurrentAnim, Action Oncomplete)
    //{
    //    yield return new WaitForEndOfFrame();

    //    while (_animator.GetCurrentAnimatorStateInfo(0).IsName(CurrentAnim))
    //        yield return null;
    //    if (Oncomplete != null)
    //        Oncomplete();
    //}
     
    void FinalizarDisparar() 
    {
        isShooting = false;
    }

    public void FinishAnimationShoot()
    {
        isShooting = false;
    }

    override public void StopGame()
    {
        _animator.SetBool(_animIDWalking, false);
        _animator.SetTrigger(_animIDShooting);
        isShooting = true;
        waitShoot = true;
        _rigidbody.gravityScale = 0;
        directionValue = 0f;
    }

    void buttonIsPressed()
    {
        directionValue = buttonLeft.ClickedStatus ? -1f : 0f;
        directionValue += buttonRight.ClickedStatus ? 1f : 0f;
    }

    new protected void Update()
    {
         if (GameManager.SharedInstance.currentGameState == GameStates.inGameOver ||
            GameManager.SharedInstance.currentGameState == GameStates.inGoal)
        {
            if (Input.GetKeyDown(KeyCode.Return))
                GameManager.SharedInstance.NewGame();
            else
                return; 
        }

        if (GameManager.SharedInstance.currentGameState==GameStates.inPlayerHit)
        {
            if (!isPlayerDeath)
            {
                isPlayerDeath = true;
                directionValue = 0f;               
                _animator.SetBool(_animIDWalking, false);
                _animator.SetBool(_animIDShooting, false);

                if (currentDirection == Direction.right)
                    _animator.SetTrigger(_animIDDyingRight);
                else
                    _animator.SetTrigger(_animIDDyingLeft);
                StartCoroutine(WaitDeath());
            }
            return;
        }        
        
        if(!harpoonInstance.activeInHierarchy)
            waitShoot=false;


        if (!waitShoot && !isShooting && (Input.GetKey(KeyCode.Space) || buttonShoot.ClickedStatus)) //Input.GetButton("Fire1")
        {
            directionValue = 0f;
            isShooting = true;
            waitShoot = true;
            _animator.SetBool(_animIDWalking, false);           
            _animator.SetTrigger(_animIDShooting);

           
   //         StartCoroutine(CheckAnimationCompleted("Shoot", () =>
			//{
			//	FinishAnimationShoot();
			//}
   //         ));

			if (_audioSource && bang != null)
                _audioSource.PlayOneShot(bang);

            Shoot();
            return;
        }
        else if (Input.GetKey(KeyCode.Space) || buttonShoot.ClickedStatus) //Input.GetButton("Fire1")
        {
            if (_audioSource && nop != null)
            {
                if (!_audioSource.isPlaying)
                    _audioSource.PlayOneShot(nop);
            }
        }


        if (!isShooting)
        {
            if (buttonLeft.HoldStatus)
                directionValue = -1f;
            else if (buttonRight.HoldStatus)
                directionValue = 1f;
            else
                directionValue = Input.GetAxisRaw("Horizontal");

            if ((directionValue < 0 && currentDirection == Direction.right)
                ||
                (directionValue > 0 && currentDirection == Direction.left))
                RotateSprite();

            if (directionValue != 0)
                _animator.SetBool(_animIDWalking, true);
            else
                _animator.SetBool(_animIDWalking, false);
        }
        
        if (continuousCollisionDetection && _rigidbody.collisionDetectionMode != CollisionDetectionMode2D.Continuous)
            _rigidbody.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        else if (!continuousCollisionDetection && _rigidbody.collisionDetectionMode != CollisionDetectionMode2D.Discrete)
            _rigidbody.collisionDetectionMode = CollisionDetectionMode2D.Discrete;

        Move();
    }

    private void Shoot()
    {
        Vector2 harpoonPosition = transform.position;       
        if(currentDirection == Direction.right)
            harpoonPosition += new Vector2(+0.12f, -3.82f);
        else
            harpoonPosition += new Vector2(-0.12f, -3.82f);
        harpoonInstance.transform.position = harpoonPosition;
        harpoonInstance.SetActive(true);
        //harpoonInstance = Instantiate(harpoon, harpoonPosition, Quaternion.identity);
        //harpoonInstance.GetComponent<HarpoonController>().parent = this;
    }


    new protected void FixedUpdate() { }
    void Move()
    {
        if (GameManager.SharedInstance.currentGameState != GameStates.inGame)
            return;

        //_rigidbody.velocity = directionValue * speed * transform.right; //speed=8        
        if(movementOnIce)
            _rigidbody.AddForce(directionValue * (speed-35) * transform.right, ForceMode2D.Force);
        else
            _rigidbody.MovePosition(new Vector2(_rigidbody.position.x + (directionValue * speed * transform.right.x * Time.fixedDeltaTime),0f));
    }  

}
