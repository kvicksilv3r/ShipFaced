using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPowerUp : MonoBehaviour
{
    public GameObject powerup;
    public List<GameObject> spawnAreas = new List<GameObject>();
    void Start()
    {
        Spawn();
        Spawn();
    }

    void Spawn()
    {
        GameObject spawnArea = spawnAreas[Random.Range(0, spawnAreas.Count)];
        Instantiate(powerup, new Vector3(Random.Range(spawnArea.GetComponent<BoxCollider>().bounds.min.x, spawnArea.GetComponent<BoxCollider>().bounds.max.x), 0.5f, Random.Range(spawnArea.GetComponent<BoxCollider>().bounds.min.z, spawnArea.GetComponent<BoxCollider>().bounds.max.z)), Quaternion.identity);
    }
}
