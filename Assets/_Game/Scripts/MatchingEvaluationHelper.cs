using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.U2D.Aseprite;
using UnityEngine;

namespace _Game.Scripts
{
    public class MatchingEvaluationHelper
    {
        /// <summary>
        ///     This object represents all of the possible matching zones for a jewel.
        /// </summary>
        /// <remarks>
        ///     The lists themselves need to be ordered by columns descending.
        /// </remarks>
        private static readonly List<List<Vector2Int>> MatchZones = new ()
        {
            new List<Vector2Int>() { new(0, 0), new(1, 0), new(0, -1), new(1, -1) }, // 2x2 top-left
            new List<Vector2Int>() { new(-1, 0), new(0, 0), new(-1, -1), new(0, -1) }, // 2x2 top-right
            new List<Vector2Int>() { new(0, 1), new(1, 1), new(0, 0), new(1, 0) }, // 2x2 bottom-left
            new List<Vector2Int>() { new(-1, 1), new(0, 1), new(-1, 0), new(0, 0) }, // 2x2 bottom-right
            new List<Vector2Int>() { new(0, 0), new(1, 0), new(2, 0) }, // 3x1 far-left [t - -]
            new List<Vector2Int>() { new(-1, 0), new(0, 0), new(1, 0) }, // 3x1 centre [- t -]
            new List<Vector2Int>() { new(-2, 0), new(-1, 0), new(0, 0) }, // 3x1 far-right [- - t]
            new List<Vector2Int>() { new(0, 0), new(0, -1), new(0, -2) }, // 1x3 top
            new List<Vector2Int>() { new(0, 1), new(0, 0), new(0, -1) }, // 1x3 centre
            new List<Vector2Int>() { new(0, 2), new(0, 1), new(0, 0) }, // 1x3 bottom
        };

        /// <summary>
        ///     Iterates over all possible match zones <see cref="MatchZones"/> and returns a list of game objects if there is a match.
        /// </summary>
        /// <param name="origin">Tile that is being evaluated</param>
        /// <returns>list of game objects if there is a match, empty list otherwise.</returns>
        public static List<GameObject> GetMatchingJewels(Vector2Int origin)
        {
            foreach (var matchZone in MatchZones)
            {
                // Find all tiles that belong to a given matching set
                var amountOfTilesToMatch = matchZone.Count;
                var tilesInMatchZone = new List<GameObject>();

                matchZone.ForEach(m =>
                {
                    var gridCoordinateOfTileInMatchZone = origin + m;
                    var nameOfTileInMatchZone =
                        GridSystem.ParseVector2IntIntoNameString(gridCoordinateOfTileInMatchZone);

                    GameObject tile;
                    
                    try
                    {
                        tile = GameObject.Find(nameOfTileInMatchZone);
                    }
                    catch (Exception e)
                    {
                        Debug.Log($"Could not find tile {nameOfTileInMatchZone}.");
                        return;
                    }
         
                    // todo: Consider putting tiles into a layer or checking for tag and checking for that here
                    if (tile == null)
                    {
                        return;
                    }

                    tilesInMatchZone.Add(tile);
                });

                // If there is a match return the tiles
                if (tilesInMatchZone.Count == amountOfTilesToMatch && AreJewelsMatching(tilesInMatchZone))
                {
                    Debug.Log($"Match zone: {matchZone} successful.");
                    return tilesInMatchZone;
                }
            }
            
            return new List<GameObject>();
        }
        
        /// <summary>
        ///     Helper to check if all the jewels are matching
        /// </summary>
        /// <param name="jewels">Jewels being evaluated for a match</param>
        /// <returns>True if all have the same colour, false otherwise.</returns>
        private static bool AreJewelsMatching(IReadOnlyCollection<GameObject> jewels)
        {
            if (jewels == null || jewels.Count == 0)
            {
                return false;
            }
            
            // Starts at one because first jewel (the one being evaluated) already counts towards the goal
            var countOfMatchedJewels = 1;

            try
            {
                var colourOfFirstJewel = jewels.First().GetComponent<SpriteRenderer>().color;
                
                foreach (var jewel in jewels.Skip(1))
                {
                    if (jewel.GetComponent<SpriteRenderer>().color != colourOfFirstJewel)
                    {
                        return false;
                    }

                    countOfMatchedJewels++;
                }
            }
            catch (Exception e)
            {
                Debug.Log($"Failed to check if jewels match. Exception: {e}");
                return false;
            }

            return countOfMatchedJewels == jewels.Count;
        }
        
        
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