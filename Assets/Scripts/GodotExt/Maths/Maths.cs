using Godot;
using System;

namespace GodotExt
{
    namespace Maths
    {
        public class Maths
        {
            public static float PI = 3.14159265359f;
            static Random random;
            public static float EaseInOut(float from, float to, float factor)
            {
                int a = 2;
                float result = (Mathf.Pow(factor,a))/(Mathf.Pow(factor,a) + Mathf.Pow(1 - factor,a));
                float slope = (to - from) / (1 - 0);
                return result * slope;
            }

            public static float LerpAngle(float start, float end, float factor)
            {
                float difference = Math.Abs(end - start);
                if (difference > 180)
                {
                    if (end > start)
                        start += 360;
                    else
                        end += 360;
                }
                float value = (start + ((end - start) * factor));
                float rangeZero = 360;

                if (value >= 0 && value <= 360)
                    return value;
                    
                return (value % rangeZero);
            }


            public static float RandomFloat(float min, float max)
            {
                if(random == null)
                {
                    random = new Random();
                }
                return (float)(random.NextDouble() * (max - min) + min);
            }

            public static int RandomInt(int min, int max)
            {
                if(random == null)
                {
                    random = new Random();
                }
                return random.Next(min,max);
            }

 
            public static float ClampAngle(float angle, float min, float max)
            {
                angle = Maths.RepeatAngle(angle);
                min = Maths.RepeatAngle(min);
                max = Maths.RepeatAngle(max);
                bool inverse = false;
                var tmin = min;
                var tangle = angle;
                if(min > 180)
                {
                    inverse = !inverse;
                    tmin -= 180;
                }
                if(angle > 180)
                {
                    inverse = !inverse;
                    tangle -= 180;
                }
                var result = !inverse ? tangle > tmin : tangle < tmin;
                if(!result)
                    angle = min;

                inverse = false;
                tangle = angle;
                var tmax = max;
                if(angle > 180)
                {
                    inverse = !inverse;
                    tangle -= 180;
                }
                if(max > 180)
                {
                    inverse = !inverse;
                    tmax -= 180;
                }
            
                result = !inverse ? tangle < tmax : tangle > tmax;
                if(!result)
                    angle = max;
                return angle;
            }

            public static float RepeatAngle(float angle)
            {
                return  angle - Mathf.Floor(angle / 360.0f) * 360.0f;
            }

            public static float Deg2Rad 
            { 
                get
                {
                    return PI / 180f;
                }
            }
            public static float Rad2Deg 
            {
                get
                {
                    return 180f / PI;;
                }
            }
        }
    }
}