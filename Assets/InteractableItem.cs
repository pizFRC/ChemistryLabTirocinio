using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableItem : MonoBehaviour
{
    // Start is called before the first frame update
   public Material original;
   public Material selected;

   public Transform player;
   Transform canvasLocalPosition;
   float timeElapsed=0f;
   public Canvas localCanvas;
   
    void Start()
    {
    var renderers=this.gameObject.GetComponents<Renderer>();
    print("renderer:"+renderers);
    Transform objTransform=this.transform;
    localCanvas.transform.position=new Vector3(objTransform.position.x,objTransform.position.y+1,objTransform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
      //localCanvas.transform.LookAt(player.transform);
    }
    //private bool isGrabbed=false;

    private void OnTriggerEnter(Collider other)
    {
        
        print("triggered :"+this.gameObject +" from : "+other.gameObject);
     changeMaterial(true);
      // StartCoroutine("changeSize");
    }
    private void OnTriggerExit(Collider other)
    {
        print("EXSIT :"+this.gameObject +" from : "+other.gameObject);
     changeMaterial(false);
     localCanvas.gameObject.SetActive(false);
    }
    private void OnTriggerStay(Collider other)
    {
     
     
      
      if(other.transform.tag== "Rayselector"){
         
         timeElapsed+=Time.deltaTime;
       
       
        if(timeElapsed>2.0f){
         localCanvas.gameObject.SetActive(true);
         print("passati due secondi"+ other.transform.name);
        
         timeElapsed=0f;

         }
        }
       // print("STAY :"+this.gameObject +" from : "+other.gameObject);
       
    }
        void changeMaterial(bool selected_material){
                var renderer = this.gameObject.GetComponents<Renderer>();
             foreach(Renderer r in renderer){
                print(r.material);
                if (selected_material){
                r.material=selected;
                }else{
                     r.material=original;
                }
             }
        }

        private IEnumerator changeSize(){
            this.GetComponent<Transform>().localScale*=1.0f;
             yield return new WaitForSeconds(1.0f);
                         this.GetComponent<Transform>().localScale/=1.0f;
        }

 
    
   
}
