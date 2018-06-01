using Godot;
using System;
using GodotExt.Maths;

using Vector3 = GodotExt.Maths.Vector3;

namespace GodotExt
{
    namespace Engine
    {
        public class TransformExt
        {
			public Vector3 position;
			public Quaternion rotation;
			public Vector3 scale;
			Matrix4x4 worldMatrix;

			public bool RHStoLHS = true;
			public Vector3 posLHS;


			public TransformExt()
			{
				position = new Vector3(0,0,0);
				rotation = Quaternion.identity;
				scale = new Vector3(1,1,1);
				posLHS = position;
				worldMatrix = Matrix4x4.identity;
			}

			public TransformExt(Spatial spatial)
			{
				position =  Vector3.FromV3(spatial.GetTranslation());
				rotation = Quaternion.FromEulerRHS(Vector3.FromV3((spatial.GetRotationDegrees())));
				scale =  Vector3.FromV3(spatial.GetScale());
				posLHS = new Vector3(position.x,position.y,-position.z);
				updateTransformMatrix();
			}

			public void RotateAround(Vector3 point, Vector3 axis, float angle)
			{
				float distance = (position - point).magnitude;
				Vector3 direction = (position - point).normalized;
				Quaternion rotate = Quaternion.AngleAxis(angle, axis);
				direction = rotate * direction;
				position = point + direction * distance;
			}
			
			void updateTransformMatrix()
			{
				worldMatrix = Matrix4x4.translationMatrix(position) * Matrix4x4.scaleMatrix(scale) * Matrix4x4.rotationMatrix(rotation);
			}

			void updateTransformMatrix(Vector3 LHS)
			{
				worldMatrix = Matrix4x4.translationMatrix(LHS) * Matrix4x4.scaleMatrix(scale) * Matrix4x4.rotationMatrix(rotation);
			}

			public static TransformExt FromTransform(Transform transform, Vector3 rotation)
			{
				TransformExt transformExt = new TransformExt();
				transformExt.worldMatrix = Matrix4x4.TransformToMatrix4x4(transform);
				transformExt.position = Vector3.FromV3(transform.origin);
				transformExt.rotation = Quaternion.FromEuler(rotation);
				transformExt.scale = new Vector3(1,1,1);
				return transformExt;
			}

			public static Transform ToTransform(TransformExt transformExt)
			{
				if(transformExt.RHStoLHS == true)
				{
					transformExt.posLHS = new Vector3(transformExt.position.x, transformExt.position.y, -transformExt.position.z);
					transformExt.updateTransformMatrix(transformExt.posLHS);
				}
				else
				{
					transformExt.updateTransformMatrix();
				}
				return Matrix4x4.Matrix4x4ToTransform(transformExt.worldMatrix);
			}

			public Transform toTransform()
			{
				return ToTransform(this);
			}

			public Vector3 right
			{
				get
				{
					return new Vector3(worldMatrix.ColumnToVector4(0).x,worldMatrix.ColumnToVector4(0).y,worldMatrix.ColumnToVector4(0).z).normalized;
				}
				
			}
			public Vector3 left
			{
				get
				{
					return -new Vector3(worldMatrix.ColumnToVector4(0).x,worldMatrix.ColumnToVector4(0).y,worldMatrix.ColumnToVector4(0).z).normalized;
				}
			}
			public Vector3 up
			{
				get
				{
					return new Vector3(-worldMatrix.ColumnToVector4(1).x,worldMatrix.ColumnToVector4(1).y,worldMatrix.ColumnToVector4(1).z).normalized;	
				}
			}
			
			public Vector3 down
			{
				get
				{
					return -new Vector3(worldMatrix.ColumnToVector4(1).x,worldMatrix.ColumnToVector4(1).y,worldMatrix.ColumnToVector4(1).z).normalized;
				}
			}
			
			public Vector3 forward
			{
				get
				{
					return new Vector3(worldMatrix.ColumnToVector4(2).x,worldMatrix.ColumnToVector4(2).y,worldMatrix.ColumnToVector4(2).z).normalized;	
				}
			}
			
			public Vector3 back
			{
				get
				{
					return -new Vector3(worldMatrix.ColumnToVector4(2).x,worldMatrix.ColumnToVector4(2).y,worldMatrix.ColumnToVector4(2).z).normalized;
				}
			}

        }
    }
}