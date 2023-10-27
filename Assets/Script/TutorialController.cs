using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class TutorialController : MonoBehaviour
{


    public GameObject[]list;
    public int currentIndex;

    public GameObject imageNextStep;
   public float elapsedTime=0f;

    [SerializeField]public Image fillImage;
    public float requiredTimeForSlide=8f;
    // Start is called before the first frame update
    void Start()
    {   
        Debug.LogError(list.Length);
         reset();
    
    }
    public void reset(){
       
        for(int i=currentIndex+1;i<list.Length;++i){
            list[i].SetActive(false);
        }
        currentIndex=0;
        fillImage.fillAmount=0f;
       list[currentIndex].SetActive(true);
    }
    // Update is called once per frame
    void Update()
    {      
       

        elapsedTime+=Time.deltaTime;
        fillImage.fillAmount=elapsedTime/requiredTimeForSlide;

        if(elapsedTime>=requiredTimeForSlide){
            
            fillImage.fillAmount=requiredTimeForSlide;
            imageNextStep.SetActive(true);
           
        }
      
    }

    public void nextStep(){
        if(elapsedTime<requiredTimeForSlide)
        return;
        currentIndex++;

        if(currentIndex>=list.Length){
            reset();
            
             UIController.instance.gesturePanel.SetActive(false);
             UIController.instance.menuButtonPanel.SetActive(true);
             UIController.instance.setstopRaycastWhileUiIsOpen(false);
             UIController.instance.setTutorialOpen(false);
             this.gameObject.SetActive(false);
             this.imageNextStep.SetActive(false);
        return;
        }


        


        for(int i=0;i<list.Length;++i){
            list[i].SetActive(false);
        }

      
        
        list[currentIndex].SetActive(true);
        fillImage.fillAmount=0;
        elapsedTime=0;
        imageNextStep.SetActive(false);
    }



    private void OnDisable() {
        UIController.instance.activeRaycast();
        UIController.instance.menuOpen=false;
    }
}
