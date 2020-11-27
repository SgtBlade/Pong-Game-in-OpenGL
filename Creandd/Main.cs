using OpenGL;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Media;
using System.Reflection;
using Tao.FreeGlut;
using Microsoft.VisualBasic;
using System.Windows.Forms;

namespace Creand
{
    class Program
    {
        
        private static ShaderProgram ShaderPrgm;
        private static VBO<Vector3> cubeVertices, Sphere;
        private static VBO<Vector2> cubeUV, SphereUV;
        private static VBO<int> cubeQuads, SphereQuads;
        private static Texture TextureBar, TextureSphere, BoxTexture;
        private static Button StartButton, ExitButton, AiButton, HumanButton,EndScreenMessage;
        private static Stopwatch watch;
        private static float angle, sphereX, sphereY, sphereVelX, sphereVelY, sphereTiming, AIVelY;
        private static bool POneDown = false, POneUp = false, fullscreen = false, sphereMoving = false, AIBar = false, PTwoDown = false, PTwoUp = false;
        private static int ScoreCounterLeft=0, ScoreCounterRight=0;
        public static SoundPlayer player = new SoundPlayer();
        private static int MaxScore = 12;

        public static Bar LeftBar;
        public static Bar RightBar;
        public static Pointer MainPoiter;
        public static MainGlutFunctions GltFunct = new MainGlutFunctions();
        public static IscoSphere Spher = new IscoSphere();
        public static Shapes ShapeRenderer = new Shapes();

        public const float TopLimit = 2.3f;
        public const float BottomLimit = -2.3f;
        public const float BaseBallSpeed = 3.0f;
        public const float LeftLimit = -3.8f;
        public const float RightLimit = 3.8f;
        public const float PaddleSize = 1.0f; // Paddle is 2 units high.
        public const float BallSpeedupFactor = 1.05f;
        public const float SphereMoveDelay = 0.5f;


        // This can be controlled to set AI difficulty.
        public float AiReactionTime = 0.1f;

        // This specifies the angle the ball gets rotated when hitting the edge of the
        // paddle in radians.
        public const double MaxControlAngle = 0.1f * (float)Math.PI;

        public enum GameState
        {
            Menu,
            Ingame,
            Endgame
        }

        public enum TypeofEnd
        {
            None,
            Player1Won,
            Player2Won,
            AiWon
        }

        public static GameState state = GameState.Menu;
        public static TypeofEnd EndType = TypeofEnd.None;

        static void Main(string[] args)
        {
            //Liedje ophalen en in soundplayer steken
            Stream str = Res.MainMenu;
            player = new SoundPlayer(str);

            //Standaard Dimensies toewijzen en scherm maken
            GltFunct.Width = 1280;
            GltFunct.Height = 720;
            GltFunct.CreateWindow();

            //Schader programma toewijzen
            ShaderPrgm = GltFunct.AssignShaderProgram();

            //De textures goed toewijzen
            TextureBar = new Texture(Res.bar);
            TextureSphere = new Texture(Res.Sphere);
            BoxTexture = new Texture(Res.Box);


            //Start en exit button maken
            StartButton = new Button(Res.startbutton, 0.0f, 0.1f, 0.35f);
            AiButton = new Button(Res.AiSelected, 0.0f, -0.3f, 0.30f);
            HumanButton = new Button(Res.PvP, 0.025f, -0.58f, 0.325f);
            ExitButton = new Button(Res.exitbutton, 0.0f, -0.9f, 0.30f);
            MainPoiter = new Pointer(Res.Pointer, -0.35f, -0.30f, 0.03f);

            //Linker en rechter speelbar maken
            LeftBar = new Bar(-4f, BottomLimit + 0.75f, TopLimit - 0.75f, 0.0f);
            RightBar = new Bar(3.8f, BottomLimit + 0.75f, TopLimit - 0.75f, 0.0f);

            //Cubus van op start menu maken
            cubeVertices = Shapes.SetCube();
            cubeUV = Shapes.SetCubeTexture();
            cubeQuads = Shapes.SetCubeElements();
            angle = 0.0f;

            //Bal gegevens initialiseren
            sphereX = 0.0f;
            sphereY = 0.0f;
            sphereVelX = 0.0f;
            sphereVelY = 0.0f;
            sphereTiming = 0.0f;

            //Bal gegevens opvullen
            Sphere = Spher.Sphere();
            SphereUV = Spher.SetSphereTexture();
            SphereQuads = Spher.SetSphereElements();

            // Glut functies maken
            Glut.glutIdleFunc(OnIdle);
            Glut.glutDisplayFunc(OnRenderFrame);
            Glut.glutCloseFunc(OnClose);
            Glut.glutReshapeFunc(OnReshape);
            Glut.glutKeyboardFunc(OnKeyboardDown);
            Glut.glutKeyboardUpFunc(OnKeyboardUp);
            Glut.glutMouseFunc(OnMouse);

            //Nieuwe stopwatch starten
            watch = Stopwatch.StartNew();

            //Muziekspeler starten
            player.Play();

            //De hoofdloop starten hierdoor gaat alles dan op het scherm komen
            GltFunct.StartGlutMainLoop();
        }

