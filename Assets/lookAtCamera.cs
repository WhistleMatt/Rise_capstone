using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lookAtCamera : MonoBehaviour
{
   [SerializeField] GameObject _cam;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 camLoc = new Vector3(_cam.transform.position.x, this.transform.position.y, _cam.transform.position.z);
        this.transform.LookAt(camLoc);
    }
}
