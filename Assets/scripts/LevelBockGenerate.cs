using UnityEngine;
using System.Collections.Generic;

public class LevelBockGenerate : MonoBehaviour
{
    private static LevelBockGenerate mInstance;
    public static LevelBockGenerate instance { get { return mInstance; } }
    public GameObject blockPrefab;
    public float blockSize = 990;
    public List<CheckRemovalBox> createdBlocks = new List<CheckRemovalBox>();

    void Start()
    {
        if (mInstance == null)
        {
            mInstance = this;
        }
    }

    public void GenerateNewBlock(CheckRemovalBox target, GenerateBlock.Direction direction)
    {
        GameObject block = (GameObject)Instantiate(blockPrefab);
        CheckRemovalBox g = block.GetComponent<CheckRemovalBox>();
        createdBlocks.Add(g);
        Vector3 addPos = Vector3.zero;
        Vector3 targetPos = target.transform.position;
        Debug.Log("GenerateNewBlock: " + direction);
        switch(direction)
        {
            case GenerateBlock.Direction.FORWARD:
                addPos = new Vector3(0, 0, blockSize);
                g.backward.gameObject.SetActive(false);
                break;
            case GenerateBlock.Direction.BACKWARD:
                addPos = new Vector3(0, 0, -blockSize);
                g.forward.gameObject.SetActive(false);
                break;
            case GenerateBlock.Direction.LEFT:
                addPos = new Vector3(-blockSize, 0, 0);
                g.right.gameObject.SetActive(false);
                break;
            case GenerateBlock.Direction.RIGHT:
                addPos = new Vector3(blockSize, 0, 0);
                g.left.gameObject.SetActive(false);
                break;
        }

        block.transform.position = targetPos + addPos;
        CheckRemoval();
    }

    public void CanBeRemoved(CheckRemovalBox block)
    {
         createdBlocks.Remove(block);
         Destroy(block.gameObject);
    }

    void CheckRemoval()
    {
        if (createdBlocks.Count > 1)
        {
            foreach (CheckRemovalBox g in createdBlocks)
            {
                if (g != null)
                {
                    g.TryRemove();
                }
            }
        }
    }
}
