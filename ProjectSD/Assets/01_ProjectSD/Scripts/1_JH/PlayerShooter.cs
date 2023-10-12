using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooter : MonoBehaviour
{
    private PlayerInputs input;
    private PlayerShop shop;

    [Header("Laser")]
    int buttonLayerMask = (1 << 8);
    public LineRenderer laserPoint;
    public Transform laserStartPoint;
    public Transform laserEndPoint;
    public GameObject laserHit;
    public string btnName;

    // Start is called before the first frame update
    void Start()
    {
        input = GetComponent<PlayerInputs>();
        shop = GetComponent<PlayerShop>();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        LaserPointer();
        Shoot();
    }

    public void LaserPointer()
    {
        laserPoint.SetPosition(0, laserStartPoint.position);

        RaycastHit rayHit;
        if (Physics.Raycast(laserStartPoint.position, laserStartPoint.forward, out rayHit, Mathf.Infinity, buttonLayerMask))
        {
            laserHit.gameObject.SetActive(true);   
            laserHit.transform.position = rayHit.point;

            laserPoint.SetPosition(1, rayHit.point);
            
            shop.btnName = rayHit.transform.name;
            shop.isBtnEnable = true;
        }
        else
        {
            laserPoint.SetPosition(1, laserEndPoint.position);

            shop.btnName = null;
            shop.isBtnEnable = false;

            laserHit.gameObject.SetActive(false);
        }

    }
    public void Shoot()
    {
        if(input.shoot)
        {
            Debug.Log("발사한다.");

            input.shoot = false;
        }
    }
}
