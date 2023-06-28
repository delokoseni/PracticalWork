using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using System;
using Unity.Mathematics;
//using UnityEngine.Profiling.Memory.Experimental;

public class UIManager : MonoBehaviour
{
    public static UIManager Singleton {  get; private set; } // Паттерн синглтон (объект данного класса может быть лишь 1)
    [SerializeField] private TextMeshProUGUI infoText;
    [SerializeField] private TMP_InputField herbivoresInputField, predatorsInputField, scavengersInputField, plantsInputField;
    [SerializeField] private TMP_InputField speedInputField, startenergyInputField, sizeInputField, timeInputField;
    [SerializeField] private TMP_InputField chanseofmutationInputField, timeOfPlantsRespawnInputField, energyInputField;
    [SerializeField] private Button startButton, pouseButton, stopButton;
    [SerializeField] private GameObject panel, secondpanel; // Панель паузы и панель с полями ввода
    [SerializeField] private GameObject infoPanel; // Панель информации
    private int numberOfHerbivores = 0, numberOfPredators = 0, numberOfScavengers = 0, numberOfPlants = 0;
    private int startenergy = 100, chanseofmutation = 100, energy = 20;
    private float speed = 5f, size = 1f, time = 1f, timeOfPlantsRespawn = 20f;
    public static Action TheEndOfTheWorld; // Делегат, событие
    public static Action StartOfTheWorld; // Делегат, событие
    public static Action DataIsDone;
    private bool isPaused = false; // Находится ли приложение на паузе
    private bool dataIsOK; // Верно ли введены исходные данные
    private bool isStarted = false; // Была ли нажата кнопка старт
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
            dataIsOK = true;
            if (int.Parse(herbivoresInputField.text) > 0 ||
                int.Parse(predatorsInputField.text) > 0 ||
                int.Parse(scavengersInputField.text) > 0) {
                if (int.Parse(herbivoresInputField.text) >= 0 && herbivoresInputField.text.Length > 0)
                    numberOfHerbivores = int.Parse(herbivoresInputField.text);
                else
                {
                    dataIsOK = false;
                }
                if (int.Parse(predatorsInputField.text) >= 0 && predatorsInputField.text.Length > 0)
                    numberOfPredators = int.Parse(predatorsInputField.text);
                else
                    dataIsOK = false;
                if (int.Parse(scavengersInputField.text) >= 0 && scavengersInputField.text.Length > 0)
                    numberOfScavengers = int.Parse(scavengersInputField.text);
                else
                    dataIsOK = false;
                if (int.Parse(plantsInputField.text) >= 1 && plantsInputField.text.Length > 0)
                    numberOfPlants = int.Parse(plantsInputField.text);
                else
                    dataIsOK = false;
                if (int.Parse(startenergyInputField.text) >= 1 && startenergyInputField.text.Length > 0)
                    startenergy = int.Parse(startenergyInputField.text);
                else
                    dataIsOK = false;
                if (int.Parse(chanseofmutationInputField.text) >= 0 && int.Parse(chanseofmutationInputField.text) <= 100
                        && chanseofmutationInputField.text.Length > 0)
                    chanseofmutation = int.Parse(chanseofmutationInputField.text);
                else
                    dataIsOK = false;
                if (float.Parse(speedInputField.text) >= 1 && speedInputField.text.Length > 0)
                    speed = float.Parse(speedInputField.text);
                else
                    dataIsOK = false;
                if (float.Parse(sizeInputField.text) >= 1 && float.Parse(sizeInputField.text) <= 10 
                        && sizeInputField.text.Length > 0)
                    size = float.Parse(sizeInputField.text);
                else
                    dataIsOK = false;
                if (float.Parse(timeInputField.text) >= 0.1 && timeInputField.text.Length > 0) // Время траты 1 ед. энергии
                    time = float.Parse(timeInputField.text);
                else
                    dataIsOK = false;
                if (float.Parse(timeOfPlantsRespawnInputField.text) >= 1 && timeOfPlantsRespawnInputField.text.Length > 0)
                    timeOfPlantsRespawn = float.Parse(timeOfPlantsRespawnInputField.text);
                else
                    dataIsOK = false;

                if (int.Parse(energyInputField.text) >= 1 && int.Parse(energyInputField.text) <= 100 
                        && energyInputField.text.Length > 0)
                    energy = int.Parse(energyInputField.text);
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
        if (dataIsOK)
            DataIsDone?.Invoke();
    }

    public void Pause() // Метод, приостанавливающий все события
    {
        if (!isPaused && isStarted)
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
        numberOfHerbivores = 0;
        numberOfPredators = 0;
        numberOfScavengers = 0;
        numberOfPlants = 0;
        startenergy = 100;
        chanseofmutation = 100;
        energy = 20;
        speed = 5f;
        size = 1f;
        time = 1f;
        timeOfPlantsRespawn = 20f;
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

    public int GetEnergy()
    {
        if (dataIsOK)
            return energy;
        else return -1;
    }

    public float GetTimeOfPlantsRespawn()
    {
        if (dataIsOK)
            return timeOfPlantsRespawn;
        else return -1;
    }

    public float GetWidth()
    {
        return width;
    }

    public float GetHeight()
    {
        return height;
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

    public int IntCheckCorrect(TMP_InputField inputField, int min, int max, int number)
    {
        if (inputField.text.Length > 0 && int.Parse(inputField.text) >= min && int.Parse(inputField.text) <= max)
        {
            number = int.Parse(inputField.text);
            return 0;
        }
        else if(inputField.text.Length > 0)
        {
            dataIsOK = false;
            return 1;
        }
        return 0;
    }

    public int FloatCheckCorrect(TMP_InputField inputField, float min, float max, float number)
    {
        if (inputField.text.Length > 0 && float.Parse(inputField.text) >= min && float.Parse(inputField.text) <= max)
        {
            number = float.Parse(inputField.text);
            return 0;
        }
        else if (inputField.text.Length > 0)
        {
            dataIsOK = false;
            return 1;
        }
        return 0;
    }
}
