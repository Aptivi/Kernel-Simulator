﻿
'    Kernel Simulator  Copyright (C) 2018-2021  EoflaOE
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

Public Class ArgumentInfo

    ''' <summary>
    ''' The argument
    ''' </summary>
    Public ReadOnly Property Argument As String
    ''' <summary>
    ''' The type of argument
    ''' </summary>
    Public ReadOnly Property Type As ArgumentType
    ''' <summary>
    ''' The untranslated help definition of argument. Translated by <see cref="GetTranslatedHelpEntry()"/>
    ''' </summary>
    Public Property HelpDefinition As String
    ''' <summary>
    ''' Does the argument require arguments?
    ''' </summary>
    Public ReadOnly Property ArgumentsRequired As Boolean
    ''' <summary>
    ''' User must specify at least this number of arguments
    ''' </summary>
    Public ReadOnly Property MinimumArguments As Integer
    ''' <summary>
    ''' Is the argument obsolete?
    ''' </summary>
    Public ReadOnly Property Obsolete As Boolean

    ''' <summary>
    ''' Installs a new instance of argument info class
    ''' </summary>
    ''' <param name="Argument">Argument</param>
    ''' <param name="Type">Argument type</param>
    ''' <param name="HelpDefinition">Argument help definition</param>
    ''' <param name="ArgumentsRequired">Does the argument require arguments?</param>
    ''' <param name="MinimumArguments">User must specify at least this number of arguments</param>
    ''' <param name="Obsolete">Is the command obsolete?</param>
    Public Sub New(ByVal Argument As String, ByVal Type As ShellCommandType, ByVal HelpDefinition As String, ByVal ArgumentsRequired As Boolean, ByVal MinimumArguments As Integer, Optional Obsolete As Boolean = False)
        Me.Argument = Argument
        Me.Type = Type
        Me.HelpDefinition = HelpDefinition
        Me.ArgumentsRequired = ArgumentsRequired
        Me.MinimumArguments = MinimumArguments
        Me.Obsolete = Obsolete
    End Sub

    ''' <summary>
    ''' Gets the translated version of help entry (KS built-in arguments only)
    ''' </summary>
    ''' <returns></returns>
    Public Function GetTranslatedHelpEntry() As String
        Return DoTranslation(HelpDefinition)
    End Function

End Class

''' <summary>
''' Argument type enumeration
''' </summary>
Public Enum ArgumentType
    ''' <summary>
    ''' Kernel arguments
    ''' </summary>
    KernelArgs
    ''' <summary>
    ''' Command line arguments
    ''' </summary>
    CommandLineArgs
End Enum