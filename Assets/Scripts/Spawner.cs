using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject creaturePrefab; //
    public GameObject plantPrefab; //
    public GameObject predatorPrefab; //
    public GameObject scavengerPrefab; //
    public int numberOfCreatures; // ���������� ����������
    public int numberOfPredators; // ���������� ��������
    public int numberOfScavengers; // ���������� �����������
    public int numberOfPlants; // ���������� ��������
    float time = 20f; // �����, ����� ������� �������� ����� ��������
    private void Start()
    {
        InvokeRepeating("SpawnPlant", 0, time); // ������� �������� ���������� �������� ��� � time ������
        Spawn(creaturePrefab, numberOfCreatures); // ������� ��������� ���������� ����������
        Spawn(predatorPrefab, numberOfPredators); // ������� ��������� ���������� ��������
        Spawn(scavengerPrefab, numberOfScavengers); // ������� ��������� ���������� �����������
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
        UIManager.TheEndOfTheWorld += EndOfSpawning; // �������� �� ������� TheEndOfTheWorld
    }

    private void OnDisable()
    {
        UIManager.TheEndOfTheWorld -= EndOfSpawning; // ������� �� ������� TheEndOfTheWorld
    }

    void EndOfSpawning()
    {
        CancelInvoke("SpawnPlant");
    }
}
