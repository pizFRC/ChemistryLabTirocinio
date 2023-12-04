
using UnityEngine;

public class SecurityController : MonoBehaviour
{


    public bool IsBecherInTheFumeHood;

    public Transform fumeHoodPosition;

    private static string  MISSING_GLOVES="Non hai utilizzato i guanti per condurre l'esperimento in sicurezza\n";
    private static string  MISSING_FUME_HOOD="L'esperimento va eseguito nella cappa d'aspirazione\n";
    private static string  MISSING_SAFETY_GOGGLES="Non hai utilizzato gli occhiali protettivi\n";
    public Vector3 dim;
    public float radius;
    public bool glovesOn;
    public GameObject glovesOBJ, safetyGogglesOBJ;


    public bool safetyGoggles;
    public GameObject glovesImage, safetyGogglesImage, fumeHoodImage;

    float timeElapsed;
    public bool isSafe = false;

    // Start is called before the first frame update
    void Start()
    {
        safetyGoggles = false;
        glovesOn = false;
    }

    void OnDisable()
    {
        safetyGoggles = false;
        glovesOn = false;
        isSafe = false;
        glovesOBJ.SetActive(!glovesOn);
        safetyGogglesOBJ.SetActive(!safetyGoggles);
        glovesImage.SetActive(!glovesOn);
        safetyGogglesImage.SetActive(!safetyGoggles);
        fumeHoodImage.SetActive(!isSafe);
    }


    // Update is called once per frame
    void Update()
    {
        if (this.gameObject.activeSelf)
        {

            glovesOBJ.SetActive(!glovesOn);
            safetyGogglesOBJ.SetActive(!safetyGoggles);
            glovesImage.SetActive(!glovesOn);
            safetyGogglesImage.SetActive(!safetyGoggles);


            if (fumeHoodPosition == null)
                return;

            isSafe = false;

            Collider[] colls = Physics.OverlapBox(fumeHoodPosition.position, dim);
            foreach (Collider c in colls)
            {


                if (c.TryGetComponent(out Contenitore contenitore))
                {
                    isSafe = true;
                }
            }


            fumeHoodImage.SetActive(!isSafe);
        }

    }

    void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        if (fumeHoodPosition != null)
            Gizmos.DrawCube(fumeHoodPosition.position, dim);

    }
    
    public void SetGlovesOn(bool value)
    {
        glovesOn = value;
    }
    public void SetSafetyGoggles(bool value)
    {
        safetyGoggles = value;
    }
}
