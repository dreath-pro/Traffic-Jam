using Godot;
using System;

public partial class HeartFloat : Area2D
{
    [Export] public float speed = 320;

    private Vector2 velocity;

    public override void _Ready()
    {
        //Random initial direction
        float angle = (float)GD.RandRange(0, Mathf.Tau);

        velocity = new Vector2(
            Mathf.Cos(angle),
            Mathf.Sin(angle)
            ) * speed;

        BodyEntered += OnBodyEntered;
    }

    public override void _PhysicsProcess(double delta)
    {
        Position += velocity * (float)delta;

        Rect2 bounds = GetViewportRect();

        Vector2 screenPosition = GlobalPosition;

        //Horizontal bouncing
        if (screenPosition.X <= 0 || screenPosition.X >= bounds.Size.X)
        {
            velocity.X *= -1;
        }

        //Vertical bouncing
        if (screenPosition.Y <= 0 || screenPosition.Y >= bounds.Size.Y)
        {
            velocity.Y *= -1;
        }

        if (GlobalPosition.X > 2000)
            QueueFree();

        if (GlobalPosition.X < -389)
            QueueFree();
    }

    private void OnBodyEntered(Node2D body)
    {
        if (body.IsInGroup("player"))
        {
            body.Call("Heal");
            QueueFree();
        }
    }
}
