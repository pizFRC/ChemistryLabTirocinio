using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayselectorPointerObject : MonoBehaviour
{
    // Start is called before the first frame update

     public int numberOfCollider;
     public int maxCollider=2;
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

                if(other.gameObject.tag == "Item" && numberOfCollider < maxCollider)
                numberOfCollider++;
                if(other.gameObject.tag == "Item" && numberOfCollider < maxCollider){
                       
                        print(other.tag +" ha attivato il trigger num"+numberOfCollider);
                        other.gameObject.GetComponent<InteractableItem>().interact("enter");
                        other.gameObject.GetComponent<InteractableItem>().changeMaterial();
                }else{
                        return;
                }

    }

   

    private void OnTriggerStay(Collider other){
           //  startPositionTransform.transform.parent.transform.localScale = newScaleTMP;
    //print(other.transform.name);
    if(other.gameObject.tag == "Item" && numberOfCollider < maxCollider){
         // print(other.gameObject.tag+" on trigger stay il trigger num"+numberOfCollider);
          other.gameObject.GetComponent<InteractableItem>().interact("stay");
          print("number of colliders "+numberOfCollider);
   } 
    
    }


     private void OnTriggerExit(Collider other){


                 if(numberOfCollider > 0 && other.gameObject.tag == "Item")
                        numberOfCollider--;
             if(other.gameObject.tag == "Item" ){
               
                         other.gameObject.GetComponent<InteractableItem>().changeMaterial();
                }

 
       

     }

     private IEnumerable changeSizeAfter(){
        
            yield return new WaitForSeconds(0.5f);
           //  startPositionTransform.transform.parent.transform.localScale = newScaleTMP;
     }
}
