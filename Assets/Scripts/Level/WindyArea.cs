using UnityEngine;

public class WindyArea : MonoBehaviour
{
    [SerializeField] private SoundsController soundsController;

    private void Awake()
    {
        soundsController = FindAnyObjectByType<SoundsController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            soundsController.ChangeEnvironmentAudio();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            soundsController.ChangeEnvironmentAudio();
        }
    }
}
