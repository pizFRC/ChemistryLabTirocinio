using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using System.Collections;
using UnityEngine.UI;
using System;

public class ImageRecv : MonoBehaviour
{
Thread receiveThread;
UdpClient client;
public  RawImage rm;
int port=6792;
public bool startRecv;
string dataStr;
byte [] data;
    // Start is called before the first frame update
    void Start()
    {
        receiveThread=new Thread(new ThreadStart(RecvData));
        receiveThread.Start();
    }

   
    private void RecvData()
    {
        client = new UdpClient(port);
        print("image recv started\n");
        while (startRecv)
        {
            try
            {
                IPEndPoint anyIP = new IPEndPoint(IPAddress.Any, 0);
                data = client.Receive(ref anyIP);
                //qui uso la nuove classe
                if(data.Length>0)
               UnityMainThreadDispatcher.Instance().Enqueue(() =>BytesToTexture2D(data));
               print("recv:" + data.ToString()); 
              
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



    void  BytesToTexture2D(byte[] imageData)
{

    
    Texture2D texture = new Texture2D(2, 2);
      texture.LoadImage(imageData);
    
        texture.Apply();
        rm.texture=texture;
  
   
}

}
