using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


using UnityEngine.UI;

public class LongClick : MonoBehaviour
{

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
                     /*    UIController.instance.setHandGesturePanel(pointerRaycastItemSelector);
                        UIController.instance.disactiveRaycast();
                       // Messenger<bool>.Broadcast(GameEvents.GESTURE_PANEL,true);
                        UIController.instance.menuOpen=true; */
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
