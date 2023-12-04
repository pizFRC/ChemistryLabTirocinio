
using System.Collections.Generic;


using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public enum selectorMode
{
    CanSelect = 1,
    CannotSelect = 2,
    LockOnItem = 3,
    MoveItem = 4,
    LockOnEmptySpace = 5,
}

public class RaycastItemSelector : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] public selectorMode mode;
    public GameObject[] spheres;

    float elapsedTime = 0f;
    public string hand;
    public Material standard;
    public float timeToWait = 1.0f;
    public float maxDistance = 150f;
    private LineRenderer lineRenderer;
    public InteractableItem lastInteractableItemSelected;
    public  InteractableItem lastInteractableItemHitten;
    InteractableEmptySpace lastEmptySpacePointed;
    public InteractableItem lastItemSelectedFor2Second;

    public SecurityTool lastSecurityToolPointed;

    public float range = 250f;
    Vector3 itemPosition;
    public Vector3 relativePosition;
    public float adjustY;
    public float adjustX;
    public float smoothingFactor = 0.02f;
    Camera mainCamera;
    public float distanceToFront;
    public GameObject centerSphere;
    public Vector3 pixelPosition;
    public Vector3 worldPosition;
    float cameraWidth, cameraHeight;
    [SerializeField] GraphicRaycaster m_Raycaster;
    PointerEventData m_PointerEventData;
    [SerializeField] EventSystem m_EventSystem;
    [SerializeField] RectTransform canvasRect;
    public GameObject handModel;
    public Button lastButtonPointed;

    Vector3 previousHandDirection, thumbDirection, thumbTipPosition, wristPosition, filteredHandDirection;
    public bool otherPointer;
    private Vector3 midway;
    public bool canRaycast;


