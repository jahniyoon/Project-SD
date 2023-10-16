using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class BossBullet : MonoBehaviour
{
    public Transform target;
    private Rigidbody rigid;
    

    //csv
    public int hp = default;
    public float lifeTime = default;  //destroy할때 사라지는 시간?
    public int damage = default;
    public float speed = default;


    // Start is called before the first frame update
    void Start()
    {
        GetData();

        rigid = GetComponent<Rigidbody>();

        rigid.velocity = transform.forward * speed;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GetData()
    {
        Dictionary<string, List<string>> dataDictionary = default;
        dataDictionary = CSVReader.ReadCSVFile("CSVFiles/Golem_Projectile"); //이름으로 가져옴
        DataManager.SetData(dataDictionary);
        hp = (int)DataManager.GetData(2004, "HP");//이름으로 가져오는거라서 순서상관 X 0번째 행  //변수 선언은 해야함
        lifeTime = (float)DataManager.GetData(2004, "Projectile_Lifetime");
        damage = (int)DataManager.GetData(2004, "Damage");
        speed = (float)DataManager.GetData(2004, "Speed");
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.tag.Equals("Player"))
        {
            hp--;

            if(hp == 0)
            {
                Destroy(this.gameObject, lifeTime);
            }
            
        }
    }
}
