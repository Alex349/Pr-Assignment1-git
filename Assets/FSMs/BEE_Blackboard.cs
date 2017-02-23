using UnityEngine;
using System.Collections;

public class BEE_Blackboard : MonoBehaviour
{

    public float hunger = 0f;   // How hungry is BATCAT?

    // CONSTANTS
    public float flowerDetectableRadius = 10f; // id for trash cans
    public float rummageTime = 5f; // the time rummaging lasts
    public float eatingTime = 5f;  // the time eating lasts
    public float placeReachedRadius = 1f; // at this distance a place has been reached
    public float perilSafetyRadius = 10f;
    public float perilDetectableRadius = 10f;
    public GameObject rusc; // the place where BATCAT hides
    public GameObject honeyPrefab;

    void Start()
    {

        if (rusc == null)
        {
            rusc = GameObject.Find("Rusc");
            if (rusc == null)
            {
                Debug.LogError("no Rusc object found in " + this);
            }
        }

        if (honeyPrefab == null)
        {
            honeyPrefab = Resources.Load<GameObject>("Honey");
            if (honeyPrefab == null)
            {
                Debug.LogError("no Honey PREFAB in Resources folder found in " + this);
            }
        }
    }

}
