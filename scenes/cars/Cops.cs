using Godot;
using System;

public partial class Cops : CharacterBody2D
{
    private int health = 3;
    public static int totalEscaped = 0;
    private Player player;

    private AnimatedSprite2D carSprite;
    private Timer blinkTimer;
    private Timer shieldTimer;

    private bool hasShield = false;

    public override void _Ready()
    {
        carSprite = GetNode<AnimatedSprite2D>("Car");
        blinkTimer = GetNode<Timer>("BlinkTimer");
        shieldTimer = GetNode<Timer>("ShieldTimer");

        blinkTimer.Timeout += onBlinkTimerTimeout;
        shieldTimer.Timeout += onShieldTimerTimeout;

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
        float moveSpeed = 100.0f;

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
                vehicle.QueueFree();
            }
            else if (body is CargoTruck)
            {
                takeDamage(1);
            }else if (body is Player)
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

    private void onBlinkTimerTimeout()
    {
        if (hasShield)
        {
            carSprite.Visible = !carSprite.Visible;
        }
    }

    private void onShieldTimerTimeout()
    {
        hasShield = false;
        carSprite.Visible = true;
    }

    public void takeDamage(int amount)
    {
        if(!hasShield)
        {
            health -= amount;

            hasShield = true;
            shieldTimer.Start();
            if (health <= 0)
            {
                health = 0;
                totalEscaped++;
                QueueFree();
            }

            GetNode<Label>("%HealthLabel").Text = "x" + health.ToString();
        }
    }
}
