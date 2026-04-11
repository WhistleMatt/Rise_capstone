using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.VisualScripting;

public class SearchOptionsPanelScript : MonoBehaviour
{
    [SerializeField] private Button PublicBTN;
    [SerializeField] private Button PrivateJoinBTN;
    [SerializeField] private Button cancelBTN;

    [SerializeField] private GameObject privateRoomCanvas;


    [SerializeField] private GameObject m_MainCanvas;
    [SerializeField] private GameObject m_optionsPanel;
    [SerializeField] private GameObject m_LobbySearchPanel;
    [SerializeField] private GameObject m_PrivateSearchPanel;
    [SerializeField] private GameObject m_WaitingRoomCanvas;

    public bool SearchToggle { get; private set; } = false;

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
            m_PrivateSearchPanel.SetActive(true);
            this.gameObject.SetActive(false);
        });

        cancelBTN.onClick.AddListener(() =>
        {
            m_optionsPanel.SetActive(true);
            this.gameObject.SetActive(false);
        });

        privateRoomCanvas.SetActive(false);

    }
}
