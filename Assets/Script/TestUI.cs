using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestUI : MonoBehaviour
{
    // Start is called before the first frame update
    public Slider slider;
     float timer;
    public float timerDuration=2.0f;
    public bool finish=false;
    void Start()
    {
    slider.value=0;
    slider.maxValue=2f;
    
        
    }

    // Update is called once per frame
    void Update()
    {
        
      
        if(this.transform.gameObject.GetComponentInChildren<Slider>().IsActive()){
            
         timer += Time.deltaTime;
         
      
        
        slider.value =timer;


        if (timer >= 2)
        {
            // Azioni da eseguire quando il timer raggiunge 0
            timer = 2;
            
           
        }
    
    }else{
        
        fillSlider(0);
    }

    
    }

    public void fillSlider(float value){
      
   
        slider.value=value;
        timer=value;
        
        
    }
}
