
using UnityEngine;
using UnityEngine.Events;

public class InteractableItem : MonoBehaviour
{
    // Start is called before the first frame update
    public float localObjectTimer,timerTheshold=2.0f;
    public bool isPointed;

    
    private RaycastItemSelector raycastSelector;
    public Item item;


    public bool wasGrabbed=false;
    
    
    public bool GetWasGrabbed() { return wasGrabbed; }
    public void SetWasGrabbed(bool value) { wasGrabbed = value; }

    public bool GetIsPointed() { return isPointed; }
    public void SetIsPointed(bool value) { isPointed = value; }

    public RaycastItemSelector GetRaycastSelector() { return raycastSelector; }

    public void SetRaycastSelector(RaycastItemSelector raycastItemSelector) { raycastSelector = raycastItemSelector; }
    [SerializeField]public UnityEvent evento;
    
   
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
        
        if(wasGrabbed){
        
        this.isPointed=false;
        localObjectTimer=0f;
          UpdateUI(false,0f);
          
        return;

        }



       
         
        
        if(!isPointed){
            if(this.TryGetComponent(out Outline outline))
                outline.enabled=false;
            if(this.GetRaycastSelector()!=null){
              UpdateUI(false,0f);
              this.raycastSelector=null;

            }
          
         
              localObjectTimer=0;
             //Debug.Log("fuori if(isPointed): "+localObjectTimer);
             return;

        }

        if (isPointed)
        {

        if(this.TryGetComponent(out Outline outline))
                outline.enabled=true;

            localObjectTimer += Time.deltaTime;
            
            if(localObjectTimer>=timerTheshold){
                localObjectTimer=timerTheshold;
                
               
                  evento.Invoke();
                
                 UpdateUI(false,0f);
                 localObjectTimer=0;
                 isPointed=false;
                 return;
            }
            UpdateUI(true,this.localObjectTimer);
           
        }
        
       

    }

    void OnDisable(){
        isPointed=false;
        wasGrabbed=false;
        localObjectTimer=0f;
    }

public void setRaycastSelectorLockOnItem(){
            GetRaycastSelector().LockPointerOnItem();
}


     
public void visibile(bool value){
    this.transform.gameObject.SetActive(value);
}
    
    
    




}
