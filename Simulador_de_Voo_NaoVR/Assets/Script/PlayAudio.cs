using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudio : MonoBehaviour
{
    public AudioClip motorLigando;
    public AudioClip motorLigado;
    public AudioClip motorDesligando;

    private AudioSource audioSource;

    public bool thrustPercent = false;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Joystick1Button1))
        {
            thrustPercent = thrustPercent == true ? false : true;

            if(thrustPercent == true)
            {
                MotorLigando();
            }

            if(thrustPercent == false)
            {
                MotorDesligando();
            }
        }

        if (Input.GetKeyDown(KeyCode.Joystick1Button11) || Input.GetKeyDown(KeyCode.R))
        {
            thrustPercent = false;
            MotorDesligando();
        }
    }

    void MotorLigando()
    {
        audioSource.PlayOneShot(motorLigando);

        audioSource.clip = motorLigado;
        audioSource.loop = true;
        audioSource.PlayDelayed(3.0f);
    }

    public void MotorDesligando()
    {
        audioSource.loop = false;
        audioSource.Stop();
        audioSource.PlayOneShot(motorDesligando);
    }
}
