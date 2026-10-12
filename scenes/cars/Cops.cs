using Godot;
using System;

public partial class Cops : CharacterBody2D
{
    private int health = 3;
    public static int totalEscaped = 0;

    public override void _Ready()
    {
        GetNode<Label>("%HealthLabel").Text = "x" + health.ToString();

        Area2D hitbox = GetNode<Area2D>("Hitbox");
        hitbox.BodyEntered += OnBodyEntered;
    }

    public override void _PhysicsProcess(double delta)
    {
        Velocity = Vector2.Right * 200;
        MoveAndSlide();

        if (GlobalPosition.X > 1290)
        {
            totalEscaped++;
            QueueFree();
        }
    }

    private void OnBodyEntered(Node2D body)
    {
        health--;

        if (health <= 0)
        {
            health = 0;
            QueueFree();
        }

        GetNode<Label>("%HealthLabel").Text = "x" + health.ToString();
    }
}
