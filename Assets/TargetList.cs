using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
//Nicolas Chatziargiriou
public class TargetList : MonoBehaviour
{
  [SerializeField]
  private List<GameObject> Targets = new List<GameObject>();
    // Update is called once per frame
    void Update()
    {
        Targets=Targets.Distinct().ToList();
       
    }

    public List<GameObject> getTargetList()
    {
        return Targets;
    }

    public void removeTarget(GameObject _target)
    {
        Targets.Remove(_target.gameObject.transform.root.gameObject);
    }
    public void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject);
        if (other.gameObject.transform.root.tag == "Enemy")
        {
            Targets.Add(other.gameObject.transform.root.gameObject);
        }
    }


    public void OnTriggerExit(Collider other)
    {
        if(other.gameObject.transform.root.tag =="Enemy")
        {
            Targets.Remove(other.gameObject.transform.root.gameObject);
        }
    }
}
