using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisitorBehaviour : IVisitor
{
    // Start is called before the first frame update
    
    

   public void Visit(BecherScripatableObject becher)
    {
       becher. function_Riempi(becher);
       Debug.Log("riempito");
       becher. function_Svuota();
    }
}
