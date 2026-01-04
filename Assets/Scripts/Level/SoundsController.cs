using UnityEngine;
using System.Collections;

public class SoundsController : MonoBehaviour
{
    [Header("Environment")]
    [SerializeField] private AudioSource defaultBackgroundSound;
    [SerializeField] private AudioSource windyBackgroundSound;
    [SerializeField] private float fadeDuration = 1.2f;

    [Header("Footsteps")]
    [SerializeField] private AudioSource walkSource;
    [SerializeField] private AudioSource runSource;
    [SerializeField] private float footstepFadeSpeed = 5f;

    private bool isWindy = false;
    private Coroutine activeFade;
    private float targetWalkVol, targetRunVol;

    private void Start()
    {
        defaultBackgroundSound.volume = 1f;
        windyBackgroundSound.volume = 0f;
        defaultBackgroundSound.Play();
        windyBackgroundSound.Play();

        walkSource.volume = 0f;
        runSource.volume = 0f;
        walkSource.Play();
        runSource.Play();
    }

    private void Update()
    {
        walkSource.volume = Mathf.MoveTowards(walkSource.volume, targetWalkVol, Time.deltaTime * footstepFadeSpeed);
        runSource.volume = Mathf.MoveTowards(runSource.volume, targetRunVol, Time.deltaTime * footstepFadeSpeed);
    }

    public void UpdateFootsteps(bool isMoving, bool isRunning)
    {
        if (!isMoving) { targetWalkVol = 0f; targetRunVol = 0f; }
        else if (isRunning) { targetWalkVol = 0f; targetRunVol = 1f; }
        else { targetWalkVol = 1f; targetRunVol = 0f; }
    }

    public void ChangeEnvironmentAudio()
    {
        isWindy = !isWindy;
        if (activeFade != null) StopCoroutine(activeFade);
        activeFade = StartCoroutine(CrossfadeRoutine());
    }

    private IEnumerator CrossfadeRoutine()
    {
        float timer = 0;
        float sDef = defaultBackgroundSound.volume;
        float sWin = windyBackgroundSound.volume;
        float tDef = isWindy ? 0f : 1f;
        float tWin = isWindy ? 1f : 0f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            defaultBackgroundSound.volume = Mathf.Lerp(sDef, tDef, timer / fadeDuration);
            windyBackgroundSound.volume = Mathf.Lerp(sWin, tWin, timer / fadeDuration);
            yield return null;
        }
    }
}