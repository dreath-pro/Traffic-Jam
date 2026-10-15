using Godot;
using System;

public partial class DamageLogic : CharacterBody2D
{
    [Signal] public delegate void GameOverEventHandler();
    [Export] public int maxHealth { get; set; } = 5;
    public int health { get; private set; }

    public static int totalEscaped { get; private set; }
    private Player player;

    private AnimatedSprite2D carSprite, characterSprite;
    private Timer blinkTimer;
    private Timer shieldTimer;

    private bool hasShield = false;

    public override void _Ready()
    {
        carSprite = GetNode<AnimatedSprite2D>("Car");
        characterSprite = GetNode<AnimatedSprite2D>("Character");
        blinkTimer = GetNode<Timer>("BlinkTimer");
        shieldTimer = GetNode<Timer>("ShieldTimer");

        blinkTimer.Timeout += onBlinkTimerTimeout;
        shieldTimer.Timeout += onShieldTimerTimeout;


        // if the class or script using this is cops
        GetNode<Label>("%HealthLabel").Text = "x" + health.ToString();

        player = GetTree().GetFirstNodeInGroup("player") as Player;

        if (player == null)
        {
            GD.PrintErr("Player not found! Check the player group.");
        }
    }

    public void takeDamage(int amount)
    {
        // for cops
        if (!hasShield)
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

        //for player
        if (!hasShield)
        {
            health -= amount;

            hasShield = true;
            shieldTimer.Start();
            if (health <= 0)
            {
                gameOverLogic();
            }

            GetNode<Label>("%HealthLabel").Text = "x" + health.ToString();
        }
    }

    private void gameOverLogic()
    {
        //for player
        health = 0;
        QueueFree();
        EmitSignal(SignalName.GameOver);
    }

    private void onBlinkTimerTimeout()
    {
        if (hasShield)
        {
            carSprite.Visible = !carSprite.Visible;
            characterSprite.Visible = !characterSprite.Visible;
        }
    }

    private void onShieldTimerTimeout()
    {
        hasShield = false;
        carSprite.Visible = true;
        characterSprite.Visible = true;
    }
}
