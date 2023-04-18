using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayselectorPointerObject : MonoBehaviour
{
    // Start is called before the first frame update

     
     public Transform startPositionTransform;

     float oldScale=1.0f;
     float newScale;
     Vector3 newScaleTMP=new Vector3();
    void Start()
    {
     
    
      
    }

    // Update is called once per frame
    void Update()
    {
        
    }

        private void OnTriggerEnter(Collider other)
    {
//raySelector.transform.localRotation=Quaternion.LookRotation(other.transform.position);
       print("scale");
       Vector3 startPosition=startPositionTransform.transform.position;
       Vector3 endPosition=other.transform.position;
       float result=Vector3.Distance(endPosition,startPosition);
          
       
        Vector3 parentLocalScale=  startPositionTransform.transform.parent.transform.localScale;
    
            float newZScale = result ;
         
   
       newScaleTMP=new Vector3(parentLocalScale.x,parentLocalScale.y, newZScale);
     //  Debug.Log(other.gameObject.name +" distante" +result+" ATTUALE " + parentLocalScale  +" NUOVO" +newScale);
         

         

    }
    private void OnTriggerStay(Collider other){
             startPositionTransform.transform.parent.transform.localScale = newScaleTMP;
    }


     private void OnTriggerExit(Collider other){


        Vector3 parentLocalScale=  startPositionTransform.transform.parent.transform.localScale;
       newScaleTMP = new Vector3(parentLocalScale.x,parentLocalScale.y, 1);
       StartCoroutine("changeSizeAfter");
       

     }

     private IEnumerable changeSizeAfter(){
        
            yield return new WaitForSeconds(0.5f);
             startPositionTransform.transform.parent.transform.localScale = newScaleTMP;
     }
}
