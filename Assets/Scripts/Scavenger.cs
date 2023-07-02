using UnityEngine;

public class Scavenger : Creature
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
    private void Move()
    {
        if (Spawner.Singleton.carrionList.Count == 0)
        {
            targetPosition = new(Random.Range(-10f, 10f), Random.Range(-10f, 10f));
        }
        else
        {
            int index = FindNearestPositionIndex(Spawner.Singleton.carrionList);
            targetPosition = Spawner.Singleton.carrionList[index];
        }

        isMoving = true;
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
        if (energy == startenergy)
            Multiply();
    }
}
