using OpenGL;
using System;
using System.Reflection;

namespace Creand
{
    class Bar
    {
        private VBO<Vector3> mVertices;
        private VBO<Vector2> mUVs;
        private VBO<int> mElements;

        public float mXpos;
        public float mYpos;
        private float mYmin;
        private float mYmax;

        public Bar( float xpos, float ymin, float ymax, float ypos )
        {
            mVertices = BarObject();
            mUVs = SetBarTexture();
            mElements = SetBarElements();

            mXpos = xpos;
            mYpos = ypos;
            mYmin = ymin;
            mYmax = ymax;
        }


        public void RenderBar( ShaderProgram shader )
        {
            Vector3 position = new Vector3(mXpos, mYpos, 0.0f);
            Matrix4 matrix = Matrix4.CreateRotationZ( (float)Math.PI / 2 ) * Matrix4.CreateTranslation(position);
            shader["model_matrix"].SetValue(matrix);
            WindowAndObjectProperties.BindBufferDrawCube( mVertices, mUVs, mElements, shader);

        }

        public void Dispose()
        {
            mVertices.Dispose();
            mUVs.Dispose();
            mElements.Dispose();
        }

        public void Move( float amount )
        {
            mYpos += amount;
            if (mYpos < mYmin) mYpos = mYmin;
            if (mYpos > mYmax) mYpos = mYmax;
        }

        private VBO<Vector3> BarObject()
        {
            VBO<Vector3> Object = new VBO<Vector3>(new Vector3[] {
                new Vector3(0.75f, 0.0f, 0.0f), new Vector3(-0.75f, 0.0f, 0.0f), new Vector3(-0.75f, 0.0f,0.25f), new Vector3(0.75f, 0.0f,0.25f),//top
                new Vector3(0.75f, -0.15f,0.25f), new Vector3(-0.75f, -0.15f,0.25f), new Vector3(-0.75f, -0.15f, 0.0f), new Vector3(0.75f, -0.15f, 0.0f),//bottom
                new Vector3(0.75f, 0.0f,0.25f), new Vector3(-0.75f, 0.0f,0.25f), new Vector3(-0.75f, -0.15f,0.25f), new Vector3(0.75f, -0.15f,0.25f),//front
                new Vector3(0.75f, -0.15f, 0.0f), new Vector3(-0.75f, -0.15f, 0.0f), new Vector3(-0.75f, 0.0f, 0.0f), new Vector3(0.75f, 0.0f, 0.0f),//back
                new Vector3(-0.75f, 0.0f,0.25f), new Vector3(-0.75f, 0.0f, 0.0f), new Vector3(-0.75f, -0.15f, 0.0f), new Vector3(-0.75f, -0.15f,0.25f),//left
                new Vector3(0.75f, 0.0f, 0.0f), new Vector3(0.75f, 0.0f,0.25f), new Vector3(0.75f, -0.15f,0.25f), new Vector3(0.75f, -0.15f, 0.0f) });//right


            return Object;
        }

        private VBO<Vector2> SetBarTexture()
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

        private VBO<int> SetBarElements()
        {
            VBO<int> Ele = new VBO<int>(new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23 }, BufferTarget.ElementArrayBuffer);
            return Ele;
        }
    }
}
