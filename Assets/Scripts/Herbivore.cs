using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Herbivore : Creature
{
    void Start() // Метод, вызываемый при воспроизведении первого кадра
    {
        Born(); // Устанавливает исходные значения всех полей
        Move();
    }
    private void Update()
    {
        if (!isMoving)
        {
            Move();
        }
    }
    private void Move()
    {
        if (Spawner.Singleton.plantList.Count == 0)
        {
            targetPosition = GetRandomPositionOnScreen();
        }
        else
        {
            int index = FindNearestPositionIndex();
            targetPosition = Spawner.Singleton.plantList[index];
        }
        isMoving = true;
    }
    private int FindNearestPositionIndex()
    {
        int index = 0;
        float minDistance = Vector2.Distance(transform.position, Spawner.Singleton.plantList[0]);
        for (int i = 1; i < Spawner.Singleton.plantList.Count; i++)
        {
            float distance = Vector2.Distance(transform.position, Spawner.Singleton.plantList[i]);
            if (distance < minDistance)
            {
                minDistance = distance;
                index = i;
            }
        }
        return index;
    }
    private Vector2 GetRandomPositionOnScreen()
    {
        Vector2 randomPosition = new (Random.Range(0f, UIManager.Singleton.GetWidth()),
            Random.Range(0f, UIManager.Singleton.GetHeight()));
        return Camera.main.ScreenToWorldPoint(randomPosition);
    }
    private void FixedUpdate()
    {
        if (isMoving)
        {
            Vector2 currentPosition = rb.position;
            Vector2 direction = (targetPosition - currentPosition).normalized;
            float distance = Vector2.Distance(currentPosition, targetPosition);
            if (distance > 0.1f)
            {
                rb.MovePosition(currentPosition + speed * Time.fixedDeltaTime * direction);
            }
            else
            {
                isMoving = false;
            }
        }
    }
    public override void Move(Vector2 newPosition) // Метод передвижения
    {
        targetPosition = Camera.main.ScreenToWorldPoint(newPosition);
        isMoving = true;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Plant"))
        {
            Eat(collision.gameObject.GetComponent<Plant>());
        }
    }
    public void Eat(Plant plant) // Метод питания
    {
        int receivedenergy = plant.Die();
        energy += receivedenergy;
        if (energy > startenergy)
            energy = startenergy;
        if (energy == startenergy)
            Multiply();
    }
}
