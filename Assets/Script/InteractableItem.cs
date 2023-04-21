using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableItem : MonoBehaviour
{
    // Start is called before the first frame update
    public Material original;
    public Material selected;

    public Transform player;
    Transform canvasLocalPosition;
    public bool isTrigger = false;
    public GameObject localCanvas;
    GameObject instance;
    bool canvasNotActive = true;

   bool materialSetted=false;
    void Start()
    {
        var renderers = this.gameObject.GetComponents<Renderer>();
       
        Transform objTransform = this.transform;
        instance = Instantiate(localCanvas);
        instance.SetActive(false);
        instance.transform.position = new Vector3(objTransform.position.x, objTransform.position.y + 1f, objTransform.position.z - 0.3f);
    }

    // Update is called once per frame
    void Update()
    {
        if(isTrigger)
            localCanvas.SetActive(true);
    }


    
    
    public void showCanvas()
    {
        instance.SetActive(true);
     
    }
    public void hideCanvas()
    {
        instance.SetActive(false);
       
    }


    public void changeMaterial(bool selected_material)
    {
        var renderer = this.gameObject.GetComponents<Renderer>();
        foreach (Renderer r in renderer)
        {

            if (selected_material)
            {
                r.material = selected;
            }
            else
            {
                r.material = original;
            }
        }
    }




}
