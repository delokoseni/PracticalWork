using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour
{
    int energy = 20; // �������, ������� ����� ������ ��� ��������

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int Die()
    {
        Destroy(gameObject);
        return energy;
    }
}
