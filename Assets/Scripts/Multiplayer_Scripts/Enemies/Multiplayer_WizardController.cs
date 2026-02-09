using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Multiplayer_WizardController : NetworkBehaviour
{
    [SerializeField] private List<GameObject> _spawns;
    [SerializeField] private List<GameObject> _fireBallSpawners;
    [SerializeField] private GameObject _ammo;

    private NetworkVariable<int> _spawnLocationVar = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone);
    private NetworkVariable<bool> _alteredSpawn = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone);
    private bool networkstarted = false;

    [SerializeField] private float _teleportTime; //time to teleport after shooting Fireballs
    [SerializeField] private float _teleportTimer;
    [SerializeField] private float _shrinkRatio;
    private float _shrinkTime;
    private float _shrinkTimer = 0;
    // Start is called before the first frame update


    public override void OnNetworkSpawn()
    {
        networkstarted = true;

        this.gameObject.SetActive(false);
        base.OnNetworkSpawn();
    }

    // Update is called once per frame
    void Update()
    {
        if (!networkstarted) return;
        //this.transform.LookAt(GameObject.FindGameObjectWithTag("Player").transform.position);
        _teleportTimer = _teleportTimer + Time.deltaTime;
        if (_teleportTimer >= _teleportTime)
        {
            Teleport();
        }
    }

    private void shrink()
    {
        _shrinkTime = this.gameObject.GetComponent<AudioSource>().clip.length;



        this.gameObject.transform.localScale = new Vector3(0.5f - _shrinkTimer * _shrinkRatio, 0.5f - _shrinkTimer * _shrinkRatio, 0.5f - _shrinkTimer * _shrinkRatio);
        if (this.gameObject.transform.localScale.x < 0)
        {
            this.gameObject.transform.localScale = new Vector3(0f, 0f, 0f);
        }
        //this.gameObject.transform.localScale = new Vector3(1,1,1);
        _shrinkTimer = _shrinkTimer + Time.deltaTime;

    }
    private void shootFireball()
    {
        foreach (GameObject fireball in _fireBallSpawners)
        {

            Instantiate(_ammo, new Vector3(fireball.transform.position.x, fireball.transform.position.y, fireball.transform.position.z), Quaternion.LookRotation(fireball.transform.forward));
        }
    }

    [Rpc(SendTo.Server, InvokePermission = RpcInvokePermission.Owner)]
    private void ChooseTeleSpotRpc(int _number)
    {
            _spawnLocationVar.Value = _number;
    }

    private void Teleport()
    { 
        this.gameObject.GetComponent<AudioSource>().Play();
        shrink();
        if (_shrinkTimer >= _shrinkTime)
        {
            int ranVal = Random.Range(0, _spawns.Count);
            var ourObjects = GameObject.FindObjectsByType<Network_Player_Controller>(FindObjectsSortMode.None);
            foreach (Network_Player_Controller controller in ourObjects)
            {
                if (controller.AreWeOwner())
                {
                    if (controller.IsOwnedByServer)
                    {
                        ChooseTeleSpotRpc(ranVal);
                    }
                    else
                    {
                        continue;
                    }
                }
                else
                {
                    continue;
                }
            }
            
            this.transform.position = _spawns[_spawnLocationVar.Value].gameObject.transform.position;
            this.gameObject.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            this.transform.LookAt(GameObject.FindGameObjectWithTag("Player").transform.position);
            _teleportTimer = 0;
            _shrinkTimer = 0;
            shootFireball();
        }

    }
}
