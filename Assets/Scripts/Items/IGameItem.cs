using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public interface IGameItem
{
}

public class MonoGameItem : MonoBehaviour, IGameItem
{
    [SerializeField]
    public string m_itemName;

    public virtual int UseItem(GameObject _user)
    {
        return 0;
    }

    public virtual int GetCharges()
    { 
        return 0; 
    }

    public virtual void Reset()
    {

    }
}