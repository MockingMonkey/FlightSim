using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flight_Controller : MonoBehaviour
{
    #region Variables
    [Space]
    [Header("Object Refference :-")]
    [SerializeField] private Transform _originalAircraft = null;
    [Space]
    [Header("Flight Controls :-")]
    [SerializeField] private AnimationCurve _aircraftBanking = AnimationCurve.Linear(0f, 0f, 1f, 30f);
    [SerializeField] private float _offAngle = 30f;
    #endregion

    #region BuildIn Methods
    void Start()
    {
        
    }

    void FixedUpdate()
    {
        realisticBanking();
    }
    #endregion

    #region Custom Methods
    private void realisticBanking()
    {
        float rotationAngle = _offAngle * -Input.acceleration.x;
        float bodyRotation = _originalAircraft.rotation.z + rotationAngle;
        Quaternion newRotation = Quaternion.Euler(0f, 0f, bodyRotation);
        _originalAircraft.rotation = Quaternion.Slerp(_originalAircraft.rotation, transform.rotation * newRotation, Time.deltaTime * 10f);
    }
    #endregion
}
