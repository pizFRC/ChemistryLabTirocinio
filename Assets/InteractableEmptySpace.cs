using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractableEmptySpace : MonoBehaviour
{
    // Start is called before the first frame update
    public bool containsObject=false;

    public GameObject objectContained;
    public bool isPointed=false;
     GameObject canvasLocal;
    public GameObject prefabCanvas;
    float timer;
    void Start()
    {
        canvasLocal=Instantiate(prefabCanvas);
        canvasLocal.SetActive(false);
    canvasLocal.transform.SetParent(this.gameObject.transform);
    canvasLocal.transform.position=new Vector3(this.transform.position.x,this.transform.position.y+0.8f,this.transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        if(isPointed && !containsObject){
            if(!canvasLocal.activeInHierarchy){
                canvasLocal.SetActive(true);
            }
            timer+=Time.deltaTime;

            if(timer>2.0f){
                Debug.Log("ora puoi inserire qui");
                timer=2.0f;
            }
            canvasLocal.GetComponentInChildren<Slider>().value=timer;

        }else{
            if(canvasLocal.activeInHierarchy){
                 canvasLocal.GetComponentInChildren<Slider>().value=0;
                canvasLocal.SetActive(false);
            }
                timer=0;

        }
    }


    public bool putObject(GameObject obj){
        if(this.timer<2.0f)
            return false;
        if(containsObject)
            return false;
        
        objectContained=obj;
        containsObject=true;
        Debug.Log("put go"+obj);
        Transform pos= this.GetComponentInParent<Transform>();
        objectContained =Instantiate(obj,pos.position,pos.rotation);
    
        objectContained.transform.rotation=obj.transform.rotation;
        objectContained.transform.position = new Vector3(pos.position.x, pos.position.y , pos.position.z );
        objectContained.transform.SetParent(this.gameObject.transform);

        if(!objectContained.activeInHierarchy){
            objectContained.SetActive(true);
        }
        if(objectContained.GetComponentInChildren<Canvas>().gameObject.activeInHierarchy){
            objectContained.GetComponentInChildren<Canvas>().gameObject.SetActive(false);
        }

      
        Color newTransparentColor= this.GetComponent<Renderer>().material.color;
        newTransparentColor.a=0.1f;
        this.GetComponent<Renderer>().material.color=newTransparentColor;
        return containsObject;
    }
}
