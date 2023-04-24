using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastItemSelector : MonoBehaviour
{
    // Start is called before the first frame update

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
    public bool canRaycast=true;
    Vector3 previousHandDirection, thumbDirection, thumbTipPosition, wristPosition, filteredHandDirection;
    void Start()
    {

        //   instanceLine=Instantiate(prefabLine);
        lineRenderer = GetComponent<LineRenderer>();
        if(hand==""){
            if(this.tag=="RightHand"){
                hand="R";
            }else{
                hand="L";
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
          if(!canRaycast){
             GetComponent<LineRenderer>().positionCount =0;
             return;
          }
        if (elapsedTime > timeToWait)
        {


            Ray ray = new Ray(thumbTipPosition, GetHandDirection() * range);

            RaycastHit hit;

            Vector3 endPosition = filteredHandDirection * maxDistance;
            int layerMask = 1 << 8;
            layerMask = ~layerMask;


            //RAYCASTO IL RAGGIO SE NON COLPISCO NIENTE NON LO MOSTRO
    
               if(lastItemSelectedFor2Second!=null){
           
            GetComponent<LineRenderer>().positionCount = 2;
            thumbTipPosition = this.transform.GetChild(4).transform.position;
                GetComponent<LineRenderer>().SetPosition(0, thumbTipPosition);
                GetComponent<LineRenderer>().SetPosition(1, lastItemSelectedFor2Second.transform.position);

        }  else  if (Physics.Raycast(ray, out hit, 250f) && hit.collider.CompareTag("Item"))
            {
                Debug.DrawRay(thumbTipPosition, filteredHandDirection * range, Color.black, 0.2f);
                endPosition = hit.point;
                selectionTimer += timeToWait + Time.deltaTime;
                isSelecting = true;
                lastItemSelected = hit.transform.GetComponent<InteractableItem>();
                lastItemSelected.isSelected = true;

                GetComponent<LineRenderer>().positionCount = 2;
                GetComponent<LineRenderer>().SetPosition(0, thumbTipPosition);
                GetComponent<LineRenderer>().SetPosition(1, endPosition);
                if (selectionTimer >= 2f)
                {
                     GetComponent<LineRenderer>().SetPosition(1, hit.point);

                     lastItemSelectedFor2Second=hit.collider.GetComponent<InteractableItem>();
                    
                    //canvas e raycast 
                    //blocca raggio sull'ggetto

                    //Inizia a prendere le gesture
                }

              
            }
            else if (Physics.Raycast(ray, out hit, 250f) && hit.collider.CompareTag("Wall"))
            {
                selectionTimer = 0;
                isSelecting = false;
                if (lastItemSelected != null){
                    lastItemSelected.isSelected = false;
                    lastItemSelected=null;
                
                }
                
                GetComponent<LineRenderer>().positionCount = 2;
                GetComponent<LineRenderer>().SetPosition(0, thumbTipPosition);
                GetComponent<LineRenderer>().SetPosition(1, hit.point);
            }
            else
            {
                GetComponent<LineRenderer>().positionCount = 0;

                selectionTimer = 0;
                isSelecting = false;
                if (lastItemSelected != null){
                    lastItemSelected.isSelected = false;
                    lastItemSelected=null;

                }
                //   lineRenderer.positionCount = 0;

            }













            elapsedTime = 0;

            previousHandDirection = filteredHandDirection;







        }




    }

}




