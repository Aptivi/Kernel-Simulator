﻿
// Kernel Simulator  Copyright (C) 2018-2022  Aptivi
// 
// This file is part of Kernel Simulator
// 
// Kernel Simulator is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Kernel Simulator is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Extensification.StringExts;
using KS.Kernel;
using KS.Kernel.Debugging;
using KS.Kernel.Power;
using KS.Languages;
using KS.Misc.Notifications;
using KS.Misc.Screensaver;

namespace KS.Network.RPC
{
    public static class RPCCommands
    {

        /// <summary>
        /// List of RPC commands.<br/>
        /// <br/>&lt;Request:Shutdown&gt;: Shuts down the remote kernel. Usage: &lt;Request:Shutdown&gt;(IP)
        /// <br/>&lt;Request:Reboot&gt;: Reboots the remote kernel. Usage: &lt;Request:Reboot&gt;(IP)
        /// <br/>&lt;Request:RebootSafe&gt;: Reboots the remote kernel to safe mode. Usage: &lt;Request:RebootSafe&gt;(IP)
        /// <br/>&lt;Request:Lock&gt;: Locks the computer remotely. Usage: &lt;Request:Lock&gt;(IP)
        /// <br/>&lt;Request:SaveScr&gt;: Saves the screen remotely. Usage: &lt;Request:SaveScr&gt;(IP)
        /// <br/>&lt;Request:Exec&gt;: Executes a command remotely. Usage: &lt;Request:Exec&gt;(Lock)
        /// <br/>&lt;Request:Acknowledge&gt;: Pings the remote kernel silently. Usage: &lt;Request:Acknowledge&gt;(IP)
        /// <br/>&lt;Request:Ping&gt;: Pings the remote kernel with notification. Usage: &lt;Request:Ping&gt;(IP)
        /// </summary>
        private readonly static List<string> RPCCommandsField = new()
        {
            "<Request:Shutdown>",
            "<Request:Reboot>",
            "<Request:RebootSafe>",
            "<Request:Lock>",
            "<Request:SaveScr>",
            "<Request:Exec>",
            "<Request:Acknowledge>",
            "<Request:Ping>"
        };

        /// <summary>
        /// Send an RPC command to another instance of KS using the specified address
        /// </summary>
        /// <param name="Request">A request</param>
        /// <param name="IP">An IP address which the RPC is hosted</param>
        public static void SendCommand(string Request, string IP)
        {
            SendCommand(Request, IP, RemoteProcedure.RPCPort);
        }

        /// <summary>
        /// Send an RPC command to another instance of KS using the specified address
        /// </summary>
        /// <param name="Request">A request</param>
        /// <param name="IP">An IP address which the RPC is hosted</param>
        /// <param name="Port">A port which the RPC is hosted</param>
        /// <exception cref="InvalidOperationException"></exception>
        public static void SendCommand(string Request, string IP, int Port)
        {
            if (RemoteProcedure.RPCEnabled)
            {
                // Get the command and the argument
                string Cmd = Request.Remove(Request.IndexOf("("));
                DebugWriter.WriteDebug(DebugLevel.I, "Command: {0}", Cmd);
                string Arg = Request.Substring(Request.IndexOf("(") + 1);
                DebugWriter.WriteDebug(DebugLevel.I, "Prototype Arg: {0}", Arg);
                Arg = Arg.Remove(Arg.Length - 1);
                DebugWriter.WriteDebug(DebugLevel.I, "Finished Arg: {0}", Arg);

                // Check the command
                var Malformed = default(bool);
                if (RPCCommandsField.Contains(Cmd))
                {
                    DebugWriter.WriteDebug(DebugLevel.I, "Command found.");

                    // Check the request type
                    string RequestType = Cmd.Substring(Cmd.IndexOf(":") + 1, Finish: Cmd.IndexOf(">"));
                    var ByteMsg = Array.Empty<byte>();
                    switch (RequestType ?? "")
                    {
                        case "Shutdown":
                        case "Reboot":
                        case "RebootSafe":
                        case "Lock":
                        case "SaveScr":
                        case "Exec":
                        case "Acknowledge":
                        case "Ping":
                            {
                                // Populate the byte message to send the confirmation to
                                DebugWriter.WriteDebug(DebugLevel.I, "Stream opened for device {0}", Arg);
                                ByteMsg = System.Text.Encoding.Default.GetBytes($"{RequestType}Confirm, " + Arg + Kernel.Kernel.NewLine);
                                break;
                            }

                        default:
                            {
                                // Rare case reached. Drop it.
                                DebugWriter.WriteDebug(DebugLevel.E, "Malformed request. {0}", Cmd);
                                Malformed = true;
                                break;
                            }
                    }

                    // Send the response
                    if (!Malformed)
                    {
                        DebugWriter.WriteDebug(DebugLevel.I, "Sending response to device...");
                        RemoteProcedure.RPCListen.Send(ByteMsg, ByteMsg.Length, IP, Port);
                        Kernel.Kernel.KernelEventManager.RaiseRPCCommandSent(Cmd, Arg, IP, Port);
                    }
                }
            }
            else
            {
                throw new InvalidOperationException(Translate.DoTranslation("Trying to send an RPC command while RPC didn't start."));
            }
        }

