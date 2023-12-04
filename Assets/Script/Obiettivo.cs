using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New obiettivo", menuName = "Obiettivo")]
public class Obiettivo : ScriptableObject
{
    [SerializeField] public string Name ;
    [SerializeField] public string Descrizione ;
    [SerializeField] public string CodiceObiettivo;
    [SerializeField] public int order ;


    bool isComplete;

    public void SetComplete()
    {
        isComplete = true;
    }

    // Start is called before the first frame update

}
