using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class GridBackground : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private float _padding = 0.5f;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void AdjustGridBg(int gridWidth,int gridHeight)
    {
        if(_spriteRenderer == null)
        {
            Debug.Log("Dow A spirte ");
        }

        transform.position = new Vector3(gridWidth / 2f - 0.5f, gridHeight / 2f - 0.5f,1f);
        _spriteRenderer.size = new Vector3(gridWidth + _padding, gridHeight + _padding,1);
    }

    private void OnEnable() => RegisterEvents();
    private void OnDisable() => UnRegisterEvents();
    private void RegisterEvents() => GridUIEvents.GridEvents += AdjustGridBg;
    private void UnRegisterEvents() => GridUIEvents.GridEvents -= AdjustGridBg;
}
