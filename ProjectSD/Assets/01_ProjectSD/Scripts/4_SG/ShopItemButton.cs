using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemButton : MonoBehaviour
{
    GameManager gameManager;

    [Header("Button_Num")]
    public int buttonNum;

    [Header("CSV_Value")]
    
    public int maxItemValue;        // 구매할수 있는 최대치
    public int price;               // 아이템의 가격
    public float coolTime;            // 구매의 쿨타임
    public string description;      // 아이템 설명

    [Header("CSV_ITEM_ID")]

    public int upgradeGunID;        // 가독성을위해 CSV아이템 ID 매핑
    public int upgradeWaekPointID;

    [Header("TEST_Parameter")]
    public PlayerShop pSC;

    private enum itemTag            // 가독성을 위해 만든 Enum 아이템 구매할때조건으로 사용
    {
        UpgradeGun,                 // 0
        UpgradeWeakPoint            // 1
    }

    [Header("NonTag")]
    private float nowCoolTime;       // 쿨타임이 현재 얼마나 흘렀는지 알려줄 변수


    private int nowItemValue;        // 현재 아이템의 갯수

    //아이템 갯수 증가,감소때마다 현재 아이템 갯수와 최대 아이템 갯수 텍스트 업데이트해주기위한 프로퍼티
    public int NowItemValue
    {
        get { return nowItemValue; }
        set
        {
            if (nowItemValue != value)
            {
                nowItemValue = value;
                UpdateItemCountText();
            }
        }
    }       


    private Coroutine buyCooltimeCoroutine;     // Start 코루틴 해줄 변수

    private bool isUseItem;          // 현재 해당 버튼의 아이템을 사용중인지 판단할 bool변수
    public bool IsUseItem
    {
        get { return isUseItem; }

        set
        {
            if (isUseItem != value)
            {
                isUseItem = value;
                if (isUseItem == true)
                {
                    // TODO : CoolTime Coroutine 실행
                    buyCooltimeCoroutine = StartCoroutine(BuyCoolTime());
                }
                else { /*PASS*/ }
            }
        }
    }       // IsUseItem 프로퍼티 End


    private bool isRayHit;           // 자신이 레이를 맞았는지 판단할 Bool 변수
    public bool IsRayHit
    {
        get { return isRayHit; }

        set
        {
            if (isRayHit != value)
            {
                //Debug.LogFormat("RayHit 값바뀜");
                isRayHit = value;
                buttonController();     // 버튼 색과 확대,축소등 관리해줄 함수
            }
        }
    }


    public bool isExpansionIng;     // 현재 스케일 값이 변경되고 있는지


    private Transform countTextObj; // 자식오브젝트로 있는 텍스트 오브젝트를 가져올 변수

    private TextMeshProUGUI itemCountText;      // 아이템 현재갯수,최대 갯수를 표기해줄 텍스트
    private TextMeshProUGUI priceText;          // 가격텍스트
    private TextMeshProUGUI cooltimeText;       // 아이템 쿨타임 텍스트
    private TextMeshProUGUI descriptionText;    // 아이템 설명 텍스트

    private Image itemSprite;       // 아이템 이미지가 들어갈 변수
    private Image thisBackGroundImage;  // 컴포넌트를 가지고 있는 Button 자신의 이미지 변수

    public Vector3 defaultScale;    // 원래의 스케일값
    public Vector3 expansionScale;  // Player가 레이를 맞추었을때에 커질만큼의 값

    private Color32 defaultColor;   // 기본색
    private Color32 choiceColor;    // 아이템을 Ray로 맞추었을때에 바뀔 색
    private Color32 buyColor;       // 아이템 쿨타임이 돌고있을때에 바뀔 색


    private void Awake()
    {
        ItemIDInIt();
        GameManagerInIt();          // 게임메니저 인스턴스하는 함수
        ImageComponentInIt();       // 이미지 컴포넌트 넣어주는 함수
        GetChildTextObj();          // Text들을 찾아서 바로바로 넣어주는 함수
        Vector3InIt();              // 아이템 UI 확대 축소를 위한 Vector3 변수에 값을 기입해주는 함수
        //ColorInIt();                // 그리드 상점일때에 컬러기입 처음 컬러값 new 할당해주는 함수
        ScrollerColorInIt();        // 스크롤러 상점일때에 컬러기입
    }

    void Start()
    {
        //Debug.LogFormat("ID In?  WINum -> {0} , WPINum - > {1}",upgradeGunID,upgradeWaekPointID);
        //Debug.LogFormat("ItemTag : WPTag -> {0}, WPITag -> {1}", itemTag.UpgradeGun, itemTag.UpgradeWeakPoint);
        CSVReadInIt();              // 필요한 변수를 ButtonCount에 따라서 기입해주는 함수
        UpdateItemCountText();      // 현재 아이템 갯수와 최대 아이템 갯수 텍스트를 업데이트 하는 함수
        FirstTextInIt();            // 처음 모든 Text에 CSV의 값을 넣어주는 함수
        ListInIt();                 // static List 에 자신스크립트를 넣어주는 함수
    }



    private void GameManagerInIt()      // 게임 메니저를 넣어줌 PlayerGold 처리를 위해
    {
        gameManager = FindAnyObjectByType<GameManager>();
    }   //  GameManagerInIt()

    private void ListInIt()     // GameManager에 선언되어있는 static List에 자신을 추가 해주는 함수
    {
        GameManager.buttonsList.Add(this);
    }

    private void ItemIDInIt()       // CSV속 아이템 ID를 매핑해주는 함수
    {
        upgradeGunID = 8001;
        upgradeWaekPointID = 8002;
    }       


    private void CSVReadInIt()      // CSV파일을 Read해와서 변수에 필요한 값을 넣어주는 함수
    {
        //Debug.LogFormat("ButtonNum -> {0}", buttonNum);
        if (buttonNum == (int)itemTag.UpgradeGun)      // 첫번째 아이템
        {
            //Debug.LogFormat("ButtonNum = {0} 가 첫번째 아이템으로 들어옴 ", buttonNum);
            nowItemValue = 0;
            maxItemValue = (int)DataManager.GetData(upgradeGunID,"Max");
            price = (int)DataManager.GetData(upgradeGunID,"Gold");
            coolTime = (int)DataManager.GetData(upgradeGunID,"Time");
            description = (string)DataManager.GetData(upgradeGunID,"Description");
        }

        else if (buttonNum == (int)itemTag.UpgradeWeakPoint) // 두번째 아이템
        {
            //Debug.LogFormat("ButtonNum = {0} 가 두번째 아이템으로 들어옴 ", buttonNum);
            nowItemValue = 0;
            maxItemValue = (int)DataManager.GetData(upgradeWaekPointID, "Max");
            price = (int)DataManager.GetData(upgradeWaekPointID, "Gold");
            coolTime = (int)DataManager.GetData(upgradeWaekPointID, "Time");
            description = (string)DataManager.GetData(upgradeWaekPointID, "Description");
        }

    }

    // ----------------------------------------- 이미지 관련 함수 -------------------------------------------
    private void ImageComponentInIt()       // 이미지 컴포넌트 넣어주는 함수
    {
        // 그리드일때에
        //thisBackGroundImage = this.transform.GetComponent<Image>();

        // 스크롤러일때에
        thisBackGroundImage = transform.GetChild(7).GetComponent<Image>();
    }       // ImageComponentInIt()

    private void ColorInIt()        // 컬러값 넣어주는 함수
    {
        defaultColor = new Color32(255, 255, 255, 255);
        choiceColor = new Color32(200, 200, 200, 255);
        buyColor = new Color32(80, 20, 30, 255);
    }       // ColorInIt()

    private void ScrollerColorInIt()
    {
        defaultColor = new Color32(255, 255, 255, 0);
        choiceColor = new Color32(200, 200, 200, 50);
        buyColor = new Color32(80, 20, 30, 50);
    }

    // ------------------------------------------ 텍스트 관련 함수 ------------------------------------------

    private void GetChildTextObj()  //텍스트 컴포넌트를 넣어주는 함수
    {
        // Item 현재갯수, 최대갯수 Text
        countTextObj = transform.GetChild(1).GetChild(0);
        itemCountText = countTextObj.GetComponent<TextMeshProUGUI>();

        // Item CoolTime Text
        countTextObj = transform.GetChild(3);
        cooltimeText = countTextObj.GetComponent<TextMeshProUGUI>();

        // Item Price Text
        countTextObj = transform.GetChild(5);
        priceText = countTextObj.GetComponent<TextMeshProUGUI>();

        // Item Description Text
        countTextObj = transform.GetChild(6);
        descriptionText = countTextObj.GetComponent<TextMeshProUGUI>();

    }       // GetChildTextObj()


    // 처음에 자신에게 맞는 텍스트를 업데이트 해주는 함수
    private void FirstTextInIt()
    {
        UpdateItemCountText();      //  nowCount,MaxCount Update
        UpdatePriceText();          // PriceText Update
        UpdateItemCoolTime();       // CoolTimeText Update
        UpdateDescriptionText();    // DescriptionText Update

    }       // FirstTextInIt()

    public void UpdateItemCountText()       // 현재 아이템 갯수와 최대 아이템 갯수 텍스트를 업데이트 하는 함수
    {
        // TODO : 게임메니저,플레이어 가 가지고 있는 bool 값을 이용하던지
        //          무언가를 이용해서 텍스트를 업데이트 해주어야함

        itemCountText.text = NowItemValue + " / " + maxItemValue;
    }

    public void UpdateItemCoolTime()        // 쿨타임 텍스트 업데이트
    {
        cooltimeText.text = coolTime.ToString();
    }

    public void UpdatePriceText()       // 가격 텍스트 업데이트
    {
        priceText.text = price.ToString();
    }
    public void UpdateDescriptionText()
    {
        //Debug.LogWarning("ㅎ한글은 나오나?");
        descriptionText.text = description; // 아이템 설명 업데이트
        
    }

    //-------------------------------------------- Ray가 닿았을때를 위한 함수들 --------------------------

    private void buttonController()     // 자신이 레이의 맞은 상태인지 아닌지에 따라서 버튼의 색,확대등을 컨트롤 해줄함수
    {
        ColorController();
    }

    // { 확대,축소
    // 23.10.16 스케일 조절 추후에 다시 할것 
    private void Vector3InIt()      // 백터 변수에 넣어줄 값
    {
        defaultScale = this.transform.localScale;

        expansionScale = new Vector3(1.2f, 2.4f, 1.2f);
    }       // Vector3InIt()


    public IEnumerator ButtonExpansion()        // TODO : 확대 Clamp,Lerp 를 사용할 코루틴
    {
        //Mathf.Clamp


        while (false)
        {

        }
        yield return null;
    }
    // } 확대,축소


    public void ColorController()       // isRayHit의 bool값을 따라서 색을 변경해줄 함수
    {
        if (isRayHit == true && IsUseItem == false)
        {
            ChangeChoiceColor();
        }
        else if (isRayHit == false && IsUseItem == false)
        {
            ChangeDefaultColor();
        }
        else if(IsUseItem == true)
        {
            ChangeBuyItemColor();
        }


    }      // ColorController()


    // { 색변경
    private void ChangeChoiceColor()     // Ray가 닿으면 색을 바꾸어줄 함수
    {
        thisBackGroundImage.color = choiceColor;
    }

    private void ChangeDefaultColor()    // Ray가 밖으로 나가면 원래색으로 바꾸어줄 함수
    {
        thisBackGroundImage.color = defaultColor;
    }
    private void ChangeBuyItemColor()
    {
        thisBackGroundImage.color = buyColor;
    }


    // } 색변경

    // ------------------------------------- 아이템 구매,구매후 효과 함수 -------------------------------------
    public void BuyItem()   // 아이템 구매
    {
        GoldCalculation();      // 플레이어 골드 차감
    }

    private void GoldCalculation()      // 플레이어 골드 차감
    {
        if (gameManager.PlayerGold >= price)
        {   //if : 플레이어소지골드가 가격과 같거나 골드가 더많을때에
            gameManager.PlayerGold -= price;
            IsUseItem = true;
            UseItemEffect();
        }
        else { Debug.Log("가격부족!"); }
    }       // GoldCalculation()

    private void UseItemEffect()        // 아이템 효과 적용
    {
        // TODO : 아이템 효과를 적용하는 것을 만들어야함
        if (buttonNum == (int)itemTag.UpgradeGun)
        {
            gameManager.UpgradeGun();
        }
        else if(buttonNum == (int)itemTag.UpgradeWeakPoint)
        {
            gameManager.UpgradeWeakPoint();
        }
    }       // UseItemEffect()


    private IEnumerator BuyCoolTime()   // 구매 쿨타임
    {
        NowItemValue++; // 현재 아이템 갯수 증가

        ColorController();      // 쿨타임 돌떄에 색을 바꾸어줌
        while (nowCoolTime < coolTime)
        {
            nowCoolTime += Time.deltaTime;
            yield return null;
        }

        nowCoolTime = 0f;
        IsUseItem = false;

        ColorController();      // 쿨타임이 끝날때에 색을 바꾸어줌

        NowItemValue--; // 현재 아이템 갯수 감소
    }       // BuyCoolTime()



}       // ClassEnd
