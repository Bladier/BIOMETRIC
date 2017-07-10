Imports Microsoft.Office.Interop
Public Class frmJelCal

    Dim fileName As String
    Dim isDone As Boolean = False
    Dim tmpSavePath As String
    Dim tmpKarats As Double = 0.0
    Dim tmpgrams As Double = 0.0
    Dim tmpcls As String

    Dim SalePrice As Double = 0.0

    Private Sub btnBrowse_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowse.Click
        ofdJeltmp.ShowDialog()

        fileName = ofdJeltmp.FileName

        If fileName = "" Then Exit Sub
        lblPath.Text = fileName
    End Sub

    Private Sub btnGenerate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGenerate.Click
        'Load Excel
        Dim oXL As New Excel.Application
        Dim oWB As Excel.Workbook
        Dim oSheet As Excel.Worksheet
        Dim lineNum As Integer = 0

        'SAVE
        Dim oXL1 As New Excel.Application
        Dim oWB1 As Excel.Workbook
        Dim oSheet1 As Excel.Worksheet
        oWB1 = oXL.Workbooks.Open(Application.StartupPath & "/tmp/IMD Template.xlsx")
        oSheet1 = oWB1.Worksheets(1)

        oWB = oXL.Workbooks.Open(fileName)
        oSheet = oWB.Worksheets(1)

        Dim MaxColumn As Integer = oSheet.Cells(1, oSheet.Columns.Count).End(Excel.XlDirection.xlToLeft).column
        Dim MaxEntries As Integer = oSheet.Cells(oSheet.Rows.Count, 1).End(Excel.XlDirection.xlUp).row

        Dim checkHeaders(MaxColumn) As String
        For cnt As Integer = 0 To MaxColumn - 1
            checkHeaders(cnt) = oSheet.Cells(1, cnt + 1).value
        Next : checkHeaders(MaxColumn) = oWB.Worksheets(1).name

        pbstatus.Maximum = MaxEntries - 1

        Me.Enabled = False
        For cnt = 2 To MaxEntries
            Dim JewTmp As New Karat
            Dim tmpClass As New Classes
            With JewTmp
                Console.WriteLine("Description:" & oSheet.Cells(cnt, 3).value)
                tmpKarats = .ParseKarat(oSheet.Cells(cnt, 3).value)
                tmpgrams = .ParseGrams(oSheet.Cells(cnt, 3).value)
                tmpcls = .ParseClass(oSheet.Cells(cnt, 2).value)

                .LoadKarat(.ParseCategory(oSheet.Cells(cnt, 2).value), tmpKarats)

                tmpClass.LoadClass(.KaratID, tmpcls)

                SalePrice = (tmpgrams * tmpClass.Price) * 2

                Console.WriteLine(SalePrice)


                Dim recCnt2 As Single = 0
                With oSheet
                    oSheet1.Cells(cnt, 1) = oSheet.Cells(cnt, 2).value
                    oSheet1.Cells(cnt, 2) = oSheet.Cells(cnt, 3).value
                    oSheet1.Cells(cnt, 3) = ""
                    oSheet1.Cells(cnt, 4) = "JEWELRY"
                    oSheet1.Cells(cnt, 5) = ""
                    oSheet1.Cells(cnt, 6) = "PIECE"
                    oSheet1.Cells(cnt, 7) = 0
                    oSheet1.Cells(cnt, 8) = SalePrice
                    oSheet1.Cells(cnt, 9) = "Y"
                    oSheet1.Cells(cnt, 10) = "Y"
                    oSheet1.Cells(cnt, 11) = "N"
                    oSheet1.Cells(cnt, 12) = "Y"
                    oSheet1.Cells(cnt, 13) = 50
                    oSheet1.Cells(cnt, 14) = 40
                End With

                pbstatus.Value = pbstatus.Value + 1
                Application.DoEvents()
                lblstatus.Text = String.Format("{0}%", ((pbstatus.Value / pbstatus.Maximum) * 100).ToString("F2"))

            End With
        Next

        sfdPath.FileName = Now.ToString("MMddyyyy")
        tmpSavePath = tmpSavePath & "/" & sfdPath.FileName

        tmpSavePath = tmpSavePath & "_JWL"
        Me.Enabled = True
        isDone = True

        oWB1.SaveAs(tmpSavePath)
        oSheet1 = Nothing
        oWB1.Close(False)
        oWB1 = Nothing
        oXL1.Quit()
        oXL1 = Nothing


