using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPositioning : MonoBehaviour
{
    public float distanceToFront;
    // Start is called before the first frame update
    void Start()
    {
          Vector3 newPosition = Camera.main.transform.position + Camera.main.transform.forward * distanceToFront;
        this.transform.position=newPosition;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
