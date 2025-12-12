using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enableInteract : MonoBehaviour
{
    [SerializeField] GameObject _canvas;
    [SerializeField] private GameObject _player;
    private float _distance;
    [SerializeField] private float _allowedDistance=1;
    // Start is called before the first frame update
    void Start()
    {
        //_player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
       
        _distance = Vector3.Distance(this.transform.position, _player.transform.position);
        if (_distance <= _allowedDistance)
        {
            _canvas.gameObject.SetActive(true);
            _player.GetComponent<playerEnvironmentInteraction>().setInteractable(this.gameObject);
        }
        else
        {
            if (_player.GetComponent<playerEnvironmentInteraction>().getInteractObj()==this.gameObject)
            {
                _canvas.gameObject.SetActive(false);
                _player.GetComponent<playerEnvironmentInteraction>().setInteractable(null);
            }
          
        }
    }
}
