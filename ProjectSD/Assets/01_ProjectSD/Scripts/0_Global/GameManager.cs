using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

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
    public Slider hpSlider;
    public bool isGameOver;         // 게임오버 상태
    public bool isVR;
    public float playerHeight;
    public bool isGameStart;

    [Header("Golem")]
    public GameObject Golem;

    [Header("Panel")]
    public GameObject panels;
    public GameObject titlePanel;   // 타이틀 패널
    public GameObject shopPanel;         // 상점
    public GameObject gameOverPanel;// 게임오버 패널
    public GameObject gameClearPanel;

    public Slider bossDistance;
    public TMP_Text distanceText;

    [Header("Upgrade")]
    public bool isGunUpgrade;
    public bool isWeakpointUpgrade;

    [Header("Debug")]
    public bool isPCMODE;   // PC 모드로 테스트 할 경우 (VR 아닐 때)
    public bool isShopTest;

    [Header("Intro")]
    public IntroControl introControl;



    #region 골드관련 변수
    [Header("Gold")]
    private int playerGold;         // 플레이어가 소지한 골드
    private float getGoldTime;      // 이 변수가 1이되면 시간골드 상승
    public int secondsGold;         // 초당 얻게될 골드
    public float bossAttackGold;      // 공격을 맞출때에 얻게될 골드
    public int monsterKillGold;     // 졸개몬스터를 잡았을때 얻게될 골드

    public int PlayerGold       // UI 상에서 상시 Update 하지 않게하기위한 Gold 프로퍼티 변수
    {
        get { return playerGold; }

        set
        {
            if(playerGold != value)
            {
                playerGold = value;
                playerGoldUpdateEvent?.Invoke();     // 골드가 올라올떄마다 이벤트 구독해놓은 얘들을 Invoke()
                ColorUpdate();
            }
        }
    }

    #endregion 골드관련 변수

    #region 이벤트 관련
    public delegate void playerGoldUpdateDelegate();
    public event playerGoldUpdateDelegate playerGoldUpdateEvent;


    #endregion 이벤트 관련

    #region 상점 관련
    // 버튼들을 관리해줄 StaticList
    public static List<ShopItemButton> buttonsList;
    
    // 현재 적용중인 아이템 남은 시간 체크를 위한 bool변수
    public bool isWeaponDuration = false;
    public bool isWeakPointDuration = false;

    #endregion 상점 관련


    private void Awake()
    {
        ButtonsListMake(); // button들을 총 관리해줄 static List를 선언
        ReadGoldCSVFile(); // Gold관련 CSV를 읽어와서 변수에 해당하는 값을 넣어주는 함수




    }       // Awake()




    void Start()
    {
        AudioManager.instance.PlayMusic("Battle");

        // 플레이어를 세팅한다.
        SetPlayer(true);
        SetBossDistance(100);
        panels.transform.position = new Vector3(0, 29.5f + playerHeight, 0.57f);


        DebugPC();  // PC로 플레이 할 경우의 세팅
    }       // Start()

    
    void Update()
    {
        GetTimeGold();      // 일정시간이 된다면 골드를 올려주는 함수

        SetBossSlider();


        // 게임오버 테스트
        if (Input.GetKeyDown(KeyCode.F1))
        {
            GameOver();
        }
        // 게임오버 테스트
        if (Input.GetKeyDown(KeyCode.F2))
        {
            //GameStart();
            StartIntro();
        }
        // 바로 시작
        if (Input.GetKeyDown(KeyCode.F3))
        {
            titlePanel.SetActive(false);
            GameStart();
        }
        if (Input.GetKeyDown(KeyCode.F4) && !isPCMODE)
        {
            isPCMODE = true;
            SetPlayer(true);
        }

        if (Input.GetKeyDown(KeyCode.F9))
        {
            Time.timeScale += 1;
        }
        if (Input.GetKeyDown(KeyCode.F10))
        {
            Time.timeScale = 1;
        }
        if (Input.GetKeyDown(KeyCode.F11))
        {
            PC.gameObject.GetComponent<PlayerHealth>().health = 99999;
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

    public void StartIntro()
    {
        titlePanel.SetActive(false);
        // 인트로 
        introControl.PlayIntro();
    }

    public void GameStart()
    {
        Debug.Log("게임 시작");
        isGameStart = true;
        //titlePanel.SetActive(false);
        shopPanel.SetActive(true);

        Golem.GetComponent<Boss>().GameStart();
        

        SetPlayer(false);

        //// 돌펜스 보이스 효과음 재생
        //AudioManager.instance.PlaySFX("Voice");
    }
    public void GameOver()
    {
        if (!isGameOver)
        {
            Golem.GetComponent<Boss>().agent.isStopped = true;
            Golem.GetComponent<Boss>().GameOver();

            isGameOver = true;
            AudioManager.instance.PlayMusic("Lose");
            shopPanel.SetActive(false);
            gameOverPanel.SetActive(true);
            SetPlayer(true);
        }
    }public void GameClear()
    {
        if (!isGameOver)
        {
            isGameOver = true;
            AudioManager.instance.PlayMusic("Win");

            shopPanel.SetActive(false);
            gameClearPanel.SetActive(true);
            SetPlayer(true);
        }
    }
    
    // 다시시작
    public void Retry()
    {
        // 현재 씬의 이름을 가져와서 로드
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
        //Golem.transform.position = golemPosition;
        //PC.SetActive(false);
        //PC.SetActive(true);
        //Golem.SetActive(false);
        //Golem.SetActive(true);

        //isGameOver = false;

        //gameOverPanel.SetActive(false);
        //titlePanel.SetActive(true);
    }

    public void DebugPC()
    {
        if (isShopTest)
        {
            //GameStart();
            StartIntro();
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
        int weakPointCount = Golem.GetComponent<Boss>().weakPoint.Length;
        for(int i = 0; i < weakPointCount; i ++)
        {
            Golem.GetComponent<Boss>().weakPoint[i].GetComponent<BossHitPoint>().UpgraedWeakPoint();
        }
    }

    #endregion
    #region Player
    public void SetMaxHealth(float newHealth)
    {
        hpSlider.maxValue = newHealth;
        hpSlider.value = newHealth;
    }
    public void SetHealth(float newHealth)
    {
        hpSlider.value = newHealth;
    }

    public void SetBossDistance(float newDistance)
    {
        bossDistance.maxValue = newDistance;
        bossDistance.value = newDistance;
    }
    public void SetDistance(float newDistance)
    {
        bossDistance.value = newDistance;
    }

    public void SetBossSlider()
    {

        if (!isGameOver)
        {
            SetDistance(Golem.transform.position.z-10f);
            int distance = Mathf.FloorToInt(Golem.transform.position.z - 10f);
            distanceText.text = string.Format("{0}m", distance);

            if(distance <= 0)
            {
                GameOver();
            }
        }
    }
    #endregion

    // Gold관련 CSV를 읽어와서 변수에 해당하는 값을 넣어주는 함수
    private void ReadGoldCSVFile()
    {
        #region LEGACY
        //Dictionary<string, List<string>> goldCSV;
        //goldCSV = CSVReader.ReadCSVFile("CSVFiles/gold");

        //DataManager.SetData(goldCSV);
        #endregion LEGACY
        playerGold = (int)DataManager.GetData(9001, "Value");
        //Debug.LogFormat("PCGold -> {0}", playerGold);
        secondsGold = (int)DataManager.GetData(9002, "Value");
        //Debug.LogFormat("secondsGold -> {0}", secondsGold);
        bossAttackGold = (float)DataManager.GetData(9003, "Value");
        //Debug.LogFormat("bossAttackGold -> {0}", bossAttackGold);
        monsterKillGold = (int)DataManager.GetData(9004, "Value");


    }       // ReadGoldCSVFile()

    // 졸개를 잡았을때에 골드가 추가되는 함수
    public void MinionKillGetGold()
    {
        playerGold += monsterKillGold;

    }       // MinionKillGetGold()

    // 보스를 맞추었을때에 골드가 추가되는 함수
    public void HitBossGetGold()
    {
        playerGold += (int)bossAttackGold;    // 임시로 1로지정
    }



    // 일정 시간이 된다면 골드를 올려주는 함수
    private void GetTimeGold()
    {
        if (isGameStart)
        {
            getGoldTime += Time.deltaTime;
            if (getGoldTime >= 1)
            {
                PlayerGold += secondsGold;

                getGoldTime = 0;
            }
            else { /*PASS*/ }
        }       // GetTimeGold()
    }

    public void ButtonsListMake()       // 버튼 확대를 위해 버튼들을 static List선언
    {
        if (buttonsList == null || buttonsList == default)
        {
            buttonsList = new List<ShopItemButton>();
        }
        else { /*PASS*/ }
    }       //ButtonsListMake()

    private void ColorUpdate()
    {
        for(int i = 0; i < buttonsList.Count; i++)
        {
            buttonsList[i].ColorController();
        }
    }       // ColorUpdate()



}       // ClassEnd
