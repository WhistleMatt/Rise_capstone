using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Multiplayer_Host_start_Script : MonoBehaviour
{
    [SerializeField] private Button start_btn;
    [SerializeField] private Button cancel_btn;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        start_btn.onClick.AddListener(async () =>
        {
            var inputstring = await Multiplayer_lobby_manager.Instance.StartSession();

            this.gameObject.SetActive(false);
        });

        cancel_btn.onClick.AddListener(async () =>
        {
            await Multiplayer_lobby_manager.Instance.CloseLobby();

            SceneManager.LoadScene("Level1");
        });

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
