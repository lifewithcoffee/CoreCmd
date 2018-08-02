﻿using CoreCmd.Config;
using NetCoreUtils.Xml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CoreCmd.BuildinCommands
{
    class CmdCommand
    {
        public void Default()
        {
            this.List();
        }

        public void Add(string filename, bool local=false)
        {
            var userDir = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            var configFileFullPath = Path.Combine(userDir, GlobalConsts.ConfigFileName);

            var xmlUtil = new XmlUtil<CoreCmdConfig>();
            CoreCmdConfig config = null;

            if (File.Exists(configFileFullPath))
                config = xmlUtil.ReadFromFile(configFileFullPath);
            else
                config = new CoreCmdConfig();

            string targetFilePath = Path.Combine(Directory.GetCurrentDirectory(), filename);
            if (File.Exists(targetFilePath))
            {
                config.CommandAssemblies.Add(new CommandAssembly { Path = targetFilePath });
                xmlUtil.WriteToFile(config, configFileFullPath);
                Console.WriteLine($"Successfully added assembly: {targetFilePath}");
            }
            else
                Console.WriteLine($"Invalid file: {targetFilePath}");
        }

        public void List()
        {
            Console.WriteLine("Cmd.List() called");
        }

        public void Remove(int[] dllIds)
        {
            Console.WriteLine("Cmd.Remove() called");
        }

        public void Disable(int[] dllIds)
        {
            Console.WriteLine("Cmd.Disable() called");
        }
    }
}
