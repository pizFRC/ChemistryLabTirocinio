using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAtCamera : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public Transform target;
 
    

    void Update()
    {
        Vector3 screenPos = Camera.main.ViewportToScreenPoint(this.transform.position);
         
          this.transform.position=screenPos;
        Debug.Log("target is " + screenPos.x + " pixels from the left");
        this.transform.LookAt(Camera.main.transform.position);
    }

   
}
