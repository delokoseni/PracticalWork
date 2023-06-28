using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carrion : MonoBehaviour
{
    private static int energy; // Энергия, которая будет отдана при съедении

    private void Update()
    {
        if (UIManager.Singleton.wasTheEndOfTheWorld)
            Destroy();
        else
            energy = 20;
    }

    private void Awake()
    {
        if (UIManager.Singleton.GetEnergy() != -1)
            energy = UIManager.Singleton.GetEnergy();
    }

    private void OnEnable()
    {
        UIManager.TheEndOfTheWorld += Destroy; // Подписка на событие TheEndOfTheWorld
    }

    private void OnDisable()
    {
        UIManager.TheEndOfTheWorld -= Destroy; // Отписка от события TheEndOfTheWorld
    }

    public int Eaten()
    {
        Spawner.Singleton.carrionList.Remove(transform.position);
        Destroy(gameObject);
        return energy;
    }

    private void Destroy()
    {
        Destroy(gameObject);
    }
}
