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
     public float  timeToWait=2f;
    private LineRenderer lineRenderer;
    void Start()
    {
       raySelector=Instantiate(prefab);
      lineRenderer=GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
           
           elapsedTime+=Time.deltaTime;
           
          Vector3 wristPosition= this.transform.GetChild(0).transform.position;

       
          Vector3 indexTipPosition= this.transform.GetChild(8).transform.position;
          Vector3 pinkyTipPosition= this.transform.GetChild(20).transform.position;
          Vector3 thumbTipPosition=this.transform.GetChild(4).transform.position;
          Vector3 indexDirection =  (indexTipPosition -wristPosition).normalized;
          Vector3 pinkyDirection =(pinkyTipPosition -wristPosition).normalized;   
         Vector3 thumbDirection =(thumbTipPosition -wristPosition).normalized;  
          Vector3 handDirection= (indexDirection+pinkyDirection+thumbDirection);
        
 





            if(elapsedTime> timeToWait){
           
               elapsedTime=0;
                 raySelector.transform.localPosition=wristPosition;
                 if(handDirection.magnitude>float.Epsilon)
                    raySelector.transform.localRotation=Quaternion.LookRotation(handDirection);

           }
           
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
