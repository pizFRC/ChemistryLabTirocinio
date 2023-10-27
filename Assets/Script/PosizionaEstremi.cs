using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PosizionaEstremi : MonoBehaviour
{
    // Start is called before the first frame update
     public Transform oggettoSinistra; // Trascina l'oggetto da posizionare a sinistra nell'Inspector
    public Transform oggettoDestra; // Trascina l'oggetto da posizionare a destra nell'Inspector

    private void Start()
    {
        // Ottieni la larghezza della finestra di gioco in pixel
        updatePosition();
    }
    // Update is called once per frame

    void updatePosition(){
            int larghezzaFinestra = Screen.width;

        // Calcola le posizioni in coordinate del mondo
        float posXSinistra = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 10)).x; // 10 è la distanza dalla camera
        float posXDestra = Camera.main.ScreenToWorldPoint(new Vector3(larghezzaFinestra, 0, 10)).x;

        // Assegna le posizioni agli oggetti
        oggettoSinistra.position = new Vector3(posXSinistra, oggettoSinistra.position.y, 10);
        oggettoDestra.position = new Vector3(posXDestra, oggettoDestra.position.y,  Camera.main.transform.position.z+10);
    }
    void Update()
    {
        updatePosition();
    }
}
