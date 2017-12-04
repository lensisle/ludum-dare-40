using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameManager>();
            }

            if (_instance == null)
            {
                GameObject game = new GameObject("GameManager");
                _instance = game.AddComponent<GameManager>();
            }

            return _instance;
        }
    }

    [SerializeField]
    private GameObject _mapContainer;

    [SerializeField]
    private List<MapChunk> _chunkDatabase;

    [SerializeField]
    private List<Item> _itemDatabase;
    public List<Item> ItemDatabase
    {
        get
        {
            return _itemDatabase;
        }
    }

    [SerializeField]
    private PlayerEntity _player;
    public PlayerEntity Player
    {
        get
        {
            return _player;
        }
    }

    [SerializeField]
    private House _house;
    public House House
    {
        get
        {
            return _house;
        }
    }

    private Grid<ChunkType> _mapGrid;
    public Grid<ChunkType> MapGrid
    {
        get
        {
            return _mapGrid;
        }
    }

    private Dictionary<Int2, MapChunk> _mapInstances;
    public Dictionary<Int2, MapChunk> MapInstances
    {
        get
        {
            return _mapInstances;
        }
    }

    private List<SpriteRenderer> _mapObjects;

    private MapChunk _currentChunkInstance;

    private bool _gameInitialized = false;

    [SerializeField]
    private bool _skipIntro;

    public int PuppiesGoal;

    private void Start()
    {
        _mapInstances = new Dictionary<Int2, MapChunk>();
        _mapObjects = new List<SpriteRenderer>();

        GenerateMap();
        RenderMap();

        _mapObjects.Sort((a, b) =>
        {
            float topVal = b.transform.position.y - (b.bounds.size.y / 2);
            float bottomVal = a.transform.position.y - (a.bounds.size.y / 2);
            return topVal.CompareTo(bottomVal);
        });

        for (var i = _mapObjects.Count - 1; i > 0; i--)
        {
            _mapObjects[i].sortingOrder = i;
        }

        _mapObjects.Add(_player.PlayerRenderer);

        Player.transform.position = _house.RespwanReference;

        _gameInitialized = true;

        Player.Initialize();

        if (!_skipIntro)
        {
            UIManager.Instance.DialogueView.ShowText("Sussie has been my friend since forever...", () => {

                UIManager.Instance.DialogueView.ShowText("now she is weak and old and her puppies are lost outside, I must help her children!", () =>
                {
                    UIManager.Instance.DialogueView.ShowText("the only problem is ... they are TOO MANY! I have to be careful when carrying them.", () => {

                        UIManager.Instance.DialogueView.Hide();

                    });
                });

            });
        }
    }

    public void Reboot()
    {
        Vector3 playerPos = Player.transform.position;
        Vector3 targetPos = House.RespwanReference;

        Player.transform.position = new Vector3(targetPos.x, targetPos.y, playerPos.z);

        Player.PlayerAnimator.SetTrigger("IdleDown");

        Player.PlayerInput.Active = true;

        foreach (Transform child in _mapContainer.transform)
        {
            Destroy(child.gameObject);
        }

        _mapInstances = new Dictionary<Int2, MapChunk>();
        _mapObjects = new List<SpriteRenderer>();

        GenerateMap();
        RenderMap();

        _mapObjects.Sort((a, b) =>
        {
            float topVal = b.transform.position.y - (b.bounds.size.y / 2);
            float bottomVal = a.transform.position.y - (a.bounds.size.y / 2);
            return topVal.CompareTo(bottomVal);
        });

        for (var i = _mapObjects.Count - 1; i > 0; i--)
        {
            _mapObjects[i].sortingOrder = i;
        }

        UIManager.Instance.DialogueView.ShowText("You wake up not knowing how you appeared here...", () =>
        {
            UIManager.Instance.DialogueView.Hide();
        });
    }

    private void Update()
    {
        if (!_gameInitialized)
        {
            return;
        }
    }

    private void GenerateMap()
    {
        int mapWidth = Random.Range(3, 5);
        int mapHeight = Random.Range(4, 12);

        PuppiesGoal = 5 * 10;

        _mapGrid = new Grid<ChunkType>(5, 10);

        for (var i = 0; i < _mapGrid.Width; i++)
        {
            for (var j = 0; j < _mapGrid.Height; j++)
            {
                if (i == 0 && j == 0)
                {
                    _mapGrid.SetNode(i, j, ChunkType.BottomLeft);
                }
                else if (i == _mapGrid.Width - 1 && j == 0)
                {
                    _mapGrid.SetNode(i, j, ChunkType.BottomRight);
                }
                else if (i == 0 && j == _mapGrid.Height - 1)
                {
                    _mapGrid.SetNode(i, j, ChunkType.WallTopLeft);
                }
                else if (i == _mapGrid.Width - 1 && j == _mapGrid.Height - 1)
                {
                    _mapGrid.SetNode(i, j, ChunkType.WallTopRight);
                }
                else if (i == 0)
                {
                    _mapGrid.SetNode(i, j, ChunkType.WallLeft);
                }
                else if (i == _mapGrid.Width - 1)
                {
                    _mapGrid.SetNode(i, j, ChunkType.WallRight);
                }
                else if (j == 0)
                {
                    _mapGrid.SetNode(i, j, ChunkType.WallBottom);
                }
                else if (j == _mapGrid.Height - 1)
                {
                    _mapGrid.SetNode(i, j, ChunkType.WallTop);
                }
                else
                {
                    int random = Random.Range(0, _chunkDatabase.Count);
                    while (_chunkDatabase[random].Type == ChunkType.Initial)
                    {
                        random = Random.Range(0, _chunkDatabase.Count);
                    }
                    _mapGrid.SetNode(i, j, _chunkDatabase[random].Type);
                }

            }
        }

        _mapGrid.SetNode(0, 0, ChunkType.Initial);
    }

    private void RenderMap()
    {
        int totalOrderInLayer = _mapGrid.Height * 500;

        for (var i = 0; i < _mapGrid.Width; i++)
        {
            for (var j = 0; j < _mapGrid.Height; j++)
            {
                ChunkType targetType = _mapGrid.GetNodeAt(i, j).Data;

                List<MapChunk> group = _chunkDatabase.FindAll(x => x.Type.Equals(targetType));
                int groupSize = group.Count;
                MapChunk target = group[Random.Range(0, groupSize)];

                MapChunk chunkInstance = Instantiate(target);
                chunkInstance.Coords.SetInt2(i, j);

                _mapInstances.Add(chunkInstance.Coords, chunkInstance);
                
                if (i > 0)
                {
                    MapChunk left = _mapInstances[new Int2(i - 1, j)];

                    float xCoord = left.WorldArea.x + left.transform.position.x;

                    chunkInstance.transform.position = new Vector3(xCoord, chunkInstance.transform.position.y, chunkInstance.transform.position.z);
                }

                if (j > 0)
                {
                    MapChunk bottom = _mapInstances[new Int2(i, j - 1)];

                    float yCoord = bottom.WorldArea.y + bottom.transform.position.y;

                    chunkInstance.transform.position = new Vector3(chunkInstance.transform.position.x, yCoord, chunkInstance.transform.position.z);

                }

                chunkInstance.transform.SetParent(_mapContainer.transform, true);

                chunkInstance.ChunkOrderLayerBase = totalOrderInLayer - (500 * j);

                chunkInstance.Initialize();

                SpriteRenderer[] chunkChilds = chunkInstance.GetComponentsInChildren<SpriteRenderer>();
                _mapObjects.AddRange(chunkChilds);

            }
        }
    }
}

[System.Serializable]
public class TeleportTarget
{
    public string Name;
    public GameObject Target;
}
