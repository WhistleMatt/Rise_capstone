using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Multiplayer_Boss_Spawnner : MonoBehaviour
{
    [SerializeField] private GameObject _wall;

    [SerializeField] private GameObject _boss;
    [SerializeField] private bool bossBeaten = false;
    private bool has_triggered = false;

    [SerializeField] private GameObject _hpBar;

    [SerializeField] private List<GameObject> _playersList = new List<GameObject>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
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
        var InteractObject = other.gameObject;
        if (other.gameObject.tag == "Player")
        {
            foreach (var p in _playersList)
            {
                if (InteractObject = p.gameObject)
                {
                    return;
                }
            }
            _playersList.Add(InteractObject);
            if (!bossBeaten && _playersList.Count == 2)
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
