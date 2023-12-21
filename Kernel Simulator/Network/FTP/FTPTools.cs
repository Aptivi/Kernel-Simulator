﻿//
// Kernel Simulator  Copyright (C) 2018-2024  Aptivi
//
// This file is part of Kernel Simulator
//
// Kernel Simulator is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Kernel Simulator is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using System;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net;

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

using System.Net.Security;
using FluentFTP;
using KS.ConsoleBase.Colors;
using KS.ConsoleBase.Inputs;
using KS.Files;
using KS.Files.Querying;
using KS.Kernel;
using KS.Languages;
using KS.Misc.Probers;
using KS.Misc.Reflection;
using KS.Misc.Writers.ConsoleWriters;
using KS.Misc.Writers.DebugWriters;
using KS.Misc.Writers.FancyWriters;
using KS.Network.FTP.Transfer;

namespace KS.Network.FTP
{
    public static class FTPTools
    {

        /// <summary>
        /// Prompts user for a password
        /// </summary>
        /// <param name="user">A user name</param>
        /// <param name="Address">A host address</param>
        /// <param name="Port">A port for the address</param>
        public static void PromptForPassword(string user, string Address = "", int Port = 0, FtpEncryptionMode EncryptionMode = FtpEncryptionMode.Explicit)
        {
            // Make a new FTP client object instance (Used in case logging in using speed dial)
            if (FTPShellCommon.ClientFTP is null)
            {
                FTPShellCommon._clientFTP = new FtpClient()
                {
                    Host = Address,
                    Port = Port
                };
                FTPShellCommon._clientFTP.Config.RetryAttempts = FTPShellCommon.FtpVerifyRetryAttempts;
                FTPShellCommon._clientFTP.Config.ConnectTimeout = FTPShellCommon.FtpConnectTimeout;
                FTPShellCommon._clientFTP.Config.DataConnectionConnectTimeout = FTPShellCommon.FtpDataConnectTimeout;
                FTPShellCommon._clientFTP.Config.EncryptionMode = EncryptionMode;
                FTPShellCommon._clientFTP.Config.InternetProtocolVersions = FTPShellCommon.FtpProtocolVersions;
            }

            // Prompt for password
            if (!string.IsNullOrWhiteSpace(FTPShellCommon.FtpPassPromptStyle))
            {
                TextWriterColor.Write(PlaceParse.ProbePlaces(FTPShellCommon.FtpPassPromptStyle), false, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Input), user);
            }
            else
            {
                TextWriterColor.Write(Translate.DoTranslation("Password for {0}: "), false, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Input), user);
            }

            // Get input
            FTPShellCommon.FtpPass = Input.ReadLineNoInput();

            // Set up credentials
            FTPShellCommon.ClientFTP.Credentials = new NetworkCredential(user, FTPShellCommon.FtpPass);

