using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Gesture
{
    GrabRight = 0,
    ReleaseRight = 1,

    GrabLeft = 2,
    ReleaseLeft = 3,

    InteractRight=4,
    InteractLeft=5

    ,NoneRight=6,
    NoneLeft=7,
    OpenPalmRight=8,
    OpenPalmLeft=9,



}

public class HandController : MonoBehaviour
{




    public RaycastItemSelector leftHandSelector; // Riferimento al RaycastSelector per la mano sinistra
    public RaycastItemSelector rightHandSelector;

    public int raycastGraph=0;

    // Start is called before the first frame update
    public static HandController instance; // Riferimento al singleton


    private bool isModalOpen;
    public bool modalGestureIsOpen;


    void Start(){
        isModalOpen=false;
    }

    private void Awake()
    {
        instance = this; // Inizializzazione del singleton
        DontDestroyOnLoad(this.gameObject);
        Messenger<Gesture>.AddListener(GameEvents.GRAB, GrabGesture);

        Messenger<Gesture>.AddListener(GameEvents.RELEASE, ReleaseGesture);
        Messenger<Gesture>.AddListener(GameEvents.INTERACT, InteractGesture);
       

    }

    void InteractGesture(Gesture gesture){
        return;
        print("interact");
  switch (gesture)
        {
            case Gesture.InteractLeft:
            if(leftHandSelector.lastInteractableItemSelected==null)
            return;
            if(leftHandSelector.lastInteractableItemSelected.TryGetComponent(out Contenitore contenitore)){
                    contenitore.clear();
            }

            if(leftHandSelector.lastInteractableItemSelected.TryGetComponent(out SecurityTool securityTool)){
                   securityTool.SetIsPointed(true);
            }
            break;

            case Gesture.InteractRight:
            if(rightHandSelector.lastInteractableItemSelected==null)
            return;
            if(rightHandSelector.lastInteractableItemSelected.TryGetComponent(out Contenitore contenitoreRight)){
                    contenitoreRight.clear();
            }

            if(rightHandSelector.lastInteractableItemSelected.TryGetComponent(out SecurityTool securityToolRight)){
                    securityToolRight.SetIsPointed(true);
            }
            break;

            
        }
        
    }
    private void OnDestroy()
    {
        Messenger<Gesture>.RemoveListener(GameEvents.GRAB, GrabGesture);
        Messenger<Gesture>.RemoveListener(GameEvents.RELEASE, ReleaseGesture);

        Messenger<Gesture>.RemoveListener(GameEvents.INTERACT, InteractGesture);
         
    }
    // Update is called once per frame
   

    public void GrabGesture(Gesture gesture)
    {


        switch (gesture)
        {
            case Gesture.GrabRight:
                //se non ho selezionato nulla non posso grabbare

                if (CheckItemInRightHand()){
                  
                    return;
                }

                Messenger<bool>.Broadcast(GameEvents.DISPLAY_GESTURE_PANEL,false);
                Debug.LogAssertion("entrato nell grab");


                rightHandSelector.mode = selectorMode.MoveItem;
                  UIController.instance.activeRaycast();
      //     /*      UnityMainThreadDispatcher.Instance().Enqueue(() => */ rightHandSelector.lastInteractableItemSelected.instanceOfItem.SetActive(false);
                    rightHandSelector.lastInteractableItemSelected.SetWasGrabbed(true);
                  //  rightHandSelector.lastInteractableItemSelected.SetIsPointed(false);

                Sprite spriteLastItemTaken = rightHandSelector.lastInteractableItemSelected.item.sprite;
                Messenger<Sprite>.Broadcast(GameEvents.RIGHT_ITEM_IMAGE_CHANGE, spriteLastItemTaken);



                break;


            case Gesture.GrabLeft:
                if (CheckItemInLeftHand())
                    return;
                
                  Messenger<bool>.Broadcast(GameEvents.DISPLAY_GESTURE_PANEL,false);
                leftHandSelector.mode = selectorMode.MoveItem;
                  UIController.instance.activeRaycast();
                leftHandSelector.lastInteractableItemSelected.SetWasGrabbed(true);
              ///*   UnityMainThreadDispatcher.Instance().Enqueue(() =>  */leftHandSelector.lastInteractableItemSelected.instanceOfItem.SetActive(false);

                Sprite spriteLastItemTakenLeft = leftHandSelector.lastInteractableItemSelected.item.sprite;
                Messenger<Sprite>.Broadcast(GameEvents.LEFT_ITEM_IMAGE_CHANGE, spriteLastItemTakenLeft);

                break;



        }
    }
    public void ReleaseGesture(Gesture gesture)
    {
        switch (gesture)
        {
            case Gesture.ReleaseRight:
            Debug.LogError("pre");
                if (CheckItemInRightHand()){
                    Debug.Log("chek item right hand ");
                    return;

                }


                  Messenger<bool>.Broadcast(GameEvents.DISPLAY_GESTURE_PANEL,false);
                UIController.instance.activeRaycast();
                rightHandSelector.ResetLastItemSelected();
               
                Debug.LogAssertion("release dx");
            //  UnityMainThreadDispatcher.Instance().Enqueue(() =>  rightHandSelector.lastInteractableItemSelected.instanceOfItem.SetActive(true));
               
                Messenger<Sprite>.Broadcast(GameEvents.RIGHT_ITEM_IMAGE_CHANGE, null);

                 rightHandSelector.mode = selectorMode.CanSelect;
                //    UnityMainThreadDispatcher.Instance().Enqueue(() => rightHandSelector.ResetLastItemSelected());
                break;


            case Gesture.ReleaseLeft:
                if (CheckItemInLeftHand()){
                    Debug.Log("chek item left hand ");
                    return;

                }


                  Messenger<bool>.Broadcast(GameEvents.DISPLAY_GESTURE_PANEL,false);
                Debug.LogAssertion("relase sx");
                 UIController.instance.activeRaycast();
                   leftHandSelector.ResetLastItemSelected();
                
                Messenger<Sprite>.Broadcast(GameEvents.LEFT_ITEM_IMAGE_CHANGE, null);
              //  UnityMainThreadDispatcher.Instance().Enqueue(() =>  leftHandSelector.lastInteractableItemSelected.instanceOfItem.SetActive(true));
                // UnityMainThreadDispatcher.Instance().Enqueue(() => leftHandSelector.ResetLastItemSelected());
                leftHandSelector.mode = selectorMode.CanSelect;
                break;
        }
    }

    public void LockOneRaycast(RaycastItemSelector ris){
        if(ris==leftHandSelector)
            {
                if(rightHandSelector.mode!=selectorMode.MoveItem)
                    rightHandSelector.mode=selectorMode.CannotSelect;
            }else{
                if(leftHandSelector.mode!=selectorMode.MoveItem)
                    leftHandSelector.mode=selectorMode.CannotSelect;
            }
            ris.mode=selectorMode.LockOnItem;
    }
    private bool CheckItemInRightHand()
    {

        return rightHandSelector.lastInteractableItemSelected == null;
    }


    private bool CheckItemInLeftHand()
    {

        return leftHandSelector.lastInteractableItemSelected == null;
    }


    



}



