using System.Collections;
using System.Collections.Generic;
using Lean.Transition;
using UnityEngine;
using UnityEngine.UIElements;

public class Contenitore : MonoBehaviour
{


    public List<GameObject> itemInside;

    bool containsWater, containsSodium;
    public List<Reagente> Reagenti { get; set; }

    public bool containsReagente = false;

    public ParticleSystem ps_smoke, ps_flame;
    public Contenitore()
    {
        this.itemInside = new List<GameObject>();
        this.Reagenti = new List<Reagente>();
    }


    public bool refill(Reagente r)
    {

        foreach (GameObject g in ItemController.instance.gameObjectPrefab)
        {
            if (g.tag == r.Nome)
            {
                if (!Reagenti.Contains(r))

                {
                    Reagenti.Add(r);
                    GameObject tmp = GameObject.Instantiate(g, this.transform);
                    tmp.tag = g.tag;
                    tmp.transform.parent = this.transform;


                    itemInside.Add(tmp);
                    Reaction();
                    return true;
                }
            }



        }

        return false;



    }

    public void clear()
    {
        Reagenti.Clear();
        foreach (GameObject child in itemInside)
        {
            Destroy(child);
        }
        itemInside.Clear();

    }




    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void Reaction()
    {

        containsWater = false;
        containsSodium = false;
        foreach (GameObject child in itemInside)
        {


            if (child.tag == "Acqua")
                containsWater = true;

            if (child.tag == "Sodio")
                containsSodium = true;
        }

        if (containsWater && containsSodium)
        {
            foreach (GameObject child in itemInside)
            {
                if (child.tag == "Sodio")
                    child.transform.RotateAround(Vector3.up, 30 * Time.deltaTime);
            }



            ps_flame.transform.parent.gameObject.SetActive(true);
            ps_flame.transform.position = this.transform.position;
            ps_smoke.transform.position = this.transform.position;
            StartCoroutine(StartAnimation());
            //send message to uicontroller to show the info panel about experiment
        }

    }
    IEnumerator StartAnimation()
    {
        yield return new WaitForSeconds(2f);

        ps_smoke.Play();
        ps_flame.Play();
        StartCoroutine(OpenPanel());

    }
    IEnumerator OpenPanel()
    {
        yield return new WaitForSeconds(0.5f);
        Messenger<bool>.Broadcast(GameEvents.SHOW_REACTION_PANEL, true);

        yield return new WaitForSeconds(5f);

        //load button avanti
        UIController.instance.showButtonAvanti();
    }

    void OnDisable()
    {
        containsWater = false;
        containsSodium = false;
        clear();
        ps_flame.Stop();
        ps_smoke.Stop();
        ps_flame.transform.parent.gameObject.SetActive(false);

    }
}

