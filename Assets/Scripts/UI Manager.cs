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
    [SerializeField] private TextMeshProUGUI infoText, herbivoresText, predatorsText, scavengersText, plantsText;
    [SerializeField] private TextMeshProUGUI speedText, startenergyText, sizeText, timeText, chanseofmutationText;
    [SerializeField] private TextMeshProUGUI timeOfPlantsRespawnText, energyText;
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
    private readonly List<string> normalTexts = new() { "Введите количество...", "Введите количество...",
                                                "Введите количество...", "Введите количество..." };
    private readonly List<string> errorTexts = new() { "0 < N <= 40", "0 < N <= 40", "0 <= N <= 40", "0 < N <= 100" };

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
        dataIsOK = true;
        try
        {
            if (herbivoresInputField.text.Length != 0)
            {
                if (int.Parse(herbivoresInputField.text) > 0 && int.Parse(herbivoresInputField.text) <= 10)
                {
                    herbivoresInputField.placeholder.GetComponent<TextMeshProUGUI>().text = "Введите количество...";
                    herbivoresText.GetComponent<TextMeshProUGUI>().color = Color.white;
                    numberOfHerbivores = int.Parse(herbivoresInputField.text);
                }
                else
                {
                    herbivoresInputField.text = "";
                    herbivoresInputField.placeholder.GetComponent<TextMeshProUGUI>().text = "1 <= N <= 10";
                    herbivoresText.GetComponent<TextMeshProUGUI>().color = Color.red;
                    dataIsOK = false;
                }
            }
            else
            {
                herbivoresInputField.placeholder.GetComponent<TextMeshProUGUI>().text = "1 <= N <= 10";
                herbivoresText.GetComponent<TextMeshProUGUI>().color = Color.red;
                dataIsOK = false;
            }
            if (predatorsInputField.text.Length != 0)
            {
                if (int.Parse(predatorsInputField.text) > 0 && int.Parse(predatorsInputField.text) <= 10)
                {
                    predatorsInputField.placeholder.GetComponent<TextMeshProUGUI>().text = "Введите количество...";
                    predatorsText.GetComponent<TextMeshProUGUI>().color = Color.white;
                    numberOfPredators = int.Parse(predatorsInputField.text);
                }
                else
                {
                    predatorsInputField.text = "";
                    predatorsInputField.placeholder.GetComponent<TextMeshProUGUI>().text = "1 <= N <= 10";
                    predatorsText.GetComponent<TextMeshProUGUI>().color = Color.red;
                    dataIsOK = false;
                }
            }
            else
            {
                predatorsInputField.placeholder.GetComponent<TextMeshProUGUI>().text = "1 <= N <= 10";
                predatorsText.GetComponent<TextMeshProUGUI>().color = Color.red;
                dataIsOK = false;
            }
            if (scavengersInputField.text.Length != 0)
            {
                if (int.Parse(scavengersInputField.text) > 0 && int.Parse(scavengersInputField.text) <= 10)
                {
                    scavengersInputField.placeholder.GetComponent<TextMeshProUGUI>().text = "Введите количество...";
                    scavengersText.GetComponent<TextMeshProUGUI>().color = Color.white;
                    numberOfScavengers = int.Parse(scavengersInputField.text);
                }
                else
                {
                    scavengersInputField.text = "";
                    scavengersInputField.placeholder.GetComponent<TextMeshProUGUI>().text = "0 < N <= 10";
                    scavengersText.GetComponent<TextMeshProUGUI>().color = Color.red;
                    dataIsOK = false;
                }
            }
            else
            {
                scavengersInputField.placeholder.GetComponent<TextMeshProUGUI>().text = "0 < N <= 10";
                scavengersText.GetComponent<TextMeshProUGUI>().color = Color.red;
                dataIsOK = false;
            }
            if (plantsInputField.text.Length != 0)
            {
                if (int.Parse(plantsInputField.text) >= 1 && int.Parse(plantsInputField.text) <= 100)
                {
                    plantsInputField.placeholder.GetComponent<TextMeshProUGUI>().text = "Введите количество...";
                    plantsText.GetComponent<TextMeshProUGUI>().color = Color.white;
                    numberOfPlants = int.Parse(plantsInputField.text);
                }
                else
                {
                    plantsInputField.text = "";
                    plantsInputField.placeholder.GetComponent<TextMeshProUGUI>().text = "0 < N <= 100";
                    plantsText.GetComponent<TextMeshProUGUI>().color = Color.red;
                    dataIsOK = false;
                }
            }
            else
            {
                plantsInputField.placeholder.GetComponent<TextMeshProUGUI>().text = "0 < N <= 100";
                plantsText.GetComponent<TextMeshProUGUI>().color = Color.red;
                dataIsOK = false;
            }
            if (startenergyInputField.text.Length != 0)
            {
                if (int.Parse(startenergyInputField.text) >= 1 && int.Parse(startenergyInputField.text) <= 1000)
                {
                    startenergyInputField.placeholder.GetComponent<TextMeshProUGUI>().text = "Введите количество...";
                    startenergyText.GetComponent<TextMeshProUGUI>().color = Color.white;
                    startenergy = int.Parse(startenergyInputField.text);
                }
                else
                {
                    startenergyInputField.text = "";
                    startenergyInputField.placeholder.GetComponent<TextMeshProUGUI>().text = "1 <= N <= 1000";
                    startenergyText.GetComponent<TextMeshProUGUI>().color = Color.red;
                    dataIsOK = false;
                }
            }
            if (chanseofmutationInputField.text.Length != 0)
            {
                if (int.Parse(chanseofmutationInputField.text) >= 0 && int.Parse(chanseofmutationInputField.text) <= 100)
                {
                    chanseofmutationInputField.placeholder.GetComponent<TextMeshProUGUI>().text = "Введите количество %...";
                    chanseofmutationText.GetComponent<TextMeshProUGUI>().color = Color.white;
                    chanseofmutation = int.Parse(chanseofmutationInputField.text);
                }
                else
                {
                    chanseofmutationInputField.text = "";
                    chanseofmutationInputField.placeholder.GetComponent<TextMeshProUGUI>().text = "0 <= N <= 100";
                    chanseofmutationText.GetComponent<TextMeshProUGUI>().color = Color.red;
                    dataIsOK = false;
                }
            }
            if (speedInputField.text.Length != 0)
            {
                if (float.Parse(speedInputField.text) >= 1 && float.Parse(speedInputField.text) <= 20)
                {
                    speedInputField.placeholder.GetComponent<TextMeshProUGUI>().text = "Введите скорость...";
                    speedText.GetComponent<TextMeshProUGUI>().color = Color.white;
                    speed = float.Parse(speedInputField.text);
                }
                else
                {
                    speedInputField.text = "";
                    speedInputField.placeholder.GetComponent<TextMeshProUGUI>().text = "1 <= N <= 20";
                    speedText.GetComponent<TextMeshProUGUI>().color = Color.red;
                    dataIsOK = false;
                }
            }
            if (sizeInputField.text.Length != 0)
            {
                if (float.Parse(sizeInputField.text) >= 1 && float.Parse(sizeInputField.text) <= 3)
                {
                    sizeInputField.placeholder.GetComponent<TextMeshProUGUI>().text = "Введите скорость...";
                    sizeText.GetComponent<TextMeshProUGUI>().color = Color.white;
                    size = float.Parse(sizeInputField.text);
                }
                else
                {
                    sizeInputField.text = "";
                    sizeInputField.placeholder.GetComponent<TextMeshProUGUI>().text = "1 <= N <= 3";
                    sizeText.GetComponent<TextMeshProUGUI>().color = Color.red;
                    dataIsOK = false;
                }
            }
            if (timeInputField.text.Length != 0)
            {
                if (float.Parse(timeInputField.text) >= 0.1f && float.Parse(timeInputField.text) <= 10f)
                {
                    timeInputField.placeholder.GetComponent<TextMeshProUGUI>().text = "Введите время...";
                    timeText.GetComponent<TextMeshProUGUI>().color = Color.white;
                    time = float.Parse(timeInputField.text);
                }
                else
                {
                    timeInputField.text = "";
                    timeInputField.placeholder.GetComponent<TextMeshProUGUI>().text = "0.1 <= N <= 10";
                    timeText.GetComponent<TextMeshProUGUI>().color = Color.red;
                    dataIsOK = false;
                }
            }
            if (timeOfPlantsRespawnInputField.text.Length != 0)
            {
                if (float.Parse(timeOfPlantsRespawnInputField.text) >= 1f && float.Parse(timeOfPlantsRespawnInputField.text) <= 100f)
                {
                    timeOfPlantsRespawnInputField.placeholder.GetComponent<TextMeshProUGUI>().text = "Введите время...";
                    timeOfPlantsRespawnText.GetComponent<TextMeshProUGUI>().color = Color.white;
                    timeOfPlantsRespawn = float.Parse(timeOfPlantsRespawnInputField.text);
                }
                else
                {
                    timeOfPlantsRespawnInputField.text = "";
                    timeOfPlantsRespawnInputField.placeholder.GetComponent<TextMeshProUGUI>().text = "1 <= N <= 100";
                    timeOfPlantsRespawnText.GetComponent<TextMeshProUGUI>().color = Color.red;
                    dataIsOK = false;
                }
            }
            if (energyInputField.text.Length != 0)
            {
                if (int.Parse(energyInputField.text) >= 1 && int.Parse(energyInputField.text) <= 100)
                {
                    energyInputField.placeholder.GetComponent<TextMeshProUGUI>().text = "Введите количество...";
                    energyText.GetComponent<TextMeshProUGUI>().color = Color.white;
                    energy = int.Parse(energyInputField.text);
                }
                else
                {
                    energyInputField.text = "";
                    energyInputField.placeholder.GetComponent<TextMeshProUGUI>().text = "1 <= N <= 100";
                    energyText.GetComponent<TextMeshProUGUI>().color = Color.red;
                    dataIsOK = false;
                }
            }
        }
        catch{ }
        if (!isStarted && dataIsOK)
        {
            isStarted = true;
            StartOfTheWorld?.Invoke(); // Вызов события, если на него есть подписавшиеся методы
            wasTheEndOfTheWorld = false;
        }
        if (dataIsOK && wasTheEndOfTheWorld)
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
        Spawner.Singleton.plantList.Clear();
        Spawner.Singleton.herbivoreList.Clear();
        Spawner.Singleton.predatorList.Clear();
        Spawner.Singleton.scavengerList.Clear();
        Spawner.Singleton.carrionList.Clear();
        InfoPanelClose();
        isPaused = false;
        panel.SetActive(false);
        Time.timeScale = 1f;
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
    public void IntCheckCorrect(TMP_InputField inputField, TextMeshProUGUI someText, int min, int max, int number, int i)
    {
        if (int.Parse(scavengersInputField.text) >= min && int.Parse(scavengersInputField.text) <= max)
        {
            inputField.placeholder.GetComponent<TextMeshProUGUI>().text = normalTexts[i];
            someText.GetComponent<TextMeshProUGUI>().color = Color.white;
            number = int.Parse(inputField.text);
        }
        else
        {
            inputField.text = "";
            inputField.placeholder.GetComponent<TextMeshProUGUI>().text = errorTexts[i];
            someText.GetComponent<TextMeshProUGUI>().color = Color.red;
            dataIsOK = false;
        }
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
