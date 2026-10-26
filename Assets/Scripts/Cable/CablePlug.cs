using UnityEngine;

public class CablePlug : MonoBehaviour
{
    [Header("Перетягни сюди об'єкт NewRope")]
    public GameObject ropeObject; // Посилання на об'єкт з LineRenderer і Rope скриптом

    [Header("Це початок чи кінець?")]
    public bool isStartPlug; // True для StartPlug, False для EndPlug

    [HideInInspector]
    public NetworkPort currentPort;
}
