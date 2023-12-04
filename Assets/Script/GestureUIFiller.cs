using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using System.Linq;
using System;
using UnityEngine.UI;

enum gesturesIndex
{
    victory = 0,
    closeHand = 1,
    love = 2,

    thumbUp = 3,
    thumpDown = 4,

}
public class GestureUIFiller : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject prototipo;
    [SerializeField]
    public string left_or_right;

    public ScriptableObject so;
    static string lastF;
    public Sprite[] gesture;
    
    // Update is called once per frame
    void Awake()
    {
        if (left_or_right == "Right")
            Messenger<ScriptableObject>.AddListener(GameEvents.SET_SCRIPATABLE_DX, setScriptableObject);



        if (left_or_right == "Left")
            Messenger<ScriptableObject>.AddListener(GameEvents.SET_SCRIPATABLE_SX, setScriptableObject);
    }
   void OnDestroy()
    {
         if (left_or_right == "Left")
         Messenger<ScriptableObject>.RemoveListener(GameEvents.SET_SCRIPATABLE_SX, setScriptableObject);
          if (left_or_right == "Right")
          Messenger<ScriptableObject>.RemoveListener(GameEvents.SET_SCRIPATABLE_DX, setScriptableObject);
    }
    void Start()
    {

        //HandController.instance.


      setScriptableObject(so);





    }
    public void setScriptableObject(ScriptableObject so)
    {
        int childs = transform.childCount;
        for (int i = childs - 1; i > 0; i--)
        {
            GameObject.Destroy(transform.GetChild(i).gameObject);
        }
        this.so = so;
        int j=0;
        foreach (string s in GetFunctions(so))
        {
            prototipo.GetComponentInChildren<Text>().text = s;
         
            prototipo.GetComponentInChildren<Image>().sprite = gesture[j];

            GameObject p = Instantiate(prototipo, transform);

            j++;



        }

    }
    private void OnEnable()
    {

    }

    public static List<string> GetFunctions(ScriptableObject so)
    {
        print("funzioni");
        Type soType = so.GetType();
        MethodInfo[] methods = soType.GetMethods(BindingFlags.Public | BindingFlags.Instance)
            .Where(m => m.Name.StartsWith("function_"))
            .ToArray();
        List<string> nomi = new List<String>();
        foreach (MethodInfo m in methods)
        {
            Debug.LogError(m.Name);
            lastF = m.Name;
            nomi.Add(m.Name);
        }
        return nomi;
    }
}
