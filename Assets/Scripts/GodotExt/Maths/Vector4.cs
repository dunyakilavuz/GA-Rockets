using Godot;
using System;

namespace GodotExt
{
    namespace Maths
    {
        public class Vector4 
        {
            public float x;
            public float y;
            public float z;
            public float w;

            public static Vector4 zero = new Vector4(0,0,0,0);
            
            public Vector4(float x, float y, float z,float w) 
            {
                this.x = x;
                this.y = y;
                this.z = z;
                this.w = w;
            }
            
            public Vector4(Vector3 vector, float w)
            {
                this.x = vector.x;
                this.y = vector.y;
                this.z = vector.z;
                this.w = w;
            }

            public static Vector4 MultiplyByMatrix4x4(Vector4 vector ,Matrix4x4 matrix)
            {
                Vector4 result = Vector4.zero;
                result.x = (vector.x * matrix.matrix[0][0]) + (vector.y * matrix.matrix[1][0]) + (vector.z * matrix.matrix[2][0]) + (vector.w * matrix.matrix[3][0]);
                result.y = (vector.x * matrix.matrix[0][1]) + (vector.y * matrix.matrix[1][1]) + (vector.z * matrix.matrix[2][1]) + (vector.w * matrix.matrix[3][1]);
                result.z = (vector.x * matrix.matrix[0][2]) + (vector.y * matrix.matrix[1][2]) + (vector.z * matrix.matrix[2][2]) + (vector.w * matrix.matrix[3][2]);
                result.w = (vector.x * matrix.matrix[0][3]) + (vector.y * matrix.matrix[1][3]) + (vector.z * matrix.matrix[2][3]) + (vector.w * matrix.matrix[3][3]);
                return result;
            }

            public Vector3 toVector3
            {
                get
                {
                    return new Vector3(x,y,z);
                }
            }
        }
    }
}