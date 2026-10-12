using Godot;
using System;

public partial class Game : Node2D
{
    int seconds = 0, minutes = 0, hours = 0;

    public override void _Process(double delta)
    {

    }

    public override void _Ready()
    {
        Timer timeCount = GetNode<Timer>("%TimeCount");
        timeCount.Timeout += DisplayTime;
        DisplayTime();
    }

    private void DisplayTime()
    {
        String timeText = "";
        seconds++;

        if (seconds > 59)
        {
            minutes++;
            seconds = 0;
        }
        if (minutes > 59)
        {
            hours++;
            minutes = 0;
            seconds = 0;
        }

        if (hours > 0)
        {
            if (hours < 10)
            {
                timeText = "0" + hours + " : ";
            }
            else
            {
                timeText = hours + " : ";
            }
        }

        if (minutes < 10)
        {
            if (hours == 0)
            {
                timeText += minutes + " : ";
            }else
            {
                timeText += "0" + minutes + " : ";
            }
        }
        else
        {
            timeText += minutes + " : ";
        }

        if (seconds < 10)
        {
            timeText += "0" + seconds;
        }
        else
        {
            timeText += seconds;
        }

        GetNode<Label>("%TimeLabel").Text = timeText;
    }
}
