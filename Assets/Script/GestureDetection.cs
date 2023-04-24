using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestureDetection : MonoBehaviour
{
    // Start is called before the first frame update
    

    void Start() {
        StartCoroutine(CheckPinchGestureCoroutine());
    }

   IEnumerator CheckPinchGestureCoroutine() {
    // Ottieni le coordinate locali dei landmark della mano da MediaPipe
    //List<Vector3> handLandmarkCoordinates = GetHandLandmarkCoordinates();

    // Controlla se la gesture di pinch è stata eseguita
    //bool pinchGestureExecuted = CheckPinchGesture(handLandmarkCoordinates);

    // Fai qualcosa se la gesture di pinch è stata eseguita
    if (true) {
        Debug.Log("Pinch gesture executed!");
    }

    // Attendi un po' di tempo prima di eseguire di nuovo la coroutine
    yield return new WaitForSeconds(0.1f);

    // Esegui di nuovo la coroutine
    StartCoroutine(CheckPinchGestureCoroutine());
}


public bool CheckPinchGesture(List<Vector3> handLandmarkCoordinates) {
    // Distanza minima per considerare la gesture di pinch eseguita
    float pinchDistanceThreshold = 0.03f; // Modifica il valore a seconda della dimensione del tuo modello di mano

    // Calcola la distanza tra il pollice e l'indice
    float distanceBetweenThumbAndIndex = Vector3.Distance(handLandmarkCoordinates[4], handLandmarkCoordinates[8]);

    // Verifica se la distanza è inferiore alla soglia di pinch
    if (distanceBetweenThumbAndIndex < pinchDistanceThreshold) {
        return true;
    } else {
        return false;
    }
}
}
