using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using Unity.Services.Lobbies.Models;
using System.Collections.Generic;
using Unity.Services.Lobbies;
using System.Threading;
using System.Threading.Tasks;
using UnityEditor;

public class MultiplayerLobbyscreenScript : MonoBehaviour
{
    //main functionality buttons
    [SerializeField] private Button LeaveButton;
    [SerializeField] private Button StartButton;

    private bool isHost = false;

    List<Player> playerList = new List<Player>();

    [SerializeField] private TextMeshProUGUI mPLobbyCode;

    //players names
    [SerializeField] private TextMeshProUGUI m_P1_Name;
    [SerializeField] private TextMeshProUGUI m_P2_Name;
    [SerializeField] private TextMeshProUGUI m_P3_Name;
    [SerializeField] private TextMeshProUGUI m_P4_Name;

    //our disconnect buttons
    [SerializeField] private Button m_disconnect_P2_BTN;
    [SerializeField] private Button m_disconnect_P3_BTN;
    [SerializeField] private Button m_disconnect_P4_BTN;

    private float updateTimer = 3f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        isHost = Multiplayer_lobby_manager.Instance.IsHost();

        mPLobbyCode.text = Multiplayer_lobby_manager.Instance.GetLobbyCode();

        playerList = Multiplayer_lobby_manager.Instance.GetConnectedPlayers();

        m_disconnect_P2_BTN.onClick.AddListener(async () =>
        {
            if (playerList.Count > 1)
            {
                await Multiplayer_lobby_manager.Instance.kickPlayer(playerList[1]);
            }
        });
        if (playerList.Count < 2 || !isHost)
        {
            m_disconnect_P2_BTN.gameObject.SetActive(false);
        }

        m_disconnect_P3_BTN.onClick.AddListener(async () =>
        {
            if (playerList.Count > 2)
            {
                await Multiplayer_lobby_manager.Instance.kickPlayer(playerList[2]);
            }
        });
        if (playerList.Count < 3 || !isHost)
        {
            m_disconnect_P3_BTN.gameObject.SetActive(false);
        }

        m_disconnect_P4_BTN.onClick.AddListener(async () =>
        {
            if (playerList.Count > 3)
            {
                await Multiplayer_lobby_manager.Instance.kickPlayer(playerList[3]);
            }
        });
        if (playerList.Count < 4 || !isHost)
        {
            m_disconnect_P4_BTN.gameObject.SetActive(false);
        }


        if (playerList.Count >= 2)
        {
            m_P2_Name.gameObject.SetActive(true);
            if (isHost)
            {
                m_disconnect_P2_BTN.gameObject.SetActive(true);
            }
        }

        if (playerList.Count >= 3)
        {
            m_P3_Name.gameObject.SetActive(true);
            if (isHost)
            {
                m_disconnect_P3_BTN.gameObject.SetActive(true);
            }
        }

        if (playerList.Count >= 4)
        {
            m_P4_Name.gameObject.SetActive(true);
            if (isHost)
            {
                m_disconnect_P4_BTN.gameObject.SetActive(true);
            }
        }

        foreach (Player player in playerList)
        {
            PlayerDataObject data = new PlayerDataObject(PlayerDataObject.VisibilityOptions.Public);
            PlayerDataObject nameData = new PlayerDataObject(PlayerDataObject.VisibilityOptions.Public);

            if (player.Data.TryGetValue("IsHost", out data))
            {
                player.Data.TryGetValue("PlayerName", out nameData);
                if (data.Value == "Host")
                {
                    m_P1_Name.text = nameData.Value;
                }
                else
                {
                    if (m_P2_Name.text == "")
                    {
                        m_P2_Name.text = nameData.Value;
                    }
                    else if (m_P3_Name.text == "")
                    {
                        m_P3_Name.text = nameData.Value;
                    }
                    else
                    {
                        m_P4_Name.text = nameData.Value;
                    }
                }
            }
        }

        LeaveButton.onClick.AddListener(async () =>
        {
            Debug.Log("we are clicked!");
            await Multiplayer_lobby_manager.Instance.LobbyScreenQuit();

            SceneManager.LoadScene("Level1");
        });

