using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour
{
    // Start is called before the first frame update
   public static HandController instance; // Riferimento al singleton

    public RaycastItemSelector leftHandSelector; // Riferimento al RaycastSelector per la mano sinistra
    public RaycastItemSelector rightHandSelector; // Riferimento al RaycastSelector per la mano destra

    private InteractableItem leftHandObject; // Oggetto tenuto dalla mano sinistra
    private InteractableItem rightHandObject; // Oggetto tenuto dalla mano destra

    private GameObject[] slots; // Array di slot vuoti
    public bool simulaGestureDestra=false;
    private void Awake()
    {
        instance = this; // Inizializzazione del singleton
    }
    // Update is called once per frame
    void Update()
    {

        if(simulaGestureDestra){
            Debug.Log(" gesture");
                 leftHandSelector.canRaycast=true;
                rightHandSelector.lastItemSelectedFor2Second.isSelected=false;
                rightHandSelector.lastItemSelectedFor2Second =null;
                simulaGestureDestra=false;
        }
        if(leftHandSelector.lastItemSelectedFor2Second!= null && leftHandObject==null)
        {
          //  Debug.LogError("HAI SELEZIONATO UN OGGETTO CON LA KMANO SINISTRA ORA DEVI USARE UNA GESTURE");
            rightHandSelector.canRaycast=false;
        }

         if(rightHandSelector.lastItemSelectedFor2Second!= null && rightHandObject==null)
        {
           // Debug.LogError("HAI SELEZIONATO UN OGGETTO CON LA KMANO DESTRA ORA DEVI USARE UNA GESTURE");
            leftHandSelector.canRaycast=false;
        }
    }
    public void setHandItem(InteractableItem item,string hand){
            if(hand=="r"){
                rightHandObject=item;
            }else{
                leftHandObject=item;
            }
    }
}
