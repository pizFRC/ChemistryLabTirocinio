using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum gestureIndex{
    victory=1,
    closed=0,
}
public class SliderController : MonoBehaviour
{
    public static SliderController instance;
    public Sprite[]gestureImage;
    public Dictionary<string,Image>gestureDic;

    void Awake(){
        gestureDic=new Dictionary<string,Image>();
        instance=this;
        Messenger<float>.AddListener(GameEvents.LEFT_SLIDER_CHANGE,updateSliderLeft);
        Messenger<float>.AddListener(GameEvents.RIGHT_SLIDER_CHANGE,updateSliderRight);
         Messenger<bool>.AddListener(GameEvents.RIGHT_SLIDER_ACTIVE,activeSliderRight);
         Messenger<bool>.AddListener(GameEvents.LEFT_SLIDER_ACTIVE,activeSliderLeft);

        Messenger<gestureIndex>.AddListener(GameEvents.RIGHT_SLIDER_IMAGE_CHANGE,setGestureRight);
         Messenger<gestureIndex>.AddListener(GameEvents.LEFT_SLIDER_IMAGE_CHANGE,setGestureLeft);
    }
    void OnDestroy(){
        Messenger<float>.RemoveListener(GameEvents.LEFT_SLIDER_CHANGE,updateSliderLeft);
        Messenger<float>.RemoveListener(GameEvents.RIGHT_SLIDER_CHANGE,updateSliderRight);
        Messenger<bool>.RemoveListener(GameEvents.RIGHT_SLIDER_ACTIVE,activeSliderRight);
        Messenger<bool>.RemoveListener(GameEvents.LEFT_SLIDER_ACTIVE,activeSliderLeft);
         
         
        Messenger<gestureIndex>.RemoveListener(GameEvents.RIGHT_SLIDER_IMAGE_CHANGE,setGestureRight);
        Messenger<gestureIndex>.RemoveListener(GameEvents.LEFT_SLIDER_IMAGE_CHANGE,setGestureLeft);
    }

    public Slider SX;
     public Slider DX;
     

      public Image SX_innerImage;
     public Image DX_innerImage;
          public bool val=false;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        if(val){
            setGestureLeft(gestureIndex.victory);
        }
    }
    public void setGestureLeft(gestureIndex gi){
      
           SX_innerImage.sprite=gestureImage[(int)gi];
          
    }

     public void setGestureRight(gestureIndex gi){
      
           DX_innerImage.sprite=gestureImage[(int)gi];
           // GameObject.FindGameObjectWithTag("slider_left_image").GetComponent<Image>().sprite=gestureImage[(int)gi];
           // GameObject.Find("slider_left_image").GetComponent<Image>().sprite=gestureImage[(int)gi];
     //   SX.GetComponentsInChildren<Image>()[1].sprite=gestureImage[(int)gi];

   
    }
    public void updateSliderLeft(float value){
        
            SX.value=value;
    }
     public void updateSliderRight(float value){
        
            DX.value=value;
    }
    public void activeSliderLeft(bool value){
        
            SX.transform.parent.gameObject.SetActive(value);
    }
     public void activeSliderRight(bool value){
        
           DX.transform.parent.gameObject.SetActive(value);
    }


    
    public  Slider getSlider(string slider_left_or_right){
            if(slider_left_or_right=="Right")
            {
                
            
             return DX;
            }
        if(slider_left_or_right=="Left")
            return SX;

        return null;
            
    }
}
