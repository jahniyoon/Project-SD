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
        if (input.rightShoot)
        {
            rightGun.Shoot();
#if Oculus
            //ARAVRInput.PlayVibration(0.1f, 500f, 100f, ARAVRInput.Controller.RTouch);
            OVRInput.SetControllerVibration(1f, 1f, OVRInput.Controller.RTouch);

#endif
        }
        if (input.leftShoot)
        {
            leftGun.Shoot();
#if Oculus
            //ARAVRInput.PlayVibration(0.1f, 500f, 100f, ARAVRInput.Controller.LTouch);
            OVRInput.SetControllerVibration(1f, 1f, OVRInput.Controller.LTouch);

#endif
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
