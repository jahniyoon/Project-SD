using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHitPoint : MonoBehaviour
{
    public float critical = 1.5f;
    private SphereCollider sphereCollider;
    public float weakPointScale = 3.0f;

    public float disableTime = 5.0f;   // 한대 맞았을 때 꺼지는 시간 넣을 변수
    public float upgradeTime = 7.0f;   // 업그레이드 했을 때 지속되는 시간 변수

    // Start is called before the first frame update
    void Start()
    {

        //Debug.Log("히트포인트 동작");
        sphereCollider = GetComponent<SphereCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator HitPoint()
    {
        sphereCollider.enabled = false;
        //Debug.Log("비활성화");
        yield return new WaitForSeconds(disableTime);
        sphereCollider.enabled = true;
        //Debug.Log("활성화");
    }

    

    //임시 데미지 함수
    public void OnDamage(float damage)
    {
        //GameManager.instance.Golem.GetComponent<Boss>().OnDamage(damage * critical);
        //boss.hp -= (int)(damage * 1.5f);
        //Debug.Log(boss.hp);
        transform.root.GetComponent<Boss>().OnDamage(damage * critical);
        StartCoroutine(HitPoint());
    }

    //public void OnTriggerEnter(Collider other)
    //{
    //    if (other.tag.Equals("Bullet"))
    //    {
    //        Bullet bullet = other.GetComponent<Bullet>();
    //        OnDamage(bullet.bulletDamage);
            
    //        StartCoroutine(HitPoint());
    //    }
    //}

    public void UpgraedWeakPoint()
    {
        float upgradeScale = 0.2f * weakPointScale;
        this.transform.localScale = new Vector3(upgradeScale, upgradeScale, upgradeScale);

        Invoke("ResetWeakPoint", upgradeTime);
    }

    public void ResetWeakPoint()
    {
        this.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
    }

    //boxCollider.transform.localScale = new Vector3(3.0f, 3.0f, 3.0f); //객체 커지는거

    //사이즈 조절 관련(아직 어떻게 조절할지 조건을 모르겠어서 보류)
    //boxCollider.size = new Vector3(3.0f, 3.0f, 3.0f);



}
