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
    public InteractableItem selectedLeftHandObject;
    public InteractableItem selectedRightHandObject;
    public string hand = "";
    private GameObject[] slots; // Array di slot vuoti
    public bool simulaGestureAfferraDestra = false;
    public bool simulaGestureRilasciaDestra = false;
    public bool simulaGestureAfferraSinistra = false;
    public bool simulaGestureRilasciaSinistra = false;
    private string lastGestureChecked = "";
    public Sprite imgTest;
    public Image selectedLeftHandItemUI;
    public Image selectedRightHandItemUI;

    private void Awake()
    {
        instance = this; // Inizializzazione del singleton
    }
    // Update is called once per frame
    public void simula_gestureDX_rilascia()
    {
        simulaGestureRilasciaDestra = true;
        simulaGestureAfferraDestra = false;

        selectedRightHandObject.setSelector(null);

    }
    public void simula_gestureDX_afferra()
    {
        simulaGestureRilasciaDestra = false;
        simulaGestureAfferraDestra = true;
    }
    public void simula_gestureSX_afferra()
    {
        simulaGestureRilasciaSinistra = false;
        simulaGestureAfferraSinistra = true;
    }

        public void simula_gestureSX_rilascia()
    {
        simulaGestureRilasciaSinistra = true;
        simulaGestureAfferraSinistra = false ;
    }


    public void setHandObject(InteractableItem itemSelectedFor2Seconds,string h)
    {
            if(h=="Left")
            selectedLeftHandObject = itemSelectedFor2Seconds;
            if(h=="Right")
            selectedRightHandObject = itemSelectedFor2Seconds;
            
        

    }
    void Update()
    {




     
        if (selectedRightHandObject != null )
        {

            if (simulaGestureRilasciaDestra && !simulaGestureAfferraDestra)
            {
                if (rightHandSelector.lastItemSelectedFor2Second != null)
                {

                    rightHandSelector.lastItemSelectedFor2Second.setSelector(null);
                    rightHandSelector.lastItemSelectedFor2Second.isSelected = false;
                    rightHandSelector.lastItemSelectedFor2Second = null;
                    Debug.LogError("reset -> Right"+simulaGestureRilasciaDestra +simulaGestureAfferraDestra);
                    rightHandSelector.reset();



                }
                if (!selectedRightHandObject.gameObject.activeInHierarchy)
                    selectedRightHandObject.gameObject.SetActive(true);

                selectedRightHandItemUI.sprite = null;
                selectedRightHandObject.isSelected = false;
                selectedRightHandObject = null;


                simulaGestureRilasciaDestra=false;
                simulaGestureAfferraDestra=false;

            }
            else if (!simulaGestureRilasciaDestra && simulaGestureAfferraDestra)
            {
                selectedRightHandObject=rightHandSelector.lastItemSelectedFor2Second;
                selectedRightHandItemUI.sprite = selectedRightHandObject.item.sprite;
                simulaGestureRilasciaDestra=false;
                simulaGestureAfferraDestra=false;
                selectedRightHandObject.isSelected = true;
                selectedRightHandObject.gameObject.SetActive(false);
            }
        }


        //mano SX
        if (selectedLeftHandObject != null )
        {
           

             if (simulaGestureRilasciaSinistra && !simulaGestureAfferraSinistra)
            {
                if (leftHandSelector.lastItemSelectedFor2Second != null)
                {

                    leftHandSelector.lastItemSelectedFor2Second.setSelector(null);
                    leftHandSelector.lastItemSelectedFor2Second.isSelected = false;
                    leftHandSelector.lastItemSelectedFor2Second = null;
                    leftHandSelector.reset();



                }
                if (!selectedLeftHandObject.gameObject.activeInHierarchy)
                    selectedLeftHandObject.gameObject.SetActive(true);

                selectedLeftHandItemUI.sprite = null;
                selectedLeftHandObject.isSelected = false;
                selectedLeftHandObject = null;

            }
            else if (!simulaGestureRilasciaSinistra && simulaGestureAfferraSinistra)
            {
                selectedLeftHandObject=leftHandSelector.lastItemSelectedFor2Second;
                selectedLeftHandItemUI.sprite = selectedLeftHandObject.item.sprite;
                selectedLeftHandObject.gameObject.SetActive(false);
                 simulaGestureRilasciaSinistra=false;
                simulaGestureAfferraSinistra=false;      
                          selectedLeftHandObject.isSelected = true;
            }



            simulaGestureRilasciaSinistra=false;
                simulaGestureAfferraSinistra=false;
        }


        


    }

public void lockGesture(string hand){
    if(hand=="Right"){
        simulaGestureRilasciaDestra=false;
        simulaGestureAfferraDestra=false;
        return;
    
    }
     simulaGestureRilasciaSinistra=false;
        simulaGestureAfferraSinistra=false;

}


}
