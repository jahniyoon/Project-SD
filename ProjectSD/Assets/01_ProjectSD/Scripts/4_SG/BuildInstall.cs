using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    private Vector3 buildPoint;             // Instantitate할 위치값
    private GameObject buildClone;          // Instantiate할 GameObj
    [SerializeField]
    private GameObject[] trapPrefabs;       // 설치시 Instantiate할 원본 Prefab

    private int trapPrice;                  // 트랩의 가격
    private int fireBombPrice;              // 불폭탄의 가격

    public RectTransform shopPanelTrans;    // 상점의 페널을 담아둘 변수
    private Vector3 defualtV3;              // 원래의 상점 포지션
    private Vector3 disappearV3;            // 설치시 이동할 포지션

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
                    RectDisappear();
                }
                else if (isBuild == false)
                {
                    ParticleOff();
                    ReturnRect();
                }
            }
        }
    }       // 프로퍼티

    public int buildNum;         // 아이템 구매 눌렀을때에 그것이 Trab류 라면 어떤것인지 구별해줄 변수
    private enum buildItemNum
    {
        trap = 2,
        fireBomb
    }

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
        PriceInIt();        // 설치시 가격차감에 필요한 값들 가져오기
        Vector3InIt();
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
                // UI레이어는 예외처리
                if (hit.collider.gameObject.layer != 5 && hit.collider.gameObject.layer != 8 && !hit.collider.gameObject.CompareTag("Bullet") && !hit.collider.gameObject.CompareTag("Explosion") && !hit.collider.gameObject.CompareTag("Stun") && !hit.collider.gameObject.CompareTag("Fire"))
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
                            Debug.Log("설치한 포지션은" + buildV3);
                            buildV3.y = buildV3.y + 30;


                            if (Physics.Raycast(buildV3, Vector3.down, out buildHit, Mathf.Infinity))
                            {
                                if (buildNum == (int)buildItemNum.trap)
                                {       // 가격차감
                                    GameManager.instance.PlayerGold -= trapPrice;
                                }
                                else if (buildNum == (int)buildItemNum.fireBomb)
                                {       // 가격차감
                                    GameManager.instance.PlayerGold -= fireBombPrice;
                                }
                                buildPoint = buildHit.point;
                                //Debug.LogFormat("V3 수정전 값 -> {0}", buildPoint);
                                BuildVector3Check(ref buildPoint);  // 설치 위치 조건체크와 설정
                                                                    //Debug.LogFormat("V3 수정후 값 -> {0}", buildPoint);
                                buildClone = Instantiate(trapPrefabs[buildNum - 2], buildPoint, Quaternion.identity);
                                //Debug.LogError("!정상제작됨?!");
                                ParticleOff();
                            }
                            input.select = false;
                        }
                    }
                    else if (hit.collider.gameObject.CompareTag("Boss") || hit.collider.gameObject.CompareTag("Finish"))
                    {
                        //Debug.Log("보스인식");
                        mainModule.startColor = noBuildColor;
                        tempTrans = hit.transform.position;
                        particleTras.position = hit.point;

                    }
                    else if (hit.collider.gameObject.CompareTag("Enemy"))
                    {
                        //Debug.Log("졸개인식");
                        mainModule.startColor = noBuildColor;
                        tempTrans = hit.transform.position;
                        particleTras.position = hit.point;
                    }
                    else
                    {
                        // 플레이어 총알이나 보스 총알이면 예외처리
                        if (hit.collider.gameObject.GetComponent<ProjectileMover>() != null || hit.collider.gameObject.CompareTag("BossBullet"))
                        {
                            return;
                        }
                        //Debug.Log("else 들어옴");
                        // Debug.LogFormat("Trans = null?  -> {0}", tempTrans == null);
                        tempTrans = hit.transform.position;
                        particleTras.position = hit.point;
                    }
                }       // if : RayCast()
            }   // 예외처리
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

    private void PriceInIt()
    {
        trapPrice = (int)DataManager.GetData(8030, "Gold");
        fireBombPrice = (int)DataManager.GetData(8040, "Gold");
    }       // PriceInIt()

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

    // 설치할때에 BuildVector3가 설치 최소 위치와 최대 위치의 조건을 충족하는지 확인
    private Vector3 BuildVector3Check(ref Vector3 _BuildPoint)
    {
        // { Z축 최소치 최대치 비충족시 조건에 맞게 설정
        if(_BuildPoint.z < 9.9f)
        {
            _BuildPoint.z = 10f;
        }
        else if(_BuildPoint.z > 70f)
        {
            _BuildPoint.z = 70f;
        }
        // } Z축 최소치 최대치 비충족시 조건에 맞게 설정

        // { X축 최소치 최대치 비충족시 조건에 맞게 설정
        if(_BuildPoint.x < -65f)
        {
            _BuildPoint.x = -65f;
        }
        else if(_BuildPoint.x > 75f)
        {
            _BuildPoint.x = 75f;
        }
        // } X축 최소치 최대치 비충족시 조건에 맞게 설정
        Debug.LogFormat("JH검수 전 Pos -> {0}", _BuildPoint);
        #region LEGACY
        // { Y축 최소치 최대치 비충족시 조건에 맞게 설정
        //if (_BuildPoint.y > 1.2f || _BuildPoint.y < 1.2f)
        //{
        //    _BuildPoint.y = 1.2f;
        //}
        //else { /*PASS*/ }
        // { Y축 최소치 최대치 비충족시 조건에 맞게 설정
        #endregion LEGACY
        FixPosition(ref _BuildPoint); // 바뀐 포지션에서 레이를 다시 쏴준다.     
        Debug.LogFormat("JH검수 후 Pos -> {0}", _BuildPoint);
        return _BuildPoint;

    }       // BuildVector3Check(Vector3)

    private void Vector3InIt()
    {
        defualtV3 = shopPanelTrans.anchoredPosition3D;
        disappearV3 = new Vector3(-5555f, -5555f, -5555f);
    }

    private void RectDisappear()
    {
        shopPanelTrans.anchoredPosition3D = disappearV3;
    }       // RectDisappear()

    private void ReturnRect()
    {
        shopPanelTrans.anchoredPosition3D = defualtV3;
    }

    // 지환 : 다시 한 번 위에서 레이를 쏴줘서 빌드 포지션을 수정시켜주는 메서드
    private Vector3 FixPosition(ref Vector3 newPosition)
    {
           
        newPosition.y += 30f;   // 새로운 포지션의 30 위에서
        // 레이를 아래로 쏜다. 터레인일 경우,
        if (Physics.Raycast(newPosition, Vector3.down, out buildHit, 100, 1 << LayerMask.NameToLayer("Terrain")))
        {
            newPosition = buildHit.point;
        }
        return newPosition;
    }



}       // ClassEnd

