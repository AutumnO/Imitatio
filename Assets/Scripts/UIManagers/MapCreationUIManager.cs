using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MapCreationUIManager : MonoBehaviour
{
    [SerializeField] private SceneLoader _sceneLoader;
    [SerializeField] private Camera _camera;
    
    private Vector3 _camPosition;
    private float _camSize;
    private float _panSpeed;
    private float _zoomSpeed;
    private Vector2 _panDirection;
    private bool _isPanning;
    private bool _isPanningButton;
    private bool _zoomEnabled;

    private PlayerInput _playerInput;

    // store controls
    private InputAction _panAction;
    private InputAction _zoomAction;

    private void Awake()
    {
        // set initial values
        _camPosition = new Vector3(0, 0, 0);
        _camSize = 5.0f;
        _panSpeed = 0.4f;
        _zoomSpeed = 0.75f;
        _panDirection = new Vector2(0, 0);
        _isPanning = false;
        _isPanningButton = false;
        _zoomEnabled = true;

        // set up input controls
        _playerInput = GetComponent<PlayerInput>();
        _panAction = _playerInput.actions["Pan"];
        _zoomAction = _playerInput.actions["Zoom"];
    }

    private void FixedUpdate()
    {
        if (_isPanning || _isPanningButton)
        {
            if (_isPanning)
                _panDirection = _panAction.ReadValue<Vector2>();
            if (_panDirection.x == 1)
                ChangeCameraPosition('E');
            else if (_panDirection.x == -1)
                ChangeCameraPosition('W');

            if (_panDirection.y == 1)
                ChangeCameraPosition('N');
            else if (_panDirection.y == -1)
                ChangeCameraPosition('S');
        }
    }


    // GENERAL BUTTONS

    public void OnClickBack()
    {
        StartCoroutine(_sceneLoader.LoadSceneAsync(1));
    }
    public void OnClickSaveExit()
    {
        StartCoroutine(_sceneLoader.LoadSceneAsync(1));
    }


    // Helper Functions
    public void ZoomEnabler(bool enable)
    {
        _zoomEnabled = enable;
    }
    private void ChangeCameraZoom(int direction)
    {
        _camSize = _camera.orthographicSize;

        if (direction == -1 && _camSize > 1)
            _camSize -= _zoomSpeed;
        else if (direction == 1 && _camSize < 40)
            _camSize += _zoomSpeed;

        _camera.orthographicSize = _camSize;
    }
    private void ChangeCameraPosition(char direction)
    {
        // TODO: add limit to panning so one doesn't get too far away from the grid
        _camPosition = _camera.transform.position;

        switch (direction)
        {
            case 'N':
                _camPosition.y += _panSpeed;
                break;
            case 'E':
                _camPosition.x += _panSpeed;
                break;
            case 'S':
                _camPosition.y -= _panSpeed;
                break;
            case 'W':
                _camPosition.x -= _panSpeed;
                break;
            default:
                break;
        }

        _camera.transform.position = _camPosition;
    }

    // Hotkey Input
    public void Zoom(InputAction.CallbackContext context)
    {
        if (context.performed && _zoomEnabled)
        {
            float zoom = _zoomAction.ReadValue<float>();
            if (zoom > 0)
                ChangeCameraZoom(-1);
            else if (zoom < 0)
                ChangeCameraZoom(1);
        }
    }
    public void Pan(InputAction.CallbackContext context)
    {
        if (context.action.inProgress)
            _isPanning = true;
        else
            _isPanning = false;
    }

    public void HandTool(InputAction.CallbackContext context)
    {
        if (context.performed)
            OnClickToolsHand();
    }
    public void HammerTool(InputAction.CallbackContext context)
    {
        if (context.performed)
            OnClickToolsHammer();
    }
    public void LightingTool(InputAction.CallbackContext context)
    {
        if (context.performed)
            OnClickToolsLighting();
    }
    public void CopyTool(InputAction.CallbackContext context)
    {
        if (context.performed)
            OnClickToolsCopy();
    }
    public void UndoTool(InputAction.CallbackContext context)
    {
        if (context.performed)
            OnClickToolsUndo();
    }
    public void RedoTool(InputAction.CallbackContext context)
    {
        if (context.performed)
            OnClickToolsRedo();
    }


    // Compass
    public void OnClickCompassZoomIn()
    {
        ChangeCameraZoom(-1);
    }
    public void OnClickCompassZoomOut()
    {
        ChangeCameraZoom(1);
    }
    public void OnClickCompassNorth(bool buttonHeld)
    {
        if (buttonHeld)
        {
            _isPanningButton = true;
            _panDirection.Set(0, 1);
        }
        else
        {
            _isPanningButton = false;
            _panDirection.Set(0, 0);
        }
    }
    public void OnClickCompassEast(bool buttonHeld)
    {
        if (buttonHeld)
        {
            _isPanningButton = true;
            _panDirection.Set(1, 0);
        }
        else
        {
            _isPanningButton = false;
            _panDirection.Set(0, 0);
        }
    }
    public void OnClickCompassSouth(bool buttonHeld)
    {
        if (buttonHeld)
        {
            _isPanningButton = true;
            _panDirection.Set(0, -1);
        }
        else
        {
            _isPanningButton = false;
            _panDirection.Set(0, 0);
        }
    }
    public void OnClickCompassWest(bool buttonHeld)
    {
        if (buttonHeld)
        {
            _isPanningButton = true;
            _panDirection.Set(-1, 0);
        }
        else
        {
            _isPanningButton = false;
            _panDirection.Set(0, 0);
        }
    }

    // Tools
    public void OnClickToolsHand()
    {
        Debug.Log("Hand");
    }
    public void OnClickToolsHammer()
    {
        Debug.Log("Hammer");
    }
    public void OnClickToolsLighting()
    {
        Debug.Log("Lighting");
    }
    public void OnClickToolsCopy()
    {
        Debug.Log("Copy");
    }
    public void OnClickToolsUndo()
    {
        Debug.Log("Undo");
    }
    public void OnClickToolsRedo()
    {
        Debug.Log("Redo");
    }
}
