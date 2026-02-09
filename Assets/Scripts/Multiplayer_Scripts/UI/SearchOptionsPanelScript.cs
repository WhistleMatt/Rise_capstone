using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SearchOptionsPanelScript : MonoBehaviour
{
    [SerializeField] private Button PublicBTN;
    [SerializeField] private Button PrivateJoinBTN;
    [SerializeField] private Button cancelBTN;

    [SerializeField] private GameObject m_MainCanvas;
    [SerializeField] private GameObject m_optionsPanel;
    [SerializeField] private GameObject m_LobbySearchPanel;
    [SerializeField] private GameObject m_WaitingRoomCanvas;

    public string LobbyID {  get; set; }

    public string PasswordInput { get; set; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PublicBTN.onClick.AddListener(() =>
        {
            m_LobbySearchPanel.SetActive(true);
            this.gameObject.SetActive(false);
        });

        PrivateJoinBTN.onClick.AddListener(async () =>
        {
            if (LobbyID.Length > 0)
            {
                string name = "Foo Fighter";
                string Username = PlayerPrefs.GetString("USERNAME", name);

                JoinLobbyByIdOptions options = new JoinLobbyByIdOptions()
                {
                    Password = PasswordInput,
                    Player = new Player()
                    {
                        Data = new Dictionary<string, PlayerDataObject>
                        {
                            {"PlayerName", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member, Username)}
                        },
                    }
                };

                var result = await Multiplayer_lobby_manager.Instance.JoinPrivateLobbyBYID(LobbyID, options);
                if (result)
                {
                    m_WaitingRoomCanvas.SetActive(true);
                    m_MainCanvas.SetActive(false);
                }
            }
        });

        cancelBTN.onClick.AddListener(() =>
        {
            m_optionsPanel.SetActive(true);
            this.gameObject.SetActive(false);
        });
    }
}