        private static void OnClose()
        {
            // Alles wegwerken -> wanneer dit niet gedaan wordt krijg je een soort van crash bij het sluiten van de applicatie
            player.Stop();

            LeftBar.Dispose();
            RightBar.Dispose();

            cubeQuads.Dispose();    
            cubeVertices.Dispose();
            cubeUV.Dispose();

            Sphere.Dispose();
            SphereUV.Dispose();
            SphereQuads.Dispose();

            TextureBar.Dispose();
            TextureSphere.Dispose();
            BoxTexture.Dispose();

            StartButton.Dispose();
            AiButton.Dispose();
            HumanButton.Dispose();
            ExitButton.Dispose();
            MainPoiter.Dispose();
            if (EndScreenMessage != null) EndScreenMessage.Dispose();

            ShaderPrgm.DisposeChildren = true;
            ShaderPrgm.Dispose();
        }

        private static void OnReshape(int width, int height)
        {   //Alles rescalen naar de nieuwe ingestelde groote
            GltFunct.Width = width;
            GltFunct.Height = height;
        }

        private static void RotateVec( ref float x, ref float y, double angle )
        {
            float sin = (float)Math.Sin(angle);
            float cos = (float)Math.Cos(angle);
            float newx = cos * x - sin * y;
            float newy = sin * x + cos * y;
            x = newx;
            y = newy;
        }

        private static void ReflectVec( ref float x, ref float y, float reflectx, float reflecty )
        {
            float d = x * reflectx + y * reflecty; //dot(v, n)
            x = x - 2.0f * d * reflectx;
            y = y - 2.0f * d * reflecty;
        }

        private static void ReflectAtAngle( ref float x, ref float y, double angle, float normalx, float normaly)
        {
            RotateVec(ref normalx,ref normaly, angle);
            ReflectVec(ref x, ref y, normalx, normaly);
        }

