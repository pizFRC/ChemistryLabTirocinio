using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastItemSelector : MonoBehaviour
{
    // Start is called before the first frame update

  
   
     GameObject raySelector;
    
    GameObject instanceLine;
    Vector3 startPoint;
    Vector3 direction;
     float elapsedTime=0f;
     public float  timeToWait=1.0f;
     public float smoothingFactor=0.020f,maxDistance=150f;
    private LineRenderer lineRenderer;
    float timeOverItem=0f;
    InteractableItem lastItem;
    public float range=2;
    Vector3 previousHandDirection, thumbDirection,  thumbTipPosition,wristPosition , filteredHandDirection;
    void Start()
    {
      
    //   instanceLine=Instantiate(prefabLine);
      lineRenderer=GetComponent<LineRenderer>();
        
    }

    // Update is called once per frame
    void Update()
    {
         
           elapsedTime+=Time.deltaTime;
          
          //ogni 1/10 di secondo 
            if(elapsedTime> timeToWait){

          //GET BONES POSITION
          wristPosition= this.transform.GetChild(0).transform.position;

          thumbTipPosition=this.transform.GetChild(4).transform.position;
        
          thumbDirection =(thumbTipPosition -wristPosition).normalized;  
 
          //SMOTHING FILTER
           filteredHandDirection=Vector3.Lerp(previousHandDirection,thumbDirection,smoothingFactor);

               
               //print("raycast");

                 
                    Ray ray=new Ray(thumbTipPosition,filteredHandDirection*range);
                    
             RaycastHit hit;
                       
           Vector3 endPosition = filteredHandDirection * maxDistance;
                      int layerMask= 1 << 8;
              layerMask = ~layerMask;

            if(Physics.Raycast(ray,out hit,250f)){
            Debug.DrawRay(thumbTipPosition,filteredHandDirection*range,Color.black,0.2f);   
              endPosition=hit.point;
              GetComponent<LineRenderer>().positionCount = 2;
              GetComponent<LineRenderer>().SetPosition(0,thumbTipPosition);
              GetComponent<LineRenderer>().SetPosition(1,endPosition);
                   if(hit.collider.tag=="Item"){
                    lastItem=hit.collider.GetComponent<InteractableItem>();
                     timeOverItem+=Time.deltaTime+0.1f;
                    
                   
                       hit.collider.GetComponent<InteractableItem>().isTrigger=true;
                       if( timeOverItem>1.5f){
                        hit.collider.GetComponent<InteractableItem>().showCanvas();
                       }
                   }else if(hit.collider.tag=="LeftArrow"){

                        Debug.LogError("left arrow ");
                        Camera.main.GetComponent<RotateCamera>().rotate(-90f);

                   } 
                   else if(hit.collider.tag=="RightArrow"){

                        Debug.LogError("right arrow ");
                        Camera.main.GetComponent<RotateCamera>().rotate(90f);

                   }else{
                     timeOverItem=0;
                     if(lastItem!=null){
                     lastItem.hideCanvas();
                       lastItem.isTrigger=false;
                     }
                     lastItem=null;
                   }
            }else
            {

              lineRenderer.positionCount = 0;
            }
         
         
         /*   raySelector.transform.localPosition=thumbTipPosition;
           if(filteredHandDirection.magnitude>float.Epsilon)
              raySelector.transform.localRotation=Quaternion.LookRotation(filteredHandDirection);
              */    
                
             elapsedTime=0;
 
            
         }

       previousHandDirection=filteredHandDirection;
                  
           
    }
                     

                       
                
              


      void OnCollisionEnter(Collision collision)
    {

      print(collision.gameObject.transform.name);
        Debug.DrawRay(collision.contacts[0].point, collision.contacts[0].normal, Color.green, 2, false);
    }
  
    
    private  IEnumerator itemSelectedChangeColor(Transform t){


        
          t.GetComponent<Renderer>().material.SetColor("_Color", Color.black);
          yield return new WaitForSeconds(1);
          t.GetComponent<Renderer>().material.SetColor("_Color", Color.white);
          
              }
}
