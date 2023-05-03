using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading;
using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using System.Globalization;


/* Currently very messy because both the server code and hand-drawn code is all in the same file here.
 * But it is still fairly straightforward to use as a reference/base.
 */

public class PipeServer : MonoBehaviour
{
    public Transform rParent;
    public Transform lParent;

    public GameObject landmarkPrefab;
    public GameObject linePrefab;
    public float multiplier = 10f;



    const int LANDMARK_COUNT = 21;
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

    void OnApplicationQuit()
    {

        recvThread.Interrupt();
        recvThread.Abort();
        t.Interrupt();
        t.Abort();


    }
    public class Hand
    {
        public Vector3[] positionsBuffer = new Vector3[LANDMARK_COUNT];
        public GameObject[] instances = new GameObject[LANDMARK_COUNT];
        public LineRenderer[] lines = new LineRenderer[5];

        public float reportedSamplesPerSecond;
        public float lastSampleTime;
        public float samplesCounter;

        public Hand(Transform parent, GameObject landmarkPrefab, GameObject linePrefab)
        {




            for (int i = 0; i < instances.Length; ++i)
            {
                instances[i] = Instantiate(landmarkPrefab);// GameObject.CreatePrimitive(PrimitiveType.Sphere);

                instances[i].transform.localScale = Vector3.one * 0.1f;
                instances[i].transform.parent = parent;
            }
            for (int i = 0; i < lines.Length; ++i)
            {
                lines[i] = Instantiate(linePrefab).GetComponent<LineRenderer>();
            }
        }


        public void UpdateLines()
        {
            lines[0].positionCount = 5;
            lines[1].positionCount = 5;
            lines[2].positionCount = 5;
            lines[3].positionCount = 5;
            lines[4].positionCount = 5;

            lines[0].SetPosition(0, instances[(int)Landmark.Wrist].transform.position);
            lines[0].SetPosition(1, instances[(int)Landmark.Thumb1].transform.position);
            lines[0].SetPosition(2, instances[(int)Landmark.Thumb2].transform.position);
            lines[0].SetPosition(3, instances[(int)Landmark.Thumb3].transform.position);
            lines[0].SetPosition(4, instances[(int)Landmark.Thumb4].transform.position);

            lines[1].SetPosition(0, instances[(int)Landmark.Wrist].transform.position);
            lines[1].SetPosition(1, instances[(int)Landmark.Index1].transform.position);
            lines[1].SetPosition(2, instances[(int)Landmark.Index2].transform.position);
            lines[1].SetPosition(3, instances[(int)Landmark.Index3].transform.position);
            lines[1].SetPosition(4, instances[(int)Landmark.Index4].transform.position);

            lines[2].SetPosition(0, instances[(int)Landmark.Wrist].transform.position);
            lines[2].SetPosition(1, instances[(int)Landmark.Middle1].transform.position);
            lines[2].SetPosition(2, instances[(int)Landmark.Middle2].transform.position);
            lines[2].SetPosition(3, instances[(int)Landmark.Middle3].transform.position);
            lines[2].SetPosition(4, instances[(int)Landmark.Middle4].transform.position);

            lines[3].SetPosition(0, instances[(int)Landmark.Wrist].transform.position);
            lines[3].SetPosition(1, instances[(int)Landmark.Ring1].transform.position);
            lines[3].SetPosition(2, instances[(int)Landmark.Ring2].transform.position);
            lines[3].SetPosition(3, instances[(int)Landmark.Ring3].transform.position);
            lines[3].SetPosition(4, instances[(int)Landmark.Ring4].transform.position);

            lines[4].SetPosition(0, instances[(int)Landmark.Wrist].transform.position);
            lines[4].SetPosition(1, instances[(int)Landmark.Pinky1].transform.position);
            lines[4].SetPosition(2, instances[(int)Landmark.Pinky2].transform.position);
            lines[4].SetPosition(3, instances[(int)Landmark.Pinky3].transform.position);
            lines[4].SetPosition(4, instances[(int)Landmark.Pinky4].transform.position);
        }

