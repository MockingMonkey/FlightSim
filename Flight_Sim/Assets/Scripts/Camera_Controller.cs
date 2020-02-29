using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class Camera_Controller : MonoBehaviour
{
    #region Variables
    [SerializeField] private Transform _aircraft;
    [SerializeField] private Camera _playerCam;
    [SerializeField] private Transform _boreAim;
    [SerializeField] private Transform _camAim;
    [SerializeField] private Text _initialAcceleration;
    [SerializeField] private Text _currentAcceleration;
    [SerializeField] private Text _currentRotation;
    [SerializeField] private RectTransform _boresight;
    [SerializeField] private float _smooth = 0.4f;
    [SerializeField] private float _sensitivity = 6f;
    private Vector3 _currentAccln, _initialAccln;
    private Vector3 _newRotation;
    #endregion

    #region BuildIn Methods
    void Start()
    {
        _initialAccln = Input.acceleration;
        _currentAccln = Vector3.zero;

        _initialAcceleration.text = "Initial Acceleraion :-\n" 
            + _initialAccln.x.ToString("#.##") + "\n" 
            + _initialAccln.y.ToString("#.##") + "\n" 
            + _initialAccln.z.ToString("#.##");
    }

    void Update()
    {
        camFollow();


        _currentAccln = Vector3.Lerp(_currentAccln, Input.acceleration - _initialAccln, Time.deltaTime / _smooth);

        _currentAcceleration.text = "Current Acceleraion :-\n" 
            + _currentAccln.x.ToString("#.##") + "\n" 
            + _currentAccln.y.ToString("#.##") + "\n" 
            + _currentAccln.z.ToString("#.##");

        _newRotation.x = Mathf.Clamp(_currentAccln.x * _sensitivity, -1f, 1f);
        _newRotation.z = Mathf.Clamp(_currentAccln.z * _sensitivity, -1f, 1f);

        transform.Rotate(-_newRotation.z, Mathf.Clamp(_newRotation.x, -10f, 10f), 0f, Space.Self);

        _currentRotation.text = "Current Rotation :-\n" 
            + transform.localEulerAngles.x.ToString("#.##") 
            + "\n" + transform.localEulerAngles.y.ToString("#.##") 
            + "\n" + transform.localEulerAngles.z.ToString("#.##");

        updateGraphic();
        aircraftRotation();
    }
    #endregion

    #region Custom Methods
    public void resetAcceleration()
    {
        _initialAccln = Input.acceleration;
        _currentAccln = Vector3.zero;

        _initialAcceleration.text = "Initial Acceleraion :-\n" 
            + _initialAccln.x.ToString("#.##") + "\n" 
            + _initialAccln.y.ToString("#.##") + "\n" 
            + _initialAccln.z.ToString("#.##");
    }

    private void camFollow()
    {
        transform.position = _aircraft.position;
    }

    private void updateGraphic()
    {
        if (_boresight != null)
        {
            _boresight.position = _playerCam.WorldToScreenPoint(_boreAim.position);
        }
    }

    private void aircraftRotation()
    {
        Vector3 directionToFace = _camAim.position - _boreAim.position;

        Debug.DrawLine(_aircraft.transform.position, directionToFace, Color.red);

        Quaternion planeRotation = Quaternion.LookRotation(directionToFace);
        _aircraft.rotation = Quaternion.Slerp(_aircraft.rotation, planeRotation, Time.deltaTime * 0.5f);
    }
    #endregion
}
