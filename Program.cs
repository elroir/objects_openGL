using System;
using System.IO;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace tarea_3
{
    class Program
    {
        public static void Main(string[] args)
        {           

            GameWindowSettings gws = GameWindowSettings.Default;
            NativeWindowSettings nws = NativeWindowSettings.Default;
            gws.IsMultiThreaded = false;
            gws.RenderFrequency = 60;
            gws.UpdateFrequency = 60;

            nws.APIVersion = Version.Parse("4.1.0");
            nws.Size = new Vector2i(1280,720);
            nws.Title = "Juego";

            GameWindow window = new GameWindow(gws,nws);

            int i = 0;
            window.UpdateFrame += (FrameEventArgs args) => {
                Console.WriteLine($"{i++}");
            };

            window.Run();
        }

        private static Shader LoadShader( string shaderLocation, ShaderType type)
        {
            int shaderId = GL.CreateShader(type);
            GL.ShaderSource(shaderId, File.ReadAllText(shaderLocation));
            GL.CompileShader(shaderId);
            string infoLog = GL.GetShaderInfoLog(shaderId);
            if (!string.IsNullOrEmpty(infoLog))
            {
                throw new Exception(infoLog);
            }

            return new Shader() { id = shaderId };

        }


        private static ShaderProgram LoadShaderProgram(string vertexShaderLocation,string fragmentShaderLocation)
        {
            int shaderProgramId = GL.CreateProgram();
            Shader vertexShader = LoadShader(vertexShaderLocation, ShaderType.VertexShader);
            Shader fragmentShader = LoadShader(fragmentShaderLocation, ShaderType.FragmentShader);

            GL.AttachShader(shaderProgramId, vertexShader.id);
            GL.AttachShader(shaderProgramId, fragmentShader.id);

            GL.LinkProgram(shaderProgramId);

            GL.DetachShader(shaderProgramId, vertexShader.id);
            GL.DetachShader(shaderProgramId, fragmentShader.id);

            GL.DeleteShader(vertexShader.id);
            GL.DeleteShader(fragmentShader.id);

            string infoLog = GL.GetProgramInfoLog(shaderProgramId);
            if (!string.IsNullOrEmpty(infoLog))
            {
                throw new Exception(infoLog);
            }

            return new ShaderProgram() { id = shaderProgramId };

        }

        public struct Shader
        {
            public int id;
        }

        public struct ShaderProgram
        {
            public int id;
        }

    }
}
