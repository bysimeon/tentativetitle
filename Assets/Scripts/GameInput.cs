using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInput
{
    // Start is called before the first frame update
    public static bool SignificantStickInput(float Vertical, float Horizontal)
    {
        return Mathf.Abs(Vertical) + Mathf.Abs(Horizontal) > 0.3f;
    }
}
