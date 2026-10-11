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

    public override void _Ready()
    {
        Area2D hitbox = GetNode<Area2D>("Hitbox");  
        hitbox.BodyEntered += OnBodyEntered;
    }

    private void OnBodyEntered(Node2D body)
    {
        QueueFree();
    }
}
