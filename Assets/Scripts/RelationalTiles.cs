using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RelationalTiles
{
    public static void SetTileSprites(TileRegion newTile, TileRegion[] otherTiles)
    {
        /*
            24 items to check
            
            
            CHECK LEFT & TOP SIDE
                if at map edge: ????
                if left tile exists && top tile exists
                    if outer tile is dominant
                        outer tile's right side = null
                        inner tile's left side = outer tile
                        (if bottom left tile exists && bottom tile exists)
                        if bottom left tile is dominant over left tile AND
                            bottom left tile is dominant over bottom tile
                            inner tile's bottom left corner = bottom left tile
                        else inner tile's bottom left corner = null
                        (if top left tile exists && top tile exists)
                        if top left tile is dominant over left tile AND
                            top left tile is dominant over top tile
                            inner tile's top left corner = top left tile
                        else inner tile's top left corner = null

                    if inner tile is dominant
                        left tile's right side = inner tile
                        inner tile's left side = null
                        (if bottom left tile exists && bottom tile exists)
                        if inner tile is dominant over bottom left tile AND
                            inner tile is dominant over bottom tile
                            left tile's bottom right corner = null
                            bottom left tile's top right corner = inner tile
                            bottom tile's top side = inner tile
                        (if top left tile exists && top tile exists)
                        if inner tile is dominant over top left tile AND
                            inner tile is dominant over top tile
                            left tile's top right corner = null
                            top left tile's bottom right corner = inner tile
                            top tile's bottom side = inner tile
                    if outer and inner are the same terrain

                if left tile exists but top tile doesn't exist
                    inner tile's left side = void
                    inner tile's bottom left corner = void
                    inner tile's top left corner = void

                if left tile doesn't exist but top tile does exist

                if left tile doesn't exist and top tile doesn't exist
                    
        
            CHECK BOTTOM LEFT
                
            CHECK TOP LEFT
            CHECK TOP
                if a tile exists there
                    if outer tile is dominant
                    if inner tile is dominant
                    if inner and outer tiles are the same
                if a tile doesn't exist there
                    inner tile's top side = void
                    inner tile's top left corner = void
                    inner tile's top right corner = void
            CHECK RIGHT SIDE
            CHECK TOP RIGHT
            CHECK BOTTOM RIGHT
            CHECK BOTTOM
        */
    }
}

public enum TileType
{
    isolated, filled, fourCorners,

    oneSideBottom, oneSideLeft, oneSideTop, oneSideRight,
    twoAdjacentSidesTopLeft, twoAdjacentSidesTopRight, twoAdjacentSidesBottomLeft, twoAdjacentSidesBottomRight,
    twoSidesRightLeft, twoSidesTopBottom,
    threeAdjacentSidesTop, threeAdjacentSidesBottom, threeAdjacentSidesLeft, threeAdjacentSidesRight,

    oneCornerTopLeft, oneCornerTopRight, oneCornerBottomLeft, oneCornerBottomRight,
    twoCornersTop, twoCornersRight, twoCornersBottom, twoCornersLeft,
    twoCornersOppositeTopLeft, twoCornersOppositeTopRight,
    threeCornersTopLeft, threeCornersTopRight, threeCornersBottomLeft, threeCornersBottomRight,

    oneSideOneCornerLeftBottomRight, oneSideOneCornerLeftTopRight, oneSideOneCornerRightBottomLeft, oneSideOneCornerRightTopLeft,
    oneSideOneCornerTopBottomRight, oneSideOneCornerTopBottomLeft, oneSideOneCornerBottomTopRight, oneSideOneCornerBottomTopLeft,
    oneSideTwoCornersTop, oneSideTwoCornersRight, oneSideTwoCornersBottom, oneSideTwoCornersLeft,
    twoSidesOneCornerBottomRight, twoSidesOneCornerBottomLeft, twoSidesOneCornerTopRight, twoSidesOneCornerTopLeft
}
