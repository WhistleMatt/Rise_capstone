using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healing_Item :IITem
{
    //public Texture texture;

    public Healing_Item() 
    {
        ItemID = "100001";
        Name = "Red Potion";
        TargetPlayer = true;
        IsVoidItem = false;
        Uses = 3;
        SetImageString("Images/Consumables/estus-flask-1");
    }

    public override void UseItem(PlayerStatsController stats)
    {
        stats.setPHealth(stats.getPHealth() + 4.0f);
    }
}
