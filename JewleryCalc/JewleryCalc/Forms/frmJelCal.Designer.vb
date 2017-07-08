<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmJelCal
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmJelCal))
        Me.btnBrowse = New System.Windows.Forms.Button()
        Me.lblPath = New System.Windows.Forms.Label()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.btnGenerate = New System.Windows.Forms.Button()
        Me.btnAddClass = New System.Windows.Forms.Button()
        Me.ofdJeltmp = New System.Windows.Forms.OpenFileDialog()
        Me.sfdPath = New System.Windows.Forms.SaveFileDialog()
        Me.pbstatus = New System.Windows.Forms.ProgressBar()
        Me.lblstatus = New System.Windows.Forms.Label()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnBrowse
        '
        Me.btnBrowse.Location = New System.Drawing.Point(6, 40)
        Me.btnBrowse.Name = "btnBrowse"
        Me.btnBrowse.Size = New System.Drawing.Size(75, 28)
        Me.btnBrowse.TabIndex = 0
        Me.btnBrowse.Text = "&Browse"
        Me.btnBrowse.UseVisualStyleBackColor = True
        '
        'lblPath
        '
        Me.lblPath.AutoSize = True
        Me.lblPath.Location = New System.Drawing.Point(10, 17)
        Me.lblPath.Name = "lblPath"
        Me.lblPath.Size = New System.Drawing.Size(59, 14)
        Me.lblPath.TabIndex = 1
        Me.lblPath.Text = "File not yet"
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.btnGenerate)
        Me.GroupBox1.Controls.Add(Me.lblPath)
        Me.GroupBox1.Controls.Add(Me.btnBrowse)
        Me.GroupBox1.Location = New System.Drawing.Point(12, 12)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(539, 78)
        Me.GroupBox1.TabIndex = 2
        Me.GroupBox1.TabStop = False
        '
        'btnGenerate
        '
        Me.btnGenerate.Location = New System.Drawing.Point(87, 40)
        Me.btnGenerate.Name = "btnGenerate"
        Me.btnGenerate.Size = New System.Drawing.Size(75, 28)
        Me.btnGenerate.TabIndex = 2
        Me.btnGenerate.Text = "&Generate"
        Me.btnGenerate.UseVisualStyleBackColor = True
        '
        'btnAddClass
        '
        Me.btnAddClass.Location = New System.Drawing.Point(18, 166)
        Me.btnAddClass.Name = "btnAddClass"
        Me.btnAddClass.Size = New System.Drawing.Size(532, 47)
        Me.btnAddClass.TabIndex = 3
        Me.btnAddClass.Text = "&Add Class"
        Me.btnAddClass.UseVisualStyleBackColor = True
        '
        'ofdJeltmp
        '
        Me.ofdJeltmp.Filter = "Excel 2003|*.xls|Excel 2007|*.xlsx"
        '
        'sfdPath
        '
        Me.sfdPath.DefaultExt = "xls"
        Me.sfdPath.Filter = "Excel File 2003|*.xls"
        '
        'pbstatus
        '
        Me.pbstatus.Location = New System.Drawing.Point(12, 113)
        Me.pbstatus.Name = "pbstatus"
        Me.pbstatus.Size = New System.Drawing.Size(538, 15)
        Me.pbstatus.TabIndex = 4
        '
        'lblstatus
        '
        Me.lblstatus.AutoSize = True
        Me.lblstatus.Location = New System.Drawing.Point(13, 94)
        Me.lblstatus.Name = "lblstatus"
        Me.lblstatus.Size = New System.Drawing.Size(38, 14)
        Me.lblstatus.TabIndex = 5
        Me.lblstatus.Text = "0.00%"
        '
        'frmJelCal
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 14.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(562, 133)
        Me.Controls.Add(Me.lblstatus)
        Me.Controls.Add(Me.pbstatus)
        Me.Controls.Add(Me.btnAddClass)
        Me.Controls.Add(Me.GroupBox1)
        Me.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmJelCal"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Jewelry Calculator"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnBrowse As System.Windows.Forms.Button
    Friend WithEvents lblPath As System.Windows.Forms.Label
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents btnGenerate As System.Windows.Forms.Button
    Friend WithEvents btnAddClass As System.Windows.Forms.Button
    Friend WithEvents ofdJeltmp As System.Windows.Forms.OpenFileDialog
    Friend WithEvents sfdPath As System.Windows.Forms.SaveFileDialog
    Friend WithEvents pbstatus As System.Windows.Forms.ProgressBar
    Friend WithEvents lblstatus As System.Windows.Forms.Label

End Class