        public float GetFingerAngle(Landmark referenceFrom, Landmark referenceTo, Landmark from, Landmark to)
        {
            Vector3 reference = (instances[(int)referenceTo].transform.position - instances[(int)referenceFrom].transform.position).normalized;
            Vector3 direction = (instances[(int)to].transform.position - instances[(int)from].transform.position).normalized;
            return Vector3.SignedAngle(reference, direction, Vector3.Cross(reference, direction));
        }
    }

    Thread recvThread, gestureController;
    UdpClient client;
    Thread t;
    private Hand left;
    private Hand right;
    public bool lineVisible = false;
    public int port = 6790;
    public bool startRecv = true;
    public bool printToConsole = false;
    public string data;
    byte[] dataByte;
    Transform tip, pip, mcp, wrist;
    public float sampleThreshold = 0.25f; // how many seconds of data should be averaged to produce a single pose of the hand.

    private void Start()
    {

        //print("height"+Screen.height/2);
        //lParent.localPosition=new Vector3(lParent.localPosition.x,lParent.localPosition.y,lParent.localPosition.z);

        left = new Hand(lParent, landmarkPrefab, linePrefab);
        right = new Hand(rParent, landmarkPrefab, linePrefab);

        t = new Thread(new ThreadStart(Run));
        t.Start();
        gestureController = new Thread(new ThreadStart(checkGesture));
        gestureController.Start();


        recvThread = new Thread(new ThreadStart(RecvData));
        recvThread.IsBackground = true;
        recvThread.Start();

    }
    private void checkGesture()
    {

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
                data = Encoding.ASCII.GetString(dataByte);
                if (printToConsole) { print("recv:" + data); }
            }
            catch (Exception err)
            {
                print(err.ToString());
            }
        }
    }
    void OnDrawGizmosSelected()
    {
        // Draw a semitransparent red cube at the transforms position
        Gizmos.color = new Color(1, 0, 0, 0.5f);

        Gizmos.DrawCube(lParent.localPosition, new Vector3(1, 1, 1));
        Gizmos.color = new Color(1, 1, 0, 0.5f);
        Gizmos.DrawCube(rParent.localPosition, new Vector3(1, 1, 1));
    }

    private void Update()
    {

        UpdateHand(left);

        UpdateHand(right);

    }
    private void UpdateHand(Hand h)
    {
        if (h.samplesCounter == 0) return;

        if (Time.timeSinceLevelLoad - h.lastSampleTime >= sampleThreshold)
        {
            for (int i = 0; i < LANDMARK_COUNT; ++i)
            {

                h.instances[i].transform.localPosition = h.positionsBuffer[i] / (float)h.samplesCounter * multiplier;// / ;

                h.positionsBuffer[i] = Vector3.zero;

            }

            h.reportedSamplesPerSecond = h.samplesCounter / (Time.timeSinceLevelLoad - h.lastSampleTime);
            h.lastSampleTime = Time.timeSinceLevelLoad;
            h.samplesCounter = 0f;
            if (lineVisible)
                h.UpdateLines();


        }
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

                    string[] s = l.Split('|');
                    if (s.Length < 4) continue;
                    int i;
                    if (s[0] == "Left") h = left;
                    else if (s[0] == "Right") h = right;
                    if (!int.TryParse(s[1], out i)) continue;

                    CultureInfo ci = (CultureInfo)CultureInfo.CurrentCulture.Clone();
                    float x = float.Parse(s[2], CultureInfo.InvariantCulture.NumberFormat);
                    float y = float.Parse(s[3], CultureInfo.InvariantCulture.NumberFormat);
                    float z = float.Parse(s[4], CultureInfo.InvariantCulture.NumberFormat);
                    //  x*= Screen.width;
                    Vector3 v = new Vector3(x, y, z);

                    h.positionsBuffer[i] += v;


                    h.samplesCounter += 1f / LANDMARK_COUNT;
                }

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
                print("nel catch");
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
