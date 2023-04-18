using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

public class PythonScriptLauncher : MonoBehaviour
{
    // Start is called before the first frame update
    Process p;
    void Start()
    {

     
         string m_Path = Application.dataPath +"\\main.py";
            UnityEngine.Debug.Log("START "+ m_Path);
           p=Process.Start("C:\\Users\\francesco.defazio\\AppData\\Local\\Programs\\Python\\Python39\\python.exe",m_Path);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

     void OnDestroy()
    {
        UnityEngine.Debug.Log("OnDestroy1");
        p.Kill();
    }
}
