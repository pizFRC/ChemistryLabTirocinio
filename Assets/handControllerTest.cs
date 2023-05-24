using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Globalization;
using System.Net;
using System.Net.Sockets;

using System.Text;
using System.Threading;

using System;

public class handControllerTest : MonoBehaviour
{
    // Start is called before the first frame update
    UdpClient client;
    Thread t;

    public GameObject SX_Hand;
    public GameObject DX_Hand;

    public GameObject handtest;
    public GameObject[] SX;
    public GameObject[] DX;
    GameObject[] hand;
    public float moltiplicatore = 10f;
    public float moltiplicatore_Z = 10f;
     
     public float  smoothTime;
     private Vector3 handVelocity = Vector3.zero;
    public float depthMin = 0f;
    public float depthMax = 10f;
    public float depth;
    float z_min, z_max, z_proportional, x_min, x_max, y_min, y_max, x_proportional, y_proportional;
    byte[] dataByte;
    
        Thread c;
    string data = "";
    public bool printToConsole;
    void Start()
    {
        t = new Thread(new ThreadStart(RecvData));

        t.IsBackground = true;
        t.Start();

        /*   c=new Thread(new ThreadStart(UpdateHand));

          c.Start();*/
    }
    private void RecvData()
    {
        IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
        client = new UdpClient(new IPEndPoint(ipAddress, 6770));

        print("world landkamrk STARTED\n");
        while (true)
        {
            try
            {
                IPEndPoint anyIP = new IPEndPoint(ipAddress, 6770);
                dataByte = client.Receive(ref anyIP);
                data = Encoding.ASCII.GetString(dataByte);
                if (printToConsole) { print("recv:" + data); }
            }
            catch (Exception err)
            {
                print(err.ToString());
            }
        }
    }

    private void UpdateHand()
    {
        print("update hand starte after");
        string handS = "";

        try
        {
            string[] lines = data.Split('\n');

            foreach (string l in lines)
            {

                string[] s = l.Split('|');

                if (s[0] == "Left") { hand = SX; handS = "Left"; }
                else if (s[0] == "Right") { hand = DX; handS = "Right"; }
                if (s.Length < 4) continue;
                int i;

                if (!int.TryParse(s[1], out i)) continue;


                print("passati controlli");
                CultureInfo ci = (CultureInfo)CultureInfo.CurrentCulture.Clone();
                float x = float.Parse(s[2], CultureInfo.InvariantCulture.NumberFormat);
                float y = float.Parse(s[3], CultureInfo.InvariantCulture.NumberFormat);
                float z = float.Parse(s[4], CultureInfo.InvariantCulture.NumberFormat);



                if (z < z_min)
                    z_min = z;

                if (z > z_max)
                    z_max = z;



                z_proportional = (z - z_min) * moltiplicatore / (z_max - z_min);
                depth = z_proportional;

                if (x < x_min)
                    x_min = x;


                if (x > x_max)
                    x_max = x;
                    x_proportional = (x - x_min) * moltiplicatore / (x_max - x_min);
                if (y < y_min)
                    y_min = y;


                if (y > y_max)
                    y_max = y;
                    y_proportional = (y - y_min) * moltiplicatore_Z / (y_max - y_min);
                // ottieni il valore z dalla mano Mediapipe


                // esegui la mappatura lineare dei valori z sulla scala di profondità della scena


                // applica la nuova profondità all'asse z delle sfere che rappresentano la mano
               Vector3 newPosition= new Vector3(x_proportional,y_proportional, depth);
                if (handS == "Left"){
                   
                     Vector3 handPosition = Vector3.SmoothDamp(SX_Hand.transform.localPosition, newPosition, ref handVelocity, smoothTime);
                    SX_Hand.transform.localPosition =handPosition;
                     Debug.Log(handPosition);

                }
                if (handS == "Right"){
                  Vector3 handPosition = Vector3.SmoothDamp(SX_Hand.transform.localPosition, newPosition, ref handVelocity, smoothTime);
                    DX_Hand.transform.localPosition =handPosition;
                Debug.Log(handPosition);
                }

                //handtest.gameObject.transform.localPosition=new Vector3(x*moltiplicatore , y*moltiplicatore,depth);
                /*   hand[i].transform.Translate(p);


                           hand[i].transform.localPosition = p;
                           hand[i].transform.position =new Vector3(x*moltiplicatore , y*moltiplicatore , z*moltiplicatore);
                           hand[i].transform.parent.localPosition =new Vector3(x*moltiplicatore , y*moltiplicatore , z*moltiplicatore);
                       hand[i].transform.localRotation=new Quaternion(180f,0,0,0);
                       */
                /* Vector3 pos=new Vector3( hand[i].transform.parent.localPosition.x,hand[i].transform.parent.localPosition.y,depth);
                  hand[i].transform.parent.localPosition=pos;*/
               

            }

        }
        catch (Exception e)
        {
            Debug.LogError(e);



        }

    }
    private void OnDestroy()
    {


        t.Abort();
        c.Abort();
    }
    // Update is called once per frame
    void Update()
    {



        UpdateHand();
    }
}
