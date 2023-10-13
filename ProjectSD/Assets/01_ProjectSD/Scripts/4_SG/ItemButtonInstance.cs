using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemButtonInstance : MonoBehaviour
{
    Dictionary<string, List<string>> buttonCSV;

    //! 상점에서 살수있는 아이템의 열값을 구해서 열의 갯수에 따라서 상점의 버튼을 생성시키는 클레스
    //! Grid가 가지고 있어야함

    public ShopItemButton[] buttons;    // 그리드의 자식으로 Instace한 버튼들을 담아둘 배열

    [SerializeField]
    private GameObject buttonPrefab;        // 인스턴트할 버튼들의 프리펨

    private GameObject prefabClone;         // 클론을 이용해서 인스턴트할 예정

    private void Awake()
    {
        // TODO : 여기서 CSV를 읽어서 열의 갯수를 채크   

        // TODO : 여기서 만들어둔 버튼을 Instace 하면서 자식오브젝트로 묶기
        ButtonInstantitate();
    }

    private void Start()
    {
        // TODO : 자식오브젝트로 묶어놓은 버튼들을 배열속에 집어넣기
        buttons = GetComponentsInChildren<ShopItemButton>();

    }

    private void ButtonInstantitate()       // 버튼을 인스턴트해주는 함수
    {

    }

    private void InItButtonNum()        // 버튼들의 고유번호를 넣어주는 함수
    {

    }


}       // ClassEnd
