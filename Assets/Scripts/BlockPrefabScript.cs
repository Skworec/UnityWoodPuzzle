using UnityEngine;

namespace BlockPrefab
{
    public class BlockPrefabScript : MonoBehaviour
    {
        [SerializeField] public Massive[] mask;
        private void Start()
        {
            for (int i = 0; i < mask.Length; i++)
            {
                for (int j = 0; j < mask[i].mass.Length; j++)
                {
                    if (mask[i].mass[j])
                    {
                        Instantiate(DataController.instance.tilePrefab,
                            new Vector3(DataController.instance.tileSize * j / 2,
                                DataController.instance.tileSize * -i / 2, 0)
                                + gameObject.transform.position,
                            Quaternion.identity,
                            gameObject.transform).
                            GetComponent<Tile>().IsFill = true;
                    }
                }
            }
        }
    }

    [System.Serializable]
    public class Massive
    {
        public bool[] mass;
    }

    
}