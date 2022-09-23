using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputControls : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    
    private Vector3 _camPosition;
    private float _camSize;
    private PlayerInput _playerInput;
    private float _panSpeed;
    private float _zoomSpeed;
    private float currentTime;
    private float timeBetweenMovementIncrements;
    private Vector2 _panDirection;
    private bool _isPanning;

    // store controls
    private InputAction _panAction;
    private InputAction _zoomAction;

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _panAction = _playerInput.actions["Pan"];
        _zoomAction = _playerInput.actions["Zoom"];
        _panSpeed = 0.4f;
        _zoomSpeed = 0.75f;
    }


    // MAP CREATION

    // TODO: add limit to panning so one doesn't get too far away from the grid

    public void OnClickBack()
    {
        Debug.Log("Back Clicked");
    }
    public void OnClickSaveExit()
    {
        Debug.Log("Save & Exit Clicked");
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
        if (context.performed == true)
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

    private void FixedUpdate()
    {
        if (_isPanning)
        {
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

    // Compass
    public void OnClickCompassZoomIn()
    {
        ChangeCameraZoom(-1);
    }
    public void OnClickCompassZoomOut()
    {
        ChangeCameraZoom(1);
    }
    public void OnClickCompassNorth()
    {
        ChangeCameraPosition('N');
    }
    public void OnClickCompassEast()
    {
        ChangeCameraPosition('E');
    }
    public void OnClickCompassSouth()
    {
        ChangeCameraPosition('S');
    }
    public void OnClickCompassWest()
    {
        ChangeCameraPosition('W');
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
