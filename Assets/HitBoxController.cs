using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Nicolas Chatziargiriou
public class HitBoxController : MonoBehaviour
{
    [SerializeField] private GameObject swordHitBox;
    [SerializeField] private GameObject hurtBox;
    [SerializeField] private ePlayer id;
    // Start is called before the first frame update

    public enum ePlayer
    {
        p1,p2
    }

    private void InitBoxes()
    {
        swordHitBox.GetComponent <HitBoxLogic> ().id = id;
        hurtBox.GetComponent<HurtBoxLogic>().id = id;
    }

    public ePlayer getID()
    {
        return id;
    }
    public void HitBoxEnable(int id)
    {
        switch (id)
        {
            case (0):
                swordHitBox.SetActive(true);

                break;
        }
    }
    public void HitBoxDisable(int id)
    {
        switch (id)
        {
            case (0):
                swordHitBox.SetActive(false);
                break;
        }
    }

    public void HurtBoxEnable()
    {
        hurtBox.SetActive(true);

    }

    public void HurtBoxDisable()
    {
        hurtBox.SetActive(false);
    }
    private void Awake()
    {
        InitBoxes();
        HitBoxDisable(0);
    }
}
