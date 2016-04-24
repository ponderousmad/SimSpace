using UnityEngine;
using System.Collections;

public class CheckRemovalBox : MonoBehaviour
{
    GameObject mCar;
    public GenerateBlock left;
    public GenerateBlock right;
    public GenerateBlock forward;
    public GenerateBlock backward;

    public void CarPassed(GameObject car)
    {
        mCar = car;
    }

    public void TryRemove()
    {
        if (mCar != null)
        {
            float distance = Vector3.Distance(gameObject.transform.position, mCar.transform.position);
            if (distance > 2000)
            {
                LevelBockGenerate.instance.CanBeRemoved(this);
            }
        }
    }
}
