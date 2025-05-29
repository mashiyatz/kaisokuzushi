using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuchkaBehavior : MonoBehaviour
{
    public static event Action OnFuchkaCollision;

    void Start()
    {
        
    }

    void Update()
    {
        transform.position = new Vector3(
            transform.position.x, 
            Mathf.Sin(2 * Time.time)/2 + 1f, 
            transform.position.z - GlobalVars.runningSpeed * Time.deltaTime
        );
        transform.RotateAround(transform.position, Vector3.up, Time.deltaTime * 15);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OnFuchkaCollision.Invoke();
            Destroy(gameObject);
        }
        else if (other.CompareTag("Bound"))
        {
            Destroy(gameObject);
        }
    }
}
