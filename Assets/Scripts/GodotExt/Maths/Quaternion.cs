using Godot;
using System;

using Vector3 = GodotExt.Maths.Vector3;

namespace GodotExt
{
	namespace Maths
	{

		public class Quaternion 
		{
			public float w;
			public float x;
			public float y;
			public float z;

			public Quaternion(float w, float x,float y, float z) 
			{
				this.w = w;
				this.x = x;
				this.y = y;
				this.z = z;
			}
			
			public void normalized()
			{
				float lenght = (float) Math.Sqrt(w*w + x*x + y*y + z*z);
				w /= lenght;
				x /= lenght;
				y /= lenght;
				z /= lenght;
			}
			
			public static Quaternion identity
			{
				get
				{
					return new Quaternion(1,0,0,0);
				}
			}
			
			public static Quaternion AngleAxis(float angle, Vector3 axis)
			{	
				angle = Mathf.Deg2Rad(angle);
				float w = (float) Math.Cos(angle / 2);
				float x = (float) (axis.x * Math.Sin(angle / 2));
				float y = (float) (axis.y * Math.Sin(angle / 2));
				float z = (float) (axis.z * Math.Sin(angle / 2));
				return new Quaternion(w,x,y,z);
			}
			
			public static Quaternion operator *(Quaternion a, Quaternion b)
			{
				Normalize(a);
				Normalize(b);
				float newX = a.w * b.x + a.x * b.w + a.y * b.z - a.z * b.y;
				float newY = a.w * b.y + a.y * b.w + a.z * b.x - a.x * b.z;
				float newZ = a.w * b.z + a.z * b.w + a.x * b.y - a.y * b.x;
				float newW = a.w * b.w - a.x * b.x - a.y * b.y - a.z * b.z;
				return new Quaternion(newW, newX, newY, newZ);
			}
			
			public static Vector3 operator *(Quaternion q, Vector3 vector)
			{
				float num = q.x * 2f;
				float num2 = q.y * 2f;
				float num3 = q.z * 2f;
				float num4 = q.x * num;
				float num5 = q.y * num2;
				float num6 = q.z * num3;
				float num7 = q.x * num2;
				float num8 = q.x * num3;
				float num9 = q.y * num3;
				float num10 = q.w * num;
				float num11 = q.w * num2;
				float num12 = q.w * num3;
				Vector3 result = new Vector3(
						(1f - (num5 + num6)) * vector.x + (num7 - num12) * vector.y + (num8 + num11) * vector.z,
						(num7 + num12) * vector.x + (1f - (num4 + num6)) * vector.y + (num9 - num10) * vector.z,
						(num8 - num11) * vector.x + (num9 + num10) * vector.y + (1f - (num4 + num5)) * vector.z);
				
				return result;
			}

			

			public static Quaternion FromToRotation(Vector3 fromDirection, Vector3 toDirection)
			{
				return RotateTowards(LookRotation(fromDirection), LookRotation(toDirection), float.MaxValue);
			}

			public static Quaternion LookRotation(Vector3 direction, Vector3 up)
			{
				Vector3 forward = direction.normalized;
				Vector3 right = Vector3.Cross(up,forward).normalized;
				up = Vector3.Cross(forward,right);

				var m00 = right.x;
				var m01 = right.y;
				var m02 = right.z;
				var m10 = up.x;
				var m11 = up.y;
				var m12 = up.z;
				var m20 = forward.x;
				var m21 = forward.y;
				var m22 = forward.z;

				float num8 = (m00 + m11) + m22;
				var quaternion = new Quaternion(1,0,0,0);
				if (num8 > 0f)
				{
					var num = (float)System.Math.Sqrt(num8 + 1f);
					quaternion.w = num * 0.5f;
					num = 0.5f / num;
					quaternion.x = (m12 - m21) * num;
					quaternion.y = (m20 - m02) * num;
					quaternion.z = (m01 - m10) * num;
					return quaternion;
				}
				if ((m00 >= m11) && (m00 >= m22))
				{
					var num7 = (float)System.Math.Sqrt(((1f + m00) - m11) - m22);
					var num4 = 0.5f / num7;
					quaternion.x = 0.5f * num7;
					quaternion.y = (m01 + m10) * num4;
					quaternion.z = (m02 + m20) * num4;
					quaternion.w = (m12 - m21) * num4;
					return quaternion;
				}
				if (m11 > m22)
				{
					var num6 = (float)System.Math.Sqrt(((1f + m11) - m00) - m22);
					var num3 = 0.5f / num6;
					quaternion.x = (m10 + m01) * num3;
					quaternion.y = 0.5f * num6;
					quaternion.z = (m21 + m12) * num3;
					quaternion.w = (m20 - m02) * num3;
					return quaternion;
				}
				var num5 = (float)System.Math.Sqrt(((1f + m22) - m00) - m11);
				var num2 = 0.5f / num5;
				quaternion.x = (m20 + m02) * num2;
				quaternion.y = (m21 + m12) * num2;
				quaternion.z = 0.5f * num5;
				quaternion.w = (m01 - m10) * num2;
				return quaternion;
			}

