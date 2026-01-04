using UnityEngine;

public class EnemyCatchTrigger : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SceneController sceneController;

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            sceneController.LoadDeadScene();
        }
    }
}
