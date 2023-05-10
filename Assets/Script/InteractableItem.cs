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
    private SliderController scInstance;
    bool materialSetted = false;
    public float localObjectTimer;
    public string hand = "";
    public int rayNumber = 0;
    public RaycastItemSelector leftOrRightSelector;
    void Awake() {
            scInstance = SliderController.instance;
    }
    void Start()
    {
        var renderers = this.gameObject.GetComponents<Renderer>();

        Transform objTransform = this.transform;
      
        /*  instance =Instantiate(localCanvas);
         instance.SetActive(false);
         instance.transform.SetParent(this.gameObject.transform);
         instance.transform.position = new Vector3(objTransform.position.x, objTransform.position.y + 1f, objTransform.position.z - 0.3f);*/
    }

    private bool updateSlider(float value){
        if(leftOrRightSelector == null )
            return false;

        string gameEvent="";
        if(leftOrRightSelector.hand=="Left")
            gameEvent=GameEvents.LEFT_SLIDER_CHANGE;
        if(leftOrRightSelector.hand=="Right")
            gameEvent=GameEvents.RIGHT_SLIDER_CHANGE;
        Messenger<float>.Broadcast(gameEvent,value);
        return true;
    }

      private bool activeSlider(bool value){
        if(leftOrRightSelector == null )
            return false;

        string gameEvent="";
        if(leftOrRightSelector.hand=="Left")
            gameEvent=GameEvents.LEFT_SLIDER_ACTIVE;
        if(leftOrRightSelector.hand=="Right")
            gameEvent=GameEvents.RIGHT_SLIDER_ACTIVE;
        Messenger<bool>.Broadcast(gameEvent,value);
        return true;
    }

    // Update is called once per frame
    void Update()
    {


        if (isSelected)
        {





            localObjectTimer += Time.deltaTime;

            if (localObjectTimer >= 2.0f)
            {

                localObjectTimer = 2;

                //HandController.instance.setHandObject(this);
                updateSlider(localObjectTimer); 
                activeSlider(false);
                // scInstance.getSlider(leftOrRightSelector.hand).GetComponentInChildren<Image>().sprite=item.sprite;

                return;
            }
            if (localObjectTimer < 2.0f)
            {


                // instance.SetActive(true);
                
                activeSlider(true);
                updateSlider(localObjectTimer); 

            }
        }
        else
        {

            localObjectTimer = 0;
            
           if(leftOrRightSelector==null)
                return;
             updateSlider(localObjectTimer); 
            activeSlider(false);
            
            leftOrRightSelector = null;
            

        }




    }


    public void setSelector(RaycastItemSelector leftOrRight)
    {
        this.leftOrRightSelector = leftOrRight;
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
