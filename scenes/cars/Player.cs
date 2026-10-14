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

        for (int i = 0; i < GetSlideCollisionCount(); i++)
        {
            var collision = GetSlideCollision(i);
            var body = collision.GetCollider();

            if (body is Cops cop)
            {
                takeDamage(3);
            }
            else if (body is CargoTruck truck)
            {
                takeDamage(1);
            }
        }
    }

    public void takeDamage(int amount)
    {
        health -= amount;
        if (health <= 0)
        {
            gameOverLogic();
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
