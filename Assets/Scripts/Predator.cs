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
    public void Eat(Creature creature) // Метод питания
    {
        if (size >= creature.GetSize()) {
            System.Random rand = new();
            if (rand.Next(0, 2) == 1)
            {
                int receivedenergy = UIManager.Singleton.GetEnergy();
                creature.Die();
                energy += receivedenergy;
                if (energy > startenergy)
                    energy = startenergy;
                if (energy == 100)
                {
                    Debug.Log("что-то съел");
                    Multiply();
                }
            }
        }
    }

}
