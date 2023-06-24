using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using System;

public class UIManager : MonoBehaviour
{

    [SerializeField] private TMPro.TextMeshProUGUI herbivoresText, predatorsText, scavangersText, plantsText;
    [SerializeField] private TMPro.TMP_InputField herbivoresInputField, predatorsInputField, scavangersInputField, plantsInputField;
    [SerializeField] private Button startButton, pouseButton, stopButton;
    private int numberOfHerbivores, numberOfPredators, numberOfScavangers, numberOfPlants;
    public static Action TheEndOfTheWorld; // Делегат
    [SerializeField] private GameObject panel; // Панель паузы
    bool isPaused = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SaveData() // Метод, сохраняющий все введенные данные
    {
        herbivoresText.text = "ХРЮ!!!";
    }

    public void Pause() // Метод, останавливающий все события
    {
        if (!isPaused)
        {
            panel.SetActive(true);
            isPaused = true;
            Time.timeScale = 0f;
        }
        else
        {
            panel.SetActive(false);
            isPaused = false;
            Time.timeScale = 1f;
        }
    }

    public void StopWasClicked()
    {
        TheEndOfTheWorld?.Invoke(); // Вызов события, если на него есть подписавшиеся методы
    }
}
