using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestUI : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other){
        print(other);
        
    }
     private void OnTriggerStay(Collider other){
        print("fermo nel canvas");
        
    }
    public void printto(){
        Debug.Log("TEST\n");
    }
}
