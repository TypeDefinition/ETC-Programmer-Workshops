using UnityEngine;

public class Menu {
    public enum Item {
        Invalid = -1,
        Pizza,
        Taco,
        Laksa,

        Num
    }

    public static Color ColorOf(Item menuItem) {
        switch (menuItem) {
            case Item.Pizza: return Color.yellow;
            case Item.Taco: return Color.green;
            case Item.Laksa: return Color.red;
            default: return Color.white;
        }
    }

    public static string StringOf(Item menuItem) {
        switch (menuItem) {
            case Menu.Item.Invalid: return "Invalid";
            case Menu.Item.Pizza: return "Pizza";
            case Menu.Item.Taco: return "Taco";
            case Menu.Item.Laksa: return "Laksa";
            case Menu.Item.Num: return "Num";
            default: throw new System.Exception("Unhandled MenuItem");
        }
    }
}