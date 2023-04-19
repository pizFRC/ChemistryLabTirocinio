using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayselectorPointerObject : MonoBehaviour
{
    // Start is called before the first frame update

     public int numberOfCollider;
     public int maxCollider=2;
     float timeElapsed=0;
    // Collider[] colliders2;
    void Start()
    {
     
       numberOfCollider=0;
      
    }

    // Update is called once per frame
    void Update()
    {
        
    }

        private void OnTriggerEnter(Collider other)
    {
//raySelector.transform.localRotation=Quaternion.LookRotation(other.transform.position);

                if(other.gameObject.tag == "Item")
                numberOfCollider++;
                else return;
                if(other.gameObject.tag == "Item" && numberOfCollider <= maxCollider){
                       
                        print(other.tag +" ha attivato il trigger num"+numberOfCollider);
                        other.gameObject.GetComponent<InteractableItem>().interact("enter");
                        other.gameObject.GetComponent<InteractableItem>().changeMaterial(true);
                       other.gameObject.GetComponent<InteractableItem>().setTrigger(true);
                }

    }

   

    private void OnTriggerStay(Collider other){
           //  startPositionTransform.transform.parent.transform.localScale = newScaleTMP;
    //print(other.transform.name);
     timeElapsed+=Time.deltaTime;
    if(other.gameObject.tag == "Item" && numberOfCollider <= maxCollider){
      
         // print(other.gameObject.tag+" on trigger stay il trigger num"+numberOfCollider);
          other.gameObject.GetComponent<InteractableItem>().interact("stay");
     
          print("number of colliders "+numberOfCollider);
           timeElapsed+=Time.deltaTime;
             
       
         if(timeElapsed>2.0f){
   other.gameObject.GetComponent<InteractableItem>().openCanvas(true);
  
  

               timeElapsed=0; 
        }
        

   } 
    
    }


     private void OnTriggerExit(Collider other){


                 if(numberOfCollider > 0 && other.gameObject.tag == "Item")
                        numberOfCollider--;
                        else
                        return;
             if(other.gameObject.tag == "Item" ){
               
                     if(    other.gameObject.GetComponent<InteractableItem>().isTrigger){
                        other.gameObject.GetComponent<InteractableItem>().changeMaterial(false);
                          other.gameObject.GetComponent<InteractableItem>().setTrigger(false);
            other.gameObject.GetComponent<InteractableItem>().openCanvas(false);
                     }
                }

 
       

     }

     private IEnumerator resetAfterOpen(){
        
            yield return new WaitForSeconds(0.5f);
           //  startPositionTransform.transform.parent.transform.localScale = newScaleTMP;
     }
}
