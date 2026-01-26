using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//Nicolas Chatziargiriou
public class EnemyPathController : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> nodes = new List<GameObject>();
    private GameObject currentNode;
    [SerializeField]
    private GameObject nextNode;
    [SerializeField]
    private eMode pathMode;
    [SerializeField]
    private eMode m_starting_PathMode;
    private float speed;
    [SerializeField]
    private bool pathing =false;
    private int nodeTracker=0;
    private bool backTrack=false;

    private float timer=0;
    private float stompTimer=0.7f;
    [SerializeField] AudioSource _stompSound;
    public enum eMode
    {
        Stand,Pace,Lap
    }


    public void resumePathing()
    {
        this.gameObject.GetComponent<NavMeshAgent>().SetDestination(nextNode.transform.position);
        this.gameObject.GetComponent<Animator>().SetBool("isWalking", true);
    }

    private void pickNextDest( )
    {
        if (pathMode == eMode.Pace)
        {

            paceMode();
            this.gameObject.GetComponent<Animator>().SetBool("isWalking", true);
        }
        else if (pathMode == eMode.Lap)
        {
            lapMode();
            this.gameObject.GetComponent<Animator>().SetBool("isWalking", true);
        }
        else if (pathMode == eMode.Stand)
        {
            idleMode();
            this.gameObject.GetComponent<Animator>().SetBool("isWalking", false);
            this.gameObject.GetComponent<Animator>().SetBool("isIdle", true);
        }
   

    }
    private void paceMode()
    {
        
        if (nodeTracker >= nodes.Count - 1)
        {
            backTrack = true;
        }
        else if (nodeTracker <= 0)
        {
            backTrack = false;
        }

        if (backTrack == false)
        {
            nodeTracker++;
            currentNode = nextNode;
            nextNode = nodes[nodeTracker];
        }
        else
        {
            nodeTracker--;
            currentNode = nextNode;
            nextNode = nodes[nodeTracker];
        }
    }

    private void lapMode()
    {
    

        if (nodeTracker< nodes.Count-1)
        {
            nodeTracker++;
            currentNode = nextNode;
            nextNode = nodes[nodeTracker];
        }
        else
        {
            nodeTracker = 0;
            currentNode = nextNode;
            nextNode = nodes[nodeTracker];
        }
    }

    private void idleMode()
    {
        nextNode = currentNode;
    }
    public bool enablePathing()
    {
      
        if (m_starting_PathMode == eMode.Stand)
        {
            return false;
        }
        pathing = true;
        Debug.Log("Pathing Enabled");
        return true;
    }
    public void disablePathing()
    {
        pathing = false;
        this.gameObject.GetComponent<NavMeshAgent>().isStopped = true;
        Debug.Log("Path Disabled");
    }
    public GameObject getCurrentDestination()
    {
        return currentNode;
    }
    private float checkDistance()
    {
        return Vector3.Distance(this.gameObject.transform.position, nextNode.transform.position);
    }
    // Start is called before the first frame update
    void Start()
    {
        currentNode = nodes[0];
        nextNode = nodes[0];
    }

    // Update is called once per frame
    void Update()
    {
        if (pathing)
        {
            timer = timer+Time.deltaTime;
            if ( timer>=stompTimer)
            {
                _stompSound.Play();
                timer = 0;
            }

            if (checkDistance() <= 2f)
            {
                pickNextDest();
            }
            resumePathing();
        }
    }
}
