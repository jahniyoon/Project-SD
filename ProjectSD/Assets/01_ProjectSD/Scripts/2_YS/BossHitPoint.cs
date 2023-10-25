using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHitPoint : MonoBehaviour
{


    private SphereCollider sphereCollider;

    private MeshRenderer[] meshRenderers;

    public GameObject weakPrefab;
    

    [Header("CSV")]
    public float critical = default; //약점배율
    public float disableTime = default;   // 한대 맞았을 때 꺼지는 시간 넣을 변수
    public float weakPointScale = default;
    public float upgradeTime = default;                        


    private const int WEAKPOINTINDEX = 1;

    // Start is called before the first frame update
    void Start()
    {
        GetData();
        //Debug.Log("히트포인트 동작");
        sphereCollider = GetComponent<SphereCollider>();
        meshRenderers = GetComponentsInChildren<MeshRenderer>();

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GetData()
    {
        critical = (float)DataManager.GetData(3001, "WeakpointRate");
        disableTime = (float)DataManager.GetData(3001, "ActTime");
        weakPointScale = (float)DataManager.GetData(7020, "Value1");
        upgradeTime = (float)DataManager.GetData(7020, "ActTime");
    }

    IEnumerator HitPoint()
    {
        sphereCollider.enabled = false;

        foreach (MeshRenderer meshRenderer in meshRenderers)
        {
            meshRenderer.enabled = false;
        }

        //Debug.Log("비활성화");
        yield return new WaitForSeconds(disableTime);
        sphereCollider.enabled = true;

        foreach (MeshRenderer meshRenderer in meshRenderers)
        {
            meshRenderer.enabled = true;
        }

        //Debug.Log("활성화");
    }




    public void OnDamage(float damage)
    {
        //GameManager.instance.Golem.GetComponent<Boss>().OnDamage(damage * critical);
        //boss.hp -= (int)(damage * 1.5f);
        //Debug.Log(boss.hp);
        transform.root.GetComponent<Boss>().OnDamage(damage * critical);
        Instantiate(weakPrefab, transform.position, Quaternion.identity);
        AudioManager.instance.PlaySFX("Boss_WeakHit");
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
        float upgradeScale = 1f * weakPointScale;
        this.transform.localScale = new Vector3(upgradeScale, upgradeScale, upgradeScale);

        Invoke("ResetWeakPoint", upgradeTime);
    }

    public void ResetWeakPoint()
    {
        this.transform.localScale = new Vector3(1f, 1f, 1f);
        if (GameManager.buttonsList[WEAKPOINTINDEX].NowItemValue > 0)
        {
            GameManager.buttonsList[WEAKPOINTINDEX].NowItemValue -= 1;
        }
    }
    //사이즈 조절 관련(아직 어떻게 조절할지 조건을 모르겠어서 보류)
    //boxCollider.size = new Vector3(3.0f, 3.0f, 3.0f);


}
