using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTriggerTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private  void OnTriggerEnter(Collider other) {
        Debug.LogError("trigger colpito");
    }

  private  void OnCollisionEnter(Collision other) {
        Debug.LogError("collision colpito");
    }
    private  void OnTriggerStay(Collider other) {
        
        Debug.Log(other.transform.name);
    }
}
