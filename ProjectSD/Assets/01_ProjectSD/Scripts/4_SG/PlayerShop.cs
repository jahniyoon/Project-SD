using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShop : MonoBehaviour
{
    private PlayerInputs input;
    public enum State
    {
        Close, // 상점 닫은 상태
        Open // 상점 오픈 상태
    }
    public State state { get; private set; }

    public GameObject shopUI;
    public bool isBtnEnable;

    public string btnName;


    #region Test변수
    public PlayerShooter pShoter;
    public LayerMask buttonMask;
    private ShopItemButton tempClass;       // Ray를 쏘는것에서 HitInfo로 Class를 끌어올 변수

    #endregion Test변수

    private void Awake()
    {
        pShoter = GetComponent<PlayerShooter>();
    }

    // Start is called before the first frame update
    void Start()
    {

        buttonMask = LayerMask.GetMask("Button");

        input = GetComponent<PlayerInputs>();
        pShoter = GetComponent<PlayerShooter>();
        // 상점과 버튼은 비활성화
        state = State.Close;
        shopUI = GameManager.instance.shopPanel;
        isBtnEnable = false;
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
                shopUI.gameObject.SetActive(true);      // 상점 UI 열기
                state = State.Open;                     // 열림 상태로 전환
            }
            else if (state == State.Open)   // 열려있는 상태
            {
                shopUI.gameObject.SetActive(false);     // 상점 UI 닫기
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
            RaycastHit rayHit;
            if (Physics.Raycast(pShoter.rightGun.firePoint.position, pShoter.rightGun.firePoint.forward, out rayHit, Mathf.Infinity, buttonMask))
            {
                //Debug.LogFormat("상점이 열리면 레이를 쏘나?");
                if (rayHit.transform.GetComponent<ShopItemButton>())
                {
                    //Debug.LogFormat("레이맞은것이 Class를 가지고있나?");
                    // if : 맞은 레이의 ShopItemButton을 얻었다면
                    tempClass = rayHit.transform.GetComponent<ShopItemButton>();
                    tempClass.IsRayHit = true;
                    //Debug.LogFormat("IsRayHit -> {0}, objName -> {1}", tempClass.IsRayHit,tempClass.gameObject.name);

                    if (input.select)
                    {
                        Debug.LogFormat("Select버튼 누름");
                        // LEGACY IF : if (tempClass.IsRayHit == true && tempClass.IsUseItem == false)
                        if (tempClass.IsRayHit == true)
                        {
                            //TEST
                            if (tempClass.NowItemValue < tempClass.maxItemValue)
                            {
                                //Debug.Log("아이템 갯수조건이 충족한가?");
                                tempClass.BuyItem();
                            }
                            //TEST
                        }
                        Debug.Log("선택버튼 false");
                        input.select = false;       // 입력을 해제한다.
                    }       // if end : 선택 버튼을 눌렀을 경우

                }       // if end : Ray맞은것이 ShoItemButton이라는 Script를 가지고 있다면


            }
            else
            {
                for (int i = 0; i < GameManager.buttonsList.Count; i++)
                {
                    GameManager.buttonsList[i].IsRayHit = false;
                }

            }

        }       // if : 상점이 열려 있을때에
    }       // TestRay()


    // 상점 UI의 버튼을 입력했을 때
    //  btnName : Ray에 충돌한 오브젝트의 Layer가 8번일 경우에 해당 gameObject.name을 PlayerShooter로부터 전달 받음
    public void PushButton(string btnName)
    {

        Debug.LogFormat("함수로 받아온 매개변수 -> {0}", btnName);

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
    }

}
