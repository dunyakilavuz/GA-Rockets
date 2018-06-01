using Godot;
using System;

namespace GodotExt
{
    namespace Maths
    {
        public class Matrix4x4 
        {
            public float[][] matrix = new float[4][];
            
            public Matrix4x4() 
            {
                matrix[0] = new float[4];
                matrix[1] = new float[4];
                matrix[2] = new float[4];
                matrix[3] = new float[4];
            }

            public static Vector3 TransformPoint(Vector3 v, Matrix4x4 m)
            {
                int w = 1;
                return new Vector3((v.x * m.matrix[0][0]) + (v.y * m.matrix[0][1]) + (v.z * m.matrix[0][2] + w * m.matrix[0][3]),
                                   (v.x * m.matrix[1][0]) + (v.y * m.matrix[1][1]) + (v.z * m.matrix[1][2] + w * m.matrix[1][3]),
                                   (v.x * m.matrix[2][0]) + (v.y * m.matrix[2][1]) + (v.z * m.matrix[2][2] + w * m.matrix[2][3]));
            }

            public static Vector3 TransformPointRHS(Vector3 v, Matrix4x4 m)
            {
                int w = 1;
                return new Vector3((v.x * m.matrix[0][0]) + (v.y * m.matrix[0][1]) + (v.z * m.matrix[0][2] + w * m.matrix[0][3]),
                                   (v.x * m.matrix[1][0]) + (v.y * m.matrix[1][1]) + (v.z * m.matrix[1][2] + w * m.matrix[1][3]),
                                   -((v.x * m.matrix[2][0]) + (v.y * m.matrix[2][1]) + (v.z * m.matrix[2][2] + w * m.matrix[2][3])));
            }
            public static Matrix4x4 TransformToMatrix4x4(Transform transform)
            {
                Vector3 basisX = Vector3.FromV3(transform.basis.x);
                Vector3 basisY = Vector3.FromV3(transform.basis.y);
                Vector3 basisZ = Vector3.FromV3(transform.basis.z);

                Matrix4x4 newMatrix = new Matrix4x4();
                newMatrix.Vector4ToRow(0, new Vector4(basisX,transform.origin.x));
                newMatrix.Vector4ToRow(1, new Vector4(basisY,transform.origin.y));
                newMatrix.Vector4ToRow(2, new Vector4(basisZ,transform.origin.z));
                newMatrix.Vector4ToRow(3, new Vector4(0,0,0,1));
                return newMatrix;
            }

            public static Transform Matrix4x4ToTransform(Matrix4x4 matrix)
            {
                Transform transform = new Transform();
                transform.basis.x = new Vector3(matrix.RowToVector4(0).x,matrix.RowToVector4(0).y,matrix.RowToVector4(0).z).toV3;
                transform.basis.y = new Vector3(matrix.RowToVector4(1).x,matrix.RowToVector4(1).y,matrix.RowToVector4(1).z).toV3;
                transform.basis.z = new Vector3(matrix.RowToVector4(2).x,matrix.RowToVector4(2).y,matrix.RowToVector4(2).z).toV3;
                transform.origin = new Vector3(matrix.ColumnToVector4(3).x,matrix.ColumnToVector4(3).y,matrix.ColumnToVector4(3).z).toV3;
                return transform;
            }
            
            public static Matrix4x4 operator *(Matrix4x4 A, Matrix4x4 B)
            {
                Matrix4x4 newMatrix = new Matrix4x4();
                
                for (int i = 0; i < 4; i++) 
                { 
                    for (int j = 0; j < 4; j++)
                    { 
                        for (int k = 0; k < 4; k++) 
                        { 
                            newMatrix.matrix[i][j] += A.matrix[i][k] * B.matrix[k][j];
                        }
                    }
                }
                return newMatrix;
            }
            
            
            public static Matrix4x4 identity
            {
                get
                {
                    Matrix4x4 newMatrix = new Matrix4x4();
                    
                    newMatrix.Vector4ToRow(0, new Vector4(1,0,0,0));
                    newMatrix.Vector4ToRow(1, new Vector4(0,1,0,0));
                    newMatrix.Vector4ToRow(2, new Vector4(0,0,1,0));
                    newMatrix.Vector4ToRow(3, new Vector4(0,0,0,1));
                    
                    return newMatrix;
                }
            }
            
            public static Matrix4x4 translationMatrix(Vector3 value)
            {
                Matrix4x4 newMatrix = new Matrix4x4();
                
                newMatrix.Vector4ToRow(0, new Vector4(1,0,0,value.x));
                newMatrix.Vector4ToRow(1, new Vector4(0,1,0,value.y));
                newMatrix.Vector4ToRow(2, new Vector4(0,0,1,value.z));
                newMatrix.Vector4ToRow(3, new Vector4(0,0,0,1));
                
                return newMatrix;
            }
            
