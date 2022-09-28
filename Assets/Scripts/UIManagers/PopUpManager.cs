using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PopUpManager : MonoBehaviour
{
    [SerializeField] private Transform _canvas;
    [SerializeField] private GameObject _confirmationPopUp;
    [SerializeField] private GameObject _inputPopUp;

    private GameObject _activePopUp;

    public GameObject ActivatePopUp(int popUpNum)
    {
        // ToDo: fade background
        if (popUpNum == 1)
            _activePopUp = Instantiate(_confirmationPopUp, new Vector3(0, 0, 0), Quaternion.identity, _canvas);
        else
            _activePopUp = Instantiate(_inputPopUp, _canvas);

        Button button = GameObject.Find("PopUpCancelButton").GetComponent<Button>();
        button.onClick.AddListener(ClosePopUp);
        return _activePopUp;
    }
    public void ClosePopUp()
    {
        Destroy(_activePopUp);
    }
}
