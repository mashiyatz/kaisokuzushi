using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleBehavior : MonoBehaviour
{
    private ObstacleGenerator obstacleGenerator;
    private GameObject obstacleMeshPrefab;
    public static event Action OnObstacleCollision;

    void Start()
    {
        obstacleGenerator = transform.parent.GetComponent<ObstacleGenerator>();
        obstacleMeshPrefab = obstacleGenerator.GetRandomMesh();
        GameObject go = Instantiate(obstacleMeshPrefab, transform);
        go.transform.localScale = Vector3.one / 2;
        go.transform.eulerAngles = new Vector3(0, UnityEngine.Random.Range(0, 180f), 0);
    }

    void Update()
    {
        transform.Translate(GlobalVars.obstacleSpeedMultiplier * GlobalVars.runningSpeed * Time.deltaTime * Vector3.back);
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) {
            OnObstacleCollision.Invoke();
        } else if (other.CompareTag("Bound"))
        {
            Destroy(gameObject);
        }
    }
}