            public static Matrix4x4 scaleMatrix(Vector3 value)
            {
                Matrix4x4 newMatrix = new Matrix4x4();
                
                newMatrix.Vector4ToRow(0, new Vector4(value.x,0,0,0));
                newMatrix.Vector4ToRow(1, new Vector4(0,value.y,0,0));
                newMatrix.Vector4ToRow(2, new Vector4(0,0,value.z,0));
                newMatrix.Vector4ToRow(3, new Vector4(0,0,0,1));
                
                return newMatrix;
            }
            
            public static Matrix4x4 rotationMatrix(Quaternion q)
            {
                return QuaternionMatrix(q);
            }

            public static Matrix4x4 setTRS(Vector3 position, Quaternion rotation, Vector3 scale)
            {
                return translationMatrix(position) * rotationMatrix(rotation) * scaleMatrix(scale);
            }

            public void SetTRS(Vector3 position, Quaternion rotation, Vector3 scale)
            {
                matrix = setTRS(position,rotation,scale).matrix;
            }
            
            public static Matrix4x4 QuaternionMatrix(Quaternion q)
            {
                Matrix4x4 newMatrix = new Matrix4x4();
                Matrix4x4 matrix1 = new Matrix4x4();
                Matrix4x4 matrix2 = new Matrix4x4();
                
                matrix1.Vector4ToRow(0, new Vector4(q.w,q.z,-q.y,q.x));
                matrix1.Vector4ToRow(1, new Vector4(-q.z,q.w,q.x,q.y));
                matrix1.Vector4ToRow(2, new Vector4(q.y,-q.x,q.w,q.z));
                matrix1.Vector4ToRow(3, new Vector4(-q.x,-q.y,-q.z,q.w));
                
                matrix2.Vector4ToRow(0, new Vector4(q.w,q.z,-q.y,-q.x));
                matrix2.Vector4ToRow(1, new Vector4(-q.z,q.w,q.x,-q.y));
                matrix2.Vector4ToRow(2, new Vector4(q.y,-q.x,q.w,-q.z));
                matrix2.Vector4ToRow(3, new Vector4(q.x,q.y,q.z,q.w));
                
                newMatrix = matrix1 * matrix2;
                
                return newMatrix;
            }
            
            public static Matrix4x4 Inverse(Matrix4x4 matrix)
            {
                return deserializeMatrix4x4(invert(serializeMatrix4x4(matrix)));
            }
            
            public void Vector4ToRow(int row, Vector4 vector)
            {
                matrix[row][0] = vector.x;
                matrix[row][1] = vector.y;
                matrix[row][2] = vector.z;
                matrix[row][3] = vector.w;
            }
            
            public void Vector4ToColumn(int column, Vector4 vector)
            {
                matrix[0][column] = vector.x;
                matrix[1][column] = vector.y;
                matrix[2][column] = vector.z;
                matrix[3][column] = vector.w;
            }

            public void Vector3ToRow(int row, Vector3 vector)
            {
                matrix[row][0] = vector.x;
                matrix[row][1] = vector.y;
                matrix[row][2] = vector.z;
            }
            
            public void Vector3ToColumn(int column, Vector3 vector)
            {
                matrix[0][column] = vector.x;
                matrix[1][column] = vector.y;
                matrix[2][column] = vector.z;
            }

            
            public Vector4 RowToVector4(int row)
            {
                return new Vector4(matrix[row][0],matrix[row][1],matrix[row][2],matrix[row][3]);
            }
            
            public Vector4 ColumnToVector4(int column)
            {
                return new Vector4(matrix[0][column],matrix[1][column],matrix[2][column],matrix[3][column]);
            }
            
            public static float[] serializeMatrix4x4(Matrix4x4 matrix)
            {
                float[] serializedMatrix = new float[16];
                
                int counter = 0;
                for(int i = 0; i < 4; i++)
                {
                    for(int j = 0; j < 4; j++)
                    {
                        serializedMatrix[counter] = matrix.matrix[j][i];
                        counter++;
                    }
                }
                return serializedMatrix;
            }
            
            private static Matrix4x4 deserializeMatrix4x4(float[] serialized)
            {
                Matrix4x4 deserializedMatrix = new Matrix4x4();
                
                int counter = 0;
                for(int i = 0; i < 4; i++)
                {
                    for(int j = 0; j < 4; j++)
                    {
                        deserializedMatrix.matrix[j][i] = serialized[counter]; 
                        counter++;
                    }
                }
                return deserializedMatrix;
            }
            
