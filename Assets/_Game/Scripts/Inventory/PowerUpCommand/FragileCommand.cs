using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Game.Scripts.Inventory.PowerUpCommand
{
    /// <summary>
    ///     A jewel with the fragile power up attached will be destroyed after it falls.
    /// </summary>
    public class FragileCommand : IPowerUpCommand<List<GameObject>>
    {
        private readonly GameObject _fragileJewel;

        public FragileCommand(GameObject fragileJewel)
        {
            _fragileJewel = fragileJewel;
        }

        public List<GameObject> Execute()
        {
            try
            {
                var powerUp = _fragileJewel.GetComponent<PowerUpSlot>().PowerUp;
                if (powerUp == PowerUps.Fragile)
                {
                    _fragileJewel.name = "fragile";
                    var fragileContainer = GameObject.Find(("Fragile"));
                    _fragileJewel.transform.parent = fragileContainer.transform;
                    
                    _fragileJewel.GetComponent<BoxCollider2D>().enabled = false;
                    _fragileJewel.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
                    _fragileJewel.GetComponent<SpriteRenderer>().enabled = false;
                    _fragileJewel.tag = "Untagged";
                    
                    var fragileJewels = new List<GameObject>();
            
                    foreach (Transform child in fragileContainer.transform)
                    {
                        fragileJewels.Add(child.gameObject);
                    }

                    return fragileJewels;
                }
            }
            catch (Exception e)
            {
                Debug.Log("No power up on that fellow.");
            }

            return new List<GameObject>();
        }
    }
}