using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Creature : MonoBehaviour
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
    public static Action<string> WasClicked;

    void Start() // Метод, вызываемый при воспроизведении первого кадра
    {
        rb = GetComponent<Rigidbody2D>(); // Получает ссылку на указанный объект класса
        Vector3 sizetoscale = new Vector3(size, size); // Вектор для присвоения размера
        transform.localScale = sizetoscale; // Присвоение стартового размера объекта класса
        energy = startenergy;
        transform.localScale = new Vector3(size, size, 0);
        targetPosition = new Vector2(UnityEngine.Random.Range(0f, UIManager.singleton.GetWidth()),
            UnityEngine.Random.Range(0f, UIManager.singleton.GetHeight()));
        Move(targetPosition);
        InvokeRepeating("Decreaseenergy", time, time); // Каждое time кол-во времени вызывается метод
    }

    private void Awake() // Вызывается лишь 1 раз для установки данных
    {
        SetData();
    }

    void FixedUpdate() //
    {
        if (isMoving)
        {
            Vector2 currentPosition = rb.position;
            Vector2 direction = (targetPosition - currentPosition).normalized;
            float distance = Vector2.Distance(currentPosition, targetPosition);

            if (distance > 0.1f)
            {
                rb.MovePosition(currentPosition + direction * speed * Time.fixedDeltaTime);
            }
            else
            {
                isMoving = false;
            }
        }
        else
        {
            Vector2 newtargetPosition = new Vector2(UnityEngine.Random.Range(0f, UIManager.singleton.GetWidth()),
                UnityEngine.Random.Range(0f, UIManager.singleton.GetHeight()));
            targetPosition = newtargetPosition;
            Move(newtargetPosition);
        }
    }

    // Метод, реагирующий на контакты с другими объектами
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall")) // Попытка исправить прилипание к стенам
        {
            Vector2 newtargetPosition = new Vector2(UnityEngine.Random.Range(0f, UIManager.singleton.GetWidth()), 
                UnityEngine.Random.Range(0f, UIManager.singleton.GetHeight()));
            targetPosition = newtargetPosition;
            Move(newtargetPosition);
        }
        if (collision.gameObject.CompareTag("Creature")) // Попытка исправить слипание между собой
        {
            Vector2 newtargetPosition = new Vector2(UnityEngine.Random.Range(0f, UIManager.singleton.GetWidth()),
                UnityEngine.Random.Range(0f, UIManager.singleton.GetHeight()));
            targetPosition = newtargetPosition;
            Move(newtargetPosition);
        }
        if (collision.gameObject.CompareTag("Plant")) 
        {
            Eat(collision.gameObject.GetComponent<Plant>());
        }
    }

    private void OnEnable()
    {
        UIManager.TheEndOfTheWorld += Die; // Подписка на событие TheEndOfTheWorld
    }

    private void OnDisable()
    {
        UIManager.TheEndOfTheWorld -= Die; // Отписка от события TheEndOfTheWorld
    }

    public void Move(Vector2 newPosition) // Метод передвижения
    {
        targetPosition = Camera.main.ScreenToWorldPoint(newPosition);
        isMoving = true;
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
        Spawner.singleton.SpawnCarrion(position);
    }

    public void Eat(Plant plant) // Метод питания
    {
        int receivedenergy = plant.Die();
        energy += receivedenergy;
        if (energy > startenergy)
            energy = startenergy;
        if (energy == 100)
            Multiply();
    }

    public void Multiply() // Метод размножения
    {
        Vector3 spawnPosition = transform.position;
        GameObject newcreature = Instantiate(creaturePrefab, spawnPosition, Quaternion.identity);
        Mutate(newcreature.GetComponent<Creature>());
    }

    public void Mutate(Creature newcreature) // Метод мутирования
    {
        System.Random rand = new System.Random();
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
        speed = UIManager.singleton.GetSpeed();
        size = UIManager.singleton.GetSize();
        time = UIManager.singleton.GetTime();
        startenergy = UIManager.singleton.GetStartEnergy();
        chanceOfMutation = UIManager.singleton.GetChanseOfMutation();
    }

    public void OnMouseDown()
    {
        string str = "Скорость: " + speed + "\nРазмер: " + size + "\nСтартовая энергия: " + startenergy +
            "\nВремя, за которое расходуется 1 ед. энергии: " + time + "\nШанс мутации потомка: " +
            chanceOfMutation + "%\nЦвет: " + GetComponent<SpriteRenderer>().color.ToString();
        WasClicked?.Invoke(str);
    }
}
