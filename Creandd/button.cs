using OpenGL;
using System;
using System.Drawing;
using System.Reflection;

namespace Creand
{
    class Button
    {
        private Texture ButtonTexture;
        public float x, y, scale;

        public Button( Bitmap texturepath, float xpos, float ypos, float size )
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

        public void Render( int screenwidth, int screenheight, ShaderProgram program )
        {
            Shapes.DrawTexture(x, y, scale, screenwidth, screenheight, program, ButtonTexture);
        }
    }
}
