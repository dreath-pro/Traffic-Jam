using Godot;
using System;

public partial class Car : CharacterBody2D
{
    [Export] public String entityName;
    [Export] public int health;

    [Export] private Godot.Collections.Array<string> carAnimationNames, characterAnimationNames;
    private AnimatedSprite2D carSprite, characterSprite;

    private Timer blinkTimer;
    private Timer shieldTimer;
    private bool hasShield = false;

    [Signal] public delegate void GameOverEventHandler();

    public static int totalEscaped = 0;
    private Player player;

    private RandomNumberGenerator randomNumber = new RandomNumberGenerator();

    private Timer stuckTimer;
    private Vector2 previousPosition;

    public override void _PhysicsProcess(double delta)
    {
        switch(entityName)
        {
            case "Player":
                Vector2 direction = Input.GetVector("move_left", "move_right", "move_up", "move_down");

                Velocity = direction * 380;
                MoveAndSlide();

                for (int i = 0; i < GetSlideCollisionCount(); i++)
                {
                    var collision = GetSlideCollision(i);
                    var body = collision.GetCollider();

                    if (body is Cops)
                    {
                        takeDamage(3);
                    }
                    else if (body is CargoTruck)
                    {
                        takeDamage(1);
                    }
                }
                break;

            case "Vehicle":
                MoveAndSlide();

                for (int i = 0; i < GetSlideCollisionCount(); i++)
                {
                    var collision = GetSlideCollision(i);
                    var body = collision.GetCollider();

                    if (body is Vehicles vehicle)
                    {
                        QueueFree();
                    }
                    else if (body is Cops cop)
                    {
                        cop.takeDamage(1);
                        QueueFree();
                    }
                    else if (body is CargoTruck truck)
                    {
                        QueueFree();
                    }
                    else if (body is Player player)
                    {
                        player.takeDamage(1);
                        QueueFree();
                    }
                }

                if (GlobalPosition.X < -389)
                    QueueFree();
                break;

            case "Cops":
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
                    }
                    else if (body is Player)
                    {
                        takeDamage(1);
                    }
                }

                if (GlobalPosition.X > 2000)
                {
                    if (GlobalPosition.X < 2800)
                    {
                        totalEscaped++;
                        QueueFree();
                    }

                    QueueFree();
                }
                break;

            case "Truck":
                Velocity = Vector2.Left * 100;
                MoveAndSlide();

                if (GlobalPosition.X < -783)
                    QueueFree();
                break;

            default:
                GD.PrintErr("Please add or double check your entity name");
                break;
        }
    }

    public override void _Ready()
    {
        carSprite = GetNode<AnimatedSprite2D>("Car");
        characterSprite = GetNode<AnimatedSprite2D>("Character");
        blinkTimer = GetNode<Timer>("BlinkTimer");
        shieldTimer = GetNode<Timer>("ShieldTimer");

        switch (entityName)
        {
            case "Player":
                blinkTimer.Timeout += onBlinkTimerTimeout;
                shieldTimer.Timeout += onShieldTimerTimeout;
                break;

            case "Vehicle":
                randomCarAnimation();
                randomCharacterAnimation();

                Timer decision = GetNode<Timer>("Decision");
                decision.Timeout += Decision;
                Decision();

                Timer steady = GetNode<Timer>("Steady");
                steady.Timeout += Steady;
                Steady();
                break;

            case "Cops":
                blinkTimer.Timeout += onBlinkTimerTimeout;
                shieldTimer.Timeout += onShieldTimerTimeout;

                GetNode<Label>("%HealthLabel").Text = "x" + health.ToString();

                player = GetTree().GetFirstNodeInGroup("player") as Player;

                if (player == null)
                {
                    GD.PrintErr("Player not found! Check the player group.");
                }
                break;

            case "Truck":
                stuckTimer = GetNode<Timer>("StuckTimer");

                stuckTimer.Timeout += onStuckTimerTimeout;
                break;

            default:
                GD.PrintErr("Please add or double check your entity name");
                break; ;
        }
    }

    private void randomCharacterAnimation()
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

    private void Steady()
    {
        MoveLeft();
    }

    private void Decision()
    {
        int choice = randomNumber.RandiRange(0, 3);

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

    private void onBlinkTimerTimeout()
    {
        if (hasShield)
        {
            carSprite.Visible = !carSprite.Visible;
            if (entityName == "Player")
            {
                characterSprite.Visible = !characterSprite.Visible;
            }
        }
    }

    private void onShieldTimerTimeout()
    {
        hasShield = false;
        carSprite.Visible = true;

        if (entityName == "Player")
        {
            characterSprite.Visible = true;
        }
    }

    public void takeDamage(int amount)
    {
        if (!hasShield)
        {
            health -= amount;

            hasShield = true;
            shieldTimer.Start();
            if (health <= 0)
            {
                if (entityName == "Player")
                {
                    gameOverLogic();
                }else if (entityName == "Cops")
                {
                    health = 0;
                    totalEscaped++;
                    QueueFree();
                }
            }

            GetNode<Label>("%HealthLabel").Text = "x" + health.ToString();
        }
    }

    private void gameOverLogic()
    {
        health = 0;
        QueueFree();
        EmitSignal(SignalName.GameOver);
    }

    private void onStuckTimerTimeout()
    {
        float distanceMoved = previousPosition.DistanceTo(GlobalPosition);

        if (distanceMoved < 5f)
        {
            GD.Print("Truck is stationary");
            QueueFree();
        }
        else
        {
            previousPosition = GlobalPosition;
        }
    }
}
