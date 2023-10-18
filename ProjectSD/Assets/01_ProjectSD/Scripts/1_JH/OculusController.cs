using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OculusController : MonoBehaviour
{
    int buttonLayerMask = (1 << 8);

    int uiLayerMask = (1 << 5);


    public Transform firePoint;
    public GameObject hitPoint;
    private LineRenderer laserRenderer;

    [Header("UI")]
    public bool isBtnEnable;
    public string btnName;

    public RaycastHit rayHit;
    public GameObject buttonObj;

    Vector3 originScale = Vector3.one * 0.02f;
    public Transform pointUI;

    // Start is called before the first frame update
    void Start()
    {
        laserRenderer = GetComponent<LineRenderer>();
        pointUI.gameObject.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        LaserPointer();
    }

    public void LaserPointer()
    {
        laserRenderer.SetPosition(0, firePoint.position);

        if (Physics.Raycast(firePoint.position, firePoint.forward, out rayHit, Mathf.Infinity, buttonLayerMask))
        {
            //hitPoint.gameObject.SetActive(true);
            //hitPoint.transform.position = rayHit.point;
            pointUI.gameObject.SetActive(true);
            pointUI.transform.position = rayHit.point;
            pointUI.localScale = originScale * Mathf.Max(1, rayHit.distance);

            laserRenderer.SetPosition(1, rayHit.point);


            btnName = rayHit.transform.name;
            isBtnEnable = true;
        }
        //else if (Physics.Raycast(firePoint.position, firePoint.forward, out rayHit, Mathf.Infinity, uiLayerMask))
        //{
        //    hitPoint.gameObject.SetActive(true);
        //    hitPoint.transform.position = rayHit.point;

        //    laserRenderer.SetPosition(1, rayHit.point);
        //    btnName = rayHit.transform.name;
        //    buttonObj = rayHit.transform.gameObject;
        //}
        else
        {
            laserRenderer.SetPosition(1, firePoint.forward * 2000);

            buttonObj = null;
            btnName = null;
            isBtnEnable = false;

            //hitPoint.gameObject.SetActive(false);
            pointUI.gameObject.SetActive(false);

        }
    }
}
