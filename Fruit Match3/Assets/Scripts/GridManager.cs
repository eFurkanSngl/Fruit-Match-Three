using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;
using static UnityEngine.Rendering.DebugUI.Table;
using TMPro;

public class GridManager : MonoBehaviour
{
    public static event UnityAction<int, int> GridManagerEvent;

    [Header("Grid Settings")]
    [SerializeField] private int _gridX;
    [SerializeField] private int _gridY;


    [Header("Tile Settings")]
    [SerializeField] private GameObject[] _tileObject;   // Prefab
    [SerializeField] private GameObject _tileBg;  // Tile bg
    [SerializeField] private float _swapDuration = 3f;  // anim s�re
    [SerializeField] private Ease swapEase = Ease.OutQuad; // anim tipi
    

    private Tile[,] _tiles;   // Tilelar� tutan 2D bir array
    private Tile[] _selectedTiles = new Tile[2];  // Se�ilen Ta�lar� Tutan bir array
    private bool _isSwapping = false;
    private bool _isFalling = false;
    private GameObject[,] _bgTiles; // Tile BG leri tutan liste
    private CheckMatches _checkMatch;
    private bool _isShowHint = false;
    private Coroutine _hintCoroutine;
    private Coroutine _stopCoroutine;
    private int _multiMatch = 0;

    [Header("UI Settings")]
    [SerializeField] private GameObject _destoryEffect;
    //[SerializeField] private TextMeshProUGUI _combotext;

    //private int _comboCount = 0;
    //private float _comboTimer = 0;
    //private float _comboResetTime = 2f;
    //private bool _isComboActive = false;

    [Header("Power Up Settings")]
    [SerializeField] private GameObject _horizontalPowerUpPrefab;
    [SerializeField] private GameObject _verticalPowerUpPrefab;
    [SerializeField] private GameObject _bombPowerUpPrefab;
    [SerializeField] private Animator _anim;
    [SerializeField] private AudioSource _sfx;
    [SerializeField] private AudioSource _bombsfx;

    private void Start()
    {
        CreateGrid();
        CreateTileBackground();
        DOTween.SetTweensCapacity(500, 50);
        _checkMatch =  GetComponent<CheckMatches>();
        StartHintCoroutine();
    }

    private void GenerateNewGrid() // Eski Tilelar� silip yeni tile yarat�yoruz
    {
        for(int i = 0; i <_gridX; i++)
        {
            for(int y = 0; y < _gridY; y++)
            {
                if(_tiles[i,y] != null)
                {
                    Destroy(_tiles[i,y].gameObject);
                }
            }
        }
        CreateGrid();
    }
    private void HandlePowerUpMatch(int matchCount)//multiMatchden al�yor say�m�
    {
        if (matchCount >= 4 && matchCount <= 6) // bu iki say� aras�nda ise e�le�me powerUp
        {
            List<Tile> matchedTiles = _checkMatch.FindTileMatches(_tiles, _gridX, _gridY);

            if (matchedTiles.Count == 0) return;

            Tile selectedTile = matchedTiles[Random.Range(0, matchedTiles.Count)];
            // e�le�melerden Random Tile se�iyoruz

            PowerUpType powerUp = PowerUpType.None; // powerUp tipi
            GameObject powerUpPrefab = null;  // buda yarat�lacak obje

            if(matchCount == 4)
            {
                Debug.Log("4l� e�le�me Horz Power Up");
                powerUp = PowerUpType.Horizontal;
                powerUpPrefab = _horizontalPowerUpPrefab;
            }
            else if(matchCount == 5)
            {
                Debug.Log("5li e�le�me vertical");
                powerUp = PowerUpType.Vertical;
                powerUpPrefab= _verticalPowerUpPrefab;
            }
            else if(matchCount == 6)
            {
                Debug.Log("6l� e�le�me bomb");
                powerUp = PowerUpType.Bomb;
                powerUpPrefab=_bombPowerUpPrefab;
            }
            //buralarda say�ya g�re powerUp tipini veriyoruz ve Prefabi ona g�re g�ncellliyoruz

            if(powerUp != PowerUpType.None && powerUpPrefab != null)
            {
                Vector3 pos = selectedTile.transform.position;
                Destroy(selectedTile.gameObject);
                //eski Tile yok ediyoruz 

                GameObject obj = Instantiate(powerUpPrefab,pos,Quaternion.identity);
                Tile powerUpTile = obj.GetComponent<Tile>();


                powerUpTile.Initialize(selectedTile.GridX,selectedTile.GridY,this);
                powerUpTile.SetPowerUp(powerUp);

                _tiles[selectedTile.GridX,selectedTile.GridY] = powerUpTile;
            } 
        }
    }

