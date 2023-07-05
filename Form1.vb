Imports System.ComponentModel
Imports System.IO
Imports System.IO.Compression
Public Class Form1
    Dim CloseAfterFinish As Boolean = False

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim OFD As New OpenFileDialog
        OFD.Filter = "Nuget Package File|*.nupkg"
        OFD.FileName = Nothing
        If OFD.ShowDialog = DialogResult.OK Then
            TextBox1.Text = OFD.FileName
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click

        If TextBox1.Text = Nothing Then
            MsgBox("Enter a nupkg file!", MsgBoxStyle.Exclamation, "Warning") : Exit Sub
        End If

        MkDir(Application.StartupPath & "\temp")
        If My.Computer.FileSystem.DirectoryExists(Application.StartupPath & "\extracted") = False Then
            MkDir(Application.StartupPath & "\extracted")
        End If
        BackgroundWorker1.RunWorkerAsync()
    End Sub

    Private Sub BackgroundWorker1_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker1.DoWork
        ZipFile.ExtractToDirectory(TextBox1.Text, Application.StartupPath & "\temp")
    End Sub

    Private Sub BackgroundWorker1_RunWorkerCompleted(sender As Object, e As RunWorkerCompletedEventArgs) Handles BackgroundWorker1.RunWorkerCompleted
        For Each f In Directory.GetFiles(Application.StartupPath & "\temp", "*.dll", SearchOption.AllDirectories)
            If File.Exists(f) Then
                File.Copy(f, Path.Combine(Application.StartupPath & "\extracted", Path.GetFileName(f)), True)
            End If

        Next
        My.Computer.FileSystem.DeleteDirectory(Application.StartupPath & "\temp", FileIO.DeleteDirectoryOption.DeleteAllContents)
        MsgBox("Conversion Successful")
        Process.Start(Application.StartupPath & "\extracted")

        If CloseAfterFinish = True Then
            End
        End If
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim args = Environment.GetCommandLineArgs()
        If args.Length > 1 Then
            If args(1).ToString.EndsWith(".nupkg") = True Then
                TextBox1.Text = args(1)
                CloseAfterFinish = True
                Button2.PerformClick()
            Else
                MsgBox("this file is not supported!, check that it is not damaged or has the "".nupkg"" extension and try again", MsgBoxStyle.Critical, "Error")
                End
            End If

        End If
    End Sub
End Class
