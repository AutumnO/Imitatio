using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New CASHeadFeatureList", menuName = "Assets/CASHeadFeatureList")]
public class CASHeadFeatureList : ScriptableObject
{
    // TODO: update GameObject types to class names
    public GameObject[] eyes;
    public GameObject[] noses;
    public GameObject[] mouths;
    public GameObject[] hairs;
    public GameObject[] eyebrows;
    public GameObject[] facialMarks;
}
