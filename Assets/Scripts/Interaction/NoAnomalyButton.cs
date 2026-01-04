using System.Collections;
using UnityEngine;

public class NoAnomalyButton : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private AnomalyController anomalyController;
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip soundEffect;

    [Header("Settings")]
    [SerializeField] private float actionCoolDown;

    private void Awake()
    {
        anomalyController = FindAnyObjectByType<AnomalyController>();
        levelManager = FindAnyObjectByType<LevelManager>();
    }

    public void ButtonPressed()
    {
        StartCoroutine(StartAction());
    }

    private IEnumerator StartAction()
    {
        audioSource.PlayOneShot(soundEffect);

        yield return new WaitForSeconds(actionCoolDown);

        if(!anomalyController.IsThereAnomaly())
        {
            levelManager.NextLevel();
        }
        else
        {
            levelManager.RestartGame();
        }
    }
}
