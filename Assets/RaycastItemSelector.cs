using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastItemSelector : MonoBehaviour
{
    // Start is called before the first frame update

  
   
     GameObject raySelector;
    public GameObject prefab;//;,prefabLine;
    GameObject instanceLine;
    Vector3 startPoint;
    Vector3 direction;
     float elapsedTime=0f;
     public float  timeToWait=1.0f;
     public float smoothingFactor=0.020f;
    private LineRenderer lineRenderer;
    Vector3 previousHandDirection, thumbDirection,  thumbTipPosition,wristPosition , filteredHandDirection;
    void Start()
    {
       raySelector=Instantiate(prefab);
    //   instanceLine=Instantiate(prefabLine);
      lineRenderer=GetComponent<LineRenderer>();
        
    }

    // Update is called once per frame
    void Update()
    {
         
           elapsedTime+=Time.deltaTime;
           
            if(elapsedTime> timeToWait){

          //GET BONES POSITION
          wristPosition= this.transform.GetChild(0).transform.position;

          thumbTipPosition=this.transform.GetChild(4).transform.position;
        
          thumbDirection =(thumbTipPosition -wristPosition).normalized;  
 
          //SMOTHING FILTER
           filteredHandDirection=Vector3.Lerp(previousHandDirection,thumbDirection,smoothingFactor);

               
               //print("raycast");

                 
                        //GetComponent<LineRenderer>().
                     //    GetComponent<LineRenderer>().transform.LookAt(thumbDirection);
           //    GetComponent<LineRenderer>().alignment = LineAlignment.TransformZ;
                 Debug.DrawRay(thumbTipPosition,filteredHandDirection*3f, Color.red,2f);
                 raySelector.transform.localPosition=thumbTipPosition;
                 if(filteredHandDirection.magnitude>float.Epsilon)
                    raySelector.transform.localRotation=Quaternion.LookRotation(filteredHandDirection);
                  
                
                elapsedTime=0;
 
            
         }

                previousHandDirection=filteredHandDirection;
                

                
           
    }
                     

                       
                
              


      void OnCollisionEnter(Collision collision)
    {
        Debug.DrawRay(collision.contacts[0].point, collision.contacts[0].normal, Color.green, 2, false);
    }
  
    
    private  IEnumerator itemSelectedChangeColor(Transform t){


        
          t.GetComponent<Renderer>().material.SetColor("_Color", Color.black);
          yield return new WaitForSeconds(1);
          t.GetComponent<Renderer>().material.SetColor("_Color", Color.white);
          
              }
}
