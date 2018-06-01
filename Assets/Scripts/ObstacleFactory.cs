using Godot;
using System.Collections;
using System;
using GodotExt.Maths;
using GodotExt.Engine;

using Vector3 = GodotExt.Maths.Vector3;

public class ObstacleFactory : Node
{
    string obstaclesPath = "/root/MainScene/Scene/Obstacles";
    Node obstacles;
    int index;

    public override void _Ready()
    {
        obstacles = GetNode(obstaclesPath);
        index = 1;
    }
    public Obstacle createObstacle()
    {
        var obstacleScene = (PackedScene)ResourceLoader.Load("res://Assets/Prefabs/Obstacle.res");
        Obstacle obstacle = (Obstacle) obstacleScene.Instance();
        obstacle.SetName("Obstacle" + index);
        obstacles.AddChild(obstacle);
        index++;
        return obstacle;
    }

    public Obstacle[] createPopulation(int size)
    {
        Obstacle[] obstacles = new Obstacle[size];
        
        for (int i = 0; i < size; i++)
        {
            obstacles[i] = createObstacle();
        }
        return obstacles;
    }
}