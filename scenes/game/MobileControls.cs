using Godot;
using System;

public partial class MobileControls : CanvasLayer
{
    public override void _Ready()
    {
        Visible = DisplayServer.IsTouchscreenAvailable();
    }
}