        StartButton.onClick.AddListener(async () =>
        {
            await Multiplayer_lobby_manager.Instance.StartSession();
        });
    }

    private void Update()
    {
        updateTimer -= Time.deltaTime;
        if(updateTimer <= 0 )
        {
            float timerRefresh = 6.0f;
            updateTimer = timerRefresh;
            refreshLobbyUIText();
        }
    }

    public void CopyIDToClip()
    {
        GUIUtility.systemCopyBuffer = mPLobbyCode.text;
    }

    private bool refreshLobbyUIText()
    {
        PlayerDataObject nameData = new PlayerDataObject(PlayerDataObject.VisibilityOptions.Public);
        playerList.Clear();
        
        playerList = Multiplayer_lobby_manager.Instance.GetConnectedPlayers();
        isHost = Multiplayer_lobby_manager.Instance.IsHost();
        switch (playerList.Count)
        {
            case 1:
                m_disconnect_P2_BTN.gameObject.SetActive(false);
                m_disconnect_P3_BTN.gameObject.SetActive(false);
                m_disconnect_P4_BTN.gameObject.SetActive(false);



                m_P1_Name.text = Multiplayer_lobby_manager.Instance.GetPlayerDataAttribute("PlayerName");
                m_P2_Name.gameObject.SetActive(false);
                m_P3_Name.gameObject.SetActive(false);
                m_P4_Name.gameObject.SetActive(false);
                break;
            case 2:
                if(isHost)
                {
                    m_disconnect_P2_BTN.gameObject.SetActive(true);
                    m_disconnect_P3_BTN.gameObject.SetActive(false);
                    m_disconnect_P4_BTN.gameObject.SetActive(false);
                }
                else
                {
                    m_P1_Name.text = Multiplayer_lobby_manager.Instance.GetHostPlayerDataAttribute("PlayerName");
                }


               playerList[1].Data.TryGetValue("PlayerName", out nameData);

                
                m_P2_Name.gameObject.SetActive(true);
                m_P2_Name.text = nameData.Value;
                m_P3_Name.gameObject.SetActive(false);
                m_P4_Name.gameObject.SetActive(false);
                break;
            case 3:
                if (isHost)
                {
                    m_disconnect_P2_BTN.gameObject.SetActive(true);
                    m_disconnect_P3_BTN.gameObject.SetActive(true);
                    m_disconnect_P4_BTN.gameObject.SetActive(false);
                }
                else
                {
                    m_P1_Name.text = Multiplayer_lobby_manager.Instance.GetHostPlayerDataAttribute("PlayerName");
                }


                playerList[1].Data.TryGetValue("PlayerName", out nameData);
                m_P2_Name.gameObject.SetActive(true);
                m_P2_Name.text = nameData.Value;

                playerList[2].Data.TryGetValue("PlayerName", out nameData);
                m_P3_Name.gameObject.SetActive(true);
                m_P3_Name.text = nameData.Value;

                m_P4_Name.gameObject.SetActive(false);
                break;
            case 4:
                if (isHost)
                {
                    m_disconnect_P2_BTN.gameObject.SetActive(true);
                    m_disconnect_P3_BTN.gameObject.SetActive(true);
                    m_disconnect_P4_BTN.gameObject.SetActive(true);
                }
                else
                {
                    m_P1_Name.text = Multiplayer_lobby_manager.Instance.GetHostPlayerDataAttribute("PlayerName");
                }

                playerList[1].Data.TryGetValue("PlayerName", out nameData);
                m_P2_Name.gameObject.SetActive(true);
                m_P2_Name.text = nameData.Value;

                playerList[2].Data.TryGetValue("PlayerName", out nameData);
                m_P3_Name.gameObject.SetActive(true);
                m_P3_Name.text = nameData.Value;

                playerList[3].Data.TryGetValue("PlayerName", out nameData);
                m_P4_Name.gameObject.SetActive(true);
                m_P4_Name.text = nameData.Value;
                break;
            default:
                break;
        }
        return true;
    }

}
