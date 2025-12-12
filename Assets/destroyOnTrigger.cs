using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroyOnTrigger : MonoBehaviour
{


    public HitBoxController.ePlayer id;
    // Start is called before the first frame update

    public void OnTriggerEnter(Collider other)
    {
        HurtBoxLogic hbl = other.gameObject.GetComponentInChildren<HurtBoxLogic>();
        if (hbl != null)
        {
            if (id != hbl.id)
            {
                Destroy(this.gameObject);
            }

        }
    }
}
