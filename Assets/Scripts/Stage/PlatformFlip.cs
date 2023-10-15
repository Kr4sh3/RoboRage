using System.Collections;
using System.Collections.Generic;
using Mono.CSharp;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlatformFlip : MonoBehaviour
{
    private PlatformEffector2D _platformEffector;
    // Start is called before the first frame update
    void Start()
    {
        _platformEffector = GetComponent<PlatformEffector2D>();
        //Copy parent tilemap
        Tilemap parentTilemap = transform.parent.GetComponent<Tilemap>();
        for (int x = parentTilemap.cellBounds.xMin; x < parentTilemap.cellBounds.xMax; x++)
        {
            for (int y = parentTilemap.cellBounds.yMin; y < parentTilemap.cellBounds.yMax; y++)
            {
                Vector3Int position = new Vector3Int(x, y, 0);
                gameObject.GetComponent<Tilemap>().SetTile(position, parentTilemap.GetTile(position));
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.InputManager.DownHeld)
            _platformEffector.surfaceArc = 0;
        else
            _platformEffector.surfaceArc = 160;
    }
}
