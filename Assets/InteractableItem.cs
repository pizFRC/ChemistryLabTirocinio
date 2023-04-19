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
   public bool isTrigger= false;
   public GameObject localCanvas;
   GameObject instance;
   bool canvasNotActive=true;


    void Start()
    {
    var renderers=this.gameObject.GetComponents<Renderer>();
    print("renderer:"+renderers);
    Transform objTransform=this.transform;
    instance=Instantiate(localCanvas);
    instance.transform.position=new Vector3(objTransform.position.x,objTransform.position.y+1,objTransform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
      
      //localCanvas.transform.LookAt(player.transform);
    }
    public void setTrigger(bool value){
      isTrigger=value;
    }
   public void interact(string str){
      print("stai interagendo con "+ this.gameObject.name +" : "+str);
      if(str == "stay"){

        // openCanvas(true);

      }
      
   }
     public void openCanvas(bool value){
     instance.SetActive(value);
    }

     public void changeMaterial(bool selected_material){
                var renderer = this.gameObject.GetComponents<Renderer>();
             foreach(Renderer r in renderer){
               
                if (selected_material){
                r.material=selected;
                }else{
                     r.material=original;
                }
             }
        }
    //private bool isGrabbed=false;
/*
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag== "Rayselector"){
       RayselectorPointerObject rpo=other.gameObject.GetComponent<RayselectorPointerObject>();
        if( rpo.numberOfCollider<rpo.maxCollider){
                 print("triggered :"+this.gameObject +" from : "+other.gameObject);
          

        }else{
         Debug.LogError("numero di collider max raggiunti-> ENTER");
        }

        }
      // StartCoroutine("changeSize");
    }
    private void OnTriggerExit(Collider other)
    {

       if(other.gameObject.tag== "Rayselector"){
        print("EXSIT :"+this.gameObject +" from : "+other.gameObject);
     changeMaterial(false);
     localCanvas.gameObject.SetActive(false);

       }
    }
    private void OnTriggerStay(Collider other)
    {
     
     
      
      if(other.transform.tag== "Rayselector"){
         
         timeElapsed+=Time.deltaTime;
             
       
        if(timeElapsed>2.0f){
    RayselectorPointerObject rpo=other.gameObject.GetComponent<RayselectorPointerObject>();
        if( rpo.numberOfCollider<rpo.maxCollider){
         
         localCanvas.gameObject.SetActive(true);
         print("passati due secondi"+ other.transform.name);
        
         timeElapsed=0f;
            print("il collider si avvia lato oggetto");
               changeMaterial(true);
        }else{
         Debug.LogError("numero di collider max raggiunti-> STAY");
         timeElapsed=0f;
         return;
        }

         }
        }
       // print("STAY :"+this.gameObject +" from : "+other.gameObject);
       
    }
       

        private IEnumerator changeSize(){
            this.GetComponent<Transform>().localScale*=1.0f;
             yield return new WaitForSeconds(1.0f);
                         this.GetComponent<Transform>().localScale/=1.0f;
        }

 
    */
   
}
