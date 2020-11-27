using OpenGL;
using System;
using System.Drawing;
using System.Reflection;

namespace Creand
{
    class Pointer
    {

        private Texture ButtonTexture;
        public float x, y, scale;

        public Pointer(Bitmap texturepath, float xpos, float ypos, float size)
        {
            ButtonTexture = new Texture(texturepath);
            x = xpos;
            y = ypos;
            scale = size;
        }

        public void Dispose()
        {
            ButtonTexture.Dispose();
        }

        public bool isClicked(float mousex, float mousey)
        {
            float texwidth = 1.0f;
            float texheight = (float)ButtonTexture.Size.Height / (float)ButtonTexture.Size.Width;
            texwidth *= scale;
            texheight *= scale;

            float xmin = x - texwidth / 2;
            float xmax = x + texwidth / 2;
            float ymin = y - texheight / 2;
            float ymax = y + texheight / 2;

            return mousex > xmin && mousex < xmax && mousey > ymin && mousey < ymax;
        }

        public void Render(int screenwidth, int screenheight, ShaderProgram program)
        {
            Shapes.DrawTexture(x, y, scale, screenwidth, screenheight, program, ButtonTexture);
        }

        public VBO<Vector3> PointerObject()
        {
            VBO<Vector3> triangle = new VBO<Vector3>(new Vector3[] {
                //x,y,z    top sideways             right leg               left leg
                new Vector3(2f, 0.5f, 0), new Vector3(3.5f, -0.5f, 0.5f), new Vector3(4.5f, -0.5f, 0.5f),
                new Vector3(2f, 0.5f, 0), new Vector3(2.5f, -0.5f, 0.5f), new Vector3(2.5f, -0.5f, -0.5f),
                new Vector3(2f, 0.5f, 0), new Vector3(2.5f, -0.5f, -0.5f), new Vector3(1.5f, -0.5f, -0.5f),
                new Vector3(2f, 0.5f, 0), new Vector3(1.5f, -0.5f, -0.5f), new Vector3(1.5f, -0.5f, 0.5f),

                new Vector3(2, -0.5f, 0), new Vector3(1.5f, -0.5f, 0.5f), new Vector3(2.5f, -0.5f, 0.5f),
                new Vector3(2, -0.5f, 0), new Vector3(2.5f, -0.5f, 0.5f), new Vector3(2.5f, -0.5f, -0.5f),
                new Vector3(2, -0.5f, 0), new Vector3(2.5f, -0.5f, -0.5f), new Vector3(1.5f, -0.5f, -0.5f),
                new Vector3(2, -0.5f, 0), new Vector3(1.5f, -0.5f, -0.5f), new Vector3(1.5f, -0.5f, 0.5f)});

            return triangle;
        }

        public VBO<Vector2> PointerTexture()
        {
            VBO<Vector2> triangleCol = new VBO<Vector2>(new Vector2[] {
               new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0),
              new Vector2(0, 0),  new Vector2(0, 0), new Vector2(0, 0),
              new Vector2(0, 0), new Vector2(0, 0),  new Vector2(0, 0),
              new Vector2(0, 0),  new Vector2(0, 0), new Vector2(0, 0),

              new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0),
              new Vector2(0, 0),  new Vector2(0, 0), new Vector2(0, 0),
              new Vector2(0, 0), new Vector2(0, 0),  new Vector2(0, 0),
              new Vector2(0, 0),  new Vector2(0, 0), new Vector2(0, 0)});
            return triangleCol;
        }

        public VBO<int> PointerElements()
        {
            VBO<int> TriaEle = new VBO<int>(new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24 }, BufferTarget.ElementArrayBuffer);
            return TriaEle;
        }
    }
}
