using Godot;
public partial class Player : Car
{
    public override void _PhysicsProcess(double delta)
    {
        Vector2 direction = Input.GetVector("move_left", "move_right", "move_up", "move_down");

        Velocity = direction * 380;
        MoveAndSlide();

        base._PhysicsProcess(delta);
    }

    public override void _Ready()
    {
        base._Ready();
    }
}
