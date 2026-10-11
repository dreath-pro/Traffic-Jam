using Godot;
using System;

public partial class Player : CharacterBody2D
{
    public override void _PhysicsProcess(double delta)
    {
        Vector2 direction = Input.GetVector("move_left", "move_right", "move_up", "move_down");
        Velocity = direction * 380;
        MoveAndSlide();
    }
}
