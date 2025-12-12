using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartBossFight : MonoBehaviour
{
    [SerializeField] private GameObject _wall;
    //[SerializeField]
    // private List<GameObject> EnemyTemplates = new List<GameObject>();
    // [SerializeField] private List<GameObject> EnemyList = new List<GameObject>();
    [SerializeField] private GameObject _boss;
    [SerializeField] private bool bossBeaten = false;
    private bool has_triggered = false;

    [SerializeField] private GameObject _hpBar;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (has_triggered == true)
        {
            return;
        }
        if (other.gameObject.tag == "Player")
        {
            if (!bossBeaten)
            {
                spawnWall();
                resetEnemies();
                has_triggered = true;
                //this.gameObject.SetActive(false);
            }

        }
    }

   

    private void spawnWall()
    {
        _wall.gameObject.SetActive(true);
        
    }

    private void resetEnemies()
    {
        _boss.gameObject.SetActive(true);
        _hpBar.SetActive(true);
        
        /*
        foreach (GameObject x in EnemyTemplates)
        {
            x.SetActive(false);
        }

        foreach (GameObject x in EnemyList)
        {
            Destroy(x);
        }
        EnemyList.Clear();
        foreach (GameObject x in EnemyTemplates)
        {
            EnemyList.Add(Instantiate(x));
        }

        foreach (GameObject x in EnemyList)
        {
            x.SetActive(true);
        }*/
    }
}
