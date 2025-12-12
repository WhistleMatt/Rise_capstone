using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SoulsGirlDialogue : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI levelUpPanel;

     public int chapter = 0;

    public string getDialogue()
    {
        this.gameObject.GetComponent<DialogueController>().toggleLock(false);
        Debug.Log(chapter);
        switch (chapter)
        {
            // Level up Dialogue
            case 0:
                chapter++;
                return "Hello brave hunter....";

            case 1:

                Cursor.lockState = CursorLockMode.Confined;

                levelUpPanel.gameObject.transform.parent.gameObject.SetActive(true);
                GameObject.FindGameObjectWithTag("SoulGirl").GetComponent<DialogueController>().toggleLock(true);
                levelUpPanel.gameObject.transform.parent.gameObject.GetComponent<LevelUpPanelController>().updateBox();
                Cursor.visible = true;


                chapter++;
                return "I am the doll of the dream, I am here to assist you on your journey..... (Click ACCEPT to continue).";
            case 2:

                chapter += 9999;

                Cursor.visible = false;
                GameObject.FindGameObjectWithTag("Single").GetComponent<PlayFabStats>().UpdateEXP((int)GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStatsController>().getExperiancePoints());
                return "Good luck on your journey brave hunter... Pray thee worship the knight's statue, and return safely";
            default:
                chapter = 0;
                GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInput>().SwitchCurrentActionMap("Player");
                this.gameObject.GetComponent<DialogueController>().toggleLock(true);
                return null;
            case 3:
                chapter++;
                return "The year is 15XX.... The dark wizard Ramiel rules...";
            case 4:
                chapter++;
                return "The people of the land are forced to hide away as the planet is overrun by monsters.... ";
            case 5:
                chapter++;
                return "It is up to you brave hero to defeat Ramiel and free the land. ";
            case 6:
                chapter++;
                return "Good luck....";
            case 7:
                chapter += 9999;
                SceneManager.LoadScene("Level1");
                return "you should not see this";
            case 8:
                chapter++;
                return "I am here to teach you!";
            case 9:
                chapter++;
                return "Movement is done with: WASD";
            case 10:
                chapter++;
                return "You can dodge with LEFT SHIFT, and block with LEFT CTRL";
            case 11:
                chapter++;
                return "Attacking is done with LEFT and RIGHT MOUSE buttons. If you have enough stamina clicks can be comboed together for special attacks and extra damage!"; //make more in depth explanation
            case 12:
                chapter++;
                return "Having a hard time hitting an enemy? Target with MIDDLE CLICK, and cycle enemies with TAB.";
            case 13:
                chapter++;
                return "If you're having a tough time out there, be sure to return here and talk to my friend to level up. Be careful, XP is lost at death!";
            case 14:
                chapter++;
                return "P.S Rest at campfires to restore your health, but be careful enemies also get to rest!";
            case 15:
                chapter += 9999;
                return "Good luck! ";

            // Level up Dialogue end 
            // tutorial dialogue
            case 16:
                chapter++;

                return "On your adventure you will need to know a few things about the game world...";
            case 17:
                chapter++;
                GameObject.FindGameObjectWithTag("TutorialController").GetComponent<ShowTutorial>().xpArrow.SetActive(true);
                return "This over here is your XP bar, you gain XP by killing monsters and spend it in the hub area.";
            case 18:
                chapter++;
                GameObject.FindGameObjectWithTag("TutorialController").GetComponent<ShowTutorial>().xpArrow.SetActive(false);
                GameObject.FindGameObjectWithTag("TutorialController").GetComponent<ShowTutorial>().staminaArrow.SetActive(true);
                return "This is your STAMINA bar, STAMINA is needed to pull off attacks. The more stamina you have, the better combos you can perform with LEFT and RIGHT CLICK.";
            case 19:
                chapter++;
                GameObject.FindGameObjectWithTag("TutorialController").GetComponent<ShowTutorial>().staminaArrow.SetActive(false);
                GameObject.FindGameObjectWithTag("TutorialController").GetComponent<ShowTutorial>().hpArrow.SetActive(true);
                return "This is your HP bar, it shows how many hit points you have.";
            case 20:
                chapter++;
                GameObject.FindGameObjectWithTag("TutorialController").GetComponent<ShowTutorial>().hpArrow.SetActive(false);
                GameObject.FindGameObjectWithTag("TutorialController").GetComponent<ShowTutorial>().CompassArrow.SetActive(true);
                return "This is your COMPASS, use it to help navigate the labyrinth. It show your position relative to the CAMERA.";
            case 21:
                chapter++;
                GameObject.FindGameObjectWithTag("TutorialController").GetComponent<ShowTutorial>().CompassArrow.SetActive(false);
                GameObject.FindGameObjectWithTag("TutorialController").GetComponent<ShowTutorial>().enemyHpArrow.SetActive(true);
                return "This is the ENEMY HP bar, when you attack an ENEMY, their HP will be displayed here.";
            case 22:
                chapter++;
                GameObject.FindGameObjectWithTag("TutorialController").GetComponent<ShowTutorial>().enemyHpArrow.SetActive(false);
                GameObject.FindGameObjectWithTag("TutorialController").GetComponent<ShowTutorial>().campArrow.SetActive(true);
                return "This is a BONFIRE, interact with it to restore your HP and save your progress. Enemies will also rest if you use it.";
            case 23:
                chapter++;
                GameObject.FindGameObjectWithTag("TutorialController").GetComponent<ShowTutorial>().campArrow.SetActive(false);
                GameObject.FindGameObjectWithTag("TutorialController").GetComponent<ShowTutorial>().statueArrow.SetActive(true);
                return "This is a KNIGHT STATUE, interact with it to travel between worlds.";
            case 24:
                chapter++;
                GameObject.FindGameObjectWithTag("TutorialController").GetComponent<ShowTutorial>().statueArrow.SetActive(false);
                GameObject.FindGameObjectWithTag("TutorialController").GetComponent<ShowTutorial>().playerArrow.SetActive(true);
                return "That's you! Use the knowledge you have aqcuired to succeed on your journey. If you die you will respawn at the start of the world.";
            case 25:
                chapter += 9999;
                GameObject.FindGameObjectWithTag("TutorialController").GetComponent<ShowTutorial>().playerArrow.SetActive(false);

                return "If you need to adjust your MOUSE sensativity, you can do so by hitting ESC.... Good Luck!";
            case 26:
                chapter++;
                return "Warrior it is I, the maiden from your dream..";
            case 27:
                chapter++;
                return "You seem to have met with a terrible fate..";
            case 28:
                chapter++;

                return "Pray bid thee, return to me when you have XP to increase your STATS.. may doing so increase your odds of SUCCESS...";
            case 29:
                chapter++;

                return "(Reviving)";
            case 30:
                GameObject.FindGameObjectWithTag("Single").GetComponent<PlayFabStats>().updateLevelTutorial(1);
                chapter += 999;
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                return "";
            case 31:
                chapter++;
                return "Congratulations warrior! You have defeated the dark wizard!";
            case 32:
                chapter += 9999;
                return "That marks the end of this adventure! You may return to the hub area if you wish to continue with your current stats.";

        }
    }
        public void setChapter(int _chapter)
    {
        chapter = _chapter;
    }

}
