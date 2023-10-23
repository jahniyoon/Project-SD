using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Windows;

public class PlayerShooter : MonoBehaviour
{
    private PlayerInputs input;
    private PlayerShop shop;


    [Header("Weapon")]
    public Weapon leftGun;
    public Weapon rightGun;

    // Start is called before the first frame update
    void Start()
    {
        input = GetComponent<PlayerInputs>();
        shop = GetComponent<PlayerShop>();
    }

    // Update is called once per frame
    void Update()
    {
        Fire();
        ButtonCheck();
    }

    // 사격 관련
    public void Fire()
    {
        if(input.rightShoot)
        {
            rightGun.Shoot();
            //ARAVRInput.PlayVibration(0.06f, 1f, 1f, ARAVRInput.Controller.RTouch);
            //VibrationManager.instance.TriggerVibration(40, 2, 255, OVRInput.Controller.RTouch);
        }
        if (input.leftShoot)
        {
            leftGun.Shoot();
            //ARAVRInput.PlayVibration(0.06f, 1f, 1f, ARAVRInput.Controller.LTouch);
            //VibrationManager.instance.TriggerVibration(40, 2, 255, OVRInput.Controller.LTouch);

        }
    }



public void ButtonCheck()
    {
        if (rightGun.isBtnEnable)
        {
            shop.isBtnEnable = rightGun.isBtnEnable;
            shop.btnName = rightGun.btnName;
            leftGun.isBtnEnable = false;
        }
        else if (leftGun.isBtnEnable)
        {
            shop.isBtnEnable = leftGun.isBtnEnable;
            shop.btnName = leftGun.btnName;
            rightGun.isBtnEnable = false;
        }
    }

   
}
