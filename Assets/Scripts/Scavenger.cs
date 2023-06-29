using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scavenger : Creature
{

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

}
