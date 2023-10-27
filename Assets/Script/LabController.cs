using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class LabController : MonoBehaviour
{

    public static LabController instance;

    public int  emptySpace=1;
    public Transform[] emptySpacePosition;
    public GameObject [] item;
    public GameObject [] emptySpaces;
    public GameObject emptySpacePrefab;
    // Start is called before the first frame update

    void Awake(){
        instance=this;
        item=GameObject.FindGameObjectsWithTag("Item");
        MeshRenderer[]meshes;
        foreach(GameObject g in item){

            meshes = g.GetComponentsInChildren<MeshRenderer>();
            print(meshes.Length);
            
            foreach(MeshRenderer m in meshes)
            {
            
                MeshFilter meshFilter = m.GetComponent<MeshFilter>();

            if (meshFilter != null)
            {
                // Ottieni la mesh dall'oggetto MeshFilter.
                Mesh mesh = meshFilter.mesh;
                

                // Adesso hai accesso alla mesh, puoi fare quello che desideri con essa.
            }
            }
            if(meshes.Length>=0)
                if(!meshes[0].transform.parent.TryGetComponent(out Outline outline)){
            Outline addedOutline=g.AddComponent<Outline>();
                        
                            addedOutline.enabled=false;
                            addedOutline.OutlineColor=Color.red;
                            addedOutline.OutlineWidth=7.0f;
                }
                
            
        }
    }
    
    void Start()
    {   
              
        for(int i=0;i<emptySpacePosition.Length;i++)
        {
            emptySpaces.Append(Instantiate(emptySpacePrefab,emptySpacePosition[i]));
           
        }


        

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void changePosition(Transform emptySpace,Transform item){

        Vector3 tmp= emptySpace.position;

        emptySpace.position=item.position;

        item.position=tmp;
    }
}
