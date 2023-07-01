using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour
{
    private static int energy; // Энергия, которая будет отдана при съедении

    private void Awake()
    {
        if (UIManager.Singleton.GetEnergy() != -1)
            energy = UIManager.Singleton.GetEnergy();
        else
            energy = 20;
    }
    private void OnEnable()
    {
        UIManager.TheEndOfTheWorld += Destroy; // Подписка на событие TheEndOfTheWorld
    }

    private void OnDisable()
    {
        UIManager.TheEndOfTheWorld -= Destroy; // Отписка от события TheEndOfTheWorld
    }

    public int Die()
    {
        Destroy(gameObject);
        Spawner.Singleton.plantList.Remove(transform.position);
        return energy;
    }

    private void Destroy()
    {
        Destroy(gameObject);
    }
}
