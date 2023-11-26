namespace _Game.Scripts.Inventory.PowerUpCommand
{
    /// <summary>
    ///     Interface for the power up commands.
    /// </summary>
    /// <typeparam name="T">Type of what the command returns</typeparam>
    public interface IPowerUpCommand<out T>
    {
        T Execute();
    }
}