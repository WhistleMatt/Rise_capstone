using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowTutorial : MonoBehaviour
{

    public GameObject xpArrow;
    public GameObject staminaArrow;
    public GameObject hpArrow;
    public GameObject CompassArrow;
    public GameObject enemyHpArrow;
    public GameObject playerArrow;
    public GameObject campArrow;
    public GameObject statueArrow;
   
    // Start is called before the first frame update
    void Start()
    {

        GameObject.FindGameObjectWithTag("Single").GetComponent<PlayFabStats>().getPlayerStats();

    }

    // Update is called once per frame
    void Update()
    {
       
        if (GameObject.FindGameObjectWithTag("Single").GetComponent<PlayFabStats>().getTutorial() == 0)
        {
            GameObject.FindGameObjectWithTag("SoulGirl").GetComponent<SoulsGirlDialogue>().chapter = 16;
            //GameObject.FindGameObjectWithTag("Single").GetComponent<PlayFabStats>().updateSeenTutorial();
            //   GameObject.FindGameObjectWithTag("Single").GetComponent<PlayFabStats>().hasSeenTutorial = 1;
            // GameObject.FindGameObjectWithTag("Single").GetComponent<PlayFabStats>().SetStatistics();

            GameObject.FindGameObjectWithTag("Single").GetComponent<PlayFabStats>().updateTutorial(1);


            GameObject.FindGameObjectWithTag("SoulGirl").GetComponent<DialogueController>().recieveDialogue();

        }
    }
}
