using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayselectorPointerObject : MonoBehaviour
{
    // Start is called before the first frame update

     public int numberOfCollider;
     public int maxCollider=2;
     float timeElapsed=0;
    // Collider[] colliders2;
    void Start()
    {
     
       numberOfCollider=0;
      
    }

    // Update is called once per frame
    void Update()
    {
        
    }


     private IEnumerator resetAfterOpen(){
        
            yield return new WaitForSeconds(0.5f);
           //  startPositionTransform.transform.parent.transform.localScale = newScaleTMP;
     }
}
