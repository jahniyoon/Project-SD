using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UI;

public class ShopScroller : MonoBehaviour
{

    private Scrollbar scrollbar;    // VR로 움직이게 하기위한 Scrollbar컴포넌트
    private Transform rHandTrans;   // 얼마나 움직였는지 기준을 잡아줄 Transform
    private Input input;            // InputSystem 플레이어의 Transform을 받을때에 같이 받아올거임

    private Transform pointPos;       // 기준점
    private Quaternion pointQuaternion;     // 움직임 계산할 Quaternion 기준점

    private Transform movePointPos;   // fixedUpdate 되는동안 움직인 포지션
    private Quaternion movePointQuaternion;

    // 쿼터니언 기준점 Y값 + 쿼터니언 움직인 Y값 = 거리 차이 값
    private float pointDistace;   // 기준점에서 움직인 포지션의 거리

    private float calculatedValue;  // pointDistace * calculatedValue = initValue
    private float initValue;        // 계산한 값을 넣어주어서 Scrollbar.value + initValue 시킬예정

    private Slider slider;

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
                // 플레이어가 상점에Ray를 조준하고 공격버튼을 누르면 true가 됨
                // 상점이 켜져있는데 발사버튼 누르지 않으면 false가 됨
            }   // if : isScroll != value
        }     // Set
    }       // 프로퍼티

    private List<Vector3> scrollPos;    // 플레이어가 클릭을 했을때에 FixedUpdate 에서 Vector3 값을 넣어줄거임
    private int scrollPosIdx;           // 움직일때에 비교할 값의 주소를 지정할  변수

    private void Awake()
    {
        ResetParamiter();       // 변수 default값으로 초기화하는함수
        InItNew();              // new할당 해주는 함수
        InItComponent();        // 컴포넌트 얻어오는 함수


    }


    private void FixedUpdate()
    {
        //if (IsScroll == true)
        //{
        //    //Debug.LogWarningFormat("클릭조건 들어옴");

        //    // 기준점 넣어주기
        //    if(pointPos == null || pointPos == default)
        //    {
        //        pointPos = rHandTrans;
        //        pointQuaternion = pointPos.rotation;
        //        Debug.LogFormat("PQ -> {0}", pointQuaternion);
        //    }

        //    if(pointPos != null && movePointPos != null)
        //    {
        //        Debug.LogFormat("!= 조건 들어옴");
        //        MoveScroll();
        //    }

        //    if(pointPos != null && pointPos.rotation != rHandTrans.rotation)
        //    {
        //        movePointPos = rHandTrans;
        //        movePointQuaternion = pointPos.rotation;
        //        Debug.LogFormat("PMQ -> {0}", movePointQuaternion);
        //    }

        //}       // Shop에 클릭중이라면

    }       // FixedUpdate()

    private void InItComponent()    // 컴포넌트 가져오는 함수
    {
        scrollbar = this.transform.GetChild(1).GetComponent<Scrollbar>(); //스크롤바 컴포넌트가져오기
    }

    private void InItNew()      // new 할당 해줄것들 할당해주는 함수
    {
        scrollPos = new List<Vector3>();    // List 할당
    }

    private void ResetParamiter()       // 변수 초기화
    {
        pointPos = default;
        movePointPos = default;
        calculatedValue = 0.05f;
    }



    public void TransformInIt(Transform _RHandTrans)
    {
        #region LEGACY
        // 프로퍼티로 가져오지말고 바로 불러야 할거같음

        // value값이 true가 된다면 한번만 Transform을 가져오고 그 값을 이용해서 스크롤 이동을 시킬거임
        //Debug.LogWarningFormat("Trans 기입은어느주기로 불러오지?");
        #region LEGACY
        //if (rHandTrans == null || rHandTrans == default)
        //{
        //    rHandTrans = _RHandTrans;
        //    if (input == null || input == default)
        //    {
        //        SerchPlayerInput();
        //    }

        //}
        //else { /* PASS */ }
        #endregion LEGACY

        rHandTrans = _RHandTrans;
        //if (input == null || input == default)
        //{
        //    SerchPlayerInput();
        //}
        #endregion LEGACY

        rHandTrans = _RHandTrans;
        CompareQuaternion();
    }       // TransformInIt(Transform _RHandTrans)


    private void CompareQuaternion()
    {
        if (IsScroll == true)
        {
            //Debug.LogWarningFormat("클릭조건 들어옴");

            // 기준점 넣어주기
            if (pointPos == null || pointPos == default)
            {
                pointPos = rHandTrans;
                pointQuaternion = pointPos.rotation;
                //Debug.LogFormat("PQ -> {0}", pointQuaternion);
            }

            if (pointPos != null && movePointPos != null)
            {
                //Debug.LogFormat("!= 조건 들어옴");
                MoveScroll();
            }

            if (pointPos != null && pointQuaternion != rHandTrans.rotation)
            {
                movePointPos = rHandTrans;
                movePointQuaternion = pointPos.rotation;
                //Debug.LogFormat("PMQ -> {0}", movePointQuaternion);
            }

        }       // Shop에 클릭중이라면
    }       //



    private void SerchPlayerInput() // Input컴포넌트가 없다면 플레이어의 최상위 부모 찾아서 Input 넣어줌
    {
        Transform tempTrans = rHandTrans;
        while (tempTrans.parent != null)
        {
            tempTrans = tempTrans.parent.transform;
        }

        input = tempTrans.GetComponent<Input>();

    }       // SerchPlayerInput()

    // 스크롤을 움직여주는 함수
    private void MoveScroll()
    {
        // TODO : 계산식을 만들어서 Pos위치 차이 Scrollbar이동 계산

        // 서로의 Y값을 더해서 움직인 값을 구함
        pointDistace = pointQuaternion.y + movePointQuaternion.y;
        //Debug.LogFormat("Dis -> {0}", pointDistace);
        initValue = pointDistace * calculatedValue;
        //Debug.LogFormat("InIt -> {0}", initValue);

        // 아래에 스크롤바 Value float로 기입 0.000 단위
        scrollbar.value = scrollbar.value - initValue;

        if(scrollbar.value > 1f)
        {
            scrollbar.value = 1f;
        }
        else if(scrollbar.value < 0f)
        {
            scrollbar.value = 0f;
        }

        //scrollbar.value = scrollbar.value + pointDistace;
        //Debug.LogFormat("Dis -> {0},  value -> {1}",initValue,scrollbar.value);

        pointPos = null;
        movePointPos = null;
        
    }




}       // ClassEnd
