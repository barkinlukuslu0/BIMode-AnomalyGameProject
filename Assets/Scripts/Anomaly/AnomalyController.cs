using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AnomalyProp
{
    public string propName; //Name of the object
    public GameObject normalObject; //Normal version of the object
    public GameObject anomalyObject; //Anomaly version of the object
}

public class AnomalyController : MonoBehaviour
{
    [SerializeField] private GameObject chasingEnemy;

    // The list which include all props in the scene
    public List<AnomalyProp> allProps;
    [SerializeField] private float anomalySpawnChance = 50f;
    [SerializeField] private float enemySpawnChance = 20f;

    private bool _isThereAnomaly = false;

    private void Start()
    {
        ResetAllAnomalies();
        SetAnomalies();
    }

    public void SetAnomalies()
    {   
        float chance = Random.Range(0f, 100f);

        // If chance smaller than spawnChance state continue
        if(chance <= anomalySpawnChance)
        {
            float enemyChance = Random.Range(0f, 100f);

            if(enemyChance <= enemySpawnChance)
            {
                chasingEnemy.SetActive(true);
            }

            else
            {
                int index = Random.Range(0, allProps.Count);

                //The anomaly spawns at the scene
                if(allProps[index] != null)
                {
                    allProps[index].normalObject.SetActive(false);
                    allProps[index].anomalyObject.SetActive(true);

                    _isThereAnomaly = true;
                }
            }
        }
    }

    public void ResetAllAnomalies()
    {
        if(chasingEnemy.gameObject.activeSelf)
            chasingEnemy.SetActive(false);

        foreach (AnomalyProp prop in allProps)
        {
            if(prop.normalObject != null)
            {
                prop.normalObject.SetActive(true);
            }
            if(prop.anomalyObject != null)
            {
                prop.anomalyObject.SetActive(false);
            }
        }

        _isThereAnomaly = false;
    }

    public void SetAnomalyBool(bool state)
    {
        _isThereAnomaly = state;
    }

    public bool IsThereAnomaly()
    {
        return _isThereAnomaly;
    }
}
