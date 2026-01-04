using UnityEngine;

public class HeadBobController : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] private bool _enable = true;

    [Header("References")]
    [SerializeField] private PlayerController _playerController;

    [Header("Bobbing Settings (Position)")]
    [SerializeField, Range(0, 0.1f)] private float _amplitude = 0.015f; 
    [SerializeField, Range(0, 30f)] private float _frequency = 10f; 

    [Header("Tilt Settings (Rotation)")]
    [SerializeField] private float _tiltAmount = 4f;
    [SerializeField] private float _swayAmount = 2f;
    [SerializeField] private float _smoothness = 10f;

    private Vector3 _startPos;
    private float _defaultYPos;

    private void Awake()
    {    
        _startPos = transform.localPosition;
        _defaultYPos = _startPos.y;
    }

    private void Update()
    {
        if (!_enable) return;

        CheckMotion();
    }

    private void CheckMotion()
    {
        Vector3 velocity = _playerController.GetVelocity();
        float speed = new Vector3(velocity.x, 0, velocity.z).magnitude;
        
        float inputX = Input.GetAxisRaw("Horizontal");

        if (speed < 0.1f) 
        {
            ResetPosition();
            return;
        }

        PlayPositionBob();
        PlayRotationTilt(inputX);
    }

    private void PlayPositionBob()
    {
        Vector3 pos = Vector3.zero;

        pos.y = Mathf.Sin(Time.time * _frequency) * _amplitude;
        pos.x = Mathf.Cos(Time.time * _frequency / 2) * _amplitude * 2;
        
        transform.localPosition = _startPos + pos;
    }

    private void PlayRotationTilt(float inputX)
    {
        float targetTilt = -inputX * _swayAmount;
        float bobTilt = Mathf.Sin(Time.time * _frequency / 2) * _tiltAmount;

        Quaternion targetRotation = Quaternion.Euler(0, 0, targetTilt + bobTilt);

        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, Time.deltaTime * _smoothness);
    }

    private void ResetPosition()
    {
        if (Vector3.Distance(transform.localPosition, _startPos) > 0.001f)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, _startPos, Time.deltaTime * _smoothness);
        }

        if (Quaternion.Angle(transform.localRotation, Quaternion.identity) > 0.1f)
        {
            transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.identity, Time.deltaTime * _smoothness);
        }
    }

    public void ChangeAmountOfBob(float newFrequency, float newAmplitude)
    {
        _frequency = newFrequency;
        _amplitude = newAmplitude;
    }
}