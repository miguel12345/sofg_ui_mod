using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Threading;
using Assets.Code;
using UnityEngine;
using UnityEngine.UI;
using Action = Assets.Code.Action;
using Object = System.Object;

namespace UIImprovements
{
    public class TCPCommandListener
    {
        private TcpListener tcpListener;
        private bool isRunning;
        private Thread serverThread;
        private Assembly currentAssembly;

        public TCPCommandListener(int port) {
            tcpListener = new TcpListener(IPAddress.Parse("127.0.0.1"), port);
        }

        public void StartAsync()
        {
            serverThread = new Thread(new ThreadStart(this.Start))
            {
                IsBackground = true // Set the thread to be a background thread
            };
            serverThread.Start();
        }

        public void Start()
        {
            tcpListener.Start();
            isRunning = true;
            Console.WriteLine("Server started. Listening for connections...");

            while (isRunning)
            {
                using (var client = tcpListener.AcceptTcpClient())
                using (var stream = client.GetStream())
                using (var reader = new StreamReader(stream))
                using (var writer = new StreamWriter(stream))
                {
                    string request = reader.ReadLine();
                    Debug.LogWarning($"Received: {request}");

                    string response = ProcessCommand(request);
                    writer.WriteLine(response);
                    writer.Flush();
                }
            }
        }

        private string ProcessCommand(string command)
        {
            if (command.StartsWith("reload_mod")) {
                //Get the file path from the command. Note that it might contain spaces.
                string path = command.Substring("reload_mod".Length).Trim();
                reloadMod(path);
                return $"Mod Reloaded from {path}!";
            }

            switch (command.ToLower())
            {
                case "patch":
                    OnPatchCommandReceived();
                    return "Methods patched";

                case "unload":
                    OnUnloadCommandReceived();
                    return "Unloaded!";

                default:
                    return "Unknown command";
            }
        }

        public void Stop()
        {
            tcpListener.Stop();
            isRunning = false;
        }

        void OnPatchCommandReceived() {
            Patches.PatchUIAction_duration();
        }

        void OnUnloadCommandReceived() {
            unloadMod();
        }

        void unloadMod() {
            Assembly currentAssembly = this.currentAssembly ?? Assembly.GetExecutingAssembly();
            Type myClassType = currentAssembly.GetType("UIImprovements.ModCore");
            MethodInfo unloadStaticMethod = myClassType.GetMethod("Unload", BindingFlags.Static | BindingFlags.Public);
            unloadStaticMethod.Invoke(null, null);
        }

        void reloadMod(string path) {

            unloadMod();

            Debug.LogWarning($"Loading mod from {path}");
            var newAssembly = Assembly.LoadFrom(path);
            var sameAssembly = Object.ReferenceEquals(newAssembly, currentAssembly);
            Debug.LogWarning($"Same assembly: {sameAssembly}");
            this.currentAssembly = newAssembly;
            Type myClassType = currentAssembly.GetType("UIImprovements.ModCore");
            MethodInfo loadStaticMethod = myClassType.GetMethod("Load", BindingFlags.Static | BindingFlags.Public);

            try
            {
                loadStaticMethod.Invoke(null, null);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }
    }
}
