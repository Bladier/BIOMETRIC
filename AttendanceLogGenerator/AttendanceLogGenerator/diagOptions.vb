Public Class diagOptions
    Dim iniFile As New IniFile

    Private Sub diagOptions_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        LoadConfig()
    End Sub

    Private Sub LoadConfig()
        On Error Resume Next

        With iniFile
            .Load(frmAttendanceLogGenerator.configFile)
            mod_system.BranchCode = .GetSection("Extractor").GetKey("Branch").Value
            mod_system.DbPath = .GetSection("Extractor").GetKey("Path").Value
            mod_system.rarPath = .GetSection("Extractor").GetKey("Rarpath").Value

            txtBranch.Text = mod_system.BranchCode
            txtpath.Text = mod_system.DbPath
            txtRarPath.Text = mod_system.rarPath

            frmAttendanceLogGenerator.txtBranch.Text = mod_system.BranchCode
        End With
    End Sub

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If txtRarPath.Text = "" Then Exit Sub

        If Not System.IO.File.Exists(frmAttendanceLogGenerator.configFile) Then
            System.IO.File.Create(frmAttendanceLogGenerator.configFile).Dispose()
        End If

        With iniFile
            .Load(frmAttendanceLogGenerator.configFile)
            .AddSection("Extractor").AddKey("Path").Value = txtpath.Text
            .AddSection("Extractor").AddKey("Branch").Value = txtBranch.Text
            .AddSection("Extractor").AddKey("Rarpath").Value = txtRarPath.Text

            .Save(frmAttendanceLogGenerator.configFile)
        End With

        LoadConfig()
        frmAttendanceLogGenerator.LoadConfig()
        MsgBox("Configuration Saved", MsgBoxStyle.Information)
        Me.Close()
    End Sub

    Private Sub txtSave_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtpath.DoubleClick
        txtpath.Text = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
    End Sub
End Class