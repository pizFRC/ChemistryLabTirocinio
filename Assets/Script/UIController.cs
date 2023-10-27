using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR;

public enum gestureIndex
{
    GET = 0,
    PUT = 1,

    USE = 2,
}
public class UIController : MonoBehaviour
{
    public static UIController instance;


    public GameObject gesturePanel;
    public TMP_Text handTitleGesturePanelPRO;
    public Text handTitleGesturePanel;
    public GameObject lab_prefab;
    public GameObject alertTextLeft, alertTextRight;
    public GameObject menuButtonPanel;
    public GameObject tutorialPanel;
    public GameObject reactionPanel;
    public GameObject gesture_panel_item;
    public GameObject leftHandStatus;
    public GameObject securityToolPanel;
    public GameObject rightHandStatus;
    public bool stopRaycastWhileUiIsOpen;
    [SerializeField] public UnityEvent loadTutorial;

    public bool val = false;
    public bool gesturePanelActiveLeft, gesturePanelActiveRight, exitLeft = false, exitOpened = false, tutorialOpened, experiment, notInUi = false;
    public bool menuOpen;

    public float time;
    public bool grabNextStepInTutorial;

    public void setExperiment(bool value)
    {
        experiment = value;
    }
    public void setTutorialOpen(bool value)
    {
        tutorialOpened = value;

    }
    public void setExitOpen(bool value)
    {
        exitOpened = value;
    }

    public void setstopRaycastWhileUiIsOpen(bool value)
    {
        this.stopRaycastWhileUiIsOpen = value;
    }

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

                    
        Messenger<bool>.AddListener(GameEvents.SHOW_REACTION_PANEL, ShowReactionPanel);
        Messenger<bool>.AddListener(GameEvents.MISSING_HAND_LEFT, displayAlertMessageLeft);
        Messenger<bool>.AddListener(GameEvents.MISSING_HAND_RIGHT, displayAlertMessageRight);
        Messenger<bool>.AddListener(GameEvents.HAND_WRONG_DISTANCE_RIGHT, DisplayMessageWrongDistanceRight);
        Messenger<bool>.AddListener(GameEvents.HAND_WRONG_DISTANCE_LEFT, DisplayMessageWrongDistanceLeft);
        Messenger<string>.AddListener(GameEvents.HAND_WRONG_DISTANCE_TEXT_LEFT, changeMessageWrongDistanceLeft);
        Messenger<string>.AddListener(GameEvents.HAND_WRONG_DISTANCE_TEXT_RIGHT, changeMessageWrongDistanceRight);
        Messenger<Gesture>.AddListener(GameEvents.UI_GESTURE, ManageHandGesture);
        Messenger<bool>.AddListener(GameEvents.DISPLAY_GESTURE_PANEL, displayGesturePanel);
        instance = this;
    }

    void OnDestroy()
    {
        Messenger<Gesture>.RemoveListener(GameEvents.UI_GESTURE, ManageHandGesture);
        Messenger<bool>.RemoveListener(GameEvents.SHOW_REACTION_PANEL, ShowReactionPanel);

        Messenger<bool>.RemoveListener(GameEvents.MISSING_HAND_RIGHT, displayAlertMessageRight);
        Messenger<bool>.RemoveListener(GameEvents.MISSING_HAND_LEFT, displayAlertMessageLeft);

        Messenger<bool>.RemoveListener(GameEvents.HAND_WRONG_DISTANCE_LEFT, DisplayMessageWrongDistanceLeft);
        Messenger<bool>.RemoveListener(GameEvents.HAND_WRONG_DISTANCE_RIGHT, DisplayMessageWrongDistanceRight);


        Messenger<string>.RemoveListener(GameEvents.HAND_WRONG_DISTANCE_TEXT_RIGHT, changeMessageWrongDistanceRight);
        Messenger<string>.RemoveListener(GameEvents.HAND_WRONG_DISTANCE_TEXT_LEFT, changeMessageWrongDistanceLeft);


        Messenger<bool>.RemoveListener(GameEvents.DISPLAY_GESTURE_PANEL, displayGesturePanel);

    }



