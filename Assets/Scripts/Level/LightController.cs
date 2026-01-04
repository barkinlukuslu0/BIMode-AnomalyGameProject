using UnityEngine;
using System.Collections;

public class LightsController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Light[] lights;
    [SerializeField] private AudioSource audioSource;

    [Header("Audio Settings")]
    [SerializeField] private AudioClip lightTurnOnSound;
    [SerializeField] private AudioClip lightTurnOffSound; 
    [SerializeField] private AudioClip flickeringLoopSound;

    [Header("Settings")]
    [SerializeField] private float _defaultIntensity = 5f;
    [SerializeField] private float _minIntensity = 0.5f;
    [SerializeField] private float _maxIntensity = 5f;
    [SerializeField] private float _flickerSpeed = 0.1f;
    [SerializeField] private Color enemyChasingColor;

    [Header("EnemySettings")]
    [SerializeField] private float LightsTurnOffDuration;

    private Coroutine _flickerCoroutine;
    private Color _defaultColor;

    private bool _areLightsOn = true; 
    private bool _isEnemySequenceActive = false;

    private void Awake()
    {
        if (audioSource == null) audioSource = GetComponent<AudioSource>();

        if (lights != null && lights.Length > 0)
        {        
            _defaultIntensity = lights[0].intensity;
            _defaultColor = lights[0].color;
        }

        _areLightsOn = lights.Length > 0 && lights[0].intensity > 0;
    }

    public void TurnOnLights()
    {
        if (_areLightsOn) return;

        PlayOneShotSound(lightTurnOnSound);

        foreach (Light light in lights)
        {
            if (light != null) light.intensity = _defaultIntensity;
        }

        _areLightsOn = true;
    }
    
    public void TurnOffLights()
    {
        if (!_areLightsOn) return;

        StopFlickerSound();
        PlayOneShotSound(lightTurnOffSound);

        foreach (Light light in lights)
        {
            if (light != null) light.intensity = 0;
        }

        _areLightsOn = false;
    }

    private void ForceTurnOnLights()
    {
        foreach (Light light in lights)
        {
            if (light != null) light.intensity = _defaultIntensity;
        }
        _areLightsOn = true;
    }

    public void StartFlicker()
    {
        if (_flickerCoroutine != null) StopCoroutine(_flickerCoroutine);
        
        PlayFlickerLoop();

        _flickerCoroutine = StartCoroutine(FlickerRoutine());
    }

    public void StopFlicker()
    {
        if (_flickerCoroutine != null)
        {
            StopCoroutine(_flickerCoroutine);
            _flickerCoroutine = null;
        }

        StopFlickerSound();

        foreach (Light light in lights)
        {
            if (light != null)
            {
                light.intensity = _defaultIntensity;
                light.color = _defaultColor;
            }
        }
        _areLightsOn = true;
    }

    private IEnumerator FlickerRoutine()
    {
        while (true)
        {
            float randomIntensity = Random.Range(_minIntensity, _maxIntensity);
            
            foreach (Light light in lights)
            {
                if (light != null) light.intensity = randomIntensity;
            }

            yield return new WaitForSeconds(_flickerSpeed);
        }
    }

    public IEnumerator EnemyStartsFollow()
    {
        if (_isEnemySequenceActive) yield break;

        _isEnemySequenceActive = true;
        
        TurnOffLights(); 
        EnemyChasingColor();

        yield return new WaitForSeconds(LightsTurnOffDuration);

        TurnOnLights(); 
        StartFlicker();

        _isEnemySequenceActive = false;
    }

    private void EnemyChasingColor()
    {
        foreach (Light light in lights)
        {
            light.color = enemyChasingColor;
        }
    }

    public void DefaultLightColor()
    {
        foreach (Light light in lights)
        {
            light.color = _defaultColor;
        }
    }

    private void PlayOneShotSound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    private void PlayFlickerLoop()
    {
        if (flickeringLoopSound != null && audioSource != null)
        {
            if (audioSource.isPlaying && audioSource.clip == flickeringLoopSound) return;

            audioSource.clip = flickeringLoopSound;
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    private void StopFlickerSound()
    {
        if (audioSource != null)
        {
            if (audioSource.clip == flickeringLoopSound)
            {
                audioSource.Stop();
                audioSource.clip = null;
                audioSource.loop = false;
            }
        }
    }
}