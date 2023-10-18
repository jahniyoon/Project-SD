using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextDisolve : MonoBehaviour
{
    public string colorName;
    public TMP_Text textObj;
    public float colorValue = 255;
    private Rigidbody rigid;
    public float power;

    // Start is called before the first frame update
    void Start()
    {
       rigid = GetComponent<Rigidbody>();
        rigid.AddForce(new Vector3(power/3, power, 0), ForceMode.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        DamageScreenUpdate();
    }

    // 스크린 데미지 업데이트
    public void DamageScreenUpdate()
    {
        // 블러드 스크린의 값이 있을 경우 0이 될때까지 실행
        if (0 < colorValue)
        {
            colorValue -= Mathf.CeilToInt(1 * Time.deltaTime);
            
            
            if (colorName == "yellow")
            {
                textObj.color = new Color(255, 255, 0, colorValue / 100);
            }
            else if (colorName == "red")
            {
                textObj.color = new Color(255, 0, 0, colorValue / 100);
            }
            else
            {
                textObj.color = new Color(255, 255, 255, colorValue / 100);
            }
        }
        if(colorValue <= 0)
        {
            Destroy(gameObject);
        }
    }

}