void ShowReactionPanel(bool value){
  reactionPanel.SetActive(value);
}
    void changeMessageWrongDistanceLeft(string value)
    {
        alertTextLeft.GetComponentInChildren<Text>().text = value;
    }
    void changeMessageWrongDistanceRight(string value)
    {
        alertTextRight.GetComponentInChildren<Text>().text = value;
    }



    void DisplayMessageWrongDistanceLeft(bool value)
    {
        alertTextLeft.SetActive(value);
    }
    void DisplayMessageWrongDistanceRight(bool value)
    {
        alertTextRight.SetActive(value);
    }
    void displayAlertMessageRight(bool value)
    {


        rightHandStatus.SetActive(value);

        if (menuOpen)
        {
            HandController.instance.rightHandSelector.mode = selectorMode.CannotSelect;
            return;
        }
        if (notInUi)
        {

            bool itemSelected = HandController.instance.rightHandSelector.lastInteractableItemSelected != null;
            if (!value && itemSelected)
            {
                return;
            }
        }
        if (!notInUi)
        {
            if (!value)
            {


                HandController.instance.rightHandSelector.mode = selectorMode.CanSelect;
                return;
            }

            HandController.instance.rightHandSelector.mode = selectorMode.CannotSelect;


        }


    }

    void displayAlertMessageLeft(bool value)
    {

        leftHandStatus.SetActive(value);
        if (menuOpen)
        {
            HandController.instance.leftHandSelector.mode = selectorMode.CannotSelect;
            return;
        }

        if (notInUi)
        {

            bool itemSelected = HandController.instance.leftHandSelector.lastInteractableItemSelected != null;
            if (!value && itemSelected)
            {
                return;
            }
        }

        if (!notInUi)
        {
            if (!value)
            {

                HandController.instance.leftHandSelector.mode = selectorMode.CanSelect;
                return;
            }

            HandController.instance.leftHandSelector.mode = selectorMode.CannotSelect;

        }



    }

    void displayGesturePanel(bool value)
    {
        UnityMainThreadDispatcher.Instance().Enqueue(() => gesture_panel_item.SetActive(value));
    }
    // Start is called before the first frame update
    void Start()
    {   grabNextStepInTutorial=false;
        time = 0.0f;
        alertTextRight.SetActive(false);
        alertTextLeft.SetActive(false);
        if (gesturePanel == null) this.gesturePanel = GameObject.FindWithTag("gesture_panel");
        if (this.leftHandStatus == null) this.leftHandStatus = GameObject.FindWithTag("panel_left");
        if (this.rightHandStatus == null) this.rightHandStatus = GameObject.FindWithTag("panel_right");
    }


    // Update is called once per frame
    void Update()
    {
        string hand="";
        time += Time.deltaTime;

        if(gesturePanelActiveLeft)
            hand="Left";
        if(gesturePanelActiveRight)
            hand="Right";

        if(gesture_panel_item.activeInHierarchy){
         handTitleGesturePanelPRO.text=hand;
           // handTitleGesturePanel.text=hand;
        }

           if(tutorialPanel.activeInHierarchy){

                grabNextStepInTutorial=true;

           }else{
            grabNextStepInTutorial=false;
           }

    }
    void loadSceneExperiment()
    {
        this.lab_prefab.SetActive(true);
        securityToolPanel.SetActive(true);
        menuOpen = false;
        activeRaycast();
        notInUi = true;
        gesture_panel_item.SetActive(false);

        menuButtonPanel.SetActive(false);
        //  SceneManager.LoadScene("SampleScene");
    }
    public void setHandGesturePanel(RaycastItemSelector ris)

    {
        if (gesturePanelActiveLeft || gesturePanelActiveRight)
        {

            Debug.LogError("already selected button");
            return;

        }
        if (ris.hand == "Left")
        {

            gesturePanelActiveRight = false;
            gesturePanelActiveLeft = true;


            return;
        }


        gesturePanelActiveRight = true;

        gesturePanelActiveLeft = false;


    }

    public void setHandGesturePanel(LongClick longClickRis)

    {
        if (gesturePanelActiveLeft || gesturePanelActiveRight)
        {

            Debug.LogError("already selected button");
            //mostrare un messaggio
            return;

        }
        this.menuOpen = true;
        this.disactiveRaycast();
        if (longClickRis.pointerRaycastItemSelector.hand == "Left")
        {

            gesturePanelActiveRight = false;
            gesturePanelActiveLeft = true;

            return;
        }
        gesturePanelActiveRight = true;

        gesturePanelActiveLeft = false;


    }


    void ManageHandGesture(Gesture gesture)
    {
        if (notInUi)
            return;
        switch (gesture)
        {
            case Gesture.GrabRight:
                if (gesturePanelActiveRight)
                {
                    //parte il tutorial
                    if(grabNextStepInTutorial){
                    print("prossimo step");
                       UnityMainThreadDispatcher.Instance().Enqueue(() =>tutorialPanel.GetComponent<TutorialController>().nextStep());
                    return;
                    }
                    if (tutorialOpened)
                        UnityMainThreadDispatcher.Instance().Enqueue(() => OpenTutorial());
                    if (exitOpened)
                    {
                        UnityMainThreadDispatcher.Instance().Enqueue(() => Application.Quit());
                        Debug.LogError("exit");
                    }
                    if (experiment)
                        UnityMainThreadDispatcher.Instance().Enqueue(() => loadSceneExperiment());



                }


                break;
            case Gesture.GrabLeft:
                if (gesturePanelActiveLeft)
                {
                    //parte il tutorial
                    if(grabNextStepInTutorial){
                    print("prossimo step");
                        UnityMainThreadDispatcher.Instance().Enqueue(() =>tutorialPanel.GetComponent<TutorialController>().nextStep());
                      return;
                    }
                    if (tutorialOpened)
                        UnityMainThreadDispatcher.Instance().Enqueue(() => OpenTutorial());
                    if (exitOpened)
                    {
                        UnityMainThreadDispatcher.Instance().Enqueue(() => Application.Quit());
                        Debug.LogError("exit");
                    }
                    if (experiment)
                        UnityMainThreadDispatcher.Instance().Enqueue(() => loadSceneExperiment());

                }
                break;
            case Gesture.ReleaseRight:

                if (gesturePanelActiveRight)
                {

                    UnityMainThreadDispatcher.Instance().Enqueue(() => SetGesturePanelNotActive());

                    UnityMainThreadDispatcher.Instance().Enqueue(() => Reset());
                }
                break;
            case Gesture.ReleaseLeft:

                if (gesturePanelActiveLeft)
                {

                    UnityMainThreadDispatcher.Instance().Enqueue(() => SetGesturePanelNotActive());


                    UnityMainThreadDispatcher.Instance().Enqueue(() => Reset());
                }




                break;
        }
    }
    void OpenTutorial()
    {


        disactiveRaycast();
        tutorialPanel.GetComponent<TutorialController>().reset();
        tutorialPanel.SetActive(true);
        gesturePanel.SetActive(false);
        menuButtonPanel.SetActive(false);
        gesture_panel_item.SetActive(false);
        print("opentutorial");

        gesturePanelActiveLeft = false;
        gesturePanelActiveRight = false;

    }

    void SetGesturePanelNotActive()
    {

        gesturePanelActiveLeft = false;
        gesturePanelActiveRight = false;

        gesturePanel.SetActive(false);
        menuButtonPanel.SetActive(true);
        gesture_panel_item.SetActive(false);

    }

    public void disactiveRaycast()
    {
        if (HandController.instance.rightHandSelector.lastInteractableItemSelected == null)
            HandController.instance.rightHandSelector.mode = selectorMode.CannotSelect;
        if (HandController.instance.leftHandSelector.lastInteractableItemSelected == null)
            HandController.instance.leftHandSelector.mode = selectorMode.CannotSelect;
    }

    public void activeRaycast()
    {

        if (HandController.instance.rightHandSelector.lastInteractableItemSelected == null)
            HandController.instance.rightHandSelector.mode = selectorMode.CanSelect;
        if (HandController.instance.leftHandSelector.lastInteractableItemSelected == null)
            HandController.instance.leftHandSelector.mode = selectorMode.CanSelect;
    }


    public void Reset()
    {
        tutorialOpened = false;

        activeRaycast();
        exitOpened = false;
        menuOpen = false;
        gesturePanelActiveLeft = false;
        gesturePanelActiveRight = false;
        experiment = false;
    }







}
