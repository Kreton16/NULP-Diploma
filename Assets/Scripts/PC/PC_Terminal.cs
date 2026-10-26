using UnityEngine;

public class PC_Terminal : MonoBehaviour
{
    public GameObject packetPrefab; // Твій префаб кульки/пакета

    [Header("Монітор цього ПК")]
    public PC_MonitorUI myMonitor; // Сюди перетягнемо скрипт екрану
    // Виклич цей метод кнопкою або клавішею
    public void SendPing()
    {
        NetworkDevice myDevice = GetComponent<NetworkDevice>();
        NetworkPort activePort = null;

        foreach (var port in myDevice.myPorts)
        {
            if (port.IsConnected)
            {
                activePort = port;
                break; // Знайшли перший-ліпший підключений порт і зупиняємо пошук
            }
        }

        if (activePort != null)
        {
            Debug.Log($"Відправляю пінг через порт: {activePort.name}");

            PacketData data = new PacketData();
            data.sourceIP = "192.168.1.1";
            data.destinationIP = "192.168.1.2";
            data.message = "Ping";

            LineRenderer cable = activePort.GetCableLine();

            myDevice.SendPacketPhysical(data, cable, activePort.NeedReverse());
            
            if (myMonitor != null)
            {
                myMonitor.ShowSent();
            }
        }
        else
        {
            Debug.LogWarning("Помилка: Жоден порт не підключено! Вставте кабель.");
            // Тут пізніше можна додати звук помилки або червоний напис на екрані
        }
    }
}