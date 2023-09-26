using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class SliderController : MonoBehaviour
{

    public Slider leftSlider;
    public Slider rightSlider;

    private bool leftSliderActive=false;
    private bool rightSliderActive=false;
    public Image leftSliderInnerImage;
    public Image rightSliderInnerImage;

    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
     void Awake()
    {
      
        Messenger<float>.AddListener(GameEvents.LEFT_SLIDER_CHANGE, updateSliderLeft);
        Messenger<float>.AddListener(GameEvents.RIGHT_SLIDER_CHANGE, updateSliderRight);
        Messenger<bool>.AddListener(GameEvents.RIGHT_SLIDER_ACTIVE, activeSliderRight);
        Messenger<bool>.AddListener(GameEvents.LEFT_SLIDER_ACTIVE, activeSliderLeft);
     
        Messenger<Sprite>.AddListener(GameEvents.LEFT_SLIDER_IMAGE_CHANGE_ICON, updateImageItemLeftSlider);
        Messenger<Sprite>.AddListener(GameEvents.RIGHT_SLIDER_IMAGE_CHANGE_ICON, updateImageItemRightSlider);

    }

    void OnDestroy()
    {
        Messenger<float>.RemoveListener(GameEvents.LEFT_SLIDER_CHANGE, updateSliderLeft);
        Messenger<float>.RemoveListener(GameEvents.RIGHT_SLIDER_CHANGE, updateSliderRight);
        Messenger<bool>.RemoveListener(GameEvents.RIGHT_SLIDER_ACTIVE, activeSliderRight);
        Messenger<bool>.RemoveListener(GameEvents.LEFT_SLIDER_ACTIVE, activeSliderLeft);
       
        Messenger<Sprite>.RemoveListener(GameEvents.LEFT_SLIDER_IMAGE_CHANGE_ICON, updateImageItemLeftSlider);
        Messenger<Sprite>.RemoveListener(GameEvents.RIGHT_SLIDER_IMAGE_CHANGE_ICON, updateImageItemRightSlider);
    }


    ///function to manage slider
    public void updateSliderLeft(float value)
    {

        leftSlider.value = value;
    }
    public void updateSliderRight(float value)
    {

        rightSlider.value = value;
    }
  
    
    public void activeSliderLeft(bool value)
    {
    
        leftSliderActive=true;
        leftSlider.transform.parent.gameObject.SetActive(value);
    }
    public void activeSliderRight(bool value)
    {
       
        rightSliderActive=true;
        rightSlider.transform.parent.gameObject.SetActive(value);
    }




     void updateImageItemRightSlider(Sprite S)
    {
        rightSliderInnerImage.sprite = S;
    }
    void updateImageItemLeftSlider(Sprite S)
    {
        leftSliderInnerImage.sprite = S;
    }

    
    public void setLeftInnerImageGesture(gestureIndex gi)
    {

        //leftSliderInnerImage.sprite = gestureImage[(int)gi];

    }
   
    public void setGestureRight(gestureIndex gi)
    {
        //rightSliderInnerImage.sprite = gestureImage[(int)gi];
  
    }


     private void NewMethod()
    {
        
        if (!leftSliderActive)
        {
            updateSliderLeft(0);
            activeSliderLeft(false);
            

        }
        if (!rightSliderActive)
        {
            updateSliderRight(0);
            activeSliderRight(false);
        }
    }
}
