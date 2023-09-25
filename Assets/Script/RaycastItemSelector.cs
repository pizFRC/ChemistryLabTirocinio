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
    public Material riponiOggMaterial;


    public float timeToWait = 1.0f;
    public float smoothingFactor = 0.020f, maxDistance = 150f;
    private LineRenderer lineRenderer;
    float selectionTimer = 0f;
    public InteractableItem lastItemSelected;
    InteractableEmptySpace lastEmptySpacePointed;
    public InteractableItem lastItemSelectedFor2Second;
    public float valueToSubtractY, valueToAddX;
    public float range = 2;
    bool isSelecting = false;
    Vector3 itemPosition;
    public bool canRaycast = true;
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
    public void resetSelected()
    {
        if (lastItemSelectedFor2Second != null)
            lastItemSelectedFor2Second.isSelected = false;
        if (lastItemSelected != null)
            lastItemSelected.isSelected = false;
        if (lastEmptySpacePointed != null)
            lastEmptySpacePointed.isPointed = false;
        this.lastItemSelected = null;
        this.lastEmptySpacePointed = null;
        this.lastItemSelectedFor2Second = null;

        this.mode = selectorMode.CanSelect;

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


        //print("raycast");

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
                    raycast();
                    break;
                case (selectorMode.CannotSelect):
                    GetComponent<LineRenderer>().positionCount = 0;
                    // GetComponent<LineRenderer>().SetPosition(1, lastItemSelected.transform.position);
                    //lastItemSelectedFor2Second = lastItemSelected.GetComponent<InteractableItem>();
                    break;

                case (selectorMode.LockOnItem):

                    GetComponent<LineRenderer>().material = riponiOggMaterial;
                    this.lastItemSelectedFor2Second = lastItemSelected;

                    thumbTipPosition = this.transform.GetChild(4).transform.position;

                    GetComponent<LineRenderer>().SetPosition(0, thumbTipPosition);
                    GetComponent<LineRenderer>().SetPosition(1, itemPosition);
                    HandController.instance.setHandObject(lastItemSelected, hand);
                    break;
                case (selectorMode.MoveItem):
                    this.lastItemSelectedFor2Second = lastItemSelected;
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

                        HandController.instance.riponiOggetto(hand);
                        //lastItemSelectedFor2Second.
                        this.lastItemSelected = null;
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
    public void reset()
    {
        this.mode = selectorMode.CanSelect;
        this.lastItemSelectedFor2Second = null;
        this.lastItemSelected.rayNumber = 0;

    }
    private void InteractWithItem(RaycastHit hit)
    {
        Debug.DrawRay(thumbTipPosition, filteredHandDirection * range, Color.black, 0.2f);
        Vector3 endPosition = hit.point;
        selectionTimer += timeToWait + Time.deltaTime;
        isSelecting = true;
        lastItemSelected = hit.transform.GetComponent<InteractableItem>();

        if (lastItemSelected.rayNumber > 0)
        {
            this.mode = selectorMode.CanSelect;
            lastItemSelected = null;

            return;
        }
        lastItemSelected.setSelector(this);
        lastItemSelected.isSelected = true;

        if (lastItemSelected.localObjectTimer >= 2.0f)
        {
            Debug.LogError("sull item per più di 2 secondi");

            this.mode = selectorMode.LockOnItem;
            this.itemPosition = hit.point;
            lastItemSelected.rayNumber += 1;

            HandController.instance.lockGesture(this.hand);



        }
    }
    private void raycast()
    {
        bool canDraw = true;
        Ray ray = new Ray(thumbTipPosition, GetHandDirection() * range);

        RaycastHit hit;

        Vector3 endPosition = filteredHandDirection * maxDistance;
        if (Physics.Raycast(ray, out hit, 250f))
        {

            print(hit.collider.tag);
            string colliderTag = hit.collider.tag;
            switch (colliderTag)
            {
                case "Item":

                    InteractWithItem(hit);

                    break;

                case "Wall":
                    break;

                case "EmptySpace":
                    break;

                case (null):
                    break;
            }
            /* 
             if (hit.collider.CompareTag("Wall"))
            {
                selectionTimer = 0;
                isSelecting = false;
                if (lastItemSelected != null)
                {
                    lastItemSelected.isSelected = false;
                    lastItemSelected = null;

                }
                if (lastEmptySpacePointed != null)
                {
                    lastEmptySpacePointed.isPointed = false;
                    lastEmptySpacePointed = null;
                }


            }
            else if (hit.collider.CompareTag("EmptySpace"))
            {

                lastEmptySpacePointed=  hit.collider.GetComponent<InteractableEmptySpace>();
                 lastEmptySpacePointed.ris = this;
                if (lastEmptySpacePointed.containsObject )
                {

                      
                    if (lastEmptySpacePointed.rayNumber>0)
                         return;

                   
                    lastEmptySpacePointed.isPointed = true;
                    lastEmptySpacePointed.stop=false;
                    if (lastEmptySpacePointed.timerContained >= 2.0f)
                    {
                        lastEmptySpacePointed.rayNumber+=1;

                     
                        Debug.LogError("sullo space riempito per più di 2 secondi");

                       
                        itemPosition = lastEmptySpacePointed.transform.position;
                        this.mode = selectorMode.LockOnEmptySpace;
                        HandController.instance.setSpaceObject(lastEmptySpacePointed,this.hand);
                        // HandController.instance.lockGesture(this.hand);

                    }

                }
            
            }
            else
            {

                selectionTimer = 0;
                isSelecting = false;
                if (lastItemSelected != null)
                {
                    lastItemSelected.isSelected = false;
                    lastItemSelected = null;

                }
                if (lastEmptySpacePointed != null)
                {
                    lastEmptySpacePointed.isPointed = false;
                    lastEmptySpacePointed = null;
                }

            } */

            draw(canDraw, hit.point);
        }
    }


}




