using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Predator : Creature
{
    void FixedUpdate() // 
    {
        if (isMoving)
        { // Тут движение
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
        else
        {
            Vector2 newtargetPosition = new(UnityEngine.Random.Range(0f, UIManager.Singleton.GetWidth()),
                UnityEngine.Random.Range(0f, UIManager.Singleton.GetHeight()));
            targetPosition = newtargetPosition;
            Move(newtargetPosition);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Herbivore")) 
        {
            Eat(collision.gameObject.GetComponent<Creature>());
        }
    }
    public void Eat(Creature creature) // Метод питания
    {
        if (size >= creature.GetSize()) {
            System.Random rand = new();
            if (rand.Next(0, 2) == 1)
            {
                int receivedenergy = UIManager.Singleton.GetEnergy();
                creature.Die();
                energy += receivedenergy;
                if (energy > startenergy)
                    energy = startenergy;
                if (energy == 100)
                    Multiply();
            }
        }
    }

    public override void Move(Vector2 newPosition) // Метод передвижения
    {
        targetPosition = Camera.main.ScreenToWorldPoint(newPosition);
        isMoving = true;
    }

}
