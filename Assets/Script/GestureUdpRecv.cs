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
                   

                    string h=HandController.instance.hand;
                 //   print("hand di hand controller"+h);
              
                    foreach(string s in handGesture){
                    
                    //la mano destra e sinistra vengono inviate invertite quindi se left -> mano destra e viceversa
                        if(s.Contains("Left")){
                            if(s.Contains("Closed_Fist")){
                                HandController.instance.simula_gestureDX_afferra();
                               Debug.Log("afferra");
                            }else if(s.Contains("Victory")){
                                Debug.Log("victory DX ");
                                HandController.instance.simula_gestureDX_rilascia();
                            }else if(s.Contains("Open_Palm")){
                                //Debug.Log("mano aperta non mi interessa");
                            }
                        }
                       if(s.Contains("Right")){

                            if(s.Contains("Closed_Fist")){
                               // HandController.instance.simula_gestureDX_afferra();
                                 HandController.instance.simula_gestureSX_afferra();
                                 Debug.Log("SINISTRA");
                            }else if(s.Contains("Victory")){
                               Debug.Log("SINISTRA");
                                 HandController.instance.simula_gestureSX_rilascia();
                              //  HandController.instance.simula_gestureDX_rilascia();
                            }else if(s.Contains("Open_Palm")){
                                //Debug.Log("mano aperta non mi interessa");
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