    public void ClearRow(int row)
    {
        for(int i= 0; i <_gridX;i++)
        {
            if (_tiles[i,row] != null)
            {
                Destroy(_tiles[i,row].gameObject);
                ScoreEvents.GameScoreEvents?.Invoke(5);
                GameUIEvents.TimerUI?.Invoke(1f);
                DestroyAnim(_tiles[i, row]);
                TileDestroySound();
                _tiles[i,row] = null;
                _sfx.Play();
            }
        }
    }

    public void ClearColumn(int column)
    {
        for (int y = 0; y < _gridY; y++)
        {
            if (_tiles[column, y] != null)
            {
                Destroy(_tiles[column, y].gameObject);
                ScoreEvents.GameScoreEvents?.Invoke(5);
                GameUIEvents.TimerUI?.Invoke(1f);
                DestroyAnim(_tiles[column, y]);
                TileDestroySound();
                _tiles[column, y] = null;
                _sfx.Play();

            }
        }
    }
    public void ClearBombTiles(int x, int y)
    {
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                int newX = x + i;
                int newY = y + j;

                if (newX >= 0 && newX < _gridX && newY >= 0 && newY < _gridY)
                {
                    if (_tiles[newX, newY] != null)
                    {
                        Destroy(_tiles[newX, newY].gameObject);
                        ScoreEvents.GameScoreEvents?.Invoke(5);
                        GameUIEvents.TimerUI?.Invoke(1f);
                        DestroyAnim(_tiles[newX, newY]);
                        TileDestroySound();
                        _tiles[newX, newY] = null;
                        _bombsfx.Play();
                    }
                }
            }
        }
    }
    public void StopHintRoutine()
    {
        if(_hintCoroutine != null)
        {
            StopCoroutine(RainDownRoutine());
            _hintCoroutine = null;
        }
        _isShowHint = false;
    }
    public void StartHintCoroutine()
    {
        if (_hintCoroutine != null)
        {
            StopCoroutine(_hintCoroutine);
        }
        _hintCoroutine = StartCoroutine(HintRoutine());
    }

    private IEnumerator HintRoutine()
    {
        _isShowHint = false;
        float timer = 0f;
        while (timer < 3f)
        {
            if (_isFalling)
            {
                timer = 0f;
            }
            else
            {
                timer += Time.deltaTime;
            }
            yield return null;
        }
        _isShowHint = true;
        ShowHint();
    }

    private void ShowHint()
    {
        if (_isShowHint && !_isFalling)
        {
            Tile hintTile = _checkMatch.FindHint(_tiles, _gridX, _gridY);
            if (hintTile != null)
            {
                hintTile.transform.DOShakePosition(0.5f, 0.3f);
                Debug.Log("Show Hint");
            }
            else
            {
                Debug.Log("Not can show hint");
            }
        }
    }


    public void CreateGrid()
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
        //GridUIEvents.GridEvents?.Invoke(_gridX, _gridY);
        //CameraFilterEvents.CameraEvents?.Invoke(_gridX,_gridY);
        //GridUIEvents.GridBorderEvents?.Invoke(_gridX, _gridY);
        
        GridManagerEvent?.Invoke(_gridX,_gridY);
        //Event kullan�p birden fazla olay� dinledik ve tetikledik hepsi s�rayla �al��t�..
       
    }
    private void CreateTileBackground()
    {
        _bgTiles = new GameObject[_gridX,_gridY]; // Gride g�re bg leri tutacaz

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

    public IEnumerator RainDownRoutine()
    {
        _isFalling = true;
        yield return new WaitForSeconds(0.3f); // Yok olma animasyonunun bitmesini bekle

        // Her s�tun i�in kontrol
        for (int x = 0; x < _gridX; x++) // S�t�nlar� say�yoruz
        {
            for (int y = 0; y < _gridY; y++)  // her s�t�n� kontrol 
            {
                if (_tiles[x, y] == null) // Bo� bir tile bulursak Gridde 
                {
                    for (int newY = y + 1; newY < _gridY; newY++) // grid7 den ba�l�cak yukar� do�ru kotnrol
                    {
                        Tile newMoveTile = _tiles[x, newY]; // yukar�da ki bakt���m�z Tile
                        if (newMoveTile != null)
                        {
                            // Tile'� a�a�� ta��
                            _tiles[x, newY] = null; // poz bo�alt�yoruz
                            _tiles[x, y] = newMoveTile; // Yeni poz bo�altt���m�z yere at�yoruz
                            newMoveTile.Initialize(x, y, this);

                            // D��me animasyonu
                            Vector3 targetPos = new Vector3(x, y, 0);
                            newMoveTile.transform.DOMove(targetPos, 0.3f).SetEase(Ease.OutBounce);//OutBounce

                            break;
                        }
                    }

                    // E�er hala bo�sa, yeni tile olu�tur
                    if (_tiles[x, y] == null)
                    {
                        CreateNewTile(x, y);
                    }
                }
            }
        }

        yield return new WaitForSeconds(0.4f); // T�m d��me animasyonlar�n�n bitmesini bekle
        _isFalling = false;
        StartCoroutine(DestroyRoutine()); // Yeni e�le�meleri yok et
        StartHintCoroutine();
    }
    private void CreateNewTile(int x, int y)
    {
        // Yeni tile'� �stten ba�latma pozisyonu
        Vector3 startPos = new Vector3(x, _gridY + 1, 0);  // ba�lang�� Poz yukar�dan d��mesi 1 yukar�s
        Vector3 targetPos = new Vector3(x, y, 0); // hedef poz da bo�altt���m�z yer

        // Rastgele tile se� ve olu�tur
        int randomIndex = Random.Range(0, _tileObject.Length);
        GameObject newTileObj = Instantiate(_tileObject[randomIndex], startPos, Quaternion.identity);
        newTileObj.transform.SetParent(transform);

        Tile newTile = newTileObj.GetComponent<Tile>();
        newTile.Initialize(x, y, this);
        _tiles[x, y] = newTile;

        // D��me animasyonu
        newTileObj.transform.DOMove(targetPos, 0.3f).SetEase(Ease.OutBounce);
    }
  
    public void HasAnyMatches() // E�le�me var ise yok et
    {
        List<Tile> matchedTile = _checkMatch.FindTileMatches(_tiles, _gridX, _gridY);

        if (matchedTile.Count > 0)
        {
            foreach (Tile tile in matchedTile)
            {
                DestroyAnim(tile);
                ScoreEvents.GameScoreEvents?.Invoke(5);
                TileDestroySound();

            }

            _multiMatch++;
            HandlePowerUpMatch(_multiMatch);
            StartCoroutine(RainDownRoutine());
        }
        else
        {
            _multiMatch = 0;
        }
    }
    public void TileDestroySound()
    {
      
        CameraEvents.CameraEvent?.Invoke();
        // Tek bir yerden dinleyip Tetikleme performans art���.
    }

    IEnumerator DestroyRoutine()
    {
        if (_isSwapping)
        {
            yield return null;
        }
        yield return new WaitForSeconds(0.1f);
        HasAnyMatches();
        GameUIEvents.TimerUI?.Invoke(1f);


    }

    private void DestroyAnim(Tile tile)
    {

        if (_destoryEffect != null)
        {
            GameObject effect = Instantiate(_destoryEffect, tile.transform.position, Quaternion.identity);
            ParticleSystem ps = effect.GetComponent<ParticleSystem>();
            if (ps != null)
            {
                ps.Play();
            }

            Destroy(effect, 1f);
        }
     
        tile.transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.Flash);
        tile.GetComponent<SpriteRenderer>().DOFade(0.2f, 0.2f).SetEase(Ease.Flash)
            .OnComplete(() =>
            {
                Destroy(tile.gameObject);
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
            // E�er se�ilen tile sol veya alt tile ile ayn�ysa, bir sonraki tile'� se�

        }
        return randomIndex;
    }

 

    private void SwapTiles()
    {
        if (_isFalling || _isSwapping)
        {
            DeSelectTile(_selectedTiles[0]);  //isFalling True olunca se�imleri s�f�rla
            DeSelectTile(_selectedTiles[1]);
            return;
        }

        if (_selectedTiles[0]!=null && _selectedTiles[1] != null)
        {
            if (!TileMoveCheck(_selectedTiles[0], _selectedTiles[1])) // ta�lar� kom�u diye kontrol ediyor
            {
               
                DeSelectTile(_selectedTiles[0]); //_selectedTiles Tile class�ndan t�r�yor.
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
        // ilk Se�ilen Tile �n koord sakl�yoruz

        int tile1X = _selectedTiles[0].GridX;
        int tile1Y = _selectedTiles[0].GridY;

        // �kinci Se�ilen Tile koord Sakl�yoruz

        int tile2X = _selectedTiles[1].GridX;
        int tile2Y = _selectedTiles[1].GridY;


        Tile tempTile = _tiles[tile1X, tile1Y];
        _tiles[tile1X, tile1Y] = _tiles[tile2X, tile2Y];
        _tiles[tile2X, tile2Y] = tempTile;
        // _tiles Arrayda ki Tilelar�n yerini de�i�tirdik

        //// Tilerlar�n grid Koord g�ncelliyoruz
        _selectedTiles[0].Initialize(tile2X, tile2Y, this);
        _selectedTiles[1].Initialize(tile1X, tile1Y, this);
    }

    private void CheckMatchAndHandle(Vector3 firstPos, Vector3 secondPos)
    {
        if (_checkMatch.FindTileMatches(_tiles, _gridX, _gridY).Count == 0)
        {
            // E�le�me yoksa geri al
            SwapTilesInGrid(); // Grid'i eski haline getir

            // Pozisyonlar� geri al
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
            // E�le�me varsa normal ak��a devam et
            _isSwapping = false;
            DeSelectTile(_selectedTiles[0]);
            DeSelectTile(_selectedTiles[1]);
            StartCoroutine(DestroyRoutine());
        }
    }


 
    private bool TileMoveCheck(Tile tile1, Tile tile2)
    {
        int xCheck = Mathf.Abs(tile1.GridX - tile2.GridX);
        // GridX - Tile class�ndan Getter  iki tile aras�n da ki X poz Kontrol

        int yCheck = Mathf.Abs(tile1.GridY - tile2.GridY);
        // GridY - Tile class Getter iki tile aras� Y poz kontrol

        return (xCheck == 1 && yCheck == 0) || (xCheck == 0 && yCheck == 1);
        // iki grid aras�nda x veya y de 1 birim uzaktaysa hareket ettir
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
        //Se�im varsa temizliyor
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
                Debug.Log("2 Tile se�ildi");
                SwapTiles();
            }
        }
        // yoksa ve bo�sa Tile olarak tan�ml�yor se�im ekliyor
    }

    public void DeSelectTile(Tile tile)
    {
        // Se�im �st�n de ki rengi kald�r 

        if (tile != null) // E�er bir se�im varsa   
        {
            GameEvent.UnSelectsTile?.Invoke(tile);  // Rengi orj haline getirmek i�in.

            if (_selectedTiles[0]== tile) // ilk index de bir tile varsa 
            {
                _selectedTiles[0] = null;  // bo�alt
            }
            else if (_selectedTiles[1]== tile)
            {
                _selectedTiles[1] = null;
            }
        }
    }


    private void OnEnable()
    {
        RegisterEvents();
    }
    private void OnDisable()
    {
        UnRegisterEvents();
    }

    private void RegisterEvents()
    {
        GameEvent.OnClickEvents += SelectedTile;
        RoutineEvents.StartRoutineEvent += StartHintCoroutine;
        RoutineEvents.StopRoutineEvent += StopHintRoutine;
        GameEvent.ShuffleEvents += GenerateNewGrid;

    } 
    private void UnRegisterEvents()
    { 
       GameEvent.OnClickEvents -= SelectedTile;
       RoutineEvents.StartRoutineEvent -= StartHintCoroutine;
       RoutineEvents.StopRoutineEvent -= StopHintRoutine;
        GameEvent.ShuffleEvents -= GenerateNewGrid; 
    } 
    // Mouse Sol T�k Eventi.


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
