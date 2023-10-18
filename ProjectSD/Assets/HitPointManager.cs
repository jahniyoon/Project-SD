using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HitPointManager : MonoBehaviour
{
    public BossHitPoint bossHitPoint;
    public float disableTime = 5.0f;
    public float critical = 1.5f;

    public void OnDamage(float damage)
    {
        transform.root.GetComponent<Boss>().OnDamage(damage * critical);
        bossHitPoint.OnDamage(damage);
        StartCoroutine(DisableAndEnableHitPoint());
    }

    IEnumerator DisableAndEnableHitPoint()
    {
        bossHitPoint.gameObject.SetActive(false);
        yield return new WaitForSeconds(disableTime);
        bossHitPoint.gameObject.SetActive(true);
    }

    
}
