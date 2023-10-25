using Newtonsoft.Json.Bson;
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
    public float unitTime;          // 유닛의 지속시간

    private bool isUnitTimeGet = false;     // 유닛이라면 유닛의 지속시간을 얻어왔는지 체크할 bool 변수
    [Header("CSV_ITEM_ID")]

    public int upgradeGunID;        // 가독성을위해 CSV아이템 ID 매핑
    public int upgradeWaekPointID;
    public int trapID;
    public int trapUnitTimeID;
    public int fireBombID;
    public int fireUnitTimeID;

    //[Header("TEST_Parameter")]
    private BuildInstall buildInstall;      // 설치형 아이템 구매시 저 스크립트에서 Ray를 쏘며 설치할예정
    private ShopItemImageInIt shopSpriteClass;      // 상점 아이템 스프라이트와 구별할 Enum이 들어가있는 클래스
    private Vector3 defaultV3;      // 원본 백터값
    private Vector3 nowV3;          // 현재 백터값
    private Vector3 expansionV3;    // 확대될 백터값 


    private Coroutine sclaeCoroutine;   // 코루틴 캐싱

    private enum itemTag            // 가독성을 위해 만든 Enum 아이템 구매할때조건으로 사용
    {
        UpgradeGun,                 // 0
        UpgradeWeakPoint,           // 1
        Trap,                       // 2
        FireBomb                    // 3
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
                if (isRayHit)
                {
                    OVRInput.SetControllerVibration(1f, 1f, OVRInput.Controller.RTouch);
                }
            }
        }
    }
    public bool isExpansionIng;     // 현재 스케일 값이 변경되고 있는지

    private bool isCoolTime;        // 현재 아이템이 쿨타임 상태인지 체크하는변수

    private Transform countTextObj; // 자식오브젝트로 있는 텍스트 오브젝트를 가져올 변수

    private RectTransform rect;

    private TextMeshProUGUI itemCountText;      // 아이템 현재갯수,최대 갯수를 표기해줄 텍스트
    private TextMeshProUGUI priceText;          // 가격텍스트
    private TextMeshProUGUI cooltimeText;       // 아이템 쿨타임 텍스트
    private TextMeshProUGUI descriptionText;    // 아이템 설명 텍스트

    private Image itemSprite;       // 아이템 스프라이트가 들어갈 변수
    private Image thisBackGroundImage;  // 컴포넌트를 가지고 있는 Button 자신의 이미지 변수

    private Color32 defaultColor;   // 기본색
    private Color32 choiceColor;    // 아이템을 Ray로 맞추었을때에 바뀔 색
    private Color32 buyColor;       // 아이템 쿨타임이 돌고있을때에 바뀔 색
    private Color32 noMoneyColor;   // 아이템 구매금액보다 현재 금액이 낮을경우나올 색

    private Ray ray;                // 설치형 아이템 구매시 쏠 레이
    private RaycastHit hitInfo;     // 레이 맞은것을 판별할 hitInfo


    private void Awake()
    {
        GameManagerInIt();          // 게임메니저 인스턴스하는 함수
        ItemIDInIt();               // 매핑할 아이템 아이디 넣어주는 함수
        FirstInIt();                // 초기값이 정해져있는것들넣어주는 함수
        ImageComponentInIt();       // 이미지 컴포넌트 넣어주는 함수
        GetChildTextObj();          // Text들을 찾아서 바로바로 넣어주는 함수
        Vector3InIt();              // 아이템 UI 확대 축소를 위한 Vector3 변수에 값을 기입해주는 함수
        //ColorInIt();                // 그리드 상점일때에 컬러기입 처음 컬러값 new 할당해주는 함수
        ScrollerColorInIt();        // 스크롤러 상점일때에 컬러기입
    }       // Awake()

    void Start()
    {
        InItSprite();
        CSVReadInIt();              // 필요한 변수를 ButtonCount에 따라서 기입해주는 함수
        UpdateItemCountText();      // 현재 아이템 갯수와 최대 아이템 갯수 텍스트를 업데이트 하는 함수
        FirstTextInIt();            // 처음 모든 Text에 CSV의 값을 넣어주는 함수
        ListInIt();                 // static List 에 자신스크립트를 넣어주는 함수
        SetColorInitialization();    // 처음시작시 상점의 색이 Ray맞은 색으로 되기에 만든함수


    }       // Start()

    private void Update()
    {

    }       // Update()

    #region InIts
    private void GameManagerInIt()      // 게임 메니저를 넣어줌 PlayerGold 처리를 위해
    {
        gameManager = FindAnyObjectByType<GameManager>();
    }   //  GameManagerInIt()

    private void ListInIt()     // GameManager에 선언되어있는 static List에 자신을 추가 해주는 함수
    {
        GameManager.buttonsList.Add(this);
    }
    private void OnDestroy()
    {       // 자신이 삭제될때에 List에 자신을 제거
        GameManager.buttonsList.Remove(this);
    }

    private void ItemIDInIt()       // CSV속 아이템 ID를 매핑해주는 함수
    {
        upgradeGunID = 8010;
        upgradeWaekPointID = 8020;
        trapID = 8030;
        trapUnitTimeID = 7030;
        fireBombID = 8040;
        fireUnitTimeID = 7040;

    }

    private void FirstInIt()
    {
        isCoolTime = false;
        shopSpriteClass = this.transform.GetComponent<ShopItemImageInIt>();
    }       // 처음에 들어가야할 값을 넣어줌


    private void CSVReadInIt()      // CSV파일을 Read해와서 변수에 필요한 값을 넣어주는 함수
    {

        if (buttonNum == (int)itemTag.UpgradeGun)      // 무기강화
        {
            nowItemValue = 0;
            maxItemValue = (int)DataManager.GetData(upgradeGunID, "Max");
            price = (int)DataManager.GetData(upgradeGunID, "Gold");
            coolTime = (int)DataManager.GetData(upgradeGunID, "Time");
            description = (string)DataManager.GetData(upgradeGunID, "Description");
        }

        else if (buttonNum == (int)itemTag.UpgradeWeakPoint) // 약점강화
        {
            nowItemValue = 0;
            maxItemValue = (int)DataManager.GetData(upgradeWaekPointID, "Max");
            price = (int)DataManager.GetData(upgradeWaekPointID, "Gold");
            coolTime = (int)DataManager.GetData(upgradeWaekPointID, "Time");
            description = (string)DataManager.GetData(upgradeWaekPointID, "Description");
        }
        else if (buttonNum == (int)itemTag.Trap)            // 덫
        {
            nowItemValue = 0;
            maxItemValue = (int)DataManager.GetData(trapID, "Max");
            price = (int)DataManager.GetData(trapID, "Gold");
            coolTime = (int)DataManager.GetData(trapID, "Time");
            description = (string)DataManager.GetData(trapID, "Description");
            unitTime = (float)DataManager.GetData(trapUnitTimeID, "Value1");
            isUnitTimeGet = true;
        }

        else if (buttonNum == (int)itemTag.FireBomb)        // 불폭탄
        {
            nowItemValue = 0;
            maxItemValue = (int)DataManager.GetData(fireBombID, "Max");
            price = (int)DataManager.GetData(fireBombID, "Gold");
            coolTime = (int)DataManager.GetData(fireBombID, "Time");
            description = (string)DataManager.GetData(fireBombID, "Description");
            unitTime = (float)DataManager.GetData(7040, "ActTime"); // 화염병 지속시간
            isUnitTimeGet = true;
        }

    }       // CSVReadInIt()

    private void SerchBuildInstallClass()       // 플레이어 아이템 건설 해주는 컴포넌트 가져오는 함수
    {
        buildInstall = GameObject.Find("Player").GetComponent<BuildInstall>();
    }

    // ----------------------------------------- 이미지 관련 함수 -------------------------------------------
    private void ImageComponentInIt()       // 이미지 컴포넌트 넣어주는 함수
    {
        // 그리드일때에
        //thisBackGroundImage = this.transform.GetComponent<Image>();

        // 스크롤러일때에
        thisBackGroundImage = transform.GetChild(7).GetComponent<Image>();
        itemSprite = transform.GetChild(0).GetComponent<Image>();
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
        noMoneyColor = new Color32(255, 0, 0, 50);
    }

    private void InItSprite()       // 아이템의 스프라이트를 넣는 함수
    {
        // 버튼의 번호와 Enum의 번호를 동일시 해놓았기 떄문에 한번 번호 확인후 스프라이트 넣기
        if ((int)ShopItemImageInIt.imageNum.UpgradeGun == buttonNum)
        {
            itemSprite.sprite = shopSpriteClass.itemSprite[buttonNum];
        }
        else if ((int)ShopItemImageInIt.imageNum.UpgradeWeakPoint == buttonNum)
        {
            itemSprite.sprite = shopSpriteClass.itemSprite[buttonNum];
        }
        else if ((int)ShopItemImageInIt.imageNum.Trap == buttonNum)
        {
            itemSprite.sprite = shopSpriteClass.itemSprite[buttonNum];
        }
        else if ((int)ShopItemImageInIt.imageNum.FireBomb == buttonNum)
        {
            itemSprite.sprite = shopSpriteClass.itemSprite[buttonNum];
        }
        else { /*PASS*/ }

    }       // InItSprite()

    public void SetColorInitialization()
    {
        IsRayHit = false;
        ChangeDefaultColor();
    }       // SetColorInitialization()

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

    #endregion InIts

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
        if (isUnitTimeGet == false)
        {
            // 유닛의 지속시간을 얻어오지 않은 경우 유닛의 쿨타임 표기
            cooltimeText.text = coolTime.ToString();
        }
        else
        {
            // 유닛의 지속시간을 얻어온 경우 유닛의 지속시간 표기
            cooltimeText.text = unitTime.ToString();
        }
    }

    public void UpdatePriceText()       // 가격 텍스트 업데이트
    {
        priceText.text = price.ToString();
    }
    public void UpdateDescriptionText()
    {
        descriptionText.text = description; // 아이템 설명 업데이트
    }

    //-------------------------------------------- Ray가 닿았을때를 위한 함수들 --------------------------

    private void buttonController()     // 자신이 레이의 맞은 상태인지 아닌지에 따라서 버튼의 색,확대등을 컨트롤 해줄함수
    {
        ColorController();
        ScaleController();
    }


    // 확대 축소의 필요한 Vector3값과 확대시켜줄 Rect가져오기
    private void Vector3InIt()      // 백터 변수에 넣어줄 값
    {
        defaultV3 = this.transform.localScale;
        nowV3 = defaultV3;
        expansionV3 = defaultV3 * 1.2f;
        rect = this.GetComponent<RectTransform>();

    }       // Vector3InIt()

    // { 확대
    public IEnumerator ButtonExpansion()
    {
        float timeElapsed = 0f;     // 경과시간
        float duration = 0.3f;     //  원하는 시간 

        while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;
            float time = Mathf.Clamp01(timeElapsed / duration);
            nowV3 = Vector3.Lerp(nowV3, expansionV3, time);
            rect.localScale = nowV3;
            yield return null;
        }

    }
    // } 확대

    // { 축소
    public IEnumerator ButtonShrink()
    {
        float timeElapsed = 0f;     // 경과시간
        float duration = 0.3f;     //  원하는 시간 

        while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;
            float time = Mathf.Clamp01(timeElapsed / duration);
            nowV3 = Vector3.Lerp(nowV3, defaultV3, time);
            rect.localScale = nowV3;
            yield return null;
        }
    }
    // } 축소



    public void ColorController()       // isRayHit의 bool값을 따라서 색을 변경해줄 함수
    {
        if (NowItemValue == maxItemValue)
        {
            ChangeNoMoneyColor();
        }
        else if (GameManager.instance.PlayerGold < price)
        {
            ChangeNoMoneyColor();
        }
        else if (isRayHit == true && IsUseItem == false)
        {
            ChangeChoiceColor();
        }
        else if (isRayHit == false && IsUseItem == false)
        {
            ChangeDefaultColor();
        }
        else if (IsUseItem == true)
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
    private void ChangeNoMoneyColor()
    {
        thisBackGroundImage.color = noMoneyColor;
    }
    // } 색변경

    public void ScaleController()       // isRayHit의 bool 값에 따라서 버튼 크기를 변경해줄 함수
    {
        if (isRayHit == true)
        {   // 확대 시작
            sclaeCoroutine = StartCoroutine(ButtonExpansion());
        }
        else if (isRayHit == false)
        {
            sclaeCoroutine = StartCoroutine(ButtonShrink());
        }
    }       // ScaleController()



    // ------------------------------------- 아이템 구매,구매후 효과 함수 -------------------------------------
    public void BuyItem()   // 아이템 구매
    {
        GoldCalculation();      // 플레이어 골드 차감
    }

    private void GoldCalculation()      // 플레이어 골드 차감
    {
        if (isCoolTime == false)
        {   // if : 아이템이 쿨타임상태가 아닐떄에
            if (gameManager.PlayerGold >= price)
            {   // if : 플레이어소지골드가 가격과 같거나 골드가 더많을때에
                if (NowItemValue < maxItemValue)
                {   // if : 현재 아이템갯수가 아이템 최대치보다 작을때만 구매                               
                    //gameManager.PlayerGold -= price; //LEGACY 사용템은 효과 적용될때에 설치템은 설치후로 변경
                    IsUseItem = true;
                    UseItemEffect();
                }

            }
            else { Debug.Log("가격부족!"); }
        }

    }       // GoldCalculation()

    private void UseItemEffect()        // 아이템 효과 적용
    {

        if (buttonNum == (int)itemTag.UpgradeGun)
        {
            gameManager.PlayerGold -= price;
            GameManager.instance.isWeaponDuration = true;
            gameManager.UpgradeGun();
        }
        else if (buttonNum == (int)itemTag.UpgradeWeakPoint)
        {
            gameManager.PlayerGold -= price;
            GameManager.instance.isWeakPointDuration = true;
            gameManager.UpgradeWeakPoint();
        }
        else if (buttonNum == (int)itemTag.Trap)
        {
            SerchBuildInstallClass();       // 건설 컴포넌트 가져오기
            buildInstall.IsBuild = true;
            buildInstall.buildNum = (int)itemTag.Trap;
        }
        else if (buttonNum == (int)itemTag.FireBomb)
        {
            SerchBuildInstallClass();       // 건설 컴포넌트 가져오기
            buildInstall.IsBuild = true;
            buildInstall.buildNum = (int)itemTag.FireBomb;
        }
    }       // UseItemEffect()


    private IEnumerator BuyCoolTime()   // 구매 쿨타임
    {
        NowItemValue++; // 현재 아이템 갯수 증가
        isCoolTime = true;

        ColorController();      // 쿨타임 돌떄에 색을 바꾸어줌
        while (nowCoolTime < coolTime)
        {
            nowCoolTime += Time.deltaTime;
            yield return null;
        }
        isCoolTime = false;
        nowCoolTime = 0f;
        IsUseItem = false;
        //Debug.LogFormat("쿨타임이 잘끝나나? name -> {0}", this.transform.name);
        ColorController();      // 쿨타임이 끝날때에 색을 바꾸어줌

        //NowItemValue--; // 현재 아이템 갯수 감소      LEGACY : 아이템 효과 종료될때에 -로 수정
    }       // BuyCoolTime()



}       // ClassEnd
