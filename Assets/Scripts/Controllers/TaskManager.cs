using UnityEngine;
using TMPro;

public class TaskManager : MonoBehaviour
{
    public static TaskManager Instance; // Робимо Сінглтон, щоб звертатися звідусіль

    [Header("UI Елементи")]
    public TMP_Text headerText;
    public TMP_Text bodyText;
    public AudioSource successSound; // Звук успішного завершення завдання

    [Header("Список завдань")]
    public Task[] tasks; // Масив наших завдань

    [HideInInspector]
    public int currentTaskIndex = 0;

    private void Awake()
    {
        Instance = this; // Ініціалізація посилання на самого себе
    }

    private void Start()
    {
        UpdateUI(); // Показуємо перше завдання зі старту
    }

    // Цей метод ми будемо викликати, коли гравець щось зробив правильно
    public void NextTask()
    {
        if (currentTaskIndex < tasks.Length - 1)
        {
            currentTaskIndex++;
            UpdateUI();

            if (successSound) successSound.Play();
            Debug.Log("Завдання виконано! Переходимо до наступного.");
        }
        else
        {
            currentTaskIndex++;
            headerText.text = "Вітаємо!";
            bodyText.text = "Лабораторна робота завершена успішно.";
            if (successSound) successSound.Play();
        }

        if (ButtonManager.Instance != null)
        {
            ButtonManager.Instance.UpdateButtonsState();
        }
    }

    public void PreviousTask()
    {
        if (currentTaskIndex <= tasks.Length - 1 && currentTaskIndex > 0)
        {
            currentTaskIndex--;
            UpdateUI();

            Debug.Log("Повертаємось до попереднього завдання.");
        }

        if (ButtonManager.Instance != null)
        {
            ButtonManager.Instance.UpdateButtonsState();
        }
    }

    void UpdateUI()
    {
        if (tasks.Length > 0 && currentTaskIndex < tasks.Length)
        {
            headerText.text = tasks[currentTaskIndex].title;
            bodyText.text = tasks[currentTaskIndex].description;
        }
    }
}

// Простий клас для зберігання даних про завдання
[System.Serializable]
public class Task
{
    public string title;
    [TextArea] public string description;
}
