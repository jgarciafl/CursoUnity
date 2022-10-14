using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement_Transform : MonoBehaviour
{
    [SerializeField] float speed = 5f;
    [SerializeField] bool throughWalls = false;


    private void Update()
    {
        if (throughWalls)
            speed = 15f;

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

    void Move(Vector3 direction) 
    {
        transform.position += direction.normalized * speed * Time.deltaTime;
    }

}
