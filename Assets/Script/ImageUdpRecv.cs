using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using System.Collections;
using UnityEngine.UI;
using System;

public class ImageUdpRecv : MonoBehaviour
{
Thread receiveThread;
UdpClient client;
public  RawImage rm;
public int port=6792;
public bool startRecv;
string dataStr;
byte [] data;
    // Start is called before the first frame update
    void Start()
    {
        receiveThread=new Thread(new ThreadStart(RecvData));
        receiveThread.Start();
    }
    void Awake(){
          DontDestroyOnLoad(this.gameObject);
    }
   
    private void RecvData()
    {
      IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
        client = new UdpClient(new IPEndPoint(ipAddress, 6792));
        
        print("image recv started\n");
        while (startRecv)
        {
            try
            {
                IPEndPoint anyIP = new IPEndPoint(ipAddress,port);
                data = client.Receive(ref anyIP);
                //qui uso la nuove classe
                if(data.Length>0)
               UnityMainThreadDispatcher.Instance().Enqueue(() =>BytesToTexture2D(data));
              
            
            }
            catch (Exception err)
            {
                print(err.ToString());
                break;
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
 private void OnDestroy() {
    startRecv=false;
    client.Close();
}
}
