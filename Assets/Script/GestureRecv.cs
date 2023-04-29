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
public class GestureRecv : MonoBehaviour
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

   
    private void RecvData()
    {
        client = new UdpClient(port);
        print("gesture recv started\n");
        while (startRecv)
        {
            try
            {
                IPEndPoint anyIP = new IPEndPoint(IPAddress.Any,port);
                    byte []data = client.Receive(ref anyIP);
                 string dataStr = Encoding.ASCII.GetString(data);
                    print("recv:"+dataStr);
                 
                    string []handGesture=dataStr.Split("---");
                    if (handGesture.Length<1)
                        continue;

                    string h=HandController.instance.hand;
                    print("hand di hand controller"+h);
                    if(h=="Left")
                        h="Right";
                    else if(h=="Right")
                        h="Left";
                    foreach(string s in handGesture){
                    
                    //la mano destra e sinistra vengono inviate invertite quindi se left -> mano destra e viceversa
                        if(s.Contains(h)){
                            if(s.Contains("Closed_Fist")){
                                HandController.instance.simula_gesture_afferra();
                                Debug.Log("afferra");
                            }else if(s.Contains("Victory")){
                                Debug.Log("victory");
                                HandController.instance.simula_gesture_rilascia();
                            }else if(s.Contains("Open_Palm")){
                                Debug.Log("mano aperta non mi interessa");
                            }
                        }
                    }
                   
                   

                    
                //qui uso la nuove classe
                
              
            }
            catch (Exception err)
            {
                print(err.ToString());
            }
        }
    }
    // Update is called once per frame
   



}
