using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.ParticleSystem;

public class BuildInstall : MonoBehaviour
{
    #region Components
    private PlayerShooter pShooter;
    private PlayerInputs input;

    private Transform particleTras;         // 움직일때엔 빈 오브젝트로 움직임
    private ParticleSystem buildParticle;    // 건설할때에 보여줄 파티클
    private MainModule mainModule;          // 파티클의 mainModule
    #endregion Components

    #region Colors
    private Color32 yesBuildColor32;          // 건설 가능 색상
    private Color32 noBuildColor32;           // 건설 불가능 색상
    private Color yesBuildColor;
    private Color noBuildColor;
    #endregion Colors

    private Vector3 buildV3;                // Build시 위에서 아래로 쏴줄 Vector3
    private Vector3 tempTrans;              // 만약 Ray가 닿지않을경우 파티클의 위치로 지정해줄 임시Transform
    private GameObject buildClone;          // Instantiate할 GameObj
    [SerializeField]
    private GameObject[] trapPrefabs;       // 설치시 Instantiate할 원본 Prefab


    private bool isBuild;         // 건설상태인지 확인하는 bool 변수
    public bool IsBuild
    {
        get { return isBuild; }
        set
        {
            if (isBuild != value)
            {
                isBuild = value;

                if (isBuild == true)
                {
                    ParticleOn();
                }
                else if (isBuild == false)
                {
                    ParticleOff();
                }
            }
        }
    }       // 프로퍼티

    public int buildNum;         // 아이템 구매 눌렀을때에 그것이 Trab류 라면 어떤것인지 구별해줄 변수

    #region Rays
    private Ray ray;                // 쏠Ray
    private RaycastHit hit;         // Ray맞은것을 검출할 hit
    private RaycastHit buildHit;    // 제작시 하늘에서 쏠레이를 검출할 Hit
    [SerializeField]
    private LayerMask layerMask;    // terrain

    private int terrainLayerMask = 1 << 7;      // terrain
    #endregion Rays

    private void Awake()
    {
        AwakeInIt();
    }

    void Start()
    {

    }


