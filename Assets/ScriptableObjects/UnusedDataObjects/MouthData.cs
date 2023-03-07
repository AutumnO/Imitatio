using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouthData : ScriptableObject
{
    // im expecting to need to alter this based on how I handle
    //      different skin color sprite variations
    public Sprite sprite;
    // talking animations
    // expression animations

    public void Init(Sprite exSprite)
    {
        sprite = exSprite;
    }
}
