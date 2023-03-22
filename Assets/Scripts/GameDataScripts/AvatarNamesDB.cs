using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AvatarNamesDB : ScriptableObject
{
    public List<AvatarName> femaleNames = new List<AvatarName>();
    public List<AvatarName> maleNames = new List<AvatarName>();

    public void ParseNamesByGender(AvatarNamesList names)
    {
        foreach (AvatarName name in names.allNames)
        {
            if (name.gender == "Female")
                femaleNames.Add(name);
            else if (name.gender == "Male")
                maleNames.Add(name);
            else if (name.gender == "Unisex")
            {
                femaleNames.Add(name);
                maleNames.Add(name);
            }    
        }
    }
}

[System.Serializable]
public class AvatarName
{
    public string avatarName;
    public string gender;
}

public class AvatarNamesList
{
    public List<AvatarName> allNames = new List<AvatarName>();
}