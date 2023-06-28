using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Predator : Creature
{

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Creature")) 
        {
            Eat(collision.gameObject.GetComponent<Creature>());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Plant") && energy <= UIManager.Singleton.GetEnergy())
        {
            Eat(collision.gameObject.GetComponent<Plant>());
        }
    }

    public void Eat(Creature creature) // Метод питания
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
