using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int _gridX;
    [SerializeField] private int _gridY;
    [SerializeField] private GameObject[] _gameObject;

    private Tile[] _tiles;   // Taþlarý tutan dizi
    private Tile[] _selectedTiles = new Tile[2];  // Seçilen Taþlarý Tutan bir array

    public object SelectedTiles { get; private set; }

    private void Start()
    {
        CreateGrid();
    }


    private void CreateGrid()
    {
        _tiles = new Tile[_gridX * _gridY];

        for (int i = 0; i < _gridX; i++)
        {
            for (int j = 0; j < _gridY; j++)
            {

                Vector3 pos = new Vector3(transform.position.x + i, transform.position.y + j, 0);
                int randomIndex = Random.Range(0, _gameObject.Length);

                GameObject Obj = Instantiate(_gameObject[randomIndex], pos, Quaternion.identity);
                Tile tile = Obj.GetComponent<Tile>();
                tile.Initialize(i, j, this);

                _tiles[i * _gridY + j] = tile;
            }
        }
    }

    public void SelectedTile(Tile tile)
    {
        if (_selectedTiles[0] == tile)
        {
            DeSelectTile(0);
        }
        else if (_selectedTiles[1] == tile)
        {
            DeSelectTile(1);
        }
        else
        {
            if (_selectedTiles[0] == null)
            {
                _selectedTiles[0] = tile;
                GameEvent.SelectsTile?.Invoke(tile);

            }
            else if(_selectedTiles[1] == null)
            {
                _selectedTiles[1] = tile;
                GameEvent.SelectsTile?.Invoke(tile);

                Debug.Log("2 Tile seçildi");
                SwapTiles();
            }
        }
    }

    private void OnEnable()=>RegisterEvents();
    private void OnDisable()=> UnRegisterEvents();

    private void RegisterEvents() => GameEvent.OnClickEvents += SelectedTile;
    private void UnRegisterEvents() => GameEvent.OnClickEvents -= SelectedTile;
    // Mouse Sol Týk Eventi.


    private void SwapTiles()
    {
        if (_selectedTiles[0]!=null && _selectedTiles[1] != null)
        {
            Vector3 pos = _selectedTiles[0].transform.position;
            _selectedTiles[0].transform.position = _selectedTiles[1].transform.position;
            _selectedTiles[1].transform.position= pos;

            DeSelectTile(0);
            DeSelectTile(1);
        }
    }

  
    private void DeSelectTile(int index)
    {
        GameEvent.UnSelectsTile?.Invoke(index);
        _selectedTiles[index] = null;
    }

#if UNITY_EDITOR_64
    private void OnDrawGizmos()
    {
        for (int i = 0; i < _gridX; i++)
        {
            for (int j = 0; j < _gridY; j++)
            {
                Gizmos.DrawWireCube(new Vector3(transform.position.x + i, transform.position.y + j, 0), new Vector3(1, 1, 1));
            }
        }
    }
#endif
}
