using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RailType { None, Horizontal, Vertical, TopRight, RightBottom, BottomLeft, LeftTop }

public static class RailTypeExtensions
{
    public static bool IsStraight(this RailType railType)
    {
        return railType == RailType.Horizontal || railType == RailType.Vertical;
    }

    public static bool IsTurn(this RailType railType)
    {
        return railType == RailType.TopRight || railType == RailType.RightBottom || railType == RailType.BottomLeft || railType == RailType.LeftTop;
    }

    public static Direction Inverse(this Direction direction)
    {
        switch (direction)
        {
            case Direction.Left:
                return Direction.Right;
            case Direction.Top:
                return Direction.Bottom;
            case Direction.Right:
                return Direction.Left;
            case Direction.Bottom:
                return Direction.Top;
        }
        return Direction.None;
    }
}