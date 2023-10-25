using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    private SphereCollider trapCollider;    //콜라이더

    public float stunTime;

    // Start is called before the first frame update
    void Start()
    {
        trapCollider = GetComponent<SphereCollider>();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))    // 적 태그를 만날 경우
        {
            other.gameObject.GetComponent<Enemy>().OnStun(stunTime);    // 스턴을 켠다.
        }

        if (other.CompareTag("Finish"))    // 보스 태그를 만날 경우
        {
            Boss boss = other.transform.root.GetComponent<Boss>();
            if (boss.state != Boss.State.STUN)
            {
                boss.OnStun(stunTime);    // 스턴을 켠다.
            }
        }
    }


}
