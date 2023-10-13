using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHitPoint : MonoBehaviour
{
    //private BoxCollider boxCollider;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("히트포인트 동작");
        //boxCollider = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //IEnumerator HitPoint()
    //{
    //    boxCollider.enabled = false;
    //    Debug.Log("비활성화");
    //    yield return new WaitForSeconds(5.0f);
    //    boxCollider.enabled = true;
    //    Debug.Log("활성화");
    //}

    ////임시 데미지 함수
    //public void OnDamage(float damage)
    //{
    //    //Weapon weapon = other.GetComponent<Weapon>();
    //    hp -= damage * 1.5f;
    //}

    //boxCollider.transform.localScale = new Vector3(3.0f, 3.0f, 3.0f); //객체 커지는거
    //boxCollider.size = new Vector3(3.0f, 3.0f, 3.0f);

    private void OnTriggerEnter(Collider other)
    {

        if(other.CompareTag("Test"))
        {
            Debug.Log("큐브에 닿음");
        }

        if(other.CompareTag("Bullet"))
        {
            Debug.Log("불렛에 트리거 댐");
            //StartCoroutine(HitPoint());     
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.CompareTag("Test"))
        {
            Debug.Log("큐브에 닿음");

        }
    }

}
