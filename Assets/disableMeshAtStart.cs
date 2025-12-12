using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class disableMeshAtStart : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.GetComponent<MeshRenderer>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
