using Godot;
using System;

public partial class CargoTruck : CharacterBody2D
{
    public override void _PhysicsProcess(double delta)
    {
        Velocity = Vector2.Left * 100;
        MoveAndSlide();

        if (GlobalPosition.X < -783)
            QueueFree();
    }
}
