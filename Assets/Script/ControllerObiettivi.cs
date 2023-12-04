using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class ControllerObiettivi : MonoBehaviour
{
    public Obiettivo[]listaObiettivi;
    public List<Obiettivo>listaObiettiviCompletati;
    string obiettivoAttuale;

    public TMP_Text obiettivoAttualeText;
       public TMP_Text obiettivoAttualeDescrizioneText;
public TMP_Text[] textObiettivi;
    int indexObiettivoAttuale;
Obiettivo obiettivoAtt;
   public  GameObject panelObiettivi;
    void Start()
    {
    obiettivoAttuale="";
    indexObiettivoAttuale=0;

    for(int i=0;i<listaObiettivi.Length;i++){
        if(listaObiettivi[i].order==0){
            obiettivoAtt=listaObiettivi[i];
        }
    }
    
        obiettivoAttualeText.text=obiettivoAtt.Name;
        obiettivoAttualeDescrizioneText.text=obiettivoAtt.Descrizione;
      

    }

void Awake(){
    for(int i=0;i<listaObiettivi.Length;i++){
    Messenger<string>.AddListener(listaObiettivi[i].CodiceObiettivo,ManageObiettivo);
    }
}
void OnDestroy(){
    for(int i=0;i<listaObiettivi.Length;i++){
    Messenger<string>.RemoveListener(listaObiettivi[i].CodiceObiettivo,ManageObiettivo);
    }
}

void ManageObiettivo(string codiceObiettivo){
      
        foreach(Obiettivo o in listaObiettivi){
            
           
            if(o.CodiceObiettivo==codiceObiettivo){
                listaObiettiviCompletati.Add(o);
            }
        }
       


}
 
    // Update is called once per frame
    void Update()
    {
       
        
    }



}
