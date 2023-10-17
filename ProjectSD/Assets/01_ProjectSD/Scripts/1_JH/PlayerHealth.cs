using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    PlayerShooter shooter;
    CharacterController playerController;
    public float health;
    public float startHealth;

    public void OnEnable()
    {
        GetData();
        GameManager.instance.SetMaxHealth(startHealth);
        GameManager.instance.SetPlayer(true);
        health = startHealth;
    }
    private void Start()
    {
        shooter=GetComponent<PlayerShooter>();
        playerController = gameObject.GetComponent<CharacterController>();  
    }
    public void OnDamage(float damage)
    {
        health -= damage;
        GameManager.instance.SetHealth(health);
        if (0 >= health)
        {
            GameManager.instance.GameOver();
        }
    }
    public void GetData()
    {
        Dictionary<string, List<string>> dataDictionary = default;
        dataDictionary = CSVReader.ReadCSVFile("CSVFiles/Player_Table");
        startHealth = float.Parse(dataDictionary["HP"][0]);
    }
    public void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Finish"))
        {
            GameManager.instance.GameOver();
        }
    }

}
