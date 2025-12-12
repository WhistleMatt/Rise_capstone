using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    public PlayerInteractEvent unityEvent;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void interact(GameObject invoker)
    {
        unityEvent.Invoke(invoker);
    }

    private void OnTriggerStay(Collider other)
    {

     if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("we have been invoked");
            unityEvent.Invoke(other.gameObject);
         }
    }
}
