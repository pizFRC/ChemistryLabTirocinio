using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum gestureIndex
{
    GET = 0,
    PUT = 1,

    USE = 2,
}
public class UIController : MonoBehaviour
{
    public static UIController instance;

    public TakenItemImageController takenItemImageController;

   
    public Sprite[] gestureImage;
    public Dictionary<string, Image> gestureDic;


  
    public bool val = false;

   
     void Awake()
    {
        gestureDic = new Dictionary<string, Image>();
        instance = this;
    }
  
    // Start is called before the first frame update
    void Start()
    {

    }


    // Update is called once per frame
    void Update()
    {
      
    }



    
   



    

}
