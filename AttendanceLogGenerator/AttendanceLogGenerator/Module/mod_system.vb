Imports Microsoft.Office.Interop
Module mod_system


    ''' <summary>
    ''' Extract Data from the database
    ''' </summary>
    ''' <param name="headers">Array of HEADERS</param>
    ''' <param name="mySql">SQL Statement</param>
    ''' <param name="dest">Excel File Destination</param>
    ''' <remarks></remarks>
    Friend Sub ExtractToExcel(headers As String(), mySql As String, dest As String)
        If dest = "" Then Exit Sub

        Dim ds As DataSet = LoadSQL(mySql)

        'Load Excel
        Dim oXL As New Excel.Application
        If oXL Is Nothing Then
            MessageBox.Show("Excel is not properly installed!!")
            Return
        End If

        Dim oWB As Excel.Workbook
        Dim oSheet As Excel.Worksheet

        oXL = CreateObject("Excel.Application")
        oXL.Visible = False

        oWB = oXL.Workbooks.Add
        oSheet = oWB.ActiveSheet
        oSheet.Name = "Attendance Log"

        ' ADD BRANCHCODE
        InsertArrayElement(headers, 0, "BRANCHCODE")

        ' HEADERS
        Dim cnt As Integer = 0
        For Each hr In headers
            cnt += 1 : oSheet.Cells(1, cnt).value = hr
        Next

        ' EXTRACTING
        Console.Write("Extracting")
        Dim rowCnt As Integer = 2
        For Each dr As DataRow In ds.Tables(0).Rows
            For colCnt As Integer = 0 To headers.Count - 1
                If colCnt = 0 Then
                    oSheet.Cells(rowCnt, colCnt + 1).value = BranchCode
                Else
                    oSheet.Cells(rowCnt, colCnt + 1).value = dr(colCnt - 1) 'dr(colCnt - 1) move the column by -1
                End If
            Next
            rowCnt += 1

            Console.Write(".")
            Application.DoEvents()
        Next

        oWB.SaveAs(dest)
        oSheet = Nothing
        oWB.Close(False)
        oWB = Nothing
        oXL.Quit()
        oXL = Nothing

        Console.WriteLine("Data Extracted")
    End Sub

    Private Sub InsertArrayElement(Of T)( _
          ByRef sourceArray() As T, _
          ByVal insertIndex As Integer, _
          ByVal newValue As T)

        Dim newPosition As Integer
        Dim counter As Integer

        newPosition = insertIndex
        If (newPosition < 0) Then newPosition = 0
        If (newPosition > sourceArray.Length) Then _
           newPosition = sourceArray.Length

        Array.Resize(sourceArray, sourceArray.Length + 1)

        For counter = sourceArray.Length - 2 To newPosition Step -1
            sourceArray(counter + 1) = sourceArray(counter)
        Next counter

        sourceArray(newPosition) = newValue
    End Sub

    ' HASHTABLE FUNCTIONS
    Public Function GetIDbyName(name As String, ht As Hashtable) As Integer
        For Each dt As DictionaryEntry In ht
            If dt.Value = name Then
                Return dt.Key
            End If
        Next

        Return 0
    End Function

    Public Function GetNameByID(id As Integer, ht As Hashtable) As String
        For Each dt As DictionaryEntry In ht
            If dt.Key = id Then
                Return dt.Value
            End If
        Next

        Return "ES" & "KIE GWA" & "PO"
    End Function
    ' END - HASHTABLE FUNCTIONS

    Public Function CheckOTP() As Boolean
        diagOTP.ShowDialog()
        diagOTP.TopMost = True
        'Return False
        Return True
    End Function


    Public Function CheckFormActive() As Boolean

        frmCollection = Application.OpenForms()
        If Application.OpenForms().OfType(Of frmInsurance).Any Then
            MsgBox("Please close the " & Application.OpenForms.Item("frmInsurance").Text & " form", MsgBoxStyle.OkOnly) : Return True
        ElseIf Application.OpenForms().OfType(Of frmPawningItemNew).Any Then
            MsgBox("Please close the " & Application.OpenForms.Item("frmPawningItemNew").Text & " form", MsgBoxStyle.OkOnly) : Return True
        ElseIf Application.OpenForms().OfType(Of frmBorrowing).Any Then
            MsgBox("Please close the " & Application.OpenForms.Item("frmBorrowing").Text & " form", MsgBoxStyle.OkOnly) : Return True
        ElseIf Application.OpenForms().OfType(Of frmMoneyTransfer).Any Then
            MsgBox("Please close the " & Application.OpenForms.Item("frmMoneyTransfer").Text & " form", MsgBoxStyle.OkOnly) : Return True
        ElseIf Application.OpenForms().OfType(Of frmSales).Any Then
            MsgBox("Please close the " & Application.OpenForms.Item("frmSales").Text & " form", MsgBoxStyle.OkOnly) : Return True
        End If

        Return False
    End Function

    Friend Function DoForfeitingItem() As Boolean
        Dim mysql As String = "Select * From tblLayAway Where Status = '1' And ForfeitDate < '" & CurrentDate.ToShortDateString & "' And Balance > 0"
        Dim fillData As String = "tblLayAway"
        Dim ds As DataSet = LoadSQL(mysql, fillData)
        If ds.Tables(0).Rows.Count = 0 Then Return True

        For Each dr In ds.Tables(0).Rows()
            Dim lay As New LayAway
            With lay
                .LoadByID(dr.item("LayID"))
                .ForfeitLayAway()
                .ItemOnLayMode(dr.item("ItemCode"), False)
            End With

        Next

        Return True
    End Function

    ''' <summary>
    ''' This method will separate the phone number.
    ''' </summary>
    ''' <param name="PhoneField"></param>
    ''' <param name="e"></param>
    ''' <param name="isPhone"></param>
    ''' <remarks></remarks>
    Friend Sub PhoneSeparator(ByVal PhoneField As TextBox, ByVal e As KeyPressEventArgs, Optional ByVal isPhone As Boolean = False)
        Dim charPos() As Integer = {}
        If PhoneField.Text = Nothing Then Return

        Select Case PhoneField.Text.Substring(0, 1)
            Case "0"
                charPos = {4, 8}
            Case "9"
                charPos = {3, 7} '922-797-7559
            Case "+"
                charPos = {3, 7, 11} '+63-919-797-7559
            Case "6"
                charPos = {2, 6, 10} '63-919-797-7559
        End Select
        If isPhone Then
            Select Case PhoneField.Text.Substring(0, 1)
                Case "0"
                    charPos = {3, 7}
                Case Else
                    charPos = {2, 6}
            End Select
        End If

        For Each pos In charPos
            If PhoneField.TextLength = pos And Not e.KeyChar = vbBack Then
                PhoneField.Text &= "-"
                PhoneField.SelectionStart = pos + 1
            End If
        Next
    End Sub

#Region "Log Module"
    Const LOG_FILE As String = "syslog.txt"
    Private Sub CreateLog()
        Dim fsEsk As New System.IO.FileStream(LOG_FILE, IO.FileMode.CreateNew)
        fsEsk.Close()
    End Sub

    Friend Sub Log_Report(ByVal str As String)
        If Not System.IO.File.Exists(LOG_FILE) Then CreateLog()

        Dim recorded_log As String = _
            String.Format("[{0}] ", Now.ToString("MM/dd/yyyy HH:mm:ss")) & str

        Dim fs As New System.IO.FileStream(LOG_FILE, IO.FileMode.Append, IO.FileAccess.Write)
        Dim fw As New System.IO.StreamWriter(fs)
        fw.WriteLine(recorded_log)
        fw.Close()
        fs.Close()
        Console.WriteLine("Recorded")
    End Sub
#End Region
End Module
