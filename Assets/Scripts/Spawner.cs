using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject creaturePrefab;
    public int numberOfCreatures;

    private void Start()
    {
        SpawnCreature();
    }

    void SpawnCreature()
    {
        for (int i = 0; i < numberOfCreatures; i++)
        {
            Vector3 randomPosition = new Vector3(Random.Range(0f, Screen.width), Random.Range(0f, Screen.height), 0f);
            Vector3 spawnPosition = Camera.main.ScreenToWorldPoint(randomPosition);
            GameObject newcreature = Instantiate(creaturePrefab, spawnPosition, Quaternion.identity);
        }
    }
}
