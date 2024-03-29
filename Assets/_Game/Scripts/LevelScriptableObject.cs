﻿using System;
using _Game.Scripts.Inventory;
using UnityEngine;

namespace _Game.Scripts
{
    /// <summary>
    ///     Level data
    /// </summary>
    /// <remarks>
    ///     If columns or rows in the grid do not match the rows and columns the designer will be notified in the debug window.
    /// </remarks>.
    [CreateAssetMenu(fileName = "New level", menuName = "Levels/New Level")]
    public class LevelScriptableObject : ScriptableObject
    {
        private const int MaxRows = 5;
        private const int MaxColumns = 7;
        
        [Header("Level")]
        [SerializeField] public string levelName;

        [SerializeField] public int amountOfSwipes;
        
        [Header("Grid size")]
        [Tooltip("This needs to match Rows.")]
        [Range(0, MaxRows)]
        public int rowsAmount = MaxRows;

        [Tooltip("The amount of tiles in a row is equal to the columns.")]
        [Range(0, MaxColumns)]
        public int colsAmount = MaxColumns;
        
        [Header("PowerUps")]
        [SerializeField]
        public PowerUps[] powerUps;
        
        [Header("Tiles")]
        [SerializeField] 
        public Rows[] rows;
        
        /// <summary>
        ///     Ensures that the Tiles match the specified rows and columns amount.
        /// </summary>
        private void OnValidate()
        {
            var rowsCountMatch = rowsAmount == rows.Length;
            var colsCountMatch = false;
            Rows invalidRow = null;
            
            foreach (var row in rows)
            {
                colsCountMatch = row.tilesInRow.Length == colsAmount;
                
                if (colsCountMatch)
                {
                    continue;
                }
                
                invalidRow = row;
                break;
            }
            
            if (!rowsCountMatch)
            {
                Debug.Log($"[{levelName}] Invalid amount of rows. Ensure they match [{rowsAmount}].");
            }

            if (!colsCountMatch && invalidRow != null)
            {
                Debug.Log($"[{levelName}] Invalid amount of columns in row at index [{Array.IndexOf(rows, invalidRow)}]. Ensure they match [{colsAmount}].");
            }

            if (rowsCountMatch && colsCountMatch)
            {
                Debug.Log($"[{levelName}] Rows and columns match the specified size.");
            }
        }
    }
    
    /// <summary>
    ///     A list of these makes up a grid.
    /// </summary>
    [Serializable]
    public class Rows
    {
        [Tooltip("Ensure the amount of tiles here is uniform across all rows.")]
        public Tiles[] tilesInRow;
    }
}