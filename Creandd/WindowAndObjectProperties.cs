using OpenGL;
using System;
using System.Reflection;

namespace Creand
{
    class WindowAndObjectProperties
    {

        public static void ClearAndAssignShaderprogram(int width,int height,Texture txtre, ShaderProgram shdr)
        {
            // set up the OpenGL viewport and clear both the color and depth bits
            Gl.Viewport(0, 0, width, height);
            Gl.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            Gl.UseProgram(shdr);
            Gl.BindTexture(txtre);
        }

        public static void BindBufferDrawCube(VBO<Vector3> Object, VBO<Vector2> ObjectUV, VBO<int> ObjectQuads, ShaderProgram Shdrprgm)
        {
            //Vector3 testangle = new Vector3(1f, 0, 0.0f);
            //ShdrProgram["model_matrix"].SetValue(Matrix4.CreateRotationY(0) * Matrix4.CreateRotationX(RotationAngle));
            // bind the vertex positions, UV coordinates and element array
            Gl.BindBufferToShaderAttribute(Object, Shdrprgm, "vertexPosition");
            Gl.BindBufferToShaderAttribute(ObjectUV, Shdrprgm, "vertexUV");
            Gl.BindBuffer(ObjectQuads);

            // draw the textured cube
            Gl.DrawElements(BeginMode.Quads, ObjectQuads.Count, DrawElementsType.UnsignedInt, IntPtr.Zero);
        }
        
    }
}
