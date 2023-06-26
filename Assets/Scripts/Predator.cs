using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Predator : Creature
{
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //SetData();
        //Move(targetPosition);
        //InvokeRepeating("Decreaseenergy", time, time); // Каждое time кол-во времени вызывается метод
        /*speed = UIManager.singleton.GetSpeed();
        size = UIManager.singleton.GetSize();
        time = UIManager.singleton.GetTime();
        startenergy = UIManager.singleton.GetStartEnergy();
        chanceOfMutation = UIManager.singleton.GetChanseOfMutation();
        Vector3 sizetoscale = new Vector3(size, size); // Вектор для присвоения размера
        transform.localScale = sizetoscale; // Присвоение стартового размера объекта класса
        energy = startenergy;
        transform.localScale = new Vector3(size, size, 0);
        targetPosition = new Vector2(UnityEngine.Random.Range(0f, Screen.width), UnityEngine.Random.Range(0f, Screen.height));
        Move(targetPosition);
        InvokeRepeating("Decreaseenergy", time, time); //Каждое time кол-во времени вызывается метод*/
    }

}
