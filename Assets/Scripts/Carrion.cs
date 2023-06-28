using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carrion : MonoBehaviour
{
    private int energy = 20; // �������, ������� ����� ������ ��� ��������

    private void Update()
    {
        if (UIManager.Singleton.wasTheEndOfTheWorld)
            Destroy();
    }
    private void OnEnable()
    {
        UIManager.TheEndOfTheWorld += Destroy; // �������� �� ������� TheEndOfTheWorld
    }

    private void OnDisable()
    {
        UIManager.TheEndOfTheWorld -= Destroy; // ������� �� ������� TheEndOfTheWorld
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
