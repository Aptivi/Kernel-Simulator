﻿'
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

Public Module StringEvaluators

    ''' <summary>
    ''' Evaluates a variable
    ''' </summary>
    ''' <param name="Var">A full path to variable</param>
    ''' <returns></returns>
    Public Function Evaluate(ByVal Var As String) As Object
        Dim EvalP As New VBCodeProvider
        Dim EvalCP As New CompilerParameters With {.GenerateExecutable = False,
                                                   .GenerateInMemory = True}
        EvalCP.ReferencedAssemblies.Add(Assembly.GetExecutingAssembly.Location) 'It should reference itself
        EvalCP.ReferencedAssemblies.Add("System.dll")
        EvalCP.ReferencedAssemblies.Add("System.Core.dll")
        EvalCP.ReferencedAssemblies.Add("System.Data.dll")
        EvalCP.ReferencedAssemblies.Add("System.DirectoryServices.dll")
        EvalCP.ReferencedAssemblies.Add("System.Xml.dll")
        EvalCP.ReferencedAssemblies.Add("System.Xml.Linq.dll")
        Dim EvalCode As String = "Imports System" & Environment.NewLine &
                                 "Public Class Eval" & Environment.NewLine &
                                 "Public Shared Function Evaluate()" & Environment.NewLine &
                                 "Return " & Var & Environment.NewLine &
                                 "End Function" & Environment.NewLine &
                                 "End Class"
        Dim cr As CompilerResults = EvalP.CompileAssemblyFromSource(EvalCP, EvalCode)
        If cr.Errors.Count > 0 Then
            Wdbg("E", "Error in evaluation.")
            For Each Errors In cr.Errors
                Wdbg("E", Errors.ToString)
            Next
            Return ""
        Else
            Dim methInfo As MethodInfo = cr.CompiledAssembly.GetType("Eval").GetMethod("Evaluate")
            Return methInfo.Invoke(Nothing, Nothing)
        End If
    End Function

End Module
