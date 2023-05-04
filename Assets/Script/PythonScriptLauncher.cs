using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System.IO;

public class PythonScriptLauncher : MonoBehaviour
{
    // Start is called before the first frame update
    Process p;
    public bool debug = false;
    public bool pcUfficio=false;
    void Start()
    {


        // string m_Path= ("C:\\Users\\fdefa\\Desktop\\tirocinio_versato\\tirocinio_testing_versionato\\Assets");

        // UnityEngine.Debug.Log(m_Path);
        string pathUfficio = "C:\\Users\\francesco.defazio\\AppData\\Local\\Programs\\Python\\Python39\\python.exe";
        string pathCasa= "C:\\Users\\fdefa\\Desktop\\ChemLabTirocinio\\venv\\Scripts\\python.exe";

        string chosenPath  = pcUfficio ? pathUfficio : pathCasa ;
       
        //  p=Process.Start("C:\\Users\\fdefa\\Desktop\\ChemLabTirocinio\venv\\Scripts\\python.exe,");
        //p=Process.Start("dir",Application.dataPath);
        string assetsPath = Application.streamingAssetsPath;
        string appPath = assetsPath + "/pyHandTracking/main/main.exe";
        string filePath = Path.Combine(Application.streamingAssetsPath, "main/main.exe");
        print("percorso"+filePath);
        string m_Path = Application.dataPath + "\\Resources\\main.py";
        UnityEngine.Debug.Log("START " + m_Path);
        if (!debug)
        {
            try
            {
                string path = "C:\\Users\\fdefa\\Desktop\\HandTracking\\main.exe";
                p = Process.Start(filePath);
            }
            catch
            {
                UnityEngine.Debug.LogError("errore");
            }
        }
        else
            p = Process.Start(chosenPath, m_Path);

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



