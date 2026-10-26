using UnityEngine;
using TMPro; // Для роботи з текстом

public class PC_MonitorUI : MonoBehaviour
{
    [Header("UI Елементи")]
    public TMP_Text statusText; // Перетягни сюди текст екрану

    private void Start()
    {
        ShowIdle(); // При старті показуємо нейтральний статус
    }

    // Нейтральний стан
    public void ShowIdle()
    {
        if (statusText != null)
        {
            statusText.text = "Waiting...";
            statusText.color = Color.gray;
        }
    }

    // Відправка
    public void ShowSent()
    {
        if (statusText != null)
        {
            statusText.text = "Host sent ping";
            statusText.color = Color.white; // Або світло-блакитний, як на скріні
        }
    }

    // Успішне отримання
    public void ShowAccepted()
    {
        if (statusText != null)
        {
            statusText.text = "Host accepts ping";
            statusText.color = Color.green;
        }
    }

    // Відхилення (не той IP)
    public void ShowRejected()
    {
        if (statusText != null)
        {
            statusText.text = "Host rejects ping";
            statusText.color = Color.red;
        }
    }
}