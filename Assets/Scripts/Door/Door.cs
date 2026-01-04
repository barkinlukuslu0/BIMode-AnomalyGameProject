using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Door : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] public bool IsOpen = true;
    [SerializeField] private bool IsRotatingDoor = true;
    [SerializeField] private float Speed = 1f;

    [Header("RotationAmount")]
    [SerializeField] private float RotationAmount = 90f;
    [SerializeField] private float ForwardDirection = 0f;

    [Header("Audio Settings")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip doorSound;

    private Vector3 StartRotation;
    private Vector3 Forward;

    private Coroutine AnimationCoroutine;

    private void Awake()
    {
        StartRotation = transform.rotation.eulerAngles;
        Forward = transform.right;

        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }

    public void Open(Vector3 UserPosition)
    {
        if(!IsOpen)
        {
            if(AnimationCoroutine != null)
            {
                StopCoroutine(AnimationCoroutine);
            }

            PlaySound(doorSound);

            if(IsRotatingDoor)
            {
                float dot = Vector3.Dot(Forward, (UserPosition - transform.position).normalized);
                AnimationCoroutine = StartCoroutine(DoRotationOpen(dot));
            }
        }
    }

    private IEnumerator DoRotationOpen(float ForwardAmount)
    {
        Quaternion startRotation = transform.rotation;
        Quaternion endRotation;

        if(ForwardAmount >= ForwardDirection)
        {
            endRotation = Quaternion.Euler(new Vector3(0, StartRotation.y - RotationAmount, 0));
        }
        else
        {
            endRotation = Quaternion.Euler(new Vector3(0, StartRotation.y + RotationAmount, 0));
        }

        IsOpen = true;

        float time = 0;
        while(time < 1)
        {
            transform.rotation = Quaternion.Slerp(startRotation, endRotation, time);
            yield return null;
            time += Time.deltaTime * Speed;
        }
    }

    public void Close()
    {
        if(IsOpen)
        {
            if(AnimationCoroutine != null)
            {
                StopCoroutine(AnimationCoroutine);   
            }

            PlaySound(doorSound);

            if(IsRotatingDoor)
            {
                AnimationCoroutine = StartCoroutine(DoRotationClose());
            }
        }
    }

    private IEnumerator DoRotationClose()
    {
        Quaternion startRotation = transform.rotation;
        Quaternion endRotation = Quaternion.Euler(StartRotation);

        IsOpen = false;

        float time = 0;
        while(time < 1)
        {
            transform.rotation = Quaternion.Slerp(startRotation, endRotation, time);
            yield return null;
            time += Time.deltaTime * Speed;
        }
    }

    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {         
            audioSource.PlayOneShot(clip);
        }
    }
}