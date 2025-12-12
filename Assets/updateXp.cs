using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class updateXp : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.GetComponent<TMP_Text>().text = GameObject.FindGameObjectWithTag("Player").gameObject.GetComponent<PlayerStatsController>().getExperiancePoints().ToString();
    }
}
