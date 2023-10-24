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

Public Module UESHCommands

    ''' <summary>
    ''' The enumeration for the choice command output type
    ''' </summary>
    Public Enum ChoiceOutputType
        ''' <summary>
        ''' A question and a set of answers in one line
        ''' </summary>
        OneLine
        ''' <summary>
        ''' A question in a line and a set of answers in another line
        ''' </summary>
        TwoLines
        ''' <summary>
        ''' The modern way of listing choices
        ''' </summary>
        Modern
    End Enum

    ''' <summary>
    ''' Prompts user for choice
    ''' </summary>
    ''' <param name="Question">A question</param>
    ''' <param name="ScriptVariable">An $variable</param>
    ''' <param name="AnswersStr">Set of answers. They can be written like this: Y/N/C.</param>
    ''' <param name="OutputType">Output type of choices</param>
    Public Sub PromptChoice(ByVal Question As String, ByVal ScriptVariable As String, ByVal AnswersStr As String, Optional OutputType As ChoiceOutputType = ChoiceOutputType.OneLine)
        While True
            'Variables
            Dim answers As String() = {}
            Dim answer As String

            'Ask a question
            Select Case OutputType
                Case ChoiceOutputType.OneLine
                    Write(Question, False, ColTypes.Neutral)
                    Write(" <{0}> ", False, ColTypes.Input, AnswersStr)
                    answers = AnswersStr.Split("/")
                Case ChoiceOutputType.TwoLines
                    Write(Question, True, ColTypes.Neutral)
                    Write("<{0}> ", False, ColTypes.Input, AnswersStr)
                    answers = AnswersStr.Split("/")
                Case ChoiceOutputType.Modern
                    Write(Question + vbNewLine, True, ColTypes.Neutral)
                    answers = AnswersStr.Split("/")
                    For Each AnswerInstance As String In answers
                        Write($"{AnswerInstance})", True, ColTypes.Option)
                    Next
                    Write(vbNewLine + ">> ", False, ColTypes.Input)
            End Select

            'Wait for an answer
            answer = Console.ReadKey.KeyChar
            Console.WriteLine()

            'Check if answer if correct.
            If answers.Contains(answer) Then
                SetVariable(ScriptVariable, answer)
                Exit While
            End If
        End While
    End Sub

    ''' <summary>
    ''' Prompts user for input (answer the question with your own answers)
    ''' </summary>
    ''' <param name="ScriptVariable">An $variable</param>
    Public Sub PromptInput(ByVal Question As String, ByVal ScriptVariable As String)
        While True
            'Variables
            Dim Answer As String
            Wdbg("I", "Script var: {0} ({1}), Question: {2}", ScriptVariable, ShellVariables.ContainsKey(ScriptVariable), Question)

            'Ask a question
            Write(Question, False, ColTypes.Input)

            'Wait for an answer
            Answer = Console.ReadLine
            Wdbg("I", "Answer: {0}", Answer)

            Wdbg("I", "Setting {0} to {1}...", ScriptVariable, Answer)
            SetVariable(ScriptVariable, Answer)
            Exit While
        End While
    End Sub

End Module
