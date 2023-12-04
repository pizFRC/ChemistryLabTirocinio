using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecurityTool : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject window;
    public Transform closedPosition,openPosition;
    public bool isClosed=false;
    public bool containsObject;
    public float timeRequired; 
    public bool isPointed;

        public float multiplier;
    public void open(){
        
        if(isClosed)
         window.transform.Translate(Vector3.up*multiplier,Space.World);


       isClosed=false;
     
    }

    void OnDisable(){
        isPointed=false;
        containsObject=false;
        close();
    }
    public void close(){
        if(!isClosed)
         window.transform.Translate(Vector3.down*multiplier,Space.World);

         
        isClosed=true;
     
    }


    public void SetIsClosed(bool value)
    {
        isClosed=value;
    }
    void Start()
    {
        
    }

    public void SetIsPointed(bool value){
        isPointed=value;
    }

    // Update is called once per frame
    void Update()
    {
        if(isClosed){
            window.transform.position=closedPosition.position;
            return;
        }
        window.transform.position=openPosition.position;


        
    }
    IEnumerator changeState(bool value){
        yield return new WaitForSeconds(0.5f);
        isClosed=value;
      
    }
    public IEnumerator OpenWindows(){
        
        if(isClosed)
            open();
        else
            close();
      
        yield return new WaitForSeconds(0.5f);
       
    }
}
