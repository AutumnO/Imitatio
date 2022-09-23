using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Transform _camera;

    private Vector3 _camPosition;
    
    // MAP CREATION
    
    // TODO: add limit to panning/zooming so one doesn't get too far away or close to grid

    private void ChangeCameraPosition(char direction, int moveAmount)
    {
        _camPosition = _camera.transform.position;

        switch (direction)
        {
            case 'N':
                _camPosition.y += moveAmount;
                break;
            case 'E':
                _camPosition.x += moveAmount;
                break;
            case 'S':
                _camPosition.y -= moveAmount;
                break;
            case 'W':
                _camPosition.x -= moveAmount;
                break;
            default:
                break;
        }

        _camera.transform.position = _camPosition;
    }

    public void OnClickBack()
    {
        Debug.Log("Back Clicked");
    }

    public void OnClickSaveExit()
    {
        Debug.Log("Save & Exit Clicked");
    }

    // Compass
    public void OnClickCompassZoomIn()
    {
        Debug.Log("Zoom In");
    }
    public void OnClickCompassZoomOut()
    {
        Debug.Log("Zoom Out");
    }
    public void OnClickCompassNorth()
    {
        ChangeCameraPosition('N', 1);
    }
    public void OnClickCompassEast()
    {
        ChangeCameraPosition('E', 1);
    }
    public void OnClickCompassSouth()
    {
        ChangeCameraPosition('S', 1);
    }
    public void OnClickCompassWest()
    {
        ChangeCameraPosition('W', 1);
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
