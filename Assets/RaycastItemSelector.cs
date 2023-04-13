using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastItemSelector : MonoBehaviour
{
    // Start is called before the first frame update

    public float maxDistance=100f;
    public float multiplier=5;
    Vector3 startPoint;
    Vector3 direction;
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {

          Vector3 wristPosition= this.transform.GetChild(0).transform.position;
          Vector3 indexTipPosition= this.transform.GetChild(8).transform.position;
          Vector3 pinkyTipPosition= this.transform.GetChild(20).transform.position;
          Vector3 thumbTipPosition=this.transform.GetChild(4).transform.position;
          Vector3 indexDirection =(indexTipPosition -wristPosition).normalized;
          Vector3 pinkyDirection =(pinkyTipPosition -wristPosition).normalized;   
          Vector3 thumbDirection =(thumbTipPosition -wristPosition).normalized;  
          Vector3 handDirection= indexDirection+pinkyDirection+thumbDirection; 
               //change color to test the correct bones
          this.transform.GetChild(0).GetComponent<Renderer>().material.SetColor("_Color", Color.black);
       // Call SetColor using the shader property name "_Color" and setting the color to red
          this.transform.GetChild(8).GetComponent<Renderer>().material.SetColor("_Color", Color.black);


          RaycastHit hit;
          Ray ray = new Ray(wristPosition, handDirection);
          if (Input.GetKeyDown("space"))
            {
                            
                      if(Physics.Raycast(ray,out hit,maxDistance*multiplier)){

                              print(hit.transform.gameObject);
                            
                      }
                

                      
                  
                      /* Debug.DrawRay(wristPosition, thumbDirection, Color.red,3f);
                       Debug.DrawRay(wristPosition, pinkyDirection, Color.blue,3f);
                       Debug.DrawRay(wristPosition, indexDirection, Color.green,3f);
                     */  Debug.DrawRay(wristPosition,handDirection,Color.yellow,5f);
                      
                  
                      
              }

              }


      void OnCollisionEnter(Collision collision)
    {
        Debug.DrawRay(collision.contacts[0].point, collision.contacts[0].normal, Color.green, 2, false);
    }
}
