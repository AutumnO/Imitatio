using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyesData : ScriptableObject
{
    // im expecting to need to alter this based on how I handle
    //      different eye/skin color sprite variations
    public Sprite sprite;
    // blinking animations
    // expression animations

    public void Init(Sprite exSprite)
    {
        sprite = exSprite;
    }
}