        private static void OnIdle()
        {
            // Tijd sinds vorige frame berekenen
            watch.Stop();
            float deltaTime = (float)watch.ElapsedTicks / (float)Stopwatch.Frequency;
            watch.Restart();

            angle += deltaTime;
            //Kijken welke state de game in is om te beslissen welke dingen er moeten op het scherm komen
            if (state == GameState.Ingame)
            {
                sphereTiming -= deltaTime;
                //Kijken of sphere in beweging is
                if (sphereTiming < 0.0f && !sphereMoving)
                {
                    sphereMoving = true;
                    Random random = new Random();
                    sphereVelX = 2.0f * ((float)random.NextDouble() - 0.5f);
                    if (Math.Abs(sphereVelX) < 0.5f)//-> beveiliging zodat de bal niet vast komt te zitten met loodrecht te botsen
                    {
                        if (sphereVelX < 0.0f) sphereVelX -= 1.0f;
                        else sphereVelX += 1.0f;
                    }
                    sphereVelY = (float)random.NextDouble() - 0.5f;

                    // Normalize
                    float length = (float)Math.Sqrt((double)(sphereVelX * sphereVelX + sphereVelY * sphereVelY));
                    sphereVelX /= length;
                    sphereVelY /= length;
                    sphereVelX *= BaseBallSpeed;
                    sphereVelY *= BaseBallSpeed;

                    //ZOrgen dat de bal niet te ver gaat
                    if (sphereVelY < 0.17f)
                    {
                        if(sphereVelY < 0 && sphereVelY > -0.17) sphereVelY -= 0.2f;
                        else sphereVelY += 0.2f;
                    }
                }
                //Snelheid van de dingetjes die de bal opvangen regelen (ik heet ze bar of paddle)
                const float PaddleSpeed = 2.0f;

                // De hoek van het object aanpassen
                if (POneDown || POneUp)
                {
                    float amount = PaddleSpeed * deltaTime;
                    if (POneDown) amount = -amount;
                    LeftBar.Move(amount);
                }

                if (AIBar)
                {
                    float dY = (sphereY - RightBar.mYpos);
                    if (Math.Abs(dY) > PaddleSize / 2.0f)
                    {
                        AIVelY += dY * deltaTime;
                        if (AIVelY > AiReactionTime)
                        {
                            RightBar.Move(PaddleSpeed * deltaTime);
                        }
                        else if( AIVelY < AiReactionTime  )
                        {
                            RightBar.Move(-PaddleSpeed * deltaTime);
                        }
                    }
                    else
                    {
                        AIVelY = 0.0f;
                    }
                }
                else if (PTwoDown || PTwoUp)
                {
                    float amount = PaddleSpeed * deltaTime;
                    if (PTwoDown) amount = -amount;
                    RightBar.Move(amount);

                }


                if (sphereMoving)
                {
                    sphereX += sphereVelX * deltaTime;
                    sphereY += sphereVelY * deltaTime;
                    
                    
                    if (sphereY > TopLimit-0.2f)
                    {
                        sphereY -= sphereVelY * deltaTime;
                        sphereVelY = -sphereVelY;
                    }
                    if (sphereY < BottomLimit + 0.2f)
                    {
                        sphereY -= sphereVelY * deltaTime;
                        sphereVelY = -sphereVelY;
                    }

                    if (sphereX < LeftLimit)
                    {
                        float hitdelta = sphereY - LeftBar.mYpos;
                        if (Math.Abs(hitdelta) < PaddleSize)
                        {
                            sphereX -= sphereVelX * deltaTime;
                            sphereVelX *= BallSpeedupFactor;
                            sphereVelY *= BallSpeedupFactor;

                            float hitpointfactor = hitdelta / PaddleSize;
                            // The new direction of the ball is influenced by hitpoint.
                            double rotateangle = (double)hitpointfactor * MaxControlAngle;
                            ReflectAtAngle(ref sphereVelX, ref sphereVelY, rotateangle, -1.0f, 0.0f );
                        }
                        else
                        {
                            //Player 1 lost
                            ScoreCounterRight++;
                            Console.WriteLine(ScoreCounterRight);
                            if (ScoreCounterRight == MaxScore)
                            {
                                if (AIBar) { EndType = TypeofEnd.AiWon;  EndScreenMessage = new Button(Res.TAW, 0.0f, 0.1f, 0.80f);}
                                else { EndType = TypeofEnd.Player1Won; EndScreenMessage = new Button(Res.PTW, 0.0f, 0.1f, 0.80f); }
                                state = GameState.Endgame;

                            }else ResetGameState();

                        }
                    }

                    if (sphereX > RightLimit)
                    {
                        float hitdelta = sphereY - RightBar.mYpos;
                        if (Math.Abs(hitdelta) < PaddleSize)
                        {
                            sphereX -= sphereVelX * deltaTime;
                            sphereVelX *= BallSpeedupFactor;
                            sphereVelY *= BallSpeedupFactor;

                            float hitpointfactor = hitdelta / PaddleSize;
                            // The new direction of the ball is influenced by hitpoint.
                            double rotateangle = (double)hitpointfactor * MaxControlAngle;
                            ReflectAtAngle(ref sphereVelX, ref sphereVelY, -rotateangle, 1.0f, 0.0f );
                        }
                        else
                        {
                            //Player 2 lost
                            ScoreCounterLeft++;
                            if (ScoreCounterLeft == MaxScore)
                            {
                                EndScreenMessage = new Button(Res.POW, 0.0f, 0.1f, 0.80f);
                                EndType = TypeofEnd.Player1Won;
                                state = GameState.Endgame;
                            }else ResetGameState();
                        }
                    }

                }




            }
            Glut.glutPostRedisplay();
        }

