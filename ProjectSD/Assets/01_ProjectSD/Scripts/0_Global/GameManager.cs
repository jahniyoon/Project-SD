using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance
    {
        get
        {
            // 만약 싱글톤 변수에 아직 오브젝트가 할당되지 않았다면
            if (m_instance == null)
            {
                // 씬에서 GameManager 오브젝트를 찾아 할당
                m_instance = FindObjectOfType<GameManager>();
            }

            // 싱글톤 오브젝트를 반환
            return m_instance;
        }
    }
    private static GameManager m_instance; // 싱글톤이 할당될 static 변수

    [Header("Player")]
    public GameObject PC;           // 플레이어
    public bool isGameOver;         // 게임오버 상태
    public bool isVR;

    [Header("Golem")]
    public GameObject Golem;
    public Vector3 golemPosition;

    [Header("Panel")]
    public GameObject titlePanel;   // 타이틀 패널
    public GameObject shopPanel;         // 상점
    public GameObject gameOverPanel;// 게임오버 패널

    [Header("Upgrade")]
    public bool isGunUpgrade;
    public bool isWeakpointUpgrade;

    [Header("Debug")]
    public bool isPCMODE;   // PC 모드로 테스트 할 경우 (VR 아닐 때)
    public bool isShopTest;




    #region 골드관련 변수
    [Header("Gold")]
    public int secondsGold;     // 초당 얻게될 골드
    public int attackGold;      // 공격을 맞출때에 얻게될 골드
    private int playerGold;      // 플레이어가 소지한 골드
    private float goldGetSeconds;

    public int PlayerGold       // UI 상에서 상시 Update 하지 않게하기위한 Gold 프로퍼티 변수
    {
        get { return playerGold; }

        set
        {
            if(playerGold != value)
            {
                playerGold = value;
                playerGoldUpdateEvent?.Invoke();     // 골드가 올라올떄마다 이벤트 구독해놓은 얘들을 Invoke()
            }
        }
    }

    #endregion 골드관련 변수

    #region 이벤트 관련
    public delegate void playerGoldUpdateDelegate();
    public event playerGoldUpdateDelegate playerGoldUpdateEvent;


    #endregion 이벤트 관련

    #region 상점 관련
    public static List<ShopItemButton> buttonsList;
    #endregion 상점 관련


    private void Awake()
    {
        ButtonsListMake(); // button들을 총 관리해줄 static List를 선언
        ReadGoldCSVFile(); // Gold관련 CSV를 읽어와서 변수에 해당하는 값을 넣어주는 함수




    }       // Awake()




    void Start()
    {
        // 플레이어를 세팅한다.
        SetPlayer(true);

        // 골렘의 초기 위치 저장한다.
        golemPosition = Golem.transform.position;

        DebugPC();  // PC로 플레이 할 경우의 세팅
    }       // Start()

    
    void Update()
    {
        GetTimeGold();      // 일정시간이 된다면 골드를 올려주는 함수

        // 게임오버 테스트
        if (Input.GetKeyDown(KeyCode.F1))
        {
            GameOver();
        }

    }       // Update()


    #region GameSystem
    public void SetPlayer(bool setHand)
    {
        // PC 모드면 카메라 조작 On
        PC.transform.GetChild(0).GetChild(0).GetComponent<CamRotate>().enabled = isPCMODE;
        
        // Shooter와 Shop 은 게임중에만 사용하기에 OFF
        PC.GetComponent<PlayerShooter>().enabled = !setHand;
        PC.GetComponent<PlayerShop>().enabled = !setHand;

        // ture면 컨트롤러를 켠다.
        PC.transform.GetComponent<PlayerHand>().enabled = setHand;

        // 플레이어의 손
        Transform leftHand = PC.transform.GetComponent<PlayerHand>().LeftHand.transform;
        Transform rightHand = PC.transform.GetComponent<PlayerHand>().RightHand.transform;
        // 플레이어의 총
        Transform leftGun = PC.transform.GetComponent<PlayerShooter>().leftGun.transform;
        Transform rightGun = PC.transform.GetComponent<PlayerShooter>().rightGun.transform;

        if(!isPCMODE)
        {
            // PC 모드라면, VR 컨트롤러 헬퍼 종료
            PC.transform.GetComponent<PlayerHand>().leftHelper.enabled = setHand;
            PC.transform.GetComponent<PlayerHand>().rightHelper.enabled = setHand;
        }

        // SetHand 가 true면 컨트롤러가 켜진다.
        leftHand.gameObject.SetActive(setHand);
        rightHand.gameObject.SetActive(setHand);
        // SetHand 가 false면 총이 켜진다.
        leftGun.gameObject.SetActive(!setHand);
        rightGun.gameObject.SetActive(!setHand);

        // PC모드일 경우
        if (isPCMODE)
        {
            // PC 모드라면, VR 컨트롤러 헬퍼 종료
            PC.transform.GetComponent<PlayerHand>().leftHelper.enabled = false;
            PC.transform.GetComponent<PlayerHand>().rightHelper.enabled = false;

            // 만약 PC 모드라면, 카메라 앞으로 손 위치 지정
            leftHand.transform.position = PC.transform.GetComponent<PlayerHand>().LeftPosition.position;
            leftHand.transform.rotation = PC.transform.GetComponent<PlayerHand>().LeftPosition.rotation;
            rightHand.transform.position = PC.transform.GetComponent<PlayerHand>().RightPosition.position;
            rightHand.transform.rotation = PC.transform.GetComponent<PlayerHand>().RightPosition.rotation;

            leftGun.transform.position = PC.transform.GetComponent<PlayerHand>().LeftPosition.position;
            leftGun.transform.rotation = PC.transform.GetComponent<PlayerHand>().LeftPosition.rotation;
            rightGun.transform.position = PC.transform.GetComponent<PlayerHand>().RightPosition.position;
            rightGun.transform.rotation = PC.transform.GetComponent<PlayerHand>().RightPosition.rotation;
        }
       
    }

    public void GameStart()
    {
        Debug.Log("게임 시작");
        titlePanel.SetActive(false);

        Golem.GetComponent<Boss>().GameStart();

        SetPlayer(false);
    }
    public void GameOver()
    {
        if (!isGameOver)
        {
            Golem.GetComponent<Boss>().agent.isStopped = true;

            isGameOver = true;

            shopPanel.SetActive(false);
            gameOverPanel.SetActive(true);
            SetPlayer(true);
        }
    }
    
    // 다시시작
    public void Retry()
    {
        Golem.transform.position = golemPosition;
        PC.SetActive(false);
        PC.SetActive(true);
        Golem.SetActive(false);
        Golem.SetActive(true);

        isGameOver = false;

        gameOverPanel.SetActive(false);
        titlePanel.SetActive(true);
    }

    public void DebugPC()
    {
        if (isShopTest)
        {
            GameStart();
        }
    }
    #endregion

    #region Upgrade
    public void UpgradeGun()
    {
        PC.GetComponent<PlayerShooter>().leftGun.GetComponent<Weapon>().WeaponUpgrade();
        PC.GetComponent<PlayerShooter>().rightGun.GetComponent<Weapon>().WeaponUpgrade();
    }
    public void UpgradeWeakPoint()
    {

    }

    #endregion

    // Gold관련 CSV를 읽어와서 변수에 해당하는 값을 넣어주는 함수
    private void ReadGoldCSVFile()
    {        
        Dictionary<string, List<string>> goldCSV;
        goldCSV = CSVReader.ReadCSVFile("CSVFiles/gold");

        DataManager.SetData(goldCSV);

        playerGold = (int)DataManager.GetData(6000, "CurrentGold");
        secondsGold = (int)DataManager.GetData(6001, "SecondsGold");
        attackGold = (int)DataManager.GetData(6002, "AttackGold");
        
    }       // ReadGoldCSVFile()

    // 일정 시간이 된다면 골드를 올려주는 함수
    private void GetTimeGold()
    {
        goldGetSeconds += Time.deltaTime;
        if (goldGetSeconds >= 1)
        {
            PlayerGold += secondsGold;            
            
            goldGetSeconds = 0;
        }
        else { /*PASS*/ }
    }       // GetTimeGold()

    public void ButtonsListMake()       // 버튼 확대를 위해 버튼들을 static List선언
    {
        if (buttonsList == null || buttonsList == default)
        {
            buttonsList = new List<ShopItemButton>();
        }
        else { /*PASS*/ }
    }       //ButtonsListMake()


}       // ClassEnd
