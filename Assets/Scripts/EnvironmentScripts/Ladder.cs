using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour
{
    [SerializeField] private Transform m_ladderTop;
    [SerializeField] private Transform m_ladderBottom;
    [SerializeField] private Transform m_player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GrabOntoLadder(GameObject _object)
    {
        m_player = _object.transform;
        float topDis = Vector3.Distance(m_ladderTop.position, _object.transform.position);
        float bottomDis = Vector3.Distance(m_ladderBottom.position, _object.transform.position);
        if(topDis < bottomDis)
        {
            _object.transform.position = m_ladderBottom.position;
        }
        else
        {
            _object.transform.position = m_ladderTop.position;
        }
    }
}
