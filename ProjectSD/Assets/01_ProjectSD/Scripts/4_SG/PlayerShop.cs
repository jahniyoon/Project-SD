using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerShop : MonoBehaviour
{
    private BuildRemove buildRemove;        // 아이템 제거해주는 GetComponent해올거임
    private PlayerInputs input;             // 플레이어의 입력값조건을 위한 클레스참조
    public enum State
    {
        Close,  // 상점 닫은 상태
        Open    // 상점 오픈 상태
    }
    public State state { get; private set; }

    public GameObject shopUI;
    public bool isBtnEnable;

    public string btnName;

    private ShopScroller shopScroller;  // 스크롤러 조절을 위한 Class를 담을변수

    private Vector3 defaultShopPos; // 상점 pos를 저장할 변수

    #region Test변수
    public PlayerShooter pShoter;
    public LayerMask buttonMask;
    private ShopItemButton tempClass;       // Ray를 쏘는것에서 HitInfo로 Class를 끌어올 변수


    private Transform cameraRig;            // OVRCameraRig Trasform을 담아둘 변수
    #endregion Test변수


    private void Awake()
    {
        SerchOVRCameraRig();
    }

    void Start()
    {

        buttonMask = LayerMask.GetMask("Button");

        input = GetComponent<PlayerInputs>();
        pShoter = GetComponent<PlayerShooter>();
        buildRemove = GetComponent<BuildRemove>();
        // 상점과 버튼은 비활성화
        state = State.Close;
        shopUI = GameManager.instance.shopPanel;
        isBtnEnable = false;

        // 상점 초기 pos 저장
        defaultShopPos = shopUI.transform.position;
    }

    void Update()
    {
        PlayerInput();  // 플레이어 입력

        if (btnName != null)
        {
            //Debug.Log(btnName);
        }
    }

    //플레이어 입력
    public void PlayerInput()
    {
        ShopInput();    // 상점 버튼 입력
        //SelectInput();  // 결정 버튼 입력
        TestRay();
    }

    // 상점 버튼 입력 시
    public void ShopInput()
    {
        if (input.shop)  // 상점 버튼을 눌렀을 때
        {
            if (state == State.Close)   // 닫혀있는 상태
            {
                //shopUI.gameObject.SetActive(true);      // 상점 UI 열기
                Vector3 tempShopPos = defaultShopPos;
                tempShopPos.y = 10000f; // 상점 위치를 위로 올려 숨김
                shopUI.transform.position = tempShopPos;
                state = State.Open;                     // 열림 상태로 전환
            }
            else if (state == State.Open)   // 열려있는 상태
            {
                //shopUI.gameObject.SetActive(false);     // 상점 UI 닫기
                shopUI.transform.position = defaultShopPos; // 상점 위치를 원래 위치로 변경
                state = State.Close;                    // 닫힘 상태로 전환
            }
            input.shop = false; // 입력을 해제한다.
        }
    }

    // 결정 버튼 입력 시 
    public void SelectInput()
    {
        if (input.select) // 결정 버튼을 눌렀을 때
        {
            //Debug.Log("결정 버튼 눌러서 들어옴");
            // 버튼 입력이 가능한 경우
            // 버튼 입력 가능 여부 : Ray에 충돌한 오브젝트의 Layer가 8번일 경우에, PlayerShooter 전달 받음.
            if (isBtnEnable)
            {
                PushButton(btnName);    // 버튼에 닿은 경우 해당 이름을 가져옴
            }
            input.select = false;       // 입력을 해제한다.
        }
    }

    public void TestRay()
    {
        if (shopUI.gameObject.activeSelf == true)
        {

            pShoter.rightGun.laserRenderer.SetPosition(0, pShoter.rightGun.firePoint.position);
            //Debug.LogFormat("LaserRenderer -> {0}", pShoter.rightGun.laserRenderer);

            RaycastHit rayHit;
            if (Physics.Raycast(pShoter.rightGun.firePoint.position, pShoter.rightGun.firePoint.forward, out rayHit, Mathf.Infinity, buttonMask))
            {
                if (rayHit.transform.GetComponent<ShopItemButton>())
                {
                    // if : 맞은 레이의 ShopItemButton을 얻었다면
                    tempClass = rayHit.transform.GetComponent<ShopItemButton>();
                    tempClass.IsRayHit = true;

                    if (input.select)
                    {
                        // LEGACY IF : if (tempClass.IsRayHit == true && tempClass.IsUseItem == false)
                        if (tempClass.IsRayHit == true)
                        {                            
                            if (tempClass.NowItemValue < tempClass.maxItemValue)
                            {
                                //Debug.Log("아이템 갯수조건이 충족한가?");
                                tempClass.BuyItem();
                            }                            
                        }
                        input.select = false;       // 입력을 해제한다.
                    }       // if end : 선택 버튼을 눌렀을 경우

                }       // if end : Ray맞은것이 ShoItemButton이라는 Script를 가지고 있다면

                //Scrollbar
                if (rayHit.transform.parent.parent.parent.GetComponent<ShopScroller>())
                {
                    // if : Ray를 맞은거의 부모의부모의부모가 ShopSroller를 가지고 있다면 들어옴


                    if (input.rightShoot)
                    {   
                        // if : 오른쪽 컨트롤러의 발사 버튼을 눌렀다면

                        // 컴포넌트를 가져옴
                        shopScroller = rayHit.transform.parent.parent.parent.GetComponent<ShopScroller>();

                        shopScroller.IsScroll = true;
                        shopScroller.TransformInIt(cameraRig);

                    }   // if : 오른쪽 컨트롤러의 발사 버튼을 눌렀다면

                    else if(!input.rightShoot)
                    {
                        if(shopScroller != null || shopScroller != default)
                        {
                            shopScroller.IsScroll = false;
                            shopScroller = null;
                        }   // if : ShopScroller가 비어있지않은 상태라면

                    }   // if : 오른쪽손 발사 버튼을 누른상태가 아니라면



                }       // if end : ShopScroller를 가져올수 있다면

            }
            else
            {
                for (int i = 0; i < GameManager.buttonsList.Count; i++)
                {
                    GameManager.buttonsList[i].IsRayHit = false;
                }

            }

            if (input.deleteUnit)
            {       // if : 상점이 열려있을때에 제거 버튼을 누른다면
                // TODO : 유닛 제거 Part 시작
                if(buildRemove.IsUnitRemove == true)
                {
                    // TODO : 제거 Ray그만쏘기
                    buildRemove.IsUnitRemove = false;

                }
                else if(buildRemove.IsUnitRemove == false)
                {                    
                    // TODO : 제거 Ray쏘기시작
                    buildRemove.IsUnitRemove = true;
                }
                input.deleteUnit = false;
                Debug.LogFormat("제거상태 -> {0}", buildRemove.IsUnitRemove);
            }

        }       // if : 상점이 열려 있을때에
    }       // TestRay()


    // 상점 UI의 버튼을 입력했을 때
    //  btnName : Ray에 충돌한 오브젝트의 Layer가 8번일 경우에 해당 gameObject.name을 PlayerShooter로부터 전달 받음
    public void PushButton(string btnName)
    {

        //Debug.LogFormat("함수로 받아온 매개변수 -> {0}", btnName);

        if (btnName == null)    // 이름이 없으면 리턴
        {
            return;
        }
        // 이름이 Exit일 경우
        if (btnName == "Exit")
        {
            // 상점 닫기 실행
            if (input.select && state == State.Open)
            {
                shopUI.gameObject.SetActive(false);
                state = State.Close;
            }
        }
    }       // PushButton


    // 상점 회전을할때에 Transform보내주기 위해 Awake단계이서 미리 찾아둠
    private void SerchOVRCameraRig()
    {
        cameraRig = this.transform.GetChild(0).GetChild(0);

    }       // SerchOVRCameraRig()



}       // ClassEnd
