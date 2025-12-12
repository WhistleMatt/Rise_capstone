using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class arrowPulse : MonoBehaviour
{
    private float time;
    private float timer = 3;
    private bool shrink=false;
    [SerializeField]private float shrinkRatio = 1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.gameObject.transform.localScale = new Vector3(1 * time * shrinkRatio, 1 * time * shrinkRatio, 1 * time * shrinkRatio);
        time = Time.deltaTime;
        if (time>timer)
        {
            time = 0;
            if (!shrink)
            {
                shrink = true;
            }
            else
            {
                shrink = false;
            }
        }
    }
}
