using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Nicolas Chatziargiriou
public class WizardController : MonoBehaviour
{
    [SerializeField] private List<GameObject> _spawns;
    [SerializeField] private List<GameObject> _fireBallSpawners;
    [SerializeField] private GameObject _ammo;

    [SerializeField] private float _teleportTime; //time to teleport after shooting Fireballs
    [SerializeField] private float _teleportTimer;
    [SerializeField] private float _shrinkRatio;
    private float _shrinkTime;
    private float _shrinkTimer=0;
    // Start is called before the first frame update
    void Start()
    {
  
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.LookAt(GameObject.FindGameObjectWithTag("Player").transform.position);
        _teleportTimer = _teleportTimer+Time.deltaTime;
        if (_teleportTimer >=_teleportTime)
        {
            
            Teleport();
        }
    }

    private void shrink()
    {
        _shrinkTime = this.gameObject.GetComponent<AudioSource>().clip.length;
        
       
    
             this.gameObject.transform.localScale= new Vector3(0.5f-_shrinkTimer*_shrinkRatio, 0.5f - _shrinkTimer*_shrinkRatio, 0.5f - _shrinkTimer*_shrinkRatio);
            if(this.gameObject.transform.localScale.x<0)
        {
            this.gameObject.transform.localScale = new Vector3(0f,0f,0f);
        }
            //this.gameObject.transform.localScale = new Vector3(1,1,1);
            _shrinkTimer = _shrinkTimer + Time.deltaTime;
        
    }
    private void shootFireball()
    {
        foreach (GameObject fireball in _fireBallSpawners)
        {
           
           Instantiate(_ammo, new Vector3(fireball.transform.position.x, fireball.transform.position.y,fireball.transform.position.z), Quaternion.LookRotation(fireball.transform.forward));
        }
    }
    private void Teleport()
    {
        int spawnLocation = 0;
        spawnLocation = Random.Range(0, 7);
        this.gameObject.GetComponent<AudioSource>().Play();
        shrink();
        if (_shrinkTimer > _shrinkTime)
        {
            this.transform.position = _spawns[spawnLocation].gameObject.transform.position;
             this.gameObject.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            this.transform.LookAt(GameObject.FindGameObjectWithTag("Player").transform.position);
            _teleportTimer = 0;
            _shrinkTimer = 0;
            shootFireball();
        }
      
    }
}
