using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using System;
using Unity.Mathematics;
using UnityEngine.Profiling.Memory.Experimental;

public class UIManager : MonoBehaviour
{
    public static UIManager singleton {  get; private set; }
    [SerializeField] private TextMeshProUGUI infoText;
    [SerializeField] private TMP_InputField herbivoresInputField, predatorsInputField, scavengersInputField, plantsInputField;
    [SerializeField] private Button startButton, pouseButton, stopButton;
    private int numberOfHerbivores = 0, numberOfPredators = 0, numberOfScavengers = 0, numberOfPlants;
    public static Action TheEndOfTheWorld; // Делегат, событие
    public static Action StartOfTheWorld; // Делегат, событие
    [SerializeField] private GameObject panel; // Панель паузы
    [SerializeField] private GameObject infoPanel; // Панель информации
    bool isPaused = false; // Находится ли приложение на паузе
    bool dateIsOK = true; // Верно ли введены исходные данные

    void Awake()
    {
        singleton = this;
    }

    private void OnEnable()
    {
        Creature.WasClicked += InfoPanelShow; // Подписка на событие TheEndOfTheWorld
    }

    public void SaveData() // Метод, сохраняющий все введенные данные
    {
        try
        {
            if (int.Parse(herbivoresInputField.text) >= 1 ||
                int.Parse(predatorsInputField.text) >= 1 ||
                int.Parse(scavengersInputField.text) >= 1) {
                if (int.Parse(herbivoresInputField.text) >= 0)
                {
                    numberOfHerbivores = int.Parse(herbivoresInputField.text);
                    dateIsOK = true;
                }
                else
                    dateIsOK = false;
                if (int.Parse(predatorsInputField.text) >= 0)
                {
                    numberOfPredators = int.Parse(predatorsInputField.text);
                    dateIsOK = true;
                }
                else
                    dateIsOK = false;
                if (int.Parse(scavengersInputField.text) >= 0)
                {
                    numberOfScavengers = int.Parse(scavengersInputField.text);
                    dateIsOK = true;
                }
                else
                    dateIsOK = false;
                if (int.Parse(plantsInputField.text) >= 1)
                {
                    numberOfPlants = int.Parse(plantsInputField.text);
                    dateIsOK = true;
                }
                else
                    dateIsOK = false;
            }
            else
            {
                dateIsOK = false;
            }
        }
        catch{ }
        StartOfTheWorld?.Invoke(); // Вызов события, если на него есть подписавшиеся методы
    }

    public void Pause() // Метод, приостанавливающий все события
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

    public void StopWasClicked() //Метод, вызываемый при нажатии кнопки Stop
    {
        TheEndOfTheWorld?.Invoke(); // Вызов события, если на него есть подписавшиеся методы
    }

    public int GetnumberOfHerbivores()
    {
        if(dateIsOK)
            return numberOfHerbivores;
        else return -1;
    }

    public int GetnumberOfPredators()
    {
        if (dateIsOK)
            return numberOfPredators;
        else return -1;
    }

    public int GetnumberOfScavengers()
    {
        if (dateIsOK)
            return numberOfScavengers;
        else return -1;
    }

    public int GetnumberOfPlants()
    {
        if (dateIsOK)
            return numberOfPlants;
        else return -1;
    }

    private void InfoPanelShow(string str)
    {
        infoPanel.SetActive(true);
        infoText.text = str;
    }
}
