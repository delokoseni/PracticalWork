using UnityEngine;

public class Carrion : MonoBehaviour
{
    private static int energy; // �������, ������� ����� ������ ��� ��������
    private void Update() // �����, ������� ���������� ������ ����
    {
        if (UIManager.Singleton.wasTheEndOfTheWorld)
            Destroy();
    }
    private void Awake() // �����, ������� ���������� ������ 1 ���
    {
        if (UIManager.Singleton.GetEnergy() != -1)
            energy = UIManager.Singleton.GetEnergy();
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
        Spawner.Singleton.carrionList.Remove(transform.position);
        Destroy(gameObject);
    }
}
