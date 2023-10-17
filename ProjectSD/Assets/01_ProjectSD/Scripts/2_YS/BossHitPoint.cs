using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHitPoint : MonoBehaviour
{
    public float critical = 1.5f;
    private BoxCollider boxCollider;
    public float weakPointScale = 3.0f;

    public float lifeTime = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        
        //Debug.Log("히트포인트 동작");
        boxCollider = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator HitPoint()
    {
        boxCollider.enabled = false;
        //Debug.Log("비활성화");
        yield return new WaitForSeconds(5.0f);
        boxCollider.enabled = true;
        //Debug.Log("활성화");
    }

    

    //임시 데미지 함수
    public void OnDamage(float damage)
    {
        GameManager.instance.Golem.GetComponent<Boss>().OnDamage(damage * critical);
        //boss.hp -= (int)(damage * 1.5f);
        //Debug.Log(boss.hp);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Bullet"))
        {
            Bullet bullet = other.GetComponent<Bullet>();
            OnDamage(bullet.bulletDamage);
            
            StartCoroutine(HitPoint());
        }
    }

    public void UpgraedWeakPoint()
    {
        this.transform.localScale = new Vector3(weakPointScale, weakPointScale, weakPointScale);

        Invoke("ResetWeakPoint", lifeTime);
    }

    public void ResetWeakPoint()
    {
        this.transform.localScale = Vector3.one;
    }

    //boxCollider.transform.localScale = new Vector3(3.0f, 3.0f, 3.0f); //객체 커지는거

    //사이즈 조절 관련(아직 어떻게 조절할지 조건을 모르겠어서 보류)
    //boxCollider.size = new Vector3(3.0f, 3.0f, 3.0f);



}
