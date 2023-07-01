using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scavenger : Creature
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Carrion"))
        {
            Eat(collision.gameObject.GetComponent<Carrion>());
        }
    }

    public void Eat(Carrion carrion) // Метод питания
    {
        int receivedenergy = carrion.Eaten();
        energy += receivedenergy;
        if (energy > startenergy)
            energy = startenergy;
        if (energy == 100)
            Multiply();
    }

    public override void Move(Vector2 newPosition) // Метод передвижения
    {
        targetPosition = Camera.main.ScreenToWorldPoint(newPosition);
        isMoving = true;
    }
}
