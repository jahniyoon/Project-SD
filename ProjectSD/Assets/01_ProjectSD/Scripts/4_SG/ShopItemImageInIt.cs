using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemImageInIt : MonoBehaviour
{
    // ShopItemImageInIt는 button들의 아이템 이미지를 넣어줄 스크립트
    public Sprite[] itemSprite;


    public enum imageNum        // itemTag과 동일한 숫자로 아이템 구별
    {
        UpgradeGun,                 // 0
        UpgradeWeakPoint,           // 1
        Trap,                       // 2
        FireBomb                    // 3
    }


}       // ClassEnd
