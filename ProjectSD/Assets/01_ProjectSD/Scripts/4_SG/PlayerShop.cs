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

    // Start is called before the first frame update
    void Start()
    {
        input = GetComponent<PlayerInputs>();

        // 상점과 버튼은 비활성화
        state = State.Close; 
        shopUI.gameObject.SetActive(false);
        isBtnEnable = false;    
    }

    void Update()
    {
        PlayerInput();  // 플레이어 입력

        if(btnName != null)
        {
            Debug.Log(btnName);
        }
    }

    //플레이어 입력
    public void PlayerInput()
    {
        ShopInput();    // 상점 버튼 입력
        SelectInput();  // 결정 버튼 입력
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
            // 버튼 입력이 가능한 경우
            // 버튼 입력 가능 여부 : Ray에 충돌한 오브젝트의 Layer가 8번일 경우에, PlayerShooter 전달 받음.
            if (isBtnEnable)    
            {
                PushButton(btnName);    // 버튼에 닿은 경우 해당 이름을 가져옴
            }
            input.select = false;       // 입력을 해제한다.
        }
    }
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
        if(btnName == "Exit")
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
