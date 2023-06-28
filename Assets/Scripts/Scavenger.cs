using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scavenger : Creature
{
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Plant") && energy <= 20)
        {
            Eat(collision.gameObject.GetComponent<Plant>());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Carrion"))
        {
            Eat(collision.gameObject.GetComponent<Carrion>());
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

    public void Eat(Carrion carrion) // Метод питания
    {
        int receivedenergy = carrion.Eaten();
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
