using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCube : MonoBehaviour
{
    private bool podeMover = false;

    // Start is called before the first frame update
    void Start()
    {
        podeMover = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (podeMover)
        {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            transform.position += new Vector3(h * 0.1f, v * 0.1f, 0);
        }
    }

    public void ToggleMove()
    {
        podeMover = !podeMover;
    }
    
    public void SetMove()
    {
        podeMover = true;
    }

    public void UnsetMove()
    {
        podeMover = false;
    }
}
