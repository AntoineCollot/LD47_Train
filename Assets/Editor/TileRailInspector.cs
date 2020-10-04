using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TileRail))]
public class TileRailInspector : Editor
{
    TileRail rail;
    const int SAMPLES = 20;

    private void OnSceneGUI()
    {
        rail = target as TileRail;
        Draw();
    }

    private void Draw()
    {
        //Direction
        Handles.color = new Color(0,1,0,0.2f);
        for (int i = 0; i <= SAMPLES; i++)
        {
            Vector3 point = rail.GetPoint((float)i / SAMPLES);
            Vector3 dir = (rail.GetDirection((float)i / SAMPLES));
            Handles.DrawLine(point, dir * 0.2f + point);
        }

        //Spline
        Handles.color = Color.blue;
        for (int i = 0; i <= SAMPLES-1; i++)
        {
            Handles.DrawLine(rail.GetPoint((float)i/SAMPLES), rail.GetPoint((float)(i+1) / SAMPLES));
        }
    }
}