        private static void OnRenderFrame()
        {
            WindowAndObjectProperties.ClearAndAssignShaderprogram(GltFunct.Width, GltFunct.Height, TextureBar, ShaderPrgm);

            if (state == GameState.Ingame)
            {
                Gl.UseProgram(ShaderPrgm.ProgramID);
                ShaderPrgm["projection_matrix"].SetValue(Matrix4.CreatePerspectiveFieldOfView(
                    0.45f, (float)GltFunct.Width / GltFunct.Height, 0.1f, 1000f));



                LeftBar.RenderBar(ShaderPrgm);
                RightBar.RenderBar(ShaderPrgm);

                Gl.BindTexture(TextureSphere);
                Shapes.DrawTexture(sphereX, sphereY, 0.10f, 2, 2, ShaderPrgm, TextureSphere);
            }
            else if(state == GameState.Menu)// Menu
            {
                // Render 3D cube.
                ShaderPrgm["projection_matrix"].SetValue(Matrix4.CreatePerspectiveFieldOfView(
                    0.45f, (float)GltFunct.Width / GltFunct.Height, 0.1f, 1000f));
                Shapes.DrawCube(0, 0, ShaderPrgm, BoxTexture, cubeVertices, cubeUV, cubeQuads, angle);

                // Render Menu.
                ShaderPrgm["projection_matrix"].SetValue(Matrix4.CreateOrthographic(GltFunct.Width, GltFunct.Height, 0.0f, 100.0f));

                Gl.Enable(EnableCap.Blend);
                Gl.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
                MainPoiter.Render(GltFunct.Width, GltFunct.Height, ShaderPrgm);
                StartButton.Render(GltFunct.Width, GltFunct.Height, ShaderPrgm);
                AiButton.Render(GltFunct.Width, GltFunct.Height, ShaderPrgm);
                HumanButton.Render(GltFunct.Width, GltFunct.Height, ShaderPrgm);
                ExitButton.Render(GltFunct.Width, GltFunct.Height, ShaderPrgm);
                Gl.Disable(EnableCap.Blend);
            }
            else if(state == GameState.Endgame)
            {
                // Render 3D cube.
                ShaderPrgm["projection_matrix"].SetValue(Matrix4.CreatePerspectiveFieldOfView(
                    0.45f, (float)GltFunct.Width / GltFunct.Height, 0.1f, 1000f));
                Shapes.DrawCube(0, 0, ShaderPrgm, BoxTexture, cubeVertices, cubeUV, cubeQuads, angle);

                // Render Menu.
                ShaderPrgm["projection_matrix"].SetValue(Matrix4.CreateOrthographic(GltFunct.Width, GltFunct.Height, 0.0f, 100.0f));

                Gl.Enable(EnableCap.Blend);
                Gl.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
                EndScreenMessage.Render(GltFunct.Width, GltFunct.Height, ShaderPrgm);
                Gl.Disable(EnableCap.Blend);
            }
            Glut.glutSwapBuffers();
        }

