
using System.IO;
using System.Text;
using System.Threading;
using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using System.Globalization;
using Unity.VisualScripting;
using UnityEngine.UIElements;




/* Currently very messy because both the server code and hand-drawn code is all in the same file here.
 * But it is still fairly straightforward to use as a reference/base.
 */
public enum Landmark
{
    Wrist = 0,
    Thumb1 = 1,
    Thumb2 = 2,
    Thumb3 = 3,
    Thumb4 = 4,
    Index1 = 5,
    Index2 = 6,
    Index3 = 7,
    Index4 = 8,
    Middle1 = 9,
    Middle2 = 10,
    Middle3 = 11,
    Middle4 = 12,
    Ring1 = 13,
    Ring2 = 14,
    Ring3 = 15,
    Ring4 = 16,
    Pinky1 = 17,
    Pinky2 = 18,
    Pinky3 = 19,
    Pinky4 = 20
}

public class HandPositionUdpRecv : MonoBehaviour
{
    public class Hand
    {
        public Vector3[] positionsBuffer = new Vector3[LANDMARK_COUNT];
        public GameObject[] instances = new GameObject[LANDMARK_COUNT];
      
        
        public GameObject palmCenterObject;
        public float reportedSamplesPerSecond;
        public float lastSampleTime;
        public float samplesCounter;


        public Hand(Transform parent, GameObject landmarkPrefab, GameObject linePrefab, GameObject[] sphere)
        {


            palmCenterObject=parent.GetChild(0).gameObject;
             
            for (int i = 0; i < instances.Length; ++i)
            {
                instances[i] = sphere[i];//Instantiate(landmarkPrefab);// GameObject.CreatePrimitive(PrimitiveType.Sphere);

                instances[i].transform.localScale = Vector3.one;
                instances[i].transform.parent = parent;
            }
           
        }

    }


    public GameObject[] leftSphere;
    public GameObject[] rightSphere;

    public float distanceForward = 3f;

    public Transform rParent;
    public Transform lParent;
    public float timeSinceLastDetection;

    public GameObject hand_left, hand_right;
    public GameObject landmarkPrefab;
    public GameObject linePrefab;

    public float multiplier = 1f;



    const int LANDMARK_COUNT = 21;
    public  int V = 5;
    Thread recvThread, gestureController;
    UdpClient client;
    Thread t;
    private Hand left;
    public int subtract_to_y, subtract_to_x;
    private Hand right;

    public int port = 6790;
    public float elapsedTimeRight = 0f;
    public float elapsedTimeLeft = 0f;
    public bool startRecv = true;
    
    public string data;
    byte[] dataByte;

    float timer;
    public int positionY,positionZ=10;
    bool resetHand;

    Transform tip, pip, mcp, wrist;
    public float sampleThreshold = 0.1f, cameraHeight, cameraWidth, widthRatio, heightRatio, mediaPipeImageHeight, mediaPipeImageWidth; // how many seconds of data should be averaged to produce a single pose of the hand.
    private void Start()
    {
        Camera mainCamera = Camera.main;
        cameraWidth = mainCamera.pixelWidth;
        cameraHeight = mainCamera.pixelHeight;

        widthRatio = cameraWidth / mediaPipeImageWidth;
        heightRatio = cameraHeight / mediaPipeImageHeight;

        left = new Hand(lParent, landmarkPrefab, linePrefab, leftSphere);
        right = new Hand(rParent, landmarkPrefab, linePrefab, rightSphere);
        //THREAD t: si occupa di aggiornare la posizione delle mani
        t = new Thread(new ThreadStart(Run));
        t.Start();

        timer = 0.0f;

        //Thread recvThread: riceve i dati delle mani dal server udp
        recvThread = new Thread(new ThreadStart(RecvData));
        recvThread.IsBackground = true;
        recvThread.Start();

    }
        void Awake(){
              DontDestroyOnLoad(this.gameObject);
        }

    void OnApplicationQuit()
    {

        recvThread.Interrupt();
        recvThread.Abort();
        t.Interrupt();
        t.Abort();


    }


    private void RecvData()
    {
        IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
        client = new UdpClient(new IPEndPoint(ipAddress, port));

        print("RECVDATA STARTED\n");
        while (startRecv)
        {

            try
            {

                IPEndPoint anyIP = new IPEndPoint(ipAddress, port);
                dataByte = client.Receive(ref anyIP);
                lock (this)
                {
                    
                    data = Encoding.ASCII.GetString(dataByte);
                    if (data.Contains("NULL")){
                        resetHand=true;
                    }

                }





            }
            catch (Exception err)
            {
                print(err.ToString());
            }
        }
    }



    private void Update()
    {

        elapsedTimeRight += Time.deltaTime;
        elapsedTimeLeft += Time.deltaTime;

        UpdateHand(left);

        UpdateHand(right);

        widthRatio = cameraWidth / mediaPipeImageWidth;
        heightRatio = cameraHeight / mediaPipeImageHeight;
        correctHandPosition();

    }

    private void correctHandPosition()
    {
        int larghezzaFinestra = Screen.width;

        // Calcola le posizioni in coordinate del mondo
        float posXSinistra = Camera.main.ScreenToWorldPoint(new Vector3(0-larghezzaFinestra, 0, 10)).x; // 10 è la distanza dalla camera
        float posXDestra = Camera.main.ScreenToWorldPoint(new Vector3(larghezzaFinestra, 0, 10)).x;

        // Assegna le posizioni agli oggetti
        lParent.localPosition = new Vector3(posXSinistra, positionY, positionZ);
        rParent.localPosition = new Vector3(posXDestra,positionY, positionZ);

    }

