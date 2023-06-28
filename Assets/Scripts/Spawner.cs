using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public static Spawner Singleton { get; private set; } // Паттерн синглтон (объект данного класса может быть лишь 1)
    public GameObject creaturePrefab; //
    public GameObject plantPrefab; //
    public GameObject predatorPrefab; //
    public GameObject scavengerPrefab; //
    public GameObject carrionPrefab; // 
    public List<Vector2> carrionList; // 
    public List<Vector2> creatureList; //???
    public List<Vector2> plantList; //
    private int numberOfCreatures; // Количество травоядных
    private int numberOfPredators; // Количество хищников
    private int numberOfScavengers; // Количество падальщиков
    private int numberOfPlants; // Количество растений
    private float time; // Время, через которое растения снова появятся

    private void Awake()
    {
        Singleton = this;
    }

    void Spawn(GameObject Prefab, int number)
    {
        for (int i = 0; i < number; i++)
        {
            Vector2 randomPosition = new Vector3(Random.Range(0f, UIManager.Singleton.GetWidth()), 
                Random.Range(0f, UIManager.Singleton.GetHeight()));
            Vector2 spawnPosition = Camera.main.ScreenToWorldPoint(randomPosition);
            Instantiate(Prefab, spawnPosition, Quaternion.identity);
        }
    }

    void SpawnPlant()
    {
        for (int i = 0; i < numberOfPlants; i++)
        {
            Vector2 randomPosition = new Vector3(Random.Range(0f, UIManager.Singleton.GetWidth()),
                Random.Range(0f, UIManager.Singleton.GetHeight()));
            Vector2 spawnPosition = Camera.main.ScreenToWorldPoint(randomPosition);
            GameObject newPlant = Instantiate(plantPrefab, spawnPosition, Quaternion.identity);
            plantList.Add(newPlant.transform.position);
        }
    }

    public void SpawnCarrion(Vector3 spawnPosition)
    {
        GameObject newCarrion = Instantiate(carrionPrefab, spawnPosition, Quaternion.identity);
        carrionList.Add(newCarrion.transform.position);
    }

    private void OnEnable()
    {
        UIManager.TheEndOfTheWorld += EndOfSpawning; // Подписка на событие TheEndOfTheWorld
        UIManager.StartOfTheWorld += SetData; // Подписка на событие StartOfTheWorld
        UIManager.DataIsDone += SetData;
    }

    private void OnDisable() // ???
    {
        UIManager.TheEndOfTheWorld -= EndOfSpawning; // Отписка от события TheEndOfTheWorld
        UIManager.StartOfTheWorld -= SetData; // Отписка от события StartOfTheWorld
        UIManager.DataIsDone -= SetData;
    }

    void EndOfSpawning()
    {
        CancelInvoke(nameof(SpawnPlant));
        Spawner.Singleton.carrionList.Clear();
        Spawner.Singleton.plantList.Clear();
    }

    void SetData()
    {
        numberOfCreatures = UIManager.Singleton.GetnumberOfHerbivores();
        numberOfPredators = UIManager.Singleton.GetnumberOfPredators();
        numberOfScavengers = UIManager.Singleton.GetnumberOfScavengers();
        numberOfPlants = UIManager.Singleton.GetnumberOfPlants();
        if (numberOfCreatures != -1 || numberOfPredators != -1 || numberOfScavengers != -1)
        {
            if (numberOfPlants != -1)
            {
                InvokeRepeating(nameof(SpawnPlant), 0, time); // Создает исходное количество растений раз в time секунд
                if (UIManager.Singleton.GetnumberOfHerbivores() != -2)
                    Spawn(creaturePrefab, numberOfCreatures); // Создает исходиное количество травоядных
                if (UIManager.Singleton.GetnumberOfPredators() != -1)
                    Spawn(predatorPrefab, numberOfPredators); // Создает исходиное количество хищников
                if (UIManager.Singleton.GetnumberOfScavengers() != -1)
                    Spawn(scavengerPrefab, numberOfScavengers); // Создает исходиное количество падальщиков
            }
        }
    }
    
    void SetRespawnTime()
    {
        if (UIManager.Singleton.GetTimeOfPlantsRespawn() != -1)
            time = UIManager.Singleton.GetTimeOfPlantsRespawn();
        else
            time = 20f;
    }
}
