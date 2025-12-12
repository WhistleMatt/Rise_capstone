using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachToPlayer : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        transform.position = GameObject.FindGameObjectWithTag("Player").transform.position;
    }
}
