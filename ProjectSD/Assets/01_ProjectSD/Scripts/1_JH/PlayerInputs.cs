using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputs : MonoBehaviour
{
    [Header("Player Input Values")]
    public bool shoot;   // 사격
    public bool select;    // 결정
    public bool shop;    // 상점

    // 사격 버튼 입력 : 왼쪽 마우스, 오른쪽 트리거
    public void OnRightTrigger(InputValue value)
    {
        RightTrggerInput(value.isPressed);
    }
    public void RightTrggerInput(bool input)
    {
        shoot = input;
    }

    // 결정 버튼 입력 : 오른쪽 마우스, A버튼
    public void OnSelectButton(InputValue value)
    {
        SelectInput(value.isPressed);
    }
    public void SelectInput(bool input)
    {
        select = input;
    }

    // 상점 버튼 입력 : 오른쪽 마우스, A버튼
    public void OnShopButton(InputValue value)
    {
        ShopInput(value.isPressed);
    }
    public void ShopInput(bool input)
    {
        shop = input;
    }
}
