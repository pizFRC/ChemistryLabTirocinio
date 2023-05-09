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
    public GameObject sliderSX;
    public GameObject sliderDX;
    GameObject instance;
    bool canvasNotActive = true;

   bool materialSetted=false;
  public float localObjectTimer;
   public string hand="";
   public int rayNumber=0;
   public RaycastItemSelector leftOrRightSelector;
    void Start()
    {
        var renderers = this.gameObject.GetComponents<Renderer>();
       
        Transform objTransform = this.transform;
        
       /*  instance =Instantiate(localCanvas);
        instance.SetActive(false);
        instance.transform.SetParent(this.gameObject.transform);
        instance.transform.position = new Vector3(objTransform.position.x, objTransform.position.y + 1f, objTransform.position.z - 0.3f);*/
    }


void setCorrectSlider(string hand){
    if(hand=="Right"){
        instance=sliderDX;
        return;
    }
    instance=sliderSX;
}
    // Update is called once per frame
    void Update()
    {
       
        
    if(isSelected){
      
       string tag="loading_slider_"+leftOrRightSelector.hand;
        Debug.Log(tag);
       setCorrectSlider(leftOrRightSelector.hand);
         Debug.Log(instance);
          localObjectTimer+=Time.deltaTime;
          
          if(localObjectTimer >=2.0f){
                
                localObjectTimer=2;
               
                //HandController.instance.setHandObject(this);
                 instance.GetComponentInChildren<Slider>().value=localObjectTimer;
                
                 return;
           } 
           if(localObjectTimer <2.0f)
           { 

               
            instance.SetActive(true);
          
              instance.GetComponentInChildren<Slider>().value=localObjectTimer;

           }
    }else{
           
            localObjectTimer=0;
            if(instance==null)
                return;
             instance.GetComponentInChildren<Slider>().value=0;
             instance.SetActive(false);
             leftOrRightSelector=null;
             instance=null;
             
           }

        

          
    }


    public void setSelector(RaycastItemSelector leftOrRight){
        this.leftOrRightSelector=leftOrRight;
    }
    
    public void showCanvas()
    {
        instance.SetActive(true);
     
    }
    public void hideCanvas()
    {
        instance.SetActive(false);
       
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