            private static float[] invert(float[] matrix)
            {
                double[] tmp = new double[12];
                double[] src = new double[16];
                double[] dst = new double[16];  

                // Transpose matrix
                for (int i = 0; i < 4; i++) {
                src[i +  0] = matrix[i*4 + 0];
                src[i +  4] = matrix[i*4 + 1];
                src[i +  8] = matrix[i*4 + 2];
                src[i + 12] = matrix[i*4 + 3];
                }

                // Calculate pairs for first 8 elements (cofactors) 
                tmp[0] = src[10] * src[15];
                tmp[1] = src[11] * src[14];
                tmp[2] = src[9]  * src[15];
                tmp[3] = src[11] * src[13];
                tmp[4] = src[9]  * src[14];
                tmp[5] = src[10] * src[13];
                tmp[6] = src[8]  * src[15];
                tmp[7] = src[11] * src[12];
                tmp[8] = src[8]  * src[14];
                tmp[9] = src[10] * src[12];
                tmp[10] = src[8] * src[13];
                tmp[11] = src[9] * src[12];
                
                // Calculate first 8 elements (cofactors)
                dst[0]  = tmp[0]*src[5] + tmp[3]*src[6] + tmp[4]*src[7];
                dst[0] -= tmp[1]*src[5] + tmp[2]*src[6] + tmp[5]*src[7];
                dst[1]  = tmp[1]*src[4] + tmp[6]*src[6] + tmp[9]*src[7];
                dst[1] -= tmp[0]*src[4] + tmp[7]*src[6] + tmp[8]*src[7];
                dst[2]  = tmp[2]*src[4] + tmp[7]*src[5] + tmp[10]*src[7];
                dst[2] -= tmp[3]*src[4] + tmp[6]*src[5] + tmp[11]*src[7];
                dst[3]  = tmp[5]*src[4] + tmp[8]*src[5] + tmp[11]*src[6];
                dst[3] -= tmp[4]*src[4] + tmp[9]*src[5] + tmp[10]*src[6];
                dst[4]  = tmp[1]*src[1] + tmp[2]*src[2] + tmp[5]*src[3];
                dst[4] -= tmp[0]*src[1] + tmp[3]*src[2] + tmp[4]*src[3];
                dst[5]  = tmp[0]*src[0] + tmp[7]*src[2] + tmp[8]*src[3];
                dst[5] -= tmp[1]*src[0] + tmp[6]*src[2] + tmp[9]*src[3];
                dst[6]  = tmp[3]*src[0] + tmp[6]*src[1] + tmp[11]*src[3];
                dst[6] -= tmp[2]*src[0] + tmp[7]*src[1] + tmp[10]*src[3];
                dst[7]  = tmp[4]*src[0] + tmp[9]*src[1] + tmp[10]*src[2];
                dst[7] -= tmp[5]*src[0] + tmp[8]*src[1] + tmp[11]*src[2];
                
                // Calculate pairs for second 8 elements (cofactors)
                tmp[0]  = src[2]*src[7];
                tmp[1]  = src[3]*src[6];
                tmp[2]  = src[1]*src[7];
                tmp[3]  = src[3]*src[5];
                tmp[4]  = src[1]*src[6];
                tmp[5]  = src[2]*src[5];
                tmp[6]  = src[0]*src[7];
                tmp[7]  = src[3]*src[4];
                tmp[8]  = src[0]*src[6];
                tmp[9]  = src[2]*src[4];
                tmp[10] = src[0]*src[5];
                tmp[11] = src[1]*src[4];

                // Calculate second 8 elements (cofactors)
                dst[8]   = tmp[0] * src[13]  + tmp[3] * src[14]  + tmp[4] * src[15];
                dst[8]  -= tmp[1] * src[13]  + tmp[2] * src[14]  + tmp[5] * src[15];
                dst[9]   = tmp[1] * src[12]  + tmp[6] * src[14]  + tmp[9] * src[15];
                dst[9]  -= tmp[0] * src[12]  + tmp[7] * src[14]  + tmp[8] * src[15];
                dst[10]  = tmp[2] * src[12]  + tmp[7] * src[13]  + tmp[10]* src[15];
                dst[10] -= tmp[3] * src[12]  + tmp[6] * src[13]  + tmp[11]* src[15];
                dst[11]  = tmp[5] * src[12]  + tmp[8] * src[13]  + tmp[11]* src[14];
                dst[11] -= tmp[4] * src[12]  + tmp[9] * src[13]  + tmp[10]* src[14];
                dst[12]  = tmp[2] * src[10]  + tmp[5] * src[11]  + tmp[1] * src[9];
                dst[12] -= tmp[4] * src[11]  + tmp[0] * src[9]   + tmp[3] * src[10];
                dst[13]  = tmp[8] * src[11]  + tmp[0] * src[8]   + tmp[7] * src[10];
                dst[13] -= tmp[6] * src[10]  + tmp[9] * src[11]  + tmp[1] * src[8];
                dst[14]  = tmp[6] * src[9]   + tmp[11]* src[11]  + tmp[3] * src[8];
                dst[14] -= tmp[10]* src[11 ] + tmp[2] * src[8]   + tmp[7] * src[9];
                dst[15]  = tmp[10]* src[10]  + tmp[4] * src[8]   + tmp[9] * src[9];
                dst[15] -= tmp[8] * src[9]   + tmp[11]* src[10]  + tmp[5] * src[8];

                // Calculate determinant
                double det = src[0]*dst[0] + src[1]*dst[1] + src[2]*dst[2] + src[3]*dst[3];
                
                // Calculate matrix inverse
                float[] inverse = new float[16];
                det = 1.0 / det;
                for (int i = 0; i < 16; i++)
                inverse[i] = (float) (dst[i] * det);
                
                return inverse;
            }
        }
    }
}