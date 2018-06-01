using Godot;
using System.Collections;
using System;
using GodotExt.Maths;
using GodotExt.Engine;

using Vector3 = GodotExt.Maths.Vector3;

public class RocketFactory : Node
{
    string populationPath = "/root/MainScene/Scene/Population";
    Node population;
    int index;

    public override void _Ready()
    {
        population = GetNode(populationPath);
        index = 1;
    }
    public Rocket createRocket()
    {
        var rocketScene = (PackedScene)ResourceLoader.Load("res://Assets/Prefabs/Rocket.res");
        Rocket rocket = (Rocket) rocketScene.Instance();
        rocket.SetName("Rocket" + index);
        population.AddChild(rocket);
        index++;
        return rocket;
    }

    public Rocket[] createPopulation(int size)
    {
        Rocket[] rockets = new Rocket[size];
        for (int i = 0; i < size; i++)
        {
            rockets[i] = createRocket();
        }
        return rockets;
    }
}