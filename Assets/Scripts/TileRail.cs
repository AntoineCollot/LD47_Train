using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TileRail : MonoBehaviour
{
    public RailType type;

    public class TypeEvent : UnityEvent<RailType> { }
    public TypeEvent onTypeChanged = new TypeEvent();

    [Header("States")]
    public EvolvingState somethingOnRailState = new EvolvingState();
    public EvolvingState lockChangeRailTypeState = new EvolvingState();
    public EvolvingState hasHazardState = new EvolvingState();

    [Header("Bezier")]
    [SerializeField] float curveSmooth = 1;

    public Vector3 Left { get => transform.position + Vector3.left * 0.5f * TileMap.TIlE_SIZE; }
    public Vector3 Top { get => transform.position + Vector3.forward * 0.5f * TileMap.TIlE_SIZE; }
    public Vector3 Right { get => transform.position + Vector3.right * 0.5f * TileMap.TIlE_SIZE; }
    public Vector3 Bottom { get => transform.position - Vector3.forward * 0.5f * TileMap.TIlE_SIZE; }

    public Vector3 PointStart
    {
        get
        {
            Vector3 startPointPos;
            switch (type)
            {
                case RailType.None:
                default:
                    startPointPos = transform.position;
                    break;
                case RailType.Vertical:
                case RailType.BottomLeft:
                    startPointPos = Bottom;
                    break;
                case RailType.TopRight:
                    startPointPos = Top;
                    break;
                case RailType.RightBottom:
                    startPointPos = Right;
                    break;
                case RailType.LeftTop:
                case RailType.Horizontal:
                    startPointPos = Left;
                    break;
            }
            return startPointPos;
        }
    }
    public Vector3 PointEnd
    {
        get
        {
            Vector3 endPointPos;
            switch (type)
            {
                case RailType.None:
                default:
                    endPointPos = transform.position;
                    break;
                case RailType.Horizontal:
                case RailType.TopRight:
                    endPointPos = Right;
                    break;
                case RailType.Vertical:
                case RailType.LeftTop:
                    endPointPos = Top;
                    break;
                case RailType.RightBottom:
                    endPointPos = Bottom;
                    break;
                case RailType.BottomLeft:
                    endPointPos = Left;
                    break;
            }
            return endPointPos;
        }
    }
    public Vector3 ControlPointStart
    {
        get
        {
            Vector3 startCPPos;
            switch (type)
            {
                case RailType.None:
                default:
                    startCPPos = transform.position;
                    break;
                case RailType.Vertical:
                case RailType.BottomLeft:
                    startCPPos = Bottom + Vector3.forward * curveSmooth;
                    break;
                case RailType.TopRight:
                    startCPPos = Top - Vector3.forward * curveSmooth;
                    break;
                case RailType.RightBottom:
                    startCPPos = Right + Vector3.left * curveSmooth;
                    break;
                case RailType.Horizontal:
                case RailType.LeftTop:
                    startCPPos = Left + Vector3.right * curveSmooth;
                    break;
            }
            return startCPPos;
        }
    }
    public Vector3 ControlPointEnd
    {
        get
        {
            Vector3 endCPPos;
            switch (type)
            {
                case RailType.None:
                default:
                    endCPPos = transform.position;
                    break;
                case RailType.Horizontal:
                case RailType.TopRight:
                    endCPPos = Right + Vector3.left * curveSmooth;
                    break;
                case RailType.Vertical:
                case RailType.LeftTop:
                    endCPPos = Top - Vector3.forward * curveSmooth;
                    break;
                case RailType.RightBottom:
                    endCPPos = Bottom + Vector3.forward * curveSmooth;
                    break;
                case RailType.BottomLeft:
                    endCPPos = Left + Vector3.right * curveSmooth;
                    break;
            }
            return endCPPos;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        lockChangeRailTypeState.Add(somethingOnRailState);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public Vector3 GetPoint(float t)
    {
        return Bezier.GetPoint(PointStart, ControlPointStart, ControlPointEnd, PointEnd, t);
    }

    public Vector3 GetVelocity(float t)
    {
        return Bezier.GetFirstDerivative(PointStart, ControlPointStart, ControlPointEnd, PointEnd, t);
    }

    public Vector3 GetDirection(float t)
    {
        return GetVelocity(t).normalized;
    }

    public Direction GetOutputDirection(Direction inputDirection)
    {
        switch (inputDirection)
        {
            case Direction.Left:
                switch (type)
                {
                    case RailType.Horizontal:
                        return Direction.Right;
                    case RailType.LeftTop:
                        return Direction.Top;
                    case RailType.BottomLeft:
                        return Direction.Bottom;
                }
                break;
            case Direction.Top:
                switch (type)
                {
                    case RailType.Vertical:
                        return Direction.Bottom;
                    case RailType.TopRight:
                        return Direction.Right;
                    case RailType.LeftTop:
                        return Direction.Left;
                }
                break;
            case Direction.Right:
                switch (type)
                {
                    case RailType.TopRight:
                        return Direction.Top;
                    case RailType.RightBottom:
                        return Direction.Bottom;
                    case RailType.Horizontal:
                        return Direction.Left;
                }
                break;
            case Direction.Bottom:
                switch (type)
                {
                    case RailType.RightBottom:
                        return Direction.Right;
                    case RailType.Vertical:
                        return Direction.Top;
                    case RailType.BottomLeft:
                        return Direction.Left;
                }
                break;
        }

        return Direction.None;
    }

    public bool IsGoingReverse(Direction inputDirection)
    {
        switch (inputDirection)
        {
            case Direction.Left:
                switch (type)
                {
                    case RailType.Horizontal:
                    case RailType.LeftTop:
                        return false;
                    case RailType.BottomLeft:
                        return true;
                }
                break;
            case Direction.Top:
                switch (type)
                {
                    case RailType.Vertical:
                    case RailType.LeftTop:
                        return true;
                    case RailType.TopRight:
                        return false;
                }
                break;
            case Direction.Right:
                switch (type)
                {
                    case RailType.RightBottom:
                        return false;
                    case RailType.Horizontal:
                    case RailType.TopRight:
                        return true;
                }
                break;
            case Direction.Bottom:
                switch (type)
                {
                    case RailType.RightBottom:
                        return true;
                    case RailType.Vertical:
                    case RailType.BottomLeft:
                        return false;
                }
                break;
        }
        return false;
    }

    /// <summary>
    /// Called by messages.
    /// </summary>
    void OnLeftClick()
    {
        if(!lockChangeRailTypeState.IsOn)
            NextType();
    }

    /// <summary>
    /// Called by messages.
    /// </summary>
    void OnRightClick()
    {
        if (!lockChangeRailTypeState.IsOn)
            PreviousType();
    }

    /// <summary>
    /// Called by messages.
    /// </summary>
    void OnCancelClick()
    {
        if (!lockChangeRailTypeState.IsOn)
            CancelType();
    }

    public void NextType()
    {
        type = (RailType)(((int)type + 1)%(System.Enum.GetNames(typeof(RailType)).Length));
        onTypeChanged.Invoke(type);
    }

    public void PreviousType()
    {
        int newId = (int)type - 1;
        if (newId < 0)
            newId = System.Enum.GetNames(typeof(RailType)).Length;
        type = (RailType)newId;
        onTypeChanged.Invoke(type);
    }

    public void CancelType()
    {
        type = RailType.None;
        onTypeChanged.Invoke(type);
    }

    public void SetType(RailType type)
    {
        this.type = type;
        onTypeChanged.Invoke(this.type);
    }
}