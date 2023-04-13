using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandColor : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject left_hand;
    public GameObject right_hand;
    void Start()
    {
       

          
    }

    // Update is called once per frame
    void Update()
    {
         left_hand.transform.GetChild(0).GetComponent<Renderer>().material.SetColor("_Color", Color.blue);
        left_hand.transform.GetChild(5).GetComponent<Renderer>().material.SetColor("_Color", Color.blue);
        left_hand.transform.GetChild(9).GetComponent<Renderer>().material.SetColor("_Color", Color.blue);
        left_hand.transform.GetChild(13).GetComponent<Renderer>().material.SetColor("_Color", Color.blue);
        left_hand.transform.GetChild(17).GetComponent<Renderer>().material.SetColor("_Color", Color.blue);
        right_hand.transform.GetChild(0).GetComponent<Renderer>().material.SetColor("_Color", Color.blue);
        right_hand.transform.GetChild(5).GetComponent<Renderer>().material.SetColor("_Color", Color.blue);
        right_hand.transform.GetChild(9).GetComponent<Renderer>().material.SetColor("_Color", Color.blue);
        right_hand.transform.GetChild(13).GetComponent<Renderer>().material.SetColor("_Color", Color.blue);
        right_hand.transform.GetChild(17).GetComponent<Renderer>().material.SetColor("_Color", Color.blue);
    }
}