unloadObj:
        'Memory Unload
        oSheet = Nothing
        oWB = Nothing
        oXL.Quit()
        oXL = Nothing

        fileName = ""
        lblPath.Text = "File not yet"

        If isDone Then
            If MsgBox("Successful generated", MsgBoxStyle.OkOnly + MsgBoxStyle.Information, _
            "Generate...") = MsgBoxResult.Ok Then pbstatus.Minimum = 0 : pbstatus.Value = 0 : lblstatus.Text = "0.00%"
        End If
    End Sub

    Private Sub frmJelCal_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        tmpSavePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
    End Sub


    'Private Sub Button1_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    Dim oXL As New Excel.Application
    '    Dim oWB As Excel.Workbook
    '    Dim oSheet As Excel.Worksheet
    '    oWB = oXL.Workbooks.Open("C:\Users\MISGWAPOHON\Desktop\2016\tmpKarat.xlsx")
    '    oSheet = oWB.Worksheets(1)

    '    Dim MaxColumn As Integer = oSheet.Cells(1, oSheet.Columns.Count).End(Excel.XlDirection.xlToLeft).column
    '    Dim MaxEntries As Integer = oSheet.Cells(oSheet.Rows.Count, 1).End(Excel.XlDirection.xlUp).row

    '    Dim checkHeaders(MaxColumn) As String
    '    For cnt As Integer = 0 To MaxColumn - 1
    '        checkHeaders(cnt) = oSheet.Cells(1, cnt + 1).value
    '    Next : checkHeaders(MaxColumn) = oWB.Worksheets(1).name

    '    Me.Enabled = False
    '    Dim mysql As String = "SELECT * FROM tblkarat"
    '    Dim ds As DataSet = LoadSQL(mysql, "tblkarat")

    '    For cnt = 2 To MaxEntries

    '        Dim dsnewrow As DataRow
    '        dsnewrow = ds.Tables(0).NewRow
    '        With dsnewrow
    '            .Item("karat") = oSheet.Cells(cnt, 1).value
    '            .Item("category") = oSheet.Cells(cnt, 2).value
    '            '.Item("KARATID") = oSheet.Cells(cnt, 3).value
    '        End With
    '        ds.Tables(0).Rows.Add(dsnewrow)
    '        database.SaveEntry(ds)
    '    Next


    '    oSheet = Nothing
    '    oWB = Nothing
    '    oXL.Quit()
    '    oXL = Nothing

    '    MsgBox("Completed")
    'End Sub

    
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim oXL As New Excel.Application
        Dim oWB As Excel.Workbook
        Dim oSheet As Excel.Worksheet
        oWB = oXL.Workbooks.Open("C:\Users\MISGWAPOHON\Desktop\2016\Copy of tmplateClass.xlsx")
        oSheet = oWB.Worksheets(1)

        Dim MaxColumn As Integer = oSheet.Cells(1, oSheet.Columns.Count).End(Excel.XlDirection.xlToLeft).column
        Dim MaxEntries As Integer = oSheet.Cells(oSheet.Rows.Count, 1).End(Excel.XlDirection.xlUp).row

        Dim checkHeaders(MaxColumn) As String
        For cnt As Integer = 0 To MaxColumn - 1
            checkHeaders(cnt) = oSheet.Cells(1, cnt + 1).value
        Next : checkHeaders(MaxColumn) = oWB.Worksheets(1).name

        Me.Enabled = False
        Dim mysql As String = "SELECT * FROM tblclass"
        Dim ds As DataSet = LoadSQL(mysql, "tblclass")

        For cnt = 2 To MaxEntries

            Dim dsnewrow As DataRow
            dsnewrow = ds.Tables(0).NewRow
            With dsnewrow
                .Item("Class") = oSheet.Cells(cnt, 1).value
                .Item("Price") = oSheet.Cells(cnt, 2).value
                .Item("KARATID") = oSheet.Cells(cnt, 3).value
            End With
            ds.Tables(0).Rows.Add(dsnewrow)
            database.SaveEntry(ds)
        Next


        oSheet = Nothing
        oWB = Nothing
        oXL.Quit()
        oXL = Nothing

        MsgBox("Completed")
    End Sub
End Class
