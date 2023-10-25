using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    PlayerShooter shooter;
    CharacterController playerController;
    public float health;
    public float startHealth;
    public Image bloodScreen;
    public float bloodScreenValue;
    public float playerHeight;
    public void OnEnable()
    {
        GetData();
        GameManager.instance.SetMaxHealth(startHealth);
        GameManager.instance.SetPlayer(true);
        health = startHealth;
        this.transform.position = new Vector3(0, playerHeight, 0);
        GameManager.instance.playerHeight = playerHeight;
    }
    private void Start()
    {
        shooter=GetComponent<PlayerShooter>();
        playerController = gameObject.GetComponent<CharacterController>();
        Vector3 ccPos = Vector3.zero;
        ccPos.y = playerHeight * -1f;
        playerController.center = ccPos;
    }
    private void Update()
    {
        DamageScreenUpdate();
    }
    public void OnDamage(float damage)
    {
        health -= damage;
        bloodScreenValue += 80;

        OVRInput.SetControllerVibration(1f, 1f, OVRInput.Controller.LTouch);
        OVRInput.SetControllerVibration(1f, 1f, OVRInput.Controller.RTouch);


        if (bloodScreenValue >= 500)
        {
            bloodScreenValue = 500;
        }
        GameManager.instance.SetHealth(health);
        if (0 >= health)
        {
            GameManager.instance.GameOver();
        }
    }
    public void GetData()
    {
        startHealth = (int)DataManager.GetData(1, "HP");
        playerHeight = (float)DataManager.GetData(1, "Height");
    }
    public void OnTriggerEnter(Collider other)
    {
        //if(other.CompareTag("Finish"))
        //{
        //    GameManager.instance.GameOver();
        //}
    }

    // 스크린 데미지 업데이트
    public void DamageScreenUpdate()
    {
        // 블러드 스크린의 값이 있을 경우 0이 될때까지 실행
        if (0 < bloodScreenValue)
        {
            bloodScreenValue -= Mathf.CeilToInt(1 * Time.deltaTime);
            bloodScreen.color = new Color(255, 0, 0, bloodScreenValue / 1000);
        }
       
    }
    // 블러드 스크린의 값 조정
    public void SetBloodScreen(float _health)
    {
        // 체력이 낮을 경우 더 붉어지게 하기 위한 값
        // 체력이 100이면 변함없음
        float newHealth = (-1 * _health + 250);

        bloodScreenValue += 200 + newHealth;
        if (2000 < bloodScreenValue)
        { bloodScreenValue = 2000; }
    }

}
