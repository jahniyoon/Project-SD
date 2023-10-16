using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemButton : MonoBehaviour
{

    public int buttonNum;

    private Transform childTextObj; // 자식오브젝트로 있는 텍스트 오브젝트를 가져올 변수

    private TextMeshProUGUI itemCountText;      // 아이템 현재갯수,최대 갯수를 표기해줄 텍스트

    private Image itemSprite;       // 아이템 이미지가 들어갈 변수


    private void Awake()
    {
        GetChildTextObj();
        itemCountText = childTextObj.GetComponent<TextMeshProUGUI>();

        UpdateItemCountText();      // 텍스트 값을 변경해주는 함수
    }

    void Start()
    {

    }


    void Update()
    {

    }


    private void GetChildTextObj()
    {
        childTextObj = transform.GetChild(0);   // 자신 바로 아래에 있는 자식오브젝트를 가져옴
    }

    public void UpdateItemCountText()
    {
        // TODO : 게임메니저,플레이어 가 가지고 있는 bool 값을 이용하던지
        //          무언가를 이용해서 텍스트를 업데이트 해주어야함

        itemCountText.text = "";
    }


}       // ClassEnd
