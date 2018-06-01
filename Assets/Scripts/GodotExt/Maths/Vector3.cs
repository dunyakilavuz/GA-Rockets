using Godot;
using System;

namespace GodotExt
{
    namespace Maths
    {
        public class Vector3
        {
            public static Vector3 up = new Vector3(0,1,0);
            public static Vector3 down = new Vector3(0,-1,0);
            public static Vector3 right = new Vector3(1,0,0);
            public static Vector3 left = new Vector3(-1,0,0);
            public static Vector3 forward = new Vector3(0,0,1);
            public static Vector3 back = new Vector3(0,0,-1);
            public static Vector3 zero = new Vector3(0,0,0);
            public static Vector3 one = new Vector3(1,1,1);
            public float x;
            public float y;
            public float z;


            public Vector3(float x, float y, float z)
            {
                this.x = x;
                this.y = y;
                this.z = z;
            }

            public static Vector3 operator *(Vector3 vector, float scale)
            {
                return new Vector3(vector.x * scale, vector.y * scale, vector.z * scale);
            }

            public static Vector3 operator /(Vector3 vector, float scale)
            {
                return new Vector3(vector.x / scale, vector.y / scale, vector.z / scale);
            }

            public static Vector3 operator *(float scale, Vector3 vector)
            {
                return new Vector3(vector.x * scale, vector.y * scale, vector.z * scale);
            }
            
            public static Vector3 operator +(Vector3 A, Vector3 B)
            {
                return new Vector3(A.x + B.x, A.y + B.y, A.z + B.z);
            }
            
            public static Vector3 operator -(Vector3 A, Vector3 B)
            {
                return new Vector3(A.x - B.x, A.y - B.y, A.z - B.z);	
            }

            public static Vector3 operator -(Vector3 A)
            {
                return new Vector3(-A.x, -A.y, -A.z);	
            }
            
            public static Vector3 operator *(Vector3 A, Vector3 B)
            {
                return new Vector3(A.x * B.x, A.y * B.y, A.z * B.z);
            }

            public static Vector3 FromV3(Godot.Vector3 godotV)
            {
                return new Vector3(godotV.x, godotV.y, godotV.z);
            }

            public static Godot.Vector3 Tov3(Vector3 v)
            {
                return new Godot.Vector3(v.x,v.y,v.z);
            }


            public static Vector3 FromV2(Godot.Vector2 godotV)
            {
                return new Vector3(godotV.x, godotV.y, 0);
            }
            public static Godot.Vector2 ToV2(Vector3 v)
            {
                return new Godot.Vector2(v.x,v.y);
            }

            public static Vector3 Cross(Vector3 lhs, Vector3 rhs)
            {
                return new Vector3(lhs.y * rhs.z - lhs.z * rhs.y,lhs.z * rhs.x - lhs.x * rhs.z,lhs.x * rhs.y - lhs.y * rhs.x);
            }


            public static float AngleTo(Vector3 source, Vector3 dest, Vector3 planeNormal)
            {
                source = source.normalized;
                dest = dest.normalized;
                planeNormal = planeNormal.normalized;

                if (source == dest)
                {
                    return 0;
                }
                float dot = Vector3.Dot(source, dest);
                float angle = Mathf.Acos(dot);
                Vector3 cross = Vector3.Cross(source,dest);
                float signDot = Vector3.Dot(cross, planeNormal);

                if(signDot < 0)
                {
                    return -angle * Maths.Rad2Deg;
                }
                else 
                {
                    return angle * Maths.Rad2Deg;;
                }
            }

            public static float Dot(Vector3 vector1, Vector3 vector2)
            {
                return (vector1.x * vector2.x) + (vector1.y * vector2.y) + (vector1.z * vector2.z);
            }

            public static Vector3 Lerp(Vector3 start, Vector3 end, float factor)
            {
                if(factor > 1)
                    factor = 1;
                if(factor < 0)
                    factor = 0;

                return new Vector3(Mathf.Lerp(start.x,end.x,factor),Mathf.Lerp(start.y,end.y,factor),Mathf.Lerp(start.z,end.z,factor));
            }
            public static Vector3 Normalize(Vector3 vector)
            {
                if(Magnitude(vector) != 0)
                {
                    return new Vector3(vector.x /Magnitude(vector),vector.y / Magnitude(vector),vector.z / Magnitude(vector));			
                }
                else
                {
                    return new Vector3(0,0,0);
                }
            }

            public static Vector3 NegateZ(Vector3 v)
            {
                return new Vector3(v.x,v.y,-v.z);
            }

            public static float Magnitude(Vector3 vector)
            {
                return Mathf.Sqrt((vector.x * vector.x) + (vector.y * vector.y) + (vector.z * vector.z)) ;
            }

            public static float Distance(Vector3 a, Vector3 b)
            {
                return Magnitude(a-b);
            }


            public float magnitude
            {
                get
                {
                    return Magnitude(this);
                }
            }

            public Vector3 normalized
            {
                get
                {
                    return Normalize(this);
                }
            }

            public Godot.Vector3 toV3
            {
                get
                {
                    return Tov3(this);
                }
            }

            public Godot.Vector2 toV2
            {
                get
                {
                    return ToV2(this);
                }
            }

            public Vector3 negateZ
            {
                get
                {
                    return NegateZ(this);
                }
            }

        }
    }
}

