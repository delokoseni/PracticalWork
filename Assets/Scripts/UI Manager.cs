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
    public static UIManager Singleton {  get; private set; } // Паттерн синглтон (объект данного класса может быть лишь 1)
    [SerializeField] private TextMeshProUGUI infoText;
    [SerializeField] private TMP_InputField herbivoresInputField, predatorsInputField, scavengersInputField, plantsInputField;
    [SerializeField] private TMP_InputField speedInputField, startenergyInputField, sizeInputField, timeInputField;
    [SerializeField] private TMP_InputField chanseofmutationInputField;
    [SerializeField] private Button startButton, pouseButton, stopButton;
    [SerializeField] private GameObject panel, secondpanel; // Панель паузы
    [SerializeField] private GameObject infoPanel; // Панель информации
    private int numberOfHerbivores = 0, numberOfPredators = 0, numberOfScavengers = 0, numberOfPlants;
    private int startenergy = 100, chanseofmutation = 100;
    private float speed = 5f, size = 1f, time = 1f;
    public static Action TheEndOfTheWorld; // Делегат, событие
    public static Action StartOfTheWorld; // Делегат, событие
    bool isPaused = false; // Находится ли приложение на паузе
    bool dataIsOK = true; // Верно ли введены исходные данные
    bool isStarted = false; // Была ли нажата кнопка старт
    public bool wasTheEndOfTheWorld = false; // Была ли нажата кнопка стоп
    private float width;
    private float height;

    void Awake()
    {
        Singleton = this;
        width = 2*secondpanel.transform.localPosition.x;// / canvas.scaleFactor;
        height = Screen.height;// / canvas.scaleFactor;
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
                if (int.Parse(startenergyInputField.text) >= 1)
                {
                    startenergy = int.Parse(startenergyInputField.text);
                    dataIsOK = true;
                }
                else
                    dataIsOK = false;
                if (int.Parse(chanseofmutationInputField.text) >= 1)
                {
                    chanseofmutation = int.Parse(chanseofmutationInputField.text);
                    dataIsOK = true;
                }
                else
                    dataIsOK = false;
                if (float.Parse(speedInputField.text) >= 1)
                {
                    speed = float.Parse(speedInputField.text);
                    dataIsOK = true;
                }
                else
                    dataIsOK = false;
                if (float.Parse(sizeInputField.text) >= 1)
                {
                    size = float.Parse(sizeInputField.text);
                    dataIsOK = true;
                }
                else
                    dataIsOK = false;
                if (float.Parse(timeInputField.text) >= 0.1)
                {
                    time = float.Parse(timeInputField.text);
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
            wasTheEndOfTheWorld = false;
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
        wasTheEndOfTheWorld = true;
        TheEndOfTheWorld?.Invoke(); // Вызов события, если на него есть подписавшиеся методы
        InfoPanelClose();
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

    public float GetSpeed()
    {
        if (dataIsOK)
            return speed;
        else return -1f;
    }

    public float GetTime()
    {
        if (dataIsOK)
            return time;
        else return -1f;
    }

    public float GetSize()
    {
        if (dataIsOK)
            return size;
        else return -1f;
    }

    public int GetStartEnergy()
    {
        if (dataIsOK)
            return startenergy;
        else return -1;
    }

    public int GetChanseOfMutation()
    {
        if (dataIsOK)
            return chanseofmutation;
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

    public void Close()
    {
        Application.Quit();
    }

    public float GetWidth()
    {
        return width;
    }

    public float GetHeight()
    {
        return height;
    }
}
