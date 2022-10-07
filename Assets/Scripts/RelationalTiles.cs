using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RelationalTiles
{
    public static void SetTileSprites(TileRegion[] surroundingTiles, TerrainData defaultTerrain)
    {
        /*
            Check 24 items
                sort dominance after, first check the tiles that dominate individual tiles
            side = 3210
            corner = 3210

            c0              s1              c1

                    t0  t1  t2  t3  t4
                    t5  t6  t7  t8  t9
            s0      t10 t11 t12 t13 t14     s2
                    t15 t16 t17 t18 t19
                    t20 t21 t22 t23 t24

            c3              s3              c2
        */

        // get list of terrain data for easy access, replace null tiles with default terrain data
        TerrainData[] terrainList = new TerrainData[surroundingTiles.Length];
        for (int i = 0; i < surroundingTiles.Length; i++)
        {
            if (surroundingTiles[i] != null)
                terrainList[i] = surroundingTiles[i].terrainTile.GetData();
            else
                terrainList[i] = defaultTerrain;
        }

        // for each of the existing 9 interior tiles, recalculate and reset sprites
        TerrainData[] currOuter;
        for (int i = 6; i < 19; i++)
        {
            if (i != 9 && i != 10 && i != 14 && i != 15)
            {
                if (surroundingTiles[i] != null)
                {
                    currOuter = new TerrainData[] { terrainList[i - 6], terrainList[i - 5], terrainList[i - 4],
                                                    terrainList[i + 1], terrainList[i + 6], terrainList[i + 5],
                                                    terrainList[i + 4], terrainList[i - 1]};
                    surroundingTiles[i].terrainTile.SetSprites(TileSpriteHelper(currOuter, terrainList[i]));
                }
            }
        }

    }

    // all terrain data must be defaulted to void if tile is null before calling function
    private static TileSprite[] TileSpriteHelper(TerrainData[] tiles, TerrainData center)
    {
        // build dictionary for each terrain type that borders the center tile
        // key: terrain type, value: side binary, corner binary
        Dictionary<TerrainData, (int, int)> spriteDict = new Dictionary<TerrainData, (int, int)>();
        (int, int) currIndices;
        for (int i = 0; i < 8; i++)
        {
            if (tiles[i].dominanceIndex < center.dominanceIndex)
            {
                if (spriteDict.TryGetValue(tiles[i], out currIndices))
                    spriteDict[tiles[i]] = GetNewIndices(i, spriteDict[tiles[i]]);
                else
                    spriteDict[tiles[i]] = GetNewIndices(i, (0b0000, 0b0000));
            }
        }

        // put terrains in order from least to most dominant (high to low dominance index)
        List<TileSprite> result = new List<TileSprite>();
        foreach (KeyValuePair<TerrainData, (int, int)> pair in spriteDict)
            result.Add(new TileSprite(pair.Key, pair.Value));
        result.Sort(CompareDominanceIndices);

        return result.ToArray();
    }

    // determine changes to the side or edge binary literals based on the orientation of the tile
    // in relation to the center
    private static (int, int) GetNewIndices (int index, (int, int) prevIndices)
    {
        switch (index)
        {
            case 0:
                prevIndices.Item2 += 0b0001;
                break;
            case 1:
                prevIndices.Item1 += 0b0010;
                break;
            case 2:
                prevIndices.Item2 += 0b0010;
                break;
            case 3:
                prevIndices.Item1 += 0b0100;
                break;
            case 4:
                prevIndices.Item2 += 0b0100;
                break;
            case 5:
                prevIndices.Item1 += 0b1000;
                break;
            case 6:
                prevIndices.Item2 += 0b1000;
                break;
            case 7:
                prevIndices.Item1 += 0b0001;
                break;
        }
        return prevIndices;
    }

    // order highest to lowest, higher indices indicate less dominance
    private static int CompareDominanceIndices (TileSprite x, TileSprite y)
    {
        if (x.terrain.dominanceIndex < y.terrain.dominanceIndex)
            return 1;
        else if (x.terrain.dominanceIndex > y.terrain.dominanceIndex)
            return -1;
        else
            return 0;
    }
}

public struct TileSprite
{
    public int sideIndex;
    public int cornerIndex;
    public TerrainData terrain;

    public TileSprite (TerrainData data, (int, int) indices)
    {
        sideIndex = indices.Item1;
        cornerIndex = indices.Item2;
        terrain = data;
    }
}
