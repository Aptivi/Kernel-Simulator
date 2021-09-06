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

Imports System.Text

Public Module FTPTransfer

    'Progress Bar Enabled
    Friend progressFlag As Boolean = True
    Friend ConsoleOriginalPosition_LEFT As Integer
    Friend ConsoleOriginalPosition_TOP As Integer

    ''' <summary>
    ''' Downloads a file from the currently connected FTP server
    ''' </summary>
    ''' <param name="File">A remote file</param>
    ''' <returns>True if successful; False if unsuccessful</returns>
    Public Function FTPGetFile(ByVal File As String) As Boolean
        Return FTPGetFile(File, File)
    End Function

    ''' <summary>
    ''' Downloads a file from the currently connected FTP server
    ''' </summary>
    ''' <param name="File">A remote file</param>
    ''' <param name="LocalFile">A name of the local file</param>
    ''' <returns>True if successful; False if unsuccessful</returns>
    Public Function FTPGetFile(ByVal File As String, ByVal LocalFile As String) As Boolean
        If FtpConnected Then
            Try
                'Show a message to download
                EventManager.RaiseFTPPreDownload(File)
                Wdbg("I", "Downloading file {0}...", File)

                'Try to download 3 times
                Dim LocalFilePath As String = NeutralizePath(LocalFile, FtpCurrentDirectory)
                Dim Result As FtpStatus = ClientFTP.DownloadFile(LocalFilePath, File, True, FtpVerify.Retry + FtpVerify.Throw, FileProgress)

                'Show a message that it's downloaded
                Wdbg("I", "Downloaded file {0}.", File)
                EventManager.RaiseFTPPostDownload(File, Result.IsSuccess)
                Return True
            Catch ex As Exception
                WStkTrc(ex)
                Wdbg("E", "Download failed for file {0}: {1}", File, ex.Message)
                EventManager.RaiseFTPPostDownload(File, False)
            End Try
        Else
            Throw New InvalidOperationException(DoTranslation("You must connect to server before performing transmission."))
        End If
        Return False
    End Function

    ''' <summary>
    ''' Downloads a folder from the currently connected FTP server
    ''' </summary>
    ''' <param name="Folder">A remote folder</param>
    ''' <returns>True if successful; False if unsuccessful</returns>
    Public Function FTPGetFolder(ByVal Folder As String) As Boolean
        Return FTPGetFolder(Folder, "")
    End Function

    ''' <summary>
    ''' Downloads a folder from the currently connected FTP server
    ''' </summary>
    ''' <param name="Folder">A remote folder</param>
    ''' <returns>True if successful; False if unsuccessful</returns>
    Public Function FTPGetFolder(ByVal Folder As String, ByVal LocalFolder As String) As Boolean
        If FtpConnected Then
            Try
                'Show a message to download
                EventManager.RaiseFTPPreDownload(Folder)
                Wdbg("I", "Downloading folder {0}...", Folder)

                'Try to download folder
                Dim LocalFolderPath As String = NeutralizePath(LocalFolder, FtpCurrentDirectory)
                Dim Results As List(Of FtpResult) = ClientFTP.DownloadDirectory(LocalFolderPath, Folder, FtpFolderSyncMode.Update, FtpLocalExists.Resume, FtpVerify.Retry + FtpVerify.Throw, Nothing, MultipleProgress)

                'Print download results to debugger
                Dim Failed As Boolean
                Wdbg("I", "Folder download result:")
                For Each Result As FtpResult In Results
                    Wdbg("I", "-- {0} --", Result.Name)
                    Wdbg("I", "Success: {0}", Result.IsSuccess)
                    Wdbg("I", "Skipped: {0}", Result.IsSkipped)
                    Wdbg("I", "Failure: {0}", Result.IsFailed)
                    Wdbg("I", "Size: {0}", Result.Size)
                    Wdbg("I", "Type: {0}", Result.Type)
                    If Result.IsFailed Then
                        Wdbg("E", "Download failed for {0}", Result.Name)

                        'Download could fail with no exception in very rare cases.
                        If Result.Exception IsNot Nothing Then
                            Wdbg("E", "Exception {0}", Result.Exception.Message)
                            WStkTrc(Result.Exception)
                        End If
                        Failed = True
                    End If
                    EventManager.RaiseFTPPostDownload(Result.Name, Not Failed)
                Next

                'Show a message that it's downloaded
                If Not Failed Then
                    Wdbg("I", "Downloaded folder {0}.", Folder)
                Else
                    Wdbg("I", "Downloaded folder {0} partially due to failure.", Folder)
                End If
                EventManager.RaiseFTPPostDownload(Folder, Not Failed)
                Return Not Failed
            Catch ex As Exception
                WStkTrc(ex)
                Wdbg("E", "Download failed for folder {0}: {1}", Folder, ex.Message)
                EventManager.RaiseFTPPostDownload(Folder, False)
            End Try
        Else
            Throw New InvalidOperationException(DoTranslation("You must connect to server before performing transmission."))
        End If
        Return False
    End Function

    ''' <summary>
    ''' Uploads a file to the currently connected FTP server
    ''' </summary>
    ''' <param name="File">A local file</param>
    ''' <returns>True if successful; False if unsuccessful</returns>
    Public Function FTPUploadFile(ByVal File As String) As Boolean
        Return FTPUploadFile(File, File)
    End Function

    ''' <summary>
    ''' Uploads a file to the currently connected FTP server
    ''' </summary>
    ''' <param name="File">A local file</param>
    ''' <param name="LocalFile">A name of the local file</param>
    ''' <returns>True if successful; False if unsuccessful</returns>
    Public Function FTPUploadFile(ByVal File As String, ByVal LocalFile As String) As Boolean
        If FtpConnected Then
            'Show a message to download
            EventManager.RaiseFTPPreUpload(File)
            Wdbg("I", "Uploading file {0}...", File)

            'Try to upload
            Dim LocalFilePath As String = NeutralizePath(LocalFile, FtpCurrentDirectory)
            Dim Success As Boolean = ClientFTP.UploadFile(LocalFilePath, File, True, True, FtpVerify.Retry, FileProgress)
            Wdbg("I", "Uploaded file {0} with status {1}.", File, Success)
            EventManager.RaiseFTPPostUpload(File, Success)
            Return Success
        Else
            Throw New InvalidOperationException(DoTranslation("You must connect to server before performing transmission."))
        End If
        Return False
    End Function

    ''' <summary>
    ''' Uploads a folder to the currently connected FTP server
    ''' </summary>
    ''' <param name="Folder">A local folder</param>
    ''' <returns>True if successful; False if unsuccessful</returns>
    Public Function FTPUploadFolder(ByVal Folder As String) As Boolean
        Return FTPUploadFolder(Folder, Folder)
    End Function

    ''' <summary>
    ''' Uploads a folder to the currently connected FTP server
    ''' </summary>
    ''' <param name="Folder">A local folder</param>
    ''' <param name="LocalFolder"></param>
    ''' <returns>True if successful; False if unsuccessful</returns>
    Public Function FTPUploadFolder(ByVal Folder As String, ByVal LocalFolder As String) As Boolean
        If FtpConnected Then
            'Show a message to download
            EventManager.RaiseFTPPreUpload(Folder)
            Wdbg("I", "Uploading folder {0}...", Folder)

            'Try to upload
            Dim LocalFolderPath As String = NeutralizePath(LocalFolder, FtpCurrentDirectory)
            Dim Results As List(Of FtpResult) = ClientFTP.UploadDirectory(LocalFolderPath, Folder, FtpFolderSyncMode.Update, FtpRemoteExists.Resume, FtpVerify.Retry, Nothing, MultipleProgress)

            'Print upload results to debugger
            Dim Failed As Boolean
            Wdbg("I", "Folder upload result:")
            For Each Result As FtpResult In Results
                Wdbg("I", "-- {0} --", Result.Name)
                Wdbg("I", "Success: {0}", Result.IsSuccess)
                Wdbg("I", "Skipped: {0}", Result.IsSkipped)
                Wdbg("I", "Failure: {0}", Result.IsFailed)
                Wdbg("I", "Size: {0}", Result.Size)
                Wdbg("I", "Type: {0}", Result.Type)
                If Result.IsFailed Then
                    Wdbg("E", "Upload failed for {0}", Result.Name)

                    'Upload could fail with no exception in very rare cases.
                    If Result.Exception IsNot Nothing Then
                        Wdbg("E", "Exception {0}", Result.Exception.Message)
                        WStkTrc(Result.Exception)
                    End If
                    Failed = True
                End If
                EventManager.RaiseFTPPostUpload(Result.Name, Not Failed)
            Next

            'Show a message that it's downloaded
            If Not Failed Then
                Wdbg("I", "Uploaded folder {0}.", Folder)
            Else
                Wdbg("I", "Uploaded folder {0} partially due to failure.", Folder)
            End If
            EventManager.RaiseFTPPostUpload(Folder, Not Failed)
            Return Not Failed
        Else
            Throw New InvalidOperationException(DoTranslation("You must connect to server before performing transmission."))
        End If
        Return False
    End Function

    ''' <summary>
    ''' Downloads a file to string
    ''' </summary>
    ''' <param name="File">A text file.</param>
    ''' <returns>Contents of the file</returns>
    Public Function FTPDownloadToString(ByVal File As String) As String
        If FtpConnected Then
            Try
                'Show a message to download
                EventManager.RaiseFTPPreDownload(File)
                Wdbg("I", "Downloading {0}...", File)

                'Try to download 3 times
                Dim DownloadedBytes() As Byte = {}
                Dim DownloadedContent As New StringBuilder
                Dim Downloaded As Boolean = ClientFTP.Download(DownloadedBytes, File)
                For Each DownloadedByte As Byte In DownloadedBytes
                    DownloadedContent.Append(Convert.ToChar(DownloadedByte))
                Next

                'Show a message that it's downloaded
                Wdbg("I", "Downloaded {0}.", File)
                EventManager.RaiseFTPPostDownload(File, Downloaded)
                Return DownloadedContent.ToString
            Catch ex As Exception
                WStkTrc(ex)
                Wdbg("E", "Download failed for {0}: {1}", File, ex.Message)
                EventManager.RaiseFTPPostDownload(File, False)
            End Try
        Else
            Throw New InvalidOperationException(DoTranslation("You must connect to server before performing transmission."))
        End If
        Return ""
    End Function

End Module
