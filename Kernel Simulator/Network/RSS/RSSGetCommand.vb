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

Imports System.IO
Imports System.Text
Imports System.Threading
Imports Microsoft.VisualBasic.FileIO

Public Module RSSGetCommand

    'Variables
    Public RSSCommandThread As New Thread(AddressOf RSSParseCommand) With {.Name = "RSS Shell Command Thread"}

    Sub RSSParseCommand(ByVal CommandText As String)
        Try
            'Indicator if required arguments are provided
            Dim RequiredArgumentsProvided As Boolean = True

            'Get the index of the first space
            Dim index As Integer = CommandText.IndexOf(" ")
            If index = -1 Then index = CommandText.Length
            Wdbg("I", "Index: {0}", index)

            'Get the String Of arguments
            Dim strArgs As String = CommandText.Substring(index)
            Wdbg("I", "Prototype strArgs: {0}", strArgs)
            If Not index = CommandText.Length Then strArgs = strArgs.Substring(1)
            Wdbg("I", "Finished strArgs: {0}", strArgs)

            'Separate between command and arguments specified
            Dim Command As String = CommandText.Split(" ")(0)
            Dim Arguments() As String
            Dim TStream As New MemoryStream(Encoding.Default.GetBytes(strArgs))
            Dim Parser As New TextFieldParser(TStream) With {
                .Delimiters = {" "},
                .HasFieldsEnclosedInQuotes = True,
                .TrimWhiteSpace = False
            }
            Arguments = Parser.ReadFields
            If Arguments IsNot Nothing Then
                For i As Integer = 0 To Arguments.Length - 1
                    Arguments(i).Replace("""", "")
                Next
                RequiredArgumentsProvided = Arguments?.Length >= RSSCommands(Command).MinimumArguments
            ElseIf RSSCommands(Command).ArgumentsRequired And Arguments Is Nothing Then
                RequiredArgumentsProvided = False
            End If

            'Try to parse command
            If Command = "help" Then
                If Arguments?.Length > 0 Then
                    Wdbg("I", "Requested help for {0}", Arguments(0))
                    RSSShowHelp(Arguments(0))
                Else
                    Wdbg("I", "Requested help for all commands")
                    RSSShowHelp()
                End If
            ElseIf Command = "exit" Then
                RSSExiting = True
            ElseIf Command = "articleinfo" Then
                If RequiredArgumentsProvided Then
                    Dim ArticleIndex As Integer = Arguments(0) - 1
                    If ArticleIndex > RSSFeedInstance.FeedArticles.Count - 1 Then
                        W(DoTranslation("Article number couldn't be bigger than the available articles."), True, ColTypes.Err)
                        Wdbg("E", "Tried to access article number {0}, but count is {1}.", ArticleIndex, RSSFeedInstance.FeedArticles.Count - 1)
                    Else
                        Dim Article As RSSArticle = RSSFeedInstance.FeedArticles(ArticleIndex)
                        W("- " + DoTranslation("Title:") + " ", False, ColTypes.ListEntry)
                        W(Article.ArticleTitle, True, ColTypes.ListValue)
                        W("- " + DoTranslation("Link:") + " ", False, ColTypes.ListEntry)
                        W(Article.ArticleLink, True, ColTypes.ListValue)
                        For Each Variable As String In Article.ArticleVariables.Keys
                            If Not Variable = "title" And Not Variable = "link" And Not Variable = "summary" And Not Variable = "description" And Not Variable = "content" Then
                                W("- {0}: ", False, ColTypes.ListEntry, Variable)
                                W(Article.ArticleVariables(Variable).InnerText, True, ColTypes.ListValue)
                            End If
                        Next
                        W(vbNewLine + Article.ArticleDescription, True, ColTypes.Neutral)
                    End If
                End If
            ElseIf Command = "chfeed" Then
                If RequiredArgumentsProvided Then
                    RSSFeedLink = Arguments(0)
                End If
            ElseIf Command = "feedinfo" Then
                W("- " + DoTranslation("Title:") + " ", False, ColTypes.ListEntry)
                W(RSSFeedInstance.FeedTitle, True, ColTypes.ListValue)
                W("- " + DoTranslation("Link:") + " ", False, ColTypes.ListEntry)
                W(RSSFeedInstance.FeedUrl, True, ColTypes.ListValue)
                W("- " + DoTranslation("Description:") + " ", False, ColTypes.ListEntry)
                W(RSSFeedInstance.FeedDescription, True, ColTypes.ListValue)
                W("- " + DoTranslation("Feed type:") + " ", False, ColTypes.ListEntry)
                W(RSSFeedInstance.FeedType, True, ColTypes.ListValue)
                W("- " + DoTranslation("Number of articles:") + " ", False, ColTypes.ListEntry)
                W(RSSFeedInstance.FeedArticles.Count, True, ColTypes.ListValue)
            ElseIf Command = "list" Then
                For Each Article As RSSArticle In RSSFeedInstance.FeedArticles
                    W("- {0}: ", False, ColTypes.ListEntry, Article.ArticleTitle)
                    W(Article.ArticleLink, True, ColTypes.ListValue)
                    W("    {0}", True, ColTypes.Neutral, Article.ArticleDescription.Truncate(200))
                Next
            ElseIf Command = "read" Then
                If RequiredArgumentsProvided Then
                    Dim ArticleIndex As Integer = Arguments(0) - 1
                    Process.Start(RSSFeedInstance.FeedArticles(ArticleIndex).ArticleLink)
                End If
            End If

            'See if the command is done (passed all required arguments)
            If RSSCommands(Command).ArgumentsRequired And Not RequiredArgumentsProvided Then
                W(DoTranslation("Required arguments are not passed to command {0}"), True, ColTypes.Err, Command)
                Wdbg("E", "Passed arguments were not enough to run command {0}. Arguments passed: {1}", Command, Arguments?.Length)
                RSSShowHelp(Command)
            End If
        Catch ex As Exception
            W(DoTranslation("Error trying to run command: {0}"), True, ColTypes.Err, ex.Message)
            Wdbg("E", "Error running command {0}: {1}", CommandText.Split(" ")(0), ex.Message)
            WStkTrc(ex)
            EventManager.RaiseRSSCommandError(RSSFeedInstance.FeedUrl, CommandText, ex)
        End Try
    End Sub

    Sub RssShellCancelCommand(sender As Object, e As ConsoleCancelEventArgs)
        If e.SpecialKey = ConsoleSpecialKey.ControlC Then
            Console.WriteLine()
            DefConsoleOut = Console.Out
            Console.SetOut(StreamWriter.Null)
            e.Cancel = True
            RSSCommandThread.Abort()
        End If
    End Sub

End Module
