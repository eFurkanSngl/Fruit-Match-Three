using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(CircleCollider2D))]
public class Tile : MonoBehaviour
{
    private Color _originColor;   // Start Color 
    private SpriteRenderer _spriteRenderer;  
    private int _gridX, _gridY; // Tilelarýn Grdi üzerinde ki yeri 
    private GridManager _gridManager;
    [SerializeField] private int _tileId;

    public int GridX => _gridX;  // Getter ile deðere dýþardan eriþim
    public int GridY => _gridY;
    public int TileID => _tileId;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _originColor = _spriteRenderer.color;
        _gridManager = FindObjectOfType<GridManager>();
    }

 
    private void OnMouseDown()
    {
       GameEvent.OnClickEvents?.Invoke(this);
        // Mouse Sol Týk Events
    }

    public void Initialize(int x,int y ,GridManager gridManager)
    {
        _gridX = x;
        _gridY = y;
        _gridManager= gridManager;
    }
    private void SelectTile(Tile tile)
    {
        if(tile == this)  // Param olarak aldýðý Tile'ý gri yap
        {
            _spriteRenderer.DOFade(0.5f,0.2f).SetLoops(2,LoopType.Yoyo);

        }
    }

    private void DeSelectedTile(Tile tile) // Param olarak aldýðý Tile' eski rengi yap
    {
        _spriteRenderer.DOFade(1f, 0.2f);

    }


    private void OnEnable() => RegisterEvents();
    private void OnDisable() => UnRegisterEvents();

    private void RegisterEvents()
    {
        GameEvent.UnSelectsTile += DeSelectedTile;
        GameEvent.SelectsTile += SelectTile;
    }
    private void UnRegisterEvents()
    {
        GameEvent.UnSelectsTile -= DeSelectedTile;
        GameEvent.SelectsTile -= SelectTile;
    }
    //  Seçim SOnrasý rengi eski haline getirme Eventi




}
