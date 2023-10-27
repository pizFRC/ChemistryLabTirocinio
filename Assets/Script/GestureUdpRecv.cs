using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

using System;
using System.Net;
using System.Globalization;
using System.IO;
using System.Net.Sockets;
using System.Text;
public class GestureUdpRecv : MonoBehaviour
{
    
Thread receiveThread;
UdpClient client;
public int port=6794;
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
        client = new UdpClient(new IPEndPoint(ipAddress, port));
        print("gesture recv started\n");
        IPEndPoint anyIP = new IPEndPoint(ipAddress,port);
        string dataStr="";
        while (startRecv)
        {
            
            try
            {
                 data = client.Receive(ref anyIP);
                 dataStr = Encoding.ASCII.GetString(data);
                 
                 }
                  catch (Exception err)
            {

               
                print(err.ToString() +  err.GetType().ToString());
            }
                 
                   // print("recv:"+dataStr);
                 if(dataStr.Length<5)
                    continue;
                    string []handGesture=dataStr.Split("---");
                   

                 //   print("hand di hand controller"+h);
              
                    foreach(string s in handGesture){
                    
                    //la mano destra e sinistra vengono inviate invertite quindi se left -> mano destra e viceversa
                        if(s.Contains("Right")){
                            if(s.Contains("Closed_Fist")){
                                
                                Messenger<Gesture>.Broadcast(GameEvents.GRAB,Gesture.GrabRight);
                               
                               Messenger<Gesture>.Broadcast(GameEvents.UI_GESTURE,Gesture.GrabRight);
                            }else if(s.Contains("Victory")){
                                Messenger<Gesture>.Broadcast(GameEvents.RELEASE,Gesture.ReleaseRight);
                                Messenger<Gesture>.Broadcast(GameEvents.UI_GESTURE,Gesture.ReleaseRight);
                            }else if(s.Contains("Thumb_Up")){
                                 Messenger<Gesture>.Broadcast(GameEvents.INTERACT,Gesture.InteractRight);
                            
                            }else if(s.Contains("None") ){
                                 Messenger<Gesture>.Broadcast(GameEvents.NONE,Gesture.NoneRight);
                            
                            }else if(s.Contains("Open_Palm")){
                                 Messenger<Gesture>.Broadcast(GameEvents.OPEN_PALM,Gesture.OpenPalmRight);
                                
                            }
                        }
                       if(s.Contains("Left")){

                            if(s.Contains("Closed_Fist")){
                                Messenger<Gesture>.Broadcast(GameEvents.GRAB,Gesture.GrabLeft);
                                Messenger<Gesture>.Broadcast(GameEvents.UI_GESTURE,Gesture.GrabLeft);
                                
                            }else if(s.Contains("Victory")){
                                Messenger<Gesture>.Broadcast(GameEvents.RELEASE,Gesture.ReleaseLeft);
                                Messenger<Gesture>.Broadcast(GameEvents.UI_GESTURE,Gesture.ReleaseLeft);
                                print("left vic");
                               
                            }else if(s.Contains("Thumb_Up")){
                                 Messenger<Gesture>.Broadcast(GameEvents.INTERACT,Gesture.InteractLeft);
                            
                            }
                            else if(s.Contains("None")){
                                 Messenger<Gesture>.Broadcast(GameEvents.NONE,Gesture.NoneLeft);
                            
                            }else if(s.Contains("Open_Palm")){
                                   Messenger<Gesture>.Broadcast(GameEvents.OPEN_PALM,Gesture.OpenPalmLeft);
                                
                            }
                        }
                    }
                   
                   

                    
                //qui uso la nuove classe
                
              
            dataStr="";
            
        }
    }
    // Update is called once per frame
   
private void OnDestroy() {
    startRecv=false;
    client.Close();
}


}
