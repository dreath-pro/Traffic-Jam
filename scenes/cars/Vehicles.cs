using Godot;
using System;

public partial class Vehicles : CharacterBody2D
{
    private RandomNumberGenerator rng = new RandomNumberGenerator();

    [Export] private Godot.Collections.Array<string> carAnimationNames;
    private AnimatedSprite2D carAnimateSprite;

    [Export] private Godot.Collections.Array<string> characterAnimationNames;
    private AnimatedSprite2D characterAnimateSprite;

    public override void _PhysicsProcess(double delta)
    {
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
            }else if (body is Player player)
            {
                player.takeDamage(1);
                QueueFree();
            }
        }

        if (GlobalPosition.X < -100)
            QueueFree();
    }

    public override void _Ready()
    {
        carAnimateSprite = GetNode<AnimatedSprite2D>("Car");
        randomCarAnimation();

        characterAnimateSprite = GetNode<AnimatedSprite2D>("Character");
        randomCharacterAnimation();

        Timer decision = GetNode<Timer>("Decision");
        decision.Timeout += Decision;
        Decision();

        Timer steady = GetNode<Timer>("Steady");
        steady.Timeout += Steady;
        Steady();
    }

    private void randomCharacterAnimation()
    {
        if (characterAnimateSprite.SpriteFrames == null)
        {
            GD.PrintErr("No SpriteFrames resource assigned to the AnimatedSprite2D!");
            return;
        }

        var animationNames = characterAnimateSprite.SpriteFrames.GetAnimationNames();
        if (animationNames.Length == 0)
        {
            GD.PrintErr("No animations found in the sprite frame!");
            return;
        }

        int randomIndex = rng.RandiRange(0, animationNames.Length - 1);
        string chosenAnimation = characterAnimationNames[randomIndex];
        characterAnimateSprite.Play(chosenAnimation);
    }

    private void randomCarAnimation()
    {
        if (carAnimateSprite.SpriteFrames == null)
        {
            GD.PrintErr("No SpriteFrames resource assigned to the AnimatedSprite2D!");
            return;
        }

        var animationNames = carAnimateSprite.SpriteFrames.GetAnimationNames();
        if (animationNames.Length == 0)
        {
            GD.PrintErr("No animations found in the sprite frame!");
            return;
        }

        int randomIndex = rng.RandiRange(0, animationNames.Length - 1);
        string chosenAnimation = carAnimationNames[randomIndex];
        carAnimateSprite.Play(chosenAnimation);
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
        int choice = rng.RandiRange(0, 3);

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
}
