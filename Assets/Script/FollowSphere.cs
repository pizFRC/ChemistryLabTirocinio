using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FollowSphere : MonoBehaviour
{
  public GameObject[] sphere;
  public GameObject[] handPoint;
  public GameObject model;
  public string hand;
  public bool rotationOn;
  public float sampleThreshold;
  float lastSampleTime;
  Vector3 wPosition, tip;
  public float speed,distance,targetDistance;

  void Awake(){
  DontDestroyOnLoad(this.gameObject);
  }
  // Start is called before the first frame update
  void Start()
  {
    lastSampleTime = 0.0f;
    wPosition = sphere[0].transform.position;
    tip = sphere[9].transform.position;
   

  }
    void OnDisable(){
    
    }
  // Update is called once per frame
  void Update()
  {





    lastSampleTime += Time.deltaTime;
    if (lastSampleTime >= sampleThreshold)
    {

      //
  var step =  speed * Time.deltaTime;
      for (int i = 0; i < 21; i++)
      {

       // this.handPoint[i].transform.position = sphere[i].transform.position;
         


         
        
                this.handPoint[i].transform.position= Vector3.MoveTowards(this.handPoint[i].transform.position,sphere[i].transform.position, step);
        

      }




  

       

      if (rotationOn)
      {
        Vector3 p8 = sphere[17].transform.position;
        Vector3 wristPosition = sphere[0].transform.localPosition;
        Vector3 directionVector0_5 = new Vector3(p8.x - wristPosition.x, p8.y - wristPosition.y, p8.z - wristPosition.z);
        Quaternion rotation = Quaternion.LookRotation(Vector3.forward, directionVector0_5.normalized);
        Transform handTransform = handPoint[0].transform;
        
        Transform sphereTransform = sphere[0].transform;
    
   
     

        for(int i=1;i<21;i+=4){
          Vector3 tmp=sphere[i+3].transform.position;
          Vector3 roottmp=sphere[i].transform.position;
          Vector3 directionVector = new Vector3(tmp.x - roottmp.x, tmp.y - roottmp.y, tmp.z - roottmp.z);
             
        Quaternion rotationtmp = Quaternion.FromToRotation(Vector3.up, directionVector.normalized);
       
          sphere[i].transform.localRotation=rotationtmp;
        
       
        }
   
          sphereTransform.localRotation=rotation;   
    
      
      }

     

      lastSampleTime = 0f;
    }


  }

}