using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using System;
using UnityEngine.UI;
public class GestureUIFiller : MonoBehaviour
{
    // Start is called before the first frame update
  
    public  GameObject prototipo;

    public ScriptableObject becher;
     static string  lastF;
  int t=0;
    // Update is called once per frame
    void Update()
    {
        if(t>0)
        return;
        if(this.isActiveAndEnabled ){
            t+=1;
            GetFunctions(becher);
            prototipo.GetComponentInChildren<Text>().text=lastF;
        }
    }

    public  static  void GetFunctions(ScriptableObject so){
        Type soType=so.GetType();
        MethodInfo[] methods=soType.GetMethods(BindingFlags.DeclaredOnly);
        foreach(MethodInfo m  in methods){
            Debug.LogError(m.Name);
            lastF=m.Name;
        }
    }
}
