using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractableItem : MonoBehaviour
{
    // Start is called before the first frame update
    public Material original;
    public Material selected;

    public Item item;

   
    
    public bool isSelected = false;
    public GameObject localCanvas;
    GameObject instance;
    bool canvasNotActive = true;

   bool materialSetted=false;
   float localObjectTimer;
    void Start()
    {
        var renderers = this.gameObject.GetComponents<Renderer>();
       
        Transform objTransform = this.transform;
       
        instance = Instantiate(localCanvas);
        instance.SetActive(false);
        instance.transform.SetParent(this.gameObject.transform);
        instance.transform.position = new Vector3(objTransform.position.x, objTransform.position.y + 1f, objTransform.position.z - 0.3f);
    }

    // Update is called once per frame
    void Update()
    {
       
        

          if(localObjectTimer >2.0f && isSelected){
                
                localObjectTimer=2;
                 instance.GetComponentInChildren<Slider>().value=localObjectTimer;
                 return;
           } 
           if(isSelected && localObjectTimer <2.0f)
           { 
            instance.SetActive(true);
            localObjectTimer+=Time.deltaTime;
              instance.GetComponentInChildren<Slider>().value=localObjectTimer;

           }
           if(!isSelected){
            localObjectTimer=0;
             instance.GetComponentInChildren<Slider>().value=0;
             instance.SetActive(false);
           }

        

          
    }


    
    
    public void showCanvas()
    {
        instance.SetActive(true);
     
    }
    public void hideCanvas()
    {
        instance.SetActive(false);
       
    }

        private void OnTriggerStay(Collider other)
    {


        Debug.LogError("stay on ");
    }


    public void changeMaterial(bool selected_material)
    {
        var renderer = this.gameObject.GetComponents<Renderer>();
        foreach (Renderer r in renderer)
        {

            if (selected_material)
            {
                r.material = selected;
            }
            else
            {
                r.material = original;
            }
        }
    }




}
