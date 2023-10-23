using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopGoldUI : MonoBehaviour
{

    private Image goldImage;           // 골드 이미지
    public TextMeshProUGUI goldText;   // 현재골드를 출력해줄 텍스트
    private GameManager gameManager;
    private Coroutine myCoroutine; // 코루틴 정지용 코루틴 보관 함수

    private void Awake()
    {
        AwakeInIt();
    }

    void Start()
    {
        
    }

    
    void Update()
    {
    }

    private void AwakeInIt()
    {

        goldImage = this.transform.GetChild(0).GetComponent<Image>();
        goldText = this.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        gameManager = FindAnyObjectByType<GameManager>();

        EventSubscription(); // 이벤트를 구독하는 함수


    }       // AwakeInIt()

    private void EventSubscription()
    {
        gameManager.playerGoldUpdateEvent += GoldTextUpdate;
    }       // EventSubscription()


    public void GoldTextUpdate()
    {

        // 상점패널이 꺼져 있을 경우
        if (gameManager.shopPanel.activeSelf == false)
        {
            // 골드만 수정
            goldText.text = gameManager.PlayerGold.ToString();

            return;
        }
        float currentGold = int.Parse(goldText.text);
        float targetGold = gameManager.PlayerGold;
        float gapNum = Math.Abs(targetGold - currentGold);
        // 숫자 갭이 2 이상일 경우
        if (gapNum > 1)
        {
            // 카운트 시간
            float duration = 0.5f;
            // 숫자 갭에 따라 다른 카운트 시간 설정
            switch (gapNum)
            {
                // 10000 이상
                case >= 10000:
                    duration = 8f;
                    break;

                // 1000 이상
                case >= 1000:
                    duration = 4f;
                    break;

                // 100 이상
                case >= 100:
                    duration = 2f;
                    break;

                // 10 이상
                case >= 10:
                    duration = 1f;
                    break;
            }

            // 코루틴이 실행 중일 경우
            if (myCoroutine != null)
            {
                // 코루틴 정지
                StopCoroutine(myCoroutine);
            }

            // 카운팅 효과 코루틴 호출
            myCoroutine = StartCoroutine(CountNumEffect(duration, targetGold, currentGold, goldText));

            return;
        }
        // 아닐 경우
        goldText.text = gameManager.PlayerGold.ToString();
    }       // GoldTextUpdate()

    // 텍스트 카운팅 효과 함수
    IEnumerator CountNumEffect(float duration, float target, 
        float current, TextMeshProUGUI goldText)
    {
        float offset = (target - current) / duration;
        while (current < target)
        {
            current += offset * Time.deltaTime;
            goldText.text = ((int)current).ToString();
            yield return null;

        }

        current = target;
        goldText.text = ((int)current).ToString();
    }
}       // ClassEnd
