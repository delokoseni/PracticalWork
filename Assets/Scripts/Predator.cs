using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Predator : Creature
{
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Creature")) 
        {
            Eat(collision.gameObject.GetComponent<Creature>());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Plant") && energy <= 20)
        {
            Eat(collision.gameObject.GetComponent<Plant>());
        }
    }

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
            Vector2 newtargetPosition = new (UnityEngine.Random.Range(0f, UIManager.Singleton.GetWidth()),
                UnityEngine.Random.Range(0f, UIManager.Singleton.GetHeight()));
            targetPosition = newtargetPosition;
            Move(newtargetPosition);
        }
    }

    public void Eat(Creature creature) // Метод питания
    {
        int receivedenergy = 20;
        creature.Die();
        energy += receivedenergy;
        if (energy > startenergy)
            energy = startenergy;
        if (energy == 100)
            Multiply();
    }

    public new void Move(Vector2 newPosition) // Метод передвижения
    {
        targetPosition = Camera.main.ScreenToWorldPoint(newPosition);
        isMoving = true;
    }
}
