using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerHand : MonoBehaviour
{
    private PlayerInputs input;
    public Transform LeftPosition, RightPosition;
    [Header("Controller")]

    public OculusController LeftHand;
    public OculusController RightHand;

    public OVRControllerHelper leftHelper;
    public OVRControllerHelper rightHelper;

    public RaycastHit leftRay;
    public RaycastHit rightRay;




    // Start is called before the first frame update
    void Start()
    {
        input = GetComponent<PlayerInputs>();
        leftRay = LeftHand.rayHit;
        rightRay = RightHand.rayHit;
    }

    // Update is called once per frame
    void Update()
    {
        ButtonCheck();
    }


    

    public void ButtonCheck()
    {
        if (input.select)
        {
            if (RightHand.isBtnEnable && RightHand.btnName == "GameStart")
            {
                input.select = false;
                input.shop = false;
                //GameManager.instance.GameStart();
                GameManager.instance.StartIntro();
                RightHand.isBtnEnable = false;
            }
            else if (LeftHand.isBtnEnable && LeftHand.btnName == "GameStart")
            {
                input.select = false;
                input.shop = false;
                //GameManager.instance.GameStart();
                GameManager.instance.StartIntro();
                LeftHand.isBtnEnable = false;
            }

            if (RightHand.isBtnEnable && RightHand.btnName == "Retry")
            {
                input.select = false;
                input.shop = false;
                GameManager.instance.Retry();
                RightHand.isBtnEnable = false;
            }
            else if (LeftHand.isBtnEnable && LeftHand.btnName == "Retry")
            {
                input.select = false;
                input.shop = false;
                GameManager.instance.Retry();
                LeftHand.isBtnEnable = false;
            }
            ButtonClick();         

                input.select = false;
        }
    }
    public void ButtonClick()
    {
        if(LeftHand.buttonObj != null)
        {
            Debug.Log("버튼을 누른다");
            PointerEventData pointerEventData = new PointerEventData(EventSystem.current);

            IPointerClickHandler clickHandler = LeftHand.buttonObj.GetComponent<IPointerClickHandler>();

            clickHandler.OnPointerClick(pointerEventData);
        }
        if (RightHand.buttonObj != null)
        {
            Debug.Log("버튼을 누른다");
            PointerEventData pointerEventData = new PointerEventData(EventSystem.current);

            IPointerClickHandler clickHandler = RightHand.buttonObj.GetComponent<IPointerClickHandler>();

            clickHandler.OnPointerClick(pointerEventData);
        }
    }
}
