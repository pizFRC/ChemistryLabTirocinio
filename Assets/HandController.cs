using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HandController : MonoBehaviour
{
    // Start is called before the first frame update
   public static HandController instance; // Riferimento al singleton

    public RaycastItemSelector leftHandSelector; // Riferimento al RaycastSelector per la mano sinistra
    public RaycastItemSelector rightHandSelector; // Riferimento al RaycastSelector per la mano destra

    // Oggetto tenuto dalla mano sinistra
    public InteractableItem selectedHandObject; // Oggetto tenuto dalla mano destra
    public string hand="";
    private GameObject[] slots; // Array di slot vuoti
    public bool simulaGestureAfferra=false;
    public bool simulaGestureRilascia=false;
    private string lastGestureChecked="";
    public Sprite imgTest;
    public Image selectedHandItemUI;
    
    private void Awake()
    {
        instance = this; // Inizializzazione del singleton
    }
    // Update is called once per frame
    public void simula_gesture_rilascia(){
    simulaGestureRilascia=true;
    simulaGestureAfferra=false;
    }
    public void simula_gesture_afferra(){
    simulaGestureRilascia=false;
    simulaGestureAfferra=true;
    }


    public void setHandObject(InteractableItem itemSelectedFor2Seconds,string hand){
        if(this.hand==""){
            selectedHandObject=itemSelectedFor2Seconds;
            this.hand=hand;
        }
    }
    void Update()
    {
        
        if(selectedHandObject!=null && hand!=""){
            if(hand=="Left"){
                rightHandSelector.canRaycast=false;
            }else if(hand=="Right")
            {
                 leftHandSelector.canRaycast=false;
            }else{

            }
        if(simulaGestureRilascia && !simulaGestureAfferra){
            Debug.Log(" gesture");

            if(rightHandSelector.lastItemSelectedFor2Second !=null){
                 leftHandSelector.canRaycast=true;
                rightHandSelector.lastItemSelectedFor2Second.isSelected=false;
                rightHandSelector.lastItemSelectedFor2Second =null;
             
             
 
            }
            if(leftHandSelector.lastItemSelectedFor2Second!=null){
                rightHandSelector.canRaycast=true;
                leftHandSelector.lastItemSelectedFor2Second.isSelected=false;
                leftHandSelector.lastItemSelectedFor2Second =null;
                
                    
            }
            simulaGestureRilascia=false;
             if(!selectedHandObject.gameObject.activeInHierarchy)
                selectedHandObject.gameObject.SetActive(true);
            selectedHandObject.isSelected=false;
            selectedHandObject=null;
            selectedHandItemUI.sprite=null;
            hand="";
           
        }else if(simulaGestureAfferra && !simulaGestureRilascia){
              selectedHandItemUI.sprite=selectedHandObject.item.sprite;
              simulaGestureAfferra=false;
              //hand="";
              selectedHandObject.isSelected=true;
              selectedHandObject.gameObject.SetActive(false);

             
        }

        
        }
      /*

      //se ho selezionato qualcoasa con ilraycast sinistro per almeno due seocondi e l'oggetto selezionato non è nullo
        if(leftHandSelector.lastItemSelectedFor2Second!= null && selectedHandObject==null)
        {
          //  Debug.LogError("HAI SELEZIONATO UN OGGETTO CON LA KMANO SINISTRA ORA DEVI USARE UNA GESTURE");
            rightHandSelector.canRaycast=false;
            selectedHandObject=leftHandSelector.lastItemSelectedFor2Second;
             selectedHandItwmUI.sprite=selectedHandObject.item.sprite;
        }

         if(rightHandSelector.lastItemSelectedFor2Second!= null && selectedHandObject==null)
        {
           // Debug.LogError("HAI SELEZIONATO UN OGGETTO CON LA KMANO DESTRA ORA DEVI USARE UNA GESTURE");
            leftHandSelector.canRaycast=false;
             selectedHandObject=rightHandSelector.lastItemSelectedFor2Second;
            selectedHandItwmUI.sprite=selectedHandObject.item.sprite;
        }*/
    }
    
}
