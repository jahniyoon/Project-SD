using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHand : MonoBehaviour
{
    private PlayerInputs input;
    public Transform LeftPosition, RightPosition;
    [Header("Controller")]

    public OculusController LeftHand;
    public OculusController RightHand;

   
    // Start is called before the first frame update
    void Start()
    {
        input = GetComponent<PlayerInputs>();

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
                GameManager.instance.GameStart();
                RightHand.isBtnEnable = false;
            }
            else if (LeftHand.isBtnEnable && LeftHand.btnName == "GameStart")
            {
                input.select = false;
                GameManager.instance.GameStart();
                LeftHand.isBtnEnable = false;
            }
            input.select = false;
        }
    }
}
