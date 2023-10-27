using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
public class LastGestureCaptured : MonoBehaviour


{

    public Sprite[] images;
    public Sprite nullSprite;
    public Image leftImage;
    public Image rightImage;

    Gesture lastLeftGesture, lastRightGesture;
    // Start is called before the first frame update
    void Start()
    {

        lastLeftGesture=Gesture.NoneLeft;
           lastRightGesture=Gesture.NoneRight;
        leftImage.sprite = null;
        rightImage.sprite = null;
    }

    void Awake()
    {
        Messenger<Gesture>.AddListener(GameEvents.GRAB, ManageGesture);
        Messenger<Gesture>.AddListener(GameEvents.NONE, ManageGesture);
        Messenger<Gesture>.AddListener(GameEvents.RELEASE, ManageGesture);
        Messenger<Gesture>.AddListener(GameEvents.INTERACT, ManageGesture);
           Messenger<Gesture>.AddListener(GameEvents.OPEN_PALM, ManageGesture);
    }
    void OnDestroy()
    {
        Messenger<Gesture>.RemoveListener(GameEvents.GRAB, ManageGesture);
        Messenger<Gesture>.RemoveListener(GameEvents.RELEASE, ManageGesture);
        Messenger<Gesture>.RemoveListener(GameEvents.NONE, ManageGesture);
        Messenger<Gesture>.RemoveListener(GameEvents.INTERACT, ManageGesture);
         Messenger<Gesture>.RemoveListener(GameEvents.OPEN_PALM, ManageGesture);
    }

    public void ManageGesture(Gesture gesture)
    {
        switch (gesture)
        {
            case (Gesture.GrabRight):
            case (Gesture.ReleaseRight):
            case (Gesture.InteractRight):
            case (Gesture.NoneRight):
            case (Gesture.OpenPalmRight):
                lastRightGesture = gesture;
                break;
            case (Gesture.InteractLeft):
            case (Gesture.GrabLeft):
            case (Gesture.ReleaseLeft):
            case (Gesture.NoneLeft):
            case (Gesture.OpenPalmLeft):
                lastLeftGesture = gesture;
                break;




        }


    }
    // Update is called once per frame
    void Update()
    {

        switch (lastLeftGesture)
        {
            case (Gesture.InteractLeft):
                leftImage.sprite = images[0];
                break;
            //;
            case (Gesture.GrabLeft):
                leftImage.sprite = images[1];
                break;
            case (Gesture.ReleaseLeft):
                leftImage.sprite = images[2];
                break;
            case (Gesture.OpenPalmLeft):
                leftImage.sprite = images[3];
                break;

            case (Gesture.NoneLeft):
                leftImage.sprite = nullSprite;
                break;
        }
        switch (lastRightGesture)
        {
            case (Gesture.InteractRight):
                rightImage.sprite = images[0];
                break;
            case (Gesture.GrabRight):
                rightImage.sprite = images[1];
                break;
            case (Gesture.ReleaseRight):
                rightImage.sprite = images[2];
                break;
            case (Gesture.OpenPalmRight):
                rightImage.sprite = images[3];
                break;


            case (Gesture.NoneRight):
                rightImage.sprite = nullSprite;
                break;
        }
    }
}
