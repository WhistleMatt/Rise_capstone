using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkPointManager : MonoBehaviour
{
    // Start is called before the first frame update
    public List<GameObject> gameObjects = new List<GameObject>();
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CheckPointTrigger()
    {
        foreach (GameObject go in gameObjects)
        {
            go.SetActive(false);
        }
    }

    public void Reset()
    {
        foreach (GameObject obj in gameObjects)
        {
            {
                obj.SetActive(true);
            }
        }
    }
}
