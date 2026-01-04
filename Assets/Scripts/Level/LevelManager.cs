using System.Collections;
using TMPro;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Header("UI and References")]
    [SerializeField] private TextMeshPro levelNumberText;
    [SerializeField] private AnomalyController anomalyController;
    [SerializeField] private LightsController lightsController;
    [SerializeField] private PPController ppController;

    [Header("Settings")]
    [SerializeField] private float darkDuration = 1.5f;

    private int _currentLevelNumber = 0;
    private bool _isChangingLevel = false;

    private void Start()
    {
        ppController.ResetPPAmount();
        _currentLevelNumber = 0;
        UpdateLevelText();
    }

    public void NextLevel()
    {
        if (!_isChangingLevel) StartCoroutine(ChangeLevelState(true));
    }

    public void RestartGame()
    {
        if (!_isChangingLevel) StartCoroutine(ChangeLevelState(false));
    }

    private IEnumerator ChangeLevelState(bool isNextLevel)
    {
        _isChangingLevel = true;
        
        ppController.SetUpdateStatus(false);
        lightsController.TurnOffLights();
        
        yield return new WaitForSeconds(darkDuration);

        if (isNextLevel)
        {
            if (_currentLevelNumber >= 8)
            {
                Debug.Log("You WIN!");
            }
            else
            {
                _currentLevelNumber++;
            }
        }
        else
        {
            _currentLevelNumber = 0;
        }

        UpdateLevelText();

        anomalyController.ResetAllAnomalies();
        anomalyController.SetAnomalies();
    
        lightsController.StopFlicker(); 

        _isChangingLevel = false;
        ppController.SetUpdateStatus(true);
    }

    private void UpdateLevelText()
    {
        if (levelNumberText != null)
            levelNumberText.text = _currentLevelNumber.ToString();
    }
}