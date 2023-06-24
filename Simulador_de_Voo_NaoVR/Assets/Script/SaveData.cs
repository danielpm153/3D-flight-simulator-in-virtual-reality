using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveData : MonoBehaviour
{
    private AirplaneController airplaneController;

    private StreamWriter writer;
    private Rigidbody rb;
    private string path ;//"Assets/DadosDeVoo/test.csv";

    private string dataNames = "tempo (s); pos_x (m); pos_y (m); altitude (m); velocidade (m/s); fator_carga; profundor [%]; leme [%]; aileron [%];";

    private void Awake()
    {
        path = Application.dataPath + "/dados.csv";
        rb = gameObject.GetComponent<Rigidbody>();

        airplaneController = gameObject.GetComponent<AirplaneController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (File.Exists(path))
        {
            File.Delete(path);
        }
        
        writer = System.IO.File.CreateText(path);
        writer.WriteLine(dataNames);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        float weight = rb.mass * Physics.gravity.magnitude;

        if(writer != null)
        {
            string data = "";
            data += Time.time.ToString() + ";";
            data += (transform.position.z / 5.0).ToString() + ";";
            data += (transform.position.x / 5.0).ToString() + ";";
            data += (transform.position.y / 5.0).ToString() + ";";
            
            data += (rb.velocity.magnitude / 5.0).ToString() + ";";
            
            Vector3 forces = gameObject.GetComponent<AircraftPhysics>().GetForces();

            data += (transform.InverseTransformDirection(forces).y /(2.5f * weight)).ToString() + ";";

            data += (airplaneController.Pitch).ToString() + ";";
            data += (airplaneController.Yaw).ToString() + ";";
            data += (airplaneController.Roll).ToString() + ";";

            //data += (forces.y / weight).ToString() + ";";

            writer.WriteLine(data);
        }

        // Debug.Log(airplaneController.Pitch);
    }

    public void CloseAndSaveFile()
    {
        if(writer != null)
        {
            writer.Close();
            writer = null;
        }
    }

    public void ResetWriter()
    {
        if(writer != null)
        {
            writer.Close();
        }

        if (File.Exists(path))
        {
            File.Delete(path);
        }

        writer = File.CreateText(path);
        writer.WriteLine(dataNames);
    }
}
