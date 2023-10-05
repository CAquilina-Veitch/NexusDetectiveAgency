using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalFunctions
{
    public static float normaliseRotation(this float rotation)
    {
        return rotation % 360;
    }
    public static Vector3 toVector3(this Vector2 v)
    {
        return new Vector3(v.x, 0, v.y);
    }

    public static Vector3 PlayerPosToOwner(this Vector3 playerPos, PlayerController controller)
    {
        int currentPlayer = controller.currentPlayerDimension;
        Vector3 diff = controller.dimensionalDiffPosition;
        Vector3 temp = playerPos;
        temp = currentPlayer==0? temp - diff:temp+diff;

        return temp;
    }
    public static Vector3[] OwnerPosToPlayer(this Vector3 playerPos, PlayerController controller)
    {
        Vector3 diff = controller.dimensionalDiffPosition;
        Vector3[] temp = { playerPos , playerPos};
        temp[0] = playerPos+diff;
        temp[1] = playerPos-diff;

        return temp;

    }



}
