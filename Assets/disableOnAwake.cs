using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Nicolas Chatziargiriou
public class disableOnAwake : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private GameObject Marker;
    void Start()
    {
        Marker.SetActive(false);
    }
  
    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject getMarker()
    {
        return Marker;
    }
    
}
