using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement_CharacterController : MonoBehaviour
{
    [SerializeField] float speed = 15;
    CharacterController cc;


    private void Awake()
    {
        cc = gameObject.GetComponent<CharacterController>();
    }

    private void Update()
    {
        Vector3 movementInput = Vector3.zero;

        if (Input.GetKey(KeyCode.W))
            movementInput.z = 1;

        if (Input.GetKey(KeyCode.S))
            movementInput.z = -1;

        if (Input.GetKey(KeyCode.D))
            movementInput.x = 1;

        if (Input.GetKey(KeyCode.A))
            movementInput.x = -1;
    
        Move(movementInput);
    }

    private void Move(Vector3 direction)
    {
        cc.SimpleMove(direction.normalized * speed);
    }
}
