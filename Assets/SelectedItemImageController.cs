using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
public class SelectedItemImageController : MonoBehaviour
{
    // Start is called before the first frame update
    public Image LeftHandImage;
    public Image RightHandImage;
    void Awake()
    {


        Messenger<Sprite>.AddListener(GameEvents.LEFT_ITEM_IMAGE_CHANGE, updateImageItemLeftHand);
        Messenger<Sprite>.AddListener(GameEvents.RIGHT_ITEM_IMAGE_CHANGE, updateImageItemRightHand);
         


    }
    void OnDestroy()
    {
        Messenger<Sprite>.RemoveListener(GameEvents.LEFT_ITEM_IMAGE_CHANGE, updateImageItemLeftHand);
        Messenger<Sprite>.RemoveListener(GameEvents.RIGHT_ITEM_IMAGE_CHANGE, updateImageItemRightHand);

    }
    private void updateImageItemLeftHand(Sprite spriteItem)
    {
        LeftHandImage.sprite = spriteItem;
    }
    private void updateImageItemRightHand(Sprite spriteItem)
    {
        RightHandImage.sprite = spriteItem;
    }


    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
