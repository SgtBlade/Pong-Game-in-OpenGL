using OpenGL;
using System;
using System.Reflection;

namespace Creand
{
    class IscoSphere
    {
        public VBO<Vector3> Sphere()
        {
            VBO<Vector3> cube = new VBO<Vector3>(new Vector3[] {
                new Vector3(0.125f, 0.125f, -0.125f), new Vector3(-0.125f, 0.125f, -0.125f), new Vector3(-0.125f, 0.125f, 0.125f), new Vector3(0.125f, 0.125f, 0.125f),
                new Vector3(0.125f, -0.125f, 0.125f), new Vector3(-0.125f, -0.125f, 0.125f), new Vector3(-0.125f, -0.125f, -0.125f), new Vector3(0.125f, -0.125f, -0.125f),
                new Vector3(0.125f, 0.125f, 0.125f), new Vector3(-0.125f, 0.125f, 0.125f), new Vector3(-0.125f, -0.125f, 0.125f), new Vector3(0.125f, -0.125f, 0.125f),
                new Vector3(0.125f, -0.125f, -0.125f), new Vector3(-0.125f, -0.125f, -0.125f), new Vector3(-0.125f, 0.125f, -0.125f), new Vector3(0.125f, 0.125f, -0.125f),
                new Vector3(-0.125f, 0.125f, 0.125f), new Vector3(-0.125f, 0.125f, -0.125f), new Vector3(-0.125f, -0.125f, -0.125f), new Vector3(-0.125f, -0.125f, 0.125f),
                new Vector3(0.125f, 0.125f, -0.125f), new Vector3(0.125f, 0.125f, 0.125f), new Vector3(0.125f, -0.125f, 0.125f), new Vector3(0.125f, -0.125f, -0.125f) });

            return cube;
        }

        public VBO<Vector2> SetSphereTexture()
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

        public VBO<int> SetSphereElements()
        {
            VBO<int> Ele = new VBO<int>(new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23 }, BufferTarget.ElementArrayBuffer);
            return Ele;
        }




    }
}
