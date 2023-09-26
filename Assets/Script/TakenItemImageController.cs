using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class TakenItemImageController : MonoBehaviour
{
    // Start is called before the first frame update
    public Image LeftHandImage;
    public Image RightHandImage;

    public GameObject sxGestureMenu;
    public GameObject dxGestureMenu;
    void Awake()
    {


        Messenger<Sprite>.AddListener(GameEvents.LEFT_ITEM_IMAGE_CHANGE, updateImageItemLeftHand);
        Messenger<Sprite>.AddListener(GameEvents.RIGHT_ITEM_IMAGE_CHANGE, updateImageItemRightHand);
        Messenger<bool>.AddListener(GameEvents.GESTURE_MENU_RIGHT, activeUiGestureDX);
        Messenger<bool>.AddListener(GameEvents.GESTURE_MENU_LEFT, activeUiGestureSX);
      
         


    }
    void OnDestroy()
    {
        Messenger<Sprite>.RemoveListener(GameEvents.LEFT_ITEM_IMAGE_CHANGE, updateImageItemLeftHand);
        Messenger<Sprite>.RemoveListener(GameEvents.RIGHT_ITEM_IMAGE_CHANGE, updateImageItemRightHand);
        Messenger<bool>.RemoveListener(GameEvents.GESTURE_MENU_RIGHT, activeUiGestureDX);
        Messenger<bool>.RemoveListener(GameEvents.GESTURE_MENU_LEFT, activeUiGestureSX);
    }
    private void updateImageItemLeftHand(Sprite spriteItem)
    {
        UnityMainThreadDispatcher.Instance().Enqueue(() =>LeftHandImage.sprite = spriteItem);
    }
    private void updateImageItemRightHand(Sprite spriteItem)
    {
      
        UnityMainThreadDispatcher.Instance().Enqueue(() =>RightHandImage.sprite = spriteItem);
       
    }
private void activeUiGestureDX(bool value)
    {
    
                dxGestureMenu.SetActive(value);
            



 }


private void activeUiGestureSX(bool value)
    {
    
                sxGestureMenu.SetActive(value);
            



 }



    


    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