    void Update()
    {
        #region Test
        //if (IsBuild == true)
        //{
        //    pShooter.rightGun.laserRenderer.SetPosition(0, pShooter.rightGun.firePoint.position);

        //    if (Physics.Raycast(pShooter.rightGun.firePoint.position, pShooter.rightGun.firePoint.forward, out hit, Mathf.Infinity))
        //    {
        //        Debug.LogFormat("Point -> {0}   PCPos -> {1} ", hit.point, buildParticle.transform.position);
        //        if (hit.collider.gameObject.layer == layerMask)
        //        {       // if : Terrain이 맞았을 경우
        //            mainModule.startColor = yesBuildColor;
        //            tempTrans = hit.transform.position;
        //            buildParticle.gameObject.transform.position = hit.point;
        //            if(input.select)
        //            {       // 선텍버튼 누를시
        //                BuildItem();
        //                buildClone = Instantiate(trapPrefabs[buildNum - 2], hit.transform.position, this.transform.rotation);
        //                ParticleOff();
        //                IsBuild = false;
        //            }
        //        }
        //        else if (hit.collider.gameObject.name == ("GOLEM"))
        //        {
        //            mainModule.startColor = noBuildColor;
        //            tempTrans = hit.transform.position;
        //            buildParticle.gameObject.transform.position = hit.point;

        //        }
        //        else if (hit.collider.gameObject.CompareTag("Enemy"))
        //        {
        //            mainModule.startColor = noBuildColor;
        //            tempTrans = hit.transform.position;
        //            buildParticle.gameObject.transform.position = hit.point;
        //        }
        //        else 
        //        {
        //            Debug.Log("else 들어옴");
        //            Debug.LogFormat("Trans = null?  -> {0}", tempTrans == null);
        //            tempTrans = hit.transform.position;
        //            buildParticle.transform.position = tempTrans;
        //        }
        //    }       // if : RayCast()
        //}       // RayEnd
        #endregion Test

        if (IsBuild == true)
        {
            pShooter.rightGun.laserRenderer.SetPosition(0, pShooter.rightGun.firePoint.position);

            if (Physics.Raycast(pShooter.rightGun.firePoint.position, pShooter.rightGun.firePoint.forward, out hit, Mathf.Infinity))
            {
                //Debug.LogFormat("Point -> {0}   PCPos -> {1} ", hit.point, buildParticle.transform.position);
                //Debug.LogFormat("HitName -> {0}", hit.collider.name);
                //Debug.LogFormat("HitTag -> {0}", hit.collider.tag);
                //Debug.LogFormat("layer -> {0}", hit.collider.gameObject.layer);
                //if (hit.collider.gameObject.layer == terrainLayerMask)
                if (hit.collider.gameObject.layer == 7)
                {       // if : Terrain이 맞았을 경우
                    //Debug.Log("땅바닥인식");
                    mainModule.startColor = yesBuildColor;
                    tempTrans = hit.transform.position;
                    particleTras.position = hit.point;

                    if (input.select)
                    {       // 선텍버튼 누를시
                        //Debug.LogError("!선택버튼누름!");
                        IsBuild = false;
                        BuildItem();
                        buildV3 = hit.point;
                        buildV3.y = buildV3.y + 30;
                        if (Physics.Raycast(buildV3, Vector3.down, out buildHit, Mathf.Infinity))
                        {
                            buildClone = Instantiate(trapPrefabs[buildNum - 2], buildHit.point, Quaternion.identity);
                            //Debug.LogError("!정상제작됨?!");
                            ParticleOff();
                        }
                    }
                }
                else if (hit.collider.gameObject.CompareTag("Boss") || hit.collider.gameObject.CompareTag("Finish"))
                {
                    Debug.Log("보스인식");
                    mainModule.startColor = noBuildColor;
                    tempTrans = hit.transform.position;
                    particleTras.position = hit.point;

                }
                else if (hit.collider.gameObject.CompareTag("Enemy"))
                {
                    Debug.Log("졸개인식");
                    mainModule.startColor = noBuildColor;
                    tempTrans = hit.transform.position;
                    particleTras.position = hit.point;
                }
                else
                {
                    //Debug.Log("else 들어옴");
                    // Debug.LogFormat("Trans = null?  -> {0}", tempTrans == null);
                    tempTrans = hit.transform.position;
                    particleTras.position = hit.point;
                }
            }       // if : RayCast()
        }       // RayEnd


    }       // Update()


    private void AwakeInIt()
    {
        IsBuild = false;

        pShooter = GetComponent<PlayerShooter>();
        input = GetComponent<PlayerInputs>();
        particleTras = this.transform.GetChild(2).GetComponent<Transform>();
        buildParticle = this.transform.GetChild(2).GetChild(0).GetComponent<ParticleSystem>();
        mainModule = buildParticle.main;

        noBuildColor32 = new Color32(255, 0, 0, 255);    // 빨강색
        yesBuildColor32 = new Color32(0, 0, 255, 255);     // 파란색

        yesBuildColor = new Color
        (
                yesBuildColor32.r / 255.0f,
                yesBuildColor32.g / 255.0f,
                yesBuildColor32.b / 255.0f,
                yesBuildColor32.a / 255.0f
                                  );
        noBuildColor = new Color
        (
        noBuildColor32.r / 255.0f,
        noBuildColor32.g / 255.0f,
        noBuildColor32.b / 255.0f,
        noBuildColor32.a / 255.0f
                                  );

    }       // AwakeInIt()



    private void ParticleOn()       // 파티클 효과 키기
    {
        buildParticle.Play();
    }       // ParticleOn()

    private void ParticleOff()      // 파티클 효과 끄기
    {
        buildParticle.Stop();
    }      // ParticleOff()            

    private void BuildItem()
    {
        // TODO : 가능하다면 파티클이 되돌아간뒤에 제작되도록
    }       // BuildItem()


}       // ClassEnd
