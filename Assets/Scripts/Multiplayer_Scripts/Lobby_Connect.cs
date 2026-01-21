using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Lobby_Connect : MonoBehaviour
{
    [SerializeField] private GameObject play_ui;
    [SerializeField] private GameObject lobby_ui;

    [SerializeField] private Button host_btn;
    [SerializeField] private Button join_btn;
    [SerializeField] private Button quit_btn;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;

        host_btn.onClick.AddListener(() =>
        { NetworkManager.Singleton.StartHost();
            this.gameObject.SetActive(false);
        }
        );


        join_btn.onClick.AddListener(() => {
            NetworkManager.Singleton.StartClient();
            this.gameObject.SetActive(false);
        });

        quit_btn.onClick.AddListener(() => {
            SceneManager.LoadScene("Level1");
        });
    }
}