			public static Quaternion LookRotation(Vector3 direction)
			{
				return LookRotation(direction,Vector3.up);
			}

			public static Quaternion LookRotationRHS(Vector3 direction, Vector3 up)
			{
				direction = new Vector3(direction.x,-direction.y,direction.z);
				return Quaternion.Inverse(LookRotation(direction,up));
			}

			public static Quaternion LookRotationRHS(Vector3 direction)
			{
				direction = new Vector3(direction.x,-direction.y,direction.z);
				return LookRotation(direction);
			}

			public static Quaternion Normalize(Quaternion q)
			{
				float lenght;
				Quaternion newQuaternion = q;
				lenght = (float) Math.Sqrt(
						newQuaternion.w * newQuaternion.w 
						+ newQuaternion.x * newQuaternion.x 
						+ newQuaternion.y * newQuaternion.y 
						+ newQuaternion.z * newQuaternion.z);
				
				if(lenght == 1)
				{
					return newQuaternion;
				}
				else
				{
					newQuaternion.w /= lenght;
					newQuaternion.x /= lenght;
					newQuaternion.y /= lenght;
					newQuaternion.z /= lenght;
				}
				return newQuaternion;
			}

			public static float Magnitude(Quaternion q)
			{
				return Mathf.Sqrt(q.w * q.w + q.x * q.x + q.y * q.y + q.z * q.z);
			}
			
			public static Vector3 ToEuler(Quaternion rotation)
			{
				float sqw = rotation.w * rotation.w;
				float sqx = rotation.x * rotation.x;
				float sqy = rotation.y * rotation.y;
				float sqz = rotation.z * rotation.z;
				float unit = sqx + sqy + sqz + sqw; // if normalised is one, otherwise is correction factor
				float test = rotation.x * rotation.w - rotation.y * rotation.z;
				Vector3 v = Vector3.zero;

				if (test > 0.4995f * unit)
				{ // singularity at north pole
					v.y = 2f * Mathf.Atan2(rotation.y, rotation.x);
					v.x = Maths.PI / 2;
					v.z = 0;
					return NormalizeAngles(v * Maths.Rad2Deg);
				}
				if (test < -0.4995f * unit)
				{ // singularity at south pole
					v.y = -2f * Mathf.Atan2(rotation.y, rotation.x);
					v.x = -Maths.PI / 2;
					v.z = 0;
					return NormalizeAngles(v * Maths.Rad2Deg);
				}
				Quaternion q = new Quaternion(rotation.w, rotation.z, rotation.x, rotation.y);
				v.y = (float)System.Math.Atan2(2f * q.x * q.w + 2f * q.y * q.z, 1 - 2f * (q.z * q.z + q.w * q.w));     // Yaw
				v.x = (float)System.Math.Asin(2f * (q.x * q.z - q.w * q.y));                             // Pitch
				v.z = (float)System.Math.Atan2(2f * q.x * q.y + 2f * q.z * q.w, 1 - 2f * (q.y * q.y + q.z * q.z));      // Roll
				return NormalizeAngles(v * Maths.Rad2Deg);
			}

			public static Vector3 NormalizeAngles(Vector3 v)
			{
				return new Vector3(Maths.RepeatAngle(v.x), Maths.RepeatAngle(v.y), Maths.RepeatAngle(v.z));
			}


			public Vector3 eulerAngles
			{
				get
				{
					return ToEuler(this);
				}
			}

			public static Vector3 ToEulerRHS(Quaternion q)
			{
				Vector3 lhsVec = ToEuler(q);
				lhsVec = new Vector3(-lhsVec.x, -lhsVec.y, lhsVec.z);
				return NormalizeAngles(lhsVec);
			}
			
			public static Quaternion FromEuler(Vector3 vector)
			{
				Vector3 rads = new Vector3((float)Mathf.Deg2Rad(vector.x),(float)Mathf.Deg2Rad(vector.y),(float)Mathf.Deg2Rad(vector.z));
				
				float hr = rads.z * 0.5f;
				float shr = (float)Math.Sin(hr);
				float chr = (float)Math.Cos(hr);
				float hp = rads.y * 0.5f;
				float shp = (float)Math.Sin(hp);
				float chp = (float)Math.Cos(hp);
				float hy = rads.x * 0.5f;
				float shy = (float)Math.Sin(hy);
				float chy = (float)Math.Cos(hy);
				float chy_shp = chy * shp;
				float shy_chp = shy * chp;
				float chy_chp = chy * chp;
				float shy_shp = shy * shp;
				
				return new Quaternion((chy_chp * chr) + (shy_shp * shr),(shy_chp * chr) - (chy_shp * shr),(chy_shp * chr) + (shy_chp * shr),(chy_chp * shr) - (shy_shp * chr));
			}

			public static Quaternion FromEulerRHS(Vector3 vector)
			{
				Vector3 lhsVec = new Vector3(-vector.x, -vector.y, vector.z);
				return FromEuler(lhsVec);
			}
			
