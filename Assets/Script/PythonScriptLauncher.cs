using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System.IO;

public class PythonScriptLauncher : MonoBehaviour
{
    // Start is called before the first frame update
    Process p;
    bool debug=false;
    void Start()
    {
         
       
       // string m_Path= ("C:\\Users\\fdefa\\Desktop\\tirocinio_versato\\tirocinio_testing_versionato\\Assets");
       
           // UnityEngine.Debug.Log(m_Path);
            string path1="C:\\Users\\francesco.defazio\\AppData\\Local\\Programs\\Python\\Python39\\python.exe";
            string path2="C:\\Users\\fdefa\\Desktop\\ChemLabTirocinio\\venv\\Scripts\\python.exe";
           
        //  p=Process.Start("C:\\Users\\fdefa\\Desktop\\ChemLabTirocinio\venv\\Scripts\\python.exe,");
                  //p=Process.Start("dir",Application.dataPath);

         string m_Path = Application.dataPath +"\\Resources\\main.py";
            UnityEngine.Debug.Log("START "+ m_Path);
            if (debug){
                try{
            string path="C:\\Users\\fdefa\\Desktop\\HandTracking\\main.exe";
            p=Process.Start(path1);
                }catch{
                    UnityEngine.Debug.LogError("errore");
                }
         }else
           p=Process.Start(path1,m_Path);

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


       
