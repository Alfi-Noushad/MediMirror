using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class UDPReceiver : MonoBehaviour
{
    [Header("Body Parts")]
    public Transform rightShoulder;
    public Transform chest;
    public Transform head;

    // Incoming values
    float shoulderX, shoulderY;
    float chestX, chestY;
    float headX, headY;

    // Default rotations
    Quaternion s0, c0, h0;

    UdpClient client;
    Thread receiveThread;

    void Start()
    {
        s0 = rightShoulder.localRotation;
        c0 = chest.localRotation;
        h0 = head.localRotation;

        client = new UdpClient(5052);
        receiveThread = new Thread(ReceiveData);
        receiveThread.IsBackground = true;
        receiveThread.Start();
    }

    void ReceiveData()
    {
        IPEndPoint ep = new IPEndPoint(IPAddress.Any, 0);

        while (true)
        {
            try
            {
                byte[] bytes = client.Receive(ref ep);
                string json = Encoding.UTF8.GetString(bytes);

                PoseData data = JsonUtility.FromJson<PoseData>(json);

                shoulderX = data.shoulderX;
                shoulderY = data.shoulderY;

                chestX = data.chestX;
                chestY = data.chestY;

                headX = data.headX;
                headY = data.headY;
            }
            catch { }
        }
    }

    void Update()
    {
        // SHOULDER
        rightShoulder.localRotation =
            Quaternion.Slerp(
                rightShoulder.localRotation,
                s0 * Quaternion.Euler(shoulderX, shoulderY, 0),
                Time.deltaTime * 8f);

        // CHEST
        chest.localRotation =
            Quaternion.Slerp(
                chest.localRotation,
                c0 * Quaternion.Euler(chestX, chestY, 0),
                Time.deltaTime * 6f);

        // HEAD
        head.localRotation =
            Quaternion.Slerp(
                head.localRotation,
                h0 * Quaternion.Euler(headX, headY, 0),
                Time.deltaTime * 8f);
    }

    [Serializable]
    public class PoseData
    {
        public float shoulderX;
        public float shoulderY;
        public float chestX;
        public float chestY;
        public float headX;
        public float headY;
    }
}