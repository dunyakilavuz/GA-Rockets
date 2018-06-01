using Godot;
using System.Collections;
using System;
using GodotExt.Maths;
using GodotExt.Engine;

using Vector3 = GodotExt.Maths.Vector3;


public class Obstacle : Spatial
{
    public TransformExt transform;
    float scale;
    public override void _Ready()
    {
        scale = Maths.RandomFloat(References.obstacleMinScale,References.obstacleMaxScale);
        transform = new TransformExt();
        transform.scale = new Vector3(scale,scale,scale);
        transform.position = new Vector3(
                    Maths.RandomFloat(-References.obstacleHorizontalRange,References.obstacleHorizontalRange),
                    Maths.RandomFloat(References.obstacleStartHeight,References.obstacleEndHeight),
                    0);
    }

    public override void _Process(float delta)
    {

        SetTransform(TransformExt.ToTransform(transform));
    }

    
}
