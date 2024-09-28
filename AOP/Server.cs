using System.Collections.Generic;
using System.Diagnostics;
using System;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using Debug = CitizenFX.Core.Debug;

namespace BeyondRP.AOP.Server
{
    public class Server : BaseScript
    {
        #region Variables
        private static string currentAOP = "Statewide";
        #endregion

        #region Constructor
        public Server()
        {
            Debug.WriteLine($"AOP Script by ReckerXF loaded. Setting default AOP to {currentAOP}!");

            SetConvarServerInfo("currentAOP", currentAOP);
            SetConvar("currentAOP", currentAOP);
        }
        #endregion

        #region Commands
        [Command("setaop")]
        private void SetAOPCmd(int src, List<object> args, string raw)
        {
            string newAOP = string.Join(" ", args);

            SetAOP(src, newAOP);
            SetConvarServerInfo("currentAOP", newAOP);
            SetConvar("currentAOP", currentAOP);
        }

        [Command("aop")]
        private void CheckAOPCmd(int src, List<object> args, string raw)
        {
            Player ply = Players[src];

            var messageData = new
            {
                args = new[] { $"The AOP is currently ~g~{currentAOP}~s~!" },
                tags = new[] { "server" },
                channel = "server"
            };
            ply.TriggerEvent("chat:addMessage", messageData);
        }
        #endregion

        #region Events
        [EventHandler("BeyondRP:AOP:SetAOP")]
        private static void SetAOP(int setter, string aop)
        {
            string setterHandle = setter.ToString();
            string setterName;
            currentAOP = aop;

            if (!IsPlayerAceAllowed(setterHandle, "vMenu.Staff") && setterHandle != "0") // Hook into our vMenu permissions
                return;

            if (setterHandle == "0")
            {
                setterName = "Administration";
                Debug.WriteLine($"SUCCESS! Set AOP to {aop}"); ;
            }
            else
            {
                setterName = GetPlayerName(setterHandle);
            }

            var messageData = new
            {
                args = new[] { $"The AOP has been set to ~g~{currentAOP} ~s~by ~g~{setterName}~s~!" },
                tags = new[] { "server" },
                channel = "server"
            };
            TriggerClientEvent("chat:addMessage", messageData);
        }
        #endregion

    }
}