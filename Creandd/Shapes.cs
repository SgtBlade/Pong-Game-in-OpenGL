using OpenGL;
using System;
using System.Reflection;

namespace Creand
{
    class Shapes
    {
        

        public static VBO<Vector3> SetCube()
        {
            VBO<Vector3> cube = new VBO<Vector3>(new Vector3[] {
                new Vector3(0.75f, 0.75f, -0.75f), new Vector3(-0.75f, 0.75f, -0.75f), new Vector3(-0.75f, 0.75f, 0.75f), new Vector3(0.75f, 0.75f, 0.75f),
                new Vector3(0.75f, -0.75f, 0.75f), new Vector3(-0.75f, -0.75f, 0.75f), new Vector3(-0.75f, -0.75f, -0.75f), new Vector3(0.75f, -0.75f, -0.75f),
                new Vector3(0.75f, 0.75f, 0.75f), new Vector3(-0.75f, 0.75f, 0.75f), new Vector3(-0.75f, -0.75f, 0.75f), new Vector3(0.75f, -0.75f, 0.75f),
                new Vector3(0.75f, -0.75f, -0.75f), new Vector3(-0.75f, -0.75f, -0.75f), new Vector3(-0.75f, 0.75f, -0.75f), new Vector3(0.75f, 0.75f, -0.75f),
                new Vector3(-0.75f, 0.75f, 0.75f), new Vector3(-0.75f, 0.75f, -0.75f), new Vector3(-0.75f, -0.75f, -0.75f), new Vector3(-0.75f, -0.75f, 0.75f),
                new Vector3(0.75f, 0.75f, -0.75f), new Vector3(0.75f, 0.75f, 0.75f), new Vector3(0.75f, -0.75f, 0.75f), new Vector3(0.75f, -0.75f, -0.75f) });


            return cube;
        }

        public static VBO<Vector2> SetCubeTexture()
        {
            VBO<Vector2> Texture = new VBO<Vector2>(new Vector2[] {
                new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, 1), new Vector2(0, 1),
                new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, 1), new Vector2(0, 1),
                new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, 1), new Vector2(0, 1),
                new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, 1), new Vector2(0, 1),
                new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, 1), new Vector2(0, 1),
                new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, 1), new Vector2(0, 1) });
            return Texture;
        }

        public static VBO<int> SetCubeElements()
        {
            VBO<int> Ele = new VBO<int>(new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23 }, BufferTarget.ElementArrayBuffer);
            return Ele;
        }

        public static void DrawCube(int width,int height, ShaderProgram ShdrProgram, Texture Txture, VBO<Vector3> form, VBO<Vector2> formColors,VBO<int> formElement, float RotationAngle)
        {
            // use our shader program and bind the crate texture
            Gl.UseProgram(ShdrProgram);
            Gl.BindTexture(Txture);

            // set the transformation of the cube

            Vector3 testangle = new Vector3(1f, 0, 0.0f);
            ShdrProgram["model_matrix"].SetValue(Matrix4.CreateRotationY(RotationAngle) * Matrix4.CreateRotationX(RotationAngle));
            // bind the vertex positions, UV coordinates and element array
            Gl.BindBufferToShaderAttribute(form, ShdrProgram, "vertexPosition");
            Gl.BindBufferToShaderAttribute(formColors, ShdrProgram, "vertexUV");
            Gl.BindBuffer(formElement);

            // draw the textured cube
            Gl.DrawElements(BeginMode.Quads, formElement.Count, DrawElementsType.UnsignedInt, IntPtr.Zero);
        }

        public static void DrawTexture( float x, float y, float scale, int screenwidth, int screenheight,
            ShaderProgram program, Texture texture )
        {
            Gl.UseProgram(program);
            Gl.BindTexture(texture);

            x *= (float)screenwidth / 2.0f;
            y *= (float)screenheight / 2.0f;

            // Scale texture based proportionally, and based on screen resolution.
            float texwidth = 1.0f;
            float texheight = (float)texture.Size.Height / (float)texture.Size.Width;
            texwidth *= screenwidth;
            texheight *= screenheight;
            texwidth *= scale;
            texheight *= scale;

            Vector3 pos = new Vector3(x - texwidth/2, y - texheight/2, 0);
            program["model_matrix"].SetValue(Matrix4.CreateTranslation(pos));
         

            VBO<Vector3> vertices = new VBO<Vector3>(new Vector3[] {
                new Vector3(texwidth, 0, 0), new Vector3(0, 0, 0), new Vector3(0, texheight, 0), new Vector3(texwidth, texheight, 0) });
            VBO<Vector3> UVs = new VBO<Vector3>(new Vector3[] {
                new Vector3(1, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 1, 0), new Vector3(1, 1, 0) });
            VBO<int> elements = new VBO<int>(new int[] { 0, 1, 2, 3 }, BufferTarget.ElementArrayBuffer );

            Gl.BindBufferToShaderAttribute(vertices, program, "vertexPosition");
            Gl.BindBufferToShaderAttribute(UVs, program, "vertexUV");
            Gl.BindBuffer(elements);

            Gl.DrawElements(BeginMode.Quads, 4, DrawElementsType.UnsignedInt, IntPtr.Zero);

            vertices.Dispose();
            UVs.Dispose();
            elements.Dispose();
        }






    }
}
