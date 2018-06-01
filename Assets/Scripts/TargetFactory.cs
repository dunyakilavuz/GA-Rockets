using Godot;
using System.Collections;
using System;
using GodotExt.Maths;
using GodotExt.Engine;

using Vector3 = GodotExt.Maths.Vector3;

public class TargetFactory : Node
{
    string targetsPath = "/root/MainScene/Scene/Targets";
    Node targets;
    public override void _Ready()
    {
        targets = GetNode(targetsPath);
    }
    public Target createTarget()
    {
        var targetScene = (PackedScene)ResourceLoader.Load("res://Assets/Prefabs/Target.res");
        Target target = (Target) targetScene.Instance();
        target.SetName("Target");
        targets.AddChild(target);
        return target;
    }
}