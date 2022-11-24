using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClasePlayerController : MonoBehaviour
{
    protected SpriteRenderer _sprite;
    private Animator _animator;

    bool rightDirection=true;
    bool disparando = false;
    float scaleValue = 1F;

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
        _sprite = gameObject.GetComponent<SpriteRenderer>();
        _animator = gameObject.GetComponent<Animator>();
        scaleValue = gameObject.transform.localScale.x;
    }


    void FinalizarDisparar() 
    {
        Debug.Log("ya no disparo");
        disparando = false;
    }




    // Update is called once per frame
    void Update()
    {
        _animator.SetBool("Caminar", false);

        if (Input.GetButton("Fire1"))
        {
            if (!disparando)
            {
                disparando = true;
                _animator.SetTrigger("Disparar");
                //Invoke(nameof(FinalizarDisparar), 0.2f);
            }
            return;
        }
        else if (disparando)
            return;


        

        if (Input.GetAxisRaw("Horizontal") > 0)
        {
            _animator.SetBool("Caminar", true);
            gameObject.transform.position = new Vector3(gameObject.transform.position.x + 0.1f, gameObject.transform.position.y, gameObject.transform.position.z);

            if (!rightDirection)
            {
                rightDirection = true;
                gameObject.transform.localScale = new Vector3(scaleValue, gameObject.transform.localScale.y, gameObject.transform.localScale.z);
            }
        }

        if (Input.GetAxisRaw("Horizontal") < 0)
        {
            _animator.SetBool("Caminar", true);
            gameObject.transform.position = new Vector3(gameObject.transform.position.x - 0.1f, gameObject.transform.position.y, gameObject.transform.position.z);

            if (rightDirection)
            {
                rightDirection = false;
                gameObject.transform.localScale = new Vector3(-scaleValue, gameObject.transform.localScale.y, gameObject.transform.localScale.z);
            }
        }

    }
}
