using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarpoonController : MonoBehaviour
{

    bool isStop = false;

    protected Rigidbody2D _rigidbody;
    //public AdvancedPlayerController parent;
    Coroutine coroutine;

    [Range(1,10)]
    [Tooltip("Usar Time.deltaTime en Update")] [SerializeField] private int speed=5;

    protected void Awake()
    {
        _rigidbody = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!isStop)
            _rigidbody.velocity = speed * transform.up * Time.fixedDeltaTime;
    }

    private void OnEnable()
    {
        isStop = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
       // if (GameManager.SharedInstance.currentGameState != GameStates.inGame)
       //     return;

        if (collision.CompareTag("Enemy"))
        {
            //parent.EncenderDisparo();
            collision.GetComponent<BallController>().Kill();
            if(coroutine!=null)
            StopCoroutine(coroutine);
            gameObject.SetActive(false);            
            //Destroy(this.gameObject);
        }
        else if (collision.CompareTag("Ceiling"))
        {
            _rigidbody.velocity = Vector2.zero;
            isStop = true;
            //parent.EncenderDisparo();
            coroutine = StartCoroutine(TurnOff());
            //Destroy(this.gameObject, 0.3f);
        }
    }

    IEnumerator TurnOff()
    { 
        yield return new WaitForSeconds(0.3f);
        gameObject.SetActive(false);        
        coroutine = null;
    }  
}
