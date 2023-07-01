using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Herbivore : Creature
{
    private void Move()
    {
        if (Spawner.Singleton.plantList.Count == 0)
        {
            Debug.Log("No positions available.");
            return;
        }

        int index = FindNearestPositionIndex();
        targetPosition = Spawner.Singleton.plantList[index];
        isMoving = true;
    }

    private int FindNearestPositionIndex()
    {
        float minDistance = Vector2.Distance(transform.position, Spawner.Singleton.plantList[0]);
        int index = 0;

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

    private void FixedUpdate()
    {
        //Debug.Log(Spawner.Singleton.plantList.Count);
        if (!isMoving)
        {
            if(Spawner.Singleton.plantList.Count != 0)
                Move();
        }
        else
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
        if (energy == 100)
            Multiply();
    }

}
