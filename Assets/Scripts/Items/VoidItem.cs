using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoidItem : IITem
{
    public VoidItem() 
    {
        ItemID = "000000";
        Name = "";
        TargetPlayer = false;
        IsVoidItem = true;
        Uses = -1;
        SetImageString("Images/Consumables/blue_flask");
    }

    public override void UseItem()
    {
        Debug.Log("shiddd");
    }
}
