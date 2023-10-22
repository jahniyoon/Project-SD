using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireCheck : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    //발에 스크립트 넣고 fireFx가 화염병 스크립트 참고
    private void OnTriggerStay(Collider other)
    {
        if(other.tag.Equals("Fire"))
        {

            //TODO:몇 초마다 장판 데미지 및 슬로우
            
        }

        
    }

}
