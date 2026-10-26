using System.Threading.Tasks;
using UnityEngine;

public class ButtonManager : MonoBehaviour
{
    public static ButtonManager Instance;
    //Заглушка, по-хорошому переписати під DI
    public TaskManager taskManager;
    public GameObject PingButton;
    public GameObject BackButton;
    public GameObject NextButton;

    private void Awake()
    {
        Instance = this; // Ініціалізація посилання на самого себе
    }

    void Start()
    {
        UpdateButtonsState();
    }

    public void UpdateButtonsState()
    {
        if (taskManager.currentTaskIndex <= 0)
        {
            BackButton.SetActive(false);
        }
        else
        {
            BackButton.SetActive(true);
        }

        if (taskManager.currentTaskIndex >= taskManager.tasks.Length)
        {
            NextButton.SetActive(false);
        }
        else
        {
            NextButton.SetActive(true);
        }

        if (taskManager.currentTaskIndex == taskManager.tasks.Length - 1)
        {
            PingButton.SetActive(true);
        }
        else
        {
            PingButton.SetActive(false);
        }
    }
}
