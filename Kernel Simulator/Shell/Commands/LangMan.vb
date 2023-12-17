﻿
'    Kernel Simulator  Copyright (C) 2018-2022  Aptivi
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

Imports KS.Files.Querying

Namespace Shell.Commands
    Class LangManCommand
        Inherits CommandExecutor
        Implements ICommand

        Public Overrides Sub Execute(StringArgs As String, ListArgs() As String, ListArgsOnly As String(), ListSwitchesOnly As String()) Implements ICommand.Execute
            If Not SafeMode Then
                Dim CommandMode As String = ListArgsOnly(0).ToLower
                Dim TargetLanguage As String = ""
                Dim TargetLanguagePath As String = ""
                Dim LanguageListTerm As String = ""

                'These command modes require two arguments to be passed, so re-check here and there. Optional arguments also lie there.
                Select Case CommandMode
                    Case "reload", "load", "unload"
                        If ListArgsOnly.Length > 1 Then
                            TargetLanguage = ListArgsOnly(1)
                            TargetLanguagePath = NeutralizePath(TargetLanguage + ".json", GetKernelPath(KernelPathType.CustomLanguages))
                            If Not (TryParsePath(TargetLanguagePath) AndAlso FileExists(TargetLanguagePath)) And Not Languages.Languages.ContainsKey(TargetLanguage) Then
                                Write(DoTranslation("Language not found or file has invalid characters."), True, GetConsoleColor(ColTypes.Error))
                                Exit Sub
                            End If
                        Else
                            Write(DoTranslation("Language is not specified."), True, GetConsoleColor(ColTypes.Error))
                            Exit Sub
                        End If
                    Case "list"
                        If ListArgsOnly.Length > 1 Then
                            LanguageListTerm = ListArgsOnly(1)
                        End If
                End Select

                'Now, the actual logic
                Select Case CommandMode
                    Case "reload"
                        UninstallCustomLanguage(TargetLanguagePath)
                        InstallCustomLanguage(TargetLanguagePath)
                    Case "load"
                        InstallCustomLanguage(TargetLanguage)
                    Case "unload"
                        UninstallCustomLanguage(TargetLanguage)
                    Case "list"
                        For Each Language As String In ListLanguages(LanguageListTerm).Keys
                            WriteSeparator(Language, True)
                            Write("- " + DoTranslation("Language short name:") + " ", False, GetConsoleColor(ColTypes.ListEntry)) : Write(Languages.Languages(Language).ThreeLetterLanguageName, True, GetConsoleColor(ColTypes.ListValue))
                            Write("- " + DoTranslation("Language full name:") + " ", False, GetConsoleColor(ColTypes.ListEntry)) : Write(Languages.Languages(Language).FullLanguageName, True, GetConsoleColor(ColTypes.ListValue))
                            Write("- " + DoTranslation("Language transliterable:") + " ", False, GetConsoleColor(ColTypes.ListEntry)) : Write($"{Languages.Languages(Language).Transliterable}", True, GetConsoleColor(ColTypes.ListValue))
                            Write("- " + DoTranslation("Custom language:") + " ", False, GetConsoleColor(ColTypes.ListEntry)) : Write($"{Languages.Languages(Language).Custom}", True, GetConsoleColor(ColTypes.ListValue))
                        Next
                    Case "reloadall"
                        UninstallCustomLanguages()
                        InstallCustomLanguages()
                    Case Else
                        Write(DoTranslation("Invalid command {0}. Check the usage below:"), True, color:=GetConsoleColor(ColTypes.Error), CommandMode)
                        ShowHelp("langman")
                End Select
            Else
                Write(DoTranslation("Language management is disabled in safe mode."), True, GetConsoleColor(ColTypes.Error))
            End If
        End Sub

    End Class
End Namespace
