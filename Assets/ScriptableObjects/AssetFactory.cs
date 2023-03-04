using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Class <c>AssetFactory</c> handles updates to assets and asset lists within the project.
/// </summary>
public class AssetFactory
{
    /// <summary>
    /// Updates all the asset folders and asset lists within the project.
    /// </summary>
    [MenuItem("AssetDatabase/UpdateAllAssets")] // add update as a method in Unity's top bar menu
    private static void UpdateAllAssets()
    {
        TerrainFactory(FindAssetList("TerrainData", "ObjectList"));

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    /// <summary>
    /// Ensures the folder for the assets exists in the ScriptableObjects asset folder,
    /// finds and returns the file path to that asset list.
    /// </summary>
    /// <param name="assetFolder">The name of the folder that holds the scriptable objects.</param>
    /// <param name="assetListName">The name of the object that links to all the scriptable objects.</param>
    /// <returns>Returns the file path to the object that holds the scriptable objects specified
    /// by the parameters.</returns>
    private static string FindAssetList(string assetFolder, string assetListName)
    {
        // ensure file path exists, if not create it
        if (!Directory.Exists("Assets/ScriptableObjects"))
            AssetDatabase.CreateFolder("Assets", "ScriptableObjects");
        if (!Directory.Exists("Assets/ScriptableObjects/" + assetFolder))
            AssetDatabase.CreateFolder("Assets/ScriptableObjects", assetFolder);
        if (!Directory.Exists("Assets/ScriptableObjects/ObjectLists"))
            AssetDatabase.CreateFolder("Assets/ScriptableObjects", "ObjectLists");

        // find existing asset list
        string[] assetListSearch = AssetDatabase.FindAssets(assetListName, new[] { "Assets/ScriptableObjects/ObjectLists" });
        // missing line
        if (assetListSearch.Length == 1) // changed from > 0
        {
            return AssetDatabase.GUIDToAssetPath(assetListSearch[0]);
            // missing line
        }
        else
            Debug.LogError("Error: \"" + assetListName + $"\" Asset List not found or returned multiple ({assetListSearch.Length}) instances");

        return "";
        // 2 missing sections
    }

    /// <summary>
    /// Ensures the tile sprite sheet contains the correct amount of sprites, then updates the terrain
    /// asset folder and from that, the terrain asset list.
    /// </summary>
    /// <param name="objectListFilePath">The file path to the object that holds the TerrainData objects.</param>
    private static void TerrainFactory(string objectListFilePath)
    {
        if (objectListFilePath == "")
        {
            Debug.LogWarning("Warning: TerrainFactory function in AssetFactory provided no file path.");
            return;
        }
        // NOTE: can the ObjectList class be generalized?
        ObjectList objList = AssetDatabase.LoadAssetAtPath<ObjectList>(objectListFilePath);

        // ensure the terrain sprite sheet contains the right amount of sprites
        string tileSheetPath = AssetDatabase.GetAssetPath(objList.tileSheet);
        Sprite[] sprites = AssetDatabase.LoadAllAssetsAtPath(tileSheetPath).OfType<Sprite>().ToArray();
        Array.Sort(sprites, CompareSpriteNames);
        if (sprites.Length % 32 != 0 || sprites.Length / 32 != objList.terrainDetails.Length)
            Debug.LogError($"Tile Sprite Atlas contains wrong # of sprites ({sprites.Length})");
        objList.terrainTiles = new TerrainData[objList.terrainDetails.Length];

        // update terrain asset folder and from that, the terrain asset list
        string[] results;
        string resultPath;
        TerrainData newTerrain = null;
        int index = 0;
        foreach (TerrainDetails details in objList.terrainDetails)
        {
            results = AssetDatabase.FindAssets(details.name, new[] { "Assets/ScriptableObjects/TerrainData" });
            if (results.Length == 0)
            {
                newTerrain = ScriptableObject.CreateInstance<TerrainData>();
                newTerrain.Init(details, index, GetSprites(index, sprites));
                AssetDatabase.CreateAsset(newTerrain, $@"Assets/ScriptableObjects/TerrainData/{details.name}.asset");
            }
            else if (results.Length == 1)
            {
                resultPath = AssetDatabase.GUIDToAssetPath(results[0]);
                newTerrain = AssetDatabase.LoadAssetAtPath<TerrainData>(resultPath);
                newTerrain.Init(details, index, GetSprites(index, sprites));
            }
            else
                Debug.LogError($"More than 1 Terrain Data Asset found ({details.name})");

            if (newTerrain != null)
            {
                objList.terrainTiles[index] = newTerrain;
                EditorUtility.SetDirty(newTerrain);
            }
            index++;
        }
    }

    /// <summary>
    /// Orders the given sprites from highest to lowest, with higher indices indicating less dominance.
    /// </summary>
    /// <param name="x">A sprite with the naming convention [anything]_[integer].</param>
    /// <param name="y">A sprite with the naming convention [anything]_[integer].</param>
    /// <returns>Returns a negative integer if x is less than y, a positive integer if x is more than y,
    /// and zero if x and y are equal.</returns>
    private static int CompareSpriteNames(Sprite x, Sprite y)
    {
        int xNum = -1, yNum = -1;
        Int32.TryParse(x.name.Split('_')[1], out xNum);
        Int32.TryParse(y.name.Split('_')[1], out yNum);
        if (xNum == -1 || yNum == -1)
            Debug.LogError("Issue in CompareSpriteNames in AssetFactory");

        return xNum.CompareTo(yNum);
    }

    /// <summary>
    /// Gets the set of 32 terrain sprites for the specified terrain type index.
    /// </summary>
    /// <param name="index">The index of the terrain type to fetch.</param>
    /// <param name="spriteSet">The sorted list of all terrain sprites.</param>
    /// <returns>Returns a list of 32 sprites that match the terrain type index provided.</returns>
    private static Sprite[] GetSprites (int index, Sprite[] spriteSet)
    {
        Sprite[] result = new Sprite[32];
        for (int i = 0; i < 32; i++)
        {
            result[i] = spriteSet[(index * 32) + i];
        }
        return result;
    }
}
