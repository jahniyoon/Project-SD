using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemButtonInstance : MonoBehaviour
{


    //! 상점에서 살수있는 아이템의 열값을 구해서 열의 갯수에 따라서 상점의 버튼을 생성시키는 클레스
    //! Grid가 가지고 있어야함

    private int childCount;

    private bool firstOpen = false;

    public ShopItemButton[] buttons;    // 그리드의 자식으로 Instace한 버튼들을 담아둘 배열

    [SerializeField]
    private GameObject buttonPrefab;        // 인스턴트할 버튼들의 프리펨

    private GameObject prefabClone;         // 클론을 이용해서 인스턴트할 예정
    private RectTransform rect;

    private void Awake()
    {
        AwakeInIt();        // Awake 단계에서 할 작업

        // TODO : 여기서 만들어둔 버튼을 Instace 하면서 자식오브젝트로 묶기
        //ButtonInstantitate();
        //StartCoroutine(Test());
    }

    private void Start()
    {
        // TODO : 자식오브젝트로 묶어놓은 버튼들을 배열속에 집어넣기
        buttons = GetComponentsInChildren<ShopItemButton>();

    }

    private void OnEnable()
    {
        if (firstOpen == false)
        {
            firstOpen = true;
            ButtonInstantitate();
        }
    }

    private void AwakeInIt()
    {
        Dictionary<string, List<string>> buttonCSV;
        // TODO : 여기서 CSV를 읽어서 열의 갯수를 채크   
        buttonCSV = CSVReader.ReadCSVFile("CSVFiles/Shop/Shop");
        DataManager.SetData(buttonCSV);
        childCount = DataManager.GetCount(100);     // ID 수치 변경시 매개변수값 변경
    }

    private void ButtonInstantitate()       // 버튼을 인스턴트해주는 함수
    {
        for (int i = 0; i < childCount; i++)
        {
            prefabClone = Instantiate(buttonPrefab);
            prefabClone.transform.SetParent(this.transform);
            prefabClone.transform.localScale = Vector3.one;
            rect = prefabClone.GetComponent<RectTransform>();
            rect.anchoredPosition3D = Vector3.zero;     // AnchoredPosition3D 로 인스턴트 후 포지션 이상한것을 해결
        }
    }

    private void InItButtonNum()        // 버튼들의 고유번호를 넣어주는 함수
    {

    }

    IEnumerator Test()
    {
        for (int i = 0; i < childCount; i++)
        {
            prefabClone = Instantiate(buttonPrefab);
            prefabClone.transform.SetParent(this.transform);
            prefabClone.transform.localScale = Vector3.one;
            rect = prefabClone.GetComponent<RectTransform>();

            yield return null;
        }
    }


}       // ClassEnd
