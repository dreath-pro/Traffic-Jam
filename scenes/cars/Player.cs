using Godot;
using System;

public partial class Player : CharacterBody2D
{
    public int health { get; private set; } = 5;
    [Signal] public delegate void GameOverEventHandler();

    public override void _PhysicsProcess(double delta)
    {
        Vector2 direction = Input.GetVector("move_left", "move_right", "move_up", "move_down");
        Velocity = direction * 380;
        MoveAndSlide();
    }

    public override void _Ready()
    {
        Area2D hitbox = GetNode<Area2D>("Hitbox");
        hitbox.BodyEntered += OnBodyEntered;
    }

    private void OnBodyEntered(Node2D body)
    {
        if (body is Cops)
        {
            health -= 3;
            if (health <= 0)
            {
                gameOverLogic();
            }
        }
        else
        {
            health--;
            if (health <= 0)
            {
                gameOverLogic();
            }
        }

        GetNode<Label>("%HealthLabel").Text = "x" + health.ToString();
    }

    private void gameOverLogic()
    {
        health = 0;
        QueueFree();
        EmitSignal(SignalName.GameOver);
    }
}
