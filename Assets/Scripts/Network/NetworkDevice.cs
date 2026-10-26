using UnityEngine;
using System.Collections.Generic;
using GogoGaga.OptimizedRopesAndCables;

public class NetworkDevice : MonoBehaviour
{
    [Header("Налаштування")]
    public string ipAddress;       // IP цього пристрою (для Switch можна залишити пустим)
    public bool isSwitch;          // Галочка: це комутатор чи кінцевий пристрій?

    [Header("Підключення")]
    public List<NetworkPort> myPorts; // Список всіх портів цього пристрою
    public GameObject packetPrefab;   // Префаб пакета для відправки далі

    [Header("Монітор (тільки для ПК)")]
    public PC_MonitorUI monitor;

    // Цей метод викликається, коли пакет фізично долітає до порту цього пристрою
    public void OnPacketReceived(PacketData data, NetworkPort incomingPort)
    {
        Debug.Log($"{name} отримав пакет від {data.sourceIP} для {data.destinationIP}");

        if (isSwitch)
        {
            // === ЛОГІКА КОМУТАТОРА ===
            // Світч просто пересилає пакет на всі інші порти (Flood) 
            // або на конкретний (якщо ми реалізуємо MAC-таблицю, але для початку Flood - ок)
            StartCoroutine(ForwardPacketRoutine(data, incomingPort));
        }
        else
        {
            // === ЛОГІКА ПК ===
            if (data.destinationIP == ipAddress)
            {
                Debug.Log($"<color=green>УСПІХ! {name} отримав пінг!</color>");
                // Тут можна запустити анімацію "Ping Reply" назад
                if (monitor != null)
                {
                    monitor.ShowAccepted();
                }
            }
            else
            {
                Debug.Log($"{name}: Цей пакет не мені. Відкидаю.");
                if (monitor != null)
                {
                    monitor.ShowRejected();
                }
            }
        }
    }

    // Затримка перед пересилкою (імітація обробки)
    private System.Collections.IEnumerator ForwardPacketRoutine(PacketData data, NetworkPort excludePort)
    {
        yield return new WaitForSeconds(0.5f); // Світч "думає"

        foreach (var port in myPorts)
        {
            Debug.Log(excludePort);
            // Не відправляємо пакет назад у той порт, звідки він прийшов!
            if (port == excludePort) continue;

            // Перевіряємо, чи є кабель у цьому порту
            LineRenderer cable = port.GetCableLine();
            if (cable != null)
            {
                bool needReverse = port.NeedReverse();
                SendPacketPhysical(data, cable, needReverse);
            }
        }
    }

    // Метод для створення візуального пакета і запуску його по кабелю
    public void SendPacketPhysical(PacketData data, LineRenderer cable, bool needReverse)
    {
        Rope ropeScript = cable.GetComponent<Rope>();

        if (ropeScript == null)
        {
            Debug.LogError("Не знайдено скрипт Rope на кабелі!");
            return;
        }

        // 2. Визначаємо цільовий Transform (Attach point всередині штекера)
        Transform targetAttachPoint = null;

        if (needReverse)
        {
            // Якщо їдемо "назад" (End -> Start), ціль - це Start
            targetAttachPoint = ropeScript.StartPoint;
        }
        else
        {
            // Якщо їдемо "прямо" (Start -> End), ціль - це End
            targetAttachPoint = ropeScript.EndPoint;
        }

        // 3. Отримуємо CablePlug
        // Оскільки ropeStart/End посилаються на об'єкт "Attach" (який є дитиною штекера),
        // нам треба шукати скрипт CablePlug у БАТЬКА цього трансформу.

        CablePlug destinationPlug = targetAttachPoint.GetComponentInParent<CablePlug>();

        // 4. Запускаємо пакет
        if (destinationPlug != null)
        {
            GameObject packetObj = Instantiate(packetPrefab);
            PacketMover mover = packetObj.GetComponent<PacketMover>();

            mover.Initialize(data, this, destinationPlug);
            mover.StartTransmission(cable, needReverse);
        }
        else
        {
            Debug.LogError($"Помилка: Не знайдено CablePlug у батьків об'єкта {targetAttachPoint.name}");
        }
    }
}