using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
public class LongClickInGame : MonoBehaviour
{
    // Start is called before the first frame update
    public float elapsedTime=0f;
    public float requiredTime=0f;
    public bool pressed;
    public RaycastItemSelector pointerRaycastItemSelector;
    
    [SerializeField]public Image image;
    [SerializeField]public UnityEvent onLongClick;

    // Start is called before the first frame update
    void Start()
    {   
        this.pressed=false;
        
    
    }

    // Update is called once per frame
    void Update()
    {
        if(pressed){
            elapsedTime+=Time.deltaTime;

            if(elapsedTime>= requiredTime){


                    if(onLongClick !=null ){
                        onLongClick.Invoke();
                     
                    }

                reset();

            }

            image.fillAmount=elapsedTime/requiredTime;

            return;
        }
        reset();
    }

    void reset(){
        this.elapsedTime=0f;
        this.pressed=false;
         image.fillAmount=0;
    }
}
