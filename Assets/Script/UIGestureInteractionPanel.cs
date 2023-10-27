using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class UIGestureInteractionPanel : MonoBehaviour
{
   
    // Start is called before the first frame update
    void Awake(){


        for(int i=0;i<this.transform.parent.childCount;i++)
             DontDestroyOnLoad(this.transform.parent.GetChild(i));
        DontDestroyOnLoad(this);
    }
    void Start()
    {
       
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    
     
}