			public static Quaternion Slerp(Quaternion start, Quaternion end,float factor)
			{
				if (factor > 1) factor = 1;
				if (factor < 0) factor = 0;
				return SlerpUnclamped(start, end, factor);
			}

			public static Quaternion QuaternionFromMatrix(Matrix4x4 m)
			{
				Quaternion q = new Quaternion(0,0,0,0);
				q.w = Mathf.Sqrt( Mathf.Max( 0, 1 + m.matrix[0][0] + m.matrix[1][1] + m.matrix[2][2] ) ) / 2; 
				q.x = Mathf.Sqrt( Mathf.Max( 0, 1 + m.matrix[0][0] - m.matrix[1][1] - m.matrix[2][2] ) ) / 2; 
				q.y = Mathf.Sqrt( Mathf.Max( 0, 1 - m.matrix[0][0] + m.matrix[1][1] - m.matrix[2][2] ) ) / 2; 
				q.z = Mathf.Sqrt( Mathf.Max( 0, 1 - m.matrix[0][0] - m.matrix[1][1] + m.matrix[2][2] ) ) / 2; 
				q.x *= Mathf.Sign( q.x * ( m.matrix[2][1] - m.matrix[1][2] ) );
				q.y *= Mathf.Sign( q.y * ( m.matrix[0][2] - m.matrix[2][0] ) );
				q.z *= Mathf.Sign( q.z * ( m.matrix[1][0] - m.matrix[0][1] ) );
				return q;
			}

			public static Quaternion Inverse(Quaternion q)
			{
				Quaternion newQuaternion;
				
				float squareOfElementSum = q.w * q.w + q.x * q.x + q.y * q.y + q.z * q.z;
				
				if (squareOfElementSum > 0.0)
				{
					float inverseSquareSum = 1.0f / squareOfElementSum;
					newQuaternion = new Quaternion(
							-q.x * inverseSquareSum,
							-q.y * inverseSquareSum,
							-q.z * inverseSquareSum,
							q.w * inverseSquareSum);
					
					return newQuaternion;
				}
				return null;
			}
			

			private static Quaternion SlerpUnclamped(Quaternion a, Quaternion b, float t)
			{
				// if either input is zero, return the other.
				if (Magnitude(a) * Magnitude(a) == 0.0f)
				{
					if (Magnitude(b) * Magnitude(b) == 0.0f)
					{
						return identity;
					}
					return b;
				}
				else if (Magnitude(b) * Magnitude(b) == 0.0f)
				{
					return a;
				}

				float cosHalfAngle = a.w * b.w + Vector3.Dot(new Vector3(a.x, a.y, a.z),new Vector3(b.x,b.y,b.z));

				Vector3.Dot(new Vector3(a.x, a.y, a.z),new Vector3(b.x,b.y,b.z));

				if (cosHalfAngle >= 1.0f || cosHalfAngle <= -1.0f)
				{
					// angle = 0.0f, so just return one input.
					return a;
				}
				else if (cosHalfAngle < 0.0f)
				{
					b.x = -b.x;
					b.y = -b.y;
					b.z = -b.z;
					b.w = -b.w;
					cosHalfAngle = -cosHalfAngle;
				}

				float blendA;
				float blendB;
				if (cosHalfAngle < 0.99f)
				{
					// do proper slerp for big angles
					float halfAngle = (float)System.Math.Acos(cosHalfAngle);
					float sinHalfAngle = (float)System.Math.Sin(halfAngle);
					float oneOverSinHalfAngle = 1.0f / sinHalfAngle;
					blendA = (float)System.Math.Sin(halfAngle * (1.0f - t)) * oneOverSinHalfAngle;
					blendB = (float)System.Math.Sin(halfAngle * t) * oneOverSinHalfAngle;
				}
				else
				{
					// do lerp if angle is really small.
					blendA = 1.0f - t;
					blendB = t;
				}
				Vector3 temp = blendA * new Vector3(a.x, a.y, a.z) + blendB * new Vector3(b.x, b.y, b.z);
				Quaternion result = new Quaternion(blendA * a.w + blendB * b.w, temp.x, temp.y, temp.z);
				if (Magnitude(result) * Magnitude(result) > 0.0f)
					return Normalize(result);
				else
					return identity;
			}

			public static Quaternion RotateTowards(Quaternion from, Quaternion to, float maxDegreesDelta)
			{
				float num = Quaternion.Angle(from, to);
				if (num == 0f)
				{
					return to;
				}
				float t = Math.Min(1f, maxDegreesDelta / num);
				return Quaternion.SlerpUnclamped(from, to, t);
			}

			public static float Angle(Quaternion a, Quaternion b)
			{
				float f = Quaternion.Dot(a, b);
				return Mathf.Acos(Mathf.Min(Mathf.Abs(f), 1f)) * 2f * Maths.Rad2Deg;
			}

			public static float Dot(Quaternion a, Quaternion b)
			{
				return a.x * b.x + a.y * b.y + a.z * b.z + a.w * b.w;
			}

		}
	}
}

