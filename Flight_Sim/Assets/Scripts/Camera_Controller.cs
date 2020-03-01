using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//[ExecuteInEditMode]
public class Camera_Controller : MonoBehaviour
{
    #region Variables
    [Space]
    [Header("Object Refference :-")]
    [SerializeField] private Transform _aircraft = null;
    [SerializeField] private Transform _mainCamera = null;
    [SerializeField] private Camera _playerCam = null;
    [SerializeField] private Transform _boreAim = null;
    [SerializeField] private Transform _camAim = null;
    [Space]
    [Header("Text Spectre :-")]
    [SerializeField] private Text _initialAcceleration = null;
    [SerializeField] private Text _currentAcceleration = null;
    [SerializeField] private Text _currentRotation = null;
    [Space]
    [Header("Accelerometer Tweaks :-")]
    [SerializeField] [Range(0, 1f)] private float _smooth = 0.4f;
    [SerializeField] [Range(0, 100f)] private float _sensitivity = 6f;
    [Space]
    [Header("Game HUD :-")]
    [SerializeField] private RectTransform _boresight = null;
    [Space]
    [Header("Camera Tweaks :-")]
    [SerializeField] [Range(0, 1f)] private float _rotationSlerp = 0.5f;

    private Vector3 _currentAccln, _initialAccln;
    private Vector3 _newRotation;
    #endregion

    #region BuildIn Methods
    private void Start()
    {
        _initialAccln = Input.acceleration;
        _currentAccln = Vector3.zero;

        initialAcclnDisplay();
    }

    private void Update()
    {
        camFollow();
        onGameGUI();
        cameraRotation();
        updateGraphic();
    }

    private void FixedUpdate()
    {
        aircraftRotation();
    }
    #endregion

    #region Custom Methods
    public void resetAcceleration()
    {
        _initialAccln = Input.acceleration;
        _currentAccln = Vector3.zero;

        initialAcclnDisplay();
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
        Vector3 directionToFace = _camAim.position - _aircraft.position;

        Debug.DrawRay(_aircraft.transform.position, directionToFace, Color.red);

        Quaternion planeRotation = Quaternion.LookRotation(directionToFace);
        _aircraft.rotation = Quaternion.Slerp(_aircraft.rotation, planeRotation, Time.deltaTime * _rotationSlerp);
    }

    private void onGameGUI()
    {
        _currentAcceleration.text = "Current Acceleraion :-\n"
            + _currentAccln.x.ToString("#.##") + "\n"
            + _currentAccln.y.ToString("#.##") + "\n"
            + _currentAccln.z.ToString("#.##");

        _currentRotation.text = "Current Rotation :-\n"
            + transform.localEulerAngles.x.ToString("#.##")
            + "\n" + transform.localEulerAngles.y.ToString("#.##")
            + "\n" + transform.localEulerAngles.z.ToString("#.##");
    }

    private void cameraRotation()
    {
        _currentAccln = Vector3.Lerp(_currentAccln, Input.acceleration - _initialAccln, Time.deltaTime / _smooth);

        _newRotation.x = Mathf.Clamp(_currentAccln.x * _sensitivity, -1f, 1f);
        _newRotation.z = Mathf.Clamp(_currentAccln.z * _sensitivity, -1f, 1f);

        _mainCamera.Rotate(-_newRotation.z, 0f, 0f, Space.Self);
        transform.Rotate(0f, _newRotation.x, 0f, Space.Self);
    }

    private void initialAcclnDisplay()
    {
        _initialAcceleration.text = "Initial Acceleraion :-\n"
            + _initialAccln.x.ToString("#.##") + "\n"
            + _initialAccln.y.ToString("#.##") + "\n"
            + _initialAccln.z.ToString("#.##");
    }
    #endregion
}
