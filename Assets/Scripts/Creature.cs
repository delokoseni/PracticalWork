using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Creature : MonoBehaviour
{
    Rigidbody2D rb; // Класс, описывающий физику объекта 
    Vector2 targetPosition; // Позиция, в которую движется объект класса
    bool isMoving = false; // Нобходимо для проверки, находится ли объект класса в движении
    float speed = 5f; // Скорость 
    int startenergy = 100; // Стартовая энергия 
    int energy; // Текущая энергия 
    float size = 1; // Размер 
    float time = 1f; // Время, за которое расходуется 1 единица энергии
    int chanceOfMutation = 100; // Шанс, с которым потомок сможет мутировать
    Color32 color; // Цвет существа
    public GameObject creaturePrefab; // 

    void Start() // Метод, вызываемый при воспроизведении первого кадра
    {
        rb = GetComponent<Rigidbody2D>(); // Получает ссылку на указанный объект класса
        Vector3 sizetoscale = new Vector3(size, size); // Вектор для присвоения размера
        transform.localScale = sizetoscale; // Присвоение стартового размера объекта класса
        energy = startenergy;
        transform.localScale = new Vector3(size, size, 0);
        targetPosition = new Vector2(Random.Range(0f, Screen.width), Random.Range(0f, Screen.height));
        Move(targetPosition);
        InvokeRepeating("Decreaseenergy", time, time); //Каждое time кол-во времени вызывается метод
    }

    void Update() // Метод, вызываемый каждый кадр
    {

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
            Vector2 newtargetPosition = new Vector2(Random.Range(0f, Screen.width), Random.Range(0f, Screen.height));
            targetPosition = newtargetPosition;
            Move(newtargetPosition);
        }
    }

    // Метод, реагирующий на контакты с другими объектами
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall")) // Попытка исправить прилипание к стенам
        {
            Vector2 newtargetPosition = new Vector2(Random.Range(0f, Screen.width), Random.Range(0f, Screen.height));
            targetPosition = newtargetPosition;
            Move(newtargetPosition);
        }
        if (collision.gameObject.CompareTag("Creature")) // Попытка исправить слипание между собой
        {
            Vector2 newtargetPosition = new Vector2(Random.Range(0f, Screen.width), Random.Range(0f, Screen.height));
            targetPosition = newtargetPosition;
            Move(newtargetPosition);
        }
        if (collision.gameObject.CompareTag("Plant")) 
        {
            Eat(collision.gameObject.GetComponent<Plant>());
        }
    }

    void Move(Vector2 newPosition) // Метод передвижения
    {
        targetPosition = Camera.main.ScreenToWorldPoint(newPosition);
        isMoving = true;
    }

    void Decreaseenergy() // Метод утраты энергии
    {
        energy--;
        if (energy == 0)
        {
            Die();
        }
    }

    void Die() // Метод смерти
    {
        Destroy(gameObject);
    }

    void Eat(Plant plant) // Метод питания
    {
        int receivedenergy = plant.Die();
        energy += receivedenergy;
        if (energy > startenergy)
            energy = startenergy;
        if (energy == 100)
            Multiply();
    }

    void Multiply() // Метод размножения
    {
        Vector3 spawnPosition = transform.position;
        GameObject newcreature = Instantiate(creaturePrefab, spawnPosition, Quaternion.identity);
        Mutate(newcreature.GetComponent<Creature>());
    }

    void Mutate(Creature newcreature) // Метод мутирования
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
                        newcreature.speed++;
                    else
                        newcreature.speed--;
                    newcreature.GetComponent<SpriteRenderer>().color -= new Color32(5, 0, 0, 0);
                    break;
                case 1:
                    n = rand.Next(2);
                    if (n == 1)
                        newcreature.startenergy++;
                    else
                        newcreature.startenergy--;
                    newcreature.GetComponent<SpriteRenderer>().color -= new Color32(0, 5, 0, 0);
                    break;
                case 2:
                    n = rand.Next(2);
                    if (n == 1)
                        newcreature.size++;
                    else
                        newcreature.size--;
                    newcreature.GetComponent<SpriteRenderer>().color -= new Color32(0, 0, 5, 0);
                    break;
                case 3:
                    n = rand.Next(2);
                    if (n == 1)
                        newcreature.time++;
                    else
                        newcreature.time--;
                    newcreature.GetComponent<SpriteRenderer>().color -= new Color32(5, 0, 5, 0);
                    break;
                case 4:
                    n = rand.Next(2);
                    if (n == 1)
                        newcreature.chanceOfMutation++;
                    else
                        newcreature.chanceOfMutation--;
                    newcreature.GetComponent<SpriteRenderer>().color -= new Color32(5, 5, 0, 0);
                    break;
            }
        }
    }
}
