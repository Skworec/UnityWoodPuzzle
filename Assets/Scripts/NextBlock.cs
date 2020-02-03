using System.Collections;
using UnityEngine;
public class NextBlock : MonoBehaviour, IRaycastable
{
    [SerializeField]
    public GameObject puzzleBlock;
    private void Start()
    {
        RaycastController.AddToRaycasttable(gameObject.GetComponent<Collider2D>());
        GetNewBlock();
    }

    public void GetNewBlock()
    {
        if (puzzleBlock)
            MyPool.instance.PutInPool(puzzleBlock);
        puzzleBlock = MyPool.instance.GetFromPool();
        puzzleBlock.transform.position = gameObject.transform.position;
        puzzleBlock.transform.localScale = new Vector3(0.5f, 0.5f, 0);
    }

    public void OnRaycastHit()
    {
        puzzleBlock.transform.localScale = new Vector3(1f, 1f, 0);
        StartCoroutine(DragBlock());
    }

    private IEnumerator DragBlock()
    {
        while (Input.GetMouseButton(0))
        {
            puzzleBlock.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0, 1, 10);
            yield return null;
        }
        if (DataController.instance.TryPutBlock(puzzleBlock.GetComponent<BlockPrefab.BlockPrefabScript>().mask, puzzleBlock.transform.position))
        {
            puzzleBlock.transform.position = new Vector3(-10, 0, 0);
            GetNewBlock();
            DataController.instance.CheckArrayLines();
            //Destroy(puzzleBlock);
        }
        else
        {
            puzzleBlock.transform.position = gameObject.transform.position;
            puzzleBlock.transform.localScale = puzzleBlock.transform.localScale = new Vector3(0.5f, 0.5f, 0);
        }
    }
}
