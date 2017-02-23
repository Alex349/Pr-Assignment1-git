using UnityEngine;
using System.Collections;

public class FlowerBehaviour : MonoBehaviour
{

    public int honeyLeft = 100;
    public float timeToCreateHoney = 100;

    void Update()
    {
        timeToCreateHoney -= Time.deltaTime;

        if (timeToCreateHoney < 0)
        {
            honeyLeft = 100;
            timeToCreateHoney = 100;
            gameObject.tag = "Flower";
        }
        if (honeyLeft <= 0)
        {
            gameObject.tag = "NoHoney";
        }


    }

    public void BeBitten()
    {
        honeyLeft-= 25;
    }
}
