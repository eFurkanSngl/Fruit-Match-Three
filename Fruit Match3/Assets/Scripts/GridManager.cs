using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    [Header("Grid Settings")]
    [SerializeField] private int _gridX;
    [SerializeField] private int _gridY;


    [Header("Tile Settings")]
    [SerializeField] private GameObject[] _tileObject;   // Prefab
    [SerializeField] private GameObject _tileBg;  // Tile bg
    [SerializeField] private float _swapDuration = 3f;  // anim süre
    [SerializeField] private Ease swapEase = Ease.InOutBack; // anim tipi
    

    private Tile[,] _tiles;   // Tilelarý tutan 2D bir array
    private Tile[] _selectedTiles = new Tile[2];  // Seçilen Taþlarý Tutan bir array
    private bool _isSwapping = false;
    private bool _isFalling = false;
    private GameObject[,] _bgTiles; // Tile BG leri tutan liste
    private CheckMatches _checkMatch;

    private void Start()
    {
        CreateGrid();
        CreateTileBackground();
        DOTween.SetTweensCapacity(500, 50);
        _checkMatch =  GetComponent<CheckMatches>();
    }


    private void CreateGrid()
    {
        _tiles = new Tile[_gridX , _gridY];

        for (int i = 0; i < _gridX; i++)
        {
            for (int j = 0; j < _gridY; j++)
            {
           
                Vector3 pos = new Vector3(transform.position.x + i, transform.position.y + j, 0);
                int  randomIndex = HasMatchStart(i, j);
                 GameObject obj = Instantiate(_tileObject[randomIndex], pos, Quaternion.identity);
                 Tile tile = obj.GetComponent<Tile>();
                 tile.Initialize(i, j, this); 

                _tiles[i,j] = tile;
                obj.transform.SetParent(transform);
            
            }
        }
        GridUIEvents.GridBorderEvents?.Invoke(_gridX, _gridY);
        GridUIEvents.GridEvents?.Invoke(_gridX, _gridY);
        CameraFilterEvents.CameraEvents?.Invoke(_gridX, _gridY);
    }
    private void CreateTileBackground()
    {
        _bgTiles = new GameObject[_gridX,_gridY]; // Gride göre bg leri tutacaz

        for (int i = 0; i < _gridX; i++)
        {
            for (int j = 0; j < _gridY; j++)
            {
                Vector3 pos = new Vector3(transform.position.x + i, transform.position.y + j, 0);
                GameObject bg = Instantiate(_tileBg, pos, Quaternion.identity);
                bg.transform.SetParent(transform);
                _bgTiles[i, j] = bg;
            }
        }
    }

   

    private IEnumerator RainDownRoutine()
    {
        _isFalling = true;
        yield return new WaitForSeconds(0.3f); // Yok olma animasyonunun bitmesini bekle

        // Her sütun için kontrol
        for (int x = 0; x < _gridX; x++) // Sütünlarý sayýyoruz
        {
            for (int y = 0; y < _gridY; y++)  // her sütünü kontrol 
            {
                if (_tiles[x, y] == null) // Boþ bir tile bulursak Gridde 
                {
                    for (int newY = y + 1; newY < _gridY; newY++) // grid7 den baþlýcak yukarý doðru kotnrol
                    {
                        Tile newMoveTile = _tiles[x, newY]; // yukarýda ki baktýðýmýz Tile
                        if (newMoveTile != null)
                        {
                            // Tile'ý aþaðý taþý
                            _tiles[x, newY] = null; // poz boþaltýyoruz
                            _tiles[x, y] = newMoveTile; // Yeni poz boþalttýðýmýz yere atýyoruz
                            newMoveTile.Initialize(x, y, this);

                            // Düþme animasyonu
                            Vector3 targetPos = new Vector3(x, y, 0);
                            newMoveTile.transform.DOMove(targetPos, 0.3f).SetEase(Ease.OutBounce);//OutBounce

                            break;
                        }
                    }

                    // Eðer hala boþsa, yeni tile oluþtur
                    if (_tiles[x, y] == null)
                    {
                        CreateNewTile(x, y);
                    }
                }
            }
        }

        yield return new WaitForSeconds(0.4f); // Tüm düþme animasyonlarýnýn bitmesini bekle
        _isFalling = false;
        StartCoroutine(DestroyRoutine()); // Yeni eþleþmeleri yok et
    }
    private void CreateNewTile(int x, int y)
    {
        // Yeni tile'ý üstten baþlatma pozisyonu
        Vector3 startPos = new Vector3(x, _gridY + 1, 0);  // baþlangýç Poz yukarýdan düþmesi 1 yukarýs
        Vector3 targetPos = new Vector3(x, y, 0); // hedef poz da boþalttýðýmýz yer

        // Rastgele tile seç ve oluþtur
        int randomIndex = Random.Range(0, _tileObject.Length);
        GameObject newTileObj = Instantiate(_tileObject[randomIndex], startPos, Quaternion.identity);
        newTileObj.transform.SetParent(transform);

        Tile newTile = newTileObj.GetComponent<Tile>();
        newTile.Initialize(x, y, this);
        _tiles[x, y] = newTile;

        // Düþme animasyonu
        newTileObj.transform.DOMove(targetPos, 0.3f).SetEase(Ease.OutBounce);
    }

    private void HasAnyMatches()
    {
        List<Tile> matchedTile = _checkMatch.FindTileMatches(_tiles, _gridX, _gridY);

        if (matchedTile.Count > 0)
        {
            foreach (Tile tile in matchedTile)
            {
                DestroyAnim(tile);
            }
            StartCoroutine(RainDownRoutine());
        }
    }

    IEnumerator DestroyRoutine()
    {
        if (_isSwapping)
        {
            yield return null;
        }
        yield return new WaitForSeconds(0.1f);
        HasAnyMatches();

    }

    private void DestroyAnim(Tile tile)
    {
        tile.transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.Flash);
        tile.GetComponent<SpriteRenderer>().DOFade(0.2f, 0.2f).SetEase(Ease.Flash)
            .OnComplete(() =>
            {
                Destroy(tile);
            });
    }
    private int HasMatchStart(int x, int y)
    {
        int leftTileId;
        if (x > 0)
        {
            leftTileId = _tiles[x - 1,y].TileID;
        }
        else
        {
            leftTileId = -1;
        }
        int bottomTileId = (y > 0) ? _tiles[x, y - 1].TileID : -1;

        int randomIndex = Random.Range(0,_tileObject.Length);

        int currentId = _tileObject[randomIndex].GetComponent<Tile>().TileID;
        if (currentId == leftTileId || currentId == bottomTileId)
        {
            randomIndex = (randomIndex + 1) % _tileObject.Length;
            // Eðer seçilen tile sol veya alt tile ile aynýysa, bir sonraki tile'ý seç

        }
        return randomIndex;
    }

 

    private void SwapTiles()
    {
        if (_isFalling)
        {
            DeSelectTile(_selectedTiles[0]);  //isFalling True olunca seçimleri sýfýrla
            DeSelectTile(_selectedTiles[1]);
            return;
        }

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

            Vector3 firstPos = _selectedTiles[0].transform.position;
            Vector3 secondPos = _selectedTiles[1].transform.position;

            SwapTilesInGrid();

            DOTween.Sequence()
           .Join(_selectedTiles[0].transform.DOMove(secondPos, _swapDuration).SetEase(swapEase))
           .Join(_selectedTiles[1].transform.DOMove(firstPos, _swapDuration).SetEase(swapEase))
           .OnComplete(() => CheckMatchAndHandle(firstPos, secondPos));

        }
    }
    private void SwapTilesInGrid()
    {
        // ilk Seçilen Tile ýn koord saklýyoruz

        int tile1X = _selectedTiles[0].GridX;
        int tile1Y = _selectedTiles[0].GridY;

        // Ýkinci Seçilen Tile koord Saklýyoruz

        int tile2X = _selectedTiles[1].GridX;
        int tile2Y = _selectedTiles[1].GridY;


        Tile tempTile = _tiles[tile1X, tile1Y];
        _tiles[tile1X, tile1Y] = _tiles[tile2X, tile2Y];
        _tiles[tile2X, tile2Y] = tempTile;
        // _tiles Arrayda ki Tilelarýn yerini deðiþtirdik

        //// Tilerlarýn grid Koord güncelliyoruz
        _selectedTiles[0].Initialize(tile2X, tile2Y, this);
        _selectedTiles[1].Initialize(tile1X, tile1Y, this);
    }

    private void CheckMatchAndHandle(Vector3 firstPos, Vector3 secondPos)
    {
        if (_checkMatch.FindTileMatches(_tiles, _gridX, _gridY).Count == 0)
        {
            // Eþleþme yoksa geri al
            SwapTilesInGrid(); // Grid'i eski haline getir

            // Pozisyonlarý geri al
            DOTween.Sequence()
                .Join(_selectedTiles[0].transform.DOShakePosition(0.2f, 0.1f))
                .Join(_selectedTiles[1].transform.DOShakePosition(0.2f, 0.1f))
                .Append(_selectedTiles[0].transform.DOMove(firstPos, _swapDuration))
                .Join(_selectedTiles[1].transform.DOMove(secondPos, _swapDuration))
                .OnComplete(() => {
                    _isSwapping = false;
                    DeSelectTile(_selectedTiles[0]);
                    DeSelectTile(_selectedTiles[1]);
                });
        }
        else
        {
            // Eþleþme varsa normal akýþa devam et
            _isSwapping = false;
            DeSelectTile(_selectedTiles[0]);
            DeSelectTile(_selectedTiles[1]);
            StartCoroutine(DestroyRoutine());
        }
    }


 
    private bool TileMoveCheck(Tile tile1, Tile tile2)
    {
        int xCheck = Mathf.Abs(tile1.GridX - tile2.GridX);
        // GridX - Tile classýndan Getter  iki tile arasýn da ki X poz Kontrol

        int yCheck = Mathf.Abs(tile1.GridY - tile2.GridY);
        // GridY - Tile class Getter iki tile arasý Y poz kontrol

        return (xCheck == 1 && yCheck == 0) || (xCheck == 0 && yCheck == 1);
        // iki grid arasýnda x veya y de 1 birim uzaktaysa hareket ettir
    }

    public void SelectedTile(Tile tile)
    {
        if (_isFalling) // isFalling true ise hareket edemezler
        {
            Debug.Log("not move is falling");
            return;
            
        }

        if (_selectedTiles[0] == tile)
        {
            DeSelectTile(tile);
        }
        else if (_selectedTiles[1] == tile)
        {
            DeSelectTile(tile);

        }
        //Seçim varsa temizliyor
        else
        {
            if (_selectedTiles[0] == null)
            {
                _selectedTiles[0] = tile;
                GameEvent.SelectsTile?.Invoke(tile);

            }
            else if (_selectedTiles[1] == null)
            {
                _selectedTiles[1] = tile;
                GameEvent.SelectsTile?.Invoke(tile);
                Debug.Log("2 Tile seçildi");
                SwapTiles();
            }
        }
        // yoksa ve boþsa Tile olarak tanýmlýyor seçim ekliyor
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


    private void OnEnable() => RegisterEvents();
    private void OnDisable() => UnRegisterEvents();

    private void RegisterEvents() => GameEvent.OnClickEvents += SelectedTile;
    private void UnRegisterEvents() => GameEvent.OnClickEvents -= SelectedTile;
    // Mouse Sol Týk Eventi.


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