            // Connect to FTP
            ConnectFTP();
        }

        /// <summary>
        /// Tries to connect to the FTP server
        /// </summary>
        /// <param name="address">An FTP server. You may specify it like "[address]" or "[address]:[port]"</param>
        public static void TryToConnect(string address)
        {
            if (FTPShellCommon.FtpConnected == true)
            {
                TextWriterColor.Write(Translate.DoTranslation("You should disconnect from server before connecting to another server"), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Error));
            }
            else
            {
                try
                {
                    // Create an FTP stream to connect to
                    string FtpHost = address.Replace("ftpes://", "").Replace("ftps://", "").Replace("ftp://", "").Replace(address.Substring(address.LastIndexOf(":")), "");
                    string FtpPort = address.Replace("ftpes://", "").Replace("ftps://", "").Replace("ftp://", "").Replace(FtpHost + ":", "");

                    // Check to see if no port is provided by client
                    if ((FtpHost ?? "") == (FtpPort ?? ""))
                    {
                        FtpPort = 0.ToString(); // Used for detecting of SSL is being used or not dynamically on connection
                    }

                    // Make a new FTP client object instance
                    FTPShellCommon._clientFTP = new FtpClient()
                    {
                        Host = FtpHost,
                        Port = Convert.ToInt32(FtpPort)
                    };
                    FTPShellCommon._clientFTP.Config.RetryAttempts = FTPShellCommon.FtpVerifyRetryAttempts;
                    FTPShellCommon._clientFTP.Config.ConnectTimeout = FTPShellCommon.FtpConnectTimeout;
                    FTPShellCommon._clientFTP.Config.DataConnectionConnectTimeout = FTPShellCommon.FtpDataConnectTimeout;
                    FTPShellCommon._clientFTP.Config.EncryptionMode = FtpEncryptionMode.Auto;
                    FTPShellCommon._clientFTP.Config.InternetProtocolVersions = FTPShellCommon.FtpProtocolVersions;

                    // Add handler for SSL validation
                    if (FTPShellCommon.FtpTryToValidateCertificate)
                        FTPShellCommon.ClientFTP.ValidateCertificate += new FtpSslValidation((ctrl, e) => TryToValidate((FtpClient)ctrl, e));

                    // Prompt for username
                    if (!string.IsNullOrWhiteSpace(FTPShellCommon.FtpUserPromptStyle))
                    {
                        TextWriterColor.Write(PlaceParse.ProbePlaces(FTPShellCommon.FtpUserPromptStyle), false, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Input), address);
                    }
                    else
                    {
                        TextWriterColor.Write(Translate.DoTranslation("Username for {0}: "), false, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Input), address);
                    }
                    FTPShellCommon.FtpUser = Input.ReadLine();
                    if (string.IsNullOrEmpty(FTPShellCommon.FtpUser))
                    {
                        DebugWriter.Wdbg(DebugLevel.W, "User is not provided. Fallback to \"anonymous\"");
                        FTPShellCommon.FtpUser = "anonymous";
                    }

                    // If we didn't abort, prompt for password
                    PromptForPassword(FTPShellCommon.FtpUser);
                }
                catch (Exception ex)
                {
                    DebugWriter.Wdbg(DebugLevel.W, "Error connecting to {0}: {1}", address, ex.Message);
                    DebugWriter.WStkTrc(ex);
                    TextWriterColor.Write(Translate.DoTranslation("Error when trying to connect to {0}: {1}"), true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Error), address, ex.Message);
                }
            }
        }

        /// <summary>
        /// Tries to connect to the FTP server.
        /// </summary>
        private static void ConnectFTP()
        {
            // Prepare profiles
            TextWriterColor.Write(Translate.DoTranslation("Preparing profiles... It could take several minutes..."), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Neutral));
            var profiles = FTPShellCommon.ClientFTP.AutoDetect(Flags.FTPFirstProfileOnly);
            var profsel = new FtpProfile();
            DebugWriter.Wdbg(DebugLevel.I, "Profile count: {0}", profiles.Count);
            if (profiles.Count > 1) // More than one profile
            {
                if (FTPShellCommon.FtpUseFirstProfile)
                {
                    profsel = profiles[0];
                }
                else
                {
                    string profanswer;
                    var profanswered = default(bool);
                    string[] ProfHeaders = ["#", Translate.DoTranslation("Host Name"), Translate.DoTranslation("Username"), Translate.DoTranslation("Data Type"), Translate.DoTranslation("Encoding"), Translate.DoTranslation("Encryption"), Translate.DoTranslation("Protocols")];
                    var ProfData = new string[profiles.Count, 7];
                    TextWriterColor.Write(Translate.DoTranslation("More than one profile found. Select one:"), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Neutral));
                    for (int i = 0, loopTo = profiles.Count - 1; i <= loopTo; i++)
                    {
                        ProfData[i, 0] = (i + 1).ToString();
                        ProfData[i, 1] = profiles[i].Host;
                        ProfData[i, 2] = profiles[i].Credentials.UserName;
                        ProfData[i, 3] = profiles[i].DataConnection.ToString();
                        ProfData[i, 4] = profiles[i].Encoding.EncodingName;
                        ProfData[i, 5] = profiles[i].Encryption.ToString();
                        ProfData[i, 6] = profiles[i].Protocols.ToString();
                    }
                    TableColor.WriteTable(ProfHeaders, ProfData, 2, KernelColorTools.ColTypes.Option);
                    while (!profanswered)
                    {
                        TextWriterColor.Write(Kernel.Kernel.NewLine + ">> ", false, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Input));
                        profanswer = Input.ReadLine();
                        DebugWriter.Wdbg(DebugLevel.I, "Selection: {0}", profanswer);
                        if (StringQuery.IsStringNumeric(profanswer))
                        {
                            try
                            {
                                DebugWriter.Wdbg(DebugLevel.I, "Profile selected");
                                int AnswerNumber = Convert.ToInt32(profanswer);
                                profsel = profiles[AnswerNumber - 1];
                                profanswered = true;
                            }
                            catch (Exception ex)
                            {
                                DebugWriter.Wdbg(DebugLevel.I, "Profile invalid");
                                TextWriterColor.Write(Translate.DoTranslation("Invalid profile selection.") + Kernel.Kernel.NewLine, true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Error));
                                DebugWriter.WStkTrc(ex);
                            }
                        }
                    }
                }
            }
            else if (profiles.Count == 1)
            {
                profsel = profiles[0]; // Select first profile
            }
            else // Failed trying to get profiles
            {
                TextWriterColor.Write(Translate.DoTranslation("Error when trying to connect to {0}: Connection timeout or lost connection"), true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Error), FTPShellCommon.ClientFTP.Host);
                return;
            }

            // Connect
            TextWriterColor.Write(Translate.DoTranslation("Trying to connect to {0} with profile {1}..."), true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Neutral), FTPShellCommon.ClientFTP.Host, profiles.IndexOf(profsel));
            DebugWriter.Wdbg(DebugLevel.I, "Connecting to {0} with {1}...", FTPShellCommon.ClientFTP.Host, profiles.IndexOf(profsel));
            FTPShellCommon.ClientFTP.Connect(profsel);

            // Show that it's connected
            TextWriterColor.Write(Translate.DoTranslation("Connected to {0}"), true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Success), FTPShellCommon.ClientFTP.Host);
            DebugWriter.Wdbg(DebugLevel.I, "Connected.");
            FTPShellCommon.FtpConnected = true;

            // If MOTD exists, show it
            if (FTPShellCommon.FtpShowMotd)
            {
                if (FTPShellCommon.ClientFTP.FileExists("welcome.msg"))
                {
                    TextWriterColor.Write(FTPTransfer.FTPDownloadToString("welcome.msg"), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Banner));
                }
                else if (FTPShellCommon.ClientFTP.FileExists(".message"))
                {
                    TextWriterColor.Write(FTPTransfer.FTPDownloadToString(".message"), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Banner));
                }
            }

            // Prepare to print current FTP directory
            FTPShellCommon.FtpCurrentRemoteDir = FTPShellCommon.ClientFTP.GetWorkingDirectory();
            DebugWriter.Wdbg(DebugLevel.I, "Working directory: {0}", FTPShellCommon.FtpCurrentRemoteDir);
            FTPShellCommon.FtpSite = FTPShellCommon.ClientFTP.Host;
            FTPShellCommon.FtpUser = FTPShellCommon.ClientFTP.Credentials.UserName;

            // Write connection information to Speed Dial file if it doesn't exist there
            var SpeedDialEntries = NetworkTools.ListSpeedDialEntries(NetworkTools.SpeedDialType.FTP);
            DebugWriter.Wdbg(DebugLevel.I, "Speed dial length: {0}", SpeedDialEntries.Count);
            if (SpeedDialEntries.ContainsKey(FTPShellCommon.FtpSite))
            {
                DebugWriter.Wdbg(DebugLevel.I, "Site already there.");
                return;
            }
            // Speed dial format is below:
            // Site,Port,Username,Encryption
            else if (FTPShellCommon.FtpNewConnectionsToSpeedDial)
                NetworkTools.AddEntryToSpeedDial(FTPShellCommon.FtpSite, FTPShellCommon.ClientFTP.Port, FTPShellCommon.FtpUser, NetworkTools.SpeedDialType.FTP, FTPShellCommon.ClientFTP.Config.EncryptionMode);
        }

        /// <summary>
        /// Tries to validate certificate
        /// </summary>
        public static void TryToValidate(FtpClient control, FtpSslValidationEventArgs e)
        {
            DebugWriter.Wdbg(DebugLevel.I, "Certificate checks");
            if (e.PolicyErrors == SslPolicyErrors.None)
            {
                DebugWriter.Wdbg(DebugLevel.I, "Certificate accepted.");
                DebugWriter.Wdbg(DebugLevel.I, e.Certificate.GetRawCertDataString());
                e.Accept = true;
            }
            else
            {
                DebugWriter.Wdbg(DebugLevel.W, $"Certificate error is {e.PolicyErrors}");
                TextWriterColor.Write(Translate.DoTranslation("During certificate validation, there are certificate errors. It might be the first time you've connected to the server or the certificate might have been expired. Here's an error:"), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Error));
                TextWriterColor.Write("- {0}", true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Error), e.PolicyErrors.ToString());
                if (FTPShellCommon.FtpAlwaysAcceptInvalidCerts)
                {
                    DebugWriter.Wdbg(DebugLevel.W, "Certificate accepted, although there are errors.");
                    DebugWriter.Wdbg(DebugLevel.I, e.Certificate.GetRawCertDataString());
                    e.Accept = true;
                }
                else
                {
                    string Answer = "";
                    while (!(Answer.ToLower() == "y" | Answer.ToLower() == "n"))
                    {
                        TextWriterColor.Write(Translate.DoTranslation("Are you sure that you want to connect?") + " (y/n) ", false, KernelColorTools.ColTypes.Question);
                        KernelColorTools.SetConsoleColor(KernelColorTools.InputColor);
                        Answer = Convert.ToString(Input.DetectKeypress().KeyChar);
                        TextWriterColor.WritePlain("", true);
                        DebugWriter.Wdbg(DebugLevel.I, $"Answer is {Answer}");
                        if (Answer.ToLower() == "y")
                        {
                            DebugWriter.Wdbg(DebugLevel.W, "Certificate accepted, although there are errors.");
                            DebugWriter.Wdbg(DebugLevel.I, e.Certificate.GetRawCertDataString());
                            e.Accept = true;
                        }
                        else if (Answer.ToLower() != "n")
                        {
                            DebugWriter.Wdbg(DebugLevel.W, "Invalid answer.");
                            TextWriterColor.Write(Translate.DoTranslation("Invalid answer. Please try again."), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Error));
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Opens speed dial prompt
        /// </summary>
        public static void QuickConnect()
        {
            if (Checking.FileExists(Paths.GetKernelPath(KernelPathType.FTPSpeedDial)))
            {
                var SpeedDialLines = NetworkTools.ListSpeedDialEntries(NetworkTools.SpeedDialType.FTP);
                DebugWriter.Wdbg(DebugLevel.I, "Speed dial length: {0}", SpeedDialLines.Count);
                string Answer;
                bool Answering = true;
                string[] SpeedDialHeaders = ["#", Translate.DoTranslation("Host Name"), Translate.DoTranslation("Host Port"), Translate.DoTranslation("Username"), Translate.DoTranslation("Encryption")];
                var SpeedDialData = new string[SpeedDialLines.Count, 5];
                if (!(SpeedDialLines.Count == 0))
                {
                    TextWriterColor.Write(Translate.DoTranslation("Select an address to connect to:"), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Neutral));
                    for (int i = 0, loopTo = SpeedDialLines.Count - 1; i <= loopTo; i++)
                    {
                        string SpeedDialAddress = SpeedDialLines.Keys.ElementAtOrDefault(i);
                        DebugWriter.Wdbg(DebugLevel.I, "Speed dial address: {0}", SpeedDialAddress);
                        SpeedDialData[i, 0] = (i + 1).ToString();
                        SpeedDialData[i, 1] = SpeedDialAddress;
                        SpeedDialData[i, 2] = (string)SpeedDialLines[SpeedDialAddress]["Port"];
                        SpeedDialData[i, 3] = (string)SpeedDialLines[SpeedDialAddress]["User"];
                        SpeedDialData[i, 4] = (string)SpeedDialLines[SpeedDialAddress]["FTP Encryption Mode"];
                    }
                    TableColor.WriteTable(SpeedDialHeaders, SpeedDialData, 2, KernelColorTools.ColTypes.Option);
                    TextWriterColor.WritePlain("", true);
                    while (Answering)
                    {
                        TextWriterColor.Write(">> ", false, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Input));
                        Answer = Input.ReadLine();
                        DebugWriter.Wdbg(DebugLevel.I, "Response: {0}", Answer);
                        if (StringQuery.IsStringNumeric(Answer))
                        {
                            DebugWriter.Wdbg(DebugLevel.I, "Response is numeric. IsStringNumeric(Answer) returned true. Checking to see if in-bounds...");
                            int AnswerInt = Convert.ToInt32(Answer);
                            if (AnswerInt <= SpeedDialLines.Count)
                            {
                                Answering = false;
                                DebugWriter.Wdbg(DebugLevel.I, "Response is in-bounds. Connecting...");
                                string ChosenSpeedDialAddress = SpeedDialLines.Keys.ElementAtOrDefault(AnswerInt - 1);
                                DebugWriter.Wdbg(DebugLevel.I, "Chosen connection: {0}", ChosenSpeedDialAddress);
                                string Address = ChosenSpeedDialAddress;
                                string Port = (string)SpeedDialLines[ChosenSpeedDialAddress]["Port"];
                                string Username = (string)SpeedDialLines[ChosenSpeedDialAddress]["User"];
                                FtpEncryptionMode Encryption = (FtpEncryptionMode)Convert.ToInt32(Enum.Parse(typeof(FtpEncryptionMode), (string)SpeedDialLines[ChosenSpeedDialAddress]["FTP Encryption Mode"]));
                                DebugWriter.Wdbg(DebugLevel.I, "Address: {0}, Port: {1}, Username: {2}, Encryption: {3}", Address, Port, Username, Encryption);
                                PromptForPassword(Username, Address, Convert.ToInt32(Port), Encryption);
                            }
                            else
                            {
                                DebugWriter.Wdbg(DebugLevel.I, "Response is out-of-bounds. Retrying...");
                                TextWriterColor.Write(Translate.DoTranslation("The selection is out of range. Select between 1-{0}. Try again."), true, color: KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Error), SpeedDialLines.Count);
                            }
                        }
                        else
                        {
                            DebugWriter.Wdbg(DebugLevel.W, "Response isn't numeric. IsStringNumeric(Answer) returned false.");
                            TextWriterColor.Write(Translate.DoTranslation("The selection is not a number. Try again."), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Error));
                        }
                    }
                }
                else
                {
                    DebugWriter.Wdbg(DebugLevel.E, "Speed dial is empty. Lines count is 0.");
                    TextWriterColor.Write(Translate.DoTranslation("Speed dial is empty. Connect to a server to add an address to it."), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Error));
                }
            }
            else
            {
                DebugWriter.Wdbg(DebugLevel.E, "File doesn't exist.");
                TextWriterColor.Write(Translate.DoTranslation("Speed dial doesn't exist. Connect to a server to add an address to it."), true, KernelColorTools.GetConsoleColor(KernelColorTools.ColTypes.Error));
            }
        }

    }

    class FTPTracer : TraceListener // Both Write and WriteLine do exactly the same thing, which is writing to a debugger.
    {

        /// <summary>
        /// Writes any message that the tracer has received to the debugger.
        /// </summary>
        /// <param name="Message">A message</param>
        public override void Write(string Message)
        {
            DebugWriter.Wdbg(DebugLevel.I, Message);
        }

        /// <summary>
        /// Writes any message that the tracer has received to the debugger. Please note that this does exactly as Write() since the debugger only supports writing with newlines.
        /// </summary>
        /// <param name="Message">A message</param>
        public override void WriteLine(string Message)
        {
            DebugWriter.Wdbg(DebugLevel.I, Message);
        }
    }
}
