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
    public Rigidbody2D rb; // Класс, описывающий физику объекта 
    protected Vector2 targetPosition; // Позиция, в которую движется объект класса
    protected bool isMoving = false; // Нобходимо для проверки, находится ли объект класса в движении
    protected float speed; // Скорость 
    protected int startenergy; // Стартовая энергия 
    protected int energy; // Текущая энергия 
    protected float size; // Размер 
    protected float time; // Время, за которое расходуется 1 единица энергии
    protected int chanceOfMutation; // Шанс, с которым потомок сможет мутировать
    protected Color32 color; // Цвет существа
    public GameObject creaturePrefab; //  
    public static Action<string> WasClicked; // Делегат
    private void Awake()
    {
        SetData();
    }
    
    private void OnEnable()
    {
        UIManager.TheEndOfTheWorld += Die; // Подписка на событие TheEndOfTheWorld
    }
    private void OnDisable()
    {
        UIManager.TheEndOfTheWorld -= Die; // Отписка от события TheEndOfTheWorld
    }
    public void Decreaseenergy() // Метод утраты энергии
    {
        energy--;
        if (energy == 0)
        {
            Die();
        }
    }
    public void Die() // Метод смерти
    {
        Vector3 position = transform.position;
        Destroy(gameObject);
        if (!UIManager.Singleton.wasTheEndOfTheWorld)
            Spawner.Singleton.SpawnCarrion(position);
    }
    public void Multiply() // Метод размножения
    {
        Vector3 spawnPosition = transform.position;
        GameObject newcreature = Instantiate(creaturePrefab, spawnPosition, Quaternion.identity);
        Mutate(newcreature.GetComponent<Creature>());
    }
    public void Mutate(Creature newcreature) // Метод мутирования
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
    public void SetData() // Устанавливает исходные данные
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
        string str = "Скорость: " + speed + "\nРазмер: " + size + "\nСтартовая энергия: " + startenergy + 
            "\nТекущая энерия: " + energy +
            "\nВремя, за которое расходуется 1 ед. энергии: " + time + "\nШанс мутации потомка: " +
            chanceOfMutation + "%\nЦвет: " + GetComponent<SpriteRenderer>().color.ToString() + 
            "\nКоличество травоядных: " + countOfHerbivores + "\nКоличество хищников: " + countOfPredators
             + "\nКоличество падальщиков: " + countOfScavengers + "\nКоличество растений: " + countOfPlants;
        WasClicked?.Invoke(str);
    }
    public float GetSize() { return size; }
    public void Born()
    {
        rb = GetComponent<Rigidbody2D>(); // Получает ссылку на указанный объект класса
        Vector2 sizetoscale = new(size, size); // Вектор для присвоения размера
        transform.localScale = sizetoscale; // Присвоение стартового размера объекта класса
        energy = startenergy; // Присвоение текущей энергии
        transform.localScale = new Vector3(size, size, 0); // Присвоение размеров;
        targetPosition = new Vector2(UnityEngine.Random.Range(0f, UIManager.Singleton.GetWidth()),
            UnityEngine.Random.Range(0f, UIManager.Singleton.GetHeight()));
        InvokeRepeating(nameof(Decreaseenergy), time, time); // Каждое time кол-во времени вызывается метод
    }
}
