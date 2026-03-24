using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class enableInteract : MonoBehaviour
{
    [SerializeField] GameObject _canvas;
    [SerializeField] private GameObject _player;
    [SerializeField] private List<Network_Player_Controller> _networkplayers = new List<Network_Player_Controller>();
    [SerializeField] private bool online_checkpoint = false;
    private float _distance;
    [SerializeField] private float _allowedDistance=1;
    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        if (_player == null)
        {
            online_checkpoint = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (online_checkpoint)
        {
            _networkplayers = GameObject.FindObjectsByType<Network_Player_Controller>(FindObjectsSortMode.InstanceID).ToList<Network_Player_Controller>();

            foreach (var player in _networkplayers)
            {
                _distance = Vector3.Distance(this.transform.position, player.transform.position);
                if (_distance <= _allowedDistance)
                {
                    _canvas.gameObject.SetActive(true);
                    player.GetComponent<playerEnvironmentInteraction>().setInteractable(this.gameObject);
                }
                else
                {
                    if (player.GetComponent<playerEnvironmentInteraction>().getInteractObj() == this.gameObject)
                    {
                        _canvas.gameObject.SetActive(false);
                        player.GetComponent<playerEnvironmentInteraction>().setInteractable(null);
                    }
                }
            }
        }

        if (_player != null)
        {
            _distance = Vector3.Distance(this.transform.position, _player.transform.position);
            if (_distance <= _allowedDistance)
            {
                _canvas.gameObject.SetActive(true);
                _player.GetComponent<playerEnvironmentInteraction>().setInteractable(this.gameObject);
            }
            else
            {
                if (_player.GetComponent<playerEnvironmentInteraction>().getInteractObj() == this.gameObject)
                {
                    _canvas.gameObject.SetActive(false);
                    _player.GetComponent<playerEnvironmentInteraction>().setInteractable(null);
                }
            }
        }
    }
}
