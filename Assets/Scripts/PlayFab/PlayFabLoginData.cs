using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayFabLoginData : MonoBehaviour
{
    PlayFabLoginData Instance;
    // Start is called before the first frame update

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
        GetStatistics();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void GetStatistics()
    {

    }
}
