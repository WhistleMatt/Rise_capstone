using UnityEngine;
using Unity.Netcode;

public class Multiplayer_UI_Manager : MonoBehaviour
{
    private GameObject m_playerObject;

    [SerializeField] GameObject m_playerUICanvas;
    [SerializeField] GameObject m_basePauseCanvas;
    [SerializeField] GameObject m_QuitUICanvas;
    [SerializeField] GameObject m_ConfirmQuitUICanvas;

    public static Multiplayer_UI_Manager instance;

    private void Awake()
    {
        instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (m_playerObject == null)
        {
            var list = GameObject.FindObjectsByType<Network_Player_Controller>(FindObjectsSortMode.None);
            foreach (Network_Player_Controller controller in list)
            {
                if (controller.AreWeOwner())
                {
                    m_playerObject = controller.gameObject;
                }
            }
        }
    }

    public void PausePlayer()
    {
        if (m_playerObject == null) return;

        //m_playerUICanvas.SetActive(false);
        m_basePauseCanvas.SetActive(true);
        m_QuitUICanvas.SetActive(true);
        m_ConfirmQuitUICanvas.SetActive(false);
    }

    public void QueueQuit()
    {
        m_QuitUICanvas.SetActive(false);
        m_ConfirmQuitUICanvas.SetActive(true);
    }

    public void ConfirmQuit()
    {
        m_playerObject.GetComponent<Network_Player_Controller>().QuitMulti();
    }

    public void CancelQuit()
    {
        m_ConfirmQuitUICanvas.SetActive(false);
        m_QuitUICanvas.SetActive(true);
        m_basePauseCanvas.SetActive(false);
        //m_playerUICanvas.SetActive(true);
        m_playerObject.GetComponent<Network_Player_Controller>().UnpauseFromUI();
    }

    public void ExitCanvas()
    {
        //m_playerUICanvas.SetActive(true);
        m_basePauseCanvas.SetActive(false);
    }

    public void ReturnFromCanvas()
    {
        m_basePauseCanvas.SetActive(false);
        //m_playerUICanvas.SetActive(true);
        m_playerObject.GetComponent<Network_Player_Controller>().UnpauseFromUI();
    }
}
