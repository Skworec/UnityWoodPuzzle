using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using BlockPrefab;

public class DataController : MonoBehaviour
{
    public static DataController instance { get; private set; }

    [SerializeField] private uint score = 0;
    [SerializeField] private uint bestScore;
    [SerializeField] private bool isSoundEnabled;

    [SerializeField] private List<GameObject> ActiveBlocks = new List<GameObject>();

    #region Array fields
    [SerializeField] private GameObject[,] tiles;
    [SerializeField] private Vector3 initCoordinates = new Vector3(-2.1328125f, 3.2378125f, 0);
    [SerializeField] public float tileSize = 0.609375f;
    [SerializeField] private ushort fieldSize = 8;
    [SerializeField] public GameObject tilePrefab;
    [SerializeField] private GameObject gameField;
    [SerializeField] public GameObject PuzzleBlock;
    #endregion

    #region Events
    [SerializeField] public UnityEvent onScoreChange = new UnityEvent();
    [SerializeField] public UnityEvent onBestScoreChange = new UnityEvent();
    [SerializeField] public UnityEvent onLoose = new UnityEvent();
    #endregion

    #region Properties
    public uint Score
    {
        get
        {
            return score;
        }
        set
        {
            score = value;
            onScoreChange?.Invoke();
            if (score > bestScore)
            {
                BestScore = score;
            }
        }
    }
    public void SetZeroScore()
    {
        Score = 0;
    }
    public uint BestScore
    {
        get
        {
            return bestScore;
        }
        private set
        {
            bestScore = value;
            UpdateBestScore();
            onBestScoreChange?.Invoke();
        }
    }
    #endregion

    #region ArrayWork
    /// <summary>
    /// Создает сетку поля
    /// </summary>
    private void InitArray()
    {
        tiles = new GameObject[fieldSize, fieldSize];
        for (int i = 0; i < fieldSize; i++)
        {
            for (int j = 0; j < fieldSize; j++)
            {
                tiles[i, j] = Instantiate(tilePrefab, initCoordinates + new Vector3(j * tileSize, -i * tileSize, 0), Quaternion.identity, gameField.transform);
                tiles[i, j].GetComponent<Tile>().IsFill = false;
            }
        }
    }


    /// <summary>
    /// Уничтожает заполнившиеся линии если таковые есть
    /// </summary>
    public void CheckArrayLines()
    {
        List<int> lines = new List<int>();
        List<int> columns = new List<int>();
        for (int i = 0; i < fieldSize; i++)
        {
            bool filled = true;
            for (int j = 0; j < fieldSize; j++)
            {
                if (tiles[i, j].GetComponent<Tile>().IsFill == false)
                {
                    filled = false;
                    break;
                }
            }
            if (filled == true)
            {
                columns.Add(i);
            }
        }
        for (int i = 0; i < fieldSize; i++)
        {
            bool filled = true;
            for (int j = 0; j < fieldSize; j++)
            {
                if (tiles[j, i].GetComponent<Tile>().IsFill == false)
                {
                    filled = false;
                    break;
                }
            }
            if (filled == true)
            {
                lines.Add(i);
            }
        }
        foreach (var x in lines)
        {
            for (int i = 0; i < fieldSize; i++)
            {
                tiles[i, x].GetComponent<Tile>().IsFill = false;
            }
            Score += (uint)(fieldSize + 2);
        }
        foreach (var x in columns)
        {
            for (int i = 0; i < fieldSize; i++)
            {
                tiles[x, i].GetComponent<Tile>().IsFill = false;
            }
            Score += (uint)(fieldSize + 2);
        }
        bool canContinue = false;
        foreach (var item in ActiveBlocks)
        {
            if (CanPutBlock(item.GetComponent<NextBlock>().puzzleBlock.GetComponent<BlockPrefabScript>().mask))
            {
                canContinue = true;
                Debug.Log("Not loose");
            }
        }
        if (!canContinue)
        {
            Debug.Log("Loose");
            onLoose.Invoke();
        }
    }

