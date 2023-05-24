using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Item",menuName ="becher")]
public class BecherScripatableObject : ScriptableObject
{

    public List<ScriptableObject>itemInside;
   public void Accept(IVisitor visitor){
    visitor.Visit(this);
   }

   public bool Riempi(ScriptableObject name){
    if(itemInside.Contains(name)){
                return false;
    }
    itemInside.Add(name);
    return true;
   }

    public bool Svuota(){
    if(itemInside.Count<1){
        return false;
    }
    itemInside.Clear();
    return true;
   }
    
}