        /// <summary>
        /// Thread to listen to commands.
        /// </summary>
        public static void ReceiveCommand()
        {
            var RemoteEndpoint = new IPEndPoint(IPAddress.Any, RemoteProcedure.RPCPort);
            while (RemoteProcedure.RPCStarted)
            {
                Thread.Sleep(1);
                byte[] MessageBuffer;
                string Message = "";
                try
                {
                    MessageBuffer = RemoteProcedure.RPCListen.Receive(ref RemoteEndpoint);
                    Message = System.Text.Encoding.Default.GetString(MessageBuffer);

                    // If the message is not empty, parse it
                    if (!string.IsNullOrEmpty(Message))
                    {
                        DebugWriter.WriteDebug((DebugLevel)Convert.ToInt32("RPC: Received message {0}"), Message);
                        Kernel.Kernel.KernelEventManager.RaiseRPCCommandReceived(Message, RemoteEndpoint.Address.ToString(), RemoteEndpoint.Port);

                        // Iterate through every confirmation message
                        if (Message.StartsWith("ShutdownConfirm"))
                        {
                            DebugWriter.WriteDebug(DebugLevel.I, "Shutdown confirmed from remote access.");
                            KernelTools.RPCPowerListener.Start(PowerMode.Shutdown);
                        }
                        else if (Message.StartsWith("RebootConfirm"))
                        {
                            DebugWriter.WriteDebug(DebugLevel.I, "Reboot confirmed from remote access.");
                            KernelTools.RPCPowerListener.Start(PowerMode.Reboot);
                        }
                        else if (Message.StartsWith("RebootSafeConfirm"))
                        {
                            DebugWriter.WriteDebug(DebugLevel.I, "Reboot to safe mode confirmed from remote access.");
                            KernelTools.RPCPowerListener.Start(PowerMode.RebootSafe);
                        }
                        else if (Message.StartsWith("LockConfirm"))
                        {
                            DebugWriter.WriteDebug(DebugLevel.I, "Lock confirmed from remote access.");
                            Screensaver.LockScreen();
                        }
                        else if (Message.StartsWith("SaveScrConfirm"))
                        {
                            DebugWriter.WriteDebug(DebugLevel.I, "Save screen confirmed from remote access.");
                            Screensaver.ShowSavers(Screensaver.DefSaverName);
                        }
                        else if (Message.StartsWith("ExecConfirm"))
                        {
                            if (Flags.LoggedIn)
                            {
                                DebugWriter.WriteDebug(DebugLevel.I, "Exec confirmed from remote access.");
                                ConsoleBase.ConsoleWrapper.WriteLine();
                                Shell.Shell.GetLine(Message.Replace("ExecConfirm, ", "").Replace(Kernel.Kernel.NewLine, ""));
                            }
                            else
                            {
                                DebugWriter.WriteDebug(DebugLevel.W, "Tried to exec from remote access while not logged in. Dropping packet...");
                            }
                        }
                        else if (Message.StartsWith("AckConfirm"))
                        {
                            DebugWriter.WriteDebug(DebugLevel.I, "{0} says \"Hello.\"", Message.Replace("AckConfirm, ", "").Replace(Kernel.Kernel.NewLine, ""));
                        }
                        else if (Message.StartsWith("PingConfirm"))
                        {
                            string IPAddr = Message.Replace("PingConfirm, ", "").Replace(Kernel.Kernel.NewLine, "");
                            DebugWriter.WriteDebug(DebugLevel.I, "{0} pinged this device!", IPAddr);
                            Notifications.NotifySend(new Notification(Translate.DoTranslation("Ping!"), Translate.DoTranslation("{0} pinged you.").FormatString(IPAddr), Notifications.NotifPriority.Low, Notifications.NotifType.Normal));
                        }
                        else
                        {
                            DebugWriter.WriteDebug(DebugLevel.W, "Not found. Message was {0}", Message);
                        }
                    }
                }
                catch (Exception ex)
                {
                    SocketException SE = (SocketException)ex.InnerException;
                    if (SE is not null)
                    {
                        if (!(SE.SocketErrorCode == SocketError.TimedOut))
                        {
                            DebugWriter.WriteDebug(DebugLevel.E, "Error from host: {0}", SE.SocketErrorCode.ToString());
                            DebugWriter.WriteDebugStackTrace(ex);
                        }
                    }
                    else
                    {
                        DebugWriter.WriteDebug(DebugLevel.E, "Fatal error: {0}", ex.Message);
                        DebugWriter.WriteDebugStackTrace(ex);
                        Kernel.Kernel.KernelEventManager.RaiseRPCCommandError(Message, ex, RemoteEndpoint.Address.ToString(), RemoteEndpoint.Port);
                    }
                }
            }
        }

    }
}
