using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement_Rigidbody : MonoBehaviour
{
    [SerializeField] float speed = 15;

    [Header("Default - MovePosition")]
    [SerializeField] bool MoveForce = false;
    [SerializeField] bool MoveSpeed = false;
 

    Rigidbody rb;
    Vector3 movementInput;

    private void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }
       

    private void Update()
    {
               
        movementInput = Vector3.zero;

        if (Input.GetKey(KeyCode.W))
            movementInput.z = 1;

        if (Input.GetKey(KeyCode.S))
            movementInput.z = -1;

        if (Input.GetKey(KeyCode.D))
            movementInput.x = 1;

        if (Input.GetKey(KeyCode.A))
            movementInput.x = -1;
    }

    private void FixedUpdate()
    {
        Move(movementInput);
    }

    private void Move(Vector3 direction) 
    {
        if (MoveSpeed)
            rb.velocity = direction.normalized * speed * Time.fixedDeltaTime;
        else if (MoveForce)
            rb.AddForce(direction.normalized * speed, ForceMode.Acceleration);
        else
        // Como mover con el Transform
        // Pero desde las físicas
        // Atraviesa objetos
            rb.MovePosition(rb.position + speed * Time.fixedDeltaTime * direction.normalized);
    }
}
