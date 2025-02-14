using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int _gridX;
    [SerializeField] private int _gridY;
    [SerializeField] private GameObject[] _gameObject;

    private Tile[,] _tiles;   // Tilelarý tutan 2D bir array
    private Tile[] _selectedTiles = new Tile[2];  // Seçilen Taþlarý Tutan bir array

    [SerializeField] private float _swapDuration = 3f;
    [SerializeField] private Ease swapEase = Ease.InOutBack;
    private bool _isSwapping = false;

    [SerializeField] private GameObject _tileBg;
    private GameObject[,] _bgTiles; // Tile BG leri tutan liste

    private void Start()
    {
        CreateGrid();
        CreateGridBackground();
        DOTween.SetTweensCapacity(500, 50);
        CameraFilterEvents.CameraEvents?.Invoke(_gridX, _gridY);
        GridUIEvents.GridEvents?.Invoke(_gridX, _gridY);
    }


    private void CreateGrid()
    {
        _tiles = new Tile[_gridX , _gridY];

        for (int i = 0; i < _gridX; i++)
        {
            for (int j = 0; j < _gridY; j++)
            {

                Vector3 pos = new Vector3(transform.position.x + i, transform.position.y + j, 0);
                int randomIndex = Random.Range(0, _gameObject.Length);

                GameObject Obj = Instantiate(_gameObject[randomIndex], pos, Quaternion.identity);
                Tile tile = Obj.GetComponent<Tile>();
                tile.Initialize(i, j, this);

                _tiles[i,j] = tile;
            
            }
        }
    }

    private void CreateGridBackground()
    {
        _bgTiles = new GameObject[_gridX , _gridY]; // Gride göre bg leri tutacaz

        for(int i = 0; i < _gridX; i++)
        {
            for(int j = 0; j < _gridY; j++)
            {
                Vector3 pos = new Vector3(transform.position.x + i, transform.position.y + j, 0);
                GameObject bg = Instantiate(_tileBg, pos, Quaternion.identity);
                bg.transform.SetParent(transform);
                _bgTiles[i, j] = bg;
            }
        }
    }

    public void SelectedTile(Tile tile)
    {
        if (_selectedTiles[0] == tile)
        {
            DeSelectTile(tile);
        }
        else if (_selectedTiles[1] == tile)
        {
            DeSelectTile(tile);

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

    private bool TileMoveCheck(Tile tile1, Tile tile2)
    {
        int xCheck = Mathf.Abs(tile1.GridX - tile2.GridX);
        // GridX - Tile classýndan Getter  iki tile arasýn da ki X poz Kontrol

        int yCheck = Mathf.Abs(tile1.GridY - tile2.GridY); 
        // GridY - Tile class Getter iki tile arasý Y poz kontrol

        return (xCheck == 1 && yCheck == 0)  || (xCheck == 0 && yCheck == 1);
        // böyle ise True döner swap çalýþýr
    }

    private void SwapTiles()
    {
        if (_selectedTiles[0]!=null && _selectedTiles[1] != null)
        {

            if (!TileMoveCheck(_selectedTiles[0], _selectedTiles[1])) // taþlarý komþu diye kontrol ediyor
            {
               
                DeSelectTile(_selectedTiles[0]); //_selectedTiles Tile classýndan türüyor.
                DeSelectTile(_selectedTiles[1]);
                Debug.Log("Not can move");
                return;
            }
            _isSwapping = true;


            int tile1X = _selectedTiles[0].GridX;
            int tile1Y = _selectedTiles[0].GridY;
            // ilk Seçilen Tile ýn koord saklýyoruz
            int tile2X = _selectedTiles[1].GridX;
            int tile2Y = _selectedTiles[1].GridY;
            // Ýkinci Seçilen Tile koord Saklýyoruz

            Tile tempTile = _tiles[tile1X,tile1Y];
            _tiles[tile1X,tile1Y] = _tiles[tile2X,tile2Y];
            _tiles[tile2X,tile2Y] = tempTile;
            // _tiles Arrayda ki Tilelarýn yerini deðiþtirdik

            //// Tilerlarýn grid Koord güncelliyoruz
            _selectedTiles[0].Initialize(tile2X, tile2Y, this);
            _selectedTiles[1].Initialize(tile1X, tile1Y, this);

            PlaySwapAnim(_selectedTiles[0], _selectedTiles[1],tempTile.transform.position);
        }
    }


    private void PlaySwapAnim(Tile firstTile, Tile secondTile, Vector3 targetPos)
    {
        // Baþlangýç pozisyonlarýný kaydet
      

        DG.Tweening.Sequence swapSequence = DOTween.Sequence();

        // Ýlk tile animasyonu
        swapSequence.Join(firstTile.transform.DOScale(new Vector3(0.2f, 0.2f, 0.2f), _swapDuration * 0.5f));
        swapSequence.Join(firstTile.transform.DOMove(secondTile.transform.position, _swapDuration).SetEase(swapEase));
        swapSequence.Join(firstTile.transform.DOScale(new Vector3(0.1f, 0.1f, 0.1f), _swapDuration * 0.5f).SetDelay(_swapDuration * 0.5f));

        // Ýkinci tile animasyonu
        swapSequence.Join(secondTile.transform.DOScale(new Vector3(0.2f, 0.2f, 0.2f), _swapDuration * 0.5f));
        swapSequence.Join(secondTile.transform.DOMove(targetPos, _swapDuration).SetEase(swapEase));
        swapSequence.Join(secondTile.transform.DOScale(new Vector3(0.1f, 0.1f,0.1f), _swapDuration * 0.5f).SetDelay(_swapDuration * 0.5f));

        // Animasyon tamamlandýðýnda çalýþacak
        swapSequence.OnComplete(() =>
        {
            DeSelectTile(firstTile);
            DeSelectTile(secondTile);
            _isSwapping = false;
        });
    }



    public void DeSelectTile(Tile tile)
    {
        // Seçim Üstün de ki rengi kaldýr 

        if (tile != null) // Eðer bir seçim varsa   
        {
            GameEvent.UnSelectsTile?.Invoke(tile);  // Rengi orj haline getirmek için.

            if (_selectedTiles[0]== tile) // ilk index de bir tile varsa 
            {
                _selectedTiles[0] = null;  // boþalt
            }
            else if (_selectedTiles[1]== tile)
            {
                _selectedTiles[1] = null;
            }
        }
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
