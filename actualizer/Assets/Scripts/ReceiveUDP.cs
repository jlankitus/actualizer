using UnityEngine;
using System.Collections;
using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

public class ReceiveUDP : MonoBehaviour
{
    static readonly object lockObject = new object();
    string returnData = "";
    bool processData = false;
    Thread thread;
    UdpClient udp;
    public bool threadOn = true;

    [ContextMenu("StartThread")]
    void Start()
    {
        udp = new UdpClient(462);
        threadOn = true;
        thread = new Thread(new ThreadStart(ThreadMethod));
        thread.Start();
    }

    [ContextMenu("StopThread")]
    public void stopThread()
    {
        threadOn = false;
    }

    void Update()
    {
        if (processData)
        {
            /*lock object to make sure there data is 
             *not being accessed from multiple threads at thesame time*/
            lock (lockObject)
            {
                processData = false;
                Debug.Log("Received: " + returnData);
                //Reset it for next read(OPTIONAL)
                returnData = "";
            }
        }
    }

    private void ThreadMethod()
    {
        print("Thread called...");  
        while (threadOn)
        {
            print("Remote IP called...");
            IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Loopback, 7400);
            byte[] receiveBytes = udp.Receive(ref RemoteIpEndPoint);

            /* lock object to make sure there data is 
            *not being accessed from multiple threads at thesame time */
            print("About to lock...");
            lock (lockObject)
            {
                returnData = Encoding.ASCII.GetString(receiveBytes);
                print("lock...");
                Debug.Log(returnData);
                if (returnData == "1\n")
                {
                    //Done, notify the Update function
                    processData = true;
                }
            }
        }
    }

    void OnApplicationQuit()
    {
        thread.Abort();
        if (udp != null)
            udp.Close();
    }
}