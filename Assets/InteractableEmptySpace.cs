using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractableEmptySpace : MonoBehaviour
{
    // Start is called before the first frame update
    public bool containsObject=false;

    public GameObject objectContained;
    public bool isPointed=false;
     GameObject canvasLocal;
    public GameObject prefabCanvas;
    public Transform centerDown;
    public float timer;
    public float timerContained;
    public int rayNumber=0;
    public RaycastItemSelector ris;

    bool stop=false;
    void Start()
    {
        canvasLocal=Instantiate(prefabCanvas);
        canvasLocal.SetActive(false);
    canvasLocal.transform.SetParent(this.gameObject.transform);
    canvasLocal.transform.position=new Vector3(this.transform.position.x,this.transform.position.y+0.8f,this.transform.position.z);
    }



 private bool updateSlider(float value){
        if(ris == null )
            return false;

        string gameEvent="";
        if(ris.hand=="Left")
            gameEvent=GameEvents.LEFT_SLIDER_CHANGE;
        if(ris.hand=="Right")
            gameEvent=GameEvents.RIGHT_SLIDER_CHANGE;
        Messenger<float>.Broadcast(gameEvent,value);
         
        return true;
    }
 private bool activeSlider(bool value){
        if(ris == null )
            return false;
          
        string gameEvent="";
        if(ris.hand=="Left")
            gameEvent=GameEvents.LEFT_SLIDER_ACTIVE;
        if(ris.hand=="Right")
            gameEvent=GameEvents.RIGHT_SLIDER_ACTIVE;
        Messenger<bool>.Broadcast(gameEvent,value);
         
        return true;
    }
private bool changeImageSlider(gestureIndex value){
    if(ris == null )
            return false;
          
        string gameEvent="";
        if(ris.hand=="Left")
            gameEvent=GameEvents.LEFT_SLIDER_IMAGE_CHANGE;
        if(ris.hand=="Right")
            gameEvent=GameEvents.RIGHT_SLIDER_IMAGE_CHANGE;
        Messenger<gestureIndex>.Broadcast(gameEvent,value);
         
        return true;

}
    // Update is called once per frame
    void Update()
    {
        if(stop)
            return;
        if(isPointed && !containsObject){
         changeImageSlider(gestureIndex.closed);
             activeSlider(true);
            timer+=Time.deltaTime;

            if(timer>2.0f){
                
                timer=2.0f;
            }
         //   canvasLocal.GetComponentInChildren<Slider>().value=timer;
            updateSlider(timer);

        }else  if(isPointed && containsObject ){
          
            changeImageSlider(gestureIndex.victory);
            
            timerContained+=Time.deltaTime;

            if(timerContained>2.0f){
               
                timerContained=2.0f;
                  updateSlider(timerContained);
                stop=true;
               activeSlider(false);
            }
          
            
            }else{
                
                
           
                 changeImageSlider(gestureIndex.victory);
                 updateSlider(0);
                 activeSlider(false);
              
            
                timer=0;
                timerContained=0;
                


        }
    }


    public bool putObject(RaycastItemSelector ris,GameObject obj){
        if(this.timer<2.0f)
            return false;
        if(containsObject)
            return false;
       
        
       
        objectContained=obj;
        containsObject=true;
        Debug.Log("put go"+obj);
        this.ris=ris;
        Transform pos= this.GetComponentInParent<Transform>();
        objectContained =Instantiate(obj,pos.position,pos.rotation);
        this.ris.lastItemSelectedFor2Second=null;
        objectContained.transform.rotation=obj.transform.rotation;
        objectContained.transform.localScale-=new Vector3(0.2f,0.2f,0.2f);
        objectContained.transform.position = centerDown.position;
        objectContained.transform.SetParent(this.gameObject.transform);

        if(!objectContained.activeInHierarchy){
            objectContained.SetActive(true);
        }
        if(objectContained.GetComponentInChildren<Canvas>().gameObject.activeInHierarchy){
            objectContained.GetComponentInChildren<Canvas>().gameObject.SetActive(false);
        }

      
        Color newTransparentColor= this.GetComponent<Renderer>().material.color;
        newTransparentColor.a=0.1f;
        this.GetComponent<Renderer>().material.color=newTransparentColor;
        
        
        return containsObject;
    }
}
