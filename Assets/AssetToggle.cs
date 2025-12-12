using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AssetToggle : MonoBehaviour
{
    [SerializeField] private GameObject _Floor;
    [SerializeField] private GameObject _Dressings;
    [SerializeField] private GameObject _tempPlatform;
    private GameObject _tempPlatformInst;
    private bool _enabled=false;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void enable()
    {
        _enabled = true;
    }
    public void disaable()
    {
        _enabled = false;
    }
    // Update is called once per frame
    void Update()
    {
        if (_enabled)
        {
            if (SceneManager.GetActiveScene().name == "Level2")
            {

                if (Input.GetKeyDown(KeyCode.Alpha1))
                {
                    if (_Floor!=null && _Floor.active == true)
                    {
                        _Floor.SetActive(false);
                        _tempPlatformInst = GameObject.Instantiate(_tempPlatform, GameObject.FindGameObjectWithTag("Player").transform.position, Quaternion.identity);
                        Debug.Log("Made Plane");
                        GameObject.FindGameObjectWithTag("Debugger").GetComponent<FileWriter>().writeDebug("Debugger: Floor Toggled Off");
                    }
                    else
                    {
                        _Floor.SetActive(true);
                        Destroy(_tempPlatformInst);
                        GameObject.FindGameObjectWithTag("Debugger").GetComponent<FileWriter>().writeDebug("Debugger: Floor Toggled On");
                    }
                }

                if (Input.GetKeyDown(KeyCode.Alpha2))
                {
                    if (_Dressings!=null && _Dressings.active == true)
                    {
                        _Dressings.SetActive(false);
                        GameObject.FindGameObjectWithTag("Debugger").GetComponent<FileWriter>().writeDebug("Debugger: Dressings Toggled Off");
                    }
                    else
                    {
                        _Dressings.SetActive(true);
                        GameObject.FindGameObjectWithTag("Debugger").GetComponent<FileWriter>().writeDebug("Debugger: Dressings Toggled On");
                    }
                }

                if (Input.GetKeyDown(KeyCode.Alpha3))
                {
                    GameObject.FindGameObjectWithTag("Single").GetComponent<PlayFabStats>().updateTutorial(1);
                }
                if (Input.GetKeyDown(KeyCode.Alpha4))
                {
                    GameObject.FindGameObjectWithTag("Single").GetComponent<PlayFabStats>().updateTutorial(0);
                }

                if (Input.GetKeyDown(KeyCode.Alpha5))
                {
                    GameObject[] eHitboxes = GameObject.FindGameObjectsWithTag("Enemy_Hitbox");
                    GameObject[] pHitboxes = GameObject.FindGameObjectsWithTag("Player_Hitbox");
                    GameObject[] hurtboxes = GameObject.FindGameObjectsWithTag("hurt box");
                    GameObject[] pushboxes = GameObject.FindGameObjectsWithTag("pushbox");

                    foreach (GameObject x  in eHitboxes)
                    {
                        x.GetComponent<MeshRenderer>().enabled =true;
                    }
                    foreach (GameObject x in pHitboxes)
                    {
                        x.GetComponent<MeshRenderer>().enabled = true;
                    }
                    foreach (GameObject x in hurtboxes)
                    {
                        x.GetComponent<MeshRenderer>().enabled = true;
                    }
                    foreach (GameObject x in pushboxes)
                    {
                        x.GetComponent<MeshRenderer>().enabled = true;
                    }
                }
                if (Input.GetKeyDown(KeyCode.Alpha6))
                {
                    GameObject[] eHitboxes = GameObject.FindGameObjectsWithTag("Enemy_Hitbox");
                    GameObject[] pHitboxes = GameObject.FindGameObjectsWithTag("Player_Hitbox");
                    GameObject[] hurtboxes = GameObject.FindGameObjectsWithTag("hurt box");
                    GameObject[] pushboxes = GameObject.FindGameObjectsWithTag("pushbox");

                    foreach (GameObject x in eHitboxes)
                    {
                        x.GetComponent<MeshRenderer>().enabled = false;
                    }
                    foreach (GameObject x in pHitboxes)
                    {
                        x.GetComponent<MeshRenderer>().enabled = false;
                    }
                    foreach (GameObject x in hurtboxes)
                    {
                        x.GetComponent<MeshRenderer>().enabled = false;
                    }
                    foreach (GameObject x in pushboxes)
                    {
                        x.GetComponent<MeshRenderer>().enabled = false;
                    }
                }

                if (Input.GetKeyDown(KeyCode.Alpha7))
                {
                    GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStatsController>().setExperiancePoints(GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStatsController>().getExperiancePoints()+1);
                }
                if (Input.GetKeyDown(KeyCode.Alpha8))
                {
                    GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStatsController>().setExperiancePoints(GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStatsController>().getExperiancePoints() - 1);
                }
                if (Input.GetKeyDown(KeyCode.Alpha9))
                {
                    GameObject[] markers = GameObject.FindGameObjectsWithTag("pathMarker");

                    foreach (GameObject x in markers)
                    {
                        x.GetComponent<MeshRenderer>().enabled = true;
                    }
                }
                if (Input.GetKeyDown(KeyCode.Alpha0))
                {
                    GameObject[] markers = GameObject.FindGameObjectsWithTag("pathMarker");

                    foreach (GameObject x in markers)
                    {
                        x.GetComponent<MeshRenderer>().enabled = false;
                    }
                }
            }

        }
    }
}
