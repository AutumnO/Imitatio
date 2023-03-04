using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New CASHeadFeatureList", menuName = "Assets/CASHeadFeatureList")]
public class CASHeadFeatureList : ScriptableObject
{
    // TODO: update GameObject types to class names
    public GameObject[] eyebrows;
    public EyesData[] eyes;
    public MouthData[] mouths;
    public GameObject[] hairs;
    public GameObject[] facialMarks;
}