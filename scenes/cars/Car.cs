using Godot;
using System;

public partial class Car : CharacterBody2D
{
    [Export] public int maxHealth;
    public int health;

    [Export] private Godot.Collections.Array<string> carAnimationNames, characterAnimationNames;
    private AnimatedSprite2D carSprite, characterSprite;

    private Timer blinkTimer;
    private Timer shieldTimer;
    private bool hasShield = false;

    private Label healthLabel;

    [Signal] public delegate void GameOverEventHandler();

    public static int totalEscaped = 0;

    private RandomNumberGenerator randomNumber = new RandomNumberGenerator();

    public override void _PhysicsProcess(double delta)
    {
        for (int i = 0; i < GetSlideCollisionCount(); i++)
        {
            var collision = GetSlideCollision(i);
            var body = collision.GetCollider() as CollisionObject2D;
            takeDamage(body);
        }
    }

    public override void _Ready()
    {
        health = maxHealth;

        carSprite = GetNodeOrNull<AnimatedSprite2D>("Car");
        characterSprite = GetNodeOrNull<AnimatedSprite2D>("Character");
        blinkTimer = GetNodeOrNull<Timer>("BlinkTimer");
        shieldTimer = GetNodeOrNull<Timer>("ShieldTimer");
        healthLabel = GetNodeOrNull<Label>("%HealthLabel");

        if (blinkTimer != null)
            blinkTimer.Timeout += onBlinkTimerTimeout;

        if (shieldTimer != null)
            shieldTimer.Timeout += onShieldTimerTimeout;

        randomCarAnimation();
        randomCharacterAnimation();
    }

    private void randomCharacterAnimation()
    {
        if (characterSprite != null)
        {
            if (characterSprite.SpriteFrames == null)
            {
                GD.PrintErr("No SpriteFrames resource assigned to the AnimatedSprite2D!");
                return;
            }

            var animationNames = characterSprite.SpriteFrames.GetAnimationNames();
            if (animationNames.Length == 0)
            {
                GD.PrintErr("No animations found in the sprite frame!");
                return;
            }

            int randomIndex = randomNumber.RandiRange(0, animationNames.Length - 1);
            string chosenAnimation = characterAnimationNames[randomIndex];
            characterSprite.Play(chosenAnimation);
        }
    }

    private void randomCarAnimation()
    {
        if (carSprite.SpriteFrames == null)
        {
            GD.PrintErr("No SpriteFrames resource assigned to the AnimatedSprite2D!");
            return;
        }

        var animationNames = carSprite.SpriteFrames.GetAnimationNames();
        if (animationNames.Length == 0)
        {
            GD.PrintErr("No animations found in the sprite frame!");
            return;
        }

        int randomIndex = randomNumber.RandiRange(0, animationNames.Length - 1);
        string chosenAnimation = carAnimationNames[randomIndex];
        carSprite.Play(chosenAnimation);
    }

    public void Heal()
    {
        health++;

        if (health > maxHealth)
        {
            health = maxHealth;
        }

        if (healthLabel != null)
            healthLabel.Text = "x" + health.ToString();
    }

    public void takeDamage(CollisionObject2D body)
    {
        if (this is CargoTruck)
            return;

        if (hasShield)
            return;

        if (body is Vehicles)
            health -= 1;
        if (body is CargoTruck)
            health -= 1;
        if (body is Player)
            health -= 1;
        if (body is Cops)
            health -= 3;
        if (body is Ambulance)
            health -= 5;

        if (this is Player && body is StaticBody2D)
            return;

        if (this is Ambulance && body is StaticBody2D)
        {
            return;
        }

        if (this is Player && body is Ambulance)
            return;

        if (this is Ambulance && body is Player)
            return;

        hasShield = true;

        if (shieldTimer != null)
            shieldTimer.Start();

        if (health <= 0)
        {
            if (this is Player)
                gameOverLogic();

            if (this is Cops)
            {
                health = 0;
                totalEscaped++;
            }

            QueueFree();
        }

        if (healthLabel != null)
            healthLabel.Text = "x" + health.ToString();
    }

    private void onBlinkTimerTimeout()
    {
        if (hasShield)
        {
            carSprite.Visible = !carSprite.Visible;

            if (characterSprite != null)
                characterSprite.Visible = !characterSprite.Visible;
        }
    }

    private void onShieldTimerTimeout()
    {
        hasShield = false;
        carSprite.Visible = true;

        if (characterSprite != null)
            characterSprite.Visible = true;
    }

    private void gameOverLogic()
    {
        health = 0;
        QueueFree();
        EmitSignal(SignalName.GameOver);
    }
}
