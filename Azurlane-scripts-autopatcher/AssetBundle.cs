﻿using System;
using System.IO;
using System.Reflection;

namespace Azurlane
{
    internal static class AssetBundle
    {
        private static readonly object Instance;

        static AssetBundle()
        {
            if (Instance != null)
                return;

            Instance = Activator.CreateInstance(Assembly.Load(Properties.Resources.Salt).GetType("LL.Salt"));
        }

        internal static void Initialize(string assetbundle, Tasks tasks)
        {
            try
            {
                if (tasks == Tasks.Decrypt || tasks == Tasks.Encrypt)
                {
                    File.WriteAllBytes(assetbundle, (byte[])Instance.GetType().GetMethod("Make", BindingFlags.Static | BindingFlags.Public).Invoke(Instance, new object[]
                    {
                        File.ReadAllBytes(assetbundle), tasks == Tasks.Encrypt
                    }));
                }
                else if (tasks == Tasks.Unpack || tasks == Tasks.Repack)
                    Utils.Command($"UnityEX.exe {(tasks == Tasks.Unpack ? "export" : "import")} \"{assetbundle}\"");
            }
            catch (Exception e)
            {
                Console.Write("<exception-detected>");
            }
        }
    }
}