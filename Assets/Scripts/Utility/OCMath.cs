using UnityEngine;

public static class OCMath
{
    /// <summary>
    /// Rounds to the closest decimal place, defined by value of decimalPlaces.
    /// decimalPlaces must be a positive value less than or equal to 10.
    /// </summary>
    public static float RoundFloat(float inFloat, int decimalPlaces = 1)
    {
        #region Error handling
        if (decimalPlaces > 10)
        {
            Debug.LogWarning("10 is the maximum decimal places allowed.");

            decimalPlaces = 10;
        }
        else if (decimalPlaces <= 0)
        {
            Debug.LogError("decimalPlaces argument must be a positive value.");
        }
        #endregion

        int factor = 10;

        for (int i = 1; i < decimalPlaces; i++)
        {
            factor *= 10;
        }

        inFloat = Mathf.Round(inFloat * factor) / factor;

        return inFloat;
    }
}
