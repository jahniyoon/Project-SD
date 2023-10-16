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
        goldText.text = gameManager.PlayerGold.ToString();
    }       // GoldTextUpdate()




}       // ClassEnd
