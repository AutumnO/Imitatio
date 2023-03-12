using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Class <c>AssetFactory</c> handles updates to assets and asset lists within the project.
/// </summary>
public static class AssetFactory
{
    private static readonly string mainPath = "Assets/ScriptableObjects"; // also change in FindAssetList at CreateFolder call
    private static readonly string assetListsPath = mainPath + "/AssetLists"; // same as above

    private static readonly string terrainDataPath = mainPath + "/TerrainData";
    private static readonly string worldObjectDataPath = mainPath + "/WorldObjects";
    private static readonly string propSpritesPath = "Assets/Images/WorldObjects/Props";
    private static readonly string buildingSpritesPath = "Assets/Images/WorldObjects/Buildings";

    private static readonly string avatarCosmeticsDataPath = mainPath + "/AvatarCosmetics"; // change in AvatarCosmeticsFactory
    private static readonly string eyebrowsSpritesPath = "Assets/Images/CASObjects/HeadFeatures/Eyebrows";
    private static readonly string eyesSpritesPath = "Assets/Images/CASObjects/HeadFeatures/Eyes";
    private static readonly string mouthSpritesPath = "Assets/Images/CASObjects/HeadFeatures/Mouths";

    /// <summary>
    /// Updates all the asset folders and asset lists within the project.
    /// </summary>
    [MenuItem("AssetDatabase/UpdateAllAssets")] // add update as a method in Unity's top bar menu
    private static void UpdateAllAssets() // TODO: delete folders and rebuild every time to update for deleted objects
    {
        TerrainFactory();
        WorldObjectFactory();
        AvatarCosmeticsFactory();

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
    /// by the parameters. Returns an empty string if no filepath was found.</returns>
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
                newTerrain.Init(details, index, GetTerrainSpriteSet(index, sprites));
                AssetDatabase.CreateAsset(newTerrain, terrainDataPath + $@"/{details.name}.asset");
            }
            else if (results.Length == 1)
            {
                resultPath = AssetDatabase.GUIDToAssetPath(results[0]);
                newTerrain = AssetDatabase.LoadAssetAtPath<TerrainData>(resultPath);
                newTerrain.Init(details, index, GetTerrainSpriteSet(index, sprites));
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
    /// Grabs the world object sprite sheets in the image asset folders, then updates the
    /// corresponding asset data folders and from that, the object asset list.
    /// </summary>
    private static void WorldObjectFactory()
    {
        string assetListFilePath = FindAssetList("WorldObjects", "ObjectList");
        if (assetListFilePath == "")
        {
            Debug.LogWarning("Warning: WorldObjectFactory in AssetFactory found no file path.");
            return;
        }

        ObjectList assetList = AssetDatabase.LoadAssetAtPath<ObjectList>(assetListFilePath);
        string[] spriteSheetGUIDs;

        // PROPS
        spriteSheetGUIDs = AssetDatabase.FindAssets("t:Sprite", new[] { propSpritesPath });
        ObjectListBuilder(assetList, BasicAssetType.Prop, spriteSheetGUIDs);

        // BUILDINGS
        spriteSheetGUIDs = AssetDatabase.FindAssets("t:Texture2D", new[] { buildingSpritesPath });
        ObjectListBuilder(assetList, BasicAssetType.Building, spriteSheetGUIDs);
    }

    /// <summary>
    /// Grabs the avatar cosmetics sprite sheets in the image asset folders, then updates the
    /// corresponding asset data folders and from that, the avatar cosmetics asset list.
    /// </summary>
    /// <param name="objectListFilePath">The file path to the object that holds the eyes and mouth objects.</param>
    private static void AvatarCosmeticsFactory()
    {
        string assetListFilePath = FindAssetList("AvatarCosmetics", "AvatarCosmeticsList");
        if (assetListFilePath == "")
        {
            Debug.LogWarning("Warning: AvatarCosmeticsFactory in AssetFactory found no file path.");
            return;
        }

        // go through each of the catagories of head features
        // TODO: edit to accept spritesheets for color and animated eye stuff

        AvatarCosmeticsList assetList = AssetDatabase.LoadAssetAtPath<AvatarCosmeticsList>(assetListFilePath);
        string[] spriteSheetGUIDs;

        // EYES
        spriteSheetGUIDs = AssetDatabase.FindAssets("t:Texture2D", new[] { eyebrowsSpritesPath });
        AvatarCosmeticsListBuilder(assetList, BasicAssetType.Eyebrow, spriteSheetGUIDs);

        // EYES
        spriteSheetGUIDs = AssetDatabase.FindAssets("t:Texture2D", new[] { eyesSpritesPath });
        AvatarCosmeticsListBuilder(assetList, BasicAssetType.Eyes, spriteSheetGUIDs);

        // MOUTHS
        spriteSheetGUIDs = AssetDatabase.FindAssets("t:Texture2D", new[] { mouthSpritesPath });
        AvatarCosmeticsListBuilder(assetList, BasicAssetType.Mouth, spriteSheetGUIDs);
    }

    /// <summary>
    /// Builds the asset array for the given asset type in the ObjectList.
    /// </summary>
    /// <param name="assetList">The ObjectList for the project.</param>
    /// <param name="assetType">The asset type associated with the list to be built.</param>
    /// <param name="spriteSheetGUIDs">The array of GUIDs associated with the sprites to build the asset list from.</param>
    private static void ObjectListBuilder(ObjectList assetList, BasicAssetType assetType, string[] spriteSheetGUIDs)
    {
        Sprite[] sprites = GetSpritesFromGUIDs(spriteSheetGUIDs);
        WorldObjectDetails[] details = assetList.GetDetails(assetType);
        assetList.ResetListWithSize(assetType, sprites.Length);
        WorldObjectData newAsset = null;
        string[] results;
        int index = 0;
        foreach (Sprite sprite in sprites)
        {
            results = AssetDatabase.FindAssets(sprite.name, new[] { worldObjectDataPath });
            if (results.Length == 0)
            {
                newAsset = ScriptableObject.CreateInstance<WorldObjectData>();
                newAsset.Init(sprite, assetType);
                try
                {
                    WorldObjectDetails objDetails = default;
                    for (int i = 0; i < details.Length; i++)
                    {
                        if (details[i].name == newAsset.name)
                        {
                            objDetails = details[i];
                            break;
                        }
                    }    
                    newAsset.rectSize = details[index].rectSize;
                    newAsset.tilesOutsideFootprint = details[index].tilesOutsideFootprint;
                }
                catch (Exception)
                {
                    Debug.LogWarning("Warning: Couldn't find details on a world object in ObjectListBuilder.");
                }
                AssetDatabase.CreateAsset(newAsset, worldObjectDataPath + $@"/{sprite.name}.asset");
            }
            else if (results.Length == 1)
            {
                string resultPath = AssetDatabase.GUIDToAssetPath(results[0]);
                newAsset = AssetDatabase.LoadAssetAtPath<WorldObjectData>(resultPath);
                newAsset.Init(sprite, assetType);
                try
                {
                    WorldObjectDetails objDetails = default;
                    for (int i = 0; i < details.Length; i++)
                    {
                        if (details[i].name == newAsset.name)
                        {
                            objDetails = details[i];
                            break;
                        }
                    }
                    newAsset.rectSize = details[index].rectSize;
                    newAsset.tilesOutsideFootprint = details[index].tilesOutsideFootprint;
                }
                catch (Exception)
                {
                    Debug.LogWarning("Warning: Couldn't find details on a world object in ObjectListBuilder.");
                }
            }
            else
                Debug.LogError($"More than 1 Data Asset found ({sprite.name})");

            if (newAsset != null)
            {
                assetList.SetListItem(assetType, newAsset, index);
                EditorUtility.SetDirty(newAsset);
            }
            index++;
        }
    }

    /// <summary>
    /// Builds the asset array for the given asset type in the AvatarCosmeticsList.
    /// </summary>
    /// <param name="assetList">The AvatarCosmeticsList for the project.</param>
    /// <param name="assetType">The asset type associated with the list to be built.</param>
    /// <param name="spriteSheetGUIDs">The array of GUIDs associated with the sprites to build the asset list from.</param>
    private static void AvatarCosmeticsListBuilder(AvatarCosmeticsList assetList, BasicAssetType assetType, string[] spriteSheetGUIDs)
    {
        Sprite[] sprites = GetSpritesFromGUIDs(spriteSheetGUIDs);
        assetList.ResetListWithSize(assetType, sprites.Length);
        BasicAsset newAsset = null;
        string[] results;
        int index = 0;
        foreach (Sprite sprite in sprites)
        {
            results = AssetDatabase.FindAssets(sprite.name, new[] { avatarCosmeticsDataPath });
            if (results.Length == 0)
            {
                newAsset = ScriptableObject.CreateInstance<BasicAsset>();
                newAsset.Init(sprite, assetType);
                AssetDatabase.CreateAsset(newAsset, avatarCosmeticsDataPath + $@"/{sprite.name}.asset");
            }
            else if (results.Length == 1)
            {
                string resultPath = AssetDatabase.GUIDToAssetPath(results[0]);
                newAsset = AssetDatabase.LoadAssetAtPath<BasicAsset>(resultPath);
                newAsset.Init(sprite, assetType);
            }
            else
                Debug.LogError($"More than 1 Data Asset found ({sprite.name})");

            if (newAsset != null)
            {
                assetList.SetListItem(assetType, newAsset, index);
                EditorUtility.SetDirty(newAsset);
            }
            index++;
        }
    }

    /// <summary>
    /// Gets the sprites at the GUIDs provided. Only loads the first sprite at a particular GUID.
    /// </summary>
    /// <param name="guids">The GUIDs of the sprites to fetch.</param>
    /// <returns>The array of sprites in the same order as their GUIDs in the parameter.</returns>
    private static Sprite[] GetSpritesFromGUIDs(string[] guids)
    {
        List<Sprite> sprites = new List<Sprite>();
        object[] newSprites;
        string assetPath;

        for (int i = 0; i < guids.Length; i++)
        {
            assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
            newSprites = AssetDatabase.LoadAllAssetRepresentationsAtPath(assetPath);
            foreach (Sprite s in newSprites)
                sprites.Add(s);
        }

        if (guids.Length == 0)
            Debug.LogWarning("Warning: GetSprites received an empty list.");
        else if (guids.Length != 0 && sprites.Count == 0)
            Debug.LogWarning("Warning: GetSprites found no sprites at the provided GUIDs.");

        return sprites.ToArray();
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
    private static Sprite[] GetTerrainSpriteSet (int index, Sprite[] spriteSet)
    {
        Sprite[] result = new Sprite[32];
        for (int i = 0; i < 32; i++)
        {
            result[i] = spriteSet[(index * 32) + i];
        }
        return result;
    }
}
