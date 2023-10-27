using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using Unity.VisualScripting;
using UnityEditor.Media;
using UnityEngine;

public class SecurityController : MonoBehaviour
{


    public bool IsBecherInTheFumeHood;

    public Transform fumeHoodPosition;
    public Vector3 dim;
    public float radius;
    public bool glovesOn;
    public GameObject glovesOBJ,safetyGogglesOBJ;
    public bool safetyGoggles;
    public GameObject glovesImage,safetyGogglesImage,fumeHoodImage;

    float timeElapsed;


    // Start is called before the first frame update
    void Start()
    {
      safetyGoggles=false;
      glovesOn=false;
    }




    // Update is called once per frame
    void Update()
    {       
        if (this.gameObject.activeSelf)
        {       

           glovesOBJ.SetActive(!glovesOn);
           safetyGogglesOBJ.SetActive(!safetyGoggles);
            glovesImage.SetActive(!glovesOn);
            safetyGogglesImage.SetActive(!safetyGoggles);
            

            if (fumeHoodPosition == null)
                return;


           bool isSafe=false;
            Collider []colls =Physics.OverlapBox(fumeHoodPosition.position,dim);
            foreach(Collider c in colls){
             

                if(c.TryGetComponent(out Contenitore contenitore)){
                    isSafe=true;
                }      
            }
          

            fumeHoodImage.SetActive(!isSafe);
        }

    }

    void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        if (fumeHoodPosition != null)
        Gizmos.DrawCube(fumeHoodPosition.position,dim);
          
    }

    public void SetGlovesOn(bool value){
        glovesOn=value;
    }
       public void SetSafetyGoggles(bool value){
        safetyGoggles=value;
    }
}
