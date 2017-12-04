using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : MonoBehaviour
{
    [SerializeField]
    private Transform _entry;
    public Transform Entry
    {
        get
        {
            return _entry;
        }
    }

    [SerializeField]
    private List<SpriteRenderer> _chunkObjects;

    [SerializeField]
    private GameObject _respawnReference;
    public Vector3 RespwanReference
    {
        get
        {
            return _respawnReference.transform.position;
        }
    }

    private int _chunkOrderLayerBase = 10000;

    private bool _playerInHouse;

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
            if (_chunkObjects[i] == GameManager.Instance.Player.PlayerRenderer)
            {
                _chunkObjects[i].sortingOrder = _chunkOrderLayerBase + i;
            }
        }
    }

    private void Update()
    {
        if (_playerInHouse)
        {
            SortPlayer();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Entered to house");
        _playerInHouse = true;
        Camera.main.backgroundColor = Color.black;
        Camera.main.GetComponent<CameraParticle>().SetParticlesVisible(false);
        _chunkObjects.Add(GameManager.Instance.Player.PlayerRenderer);
        SortPlayer();
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("Leaved house");
        _playerInHouse = false;
        Camera.main.backgroundColor = Color.white;
        Camera.main.GetComponent<CameraParticle>().SetParticlesVisible(true);
        _chunkObjects.Add(GameManager.Instance.Player.PlayerRenderer);
    }
}
