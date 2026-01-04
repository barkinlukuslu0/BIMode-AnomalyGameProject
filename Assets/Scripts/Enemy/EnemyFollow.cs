using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;

public class EnemyFollow : MonoBehaviour
{
    [Header ("References")]
    [SerializeField] private NavMeshAgent enemy;
    [SerializeField] private Transform player;
    [SerializeField] private AnomalyController anomalyController;
    [SerializeField] private LightsController lightsController;

    [Header("Speed Settings")]
    [SerializeField] private float maxSpeed = 2f;    
    [SerializeField] private float minSpeed = 2f;    
    [SerializeField] private float maxDistance = 10f;
    [SerializeField] private float minDistance = 2f;  

    private Vector3 _defaultEnemyPosition;
    private quaternion _defaultEnemyRotation;
    private bool _isEnemyTriggered = false;

    private void Awake()
    {
        if (anomalyController == null)
            anomalyController = FindAnyObjectByType<AnomalyController>();

        _defaultEnemyPosition = transform.position;
        _defaultEnemyRotation = transform.rotation;
    }

    private void OnEnable()
    {
        if(anomalyController != null)
        {
             anomalyController.SetAnomalyBool(true);
        }
        _isEnemyTriggered = false;

        if (enemy != null)
        {
            enemy.Warp(_defaultEnemyPosition);
            transform.rotation = _defaultEnemyRotation;
            enemy.ResetPath();
            enemy.velocity = Vector3.zero; 
        }
    }

    private void Update()
    {
        if (!_isEnemyTriggered) return;

        enemy.SetDestination(player.position);
        SetEnemySpeed();
    }

    private void SetEnemySpeed()
    {
        float _distance = Vector3.Distance(transform.position, player.position);

        float speedFactor = Mathf.InverseLerp(minDistance, maxDistance, _distance);
        float finalSpeed = Mathf.Lerp(minSpeed, maxSpeed, speedFactor);

        enemy.speed = finalSpeed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_isEnemyTriggered) return; 

        if (other.CompareTag("Player"))
        {
            _isEnemyTriggered = true;
            StartCoroutine(lightsController.EnemyStartsFollow());
        }
    }
}