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


    Friend configFile As String = "bladier.ini"
    Friend iniFile As New IniFile

    Private Sub btnGenerate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGenerate.Click
        If txtBranch.Text = "" Then txtBranch.Focus() : Exit Sub

        If Not mod_system.DbPath.Contains(".mdb") Then
            MsgBox("Database not found", MsgBoxStyle.Critical, "Error")
            Exit Sub
        End If

        Me.Enabled = False
        Dim str As String = Now.ToString("MM/dd/yyyy")
        Dim filename As String = str.Replace("/"c, "_"c)

        Dim msg As DialogResult = MsgBox("Do you want to generate selected file?", MsgBoxStyle.YesNo)
        If msg = vbNo Then Me.Enabled = True : Exit Sub

        generator()

        Dim tmp_path1 As String = (Environment.GetFolderPath(Environment.SpecialFolder.Desktop) & "\attlog" & txtBranch.Text & "" & filename & ".rar")

        If My.Computer.FileSystem.FileExists(tmp_path1) Then
            MsgBox("This file is already exists" & vbCrLf & "Please Delete other one!", MsgBoxStyle.Critical, "Error")
            If My.Computer.FileSystem.FileExists(tmpDestination) Then
                MsgBox("Please try again!", MsgBoxStyle.Critical, "Error")
                My.Computer.FileSystem.DeleteFile(tmpDestination)
            End If : tmpDestination = ""
            clear()
            DeleteXLS() : Exit Sub
        End If

        Dim idx As Integer = 100

        For id As Integer = 0 To idx - 1
            If id = 99 Then Exit For
        Next

        MsgBox("Thank you!", MsgBoxStyle.Information, "Information")

        'My.Computer.FileSystem.RenameFile(tmpDestination, "attlog" & txtBranch.Text & "" & filename & ".rar")

        If MsgBox("Command Successful", MsgBoxStyle.OkOnly + MsgBoxStyle.Information, _
            "Command") = MsgBoxResult.Ok Then tpbStatus.Minimum = 0 : tpbStatus.Value = 0 : lblStatus.Text = "Idle"

        DeleteXLS()
        Me.Enabled = True
    End Sub

    Private Sub clear()
        tpbStatus.Maximum = 0 : tpbStatus.Value = 0 : lblStatus.Text = "Idle"
    End Sub
    Private Sub DeleteXLS()
        Dim myFile As String
        Dim mydir As String = Application.StartupPath
        'readValue()
        For Each myFile In Directory.GetFiles(mydir, "*.xlsx")
            File.Delete(myFile)
        Next
        Me.Enabled = True
    End Sub

    Private Sub generator()


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
            tmpClock = dr.Item(3)
            tmpClock = tmpClock.ToShortDateString

            If tmpClock >= dtFrom.Text And tmpClock <= dtTo.Text Then
                i = i + 1
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

        Dim truFileName As String = SFD.FileName
        'Extracting
        ExtractToExcel(headers, dsEmpRecNew, verified_url, txtBranch.Text)

        If File.Exists(appPath & "\" & batch) Then
            My.Computer.FileSystem.DeleteFile(appPath & "\" & batch)
        End If
        ' MsgBox("rar a " & mod_system.rarPath & "Attlog" & txtBranch.Text & Now.ToString("MMddyyyy") & ".rar " & truFileName & " -hp" & pswd)
        'Making rar
        'tmpDestination = tmpDestination.Replace(" ", "")
        Using sw As StreamWriter = File.CreateText("Extract.bat")
            sw.WriteLine("@echo off")
            sw.WriteLine("title ATTLOG - Extract")
            sw.WriteLine("echo Extracting. . .")
            sw.WriteLine("pause")
            sw.WriteLine("echo PLEASE WAIT WHILE SYSTEM Extracting...")
            sw.WriteLine("rar a " & mod_system.rarPath & "\Attlog" & txtBranch.Text & Now.ToString("MMddyyyy") & ".rar " & truFileName & " -hp" & pswd)
            sw.WriteLine("cls ")
            sw.WriteLine("echo DONE!!! THANK YOU FOR WAITING")
            sw.WriteLine("pause")
            sw.WriteLine("exit")
        End Using

        tmpDestination = tmpDestination & "\" & "Attlog" & txtBranch.Text & Now.ToString("MMddyyyy") & ".rar "
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
        LoadConfig()
        tmpDestination = mod_system.rarPath
        ' tmpDestination = Path.GetPathRoot(Environment.SystemDirectory)
        'Environment.GetFolderPath(Environment.SpecialFolder.Desktop)

    End Sub


#Region "Database"

    Friend Sub LoadConfig()
        With iniFile
            .Load(configFile)
            mod_system.DbPath = IIf(IsError(.GetSection("Extractor").GetKey("Path").Value), "", .GetSection("Extractor").GetKey("Path").Value)
            mod_system.BranchCode = IIf(IsError(.GetSection("Extractor").GetKey("Branch").Value), "", .GetSection("Extractor").GetKey("Branch").Value)
            mod_system.rarPath = IIf(IsError(.GetSection("Extractor").GetKey("Rarpath").Value), "", .GetSection("Extractor").GetKey("Rarpath").Value)
            database.dbSource = mod_system.DbPath

            If mod_system.BranchCode = "" Or _
                mod_system.DbPath = "" Or _
                database.dbSource = "ANOISIM.MDB" Then
                diagOptions.Show()
            End If

            If Not mod_system.DbPath.Contains(".mdb") Then
                MsgBox("File not set", MsgBoxStyle.Critical, "Error")
                Exit Sub
            End If

            txtBranch.Text = mod_system.BranchCode
        End With
    End Sub
#End Region

    Private Sub frmAttendanceLogGenerator_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.DoubleClick
        diagOptions.Show()
    End Sub
End Class
