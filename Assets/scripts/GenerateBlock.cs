using UnityEngine;
using UnityStandardAssets.Vehicles.Car;
using System.Collections;

public class GenerateBlock : MonoBehaviour
{
    public Direction generateDirection;
    public GameObject targetParent;
    bool mTriggered;

    public enum Direction
    {
        FORWARD,
        BACKWARD,
        LEFT,
        RIGHT
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("other name: " + other.name);
        if(string.Equals("ColliderBody", other.name) && LevelBockGenerate.instance && !mTriggered)
        {
            Debug.Log("Create");
            mTriggered = true;
            LevelBockGenerate.instance.GenerateNewBlock(targetParent, generateDirection);
        }
    }
}
