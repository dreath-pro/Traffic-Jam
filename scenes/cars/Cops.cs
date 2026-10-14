using Godot;
using System;

public partial class Cops : CharacterBody2D
{
    private int health = 3;
    public static int totalEscaped = 0;
    private Player player;

    public override void _Ready()
    {
        GetNode<Label>("%HealthLabel").Text = "x" + health.ToString();

        player = GetTree().GetFirstNodeInGroup("player") as Player;

        if (player == null)
        {
            GD.PrintErr("Player not found! Check the player group.");
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        if (player == null)
            return;

        float followSpeed = 1.0f;
        float moveSpeed = 200.0f;

        float targetY = player.GlobalPosition.Y;
        float newY = Mathf.Lerp(GlobalPosition.Y, targetY, followSpeed * (float)delta);

        Velocity = new Vector2(moveSpeed, (newY - GlobalPosition.Y) / (float)delta);
        MoveAndSlide();

        for (int i = 0; i < GetSlideCollisionCount(); i++)
        {
            var collision = GetSlideCollision(i);
            var body = collision.GetCollider();

            if (body is Vehicles vehicle)
            {
                takeDamage(1);
            }
            else if (body is CargoTruck truck)
            {
                takeDamage(1);
            }
        }

        if (GlobalPosition.X > 1290)
        {
            if (GlobalPosition.X < 2000)
            {
                totalEscaped++;
                QueueFree();
            }

            QueueFree();
        }
    }

    public void takeDamage(int amount)
    {
        health -= amount;

        if (health <= 0)
        {
            health = 0;
            totalEscaped++;
            QueueFree();
        }

        GetNode<Label>("%HealthLabel").Text = "x" + health.ToString();
    }
}
