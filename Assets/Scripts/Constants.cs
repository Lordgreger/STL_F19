using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Constants {
    public const int elementValueMin = 1;
    public const int elementValueMax = 7;

    public const int targetValueMin = 11;
    public const int targetValueMax = 19;

    public const int gridElementsX = 5;
    public const int gridElementsY = 8;

    public static string elementListToString(List<GameGrid.Element> elements) {
        string output = "[ ";
        foreach (var e in elements) {
            output += e.value + ", ";
        }
        output = output.Remove(output.Length - 2, 2);
        output += " ]";
        return output;
    }

}
