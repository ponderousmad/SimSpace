using UnityEngine;
using UnityStandardAssets.Vehicles.Car;
using System.Collections;

public class GenerateBlock : MonoBehaviour
{
    public Direction        generateDirection;
    public CheckRemovalBox  targetParent;
    bool mTriggered;
    GameObject mCar;

    void Start()
    {
        name = generateDirection.ToString();
    }

    public enum Direction
    {
        FORWARD,
        BACKWARD,
        LEFT,
        RIGHT
    }

    void OnTriggerEnter(Collider other)
    {
        bool carHit = string.Equals("ColliderBody", other.name);
        if (carHit && LevelBockGenerate.instance && !mTriggered)
        {
            mCar = other.gameObject;
            targetParent.CarPassed(mCar);
            mTriggered = true;
            LevelBockGenerate.instance.GenerateNewBlock(targetParent, generateDirection);
            return;
        }
    }
}
