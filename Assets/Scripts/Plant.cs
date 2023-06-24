using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour
{
    int energy = 20; // �������, ������� ����� ������ ��� ��������

    private void OnEnable()
    {
        UIManager.TheEndOfTheWorld += Destroy; // �������� �� ������� TheEndOfTheWorld
    }

    private void OnDisable()
    {
        UIManager.TheEndOfTheWorld -= Destroy; // ������� �� ������� TheEndOfTheWorld
    }

    public int Die()
    {
        Destroy(gameObject);
        return energy;
    }

    private void Destroy()
    {
        Destroy(gameObject);
    }
}
