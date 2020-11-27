using System;
using System.Reflection;

namespace Creand
{
    class DllLoader
    {
        public void LoadResources()
        {
            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(
           (s, a) =>
           {
                   if (a.Name.Substring(0, a.Name.IndexOf(",")) == "OpenGL.dll")
                   {
                       return Assembly.Load(Res.OpenGL);
                   }

                   if (a.Name.Substring(0, a.Name.IndexOf(",")) == "freeglut.dll")
                   {
                       return Assembly.Load(Res.freeglut);
                   }
                   if (a.Name.Substring(0, a.Name.IndexOf(",")) == "Tao.FreeGlut.dll")
                   {
                       return Assembly.Load(Res.Tao_FreeGlut);
                   }
                   return null;
           }
);
        }
    }
}
