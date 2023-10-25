using TMPro;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody rigid = default;
    private SphereCollider bulletCollider;

    public int weaponID;
    public bool isUpgrade;
    public GameObject damageEffect;
    public GameObject hitEffect;


    [Header("Bullet Damage")]
    public float bulletDamage;         // 총알 데미지
    public float critIncrease;        // 총알 치명타율
    public float critProbability;   // 총알 치명타 확률
    public float finalDamage;               // 최종 데미지

    //public MeshRenderer mat;

    [Header("Bullet Option")]
    public float bulletSpeed = 10f;               // 총알 속도  
    public float bulletLifeTime = 10f;            // 총알 유지시간
    public float bulletSize = 0.5f;

    private TMP_Text bulletText;

    public bool isCrit;

    // Start is called before the first frame update
    void Start()
    {
        GetData();

        rigid = GetComponent<Rigidbody>();
        rigid.velocity = transform.forward * bulletSpeed;

        bulletCollider = GetComponent<SphereCollider>();
        bulletCollider.radius = bulletSize;

        DamageCalculate();
    }

    // 데미지 계산
    public void DamageCalculate()
    {
        float clit = Random.Range(0f, 1f);
        //Debug.Log("치명타율 : " + clit);

        if(clit < critProbability)
        {
            isCrit = true;
            //bulletCollider.radius = 2f;
            finalDamage = bulletDamage * (critIncrease / 100);
        }
        else
        finalDamage = bulletDamage;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") && !other.CompareTag("Bullet") && !other.CompareTag("Unit") && !other.CompareTag("Explosion") && !other.CompareTag("Stun") && !other.CompareTag("Fire"))
        {
            Vector3 effectPosition = this.transform.position;

            if (other.CompareTag("BossBullet"))
            {
                GameObject bossBullet = other.gameObject;
                bossBullet.GetComponent<BossBullet>().OnDamage(Mathf.FloorToInt(finalDamage));
                DamageEffect(other.tag);
                bulletCollider.enabled = false;

                //Destroy(bullet);
            }
            else if (other.CompareTag("Boss"))
            {
                Debug.Log("보스를 맞췄다.");
                GameManager.instance.HitBossGetGold();

                other.transform.root.GetComponent<Boss>().OnDamage(Mathf.FloorToInt(finalDamage));
                DamageEffect(other.tag);

                bulletCollider.enabled = false;


                //other.GetComponent<Boss>().OnDamage();
            }
            else if (other.CompareTag("WeakPoint")) //보스에서 약점 데미지 처리
            {
                //Debug.Log("약점을 맞췄다.");
                if (other.transform.GetComponent<BossHitPoint>() != null)
                {
                    GameManager.instance.HitBossGetGold();

                    other.transform.GetComponent<BossHitPoint>().OnDamage(Mathf.FloorToInt(finalDamage));
                    DamageEffect(other.tag);

                    bulletCollider.enabled = false;
                }


            }
            else if (other.CompareTag("Enemy"))
            {
                //Debug.Log("적을 맞췄다.");

                if (other.transform.GetComponent<EnemyNormal>() != null)
                {
                    other.transform.GetComponent<EnemyNormal>().enemy.
                        OnDamage(Mathf.FloorToInt(finalDamage));
                }
                else if (other.transform.GetComponent<EnemyFast>() != null)
                {
                    other.transform.GetComponent<EnemyFast>().enemy.
                    OnDamage(Mathf.FloorToInt(finalDamage));
                }

                DamageEffect(other.tag);
                bulletCollider.enabled = false;


                //other.transform.root.GetComponent<Boss>().OnWeakPointDamage(Mathf.FloorToInt(finalDamage));
            }




            GameObject hitParticle =
            Instantiate(hitEffect, effectPosition, this.transform.rotation);
            Destroy(hitParticle, 5f);
            Debug.Log("뭐때문에 생성될까 :" + other.gameObject.name);
            this.gameObject.SetActive(false);
        }
    }


    public void DamageEffect(string tag)
    {
        Vector3 effectPos = this.transform.position;
        Vector3 vDist = effectPos - GameManager.instance.PC.transform.position; // 이펙트와 플레이어의 거리
        Vector3 vDir = vDist.normalized;    // 이펙트와 플레이어의 방향
        Vector3 damageFXPos = vDir * 10;
        damageFXPos.y += GameManager.instance.playerHeight;
        GameObject damageFX =
             Instantiate(damageEffect, damageFXPos, Quaternion.identity);
        bulletText = damageFX.GetComponent<TextDisolve>().textObj;
        bulletText.text = string.Format("{0}", finalDamage);
        damageFX.GetComponent<TextDisolve>().colorName = "white";

        // 약점을 맞췄을 경우
        if (tag == "WeakPoint")
        {
            float critical = (float)DataManager.GetData(3001, "WeakpointRate");

            bulletText.text = string.Format("{0}", finalDamage * critical);
            damageFX.GetComponent<TextDisolve>().colorName = "yellow";
        }
        // 크리티컬일 경우
        if (isCrit)
        {
            damageFX.GetComponent<TextDisolve>().colorName = "red";
        }

        damageFX.transform.forward = Camera.main.transform.forward;


    }


    public void GetData()
    {
        int index = 2001;
        if(isUpgrade)    // 2000이면 강화무기
        { 
            index += 1; 
        }

        //Dictionary<string, List<string>> dataDictionary = default;
        //dataDictionary = CSVReader.ReadCSVFile("CSVFiles/Projectile_Table");
        //bulletDamage = float.Parse(dataDictionary["Att"][index]);
        //critIncrease = float.Parse(dataDictionary["Critical_Rate"][index]);
        //critProbability = float.Parse(dataDictionary["Critical_Chance"][index]);
        //bulletSpeed = float.Parse(dataDictionary["Vlocity"][index]);
        //bulletLifeTime = float.Parse(dataDictionary["LifeTime"][index]);

        bulletDamage = (float)DataManager.GetData(index, "Att");
        critIncrease = (int)DataManager.GetData(index, "Critical_Rate");
        critProbability = (float)DataManager.GetData(index, "Critical_Chance");
        bulletSpeed = (int)DataManager.GetData(index, "Velocity");
        bulletLifeTime = (int)DataManager.GetData(index, "LifeTime");
        
    }
}
