using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;

public class LobbyListIUI : MonoBehaviour
{
    [SerializeField] private GameObject m_uiPrefab;
    [SerializeField] private GameObject m_content;
    [SerializeField] private Button m_refresh_BTN;
    [SerializeField] private Button m_cancel;
    [SerializeField] private GameObject m_selectScreen;
    [SerializeField] private Canvas m_parentCanvas;
    [SerializeField] private GameObject m_waitCanvas;
    [SerializeField] private TextMeshProUGUI errorText;

    [SerializeField] private GameObject PasswordInputBox;
    [SerializeField] private GameObject RoomNameInputBox;

    public bool SearchPrivate { get; set; } = false;
    public bool SearchByName { get; set; } = false;
    public int MaxPlayers { get; set; } = 2;
    public int ListNumber { get; set; } = 25;
    public string RoomName { get; set; } = "";
    public string RoomPassword { get; set; } = "";

    public static LobbyListIUI Instance;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Instance = this;


        m_refresh_BTN.onClick.AddListener(async () =>
        {
            //var Query = await Multiplayer_lobby_manager.Instance.ListLobbies();
            await RefreshLobbyList();

        });

        m_cancel.onClick.AddListener(() =>
        {
            m_selectScreen.SetActive(true);
            m_parentCanvas.gameObject.SetActive(false);

        });
    }

    private async void OnEnable()
    {
        await CreateUI();
    }

    public async Task<bool> CreateUI()
    {
        List<Lobby> _lobbyList = new List<Lobby>();

        QueryLobbiesOptions queryOptions = new QueryLobbiesOptions()
        {
            Count = 10,
            Filters = new List<QueryFilter>()
            {
                new QueryFilter(QueryFilter.FieldOptions.IsLocked, SearchPrivate.ToString(), QueryFilter.OpOptions.EQ),
                new QueryFilter(QueryFilter.FieldOptions.AvailableSlots, "0", QueryFilter.OpOptions.NE)
            }
        };

        if (RoomName.Length > 0 && SearchByName)
        {
            queryOptions.Filters.Add(new QueryFilter(QueryFilter.FieldOptions.Name, RoomName, QueryFilter.OpOptions.EQ));
        }

        _lobbyList = await Multiplayer_lobby_manager.Instance.ListLobbies(queryOptions);

        foreach (Lobby _lobby in _lobbyList)
        {
            var newObject = GameObject.Instantiate(m_uiPrefab, m_content.transform);
            if (m_content.gameObject.transform.childCount > 1)
            {
                Vector3 relocateVar = newObject.transform.localPosition;
                var sampleObject = m_content.gameObject.transform.GetChild(m_content.gameObject.transform.childCount - 2);
                newObject.transform.localPosition = new Vector3(newObject.transform.localPosition.x, sampleObject.localPosition.y - 110f, newObject.transform.localPosition.z);
            }
            newObject.GetComponent<Multiplayer_Lobby_Panel>().SetText(_lobby);
            newObject.GetComponent<Multiplayer_Lobby_Panel>().setPassword(RoomPassword);
            newObject.GetComponent<Multiplayer_Lobby_Panel>().SetHasPassword(PasswordInputBox.activeInHierarchy);
        }
        return true;
    }

    public void OnSelectPrivate(bool _privacy)
    {
        SearchPrivate = _privacy;
        if (SearchPrivate)
        {
            PasswordInputBox.SetActive(true);
        }
        else
        {
            PasswordInputBox.SetActive(false);
        }
        int menus = m_content.transform.childCount;
        for (int i = 0; i < menus; i++)
        {
            m_content.transform.GetChild(i).GetComponent<Multiplayer_Lobby_Panel>().SetHasPassword(SearchPrivate);
        }
    }
    public void OnTypePassword(string _password)
    {
        RoomPassword = _password;
        int menus = m_content.transform.childCount;
        for (int i = 0; i < menus; i++)
        {
            m_content.transform.GetChild(i).GetComponent<Multiplayer_Lobby_Panel>().setPassword(RoomPassword);
        }
    }

    public void OnSelectName(bool _named)
    {
        SearchByName = _named;
        if (SearchByName)
        {
            RoomNameInputBox.SetActive(true);
        }
        else
        {
            RoomNameInputBox.SetActive(false);
        }
    }

    private async Task<bool> RefreshLobbyList()
    {
        List<GameObject> _currentChildren = new List<GameObject>();
        for (int i = 0; i < m_content.transform.childCount; i++)
        {
            _currentChildren.Add(m_content.transform.GetChild(i).gameObject);
        }
        int childcount = _currentChildren.Count;
        for (int i = childcount - 1; i >= 0; i--)
        {
            GameObject.Destroy(_currentChildren[i]);
        }
        _currentChildren.Clear();
        await CreateUI();
        return true;
    }

    public void OpenWaitingUI(bool worked = true)
    {
        if (!worked)
        {
            errorText.gameObject.SetActive(true);
            errorText.text = Multiplayer_lobby_manager.Instance.ErrorText;
            return;
        }
        m_waitCanvas.SetActive(true);
        this.transform.parent.gameObject.SetActive(false);
    }

}
