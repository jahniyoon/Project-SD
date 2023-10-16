using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    PlayerShooter shooter;
    public float health;
    public float startHealth;

    public void OnEnable()
    {
        GetData();
        health = startHealth;
    }

    public void OnDamage(float damage)
    {
        health -= damage;
        if (0 <= health)
        {
            GameManager.instance.GameOver();
        }
    }
    public void GetData()
    {
        Dictionary<string, List<string>> dataDictionary = default;
        dataDictionary = CSVReader.ReadCSVFile("CSVFiles/PC_Table");
        startHealth = float.Parse(dataDictionary["HP"][0]);
    }
  
}
