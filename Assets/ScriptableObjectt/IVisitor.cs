using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IVisitor 
{
    // Start is called before the first frame update
   void Visit(BecherScripatableObject becher);
}
