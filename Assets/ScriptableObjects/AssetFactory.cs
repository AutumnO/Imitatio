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
    private static readonly string mainPath = "Assets/ScriptableObjects"; // also change in FindAssetList at CreateFolder call
    private static readonly string assetListsPath = mainPath + "/AssetLists"; // same as above

    private static readonly string terrainDataPath = mainPath + "/TerrainData";

    private static readonly string headFeaturesDataPath = mainPath + "/HeadFeatures";
    private static readonly string eyesSpritesPath = "Assets/Images/CASObjects/HeadFeatures/Eyes";
    private static readonly string mouthSpritesPath = "Assets/Images/CASObjects/HeadFeatures/Mouths";

    /// <summary>
    /// Updates all the asset folders and asset lists within the project.
    /// </summary>
    [MenuItem("AssetDatabase/UpdateAllAssets")] // add update as a method in Unity's top bar menu
    private static void UpdateAllAssets()
    {
        TerrainFactory();
        HeadFeatureFactory();

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
        if (!Directory.Exists(mainPath))
            AssetDatabase.CreateFolder("Assets", "ScriptableObjects");
        if (!Directory.Exists(mainPath + "/" + assetFolder))
            AssetDatabase.CreateFolder(mainPath, assetFolder);
        if (!Directory.Exists(assetListsPath))
            AssetDatabase.CreateFolder(mainPath, "AssetLists");

        // find existing asset list
        string[] assetListSearch = AssetDatabase.FindAssets(assetListName, new[] { assetListsPath });
        
        if (assetListSearch.Length == 1)
            return AssetDatabase.GUIDToAssetPath(assetListSearch[0]);
        else
            Debug.LogError("Error: \"" + assetListName + $"\" Asset List not found or returned multiple ({assetListSearch.Length}) instances");

        return "";
    }

    /// <summary>
    /// Ensures the tile sprite sheet contains the correct amount of sprites, then updates the terrain
    /// asset folder and from that, the terrain asset list.
    /// </summary>
    /// <param name="objectListFilePath">The file path to the object that holds the TerrainData objects.</param>
    private static void TerrainFactory()
    {
        string objectListFilePath = FindAssetList("TerrainData", "ObjectList");
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
            results = AssetDatabase.FindAssets(details.name, new[] { terrainDataPath });
            if (results.Length == 0)
            {
                newTerrain = ScriptableObject.CreateInstance<TerrainData>();
                newTerrain.Init(details, index, GetSprites(index, sprites));
                AssetDatabase.CreateAsset(newTerrain, terrainDataPath + $@"/{details.name}.asset");
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
    /// Grabs the various head feature sprite sheets in the image asset folders, then updates the head
    /// features data folder and from that, the head feature asset list.
    /// </summary>
    /// <param name="objectListFilePath">The file path to the object that holds the eyes and mouth objects.</param>
    private static void HeadFeatureFactory()
    {
        string objectListFilePath = FindAssetList("HeadFeatures", "CASHeadFeatureList");
        if (objectListFilePath == "")
        {
            Debug.LogWarning("Warning: HeadFeatureFactory function in AssetFactory provided no file path.");
            return;
        }

        CASHeadFeatureList objList = AssetDatabase.LoadAssetAtPath<CASHeadFeatureList>(objectListFilePath);

        if (!Directory.Exists(headFeaturesDataPath + "/EyesData"))
            AssetDatabase.CreateFolder(headFeaturesDataPath, "EyesData");
        if (!Directory.Exists(headFeaturesDataPath + "/MouthData"))
            AssetDatabase.CreateFolder(headFeaturesDataPath, "MouthData");

        // go through each of the catagories of head features
        // TODO: edit to accept spritesheets for color and animated eye stuff
        string[] spriteSheetGUIDs;
        Texture2D[] spriteSheets;
        Sprite[] sprites;
        string spriteSheetPath;
        string[] results;
        string resultPath;
        int index;

        // EYES
        spriteSheetGUIDs = AssetDatabase.FindAssets("t:Texture2D", new[] { eyesSpritesPath });
        spriteSheets = new Texture2D[spriteSheetGUIDs.Length];
        sprites = new Sprite[spriteSheets.Length];
        for (int i = 0; i < spriteSheetGUIDs.Length; i++)
        {
            spriteSheetPath = AssetDatabase.GUIDToAssetPath(spriteSheetGUIDs[i]);
            spriteSheets[i] = AssetDatabase.LoadAssetAtPath(spriteSheetPath, typeof(Texture2D)) as Texture2D;
            sprites[i] = AssetDatabase.LoadAssetAtPath(spriteSheetPath, typeof(Sprite)) as Sprite;
        }
        
        if (sprites.Length == 0)
            Debug.LogWarning("Warning: HeadFeatureFactory found no eye sprites at the set path.");

        objList.eyes = new EyesData[spriteSheets.Length];
        EyesData newEyes = null;
        index = 0;
        foreach (Sprite sprite in sprites)
        {
            results = AssetDatabase.FindAssets(sprite.name, new[] { headFeaturesDataPath + "/EyesData" });
            if (results.Length == 0)
            {
                newEyes = ScriptableObject.CreateInstance<EyesData>();
                newEyes.Init(sprite);
                AssetDatabase.CreateAsset(newEyes, headFeaturesDataPath + $@"/EyesData/{sprite.name}.asset");
            }
            else if (results.Length == 1)
            {
                resultPath = AssetDatabase.GUIDToAssetPath(results[0]);
                newEyes = AssetDatabase.LoadAssetAtPath<EyesData>(resultPath);
                newEyes.Init(sprite);
            }
            else
                Debug.LogError($"More than 1 Data Asset found ({sprite.name})");

            if (newEyes != null)
            {
                objList.eyes[index] = newEyes;
                EditorUtility.SetDirty(newEyes);
            }
            index++;
        }

        // MOUTHS
        spriteSheetGUIDs = AssetDatabase.FindAssets("t:Texture2D", new[] { mouthSpritesPath });
        spriteSheets = new Texture2D[spriteSheetGUIDs.Length];
        sprites = new Sprite[spriteSheets.Length];
        for (int i = 0; i < spriteSheetGUIDs.Length; i++)
        {
            spriteSheetPath = AssetDatabase.GUIDToAssetPath(spriteSheetGUIDs[i]);
            spriteSheets[i] = AssetDatabase.LoadAssetAtPath(spriteSheetPath, typeof(Texture2D)) as Texture2D;
            sprites[i] = AssetDatabase.LoadAssetAtPath(spriteSheetPath, typeof(Sprite)) as Sprite;
        }
        if (spriteSheets.Length == 0)
            Debug.LogWarning("Warning: HeadFeatureFactory found no mouth sprites at the set path.");

        objList.mouths = new MouthData[spriteSheets.Length];
        MouthData newMouth = null;
        index = 0;
        foreach (Sprite sprite in sprites)
        {
            results = AssetDatabase.FindAssets(sprite.name, new[] { headFeaturesDataPath + "/MouthData" });
            if (results.Length == 0)
            {
                newMouth = ScriptableObject.CreateInstance<MouthData>();
                newMouth.Init(sprite);
                AssetDatabase.CreateAsset(newMouth, headFeaturesDataPath + $@"/MouthData/{sprite.name}.asset");
            }
            else if (results.Length == 1)
            {
                resultPath = AssetDatabase.GUIDToAssetPath(results[0]);
                newMouth = AssetDatabase.LoadAssetAtPath<MouthData>(resultPath);
                newMouth.Init(sprite);
            }
            else
                Debug.LogError($"More than 1 Data Asset found ({sprite.name})");

            if (newMouth != null)
            {
                objList.mouths[index] = newMouth;
                EditorUtility.SetDirty(newMouth);
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
