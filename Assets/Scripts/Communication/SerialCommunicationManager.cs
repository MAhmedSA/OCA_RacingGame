using System;
using System.IO;
using System.IO.Ports;
using System.Threading;
using UnityEngine;

[System.Serializable]
public class PortConfig
{
    public string port1;
    public string port2;
}
public class SerialCommunicationManager : MonoBehaviour, ISerialReader
{
    public event Action<string> OnMessageReceived;
   
    [Header("Serial Settings")]
    [SerializeField] private string portName1 = "COM10"; //Player1 S1
    [SerializeField] private string portName2 = "COM11"; //Player2 S2

    [SerializeField] private int baudRate = 115200;
    [SerializeField] private int readTimeout = 50;

    private SerialPort serialPort1;
    private SerialPort serialPort2;

    [Header("Timer Settings")]
    [SerializeField] private TimerCount timer;

    private Thread readThread1;
    private Thread readThread2;

    private bool keepReading = true;

    void Awake()
    {
#if !UNITY_EDITOR
        LoadPortsFromJson(); 
            InitializeSerialPorts();
#endif
    }

    void Start()
    {
#if !UNITY_EDITOR
            Open();
            StartReadingThreads();
#endif
    }

    void InitializeSerialPorts()
    {
        serialPort1 = new SerialPort(portName1, baudRate)
        {
            ReadTimeout = readTimeout,
            NewLine = "\n"
        };

        serialPort2 = new SerialPort(portName2, baudRate)
        {
            ReadTimeout = readTimeout,
            NewLine = "\n"
        };
    }

    void StartReadingThreads()
    {
        keepReading = true;
        readThread1 = new Thread(() => ReadSerial(serialPort1, "Player1Finished"));
        readThread1.Start();

        readThread2 = new Thread(() => ReadSerial(serialPort2, "Player2Finished"));
        readThread2.Start();
    }

    private void ReadSerial(SerialPort port, string playerEvent)
    {
        while (keepReading)
        {
            try
            {
                if (port == null || !port.IsOpen) continue;

                string msg = port.ReadLine().Trim();

                // Only allow detection after timer >= 1
                if (timer.time < 1f) continue;

                bool detected = !msg.Contains(">1m");
                if (detected)
                {
                    OnMessageReceived?.Invoke(playerEvent);
                }
            }
            catch (TimeoutException)
            {
                // normal, just retry
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"Serial read error ({playerEvent}): {ex.Message}");
            }
        }
    }

    public void Restart()
    {
        // Nothing else needed; events will fire again on next detection
    }

    public void Open()
    {
        if (serialPort1 != null && !serialPort1.IsOpen) serialPort1.Open();
        if (serialPort2 != null && !serialPort2.IsOpen) serialPort2.Open();
    }

    public void Close()
    {
        keepReading = false;

        readThread1?.Join();
        readThread2?.Join();

        if (serialPort1 != null && serialPort1.IsOpen) serialPort1.Close();
        if (serialPort2 != null && serialPort2.IsOpen) serialPort2.Close();
    }

    void OnDisable()
    {
#if !UNITY_EDITOR
        Close();
#endif
    }
    
    void LoadPortsFromJson()
    {
        try
        {
            string path = Path.Combine(Application.streamingAssetsPath, "config.json");

            if (!File.Exists(path))
            {
                Debug.LogWarning("Config not found, creating one...");

                PortConfig defaultConfig = new PortConfig()
                {
                    port1 = "COM10",
                    port2 = "COM11"
                };

                string json = JsonUtility.ToJson(defaultConfig, true);

                try
                {
                    File.WriteAllText(path, json);
                }
                catch
                {
                    Debug.LogError("Can't write to StreamingAssets!");
                }

                portName1 = defaultConfig.port1;
                portName2 = defaultConfig.port2;
                return;
            }

            string fileContent = File.ReadAllText(path);
            PortConfig config = JsonUtility.FromJson<PortConfig>(fileContent);

            if (config != null)
            {
                portName1 = config.port1;
                portName2 = config.port2;
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("Error loading config: " + ex.Message);
        }
    }
}