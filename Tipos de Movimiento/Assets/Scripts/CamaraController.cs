using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CamaraController : MonoBehaviour
{
    [SerializeField] GameObject[] targets;
    GameObject currentTarget;
    Vector3 direction;
    float distance;
    bool asignada = false;

    private void Awake()
    {
        if (targets.Length > 0 && targets[0].activeSelf)
            SetTarget(targets[0]);
        else if (targets.Length > 1 && targets[1].activeSelf)
            SetTarget(targets[1]);
        else if (targets.Length > 2 && targets[2].activeSelf)
            SetTarget(targets[2]);
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.R))
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);


        if (targets.Length > 0 && targets[0].activeSelf)
        {
            if (currentTarget != targets[0])
                SetTarget(targets[0]);
        }
        else if (targets.Length > 1 && targets[1].activeSelf)
        {
            if (currentTarget != targets[1])
                SetTarget(targets[1]);
        }
        else if (targets.Length > 2 && targets[2].activeSelf)
        {
            if (currentTarget != targets[2])
                SetTarget(targets[2]);
        }
        else
            currentTarget = null;

        if (currentTarget!=null)
            transform.position = (currentTarget.transform.position + direction); 
    }

    void SetTarget(GameObject target)
    {
        if (!asignada)
        {
            asignada = true;
            direction = transform.position - target.transform.position;
            distance = direction.magnitude;
        }
        currentTarget = target;
    }
}
