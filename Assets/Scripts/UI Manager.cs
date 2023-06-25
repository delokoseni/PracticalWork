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
    [SerializeField] private Button startButton, pouseButton, stopButton, closeInfoButton;
    private int numberOfHerbivores = 0, numberOfPredators = 0, numberOfScavengers = 0, numberOfPlants;
    public static Action TheEndOfTheWorld; // Делегат, событие
    public static Action StartOfTheWorld; // Делегат, событие
    [SerializeField] private GameObject panel; // Панель паузы
    [SerializeField] private GameObject infoPanel; // Панель информации

    bool isPaused = false; // Находится ли приложение на паузе
    bool dataIsOK = true; // Верно ли введены исходные данные
    bool isStarted = false; // Была ли нажата кнопка старт

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
                    dataIsOK = true;
                }
                else
                    dataIsOK = false;
                if (int.Parse(predatorsInputField.text) >= 0)
                {
                    numberOfPredators = int.Parse(predatorsInputField.text);
                    dataIsOK = true;
                }
                else
                    dataIsOK = false;
                if (int.Parse(scavengersInputField.text) >= 0)
                {
                    numberOfScavengers = int.Parse(scavengersInputField.text);
                    dataIsOK = true;
                }
                else
                    dataIsOK = false;
                if (int.Parse(plantsInputField.text) >= 1)
                {
                    numberOfPlants = int.Parse(plantsInputField.text);
                    dataIsOK = true;
                }
                else
                    dataIsOK = false;
            }
            else
                dataIsOK = false;
        }
        catch{ }
        if (!isStarted)
        {
            StartOfTheWorld?.Invoke(); // Вызов события, если на него есть подписавшиеся методы
            isStarted = true;
        }
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
        isStarted = false;
        TheEndOfTheWorld?.Invoke(); // Вызов события, если на него есть подписавшиеся методы
    }

    public int GetnumberOfHerbivores()
    {
        if(dataIsOK)
            return numberOfHerbivores;
        else return -1;
    }

    public int GetnumberOfPredators()
    {
        if (dataIsOK)
            return numberOfPredators;
        else return -1;
    }

    public int GetnumberOfScavengers()
    {
        if (dataIsOK)
            return numberOfScavengers;
        else return -1;
    }

    public int GetnumberOfPlants()
    {
        if (dataIsOK)
            return numberOfPlants;
        else return -1;
    }

    private void InfoPanelShow(string str)
    {
        infoPanel.SetActive(true);
        infoText.text = str;
    }

    public void InfoPanelClose()
    {
        infoPanel.SetActive(false);
    }
}
