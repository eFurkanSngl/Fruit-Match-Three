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
    private int _gridX, _gridY; // Tilelar�n Grdi �zerinde ki yeri 
    private GameManager _gameManager;
    [SerializeField] private int _tileId;

    public int GridX => _gridX;  // Getter ile de�ere d��ardan eri�im
    public int GridY => _gridY;
    public int TileID => _tileId;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _originColor = _spriteRenderer.color;
        _gameManager = FindObjectOfType<GameManager>();

    }

    private void OnMouseDown()
    {
       GameEvent.OnClickEvents?.Invoke(this);
        // Mouse Sol T�k Events
    }

    public void Initialize(int x,int y ,GameManager gameManager)
    {
        _gridX = x;
        _gridY = y;
        _gameManager= gameManager;
    }
    private void SelectTile(Tile tile)
    {
        if(tile == this)  // Param olarak ald��� Tile'� gri yap
        {
            _spriteRenderer.color = Color.white;  // Se�ili Ta�� gri yapt�k

        }
    }

    private void DeSelectedTile(Tile tile) // Param olarak ald��� Tile' eski rengi yap
    {
           _spriteRenderer.color = _originColor;

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
    //  Se�im SOnras� rengi eski haline getirme Eventi




}
