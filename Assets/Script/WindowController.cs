using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowController : MonoBehaviour
{
    // Start is called before the first frame update
    public bool isOpen=false;
   
   
    public Transform opened,closed;

   
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isOpen)
          this.gameObject.transform.position=opened.position;
        else
            this.gameObject.transform.position=closed.position;
    }
    public void setOpenOrClose(){
       
       isOpen=!isOpen;
           
    }
}
