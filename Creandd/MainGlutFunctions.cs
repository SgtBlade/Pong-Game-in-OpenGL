using OpenGL;
using Tao.FreeGlut;

namespace Creand
{
    class MainGlutFunctions
    {
        private int _Width;
        private int _Height;

        public void CreateWindow()
        {
            Glut.glutInit();
            Glut.glutInitDisplayMode(Glut.GLUT_DOUBLE | Glut.GLUT_DEPTH);
            Glut.glutInitWindowSize(Width, Height);
            Glut.glutCreateWindow("Ping dat pong");
            Gl.Enable(EnableCap.DepthTest);
            //Gl.ClearColor(0.498f, 0.133f, 0.529f,1f);
            Gl.ClearColor(0, 0, 0, 1f);
        }

        public ShaderProgram AssignShaderProgram()
        {
            ShaderProgram prgm = new ShaderProgram(VertexShader, FragmentShader);
            prgm.Use();
            prgm["projection_matrix"].SetValue(Matrix4.CreatePerspectiveFieldOfView(0.45f, (float)Width / Height, 0.1f, 1000f));
            prgm["view_matrix"].SetValue(Matrix4.LookAt(new Vector3(0, 0, 10), Vector3.Zero, new Vector3(0, 1, 0)));
            return prgm;
        }

        public void StartGlutMainLoop()
        {
            Glut.glutMainLoop();
        }

        public int Width
        {
            get { return _Width; }
            set { _Width = value; }
        }

        public int Height
        {
            get { return _Height; }
            set { _Height = value; }
        }

        public static string VertexShader = @"
#version 130

in vec3 vertexPosition;
in vec2 vertexUV;

out vec2 uv;

uniform mat4 projection_matrix;
uniform mat4 view_matrix;
uniform mat4 model_matrix;

void main(void)
{
    uv = vertexUV;
    gl_Position = projection_matrix * view_matrix * model_matrix * vec4(vertexPosition, 1);
}
";

        public static string FragmentShader = @"
#version 130

uniform sampler2D texture;

in vec2 uv;

out vec4 fragment;

void main(void)
{
    vec4 color = texture2D(texture, uv);
    fragment = color;
}
";
    }
}
