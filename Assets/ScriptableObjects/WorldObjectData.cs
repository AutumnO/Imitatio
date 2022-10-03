using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum ObjectType
{
    Prop, Building, Other
};

[CreateAssetMenu(fileName = "New World Object", menuName = "Assets/WorldObject")]
public class WorldObjectData : ScriptableObject
{
    public Sprite sprite;
    public ObjectType type;
    public bool[,] footprint;

    /*public void OnValidate()
    {
        Debug.Log(name);
        if (sprite != null)
        {
            footprint = new bool[(int)sprite.rect.width/16, (int)sprite.rect.height/16];
            for (int i = footprint.GetLowerBound(0); i <= footprint.GetUpperBound(0); i++)
                for (int j = footprint.GetLowerBound(1); j <= footprint.GetUpperBound(1); j++)
                    footprint[i, j] = false;
            
            for (int x = (int)sprite.rect.x; x < sprite.rect.xMax; x++)
            {
                for (int y = (int)sprite.rect.y; y < sprite.rect.yMax; y++)
                {
                    try
                    {
                        Debug.Log("X:" + x + " Y:" + y + " = " + footprint[x / 16, y / 16]);
                        if (footprint[x / 16, y / 16] == true)
                            y += 16 - y;
                        else if (sprite.texture.GetPixel(x, y).a > 0.1f)
                        {
                            footprint[x / 16, y / 16] = true;
                            y += 16 - y;
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.Log("help");
                        return;
                    }
                }
            }
        }
    }*/
}
