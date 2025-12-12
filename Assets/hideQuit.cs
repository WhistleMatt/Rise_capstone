using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hideQuit : MonoBehaviour
{
    [SerializeField] private GameObject _quitBtn;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake()
    {
        _quitBtn.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
