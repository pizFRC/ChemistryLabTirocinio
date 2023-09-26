using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Gesture{
    GrabRight = 0,
    ReleaseRight = 1 ,

    GrabLeft = 2,
    ReleaseLeft = 3 ,


    
}

public class HandController : MonoBehaviour
{


    public RaycastItemSelector leftHandSelector; // Riferimento al RaycastSelector per la mano sinistra
    public RaycastItemSelector rightHandSelector;

    
    public InteractableItem selectedRightHandObject;






    // Start is called before the first frame update
    public static HandController instance; // Riferimento al singleton







    private void Awake()
    {
        instance = this; // Inizializzazione del singleton

        Messenger<Gesture>.AddListener(GameEvents.GRAB,GrabGesture);
    }
    private void OnDestroy(){
          Messenger<Gesture>.RemoveListener(GameEvents.GRAB,GrabGesture);
    }
    // Update is called once per frame
   

    public void GrabGesture(Gesture gesture){


            switch(gesture){
            case Gesture.GrabRight:
            //se non ho selezionato nulla non posso grabbare
          
                if(CheckItemInRightHand())
                    return;
                  Debug.LogError("grab rigt");
                //SALVO L'OGGETTO COME PRESO IN MANO
                Sprite spriteLastItemTaken=rightHandSelector.lastInteractableItemSelected.item.sprite;
                Messenger<Sprite>.Broadcast(GameEvents.RIGHT_ITEM_IMAGE_CHANGE,spriteLastItemTaken);
                rightHandSelector.lastInteractableItemSelected.SetWasGrabbed(true);
                rightHandSelector.mode=selectorMode.CannotSelect;
                break;

            case Gesture.GrabLeft:
              if(CheckItemInLeftHand())
                    return;
                  Debug.LogError("grab rigt");
                //SALVO L'OGGETTO COME PRESO IN MANO
                Sprite spriteLastItemTakenLeft=leftHandSelector.lastInteractableItemSelected.item.sprite;
                Messenger<Sprite>.Broadcast(GameEvents.LEFT_ITEM_IMAGE_CHANGE,spriteLastItemTakenLeft);
                leftHandSelector.lastInteractableItemSelected.SetWasGrabbed(true);
                leftHandSelector.mode=selectorMode.CannotSelect;
                break;
            }
    }
    
    void Update()
    {

    }

private bool CheckItemInRightHand(){
   
    return rightHandSelector.lastInteractableItemSelected==null;
}


private bool CheckItemInLeftHand(){
   
    return leftHandSelector.lastInteractableItemSelected==null;
}


    
   

    
    }