    public bool TryPutBlock(Massive[] mass, Vector3 point)
    {
        if (((point.x + tileSize / 2) < initCoordinates.x) || ((point.y - tileSize / 2 ) > initCoordinates.y))
        {
            return false;
        }
        Vector2 index = new Vector2(
            Mathf.Round(Mathf.Abs((point.x - initCoordinates.x) / tileSize)),
            Mathf.Round(Mathf.Abs((point.y - initCoordinates.y) / tileSize)));
        if (!(((int)index.x + mass[0].mass.Length <= tiles.GetLength(0)) && ((int)index.y + mass.Length <= tiles.GetLength(1))))
        {
            return false;
        }
        bool success = true;

        for (int i = (int)index.x; i < (int)index.x + mass[0].mass.Length; i++)
        {
            for (int j = (int)index.y; j < (int)index.y + mass.Length; j++)
            {
                if (tiles[j, i].GetComponent<Tile>().IsFill == mass[j - (int)index.y].mass[i - (int)index.x] && mass[j - (int)index.y].mass[i - (int)index.x])
                {
                    success = false;
                    return false;
                }
                if (!mass[j - (int)index.y].mass[i - (int)index.x])
                {
                    continue;
                }
            }
        }

        if (success)
        {
            for (int i = (int)index.x; i < (int)index.x + mass[0].mass.Length; i++)
            {
                for (int j = (int)index.y; j < (int)index.y + mass.Length; j++)
                {
                    if (mass[j - (int)index.y].mass[i - (int)index.x])
                    {
                        tiles[j, i].GetComponent<Tile>().IsFill = true;
                        Score++;
                    }
                }
            }
            return true;
        }
        return false;
    }

    public bool CanPutBlock(Massive[] mass)
    {
        for (int i = 0; i <= tiles.GetLength(0) - mass.Length; i++)
        {
            for (int j = 0; j <= tiles.GetLength(1) - mass[0].mass.Length; j++)
            {
                bool succes = true;
                for (int k = 0; k < mass.Length; k++)
                {
                    for (int l = 0; l < mass[0].mass.Length; l++)
                    {
                        if (mass[k].mass[l])
                        {
                            if (mass[k].mass[l] == tiles[k + i, l + j].GetComponent<Tile>().IsFill) 
                            {
                                succes = false;
                                break;
                            }
                        }
                    }
                    if (!succes)
                        break;
                }
                if (succes)
                    return true;
            }
        }
        return false;
    }

    public void RestartGame()
    {
        Score = 0;
        for (int i = 0; i < fieldSize; i++)
        {
            for (int j = 0; j < fieldSize; j++)
            {
                tiles[i, j].GetComponent<Tile>().IsFill = false;
            }
        }
        ActiveBlocks.ForEach(Block =>
        {
            Block.GetComponent<NextBlock>().GetNewBlock();
        });
    }
    #endregion

    #region PlayerPrefs
    public void SwitchSoundState()
    {
        isSoundEnabled = !isSoundEnabled;
        PlayerPrefs.SetInt("isSoundEnabled", isSoundEnabled ? 1 : 0);
        if (isSoundEnabled && !gameObject.GetComponent<AudioSource>().isPlaying)
        {
            gameObject.GetComponent<AudioSource>().Play();
        }
        else
        {
            gameObject.GetComponent<AudioSource>().Stop();
        }
    }
    public bool IsSoundEnabled()
    {
        if (PlayerPrefs.HasKey("isSoundEnabled"))
        {
            if (PlayerPrefs.GetInt("isSoundEnabled") != 1)
            {
                gameObject.GetComponent<AudioSource>().Stop();
            }
            return PlayerPrefs.GetInt("isSoundEnabled") == 1;
        }
        else
        {
            PlayerPrefs.SetInt("isSoundEnabled", 1);
            return true;
        }
    }
    private uint GetBestScore()
    {
        if (PlayerPrefs.HasKey("bestScore"))
        {
            return (uint)PlayerPrefs.GetInt("bestScore");
        }
        else
        {
            PlayerPrefs.SetInt("bestScore", 0);
            return 0;
        }
    }
    private void UpdateBestScore()
    {
        PlayerPrefs.SetInt("bestScore", (int)bestScore);
    }
    #endregion

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        isSoundEnabled = IsSoundEnabled();
        BestScore = GetBestScore();
        InitArray();
    }
}
