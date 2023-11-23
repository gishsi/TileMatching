using System;

namespace _Game.Scripts.Legacy
{
    /// <summary>
    ///     During the first attempt at grid restoration I came across some issues I had trouble identifying.
    ///     I tried many different things, and even considered casting rays - which would be messy, as I have found out from the ray approach in <see cref="Falling"/>.
    ///     Thankfully, I did research and found out that the Destroy(gameObject) method does not complete until the end of frame. I.e. first it "marks" a game object
    ///     for deletion, and only deletes it at the end of the frame - this caused a lot of issues and frustration during the restructuring attempt.
    /// </summary>
    [Obsolete]
    public class FirstAttemptAtGridRestoration
    {
        // /// <summary>
        // ///     Takes a row and counts how many
        // /// </summary>
        // /// <param name="tile">A position of the tile</param>
        // private int GetAmountOfTilesAboveTile(Vector2Int tile)
        // {
        //     var amount = 0;
        //     
        //     foreach (Transform child in transform)
        //     {
        //         if (!child.CompareTag("Tile"))
        //         {
        //             continue;
        //         }
        //         var isInTheSameRow = child.name.StartsWith(tile.x.ToString());
        //         var isAboveTheStartingTile = ParseNameIntoVector2Int(child.name).y > tile.y;
        //         var exists = GameObject.Find(child.name) != null;
        //         
        //         if (isInTheSameRow && isAboveTheStartingTile && exists)
        //         {
        //             amount++;
        //         }
        //     }
        //     
        //     return amount;
        // }
        //
        // private void RestructureGrid(IEnumerable<GameObject> tilesToDelete)
        // {
        //     Debug.Log("");
        //     Debug.Log("--------------- Restructuring --------------------");
        //
        //     foreach (var tileToDelete in tilesToDelete)
        //     {
        //         var tileToDeleteInGridPosition = ParseNameIntoVector2Int(tileToDelete.name);
        //         var toDestroy = GameObject.Find(tileToDelete.name);
        //         var amountOfTilesAboveTile = GetAmountOfTilesAboveTile(tileToDeleteInGridPosition);
        //
        //         if (amountOfTilesAboveTile == 0)
        //         {
        //             Debug.Log("No tiles above: " + tileToDelete.name);
        //             Destroy(toDestroy);
        //             return;
        //         }
        //         
        //         var i = 0;
        //
        //         Destroy(toDestroy);
        //         
        //         while (i < amountOfTilesAboveTile)
        //         {
        //             var tileAbovePosition =
        //                 new Vector2Int(tileToDeleteInGridPosition.x, tileToDeleteInGridPosition.y + i + 1);
        //             
        //             var tileToReplace =
        //                 new Vector2Int(tileAbovePosition.x, tileAbovePosition.y - 1);
        //             
        //             var newPosition = GetNewPositionFromTileBelow(tileAbovePosition.x, tileToReplace.y);
        //             
        //             
        //             var nameOfTile = ParseVector2IntIntoNameString(tileAbovePosition);
        //             var tileToMove = GameObject.Find(nameOfTile);
        //             
        //             tileToMove.transform.localPosition = newPosition;
        //             tileToMove.gameObject.name = ParseVector2IntIntoNameString(tileToReplace);
        //             
        //             i++;
        //         }
        //
        //         
        //     }
        //     
        //     Debug.Log("---------------------------------------------------");
        //     Debug.Log("");
        // }
    }
}