using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleGenerator : MonoBehaviour
{
    public GameObject[] obstacleMeshPrefabs;
    public GameObject obstacleShellPrefab;
    public GameObject fuchkaPrefab;

    private float z;
    private float timer;

    void Start()
    {
        z = transform.position.z;
    }

    public GameObject GetRandomMesh()
    {
        return obstacleMeshPrefabs[Random.Range(0, obstacleMeshPrefabs.Length)];
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer > GlobalVars.spawnInterval)
        {
            float p = Random.Range(0f, 1f);
            if (p < GlobalVars.fuchkaProbability)
            {
                Instantiate(fuchkaPrefab, new Vector3(Random.Range(-1, 2), 0, z), Quaternion.identity, transform);
            } else
            {
                Instantiate(obstacleShellPrefab, new Vector3(Random.Range(-1, 2), 0, z), Quaternion.identity, transform);
            }
            timer = 0f;
        }
    }
}
