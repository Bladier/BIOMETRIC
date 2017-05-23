Imports System.Data.OleDb
Imports Microsoft.Office.Interop
Imports System.Data.Odbc
Imports System.IO
Imports System.IO.Compression

Public Class frmAttendanceLogGenerator
    Dim dsEmpRecNew As DataSet
    Dim i As Integer = 0
    Dim tmpClock As Date
    Dim mysql As String = String.Empty
    Dim appPath As String = Application.StartupPath
    Dim verified_url As String
    Dim tmpDestination As String = ""
    Const batch As String = "\Extract.bat"

    Private Sub btnBrowse_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowse.Click
        OFD.ShowDialog()
        txtDBpath.Text = OFD.FileName
        database.dbSource = txtDBpath.Text
    End Sub

    Private Sub btnGenerate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGenerate.Click

        Dim sd As Date = dtFrom.Text
        Dim str As String = sd
        Dim filename As String = str.Replace("/"c, "_"c)
        generator()

        'Dim tmp_path1 As String = (Environment.GetFolderPath(Environment.SpecialFolder.Desktop) & "\Attlog" & txtBranch.Text & "" & filename & ".rar")

        'If My.Computer.FileSystem.FileExists(tmp_path1) Then
        '    My.Computer.FileSystem.DeleteFile(tmp_path1)
        'End If

        My.Computer.FileSystem.RenameFile(tmpDestination & "\Attlog" & txtBranch.Text & ".rar", "Attlog" & txtBranch.Text & "" & filename & ".rar")

        Dim myFile As String
        Dim mydir As String = Application.StartupPath
        'readValue()
        For Each myFile In Directory.GetFiles(mydir, "*.xlsx")
            File.Delete(myFile)
        Next

        If MsgBox("Command Successful", MsgBoxStyle.OkOnly + MsgBoxStyle.Information, _
            "Command") = MsgBoxResult.Ok Then tpbStatus.Minimum = 0 : tpbStatus.Value = 0 : lblStatus.Text = "Idle"
    End Sub

    Private Sub generator()
        If txtDBpath.Text = "" Then Exit Sub : If txtBranch.Text = "" Then Exit Sub

        Dim headers As String() = {"ID", "Employe ID", "Time Log"}
        mysql = "SELECT * FROM RAS_ATTRECORD"
        Dim ds As DataSet = LoadSQL(mysql, "RAS_ATTRECORD")

        ' MsgBox(ds.Tables(0).Rows.Count)

        Dim tblEmployeeTimeRecord As DataTable
        tblEmployeeTimeRecord = New DataTable

        'Add columns to it
        With tblEmployeeTimeRecord
            .Columns.Add("ID", Type.GetType("System.Int32"))
            .Columns.Add("EmployeID", Type.GetType("System.String"))
            .Columns.Add("TimeLog", Type.GetType("System.DateTime"))
        End With

        'Add rows and fill with data
        Dim EditRow As DataRow
        dsEmpRecNew = New DataSet
        For Each dr As DataRow In ds.Tables(0).Rows
            i = i + 1
            tmpClock = dr.Item(3)
            tmpClock = tmpClock.ToShortDateString

            If tmpClock >= dtFrom.Text Or tmpClock = dtTo.Text Then
                EditRow = tblEmployeeTimeRecord.NewRow()
                EditRow(0) = i
                EditRow(1) = IIf(Not IsDBNull(dr.Item("DIN")), dr.Item("DIN"), "")
                EditRow(2) = dr.Item("Clock")
                tblEmployeeTimeRecord.Rows.Add(EditRow)
                Console.WriteLine(i & " :" & dr.Item(2) & " :" & dr.Item(3))
            End If
        Next
        dsEmpRecNew.Tables.Add(tblEmployeeTimeRecord)

        SFD.FileName = String.Format("{2}{1}{0}.xlsx", Now.ToString("MMddyyyy"), txtBranch.Text, "Attlog")  'BranchCode + Date
        verified_url = appPath & "\" & SFD.FileName

        'Extracting
        ExtractToExcel(headers, dsEmpRecNew, verified_url, txtBranch.Text)

        'Making rar
        ' tmpDestination = tmpDestination
        Using sw As StreamWriter = File.CreateText("Extract.bat")
            sw.WriteLine("@echo off")
            sw.WriteLine("title ATTLOG - Extract")
            sw.WriteLine("echo Extracting. . .")
            sw.WriteLine("pause")
            sw.WriteLine("echo PLEASE WAIT WHILE SYSTEM Extracting...")
            sw.WriteLine("rar a " & tmpDestination & "\" & "Attlog" & txtBranch.Text & ".rar " & SFD.FileName & " -hp" & pswd)
            sw.WriteLine("cls ")
            sw.WriteLine("echo DONE!!! THANK YOU FOR WAITING")
            sw.WriteLine("pause")
            sw.WriteLine("exit")
        End Using
        'CommandPrompt(batch, appPath)
        Dim pro As New ProcessStartInfo(appPath & batch)
        pro.RedirectStandardError = True
        pro.RedirectStandardOutput = True
        pro.CreateNoWindow = False
        pro.WindowStyle = ProcessWindowStyle.Hidden
        pro.UseShellExecute = False
        Dim process As Process = process.Start(pro)

    End Sub

    Private Sub OFD_FileOk(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles OFD.FileOk

        If (OFD.FileName).Contains(".mdb") Then
            MsgBox("Dabatase connection successful!", MsgBoxStyle.Information)
        Else
            MsgBox("Unvalid file", MsgBoxStyle.Critical, "Error")
        End If
    End Sub

    Private Sub frmAttendanceLogGenerator_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        tmpDestination = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
    End Sub
End Class
