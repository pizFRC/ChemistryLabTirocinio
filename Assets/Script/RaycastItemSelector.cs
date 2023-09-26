using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum selectorMode
{
    CanSelect = 1,
    CannotSelect = 2,
    RaycastOff = 3,

    AlreadySelect = 4,

    LockOnItem = 5,
    MoveItem = 6,
    LockOnEmptySpace = 7,
}

public class RaycastItemSelector : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] public selectorMode mode;
    public GameObject uiGesture;

    float elapsedTime = 0f;
    public string hand;
    public Material standard;
  


    public float timeToWait = 1.0f;
    public float smoothingFactor = 0.020f, maxDistance = 150f;
    private LineRenderer lineRenderer;
    public InteractableItem lastInteractableItemSelected;
    InteractableEmptySpace lastEmptySpacePointed;
    public InteractableItem lastItemSelectedFor2Second;
    public float valueToSubtractY, valueToAddX;
    public float range = 2;
    Vector3 itemPosition;
    
    Vector3 previousHandDirection, thumbDirection, thumbTipPosition, wristPosition, filteredHandDirection;
    void Start()
    {

        //   instanceLine=Instantiate(prefabLine);
        lineRenderer = GetComponent<LineRenderer>();
        if (hand == "")
        {
            if (this.tag == "RightHand")
            {
                hand = "Right";
            }
            else
            {
                hand = "Left";
            }
        }

    }
    
    private Vector3 GetHandDirection()
    {
        //GET BONES POSITION
        wristPosition = this.transform.GetChild(0).transform.position;

        thumbTipPosition = this.transform.GetChild(4).transform.position;
        Vector3 indexTipPosition = this.transform.GetChild(8).transform.position;

        thumbDirection = (thumbTipPosition - wristPosition).normalized;
        Vector3 indexDirection = (indexTipPosition - wristPosition).normalized;

        Vector3 middleFingerTipPosition = this.transform.GetChild(12).transform.position;
        Vector3 middleFingerDirection = (middleFingerTipPosition - wristPosition).normalized;
        //SMOTHING FILTER
        filteredHandDirection = Vector3.Lerp(previousHandDirection, thumbDirection + indexDirection + middleFingerDirection, smoothingFactor);


        //print("RaycastPointer");

        filteredHandDirection.y -= valueToSubtractY;
        filteredHandDirection.x += valueToAddX;
        return filteredHandDirection;
    }
    // Update is called once per frame
    void Update()
    {

        elapsedTime += Time.deltaTime;


        //ogni 1/10 di secondo  sparo non ogni frame

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
                  
                    break;

                case (selectorMode.LockOnItem):
                        LockPointerOnItem(lastInteractableItemSelected);
               
                    break;
                case (selectorMode.MoveItem):
                  
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
    public void LockPointerOnItem(InteractableItem interactableItemHitten){
                    this.mode=selectorMode.LockOnItem;
                    lastInteractableItemSelected=interactableItemHitten;
                    thumbTipPosition = this.transform.GetChild(4).transform.position;
                    Vector3 itemPosition=lastInteractableItemSelected.transform.position;
                    itemPosition.z+=0.3f;
                    
                    GetComponent<LineRenderer>().SetPosition(0, thumbTipPosition);
                    GetComponent<LineRenderer>().SetPosition(1, lastInteractableItemSelected.transform.position);
    }


    public void raycastMoveObject()
    {
        bool canDraw = true;
        Ray ray = new Ray(thumbTipPosition, GetHandDirection() * range);

        RaycastHit hit;

        Vector3 endPosition = filteredHandDirection * maxDistance;
        if (Physics.Raycast(ray, out hit, 250f))
        {

            if (hit.collider.gameObject.CompareTag("EmptySpace"))
            {

                lastEmptySpacePointed = hit.transform.GetComponent<InteractableEmptySpace>();

                lastEmptySpacePointed.ris = this;
                if (!lastEmptySpacePointed.containsObject)
                {



                    lastEmptySpacePointed.isPointed = true;

                    if (lastEmptySpacePointed.putObject(this, lastItemSelectedFor2Second.gameObject))
                    {

                       // HandController.instance.riponiOggetto(hand);
                        //lastItemSelectedFor2Second.
                        
                        mode = selectorMode.CanSelect;
                        //lastEmptySpacePointed.isPointed=false;
                        Debug.Log("Posizionato");
                        lastEmptySpacePointed.isPointed = false;
                    }

                }
                else
                {


                    lastEmptySpacePointed.isPointed = true;

                }



            }
            else if (lastEmptySpacePointed != null)
            {
                lastEmptySpacePointed.isPointed = false;
                lastEmptySpacePointed = null;

            }

            draw(canDraw, hit.point);
        }

    }

//disegna la linea che fa da puntaotre
    public void draw(bool canDraw, Vector3 point)
    {
        if (canDraw)
        {


            thumbTipPosition = this.transform.GetChild(4).transform.position;
            GetComponent<LineRenderer>().positionCount = 2;
            GetComponent<LineRenderer>().SetPosition(0, thumbTipPosition);
            GetComponent<LineRenderer>().SetPosition(1, point);
        }
        else
            GetComponent<LineRenderer>().positionCount = 0;

    }
    
  
  //funzione che instanzia il raycast e controlla il tag dell'oggetto colpito
    private void RaycastPointer()
    {
        bool canDraw = true;
        Ray ray = new Ray(thumbTipPosition, GetHandDirection() * range);

        RaycastHit hit;

        Vector3 endPosition = filteredHandDirection * maxDistance;
        if (Physics.Raycast(ray, out hit, 250f))
        {

            string colliderTag = hit.collider.tag;
            switch (colliderTag)
            {
                case "Item":

                    InteractWithItem(hit);

                    break;

                case "Wall":
                    ResetLastItemSelected();
                    break;

                case "EmptySpace":
                ResetLastItemSelected();
                    break;

                case (null):
                    
                    break;
            }
            draw(canDraw, hit.point);
        }

        void ResetLastItemSelected()
        {
            if (lastInteractableItemSelected != null)
            {
                Debug.LogWarning("Pointer on the wall");
                lastInteractableItemSelected.SetIsPointed(false);
                lastInteractableItemSelected = null;

            }
        }
    }

    //Serve a interagire con un oggetto quando lo si punta con il raycast
      private void InteractWithItem(RaycastHit hit)
    {
        Debug.DrawRay(thumbTipPosition, filteredHandDirection * range, Color.black, 0.2f);
      
       InteractableItem interactableItemHitten=hit.transform.GetComponent<InteractableItem>();
        //isSelecting = true;
        
        if(interactableItemHitten.GetIsPointed() && interactableItemHitten.GetRaycastSelector() != this){
            Debug.LogError( this.hand  + " point on item already pointed ");
            return;

        }

      //  lastItemSelected=interactableItemHitten;
        
        interactableItemHitten.SetIsPointed(true);
        interactableItemHitten.SetRaycastSelector(this);

        
    }


}




