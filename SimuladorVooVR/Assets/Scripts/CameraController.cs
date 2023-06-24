using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform planePosition;
    private Camera camView;

    private float DISTANCE_THRESHOLD = 101f;

    private float stepFieldOfView = 10f;

    private bool isReseting = false; 

    void Awake()
    {
        camView = GetComponent<Camera>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(planePosition.position, transform.position);

        if(distance > DISTANCE_THRESHOLD && !isReseting)
        {
            camView.fieldOfView = 60 / (Mathf.Pow(distance - 100, 0.2f));
        }

        if (isReseting)
        {
            ResetCamera();
        }

        if(Input.GetKeyDown(KeyCode.Joystick1Button11) || Input.GetKeyDown(KeyCode.R))
        {
            isReseting = true;
        }
        
    }

    void ResetCamera()
    {
        if (camView.fieldOfView < 60)
        {
            camView.fieldOfView += stepFieldOfView * Time.deltaTime;
        }
        else
        {
            isReseting = false;
        }
    }
}
