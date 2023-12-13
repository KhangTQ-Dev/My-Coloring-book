using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GetUv
{
    public static Vector2 GetUvFrom(Vector2 point, Vector2 pivot, Vector2 size)
    {
        Vector2 distance = pivot - point;

        distance.x = Mathf.Abs(distance.x);

        distance.y = Mathf.Abs(distance.y);

        size.x = size.x / 2;

        size.y = size.y / 2;

        float x = distance.x / size.x;

        x = x * 0.5f;

        x = 1 - x;

        float y = distance.y / size.y;

        y = y * 0.5f;

        y = 1 - y;

        return new Vector2(x, y);
    }
}
