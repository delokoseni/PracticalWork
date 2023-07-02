using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Burst.Intrinsics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

abstract public class Creature : MonoBehaviour
{
    public Rigidbody2D rb; // �����, ����������� ������ ������� 
    protected Vector2 targetPosition; // �������, � ������� �������� ������ ������
    protected bool isMoving = false; // ��������� ��� ��������, ��������� �� ������ ������ � ��������
    protected float speed; // �������� 
    protected int startenergy; // ��������� ������� 
    protected int energy; // ������� ������� 
    protected float size; // ������ 
    protected float time; // �����, �� ������� ����������� 1 ������� �������
    protected int chanceOfMutation; // ����, � ������� ������� ������ ����������
    protected Color32 color; // ���� ��������
    public GameObject creaturePrefab; //  
    public static Action<string> WasClicked; // �������
    private void Awake()
    {
        SetData();
    }
    
    private void OnEnable()
    {
        UIManager.TheEndOfTheWorld += Die; // �������� �� ������� TheEndOfTheWorld
    }
    private void OnDisable()
    {
        UIManager.TheEndOfTheWorld -= Die; // ������� �� ������� TheEndOfTheWorld
    }
    public void Decreaseenergy() // ����� ������ �������
    {
        energy--;
        if (energy == 0)
        {
            Die();
        }
    }
    public void Die() // ����� ������
    {
        Vector3 position = transform.position;
        Destroy(gameObject);
        if (!UIManager.Singleton.wasTheEndOfTheWorld)
            Spawner.Singleton.SpawnCarrion(position);
    }
    public void Multiply() // ����� �����������
    {
        Vector3 spawnPosition = transform.position;
        GameObject newcreature = Instantiate(creaturePrefab, spawnPosition, Quaternion.identity);
        Mutate(newcreature.GetComponent<Creature>());
    }
    public void Mutate(Creature newcreature) // ����� �����������
    {
        System.Random rand = new ();
        if(rand.Next(1,101) <= chanceOfMutation)
        {
            int n = rand.Next(5);
            switch (n)
            {
                case 0:
                    n = rand.Next(2);
                    if (n == 1)
                    {
                        newcreature.speed = speed + 1f;
                        newcreature.GetComponent<SpriteRenderer>().color -= new Color32(0, 0, 5, 0);
                    }
                    else
                    {
                        newcreature.speed = speed - 1f;
                        newcreature.GetComponent<SpriteRenderer>().color += new Color32(0, 0, 5, 0);
                    }
                    break;
                case 1:
                    n = rand.Next(2);
                    if (n == 1)
                    {
                        newcreature.startenergy = startenergy + 1;
                        newcreature.GetComponent<SpriteRenderer>().color -= new Color32(0, 5, 0, 0);
                    }
                    else
                    {
                        newcreature.startenergy = startenergy - 1;
                        newcreature.GetComponent<SpriteRenderer>().color += new Color32(0, 5, 0, 0);
                    }
                    break;
                case 2:
                    n = rand.Next(2);
                    if (n == 1)
                    {
                        newcreature.size = size + 0.1f;
                        newcreature.GetComponent<SpriteRenderer>().color -= new Color32(0, 5, 5, 0);
                    }
                    else
                    {
                        newcreature.size = size - 0.1f;
                        newcreature.GetComponent<SpriteRenderer>().color += new Color32(0, 5, 5, 0);
                    }
                    break;
                case 3:
                    n = rand.Next(2);
                    if (n == 1)
                    {
                        newcreature.time = time + 0.1f;
                        newcreature.GetComponent<SpriteRenderer>().color -= new Color32(5, 0, 0, 0);
                    }
                    else
                    {
                        newcreature.time = time - 0.1f;
                        newcreature.GetComponent<SpriteRenderer>().color += new Color32(5, 0, 0, 0);
                    }
                    break;
                case 4:
                    n = rand.Next(2);
                    if (n == 1)
                    {
                        if (chanceOfMutation < 100) {
                            newcreature.chanceOfMutation = chanceOfMutation + 1;
                            newcreature.GetComponent<SpriteRenderer>().color -= new Color32(5, 0, 5, 0);
                        }
                    }
                    else
                    {
                        newcreature.chanceOfMutation = chanceOfMutation - 1;
                        newcreature.GetComponent<SpriteRenderer>().color += new Color32(5, 0, 5, 0);
                    }
                    break;
            }
        }
    }
    public void SetData() // ������������� �������� ������
    {
        speed = UIManager.Singleton.GetSpeed();
        size = UIManager.Singleton.GetSize();
        time = UIManager.Singleton.GetTime();
        startenergy = UIManager.Singleton.GetStartEnergy();
        chanceOfMutation = UIManager.Singleton.GetChanseOfMutation();
    }
    public void OnMouseDown()
    {
        int countOfHerbivores, countOfPredators, countOfScavengers, countOfPlants;
        GameObject[] arr = GameObject.FindGameObjectsWithTag("Herbivore");
        countOfHerbivores = arr.Count();
        arr = GameObject.FindGameObjectsWithTag("Predator");
        countOfPredators = arr.Count();
        arr = GameObject.FindGameObjectsWithTag("Scavenger");
        countOfScavengers = arr.Count();
        arr = GameObject.FindGameObjectsWithTag("Plant");
        countOfPlants = arr.Count();
        string str = "��������: " + speed + "\n������: " + size + "\n��������� �������: " + startenergy + 
            "\n������� ������: " + energy +
            "\n�����, �� ������� ����������� 1 ��. �������: " + time + "\n���� ������� �������: " +
            chanceOfMutation + "%\n����: " + GetComponent<SpriteRenderer>().color.ToString() + 
            "\n���������� ����������: " + countOfHerbivores + "\n���������� ��������: " + countOfPredators
             + "\n���������� �����������: " + countOfScavengers + "\n���������� ��������: " + countOfPlants;
        WasClicked?.Invoke(str);
    }
    public float GetSize() { return size; }
    public void Born()
    {
        rb = GetComponent<Rigidbody2D>(); // �������� ������ �� ��������� ������ ������
        Vector2 sizetoscale = new(size, size); // ������ ��� ���������� �������
        transform.localScale = sizetoscale; // ���������� ���������� ������� ������� ������
        energy = startenergy; // ���������� ������� �������
        transform.localScale = new Vector3(size, size, 0); // ���������� ��������;
        targetPosition = new Vector2(UnityEngine.Random.Range(0f, UIManager.Singleton.GetWidth()),
            UnityEngine.Random.Range(0f, UIManager.Singleton.GetHeight()));
        InvokeRepeating(nameof(Decreaseenergy), time, time); // ������ time ���-�� ������� ���������� �����
    }
}
