using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;

using System;

public class ImageRecv : MonoBehaviour
{
Thread receiveThread;
UdpClient client;
int port;
public bool startRecv;
string dataStr;
byte [] data;
    // Start is called before the first frame update
    void Start()
    {
        receiveThread=new Thread(new ThreadStart(RecvData));
    }

   
    private void RecvData()
    {
        client = new UdpClient(port);
        print("RECVDATA STARTED\n");
        while (startRecv)
        {
            try
            {
                IPEndPoint anyIP = new IPEndPoint(IPAddress.Any, 0);
                data = client.Receive(ref anyIP);
                dataStr = Encoding.ASCII.GetString(data);
               print("recv:" + data); 
            }
            catch (Exception err)
            {
                print(err.ToString());
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
