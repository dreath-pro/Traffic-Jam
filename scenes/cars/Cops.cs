using Godot;
using System;

public partial class Cops : Car
{
    private Player player;

    public override void _PhysicsProcess(double delta)
    {
        if (player == null)
            return;

        float followSpeed = 1.0f;
        float moveSpeed = 100.0f;

        float targetY = player.GlobalPosition.Y;
        float newY = Mathf.Lerp(GlobalPosition.Y, targetY, followSpeed * (float)delta);

        Velocity = new Vector2(moveSpeed, (newY - GlobalPosition.Y) / (float)delta);
        MoveAndSlide();

        base._PhysicsProcess(delta);
    }

    public override void _Ready()
    {
        player = GetTree().GetFirstNodeInGroup("player") as Player;
        if (player == null)
        {
            GD.PrintErr("Player not found! Check the player group.");
        }

        GetNode<Label>("%HealthLabel").Text = "x" + health.ToString();

        base._Ready();

        if (GlobalPosition.X > 2000)
        {
            if (GlobalPosition.X < 2800)
            {
                totalEscaped++;
                QueueFree();
            }

            QueueFree();
        }
    }
}
