using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Globalization;
public class handControllerTest : MonoBehaviour
{
    // Start is called before the first frame update
    public PipeServer p=null;
    public GameObject[] sfere;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(p!=null){
                string[] lines = p.data.Split('\n');
            Debug.LogError(lines);
                foreach (string l in lines)
                {

                    string[] s = l.Split('|');
                    if (s.Length < 4) continue;
                    int i;
                   
                    if (!int.TryParse(s[1], out i)) continue;

                    CultureInfo ci = (CultureInfo)CultureInfo.CurrentCulture.Clone();
                    float x = float.Parse(s[2] ,CultureInfo.InvariantCulture.NumberFormat);
                    float y = float.Parse(s[3], CultureInfo.InvariantCulture.NumberFormat);
                    float z = float.Parse(s[4], CultureInfo.InvariantCulture.NumberFormat);
                    sfere[i].transform.localPosition=new Vector3(x*10,y*10,z);
                    Debug.Log(x +" /  " + y +" / "+z);
        }
        }
        }
}
