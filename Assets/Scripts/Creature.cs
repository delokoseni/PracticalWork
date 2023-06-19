using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature : MonoBehaviour
{
    Rigidbody2D rb; // �����, ����������� ������ ������� 
    Vector2 targetPosition; // �������, � ������� �������� ������ ������
    bool isMoving = false; // ��������� ��� ��������, ��������� �� ������ ������ � ��������
    float speed = 5f; // �������� 
    int energy = 100; // ������� 
    float size = 1; // ������ 
    float time = 1f; // �����, �� ������� ����������� 1 ������� �������

    void Start() // �����, ���������� ��� ��������������� ������� �����
    {
        rb = GetComponent<Rigidbody2D>(); // �������� ������ �� ��������� ������ ������
        Vector3 sizetoscale = new Vector3(size, size); // ������ ��� ���������� �������
        transform.localScale = sizetoscale; // ���������� ���������� ������� ������� ������
        Move(); 
        InvokeRepeating("Decreaseenergy", time, time); //������ time ���-�� ������� ���������� �����
    }

    void Update() // �����, ���������� ������ ����
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

    // �����, ����������� �� �������� � ������� ���������
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall")) // ������� ��������� ���������� � ������
        {
            isMoving = false;
        }
        if (collision.gameObject.CompareTag("Creature")) // ������� ��������� �������� ����� �����
        {
            isMoving = false;
        }
    }

    void Move() // ����� ������������
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

    void Die() // ����� ������
    {
        Destroy(gameObject);
    }

    void Eat() // ����� �������
    {

    }

    void Mutate() // ����� �����������
    {

    }
}
