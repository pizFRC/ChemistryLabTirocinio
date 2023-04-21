using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastItemSelector : MonoBehaviour
{
    // Start is called before the first frame update



    GameObject raySelector;

    GameObject instanceLine;
    Vector3 startPoint;
    Vector3 direction;
    float elapsedTime = 0f;
    public float timeToWait = 1.0f;
    public float smoothingFactor = 0.020f, maxDistance = 150f;
    private LineRenderer lineRenderer;
    float timeOverItem = 0f;
    InteractableItem lastItem;
    InteractableItem lastItemSelectedFor2Second;
    public float valueToSubtractY,valueToAddX;
    public float range = 2;
    bool lockOnItem=false;
    Vector3 previousHandDirection, thumbDirection, thumbTipPosition, wristPosition, filteredHandDirection;
    void Start()
    {

        //   instanceLine=Instantiate(prefabLine);
        lineRenderer = GetComponent<LineRenderer>();

    }

    // Update is called once per frame
    void Update()
    {

        elapsedTime += Time.deltaTime;

        //ogni 1/10 di secondo 
        if (elapsedTime > timeToWait)
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
            filteredHandDirection = Vector3.Lerp(previousHandDirection, thumbDirection , smoothingFactor);


            //print("raycast");

                filteredHandDirection.y-=valueToSubtractY;
                filteredHandDirection.x+=valueToAddX;
            Ray ray = new Ray(thumbTipPosition, filteredHandDirection * range);

            RaycastHit hit;

            Vector3 endPosition = filteredHandDirection * maxDistance;
            int layerMask = 1 << 8;
            layerMask = ~layerMask;


            //RAYCASTO IL RAGGIO SE NON COLPISCO NIENTE NON LO MOSTRO
           
            if (Physics.Raycast(ray, out hit, 250f))
            {
                Debug.DrawRay(thumbTipPosition, filteredHandDirection * range, Color.black, 0.2f);
                endPosition = hit.point;
                timeOverItem += 0.02f + Time.deltaTime;

                GetComponent<LineRenderer>().positionCount = 2;
                GetComponent<LineRenderer>().SetPosition(0, thumbTipPosition);

                if (lastItemSelectedFor2Second != null)
                {
                    GetComponent<LineRenderer>().SetPosition(1, lastItemSelectedFor2Second.transform.position);
                    
                }
                else{
                    GetComponent<LineRenderer>().SetPosition(1, endPosition);
                }
                //SE COLPISCO UN ITEM :
                // SE LO COLPISCO PER PIù DI DUE SECONDI POSSO FARE UNA GESTURE E SCEGLIERE COSA FARE
                if (hit.collider.tag == "Item")
                {


                    lastItem = hit.collider.GetComponent<InteractableItem>();
                    lastItem.isTrigger=true;

                    //  lastItem.localCanvas.GetComponent<TestUI>().fillSlider(1.5f/timeOverItem);

                    hit.collider.GetComponent<InteractableItem>().changeMaterial(true);
                    hit.collider.GetComponent<InteractableItem>().showCanvas();
                    if (timeOverItem > 2)
                    {

                       
                        lastItemSelectedFor2Second = lastItem;
                        lockOnItem=true;
                      
                    }
                  


                }
                else if (hit.collider.tag == "LeftArrow")
                {

                    Debug.LogError("left arrow ");
                    Camera.main.GetComponent<RotateCamera>().rotate(-90f);

                }
                else if (hit.collider.tag == "RightArrow")
                {
                    Camera.main.GetComponent<RotateCamera>().rotate(90f);
                    Debug.LogError("right arrow ");


                }
                else
                {
                    timeOverItem = 0;
                    if (lastItem != null)
                    {
                        
                        lastItem.hideCanvas();
                        lastItem.changeMaterial(false);
                         lastItem.isTrigger=false;
                       
                    }
                    lastItem = null;
                }
                }
            else
            {

                timeOverItem = 0;
                if(lockOnItem){
                    lastItem.showCanvas();
                    lastItem.isTrigger=true;
                }
                if (lastItem != null)
                {
                    lastItem.hideCanvas();
                    lastItem.changeMaterial(false);
                        lastItem.isTrigger=false;
                }
                lastItem = null;
                lineRenderer.positionCount = 0;
            }



            elapsedTime = 0;

            previousHandDirection = filteredHandDirection;
        }




    }





}
