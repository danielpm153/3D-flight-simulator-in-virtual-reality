using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
public class AircraftPhysics : MonoBehaviour
{
    const float PREDICTION_TIMESTEP_FRACTION = 0.5f;

    [SerializeField] 
    float thrust = 0;
    [SerializeField] 
    List<AeroSurface> aerodynamicSurfaces = null;

    Rigidbody rb;
    float thrustPercent;
    BiVector3 currentForceAndTorque;

    public void SetThrustPercent(float percent)
    {
        thrustPercent = percent;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private static int _counter = 0;
    private static float runningTime = 0;

    private void FixedUpdate()
    {
        
        float h = transform.position.y/5;
        float wind_modulo = Mathf.Sqrt(40)*GetWindVector(_counter).x;
        Vector3 wind_direction = directionToUnitVector(GetWindVector(_counter).y,GetWindVector(_counter).z);

        runningTime += Time.deltaTime;

        if (runningTime > 10){
            runningTime = 0;
            _counter+=1;
            //Debug.Log(_counter);

        }

        
        Vector3 wind = wind_profile(h,wind_modulo) *  wind_direction;

        //Vector3 wind = wind_profile(h,wind_modulo) * Vector3.left;
        //Vector3 wind = 0 * Vector3.up;


        BiVector3 forceAndTorqueThisFrame = 
            CalculateAerodynamicForces(rb.velocity, rb.angularVelocity, wind, 1.2f, rb.worldCenterOfMass);

        Vector3 velocityPrediction = PredictVelocity(forceAndTorqueThisFrame.p
            + transform.forward * thrust * thrustPercent + Physics.gravity * rb.mass);
        Vector3 angularVelocityPrediction = PredictAngularVelocity(forceAndTorqueThisFrame.q);

        BiVector3 forceAndTorquePrediction = 
            CalculateAerodynamicForces(velocityPrediction, angularVelocityPrediction, wind, 1.2f, rb.worldCenterOfMass);

        currentForceAndTorque = (forceAndTorqueThisFrame + forceAndTorquePrediction) * 0.5f;
        rb.AddForce(currentForceAndTorque.p);
        rb.AddTorque(currentForceAndTorque.q);

        rb.AddForce(transform.forward * thrust * thrustPercent);
    }
    
    private float wind_profile(float altitude, float wind_inf, float d = 8, float altitude0 = 1) {
        float windIntensity = 0;
        
        if (altitude > altitude0) {
            windIntensity = wind_inf* (1.0f - Mathf.Exp(-(altitude - altitude0) / d));
        }
        return windIntensity;
    }
    
    private Vector3 directionToUnitVector(float theta, float phi) {
        float radians = Mathf.Deg2Rad * theta;
        phi = Mathf.Deg2Rad * phi;
        float x = Mathf.Cos(radians);
        float z = Mathf.Sin(radians);
        
        x = Mathf.Cos(radians) * Mathf.Cos(phi);
        z = Mathf.Sin(radians) * Mathf.Cos(phi);
        float y = Mathf.Sin(phi);
        return new Vector3(x, y, z);
    }

    private Vector3 GetWindVector(int currentIndex){
        List<float> _windSpeed = new List<float> { 2.2f, 1.2f, 1.6f, 1.6f, 2.6f, 1.8f, 1.3f, 1.4f, 2.1f, 1.7f, 1.6f, 1.6f, 2.3f, 2.7f, 2.3f, 2.9f, 2.8f, 2.7f, 3.3f, 3.0f, 2.4f, 2.6f, 1.7f, 2.5f };
        List<float> _windDirection = new List<float> { 120f, 106f, 110f, 129f, 113f, 114f, 132f, 143f, 146f, 136f, 87f, 115f, 127f, 156f, 150f, 140f, 129f, 118f, 142f, 149f, 163f, 158f, 132f, 135f };
        List<float> _windElevation = new List<float> { 17f, 37f, 41f, 22f, 3f, 28f, 40f, 2f, 45f, 10f, 18f, 6f, 1f, 13f, 21f, 35f, 39f, 26f, 16f, 11f, 12f, 38f, 42f, 4f };
        
        if (currentIndex >= _windSpeed.Count)
        {
            currentIndex = currentIndex%_windSpeed.Count;
        }

        float speed = _windSpeed[currentIndex];
        float direction = _windDirection[currentIndex];
        float elevation = _windElevation[currentIndex];
        
        return new Vector3(speed, direction, elevation);
    }

    private BiVector3 CalculateAerodynamicForces(Vector3 velocity, Vector3 angularVelocity, Vector3 wind, float airDensity, Vector3 centerOfMass)
    {
        BiVector3 forceAndTorque = new BiVector3();
        foreach (var surface in aerodynamicSurfaces)
        {
            Vector3 relativePosition = surface.transform.position - centerOfMass;
            forceAndTorque += surface.CalculateForces(-velocity + wind
                -Vector3.Cross(angularVelocity,
                relativePosition),
                airDensity, relativePosition);
        }
        return forceAndTorque;
    }

    private Vector3 PredictVelocity(Vector3 force)
    {
        return rb.velocity + Time.fixedDeltaTime * PREDICTION_TIMESTEP_FRACTION * force / rb.mass;
    }

    private Vector3 PredictAngularVelocity(Vector3 torque)
    {
        Quaternion inertiaTensorWorldRotation = rb.rotation * rb.inertiaTensorRotation;
        Vector3 torqueInDiagonalSpace = Quaternion.Inverse(inertiaTensorWorldRotation) * torque;
        Vector3 angularVelocityChangeInDiagonalSpace;
        angularVelocityChangeInDiagonalSpace.x = torqueInDiagonalSpace.x / rb.inertiaTensor.x;
        angularVelocityChangeInDiagonalSpace.y = torqueInDiagonalSpace.y / rb.inertiaTensor.y;
        angularVelocityChangeInDiagonalSpace.z = torqueInDiagonalSpace.z / rb.inertiaTensor.z;

        return rb.angularVelocity + Time.fixedDeltaTime * PREDICTION_TIMESTEP_FRACTION
            * (inertiaTensorWorldRotation * angularVelocityChangeInDiagonalSpace);
    }

    public Vector3 GetForces()
    {
        return currentForceAndTorque.p;
    }

#if UNITY_EDITOR
    // For gizmos drawing.
    public void CalculateCenterOfLift(out Vector3 center, out Vector3 force, Vector3 displayAirVelocity, float displayAirDensity)
    {
        Vector3 com;
        BiVector3 forceAndTorque;
        if (aerodynamicSurfaces == null)
        {
            center = Vector3.zero;
            force = Vector3.zero;
            return;
        }

        if (rb == null)
        {
            com = GetComponent<Rigidbody>().worldCenterOfMass;
            forceAndTorque = CalculateAerodynamicForces(-displayAirVelocity, Vector3.zero, Vector3.zero, displayAirDensity, com);
        }
        else
        {
            com = rb.worldCenterOfMass;
            forceAndTorque = currentForceAndTorque;
        }

        force = forceAndTorque.p;
        center = com + Vector3.Cross(forceAndTorque.p, forceAndTorque.q) / forceAndTorque.p.sqrMagnitude;
    }
#endif
}


