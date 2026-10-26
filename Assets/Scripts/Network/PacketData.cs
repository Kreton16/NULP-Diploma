[System.Serializable]
public class PacketData
{
    public string sourceIP;      // Хто відправив (наприклад "192.168.1.1")
    public string destinationIP; // Куди летить (наприклад "192.168.1.2")
    public string message;       // Наприклад "Ping Request"
}
