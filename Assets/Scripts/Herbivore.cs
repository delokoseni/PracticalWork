using UnityEngine;

public class Herbivore : Creature
{
    void Start() // Метод, вызываемый при воспроизведении первого кадра
    {
        Born();
        Move();
    }
    private void Update()
    {
        if (!isMoving)
        {
            Move();
        }
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
    private void Move()
    {
        if (Spawner.Singleton.plantList.Count == 0)
        {
            targetPosition = new(Random.Range(-10f, 10f), Random.Range(-10f, 10f));
        }
        else
        {
            int index = FindNearestPositionIndex(Spawner.Singleton.plantList);
            targetPosition = Spawner.Singleton.plantList[index];
        }

        isMoving = true;
    }
    // Метод, реагирующий на контакты с другими объектами
    void OnCollisionEnter2D(Collision2D collision)
    {
        Vector2 newtargetPosition = new(UnityEngine.Random.Range(0f, UIManager.Singleton.GetWidth()),
           UnityEngine.Random.Range(0f, UIManager.Singleton.GetHeight()));
        targetPosition = newtargetPosition;
        Move();
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
