using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Cinemachine;

//Nicolas Chatziargirou
public class TargetSelector : MonoBehaviour
{
    [SerializeField]
    GameObject marker;
    public PlayerInPutActions playerControls;
    private InputAction Target;
    private InputAction Cycle;
    private List<GameObject> Targets;
   [SerializeField]
   private CinemachineFreeLook characterCamera;
    [SerializeField]
    private GameObject playerObject;
    private int targetTracker=0;
    private GameObject selectedTarget;
    private bool targeting =false;
    // Start is called before the first frame update

    private void OnEnable()
    {


        Target = playerControls.Player.Target;
        Target.Enable();
        Target.performed += TargetPerformed;

        Cycle = playerControls.Player.CycleTarget;
        Cycle.Enable();
        Cycle.performed += CycleTarget;
    }
    private void OnDisable()
    {
        Target.Disable();
        Cycle.Disable();
    }
    private void Awake()
    {
        playerControls = new PlayerInPutActions();
    }
    private void Update()
    {
        if (targeting)
        {
            playerObject.transform.LookAt(new Vector3(selectedTarget.transform.position.x,playerObject.transform.position.y,selectedTarget.transform.position.z));
        }
    }

    public void disableTargeting()
    {
        if (targeting==true)
        {
            selectedTarget.GetComponent<disableOnAwake>().getMarker().gameObject.SetActive(false);
            targeting = false;
            characterCamera.LookAt = playerObject.transform;
        }
       
    }
    private void TargetPerformed(InputAction.CallbackContext context)
    {
        if (targeting==false)
        {
            getTarget();
            
        }
        else if (targeting ==true)
        {
            selectedTarget.GetComponent<disableOnAwake>().getMarker().gameObject.SetActive(false);
            targeting = false;
            characterCamera.LookAt = playerObject.transform;
        }
      
       

    }
    private void CycleTarget(InputAction.CallbackContext context)
    {
        if (this.gameObject.GetComponent<TargetList>().getTargetList() != null)
        {
            Targets = this.gameObject.GetComponent<TargetList>().getTargetList();

            if (targeting == true && Targets != null)
            {
                if (Targets.Count - 1 > targetTracker)
                {
                    selectedTarget.transform.Find("Marker").gameObject.SetActive(false);
                    targetTracker++;
                    selectedTarget = Targets[targetTracker].transform.Find("BigZombie").gameObject;
                    selectedTarget.transform.Find("Marker").gameObject.SetActive(true);
                    characterCamera.LookAt = selectedTarget.transform;
                    

                }
                else
                {
                    selectedTarget.transform.Find("Marker").gameObject.SetActive(false);
                    targetTracker = 0;
                    if (Targets.Count > 0)
                    {
                        selectedTarget = Targets[targetTracker].transform.Find("BigZombie").gameObject;
                        selectedTarget.transform.Find("Marker").gameObject.SetActive(true);
                        characterCamera.LookAt = selectedTarget.transform;
                    }
                    else
                    {
                        targeting = false;
                        characterCamera.LookAt = playerObject.transform;
                    }
                }
            }
        }
    }


    private void  getTarget()
    {
        if (this.gameObject.GetComponent<TargetList>().getTargetList()!=null)
        {
            Targets = this.gameObject.GetComponent<TargetList>().getTargetList();
        }
        
        if (Targets.Count > 0)
        {
            targetTracker = 0;
            if (Targets[targetTracker].transform.Find("BigZombie").gameObject)
            {
                selectedTarget = Targets[targetTracker].transform.Find("BigZombie").gameObject;
                targeting = true;
                Debug.Log("target: " + selectedTarget.gameObject);
                Debug.Log("marker: " + selectedTarget.gameObject.GetComponent<disableOnAwake>().getMarker().gameObject);
                selectedTarget.gameObject.GetComponent<disableOnAwake>().getMarker().gameObject.SetActive(true);
                characterCamera.LookAt = selectedTarget.gameObject.GetComponent<disableOnAwake>().getMarker().gameObject.transform.parent.transform;
                // selectedTarget.gameObject.transform.Find("Marker").gameObject.SetActive(true);
            }



        }
       // else targeting = false;
    }
}
