using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature : MonoBehaviour
{
    Rigidbody2D rb; // Класс, описывающий физику объекта 
    Vector2 targetPosition; // Позиция, в которую движется объект класса
    bool isMoving = false; // Нобходимо для проверки, находится ли объект класса в движении
    float speed = 5f; // Скорость 
    int energy = 100; // Энергия 
    float size = 1; // Размер 
    float time = 1f; // Время, за которое расходуется 1 единица энергии

    void Start() // Метод, вызываемый при воспроизведении первого кадра
    {
        rb = GetComponent<Rigidbody2D>(); // Получает ссылку на указанный объект класса
        Vector3 sizetoscale = new Vector3(size, size); // Вектор для присвоения размера
        transform.localScale = sizetoscale; // Присвоение стартового размера объекта класса
        Move(); 
        InvokeRepeating("Decreaseenergy", time, time); //Каждое time кол-во времени вызывается метод
    }

    void Update() // Метод, вызываемый каждый кадр
    {
        if (!isMoving)
        {
            Move();
        }
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
    }

    // Метод, реагирующий на контакты с другими объектами
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall")) // Попытка исправить прилипание к стенам
        {
            isMoving = false;
        }
        if (collision.gameObject.CompareTag("Creature")) // Попытка исправить слипание между собой
        {
            isMoving = false;
        }
    }

    void Move() // Метод передвижения
    {
        targetPosition = new Vector2(Random.Range(0f, Screen.width), Random.Range(0f, Screen.height));
        targetPosition = Camera.main.ScreenToWorldPoint(targetPosition);
        isMoving = true;
    }

    void Decreaseenergy()
    {
        energy--;
        if (energy <= 0)
        {
            Die();
        }
    }

    void Die() // Метод смерти
    {
        Destroy(gameObject);
    }

    void Eat() // Метод питания
    {

    }

    void Mutate() // Метод мутирования
    {

    }
}
