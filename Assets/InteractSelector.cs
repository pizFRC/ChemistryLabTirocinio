using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractSelector : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //private bool isGrabbed=false;

    private void OnTriggerEnter(Collider other)
    {
        print("triggered :"+this.gameObject +" from : "+other.gameObject);
         selectedItem();
    }
    private void OnTriggerExit(Collider other)
    {
        print("EXSIT :"+this.gameObject +" from : "+other.gameObject);
       
    }

 
        private IEnumerator selectedItem(){
            print("caroutine");
                 yield  return new WaitForSeconds(0.5f);           
             foreach(Renderer m in this.GetComponents<Renderer>()){
            
                 m.material.SetColor("Color", Color.red);
            
             }

            
        }
   
}
