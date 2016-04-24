using UnityEngine;
using System.Collections;

public class LevelBockGenerate : MonoBehaviour
{
    private static LevelBockGenerate mInstance;
    public static LevelBockGenerate instance { get { return mInstance; } }
    public GameObject blockPrefab;
    public float blockSize = 990;

    void Start()
    {
        if (mInstance == null)
        {
            mInstance = this;
        }
    }

    public void GenerateNewBlock(GameObject target, GenerateBlock.Direction direction)
    {
        GameObject block = (GameObject)Instantiate(blockPrefab);
        Vector3 addPos = Vector3.zero;
        Vector3 targetPos = target.transform.position;
        Debug.Log("GenerateNewBlock");
        switch(direction)
        {
            case GenerateBlock.Direction.FORWARD:
                addPos = new Vector3(0, 0, blockSize);
                break;
            case GenerateBlock.Direction.BACKWARD:
                addPos = new Vector3(0, 0, -blockSize);
                break;
            case GenerateBlock.Direction.LEFT:
                addPos = new Vector3(-blockSize, 0, 0);
                break;
            case GenerateBlock.Direction.RIGHT:
                addPos = new Vector3(blockSize, 0, 0);
                break;
        }

        block.transform.position = targetPos + addPos;
    }
}
