using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCamera : MonoBehaviour
{

    public GameObject handsObject,leftArraow,rightArrow;
    bool alreadyRotateSecondoAgo=false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
   public void rotate(float rotationAngle){

           if(!alreadyRotateSecondoAgo){
            StartCoroutine("waitAndRotate",rotationAngle);
           }else{
            Debug.LogError("non puoi farlo s enon passano 2,5 secondo");
           }
      
    }
    public IEnumerator waitAndRotate(float rotationAngle){

            alreadyRotateSecondoAgo=true;
            this.transform.Rotate(new Vector3(0,rotationAngle,0));
            leftArraow.transform.RotateAround(this.transform.position,new Vector3(0,1,0),rotationAngle);
            rightArrow.transform.RotateAround(this.transform.position,new Vector3(0,1,0),rotationAngle);
            handsObject.transform.RotateAround(this.transform.position,new Vector3(0,1,0),rotationAngle);


            yield return new WaitForSeconds(2.5f);
            alreadyRotateSecondoAgo=false;
            
            }
}
