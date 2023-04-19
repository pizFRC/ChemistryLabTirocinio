using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCamera : MonoBehaviour
{

    public GameObject handsObject;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)){
            print("key down");
            this.transform.Rotate(new Vector3(0,90,0));
            handsObject.transform.RotateAround(this.transform.position,new Vector3(0,1,0),90f);
        }
    }
}
