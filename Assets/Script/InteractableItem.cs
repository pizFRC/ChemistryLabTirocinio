using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using Unity.VisualScripting.ReorderableList;
using UnityEngine;
using UnityEngine.UI;

public class InteractableItem : MonoBehaviour
{
    // Start is called before the first frame update
    public float localObjectTimer;
    private bool isPointed;
    public Material original;
    public Material selected;
    private UIController uiControllerInstance;
    private RaycastItemSelector raycastSelector;
    public Item item;

    private bool wasGrabbed=false;
    
    public bool GetWasGrabbed() { return wasGrabbed; }
    public void SetWasGrabbed(bool value) { wasGrabbed = value; }

    public bool GetIsPointed() { return isPointed; }
    public void SetIsPointed(bool value) { isPointed = value; }

    public RaycastItemSelector GetRaycastSelector() { return raycastSelector; }

    public void SetRaycastSelector(RaycastItemSelector raycastItemSelector) { raycastSelector = raycastItemSelector; }


    


    void Start()
    {
        var renderers = this.gameObject.GetComponents<Renderer>();
      

    }

   
    private void UpdateUI(bool isSliderActive,float localTimer){
        if(this.GetRaycastSelector()== null)
            return;
        
        string eventNameActiveSlider=GameEvents.SLIDER_ACTIVE;
        string eventNameUpdateSlider=GameEvents.SLIDER_CHANGE;
        Messenger<bool>.Broadcast(this.GetRaycastSelector().hand.ToUpper()+eventNameActiveSlider, isSliderActive);
        Messenger<float>.Broadcast(this.GetRaycastSelector().hand.ToUpper()+eventNameUpdateSlider, localTimer);
    }

    // Update is called once per frame
    void Update()
    {
        

        if(!isPointed){
            localObjectTimer=0;
            if(this.GetRaycastSelector()!=null){
              string eventNameActiveSlider=GameEvents.SLIDER_ACTIVE;
              Messenger<bool>.Broadcast(this.GetRaycastSelector().hand.ToUpper()+eventNameActiveSlider, false);
            this.SetRaycastSelector(null);
            }
            UpdateUI(false,0f);
            
             //Debug.Log("fuori if(isPointed): "+localObjectTimer);
             return;

        }

        if (isPointed)
        {
            localObjectTimer += Time.deltaTime;

            if(localObjectTimer>=2.0f){
                localObjectTimer=2.0f;
                //TO-DO grab degli oggetti
                GetRaycastSelector().LockPointerOnItem(this);
            }
            UpdateUI(true,this.localObjectTimer);
           
        }
        
        if(wasGrabbed){
            this.gameObject.SetActive(false);
        }

    }



    public void changeMaterial(bool selected_material)
    {
        var renderer = this.gameObject.GetComponents<Renderer>();
        foreach (Renderer r in renderer)
        {

            if (selected_material)
            {
                r.material = selected;
            }
            else
            {
                r.material = original;
            }
        }
    }




}
