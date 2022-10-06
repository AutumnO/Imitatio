using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class TerrainFactory
{
    [MenuItem("AssetDatabase/UpdateTerrainAssets")]
    private static void UpdateTerrainAssets()
    {
        // make sure file path exists
        if (!Directory.Exists("Assets/ScriptableObjects"))
            AssetDatabase.CreateFolder("Assets", "ScriptableObjects");
        if (!Directory.Exists("Assets/ScriptableObjects/TerrainData"))
            AssetDatabase.CreateFolder("Assets/ScriptableObjects", "TerrainData");

        // find object list
        string[] objListSearch = AssetDatabase.FindAssets("ObjectList", new[] { "Assets/ScriptableObjects" });
        ObjectList objList = null;
        if (objListSearch.Length > 0)
        {
            string objListPath = AssetDatabase.GUIDToAssetPath(objListSearch[0]);
            objList = AssetDatabase.LoadAssetAtPath<ObjectList>(objListPath);
        }
        else
            Debug.LogError("Object List not found or returned multiple instances");

        // make sure sprite atlas contains the right amount of sprites
        string tileSheetPath = AssetDatabase.GetAssetPath(objList.tileSheet);
        Sprite[] sprites = AssetDatabase.LoadAllAssetsAtPath(tileSheetPath).OfType<Sprite>().ToArray();
        Array.Sort(sprites, CompareSpriteNames);
        if (sprites.Length % 32 != 0 || sprites.Length / 32 != objList.terrainDetails.Length)
            Debug.LogError($"Tile Sprite Atlas contains wrong # of sprites ({sprites.Length})");
        objList.terrainTiles = new TerrainData[objList.terrainDetails.Length];

        // create/edit assets
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

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    private static Sprite[] GetSprites (int index, Sprite[] spriteSet)
    {
        Sprite[] result = new Sprite[32];
        for (int i = 0; i < 32; i++)
        {
            result[i] = spriteSet[(index * 32) + i];
        }
        return result;
    }

    // order highest to lowest, higher indices indicate less dominance
    private static int CompareSpriteNames(Sprite x, Sprite y)
    {
        int xNum = -1;
        int yNum = -1;
        Int32.TryParse(x.name.Split('_')[1], out xNum);
        Int32.TryParse(y.name.Split('_')[1], out yNum);
        if (xNum == -1 || yNum == -1)
            Debug.LogError("Issue in CompareSpriteNames in TerrainFactory");

        return xNum.CompareTo(yNum);
    }
}
