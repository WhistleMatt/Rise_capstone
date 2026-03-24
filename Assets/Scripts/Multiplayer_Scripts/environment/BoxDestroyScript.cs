using Unity.Netcode;
using UnityEngine;

public class BoxDestroyScript : NetworkBehaviour
{
    public NetworkVariable<bool> is_Destroyed = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    [SerializeField] GameObject destroyedModel;
    [SerializeField] AudioClip destroySound;
    [SerializeField] BoxCollider Collider;
    [SerializeField] BoxCollider Trigger;

    private NetworkObject netob;

    private GameObject model;
    public HitBoxController.ePlayer id;
    private bool spawnedBroken = false;

    [SerializeField] bool BreakUpdate = false;
    // Start is called before the first frame update
    private void Start()
    {
        netob = GetComponent<NetworkObject>();
    }

    private void Update()
    {
        if (is_Destroyed.Value && !BreakUpdate)
        {
            BreakUpdate = true;
            toggleDamged();
        }

        if (!is_Destroyed.Value && BreakUpdate)
        {
            BreakUpdate = false;
            toggleFixed();
        }
    }

    [Rpc(SendTo.Server, InvokePermission = RpcInvokePermission.Everyone)]
    public void toggleDamgedRpc()
    {
        is_Destroyed.Value = true;
    }

    public void toggleDamged()
    {

        this.gameObject.GetComponent<AudioSource>().clip = destroySound;
        this.gameObject.GetComponent<AudioSource>().volume = 0.5f;
        this.gameObject.GetComponentInChildren<MeshRenderer>().enabled = false;
        this.gameObject.GetComponent<Rigidbody>().useGravity = false;
        this.gameObject.GetComponent<BoxCollider>().enabled = false;
        Trigger.enabled = false;
        this.gameObject.GetComponent<AudioSource>().Play();
        if (!spawnedBroken)
        {
            //if (netob != null)
            //{
                //model = NetworkManager.Instantiate(destroyedModel);
                //spawnedBroken = true;
            //}
            //else
            //{
                model = Instantiate(destroyedModel);
                spawnedBroken = true;
            //}
        }

        model.transform.position = this.gameObject.transform.position;
    }

    [Rpc(SendTo.Server, InvokePermission = RpcInvokePermission.Everyone)]
    public void toggleFixedRpc()
    {
        is_Destroyed.Value = false;
    }

    public void toggleFixed()
    {
        this.gameObject.GetComponentInChildren<MeshRenderer>().enabled = true;
        this.gameObject.GetComponent<BoxCollider>().enabled = true;
        this.gameObject.GetComponent<Rigidbody>().useGravity = true;
        Trigger.enabled = true;
        spawnedBroken = false;
        Destroy(model);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6 || other.gameObject.tag == "Crate")
        {
            return;
        }
        HitBoxLogic hbl = other.gameObject.GetComponentInChildren<HitBoxLogic>();
        if (hbl != null)
        {
            if (id != hbl.id)
            {
       
                toggleDamgedRpc();

                //}

                /*
                if (id == HitBoxController.ePlayer.p2)
                {
                    enemyBar = GameObject.FindGameObjectWithTag("EnemyHealthBar");
                    enemyBar.GetComponent<EnemyHealthBarController>().SetMaxHealth(this.transform.parent.gameObject.GetComponent<PlayerStatsController>().getPHealthMax());
                    enemyBar.GetComponent<EnemyHealthBarController>().SetHealth(this.transform.parent.gameObject.GetComponent<PlayerStatsController>().getPHealth());
                    if (this.transform.parent.gameObject.GetComponent<PlayerStatsController>().getPHealth() <= 0)
                    {
                        this.gameObject.SetActive(false);
                    }
                }*/

            }
        }

    }
}
