using Godot;
public partial class Player : Car
{
    [Export] public float DriftSpeed = 200f;
    [Export] public float MoveSpeed = 380f;

    public override void _PhysicsProcess(double delta)
    {
        Vector2 direction = Input.GetVector("move_left", "move_right", "move_up", "move_down");

        if (direction.LengthSquared() > 0.0001f)
            Velocity = direction * 380;
        else
            Velocity = Vector2.Left * DriftSpeed;

        base._PhysicsProcess(delta);
        MoveAndSlide();
    }

    public override void _Ready()
    {
        base._Ready();
    }
}
