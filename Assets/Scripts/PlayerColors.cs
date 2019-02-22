using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerColors
{
    static Color player1 = new Color(253, 147, 89);
    static Color player2 = new Color(173, 227, 235);
    static Color player3 = new Color(226, 110, 223);
    static Color player4 = new Color(58, 255, 120);
    private static Color[] playerColors = { player1, player2, player3, player4 };
    public static Color getPlayerColor(GameObject player)
    {
        return playerColors[player.GetComponent<Outer_Movement>().playerId];
    }
}