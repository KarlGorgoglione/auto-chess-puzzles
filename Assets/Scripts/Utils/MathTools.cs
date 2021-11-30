using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MathTools
{

    public static float CustomRound(float min, float max, float val)
    {

        float normalMin = min - Mathf.Floor(min);
        float normalMax = max - Mathf.Floor(max);

        if (normalMin > normalMax)
        {
            string exString = "Min: " + min.ToString() + " is greater than Max: " + max.ToString() + " in CustomRand";
            throw new ArgumentException(exString);
        }

        float innerRange = normalMax - normalMin;
        float outerRange = 1 + normalMin - normalMax;

        float wholePart = (float)Math.Floor(val);
        float fracPart = val - wholePart;
        if (fracPart < normalMin || fracPart > normalMax)
        {
            int carry = fracPart > normalMax ? 1 : 0;
            fracPart += (1 - normalMax);
            if (fracPart >= 1)
                fracPart--;
            fracPart /= outerRange;
            fracPart = (float)Math.Round(fracPart, MidpointRounding.AwayFromZero);
            if (fracPart == 0)
                fracPart = normalMax - (1 - carry);
            else
                fracPart = normalMin + carry;
        }

        else
        {

            fracPart -= normalMin;
            fracPart /= innerRange;
            fracPart = (float)Math.Round(fracPart, MidpointRounding.AwayFromZero);
            fracPart *= innerRange;
            fracPart += normalMin;
        }
        return fracPart + wholePart;
    }
}