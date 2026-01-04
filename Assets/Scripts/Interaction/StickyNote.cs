using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class StickyNoteController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject stickyNoteOnTable;
    [SerializeField] private TextMeshPro stickyNoteOnTableText;
    [SerializeField] private GameObject stickyNoteOnCamera;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip soundEffect;
    
    [Header("Post Processing")]
    [SerializeField] private Volume globalVolume;
    private DepthOfField _dof;

    [Header("Settings")]
    [SerializeField] private float actionCooldown = 0.5f;
    [SerializeField] private float transitionSpeed = 15f;
    [SerializeField] private float focusDistance = 0.3f; 
    [SerializeField] private float normalDistance = 10f; 

    private MeshRenderer _tableNoteRenderer;
    private Collider _tableNoteCollider;
    private bool isNoteTaken = false; 
    private float _nextActionTime = 0f;

    private void Awake()
    {
        if (stickyNoteOnTable != null)
        {
            audioSource = GetComponent<AudioSource>();

            _tableNoteRenderer = stickyNoteOnTable.GetComponent<MeshRenderer>();
            _tableNoteCollider = stickyNoteOnTable.GetComponent<Collider>();
        }

        if (globalVolume.profile.TryGet<DepthOfField>(out var dof))
        {
            _dof = dof;
        }
    }

    private void Update()
    {
        if (isNoteTaken && Input.GetKeyDown(KeyCode.F))
        {
            if (Time.time >= _nextActionTime) PutNoteBack();
        }
        
        float targetDistance = isNoteTaken ? focusDistance : normalDistance;
        if (_dof != null)
        {
            _dof.focusDistance.value = Mathf.Lerp(_dof.focusDistance.value, targetDistance, Time.deltaTime * transitionSpeed);
        }
    }

    public void TakeNote()
    {
        if (isNoteTaken || Time.time < _nextActionTime) return;

        isNoteTaken = true;
        playerController.canMove = false;

        audioSource.PlayOneShot(soundEffect);   

        _nextActionTime = Time.time + actionCooldown;

        _tableNoteRenderer.enabled = false;
        _tableNoteCollider.enabled = false;
        stickyNoteOnTableText.enabled = false;
        stickyNoteOnCamera.SetActive(true);
    }

    private void PutNoteBack()
    {
        isNoteTaken = false; 
        playerController.canMove = true;

        audioSource.PlayOneShot(soundEffect);

        _nextActionTime = Time.time + actionCooldown;

        _tableNoteRenderer.enabled = true;
        _tableNoteCollider.enabled = true;
        stickyNoteOnTableText.enabled = true;
        stickyNoteOnCamera.SetActive(false);
    }
}