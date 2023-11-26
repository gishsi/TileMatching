namespace _Game.Scripts.Inventory
{
    public class PowerUpDropped
    {
        public string NameOfTile;
        public PowerUps PowerUp;
        
        public PowerUpDropped(string nameOfTile, PowerUps powerUp)
        {
            NameOfTile = nameOfTile;
            PowerUp = powerUp;
        }
    }
}