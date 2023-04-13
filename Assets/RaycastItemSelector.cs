using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastItemSelector : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
  
               this.transform.GetChild(0).GetComponent<Renderer>().material.SetColor("_Color", Color.blue);
       // Call SetColor using the shader property name "_Color" and setting the color to red
      

       Transform t=this.transform.GetChild(0).transform;
         Vector3 forward = t.TransformVector(Vector3.forward)*10;// * 10;
        Debug.DrawRay(t.position, forward, Color.green);
       
  

        
    }
      void OnCollisionEnter(Collision collision)
    {
        Debug.DrawRay(collision.contacts[0].point, collision.contacts[0].normal, Color.green, 2, false);
    }
}
