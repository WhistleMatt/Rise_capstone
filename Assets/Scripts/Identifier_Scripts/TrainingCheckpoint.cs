using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainingCheckpoint : MonoBehaviour
{
    //[SerializeField] GameObject m_managerObj;
    //checkPointManager m_checkPointManager;
    // Start is called before the first frame update
    void Start()
    {
        //m_checkPointManager = m_managerObj.GetComponent<checkPointManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        //m_checkPointManager.CheckPointTrigger();
    }
}
