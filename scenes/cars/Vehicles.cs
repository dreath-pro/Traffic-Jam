using Godot;
using System;

public partial class Vehicles : CharacterBody2D
{
    public override void _PhysicsProcess(double delta)
    {
        Velocity = Vector2.Left * 200;
        MoveAndSlide();

        if (GlobalPosition.X < -100)
            QueueFree();
    }
}
