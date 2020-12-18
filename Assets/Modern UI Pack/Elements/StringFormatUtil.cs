using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class StringFormatUtil
{
    public static string VaraibleNameToString(string variableName)
    {
        StringBuilder stringBuilder = new StringBuilder(variableName);

        if (char.IsLower(stringBuilder[0]))

            for (int i = 2; i < stringBuilder.Length; i++)
            {
                if (char.IsUpper(stringBuilder[i]))
                {
                    stringBuilder.Insert(i, " ");
                    i++;
                }
            }

        stringBuilder[0] = char.ToUpper(stringBuilder[0]);

        return stringBuilder.ToString();

    }
}
