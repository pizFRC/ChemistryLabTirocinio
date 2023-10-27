using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class VirtualCursor : MonoBehaviour
{
    // Start is called before the first frame update
     [SerializeField]  GraphicRaycaster m_Raycaster;
    PointerEventData m_PointerEventData;
    [SerializeField] EventSystem m_EventSystem;
    [SerializeField] RectTransform canvasRect;

    public RaycastItemSelector raycastItemSelector;
    Vector3 filteredHandDirection,wristPosition,thumbTipPosition,thumbDirection,previousHandDirection;
    
    public float smoothingFactor = 0.020f;
    public float maxDistance = 150f;

    void Start(){
          //  RaycastItemSelector raycastItemSelector=GetComponent<RaycastItemSelector>();

    }
  private Vector3 GetHandDirection()
    {
        //GET BONES POSITION
       
         wristPosition = raycastItemSelector.spheres[0].transform.position;
        
        thumbTipPosition = raycastItemSelector.spheres[8].transform.position;
       

        thumbDirection = (thumbTipPosition - wristPosition).normalized;
        /* Debug.DrawRay(centerSphere.transform.position,-centerSphere.transform.forward * 100, Color.red, 0.2f);
        Debug.DrawRay(wristPosition,thumbDirection * 100, Color.blue, 0.2f); */
        GetComponent<LineRenderer>().positionCount = 2;

        Vector3 midway = raycastItemSelector.spheres[5].transform.forward.normalized + raycastItemSelector.spheres[4].transform.forward.normalized+thumbDirection.normalized;
        midway.y += raycastItemSelector.adjustY;
        midway.x += raycastItemSelector.adjustX;
        midway = midway.normalized;
        
        
        filteredHandDirection=-raycastItemSelector.centerSphere.transform.forward  ;
        filteredHandDirection.y+= raycastItemSelector.adjustY;
        filteredHandDirection.x += raycastItemSelector.adjustX;
        return filteredHandDirection;
    }
    void Update()
    {
     
            //Set up the new Pointer Event
            m_PointerEventData = new PointerEventData(m_EventSystem);
            //Set the Pointer Event Position to that of the game object
           Vector2 screenPos=new Vector2(GetHandDirection().x*maxDistance,GetHandDirection().y*maxDistance);
             Ray ray = new Ray(thumbTipPosition, GetHandDirection() * 100);

        RaycastHit hit;

        Vector3 endPosition = filteredHandDirection * maxDistance;

       Debug.DrawRay(raycastItemSelector.spheres[5].transform.position, GetHandDirection()*100, Color.green, 0.2f);

        if (Physics.Raycast(ray, out hit, 250f))
        {
            

            m_PointerEventData.position = Camera.main.WorldToScreenPoint(hit.point);
         
              List<RaycastResult> results = new List<RaycastResult>();
 
            //Raycast using the Graphics Raycaster and mouse click position
            m_Raycaster.Raycast(m_PointerEventData, results);
             if(results.Count > 0){ 
              //Debug.LogError("Hit " + results[0].gameObject.name);
               for(int i=0;i<results.Count;i++)
               if(results[i].gameObject.TryGetComponent<Button>(out Button btn)){
                  btn.OnPointerClick( m_PointerEventData);
                  
                
               }
             }
 
              }
           
    }
}