void Awake(){
    for(int i=0;i<this.transform.childCount;i++)
          DontDestroyOnLoad(  this.transform.GetChild(i));
      DontDestroyOnLoad(this);
}
    void Start()
    {
        lastButtonPointed = null;


        
        // Usa nearClipPlane come valore Z

        
    }


    void Update()
    {

        if(!handModel.activeSelf){
            if(this.lastButtonPointed!=null)
                this.lastButtonPointed.GetComponent<LongClick>().pressed=false;

            this.lastButtonPointed=null;


        }
 

        elapsedTime += Time.deltaTime;
       
        if (elapsedTime > timeToWait)
        {


            switch (mode)
            {
                case (selectorMode.CanSelect):
                    GetComponent<LineRenderer>().material = standard;
                    RaycastPointer();
                    break;
                case (selectorMode.CannotSelect):
                    GetComponent<LineRenderer>().positionCount = 0;
                    if (lastButtonPointed != null)
                        lastButtonPointed.GetComponent<LongClick>().pressed = false;
            
                    break;

                case (selectorMode.LockOnItem):
                    LockPointerOnItem();

                    break;
                case (selectorMode.MoveItem):
                        GetComponent<LineRenderer>().material.color=Color.red;
                        raycastMoveObject();
                    break;
                case (selectorMode.LockOnEmptySpace):
                    // uiGesture.SetActive(true);

                    thumbTipPosition = this.transform.GetChild(4).transform.position;
                    GetComponent<LineRenderer>().SetPosition(0, thumbTipPosition);
                    GetComponent<LineRenderer>().SetPosition(1, itemPosition);

                    break;






                default: break;
            }



            elapsedTime = 0;

            previousHandDirection = filteredHandDirection;


        }




    }

    //Dopo 2 secondi questa funzione viene richiamata per settare l'item selezionato e blocca la linea sull item
    public void LockPointerOnItem()
    {
        if (lastInteractableItemHitten == null)
        {

            return;

        }
       
        HandController.instance.LockOneRaycast(this);
        lastInteractableItemSelected = lastInteractableItemHitten;
        

       
        thumbTipPosition = this.transform.GetChild(4).transform.position;
        Vector3 itemPosition = lastInteractableItemSelected.transform.position;
        itemPosition.z += 0.3f;
        GetComponent<LineRenderer>().positionCount=2;
        GetComponent<LineRenderer>().SetPosition(0, thumbTipPosition);
        GetComponent<LineRenderer>().SetPosition(1, itemPosition);

        //aggiungere pannello per gesture
        Messenger<bool>.Broadcast(GameEvents.DISPLAY_GESTURE_PANEL,true);
        UIController.instance.setHandGesturePanel(this);
   
    }


    public void raycastMoveObject()
    {
        bool canDraw = true;
        Vector3 direction= GetHandDirection();
        if(direction==null)
        return;
        Ray ray = new Ray(thumbTipPosition, direction* range);

        RaycastHit hit;

        Vector3 endPosition = filteredHandDirection * range;
        if (Physics.Raycast(ray, out hit, 250f))
        {
             string colliderTag = hit.transform.tag;
            if(!colliderTag.Equals("EmptySpace"))
                ResetLastEmprySpacePointed();

            switch (colliderTag)
            {
                
                case("EmptySpace"):

                    InteractableEmptySpace lastEmptySpacePointedTMP = hit.transform.GetComponent<InteractableEmptySpace>();
                    if(this.lastEmptySpacePointed!=lastEmptySpacePointedTMP ){
                        ResetLastEmprySpacePointed();
                        this.lastEmptySpacePointed=lastEmptySpacePointedTMP;
                        }

                    lastEmptySpacePointed.ris = this;
                    lastEmptySpacePointed.isPointed=true;
                    break;

                case("Item"):
                    if( hit.collider.gameObject.TryGetComponent(out Contenitore contenitore)){
                    // contenitore.refill(this.lastInteractableItemSelected);
                        if(this.lastInteractableItemSelected.TryGetComponent(out Reagente r)){


                            if(contenitore.refill(r)){
                            this.ResetLastItemSelected();
                            this.ResetLastItemHitten();
                            this.mode=selectorMode.CanSelect;
                            }
                        }

                    }

                break;
                default:
                break;
            }
           

            draw(canDraw, hit.point);
            return;
        }

        print("nul space pointerd");
        if(lastEmptySpacePointed!=null){
            lastEmptySpacePointed.isPointed=false;
            lastEmptySpacePointed.ris=null;
        }
        lastEmptySpacePointed=null;

    }

    //disegna la linea che fa da puntaotre
    public void draw(bool canDraw, Vector3 point)
    {
        if (canDraw)
        {


            thumbTipPosition = spheres[4].transform.position;
            GetComponent<LineRenderer>().positionCount = 2;





            GetComponent<LineRenderer>().SetPosition(0, spheres[4].transform.position);

            GetComponent<LineRenderer>().SetPosition(1, point);
            Debug.DrawRay(spheres[4].transform.position, point, Color.red, 0.2f);
        
        }
        else
            GetComponent<LineRenderer>().positionCount = 0;

    }
    void graphRaycast(RaycastHit hit, Vector3 dir)
    {
        if (dir == Vector3.zero)
            return;
        if(this.mode==selectorMode.CannotSelect)
            return;
        m_PointerEventData = new PointerEventData(m_EventSystem);
        //Set the Pointer Event Position to that of the game object

        Vector2 point = new Vector2(Camera.main.WorldToScreenPoint(hit.point).x, Camera.main.WorldToScreenPoint(dir).y);

        Vector2 localPoint = Camera.main.WorldToScreenPoint(hit.point);




        m_PointerEventData.position = localPoint;


        List<RaycastResult> results = new List<RaycastResult>();

        //Raycast using the Graphics Raycaster and mouse click position
        m_Raycaster.Raycast(m_PointerEventData, results);
        bool btnPointed=false;
        
        if (results.Count > 0)
        {
        
            for (int i = 0; i < results.Count; i++)
                
                if (results[i].gameObject.TryGetComponent<Button>(out Button btn))
                {

                    if(btn.TryGetComponent(out LongClick lc)){
//                    Debug.LogError("Hit " + btn);

                    if (lastButtonPointed != null && lastButtonPointed != btn)
                        lastButtonPointed.GetComponent<LongClick>().pressed = false;
                    

                  //  btn.OnPointerClick(m_PointerEventData);
                    btn.GetComponent<LongClick>().pressed = true;
                    btn.GetComponent<LongClick>().pointerRaycastItemSelector=this;
                    lastButtonPointed = btn;
                    btnPointed=true;

                    }
                    
                }
              



            
            if(btnPointed==false &&lastButtonPointed!=null){
            lastButtonPointed.GetComponent<LongClick>().pressed=false;
             lastButtonPointed.GetComponent<LongClick>().pointerRaycastItemSelector=null;

            }
            return;
        }
        if(lastButtonPointed!=null){
            lastButtonPointed.GetComponent<LongClick>().pressed=false;
             lastButtonPointed.GetComponent<LongClick>().pointerRaycastItemSelector=null;
         UIController.instance.Reset();
         print("non ho preso un button");
        }
        lastButtonPointed = null;

    }

    
    private void setHandButtonInteraction(){
        
    }
    private Vector3 GetHandDirection()
    {
        if (!this.gameObject.activeSelf)
            return Vector3.zero;

        wristPosition = spheres[0].transform.position;

        thumbTipPosition = spheres[8].transform.position;


        thumbDirection = (thumbTipPosition - wristPosition).normalized;
       
        previousHandDirection = filteredHandDirection;

        filteredHandDirection = -centerSphere.transform.forward;
        filteredHandDirection.y += adjustY;
        filteredHandDirection.x += adjustX;
        filteredHandDirection = Vector3.Lerp(previousHandDirection, filteredHandDirection, smoothingFactor);
        return filteredHandDirection.normalized;
    }
    //funzione che instanzia il raycast e controlla il tag dell'oggetto colpito
    private void RaycastPointer()
    {
        bool canDraw = true;

        
        Vector3 direction = GetHandDirection();
        if (direction == Vector3.zero)
        {
            if (lastButtonPointed != null)
                lastButtonPointed.GetComponent<LongClick>().pressed = false;
            return;
        }


        GetComponent<LineRenderer>().positionCount = 2;
        Ray ray = new Ray(centerSphere.transform.position, direction*range );

        RaycastHit hit;

        //Vector3 endPosition = filteredHandDirection * maxDistance;
        if (Physics.Raycast(ray, out hit))
        {

         
        

            string colliderTag = hit.transform.tag;

            graphRaycast(hit, direction);
            draw(canDraw,hit.point);
            if(!colliderTag.Equals("Item")){
                ResetLastItemHitten();
                ResetLastItemSelected();
            }
            switch (colliderTag)
            {
                case "Item":

                    InteractWithItem(hit);

                    break;

                case "Wall":
                case "EmptySpace":
                case "Untagged":       
                
                    ResetLastItemSelected();
                    ResetLastItemHitten();

                    break;

                case (null):
                
                    break;
            }

          
          return;
        }
        draw(canDraw, direction * range);



    }

    public void ResetLastItemSelected()
    {
        if (lastInteractableItemSelected != null)
        {
            Debug.LogWarning("Pointer not enough on the item");
            lastInteractableItemSelected.SetIsPointed(false);
           
            lastInteractableItemSelected.SetWasGrabbed(false);
            lastInteractableItemSelected.localObjectTimer=0;
            lastInteractableItemSelected = null;
          string evento= this.hand.ToUpper()+"_ITEM_IMAGE_CHANGE";
            Messenger<Sprite>.Broadcast(evento,null);
        }

    }
    public void ResetLastItemHitten(){
        if(lastInteractableItemHitten!=null){
       
          lastInteractableItemHitten.SetIsPointed(false);
         lastInteractableItemHitten.localObjectTimer=0;
         lastInteractableItemHitten.SetWasGrabbed(false);
            lastInteractableItemHitten = null;

        }
    }

    public void ResetLastEmprySpacePointed(){

        if(this.lastEmptySpacePointed!=null){
        this.lastEmptySpacePointed.isPointed=false;

       
        }
        this.lastEmptySpacePointed=null;
     
    }
    //Serve a interagire con un oggetto quando lo si punta con il raycast
    private void InteractWithItem(RaycastHit hit)
    {
        // Debug.DrawRay(thumbTipPosition, filteredHandDirection * range, Color.black, 0.2f);

        InteractableItem interactableItemHitten = hit.transform.GetComponent<InteractableItem>();
        //isSelecting = true;
        if(interactableItemHitten!=lastInteractableItemHitten){
            ResetLastItemHitten();
        }
        if (interactableItemHitten.GetIsPointed() && interactableItemHitten.GetRaycastSelector() != this)
        {
            Debug.LogError(this.hand + " point on item already pointed ");
            return;

        }

        //  lastItemSelected=interactableItemHitten;

        interactableItemHitten.SetIsPointed(true);
        interactableItemHitten.SetRaycastSelector(this);
        lastInteractableItemHitten=interactableItemHitten;

    }

public void resetAll(){
    ResetLastEmprySpacePointed();
    ResetLastItemHitten();
    ResetLastItemSelected();
    
    lastSecurityToolPointed=null;
    this.mode=selectorMode.CanSelect;
}

}




