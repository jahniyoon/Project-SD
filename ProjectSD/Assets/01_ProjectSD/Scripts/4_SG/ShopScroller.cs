using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopScroller : MonoBehaviour
{

    private Scrollbar scrollbar;    // VR로 움직이게 하기위한 Scrollbar컴포넌트
    private Transform rHandTrans;   // 얼마나 움직였는지 기준을 잡아줄 Transform
    private Input input;            // InputSystem 플레이어의 Transform을 받을때에 같이 받아올거임

    private bool isScroll;          // 스크롤을위해 상점에서 공격버튼을 눌렀는지 확인할 Bool변수
    public bool IsScroll
    {
        get { return isScroll; }
        set
        {
            if (isScroll != value)
            {
                isScroll = value;
                // TODO : isScroll의 bool 값에 따라서
                // true일때에 트렌스폼을 가져와야하고
                // false일때에 trasform을 null해주어야함
            }   // if : isScroll != value
        }     // Set
    }       // 프로퍼티

    private void Awake()
    {
        InItComponent();        // 컴포넌트 얻어오는 함수

    }

    void Start()
    {

    }


    void Update()
    {

    }

    private void InItComponent()    // 컴포넌트 가져오는 함수
    {
        scrollbar = this.transform.GetChild(1).GetComponent<Scrollbar>(); //스크롤바 컴포넌트가져오기
    }



    public void TransformInIt(Transform _RHandTrans)
    {
        // 프로퍼티로 가져오지말고 바로 불러야 할거같음

        // value값이 true가 된다면 한번만 Transform을 가져오고 그 값을 이용해서 스크롤 이동을 시킬거임

        if (rHandTrans == null || rHandTrans == default)
        {
            rHandTrans = _RHandTrans;
            if (input == null || input == default)
            {
                SerchPlayerInput();
            }

        }
        else { /* PASS */ }

    }       // TransformInIt(Transform _RHandTrans)



    private void SerchPlayerInput() // Input컴포넌트가 없다면 플레이어의 최상위 부모 찾아서 Input 넣어줌
    {
        Transform tempTrans = rHandTrans;
        while(tempTrans.parent != null)
        {
            tempTrans = tempTrans.parent;
        }

        input = tempTrans.GetComponent<Input>();

    }       // SerchPlayerInput()




}       // ClassEnd
