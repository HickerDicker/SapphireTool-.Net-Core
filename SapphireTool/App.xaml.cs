using SapphireTool.Classes;
using OSVersionExtension;
using System.Reflection;
using System.Windows;

namespace SapphireTool
{
    public partial class App : Application
    {
        // this is a rip-off of TheWorldOfPC's configurator.NET Core btw jsyk
        public static string Winver;

        public App() 
        {
            switch (OSVersion.GetOSVersion().Version.Build)
            {
                case 19045:
                    Winver = "22H2";
                    break;
                case 22631:
                    Winver = "23H2";
                    break;
                default:
                    Winver = "24H2";
                    break;
            }
        }
    }
}