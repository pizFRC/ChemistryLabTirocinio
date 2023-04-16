using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastItemSelector : MonoBehaviour
{
    // Start is called before the first frame update

    public float maxDistance=100f;
   
     GameObject raySelector;
    public GameObject prefab;
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
      lineRenderer=GetComponent<LineRenderer>();
        
    }

    // Update is called once per frame
    void Update()
    {
         
           elapsedTime+=Time.deltaTime;
           
           /*
       
        
         
       

 */

          
          //+thumbDirection);
         
 




            if(elapsedTime> timeToWait){
          wristPosition= this.transform.GetChild(0).transform.position;
          thumbTipPosition=this.transform.GetChild(4).transform.position;
          thumbDirection =(thumbTipPosition -wristPosition).normalized;  
        
             
            Vector3 indexTipPosition= this.transform.GetChild(8).transform.position;
          Vector3 indexDirection =  (indexTipPosition -wristPosition).normalized;
         
       Vector3 wristToMiddleFingeMCPDirection = (this.transform.GetChild(9).transform.position-wristPosition ).normalized;

        Vector3 direction=thumbDirection+wristToMiddleFingeMCPDirection/2+indexDirection/2;
             

           filteredHandDirection=Vector3.Lerp(previousHandDirection,thumbDirection,smoothingFactor);

               elapsedTime=0;
               print("raycast");
               
                Debug.DrawRay(wristPosition,filteredHandDirection*3f,Color.red,1f);
                 raySelector.transform.localPosition=wristPosition;
                 if(filteredHandDirection.magnitude>float.Epsilon)
                    raySelector.transform.localRotation=Quaternion.LookRotation(filteredHandDirection);
                   
           }
                previousHandDirection=filteredHandDirection;
           
    }
                     

                       
                
              


      void OnCollisionEnter(Collision collision)
    {
        Debug.DrawRay(collision.contacts[0].point, collision.contacts[0].normal, Color.green, 2, false);
    }
    private void OnTriggerEnter(Collider other){
      print(other.gameObject.name);
    }
    
    private  IEnumerator itemSelectedChangeColor(Transform t){


        
          t.GetComponent<Renderer>().material.SetColor("_Color", Color.black);
          yield return new WaitForSeconds(1);
          t.GetComponent<Renderer>().material.SetColor("_Color", Color.white);
          
              }
}
