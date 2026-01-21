using UnityEngine;

public class Multiplayer_Lever_Script : MonoBehaviour
{
    [SerializeField] private GameObject lobby_ui;
    [SerializeField] private GameObject player_ui;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
}
