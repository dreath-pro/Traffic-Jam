using Godot;
using System;

public partial class Vehicles : CharacterBody2D
{
    private RandomNumberGenerator rng = new RandomNumberGenerator();

    public override void _PhysicsProcess(double delta)
    {
        MoveAndSlide();

        if (GlobalPosition.X < -100)
            QueueFree();
    }

    private void MoveRight()
    {
        Velocity = Vector2.Right * 380;
    }

    private void MoveLeft()
    {
        Velocity = Vector2.Left * 200;
    }

    private void MoveLeftUp()
    {
        Velocity = new Vector2(-200, -100);
    }

    private void MoveLeftDown()
    {
        Velocity = new Vector2(-200, 100);
    }

    public override void _Ready()
    {
        Area2D hitbox = GetNode<Area2D>("Hitbox");  
        hitbox.BodyEntered += OnBodyEntered;
        hitbox.AreaEntered += OnAreaEntered;

        Timer decision = GetNode<Timer>("Decision");
        decision.Timeout += Decision;
        Decision();

        Timer steady = GetNode<Timer>("Steady");
        steady.Timeout += Steady;
        Steady();
    }

    private void Steady()
    {
        MoveLeft();
    }

    private void Decision()
    {
        int choice = rng.RandiRange(0, 3);

        switch (choice)
        {
            case 0:
                MoveLeft();
                break;
            case 1:
                MoveRight();
                break;
            case 2:
                MoveLeftUp();
                break;
            case 3:
                MoveLeftDown();
                break;
        }
    }

    private void OnAreaEntered(Area2D area)
    {
        QueueFree();
    }

    private void OnBodyEntered(Node2D body)
    {
        QueueFree();
    }
}
