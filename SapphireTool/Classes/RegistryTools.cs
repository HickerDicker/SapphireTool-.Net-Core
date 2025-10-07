using Microsoft.Win32;

namespace SapphireTool.Classes
{
    public class RegistryTools
    {
        public static bool CheckTweakState(RegistryKey key, string dwordName, int value)
        {
            try
            {
                object dwordValue = key.GetValue(dwordName);

                if (dwordValue != null && dwordValue is int && (int)dwordValue == value)
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }

            return false;
        }
    }
}