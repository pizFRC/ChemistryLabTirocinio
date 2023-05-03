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
    MoveItem=6,
}

public class RaycastItemSelector : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] public selectorMode mode;

    float elapsedTime = 0f;
    public string hand;


    public float timeToWait = 1.0f;
    public float smoothingFactor = 0.020f, maxDistance = 150f;
    private LineRenderer lineRenderer;
    float selectionTimer = 0f;
    InteractableItem lastItemSelected;
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
        filteredHandDirection = Vector3.Lerp(previousHandDirection, thumbDirection, smoothingFactor);


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
        if (!canRaycast)
        {

        }
        if (elapsedTime > timeToWait)
        {

            switch (mode)
            {
                case (selectorMode.CanSelect):
                    raycast();
                    return;
                case (selectorMode.CannotSelect):
                    GetComponent<LineRenderer>().positionCount = 0;
                    // GetComponent<LineRenderer>().SetPosition(1, lastItemSelected.transform.position);
                    //lastItemSelectedFor2Second = lastItemSelected.GetComponent<InteractableItem>();
                    return;

                case (selectorMode.LockOnItem):
                 //   Debug.Log("lock on item");
                    //lastItemSelectedFor2Second.setSelector(this);
                    this.lastItemSelectedFor2Second=lastItemSelected;
                    thumbTipPosition = this.transform.GetChild(4).transform.position;
                    GetComponent<LineRenderer>().SetPosition(0, thumbTipPosition);
                    GetComponent<LineRenderer>().SetPosition(1, itemPosition);
                    HandController.instance.setHandObject(lastItemSelected, this.hand);
                    return;
                 case (selectorMode.MoveItem):
                 break;
                

               


                default: break;
            }





           













            elapsedTime = 0;

            previousHandDirection = filteredHandDirection;







        }




    }
    public void reset()
    {
        this.mode = selectorMode.CanSelect;
          this.lastItemSelectedFor2Second=null;
          this.lastItemSelected.rayNumber=0;
        Debug.LogError("reset called");
    }
    private void raycast()
    {
        bool draw=true;
        Ray ray = new Ray(thumbTipPosition, GetHandDirection() * range);

        RaycastHit hit;

        Vector3 endPosition = filteredHandDirection * maxDistance;
        if (Physics.Raycast(ray, out hit, 250f))
        {


            if (hit.collider.CompareTag("Item") )
            {
               
                Debug.DrawRay(thumbTipPosition, filteredHandDirection * range, Color.black, 0.2f);
                endPosition = hit.point;
                selectionTimer += timeToWait + Time.deltaTime;
                isSelecting = true;
                lastItemSelected = hit.transform.GetComponent<InteractableItem>();
                lastItemSelected.isSelected = true;
                 
                if (lastItemSelected.localObjectTimer >= 2.0f)
                {
                    Debug.LogError("sull item per più di 2 secondi");

                    this.mode = selectorMode.LockOnItem;
                    this.itemPosition = hit.point;
                    lastItemSelected.rayNumber+=1;
                    
                }

                if(lastItemSelected!=null && lastItemSelected.rayNumber<=1){
                    Debug.LogError("puoi usarlo ok");
                    
                }else
                {
                    Debug.LogError("a quanto pare qualcosa non va");
                     this.mode = selectorMode.CanSelect;
                    return;
                }
                
            }
            else if (hit.collider.CompareTag("Wall"))
            {
                selectionTimer = 0;
                isSelecting = false;
                if (lastItemSelected != null)
                {
                    lastItemSelected.isSelected = false;
                    lastItemSelected = null;

                }
                

            }else{
                draw=false;
                 selectionTimer = 0;
                isSelecting = false;
                if (lastItemSelected != null)
                {
                    lastItemSelected.isSelected = false;
                    lastItemSelected = null;

                }

            }

            if(draw){
            GetComponent<LineRenderer>().positionCount = 2;
            GetComponent<LineRenderer>().SetPosition(0, thumbTipPosition);
            GetComponent<LineRenderer>().SetPosition(1, hit.point);
            }else
             GetComponent<LineRenderer>().positionCount = 0;

        }
    }


}




