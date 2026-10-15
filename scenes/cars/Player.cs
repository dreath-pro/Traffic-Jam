using Godot;
public partial class Player : CharacterBody2D
{
    public int health { get; private set; } = 6;
    [Signal] public delegate void GameOverEventHandler();

    private AnimatedSprite2D carSprite, characterSprite;
    private Timer blinkTimer;
    private Timer shieldTimer;

    private bool hasShield = false;

    public override void _PhysicsProcess(double delta)
    {
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
    }

    public override void _Ready()
    {
        carSprite = GetNode<AnimatedSprite2D>("Car");
        characterSprite = GetNode<AnimatedSprite2D>("Character");
        blinkTimer = GetNode<Timer>("BlinkTimer");
        shieldTimer = GetNode<Timer>("ShieldTimer");

        blinkTimer.Timeout += onBlinkTimerTimeout;
        shieldTimer.Timeout += onShieldTimerTimeout;
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
                gameOverLogic();
            }

            GetNode<Label>("%HealthLabel").Text = "x" + health.ToString();
        }
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

    private void gameOverLogic()
    {
        health = 0;
        QueueFree();
        EmitSignal(SignalName.GameOver);
    }
}
