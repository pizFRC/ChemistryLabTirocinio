using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractableEmptySpace : MonoBehaviour
{
    // Start is called before the first frame update
    public bool containsObject = false;

    public GameObject objectContained;
    public bool isPointed = false;

    public Transform centerDown;
    public float timer;
    public RaycastItemSelector ris;
  
    public bool  canInsertObject=false;
        void Start()
    {
        
    }



    private bool updateSlider(float value)
    {
        if (ris == null)
            return false;

        string gameEvent = "";
        if (ris.hand == "Left")
            gameEvent = GameEvents.LEFT_SLIDER_CHANGE;
        if (ris.hand == "Right")
            gameEvent = GameEvents.RIGHT_SLIDER_CHANGE;
        Messenger<float>.Broadcast(gameEvent, value);

        return true;
    }
    private bool activeSlider(bool value)
    {
        if (ris == null)
            return false;

        string gameEvent = "";
        if (ris.hand == "Left")
            gameEvent = GameEvents.LEFT_SLIDER_ACTIVE;
        if (ris.hand == "Right")
            gameEvent = GameEvents.RIGHT_SLIDER_ACTIVE;

        Messenger<bool>.Broadcast(gameEvent, value);

        return true;
    }
   
    // Update is called once per frame
    void Update()
    {
        

        if (!isPointed)
        {
            timer = 0.0f;
           
              updateSlider(0);
               activeSlider(false);
               this.ris=null;
          
        }
        if (isPointed )
        {
            
            //   changeImageSlider(ris.lastItemSelected.item.sprite);
            
            timer += Time.deltaTime;
            updateSlider(timer);
            activeSlider(true);
            if (timer >= 2.0f)
            {

                timer = 2.0f;
                //posiziona oggetto
               
                InserObjectInEmptySpace();
               timer=0f;
               Debug.LogError("POST INSERT");
               updateSlider(timer);
               activeSlider(false);
                isPointed=false;               
             return;
            }
            //   canvasLocal.GetComponentInChildren<Slider>().value=timer;
            
        
        }

       
      
       
    }

    void OnDisable(){
        isPointed=false;
        timer=0f;
        ris=null;
    }

    public void InserObjectInEmptySpace(){
        if(ris==null || ris.lastInteractableItemSelected==null)
            return;

        
       LabController.instance.changePosition(this.transform,ris.lastInteractableItemSelected.transform);
        this.isPointed=false;
        ris.lastInteractableItemSelected.SetWasGrabbed(false);
        
        
       ris.ResetLastItemSelected();
       ris.ResetLastItemHitten();
        
        ris.mode=selectorMode.CanSelect;
        this.ris=null;
  


      
    }
    
}
