Imports System.Data.OleDb
Public Class frmAttendanceLogGenerator
    Private Sub btnBrowse_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowse.Click
        OFD.ShowDialog()
        txtDBpath.Text = OFD.FileName
        database.dbSource = txtDBpath.Text


        If ds.Tables(0).Rows.Count > 0 Then
            MsgBox("Dabatase connection successful!", MsgBoxStyle.Information)
        End If


    End Sub

    Private Sub btnGenerate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGenerate.Click
        If txtDBpath.Text = "" Then Exit Sub
        SFD.ShowDialog()


        Dim mysql As String = "SELECT * FROM RAS_ATTRECORD"
        Dim ds As DataSet = LoadSQL(mysql, "RAS_ATTRECORD")
    End Sub
End Class
