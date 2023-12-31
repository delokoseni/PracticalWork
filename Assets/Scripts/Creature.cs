using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Creature : MonoBehaviour
{
    public Rigidbody2D rb; // �����, ����������� ������ ������� 
    Vector2 targetPosition; // �������, � ������� �������� ������ ������
    bool isMoving = false; // ��������� ��� ��������, ��������� �� ������ ������ � ��������
    float speed = 5f; // �������� 
    int startenergy = 100; // ��������� ������� 
    int energy; // ������� ������� 
    float size = 1; // ������ 
    float time = 1f; // �����, �� ������� ����������� 1 ������� �������
    int chanceOfMutation = 100; // ����, � ������� ������� ������ ����������
    Color32 color; // ���� ��������
    public GameObject creaturePrefab; // 
    public static Action<string> WasClicked;

    private void Start() // �����, ���������� ��� ��������������� ������� �����
    {
        rb = GetComponent<Rigidbody2D>(); // �������� ������ �� ��������� ������ ������
        Vector3 sizetoscale = new Vector3(size, size); // ������ ��� ���������� �������
        transform.localScale = sizetoscale; // ���������� ���������� ������� ������� ������
        energy = startenergy;
        transform.localScale = new Vector3(size, size, 0);
        targetPosition = new Vector2(UnityEngine.Random.Range(0f, Screen.width), UnityEngine.Random.Range(0f, Screen.height));
        Move(targetPosition);
        InvokeRepeating("Decreaseenergy", time, time); //������ time ���-�� ������� ���������� �����
    }

    private void FixedUpdate() //
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
        else
        {
            Vector2 newtargetPosition = new Vector2(UnityEngine.Random.Range(0f, Screen.width), UnityEngine.Random.Range(0f, Screen.height));
            targetPosition = newtargetPosition;
            Move(newtargetPosition);
        }
    }

    // �����, ����������� �� �������� � ������� ���������
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall")) // ������� ��������� ���������� � ������
        {
            Vector2 newtargetPosition = new Vector2(UnityEngine.Random.Range(0f, Screen.width), 
                UnityEngine.Random.Range(0f, Screen.height));
            targetPosition = newtargetPosition;
            Move(newtargetPosition);
        }
        if (collision.gameObject.CompareTag("Creature")) // ������� ��������� �������� ����� �����
        {
            Vector2 newtargetPosition = new Vector2(UnityEngine.Random.Range(0f, Screen.width), 
                UnityEngine.Random.Range(0f, Screen.height));
            targetPosition = newtargetPosition;
            Move(newtargetPosition);
        }
        if (collision.gameObject.CompareTag("Plant")) 
        {
            Eat(collision.gameObject.GetComponent<Plant>());
        }
    }

    private void OnEnable()
    {
        UIManager.TheEndOfTheWorld += Die; // �������� �� ������� TheEndOfTheWorld
    }

    private void OnDisable()
    {
        UIManager.TheEndOfTheWorld -= Die; // ������� �� ������� TheEndOfTheWorld
    }

    public void Move(Vector2 newPosition) // ����� ������������
    {
        targetPosition = Camera.main.ScreenToWorldPoint(newPosition);
        isMoving = true;
    }

    public void Decreaseenergy() // ����� ������ �������
    {
        energy--;
        if (energy == 0)
        {
            Die();
        }
    }

    public void Die() // ����� ������
    {
        Destroy(gameObject);
    }

    public void Eat(Plant plant) // ����� �������
    {
        int receivedenergy = plant.Die();
        energy += receivedenergy;
        if (energy > startenergy)
            energy = startenergy;
        if (energy == 100)
            Multiply();
    }

    public void Multiply() // ����� �����������
    {
        Vector3 spawnPosition = transform.position;
        GameObject newcreature = Instantiate(creaturePrefab, spawnPosition, Quaternion.identity);
        Mutate(newcreature.GetComponent<Creature>());
    }

    public void Mutate(Creature newcreature) // ����� �����������
    {
        System.Random rand = new System.Random();
        if(rand.Next(1,101) <= chanceOfMutation)
        {
            int n = rand.Next(5);
            switch (n)
            {
                case 0:
                    n = rand.Next(2);
                    if (n == 1)
                    {
                        newcreature.speed++;
                        newcreature.GetComponent<SpriteRenderer>().color -= new Color32(0, 0, 5, 0);
                    }
                    else
                    {
                        newcreature.speed--;
                        newcreature.GetComponent<SpriteRenderer>().color += new Color32(0, 0, 5, 0);
                    }
                    break;
                case 1:
                    n = rand.Next(2);
                    if (n == 1)
                    {
                        newcreature.startenergy++;
                        newcreature.GetComponent<SpriteRenderer>().color -= new Color32(0, 5, 0, 0);
                    }
                    else
                    {
                        newcreature.startenergy--;
                        newcreature.GetComponent<SpriteRenderer>().color += new Color32(0, 5, 0, 0);
                    }
                    break;
                case 2:
                    n = rand.Next(2);
                    if (n == 1)
                    {
                        newcreature.size++;
                        newcreature.GetComponent<SpriteRenderer>().color -= new Color32(0, 5, 5, 0);
                    }
                    else
                    {
                        newcreature.size--;
                        newcreature.GetComponent<SpriteRenderer>().color += new Color32(0, 5, 5, 0);
                    }
                    break;
                case 3:
                    n = rand.Next(2);
                    if (n == 1)
                    {
                        newcreature.time++;
                        newcreature.GetComponent<SpriteRenderer>().color -= new Color32(5, 0, 0, 0);
                    }
                    else
                    {
                        newcreature.time--;
                        newcreature.GetComponent<SpriteRenderer>().color += new Color32(5, 0, 0, 0);
                    }
                    break;
                case 4:
                    n = rand.Next(2);
                    if (n == 1)
                    {
                        newcreature.chanceOfMutation++;
                        newcreature.GetComponent<SpriteRenderer>().color -= new Color32(5, 0, 5, 0);
                    }
                    else
                    {
                        newcreature.chanceOfMutation--;
                        newcreature.GetComponent<SpriteRenderer>().color += new Color32(5, 0, 5, 0);
                    }
                    break;
            }
        }
    }

    public void OnMouseDown()
    {
        string str = "��������: " + speed + "\n������: " + size + "\n��������� �������" + startenergy +
            "\n�����, �� ������� ����������� 1 ��. �������: " + time + "\n���� ������� �������: " +
            chanceOfMutation + "%";
        WasClicked?.Invoke(str);
    }
}
