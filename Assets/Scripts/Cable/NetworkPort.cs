using GogoGaga.OptimizedRopesAndCables;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class NetworkPort : MonoBehaviour
{
    private LineRenderer connectedLineRenderer; // Тут ми збережемо посилання на лінію
    private bool isPluggedIntoStart; // Щоб знати напрямок
    private AudioSource clickSound;

    // Цей метод прив'яжи до події Select Entered на XR Socket Interactor
    public void OnCableConnected(SelectEnterEventArgs args)
    {
        // Отримуємо штекер, який в нас встромили
        var plug = args.interactableObject.transform.GetComponent<CablePlug>();

        clickSound = gameObject.GetComponent<AudioSource>();

        if (plug != null && plug.ropeObject != null)
        {
            // Отримуємо LineRenderer з об'єкта NewRope
            connectedLineRenderer = plug.ropeObject.GetComponent<LineRenderer>();
            isPluggedIntoStart = plug.isStartPlug;

            plug.currentPort = this;

            Debug.Log($"Порт {name} з'єднано з {plug.name}");

            Rope ropeScript = connectedLineRenderer.GetComponent<Rope>();
            if (ropeScript != null) {
                ropeScript.ropeLength = connectedLineRenderer.GetComponentInParent<DynamicCableLength>().CalculateLength();
            }

            clickSound.Play();

            //TaskManager.Instance.NextTask();
            if (ButtonManager.Instance != null)
            {
                ButtonManager.Instance.UpdateButtonsState();
            }
        }
    }

    public void OnCableDisconnected(SelectExitEventArgs args)
    {
        var plug = args.interactableObject.transform.GetComponent<CablePlug>();

        if (plug != null)
        {
            plug.currentPort = null;
        }

        connectedLineRenderer = null;

        Debug.Log("Порт відключено");
    }

    // Метод для отримання лінії (для пакетів)
    public LineRenderer GetCableLine()
    {
        return connectedLineRenderer;
    }

    // Повертає true, якщо в порті є кабель (LineRenderer не null)
    public bool IsConnected
    {
        get { return GetCableLine() != null; }
    }

    //Чи треба розвертати шлях пакету?
    public bool NeedReverse()
    {
        // Якщо ми встромилися у StartPlug, то пакет йде "туди" (не реверс)
        // Якщо в EndPlug — то пакет йде "назад" (реверс)
        return !isPluggedIntoStart;
    }
}