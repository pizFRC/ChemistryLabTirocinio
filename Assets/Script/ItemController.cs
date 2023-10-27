using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemController : MonoBehaviour
{

    public GameObject[] gameObjectPrefab;
    public  static ItemController instance; 
    // Start is called before the first frame update

    void Awake(){

        instance=this;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
