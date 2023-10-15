using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum Direction
{
    Up, Down, Right, Left
}

public class InnerStageController : MonoBehaviour
{
    public int Difficulty;

    private static Vector3 _stageLeft = new Vector3(-35, 0, 0);
    private static Vector3 _stageRight = new Vector3(35, 0, 0);
    private static Vector3 _stageUp = new Vector3(0, 25, 0);
    private static Vector3 _stageDown = new Vector3(0, -25, 0);
    public static float transitionLerpSpeed = 3;
    public Direction StartDirection { get; private set; }
    private Vector3 _target;
    private bool _entering = false; // Are we currently exiting the stage
    private bool _ready = false; // Ready to spawn enemies and stuff
    void Awake()
    {
        StartDirection = GetRandomDirection();
        _target = Vector3.zero;
    }

    void Start()
    {
        //Init Stage Components
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<Tilemap>().color = Color.gray;
            transform.GetChild(i).GetComponent<TilemapCollider2D>().enabled = false;
        }
        //Move ourselves to random starting position
        transform.position = DirectionToWorldPoint(StartDirection);
    }

    void Update()
    {
        //When we're ready to advance onto the stage
        if (GameManager.Instance.WaveManager.GetCurrentStage() == this && !_entering)
        {
            LeanTween.move(gameObject, _target, GameManager.Instance.WaveManager.TimeToTransitionTransform).setOnComplete(() =>
                        {
                            LeanTween.value(gameObject, setColorCallback, transform.GetChild(0).GetComponent<Tilemap>().color, Color.white, GameManager.Instance.WaveManager.TimeToTransitionColor).setOnComplete(() =>
                    {
                        _ready = true;
                        for (int i = 0; i < transform.childCount; i++)
                            transform.GetChild(i).GetComponent<TilemapCollider2D>().enabled = true;
                    });
                        });
            _entering = true;
        }
    }

    private void setColorCallback(Color c)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<Tilemap>().color = c;
        }
    }

    public void ExitStage(Direction exitDirection)
    {
        _target = DirectionToWorldPoint(exitDirection);
        _ready = false;
        //Disable Colliders
        for (int i = 0; i < transform.childCount; i++)
            transform.GetChild(i).GetComponent<TilemapCollider2D>().enabled = false;
        //Tween colors
        LeanTween.value(gameObject, setColorCallback, transform.GetChild(0).GetComponent<Tilemap>().color, Color.gray, GameManager.Instance.WaveManager.TimeToTransitionColor).setOnComplete(() =>
        {
            //Advance stage, tween to position and destroy self
            GameManager.Instance.WaveManager.AdvanceStage();
            LeanTween.move(gameObject, _target, GameManager.Instance.WaveManager.TimeToTransitionTransform).setDestroyOnComplete(true);
        });
    }

    public static Vector3 DirectionToWorldPoint(Direction stageDirection)
    {
        if (stageDirection == Direction.Up)
            return _stageUp;
        else
        if (stageDirection == Direction.Down)
            return _stageDown;
        else
        if (stageDirection == Direction.Right)
            return _stageRight;
        else
            return _stageLeft;
    }

    public static Direction GetRandomDirection()
    {
        Direction[] values = new Direction[4] { Direction.Up, Direction.Down, Direction.Right, Direction.Left };
        return values[UnityEngine.Random.Range(0, 4)];
    }
    public static Direction GetOppositeDirection(Direction direction)
    {
        if (direction == Direction.Up)
            return Direction.Down;
        else
        if (direction == Direction.Down)
            return Direction.Up;
        else
        if (direction == Direction.Right)
            return Direction.Left;
        else
            return Direction.Right;
    }
}
