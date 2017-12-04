using System.Collections.Generic;
using UnityEngine;

public enum ChunkType
{
    WallLeft = 1,
    WallRight = 2,
    WallTop = 3,
    WallBottom = 4,

    WallTopLeft = 5,
    WallTopRight = 6,
    BottomLeft = 7,
    BottomRight = 8,
    
    Outer = 10,

    Initial = 0
}

public class MapChunk : MonoBehaviour
{
    [SerializeField]
    private Vector2 _worldArea;
    public Vector2 WorldArea
    {
        get
        {
            return _worldArea;
        }
    }

    [SerializeField]
    private List<GameObject> _subChunks;

    [SerializeField]
    private ChunkType _type;
    public ChunkType Type
    {
        get
        {
            return _type;
        }
    }

    public Int2 Coords;
    public int ChunkOrderLayerBase;

    private List<SpriteRenderer> _chunkObjects;

    [SerializeField]
    private List<GameObject> _spawnBoxes;

    private bool _playerOnChunk;

    [SerializeField]
    private List<TeleportTarget> _teleportTargets;
    public List<TeleportTarget> TeleportTargets
    {
        get
        {
            return _teleportTargets;
        }
    }

    public void Initialize()
    {
        _chunkObjects = new List<SpriteRenderer>();
        _chunkObjects.AddRange(GetComponentsInChildren<SpriteRenderer>());

        foreach(GameObject spawnBox in _spawnBoxes)
        {
            bool shouldSpawn = Random.Range(0, 100) > 0;
            spawnBox.SetActive(shouldSpawn);
        }

    }

    private void Update()
    {
        if (_playerOnChunk)
        {
            _chunkObjects.Sort((a, b) =>
            {
                float topVal = b.transform.position.y - (b.bounds.size.y / 2);
                float bottomVal = a.transform.position.y - (a.bounds.size.y / 2);
                return topVal.CompareTo(bottomVal);
            });

            for (var i = _chunkObjects.Count - 1; i > 0; i--)
            {
                if (_chunkObjects[i] == GameManager.Instance.Player.PlayerRenderer)
                {
                    _chunkObjects[i].sortingOrder = ChunkOrderLayerBase + i;
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        _playerOnChunk = true;
        Debug.Log("Player entered to chunk " + Coords);
        _chunkObjects.Add(GameManager.Instance.Player.PlayerRenderer);
        SortPlayer();
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        _playerOnChunk = false;
        Debug.Log("Player leave chunk " + Coords);
        _chunkObjects.Remove(GameManager.Instance.Player.PlayerRenderer);
        SortPlayer();
    }

    private void SortPlayer()
    {
        _chunkObjects.Sort((a, b) =>
        {
            float topVal = b.transform.position.y - (b.bounds.size.y / 2);
            float bottomVal = a.transform.position.y - (a.bounds.size.y / 2);
            return topVal.CompareTo(bottomVal);
        });

        for (var i = _chunkObjects.Count - 1; i > 0; i--)
        {
            _chunkObjects[i].sortingOrder = ChunkOrderLayerBase + i;
        }
    }

#if UNITY_EDITOR

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

        Gizmos.DrawWireCube(transform.position, new Vector3(_worldArea.x, _worldArea.y, transform.position.z));
    }

#endif
}
