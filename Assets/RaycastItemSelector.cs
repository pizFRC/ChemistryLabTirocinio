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
           
            if(elapsedTime> timeToWait){

          //GET BONES POSITION
          wristPosition= this.transform.GetChild(0).transform.position;

          thumbTipPosition=this.transform.GetChild(4).transform.position;
        
          thumbDirection =(thumbTipPosition -wristPosition).normalized;  
 
          //SMOTHING FILTER
           filteredHandDirection=Vector3.Lerp(previousHandDirection,thumbDirection,smoothingFactor);

               
               //print("raycast");


     
                 Debug.DrawRay(thumbTipPosition,filteredHandDirection*3f, Color.red,2f);
                 raySelector.transform.localPosition=thumbTipPosition;
                 if(filteredHandDirection.magnitude>float.Epsilon)
                    raySelector.transform.localRotation=Quaternion.LookRotation(filteredHandDirection);
                   
                }

                previousHandDirection=filteredHandDirection;
                elapsedTime=0;


/*

        int layerMask = 1 << 5;

      

        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(thumbTipPosition,filteredHandDirection*3f, out hit))
        {
          
            Debug.Log(hit.transform.gameObject.name);
        }
*/




                
           
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
