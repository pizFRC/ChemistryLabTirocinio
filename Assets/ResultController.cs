using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResultController : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject glovesImageAndText;
    public GameObject gogglesImageAndText;
    public GameObject fumeHoodImageAndText;
    public SecurityController securityController;
    public GameObject esitoSuperatoText,esitoNonSuperatoSuperatoText;
    void Start()
    {
        if(securityController.glovesOn && securityController.isSafe && securityController.safetyGoggles){
            esitoSuperatoText.SetActive(true);
            esitoNonSuperatoSuperatoText.SetActive(false);
        }
        else{
            esitoSuperatoText.SetActive(false);
            esitoNonSuperatoSuperatoText.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(this.gameObject.activeSelf){
            glovesImageAndText.SetActive(!securityController.glovesOn);
            fumeHoodImageAndText.SetActive(!securityController.isSafe);
            gogglesImageAndText.SetActive(!securityController.safetyGoggles);
        }
    }
}
