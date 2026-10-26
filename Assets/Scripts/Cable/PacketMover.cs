using UnityEngine;
using System.Collections;

public class PacketMover : MonoBehaviour
{
    public float speed = 2.0f; // Швидкість руху пакета
    private Vector3[] pathPoints; // Точки, по яких будемо рухатись
    private int currentPointIndex = 0;
    private bool isMoving = false;

    private PacketData myData;       // Дані, які несе пакет
    private NetworkDevice senderDevice; // Хто створив цей пакет (щоб знати контекст)

    private CablePlug targetPlug;

    public void Initialize(PacketData data, NetworkDevice sender, CablePlug destination)
    {
        myData = data;
        senderDevice = sender;
        targetPlug = destination;
    }

    // Цей метод ми викличемо ззовні, коли спавнимо пакет
    public void StartTransmission(LineRenderer cablePath, bool needReverse)
    {
        // 1. Отримуємо всі точки з LineRenderer кабелю
        int pointCount = cablePath.positionCount;
        pathPoints = new Vector3[pointCount];
        cablePath.GetPositions(pathPoints);

        if (needReverse)
        {
            System.Array.Reverse(pathPoints);
        }

        // 2. Ставимо пакет на початок
        if (pathPoints.Length > 0)
        {
            transform.position = pathPoints[0];
            currentPointIndex = 1; // Починаємо рух до наступної точки
            isMoving = true;
        }
    }

    void Update()
    {
        if (!isMoving || pathPoints == null || pathPoints.Length == 0) return;

        // Рухаємось до поточної цільової точки
        Vector3 targetPosition = pathPoints[currentPointIndex];
        float step = speed * Time.deltaTime;

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);

        // Перевіряємо, чи дійшли ми до точки
        if (Vector3.Distance(transform.position, targetPosition) < 0.001f)
        {
            currentPointIndex++; // Переходимо до наступної точки

            // Якщо точки закінчилися — пакет дійшов
            if (currentPointIndex >= pathPoints.Length)
            {
                OnPacketArrived();
            }
        }
    }

    void OnPacketArrived()
    {
        isMoving = false;
        Debug.Log("Пакет успішно доставлено!");
        Debug.Log(targetPlug);
        Debug.Log(targetPlug.currentPort);

        if (targetPlug != null && targetPlug.currentPort != null)
        {
            // Отримуємо порт прямо зі штекера
            NetworkPort arrivalPort = targetPlug.currentPort;

            // Отримуємо пристрій
            NetworkDevice receiver = arrivalPort.GetComponentInParent<NetworkDevice>();

            if (receiver != null)
            {
                receiver.OnPacketReceived(myData, arrivalPort);
            }
        }
        else
        {
            Debug.Log("Пакет дійшов до кінця кабелю, але він нікуди не підключений!");
        }

        Destroy(gameObject);
    }
}
