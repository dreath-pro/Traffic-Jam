using Godot;
using System;
using System.Runtime.CompilerServices;

public partial class Homescreen : Control
{
    private Button playButton, exitButton;
    public override void _Ready()
    {
        playButton = GetNodeOrNull<Button>("%PlayButton");

        if (playButton == null )
        {
            GD.PrintErr("Your play button is null!");
        }
        playButton.Pressed += OnPlayPressed;

        exitButton = GetNode<Button>("%ExitButton");
        if (exitButton == null)
        {
            GD.PrintErr("Your exit button is null!");
        }
        exitButton.Pressed += OnExitPressed;
    }

    private void OnPlayPressed()
    {
        GetTree().ChangeSceneToFile("res://scenes/game/game.tscn");
    }

    private void OnExitPressed()
    {
        GetTree().Quit();
    }

}
