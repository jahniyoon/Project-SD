using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemDuration : MonoBehaviour
{

    private Transform slot001;      // 슬롯 1의 Transform
    private Transform slot002;      // 슬롯 2의 Transform

    private Image slot001Img;       // 슬롯1의 이미지
    private Image slot002Img;       // 슬롯2의 이미지

    private TextMeshProUGUI slot001Text;    // 슬롯1의 텍스트
    private TextMeshProUGUI slot002Text;    // 슬롯2의 텍스트

    private float weaponDuration = 0f;      // 무기강화 지속시간
    private float weakPointDuration = 0f;   // 약점확대 지속시간

    private Coroutine durationCouroutine;   // 코루틴 캐싱
    private WaitForSeconds waitforSeconds;  // waitforseconds 캐싱

    private void Awake()
    {
        AwakeInIt();
        DurationGetData();
    }

    void Start()
    {
        slot001.gameObject.SetActive(false);
        slot002.gameObject.SetActive(false);
    }

    void Update()
    {
        if (GameManager.instance.isWeaponDuration == true)
        {
            slot001.gameObject.SetActive(true);
            GameManager.instance.isWeaponDuration = false;
            durationCouroutine = StartCoroutine(WeaponDurationStart());
        }
        else { /*PASS*/ }
        if(GameManager.instance.isWeakPointDuration == true)
        {
            slot002.gameObject.SetActive (true);
            GameManager.instance.isWeakPointDuration = false;
            durationCouroutine = StartCoroutine(WeakPointDurationStart());
        }
    }

    // Awake단계에서 얻어올것들
    private void AwakeInIt()
    {
        slot001 = this.transform.GetChild(0).GetComponent<Transform>();
        slot002 = this.transform.GetChild(1).GetComponent<Transform>();

        slot001Img = slot001.transform.GetChild(1).GetComponent<Image>();
        slot002Img = slot002.transform.GetChild(1).GetComponent<Image>();

        slot001Text = slot001.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        Debug.LogFormat("text -> {0}", slot001Text);

        slot002Text = slot002.transform.GetChild(2).GetComponent<TextMeshProUGUI>();

        waitforSeconds = new WaitForSeconds(1f);

    }       // AwakeInIt()

    // CSV에 내장되어있는 Duration을 가져오기
    private void DurationGetData()
    {
        weaponDuration = (float)DataManager.GetData(7010, "ActTime");
        weakPointDuration = (float)DataManager.GetData(7020, "ActTime");
    }       // DurationGetData()

    #region 지속시간 코루틴,텍스트 업데이트
    IEnumerator WeaponDurationStart()
    {        
        float nowduration = weaponDuration;
        WeaponDurationTextUpdate(nowduration);

        while (nowduration != 0f)
        {            
            yield return waitforSeconds;
            nowduration -= 1f;
            WeaponDurationTextUpdate(nowduration);
        }

        slot001.gameObject.SetActive(false);
    }       // WeaponDurationStart()

    private void WeaponDurationTextUpdate(float nowDuration)
    {         
        slot001Text.text = nowDuration.ToString();
    }       // WeaponDurationTextUpdate()

    IEnumerator WeakPointDurationStart()
    {
        float nowduration = weakPointDuration;
        WeakPointDurationTextUpdate(nowduration);

        while (nowduration != 0f)
        {
            yield return waitforSeconds;
            nowduration -= 1f;
            WeakPointDurationTextUpdate(nowduration);
        }

        slot002.gameObject.SetActive(false);
    }       // WeakPointDurationStart()

    private void WeakPointDurationTextUpdate(float nowDuration)
    {
        slot002Text.text = nowDuration.ToString();
    }       // WeakPointDurationTextUpdate(float)
    #endregion 지속시간 코루틴,텍스트 업데이트

}       // ClassEnd
