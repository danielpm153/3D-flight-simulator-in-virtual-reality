using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCameraRig : MonoBehaviour
{
    public float velocidade = 30f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetAxis("Forward") > 0.5f)
        {
            Vector3 forwardCamera = transform.GetChild(0).transform.forward;
            float xDirection = Vector3.Dot(Vector3.right, forwardCamera);
            float yDirection = Vector3.Dot(Vector3.forward, forwardCamera);
            Vector3 dir = new Vector3(xDirection, 0, yDirection);
            transform.position += dir * velocidade * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.DownArrow) || Input.GetAxis("Forward") < -0.5f)
        {
            Vector3 forwardCamera = transform.GetChild(0).transform.forward;
            float xDirection = Vector3.Dot(Vector3.right, forwardCamera);
            float yDirection = Vector3.Dot(Vector3.forward, forwardCamera);
            Vector3 dir = new Vector3(xDirection, 0, yDirection);
            transform.position -= dir * velocidade * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetAxis("Side") > 0.5f)
        {
            Vector3 forwardCamera = transform.GetChild(0).transform.right;
            float xDirection = Vector3.Dot(Vector3.right, forwardCamera);
            float yDirection = Vector3.Dot(Vector3.forward, forwardCamera);
            Vector3 dir = new Vector3(xDirection, 0, yDirection);
            transform.position -= dir * velocidade * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.RightArrow) || Input.GetAxis("Side") < -0.5f)
        {
            Vector3 forwardCamera = transform.GetChild(0).transform.right;
            float xDirection = Vector3.Dot(Vector3.right, forwardCamera);
            float yDirection = Vector3.Dot(Vector3.forward, forwardCamera);
            Vector3 dir = new Vector3(xDirection, 0, yDirection);
            transform.position += dir * velocidade * Time.deltaTime;
        }
    }
}
