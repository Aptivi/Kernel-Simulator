﻿
'    Kernel Simulator  Copyright (C) 2018  EoflaOE
'
'    This file is part of Kernel Simulator
'
'    Kernel Simulator is free software: you can redistribute it and/or modify
'    it under the terms of the GNU General Public License as published by
'    the Free Software Foundation, either version 3 of the License, or
'    (at your option) any later version.
'
'    Kernel Simulator is distributed in the hope that it will be useful,
'    but WITHOUT ANY WARRANTY; without even the implied warranty of
'    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
'    GNU General Public License for more details.
'
'    You should have received a copy of the GNU General Public License
'    along with this program.  If not, see <https://www.gnu.org/licenses/>.

Imports System.CodeDom.Compiler
Imports System.Reflection

Public Module ModParser

    Public Interface IScript
        Sub StartMod()
        Sub StopMod()
        Property Cmd As String
        Property Def As String
        Property Name As String
        Property Version As String
        Sub PerformCmd(Optional ByVal args As String = "")
        Sub InitEvents(ByVal ev As String)
    End Interface
    Public scripts As New Dictionary(Of String, IScript)

    Private Function GenMod(ByVal code As String) As IScript
        Using provider As New VBCodeProvider()
            Dim prm As New CompilerParameters With {
                .GenerateExecutable = False,
                .GenerateInMemory = True
            }
            prm.ReferencedAssemblies.Add(Assembly.GetExecutingAssembly.Location)
            prm.ReferencedAssemblies.Add("System.dll")
            prm.ReferencedAssemblies.Add("System.Core.dll")
            prm.ReferencedAssemblies.Add("System.Data.dll")
            prm.ReferencedAssemblies.Add("System.DirectoryServices.dll")
            prm.ReferencedAssemblies.Add("System.Xml.dll")
            prm.ReferencedAssemblies.Add("System.Xml.Linq.dll")
            Dim namespc As String = GetType(IScript).Namespace
            Dim modCode() As String = New String() {"Imports " & namespc & vbNewLine & code}
            Dim res As CompilerResults = provider.CompileAssemblyFromSource(prm, modCode)
            If (res.Errors.HasErrors) And (Quiet = False) Then
                Wln(DoTranslation("Mod can't be loaded because of the following: ", currentLang), "neutralText")
                For Each errorName In res.Errors
                    Wln(errorName.ToString, "neutralText") : Wdbg(errorName.ToString, True)
                Next
                Exit Function
            End If
            For Each t As Type In res.CompiledAssembly.GetTypes()
                If t.GetInterface(GetType(IScript).Name) IsNot Nothing Then Return CType(res.CompiledAssembly.CreateInstance(t.Name), IScript)
            Next
        End Using
    End Function

    Private modPath As String
    Sub ReadyPath_MOD()
        If (EnvironmentOSType.Contains("Unix")) Then
            modPath = Environ("HOME") + "/KSMods/"
        Else
            modPath = Environ("USERPROFILE") + "\KSMods\"
        End If
    End Sub

    Sub ParseMods(ByVal StartStop As Boolean)
        'StartStop: If true, the mods start, otherwise, the mod stops.
        ReadyPath_MOD()
        If Not FileIO.FileSystem.DirectoryExists(modPath) Then FileIO.FileSystem.CreateDirectory(modPath)
        Dim count As Integer = FileIO.FileSystem.GetFiles(modPath).Count
        If (Quiet = False) And (count <> 0) And StartStop = True Then
            Wln(DoTranslation("mod: Loading mods...", currentLang), "neutralText")
            Wdbg("Mods are being loaded. Total mods with screensavers = {0}", count)
        ElseIf (Quiet = False) And (count <> 0) And StartStop = False Then
            Wln(DoTranslation("mod: Stopping mods...", currentLang), "neutralText")
            Wdbg("Mods are being stopped. Total mods with screensavers = {0}", count)
        End If
        If (StartStop = False) Then
            For Each script As IScript In scripts.Values
                script.StopMod()
                Wdbg("script.StopMod() initialized. Mod name: {0} | Version: {0}", script.Name, script.Version)
                If script.Name <> "" And script.Version <> "" Then Wln("{0} v{1} stopped", "neutralText", script.Name, script.Version)
            Next
        Else
            For Each modFile As String In FileIO.FileSystem.GetFiles(modPath)
                StartParse(modFile, StartStop)
            Next
        End If
    End Sub

    Sub StartParse(ByVal modFile As String, Optional ByVal StartStop As Boolean = True)
        modFile = modFile.Replace(modPath, "")
        If Not modFile.EndsWith(".m") Then
            'Ignore all mods who doesn't end with .m
            Wdbg("Unsupported file type for mod file {0}.", modFile)
        ElseIf modFile.EndsWith("SS.m") Then
            'Ignore all mods who ends with SS.m
            Wdbg("Mod file {0} is a screensaver and is ignored.", modFile)
        Else
            Dim script As IScript = GenMod(IO.File.ReadAllText(modPath + modFile))
            If Not IsNothing(script) Then
                script.StartMod()
                Wdbg("script.StartMod() initialized. Mod name: {0} | Version: {0}", script.Name, script.Version)
                If (script.Name = "") Then
                    Wdbg("No name for {0}", modFile)
                    Wln(DoTranslation("Mod {0} does not have the name. Review the source code.", currentLang), "neutralText", modFile)
                    scripts.Add(script.Cmd, script)
                Else
                    Wdbg("There is a name for {0}", modFile)
                    scripts.Add(script.Name, script)
                End If
                If (script.Version = "" And script.Name <> "") Then
                    Wdbg("{0}.Version = """" | {0}.Name = {1}", modFile, script.Name)
                    Wln(DoTranslation("Mod {0} does not have the version.", currentLang), "neutralText", script.Name)
                ElseIf script.Name <> "" And script.Version <> "" Then
                    Wdbg("{0}.Version = {2} | {0}.Name = {1}", modFile, script.Name, script.Version)
                    Wln(DoTranslation("{0} v{1} started", currentLang), "neutralText", script.Name, script.Version)
                End If
                If (script.Cmd <> "" And StartStop = True) Then
                    modcmnds.Add(script.Cmd)
                    If (script.Def = "") Then
                        Wln(DoTranslation("No definition for command {0}.", currentLang), "neutralText", script.Cmd)
                        Wdbg("{0}.Def = Nothing, {0}.Def = ""Command defined by {1}""", script.Cmd, script.Name)
                        script.Def = DoTranslation("Command defined by ", currentLang) + script.Name
                    End If
                    moddefs.Add(script.Cmd, script.Def)
                End If
            End If
        End If
    End Sub

End Module
