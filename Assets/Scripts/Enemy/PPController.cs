using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PPController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject enemy;
    [SerializeField] private Volume volume;

    [Header("Distance Settings")]
    [SerializeField] private float maxDistance = 10f;
    [SerializeField] private float minDistance = 2f;

    [Header("Smooth Settings")]
    [SerializeField] private float lerpSpeed = 5f; // Geçiş hızı (Yüksek değer = daha hızlı)

    private Vignette vignette;
    private ChromaticAberration chromatic;

    private float _defaultVignette;
    private float _defaultChromatic;
    
    private float _targetIntensity = 0f;
    private float _currentIntensity = 0f;
    private bool _canUpdate = true;

    private void Awake()
    {
        if (volume != null && volume.profile != null)
        {
            volume.profile.TryGet(out vignette);
            volume.profile.TryGet(out chromatic);

            _defaultVignette = vignette.intensity.value;
            _defaultChromatic = chromatic.intensity.value;
        }
    }

    private void Update()
    {
        // 1. Hedef Yoğunluğu Belirle
        if (_canUpdate && enemy != null && enemy.activeSelf)
        {
            float distance = Vector3.Distance(player.transform.position, enemy.transform.position);
            // InverseLerp ile mesafeyi 0-1 arasına normalize et
            _targetIntensity = Mathf.InverseLerp(maxDistance, minDistance, distance);
        }
        else
        {
            _targetIntensity = 0f; // Update kapalıysa veya düşman yoksa hedef 0
        }

        // 2. Yumuşak Geçiş (Lerp) Uygula
        _currentIntensity = Mathf.Lerp(_currentIntensity, _targetIntensity, Time.deltaTime * lerpSpeed);

        // 3. Değerleri Post-Process'e Yaz
        ApplyEffectValues(_currentIntensity);
    }

    private void ApplyEffectValues(float intensity)
    {
        if (vignette != null)
            vignette.intensity.value = Mathf.Clamp(intensity * 0.45f, _defaultVignette, 0.45f);

        if (chromatic != null)
            chromatic.intensity.value = Mathf.Clamp(intensity, _defaultChromatic, 1f);
    }

    public void SetUpdateStatus(bool status)
    {
        _canUpdate = status;
        if (!status) 
        {
            _targetIntensity = 0f;
            // Not: _currentIntensity'i sıfırlamıyoruz ki geçiş hala yumuşak olsun
        }
    }

    public void ResetPPAmount()
    {
        _targetIntensity = 0f;
        _currentIntensity = 0f;
        ApplyEffectValues(0f);
    }
}