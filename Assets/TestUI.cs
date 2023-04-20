using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestUI : MonoBehaviour
{
    // Start is called before the first frame update
    public Slider slider;
    public float timer;
    public float timerDuration=2.0f;
    void Start()
    {
    slider.value=0;
    slider.maxValue=timerDuration;
        
    }

    // Update is called once per frame
    void Update()
    {
        if(this.isActiveAndEnabled){
         timer += Time.deltaTime;
        
        
        slider.value =timer;


        if (timer >= 2)
        {
            // Azioni da eseguire quando il timer raggiunge 0
            timer = 2;
        }
    }
    }
    public void fillSlider(float value){
       // Debug.LogError(value);
   
        slider.value=Mathf.Clamp01(value);
    }
}
