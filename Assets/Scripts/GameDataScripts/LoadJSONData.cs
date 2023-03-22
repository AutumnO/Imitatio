using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public static class LoadJSONData
{
    [MenuItem("AssetDatabase/LoadJSONDatabases")]
    public static void LoadDBFiles()
    {
        LoadFromJson("AvatarNamesData.json");
    }

    public static void LoadFromJson(string fileName)
    {
        string jsonDataFilePath = Application.dataPath + "/Data/JSONData/" + fileName;
        string nameData = System.IO.File.ReadAllText(jsonDataFilePath);

        string DBObjectsFolder = "Assets/Data/DatabaseObjects";

        // delete database objects folder
        if (Directory.Exists(DBObjectsFolder))
            Directory.Delete(DBObjectsFolder, true);

        // create database object
        Directory.CreateDirectory(DBObjectsFolder);
        AvatarNamesDB namesDB = ScriptableObject.CreateInstance<AvatarNamesDB>();

        // populate object with json data, then add as asset
        namesDB.ParseNamesByGender(JsonUtility.FromJson<AvatarNamesList>(nameData));
        AssetDatabase.CreateAsset(namesDB, DBObjectsFolder + "/AvatarNamesDB.asset");
        
    }
}
