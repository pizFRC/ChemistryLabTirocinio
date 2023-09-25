using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCamera : MonoBehaviour
{

    public GameObject handsObject, leftArraow, rightArrow,camera;
    bool alreadyRotateSecondoAgo = false;
    public bool rotateManual=false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
                if(rotateManual){
                    rotate(-90f);
                    rotateManual=false;
                }
    }
    public void rotate(float rotationAngle)
    {

        if (!alreadyRotateSecondoAgo)
        {
            StartCoroutine("waitAndRotate", rotationAngle);
        }
        else
        {
            Debug.LogError("non puoi farlo s enon passano 2 secondi");
        }

    }
    public IEnumerator waitAndRotate(float rotationAngle)
    {

        alreadyRotateSecondoAgo = true;
      //  this.transform.Rotate(new Vector3(0, rotationAngle, 0));
      camera.transform.RotateAround(this.transform.position, new Vector3(0, 1, 0), rotationAngle);
        leftArraow.transform.RotateAround(this.transform.position, new Vector3(0, 1, 0), rotationAngle);
        rightArrow.transform.RotateAround(this.transform.position, new Vector3(0, 1, 0), rotationAngle);
        handsObject.transform.RotateAround(this.transform.position, new Vector3(0, 1, 0), rotationAngle);
      

        yield return new WaitForSeconds(2f);
        alreadyRotateSecondoAgo = false;

    }
}
