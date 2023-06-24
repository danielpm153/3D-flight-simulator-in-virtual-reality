using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reset : MonoBehaviour
{
    private Vector3 initialPos;
    private Quaternion initialRot;
    
    private void Awake()
    {
        initialPos = transform.position;
        initialRot = transform.rotation;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Joystick1Button11) || Input.GetKeyDown(KeyCode.R))
        {
            ResetArcraft();
        }
    }

    public void ResetArcraft()
    {
        gameObject.GetComponent<AirplaneController>().ResetThrustPercent();
        gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        transform.position = initialPos;
        transform.rotation = initialRot;
        gameObject.GetComponent<SaveData>().ResetWriter();
    }

    public void Teste()
    {
        
    }
}
