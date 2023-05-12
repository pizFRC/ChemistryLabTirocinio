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
  
    public Transform centerDown;
    public float timer;
    public float timerContained;
    public int rayNumber=0;
    public RaycastItemSelector ris;

    bool stop=false;
    void Start()
    {
        
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
       
        
       
        print(obj);
       
        this.ris=ris;
        Transform parentTransform= this.GetComponentInParent<Transform>();
        
       
        string hand=ris.hand.ToUpper();
        string evento=hand+"_ITEM_IMAGE_CHANGE";
        Debug.LogError(evento);
        Messenger<Sprite>.Broadcast(evento,ris.lastItemSelectedFor2Second.item.sprite);
        objectContained=obj;
        containsObject=true;
        
        objectContained =Instantiate(obj,parentTransform.position,parentTransform.rotation);
        objectContained.transform.rotation=obj.transform.rotation;
       // objectContained.transform.localScale=new Vector3(0.7f,0.7f,0.7f);
       
        objectContained.transform.position = centerDown.position;
        objectContained.transform.SetParent(this.gameObject.transform);
        

/*if(ris.hand=="Right"){
    HandController.instance.selectedRightHandObject=null;

 HandController.instance.selectedRightHandItemUI=null;
}*/
    
        if(ris.hand=="Left"){
    HandController.instance.selectedLeftHandObject=null;
        HandController.instance.selectedLeftHandItemUI=null;
        
        
        
        }
        
        if(!objectContained.activeInHierarchy){
            objectContained.SetActive(true);
        }
        

      
        Color newTransparentColor= this.GetComponent<Renderer>().material.color;
        newTransparentColor.a=0.1f;
        this.GetComponent<Renderer>().material.color=newTransparentColor;
        
        
        return containsObject;
    }
}
