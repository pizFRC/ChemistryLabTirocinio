using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reagente : MonoBehaviour
{
    [SerializeField]public string Nome ;
    public int Quantità { get; set; }

    public Reagente(string nome, int quantità)
    {
        Nome = nome;
        if(this.gameObject.TryGetComponent(out InteractableItem item))
        Nome =   item.item.name;

       
        Quantità = quantità;
    }

    public string getNome(){
        return Nome;
    }
     public void setNome(string value){
        this.Nome=value;
    }
}
