using Godot;
using Save;

public partial class HomeScreen : Control
{
    public override void _Input(InputEvent inputEvent)
    {
        // Check if the ui_cancel action is pressed
        if (inputEvent.IsActionPressed("ui_cancel"))
        {
            // Close the game
            GetTree().Quit();
        }
    }
}
