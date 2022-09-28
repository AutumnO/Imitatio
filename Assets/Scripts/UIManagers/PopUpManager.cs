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

    public GameObject ActivatePopUp(string popUpType, string popUpText)
    {
        // ToDo: fade background
        if (popUpType == "Confirmation")
            _activePopUp = Instantiate(_confirmationPopUp, new Vector3(0, 0, 0), Quaternion.identity, _canvas);
        else if (popUpType == "Input")
            _activePopUp = Instantiate(_inputPopUp, _canvas);

        // add description text for pop-up
        TMP_Text promptText = _activePopUp.GetComponentInChildren<TMP_Text>();
        promptText.text = popUpText;

        // add onclick listener for cancel button
        Button button = _activePopUp.GetComponentInChildren<Button>();
        button.onClick.AddListener(ClosePopUp);
        return _activePopUp;
    }
    public void ClosePopUp()
    {
        Destroy(_activePopUp);
    }
}
