using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverController : MonoBehaviour
{
    [SerializeField] private Transform _OnPos;
    [SerializeField] private float _Speed;
    [SerializeField] private GameObject _lGate;
    [SerializeField] private Transform _lGateOP;
    [SerializeField] private GameObject _rGate;
    [SerializeField] private Transform _rGateOP;
    [SerializeField] private float _GateSpeed;

    private bool isOpen = false;
   [SerializeField] private bool interActed = false;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {

        if (interActed)
        {
            rotate();
        }
    }

    public void rotate()
    {
        if (!isOpen)
        {
            this.transform.rotation = Quaternion.Slerp(transform.rotation, _OnPos.rotation, _Speed * Time.deltaTime);
            _lGate.transform.position = Vector3.Lerp(_lGate.transform.position, _lGateOP.position, _GateSpeed * Time.deltaTime);
            _rGate.transform.position = Vector3.Lerp(_rGate.transform.position, _rGateOP.position, _GateSpeed * Time.deltaTime);
        }
      

        if (this.transform.rotation == _OnPos.rotation)
        {
            isOpen = true;
            if (this.transform.position != _lGateOP.transform.position)
            moveGate();
        }
    }

    public void moveGate()
    {
        
    }

    public void open()
    {
        interActed = true;
    }
}
