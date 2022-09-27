using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private GameObject _loadingBarObject;

    private Slider _loadingBar;
    private TextMeshProUGUI _progressText;

    public IEnumerator LoadSceneAsync(int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        Instantiate(_loadingBarObject, new Vector3(0, 0, 0), Quaternion.identity);
        _loadingBar = _loadingBarObject.GetComponent<Slider>();
        _progressText = _loadingBarObject.GetComponentInChildren<TextMeshProUGUI>();

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            _loadingBar.value = progress;
            _progressText.text = progress * 100f + "%";

            yield return null;
        }
    }
}
