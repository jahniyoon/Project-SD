using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemButtonInstance : MonoBehaviour
{

    //! 상점에서 살수있는 아이템의 열값을 구해서 열의 갯수에 따라서 상점의 버튼을 생성시키는 클레스
    //! Grid가 가지고 있어야함

    private int childCount;             // 인스턴스할 자식오브젝트의 수를 정해주는 변수
    
    public ShopItemButton[] buttons;    // 그리드의 자식으로 Instace한 버튼들을 담아둘 배열

    [SerializeField]
    private GameObject buttonPrefab;        // 인스턴트할 버튼들의 프리펨

    private GameObject prefabClone;         // 클론을 이용해서 인스턴트할 예정
    private RectTransform rect;
    public RectTransform shopRect;

    private Vector3 buttonSclae;            // 버튼의 크기를 정해줄 변수

    private void Awake()
    {
        AwakeInIt();                // Awake 단계에서 할 작업
        ButtonInstantitate();       // 버튼을 인스턴트해주는 함수

    }

    private void Start()
    {
        // TODO : 자식오브젝트로 묶어놓은 버튼들을 배열속에 집어넣기
        buttons = GetComponentsInChildren<ShopItemButton>();   
        InItButtonNum();            // 버튼의 고유번호를 넣어주는 함수

    }



    private void AwakeInIt()
    {
        #region CSVFile읽어오는것
        //Dictionary<string, List<string>> buttonCSV;
        // TODO : 여기서 CSV를 읽어서 열의 갯수를 채크   
        //buttonCSV = CSVReader.ReadCSVFile("CSVFiles/Shop/Shop");
        //Debug.LogFormat("buttonCSV -> {0}", buttonCSV);
        //DataManager.SetData(buttonCSV);
        //Debug.Log("SetData 이후로 내려오나?");
        #endregion CSVFile읽어오는것
        childCount = DataManager.GetCount(8010);     // ID 수치 변경시 매개변수값 변경       
        buttonSclae = new Vector3(1f, 2f, 1f);

    }       // AwakeInIt()

    private void ButtonInstantitate()       // 버튼을 인스턴트해주는 함수
    {        
        for (int i = 0; i < childCount; i++)
        {
            prefabClone = Instantiate(buttonPrefab);
            prefabClone.transform.SetParent(this.transform);
            prefabClone.transform.localScale = buttonSclae;
            rect = prefabClone.GetComponent<RectTransform>();

            Vector3 buttonPos = shopRect.anchoredPosition3D;    // 지환 : 기존 상점 포지션을 가져와서
            buttonPos.z = 0f;                                   // z는 0으로 바꿔주고
                                                                // 버튼포지션을 지정
            rect.anchoredPosition3D = buttonPos;

            //rect.anchoredPosition3D = shopRect.anchoredPosition3D;     // AnchoredPosition3D 로 인스턴트 후 포지션 이상한것을 해결
            rect.rotation = shopRect.rotation;     // AnchoredPosition3D 로 인스턴트 후 포지션 이상한것을 해결
        }
    }       // ButtonInstantitate()

    private void InItButtonNum()        // 버튼들의 고유번호를 넣어주는 함수
    {
        //Debug.Log(buttons.Length);
        for(int i = 0; i < buttons.Length; i++)
        {
            buttons[i].buttonNum = i;
            //Debug.Log(buttons[i].buttonNum);
        }
    }       // InItButtonNum()




}       // ClassEnd
