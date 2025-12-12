using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Empty_Item : MonoGameItem
{
    public override int UseItem(GameObject _user)
    {
        Debug.Log("Used Empty Item");
        return -1;
    }
}
