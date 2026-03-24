using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DisconnectedUI : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI disconnectReason;

    public void DisconnectReasonText(string text)
    {
        disconnectReason.text = text;
    }

    public void ReturnToHub()
    {
        SceneManager.LoadScene("Level1");
    }

}