        private static void OnKeyboardDown(byte key, int x, int y)
        {
            if (state == GameState.Menu)
            {
                if (key == 'w' || key == 'a') MainPoiter.y = -0.30f;
                else if (key == 's') MainPoiter.y = -0.58f;
                else if (key == 32 || key == 13)
                { state = GameState.Ingame; ResetGameState(); }
                if(key == 'm') {
                    string score = Interaction.InputBox("Gelieve de maximum score in te geven", "MaxScore", MaxScore.ToString(), -1, -1);
                    try { if (int.Parse(score) == 0)MessageBox.Show("Geen geldig nummer ingevoerd"); else MaxScore = int.Parse(score); } catch (Exception) { MessageBox.Show("Geen geldig nummer ingevoerd"); }
                }
            }

            if (state == GameState.Endgame)
            {
                EndScreenMessage.Dispose();
                state = GameState.Menu;
                ScoreCounterRight = 0;
                ScoreCounterLeft = 0;
            }
            
            if (key == 27) { if (state == GameState.Ingame) state = GameState.Menu; else if (state == GameState.Menu) Glut.glutLeaveMainLoop(); }

            if (key == 'w' || key == 'a') POneUp = true;
            else if (key == 's') POneDown = true;
            
            if (key == 'i') PTwoUp = true;
            else if (key == 'k') PTwoDown = true;
        }

        private static void OnKeyboardUp(byte key, int x, int y)
        {
            if (key == 'w' || key == 'a') POneUp = false;
            else if (key == 's') POneDown = false;
            else if (key == 'i') PTwoUp = false;
            else if (key == 'k') PTwoDown = false;
            else if (key == 'f')
            {
                fullscreen = !fullscreen;
                if (fullscreen) Glut.glutFullScreen();
                else
                {
                    Glut.glutPositionWindow(0, 0);
                    Glut.glutReshapeWindow(1280, 720);
                }
            }
        }

        private static void ResetGameState()
        {
            sphereTiming = SphereMoveDelay;
            sphereMoving = false;
            sphereX = 0.0f;
            sphereY = 0.0f;

            LeftBar.mYpos = 0.0f;
            RightBar.mYpos = 0.0f;

            if (MainPoiter.y == -0.3f) AIBar = true;
            else AIBar = false;
            AIVelY = 0.0f;

        }

        private static void OnMouse(int button, int buttonstate, int x, int y)
        {
            if (state == GameState.Menu)
            {
                if (button != 0 || buttonstate != 1)
                    return;
                float mousex = (float)x / (float)GltFunct.Width;
                float mousey = (float)y / (float)GltFunct.Height;
                mousex -= 0.5f;
                mousex *= 2;
                mousey -= 0.5f;
                mousey *= 2;
                mousey = -mousey; // Y is omgekeerd.
                if (StartButton.isClicked(mousex, mousey))
                {
                    state = GameState.Ingame;
                    ScoreCounterLeft = 0;
                    ScoreCounterRight = 0;
                    ResetGameState();
                }
                if (ExitButton.isClicked(mousex, mousey))
                {
                    Glut.glutLeaveMainLoop();
                }
                if (AiButton.isClicked(mousex, mousey))
                {
                    MainPoiter.y = -0.30f;
                }
                if (HumanButton.isClicked(mousex, mousey))
                {
                    MainPoiter.y = -0.58f;
                }
            }
        }

        //LEGACY CODE
        /*Controlleren van dll's en ze plaatsen indien nodig
        private static void PlaceDlls()
        {
            try
            {
                if (!File.Exists(Directory.GetCurrentDirectory() + "\\freeglut.dll")) File.WriteAllBytes(Directory.GetCurrentDirectory() + "\\freeglut.dll", Res.freeglut);
                if (!File.Exists(Directory.GetCurrentDirectory() + "\\OpenGL.dll")) File.WriteAllBytes(Directory.GetCurrentDirectory() + "\\OpenGL.dll", Res.OpenGL);
                if (!File.Exists(Directory.GetCurrentDirectory() + "\\Tao.FreeGlut.dll")) File.WriteAllBytes(Directory.GetCurrentDirectory() + "\\Tao.FreeGlut.dll", Res.Tao_FreeGlut);
            }
            catch (Exception)
            {
                MessageBox.Show("Er is een fout opgetreden bij het schrijven van de DLL's. Gelieve ze handmatig bij de exe te zetten of de applicatie schrijfrechten te geven");
                throw;
            }
        }
        */
    }
}
