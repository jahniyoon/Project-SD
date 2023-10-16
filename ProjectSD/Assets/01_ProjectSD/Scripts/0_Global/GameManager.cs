using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    #region 골드관련 변수

    private int playerGold;      // 플레이어가 소지한 골드
    public int secondsGold;     // 초당 얻게될 골드
    public int attackGold;      // 공격을 맞출때에 얻게될 골드
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

    }       // Start()

    
    void Update()
    {
        GetTimeGold();      // 일정시간이 된다면 골드를 올려주는 함수

    }       // Update()


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
