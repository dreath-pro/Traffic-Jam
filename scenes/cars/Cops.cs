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

        Area2D hitbox = GetNode<Area2D>("Hitbox");
        hitbox.BodyEntered += OnBodyEntered;

        player = GetTree().GetFirstNodeInGroup("player") as Player;

        if (player == null)
        {
            GD.PrintErr("Player not found! Check the player group.");
        }
        else
        {
            GD.Print("Player found!");
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        if (player == null)
            return;

        float followSpeed = 3.0f;
        float moveSpeed = 200.0f;

        float targetY = player.GlobalPosition.Y;
        float newY = Mathf.Lerp(GlobalPosition.Y, targetY, followSpeed * (float)delta);

        Velocity = new Vector2(moveSpeed, (newY - GlobalPosition.Y) / (float)delta);
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
            totalEscaped++;
            QueueFree();
        }

        GetNode<Label>("%HealthLabel").Text = "x" + health.ToString();
    }
}
