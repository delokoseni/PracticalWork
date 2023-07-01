using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public static Spawner Singleton { get; private set; } // Паттерн синглтон (объект данного класса может быть лишь 1)
    public GameObject herbivorePrefab; //
    public GameObject plantPrefab; //
    public GameObject predatorPrefab; //
    public GameObject scavengerPrefab; //
    public GameObject carrionPrefab; // 
    public List<Vector2> carrionList; // 
    public List<Vector2> plantList; //
    public List<Vector2> herbivoreList; //???
    public List<Vector2> predatorList; //
    public List<Vector2> scavengerList; //
    private int numberOfHerbivores; // Стартовое количество травоядных
    private int numberOfPredators; // Стартовое количество хищников
    private int numberOfScavengers; // Стартовое количество падальщиков
    private int numberOfPlants; // Стартовое количество растений
    private float time; // Время, через которое растения снова появятся
    public bool spawned = false;
    private void Update()
    {
        if (spawned) {
            GameObject[] arr = GameObject.FindGameObjectsWithTag("Herbivore");
            NewCreatures(arr,numberOfHerbivores, herbivorePrefab);
            arr = GameObject.FindGameObjectsWithTag("Predator");
            NewCreatures(arr, numberOfPredators, predatorPrefab);
            arr = GameObject.FindGameObjectsWithTag("Scavenger");
            NewCreatures(arr, numberOfScavengers, scavengerPrefab);
        }
    }
    private void Awake()
    {
        Singleton = this;
    }
    void Spawn(GameObject Prefab, int number)
    {
        for (int i = 0; i < number; i++)
        {
            Vector2 randomPosition = new (Random.Range(0f, UIManager.Singleton.GetWidth()), 
                Random.Range(0f, UIManager.Singleton.GetHeight()));
            Vector2 spawnPosition = Camera.main.ScreenToWorldPoint(randomPosition);
            Instantiate(Prefab, spawnPosition, Quaternion.identity);
        }
    }
    void SpawnPlant()
    {
        for (int i = 0; i < numberOfPlants; i++)
        {
            Vector2 randomPosition = new (Random.Range(0f, UIManager.Singleton.GetWidth()),
                Random.Range(0f, UIManager.Singleton.GetHeight()));
            Vector2 spawnPosition = Camera.main.ScreenToWorldPoint(randomPosition);
            GameObject newPlant = Instantiate(plantPrefab, spawnPosition, Quaternion.identity);
            plantList.Add(newPlant.transform.position);
        }
    }
    public void SpawnCarrion(Vector2 spawnPosition)
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
        spawned = false;
    }
    void SetData()
    {
        numberOfHerbivores = UIManager.Singleton.GetnumberOfHerbivores();
        numberOfPredators = UIManager.Singleton.GetnumberOfPredators();
        numberOfScavengers = UIManager.Singleton.GetnumberOfScavengers();
        numberOfPlants = UIManager.Singleton.GetnumberOfPlants();
        if (numberOfHerbivores != -1 && numberOfPredators != -1 && numberOfScavengers != -1 && numberOfPlants != -1)
        {
            SetRespawnTime();
            InvokeRepeating(nameof(SpawnPlant), 0, time); // Создает исходное количество растений раз в time секунд
            Spawn(herbivorePrefab, numberOfHerbivores); // Создает исходиное количество травоядных
            Spawn(predatorPrefab, numberOfPredators); // Создает исходиное количество хищников
            Spawn(scavengerPrefab, numberOfScavengers); // Создает исходиное количество падальщиков
            spawned = true;
        }
    }
    void SetRespawnTime()
    {
        if (UIManager.Singleton.GetTimeOfPlantsRespawn() != -1)
            time = UIManager.Singleton.GetTimeOfPlantsRespawn();
        else
            time = 20f;
    }
    void NewCreatures(GameObject[] arr, int numberOf, GameObject prefab)
    {
        System.Random rand = new();
        if (arr.Length <= 3 && numberOf > 3)
        {
            Spawn(prefab, rand.Next(5));
        }
        if (arr.Length == 0 && numberOf <= 3)
        {
            Spawn(prefab, rand.Next(5));
        }
    }
}
