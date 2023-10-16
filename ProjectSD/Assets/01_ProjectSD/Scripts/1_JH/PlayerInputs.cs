using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputs : MonoBehaviour
{
    [Header("Player Input Values")]
    public bool leftShoot;   // 왼쪽 사격
    public bool rightShoot;  // 오른쪽 사격
    public bool select;      // 결정
    public bool shop;        // 상점
    public bool deleteUnit;      // 유닛제거

    // 사격 버튼 입력 : 왼쪽 마우스, 왼쪽 트리거
    public void OnLeftTrigger(InputValue value)
    {
        LeftTriggerInput(value.isPressed);
    }
    public void LeftTriggerInput(bool input)
    {
        leftShoot = input;
    }

    // 사격 버튼 입력 : 왼쪽 마우스, 오른쪽 트리거
    public void OnRightTrigger(InputValue value)
    {
        RightTriggerInput(value.isPressed);
    }
    public void RightTriggerInput(bool input)
    {
        rightShoot = input;
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

    // 유닛제거 버튼 입력 : 왼쪽 마우스, 왼쪽 트리거
    public void OnDelete(InputValue value)
    {
        DeleteButtonInput(value.isPressed);
    }
    public void DeleteButtonInput(bool input)
    {
        deleteUnit = input;
    }

}
