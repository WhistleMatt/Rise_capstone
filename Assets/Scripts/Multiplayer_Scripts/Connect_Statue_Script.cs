using UnityEngine;
using UnityEngine.SceneManagement;

public class Connect_Statue_Script : MonoBehaviour
{
    [SerializeField] private GameObject lobby_ui;
    [SerializeField] private GameObject player_ui;

    private float timer = 0;
    private float time = 3f;
    private bool beginTimer = false;
    private string level;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (beginTimer)//&& GameObject.FindGameObjectWithTag("Single").GetComponent<PlayFabStats>().wereStatsSent())
        {
            //  SceneManager.LoadSceneAsync(level);

            timer = timer + Time.deltaTime;
            if (timer > time)
            {
                beginTimer = false;
                timer = 0;
                //GameObject.FindGameObjectWithTag("Debugger").GetComponent<FileWriter>().writeDebug("Interaction Object Response: Knight Statue; Load Async Called");
                SceneManager.LoadSceneAsync(level);
                timer = 0;
                //GameObject.FindGameObjectWithTag("Debugger").GetComponent<FileWriter>().writeDebug("Interaction Object Response: Knight Statue; Load Async Called Confirmed");
            }
        }
    }

    public void OnLobbyConnection()
    {
        if (!lobby_ui.activeInHierarchy)
        {
            lobby_ui.SetActive(true);
            player_ui.SetActive(false);
        }
    }

    public void CancelLobbyConnection()
    {
        if (lobby_ui.activeInHierarchy)
        {
            lobby_ui.SetActive(false);
            player_ui.SetActive(true);
        }
        if (!player_ui.activeInHierarchy)
        {
            lobby_ui.SetActive(false);
            player_ui.SetActive(true);
        }
    }

    public void Teleport()
    {
        var player = GameObject.FindGameObjectWithTag("Player").gameObject;
        var playerStats = GameObject.FindGameObjectWithTag("Player").gameObject.GetComponent<PlayerStatsController>();
        //PlayFabStats.Instance.UpdateStats((int)playerStats.getPHealthMax(), (int)playerStats.getPHealth(), 7, 0, 11, 1, (int)playerStats.getManaMax(), (int)playerStats.getPMana(), (int)playerStats.getStaminahMax(), (int)playerStats.getPStamina(), (int)playerStats.getAttckMax(), (int)playerStats.getPAttck(), (int)playerStats.getPDefenseMax(), (int)playerStats.getPDefense(), (int)playerStats.getExperiancePoints(),2);
        PlayFabStats.Instance.UpdateStats((int)player.GetComponent<PlayerStatsController>().getPHealthMax(), (int)player.GetComponent<PlayerStatsController>().getPHealth(), (int)player.transform.position.x, (int)player.transform.position.y,
            (int)player.transform.position.z, 1, (int)player.GetComponent<PlayerStatsController>().getManaMax(), (int)player.GetComponent<PlayerStatsController>().getPMana(), (int)player.GetComponent<PlayerStatsController>().getStaminahMax(), (int)player.GetComponent<PlayerStatsController>().getPStamina(),
            (int)player.GetComponent<PlayerStatsController>().getAttckMax(), (int)player.GetComponent<PlayerStatsController>().getPAttck(), (int)player.GetComponent<PlayerStatsController>().getPDefenseMax(), (int)player.GetComponent<PlayerStatsController>().getPDefense(), (int)player.GetComponent<PlayerStatsController>().getExperiancePoints(), 3);


        beginTimer = true;
        timer = 2;
        level = "Level_3";
        SceneManager.LoadScene("Level_3");
    }

}
