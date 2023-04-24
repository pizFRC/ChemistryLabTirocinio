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
    private InteractableItem selectedHandObject; // Oggetto tenuto dalla mano destra

    private GameObject[] slots; // Array di slot vuoti
    public bool simulaGestureRilascia=false;
    private string lastGestureChecked="";
    public Sprite imgTest;
    public Image selectedHandItwmUI;
    private void Awake()
    {
        instance = this; // Inizializzazione del singleton
    }
    // Update is called once per frame
    void Update()
    {

        if(simulaGestureRilascia){
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
            selectedHandObject=null;
            selectedHandItwmUI.sprite=null;
            return;
        }
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
        }
    }
    
}
