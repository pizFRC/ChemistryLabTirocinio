using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayselectorPointerObject : MonoBehaviour
{
    // Start is called before the first frame update

     
    // Collider[] colliders2;
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
     

    }

   

    private void OnTriggerStay(Collider other){
           //  startPositionTransform.transform.parent.transform.localScale = newScaleTMP;
    //print(other.transform.name);

    
    }


     private void OnTriggerExit(Collider other){


 
       

     }

     private IEnumerable changeSizeAfter(){
        
            yield return new WaitForSeconds(0.5f);
           //  startPositionTransform.transform.parent.transform.localScale = newScaleTMP;
     }
}