    private void UpdateHand(Hand h)
    {
        if (elapsedTimeLeft > timeSinceLastDetection || resetHand)
        {
            SetLeftVisibile(false);
            Messenger<bool>.Broadcast(GameEvents.MISSING_HAND_LEFT, true);
        }
        else
        {
            SetLeftVisibile(true);
            Messenger<bool>.Broadcast(GameEvents.MISSING_HAND_LEFT, false);
        }

        if (elapsedTimeRight >timeSinceLastDetection || resetHand)
        {
            SetRightVisibile(false);
            Messenger<bool>.Broadcast(GameEvents.MISSING_HAND_RIGHT, true);
        
        }
        else
        {
            SetRightVisibile(true);
            Messenger<bool>.Broadcast(GameEvents.MISSING_HAND_RIGHT, false);
        }
        if(resetHand)
            resetHand=!resetHand;

        timer += Time.deltaTime;

        if (Time.timeSinceLevelLoad - h.lastSampleTime >= sampleThreshold)
        {
            Vector3 palmCenter=Vector3.zero;
            for (int i = 0; i < LANDMARK_COUNT; ++i)
            {   

                h.instances[i].transform.localPosition = h.positionsBuffer[i] * multiplier;// / (float)h.samplesCounter * multiplier;// / ;

                h.positionsBuffer[i] = Vector3.zero;

                palmCenter+= h.instances[i].transform.position;

            }
           
            Vector3 forwardDirection = h.instances[0].transform.forward; // Assumendo che il punto chiave 4 rappresenti un punto nella parte anteriore della mano
            
             palmCenter/=21;
           

            Vector3 palmCenterForward = palmCenter + (forwardDirection.normalized * distanceForward);
            // Crea un oggetto sfera
            h.palmCenterObject.transform.position = palmCenterForward; //
            h.palmCenterObject.transform.LookAt(h.instances[0].transform,Vector3.up);
            h.reportedSamplesPerSecond = h.samplesCounter / (Time.timeSinceLevelLoad - h.lastSampleTime);
            h.lastSampleTime = Time.timeSinceLevelLoad;
            h.samplesCounter = 0f;

        }

    }

    private void SetRightVisibile(bool value)
    {
        hand_right.SetActive(value);
       /*  if(value)
        rParent.GetComponent<RaycastItemSelector>().mode=selectorMode.CanSelect;
        else
        rParent.GetComponent<RaycastItemSelector>().mode=selectorMode.CannotSelect; */
        if(!value)
        rParent.GetComponent<LineRenderer>().positionCount = 0;
    }

    private void SetLeftVisibile(bool value)
    {
        hand_left.SetActive(value);
        
         /*  if(value)
        lParent.GetComponent<RaycastItemSelector>().mode=selectorMode.CanSelect;
        else
        lParent.GetComponent<RaycastItemSelector>().mode=selectorMode.CannotSelect; */
          if(!value)
         lParent.GetComponent<LineRenderer>().positionCount = 0;
    }

    void Run()
    {


        print("Waiting for connection...");


        print("Connected.");

        while (true)
        {


            try
            {
               
                Hand h = null;
                // var len = (int)br.ReadUInt32();
                //var str = new string(br.ReadChars(len));

                //Debug.Log(data);

                string[] lines = data.Split('\n');

                foreach (string l in lines)
                {
                    int rangeToSum=0;
                    string[] s = l.Split('|');
                    if (s.Length < 4) continue;
                    int i;
                    if (!int.TryParse(s[1], out i)) continue;
                    if (s[0] == "Left")
                    {
                        h = left;
                        elapsedTimeLeft = 0f;
                        rangeToSum=V;

                    }
                    else if (s[0] == "Right")
                    {
                        h = right;
                        elapsedTimeRight = 0f;
                        rangeToSum=-V;

                    }

                    h.samplesCounter += 1f / LANDMARK_COUNT;
                    CultureInfo ci = (CultureInfo)CultureInfo.CurrentCulture.Clone();
                    float x= float.Parse(s[2], CultureInfo.InvariantCulture.NumberFormat);
                    float y = float.Parse(s[3], CultureInfo.InvariantCulture.NumberFormat);
                    float z = float.Parse(s[4], CultureInfo.InvariantCulture.NumberFormat);


                    Vector3 v = new Vector3(x, y, z);
                Vector3 screenPosition ;
                    if(h==left)
                 screenPosition = new Vector3((Screen.width/2)+ x * Screen.width,Screen.height - (y * Screen.height), -z*Screen.width);
                    else
                    screenPosition = new Vector3( (-Screen.width/2)+x * Screen.width,Screen.height - (y * Screen.height), -z*Screen.width);
                    screenPosition.x -= subtract_to_x;
                    screenPosition.y -= subtract_to_y;
                  
                 /*    v.x *= widthRatio;
                    v.y *= heightRatio; */
                    
                    if(v.z==0)
                     v.z+=10;
                    h.positionsBuffer[i] = screenPosition;

                    

                }

                 

                  // Assumendo che il punto chiave 4 rappresenti un punto nella parte anteriore della mano
                

                if (!recvThread.IsAlive)
                {
                    t.Abort();
                    this.recvThread.Abort();
                }
            }
            catch (EndOfStreamException)
            {
                t.Abort();
                this.recvThread.Abort();
                
                break;                    // When client disconnects

            }


        }
        t.Abort();
        this.recvThread.Abort();
    }

    private void OnDisable()
    {
        print("Client disconnected.");

    }
}
