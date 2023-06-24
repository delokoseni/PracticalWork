using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject creaturePrefab; //
    public GameObject plantPrefab; //
    public GameObject predatorPrefab; //
    public GameObject scavengerPrefab; //
    public int numberOfCreatures; // Количество травоядных
    public int numberOfPredators; // Количество хищников
    public int numberOfScavengers; // Количество падальщиков
    public int numberOfPlants; // Количество растений
    float time = 20f; // Время, через которое растения снова появятся
    private void Start()
    {
        InvokeRepeating("SpawnPlant", 0, time); // Создает исходное количество растений раз в time секунд
        Spawn(creaturePrefab, numberOfCreatures); // Создает исходиное количество травоядных
        Spawn(predatorPrefab, numberOfPredators); // Создает исходиное количество хищников
        Spawn(scavengerPrefab, numberOfScavengers); // Создает исходиное количество падальщиков
    }

    void Spawn(GameObject Prefab, int number)
    {
        for (int i = 0; i < number; i++)
        {
            Vector3 randomPosition = new Vector3(Random.Range(0f, Screen.width), Random.Range(0f, Screen.height), 1f);
            Vector3 spawnPosition = Camera.main.ScreenToWorldPoint(randomPosition);
            GameObject newGameObject = Instantiate(Prefab, spawnPosition, Quaternion.identity);
        }
    }

    void SpawnPlant()
    {
        for (int i = 0; i < numberOfPlants; i++)
        {
            Vector3 randomPosition = new Vector3(Random.Range(0f, Screen.width), Random.Range(0f, Screen.height), 1f);
            Vector3 spawnPosition = Camera.main.ScreenToWorldPoint(randomPosition);
            GameObject newcreature = Instantiate(plantPrefab, spawnPosition, Quaternion.identity);
        }
    }

    private void OnEnable()
    {
        UIManager.TheEndOfTheWorld += EndOfSpawning; // Подписка на событие TheEndOfTheWorld
    }

    private void OnDisable()
    {
        UIManager.TheEndOfTheWorld -= EndOfSpawning; // Отписка от события TheEndOfTheWorld
    }

    void EndOfSpawning()
    {
        CancelInvoke("SpawnPlant");
    }
}
