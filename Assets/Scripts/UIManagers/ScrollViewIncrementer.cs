using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ScrollViewIncrementer : MonoBehaviour
{
    public ScrollRect scrollRect;
    
    public void Increment()
    {
        float step = (scrollRect.scrollSensitivity * 10) / scrollRect.content.rect.width;
        if (step < 0)
            step = 0;
        else if (step > 1)
            step = 1;
        scrollRect.horizontalNormalizedPosition += step;
    }

    public void Decrement()
    {
        float step = (scrollRect.scrollSensitivity * 10) / scrollRect.content.rect.width;
        if (step < 0)
            step = 0;
        else if (step > 1)
            step = 1;
        scrollRect.horizontalNormalizedPosition -= step;
    }
}
