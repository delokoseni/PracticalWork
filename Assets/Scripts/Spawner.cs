using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject creaturePrefab; //
    public GameObject plantPrefab; //
    public int numberOfCreatures; //
    public int numberOfPlants; //
    float time = 20f; // Время, через которое еда снова появится
    private void Start()
    {
        InvokeRepeating("SpawnPlant", 0, time);
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

    void SpawnPlant()
    {
        for (int i = 0; i < numberOfPlants; i++)
        {
            Vector3 randomPosition = new Vector3(Random.Range(0f, Screen.width), Random.Range(0f, Screen.height), 0f);
            Vector3 spawnPosition = Camera.main.ScreenToWorldPoint(randomPosition);
            GameObject newcreature = Instantiate(plantPrefab, spawnPosition, Quaternion.identity);
        }
    }
}
