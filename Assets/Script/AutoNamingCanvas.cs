using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AutoNamingCanvas : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if(this.gameObject.activeInHierarchy)
        this.gameObject.GetComponentInChildren<TMP_Text>().text=this.transform.parent.GetComponent<InteractableItem>().item.name;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
