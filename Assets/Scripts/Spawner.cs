using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject creaturePrefab; //
    public GameObject plantPrefab; //
    public GameObject predatorPrefab; //
    public GameObject scavengerPrefab; //
    int numberOfCreatures; // Количество травоядных
    int numberOfPredators; // Количество хищников
    int numberOfScavengers; // Количество падальщиков
    int numberOfPlants; // Количество растений
    float time = 20f; // Время, через которое растения снова появятся

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
        UIManager.StartOfTheWorld += SetData; // Подписка на событие StartOfTheWorld
    }

    private void OnDisable() // ???
    {
        //UIManager.TheEndOfTheWorld -= EndOfSpawning; // Отписка от события TheEndOfTheWorld
        //UIManager.StartOfTheWorld -= SetData; // Отписка от события StartOfTheWorld
    }

    void EndOfSpawning()
    {
        CancelInvoke("SpawnPlant");
    }

    void SetData()
    {
        numberOfCreatures = UIManager.singleton.GetnumberOfHerbivores();
        numberOfPredators = UIManager.singleton.GetnumberOfPredators();
        numberOfScavengers = UIManager.singleton.GetnumberOfScavengers();
        numberOfPlants = UIManager.singleton.GetnumberOfPlants();
        if (numberOfCreatures != -1 && numberOfPredators != -1 && numberOfScavengers != -1 && numberOfPlants != -1)
        {
            InvokeRepeating("SpawnPlant", 0, time); // Создает исходное количество растений раз в time секунд
            Spawn(creaturePrefab, numberOfCreatures); // Создает исходиное количество травоядных
            Spawn(predatorPrefab, numberOfPredators); // Создает исходиное количество хищников
            Spawn(scavengerPrefab, numberOfScavengers); // Создает исходиное количество падальщиков
        }
    }
}
