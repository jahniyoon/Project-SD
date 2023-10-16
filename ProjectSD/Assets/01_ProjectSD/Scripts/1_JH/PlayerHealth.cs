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
        health = startHealth;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnDamage(float damage)
    {
        health -= damage;
        if (0 <= health)
        {
            GameManager.instance.GameOver();
        }
    }
  
}
