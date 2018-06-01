using Godot;
using System.Collections;
using System;
using GodotExt.Maths;
using GodotExt.Engine;

using Vector3 = GodotExt.Maths.Vector3;
public class Target : Spatial
{
    public TransformExt transform;
    float scale;
    public override void _Ready()
    {
        transform = new TransformExt();
        scale = Maths.RandomFloat(References.targetMinScale,References.targetMaxScale);
        transform.scale = new Vector3(scale,scale,scale);
        transform.position = new Vector3(
                    Maths.RandomFloat(-References.targetHorizontalRange,References.targetHorizontalRange),
                    Maths.RandomFloat(References.targetStartHeight ,References.targetEndHeight),
                    0);
    }

    public override void _Process(float delta)
    {

        SetTransform(TransformExt.ToTransform(transform));
    }

    
}
