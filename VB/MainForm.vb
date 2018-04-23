﻿Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Linq
Imports System.Text
Imports System.Windows.Forms
Imports DevExpress.XtraGrid

Namespace FormatCondititonsDesigner
    Partial Public Class MainForm
        Inherits Form
        Private formatDesigner As ViewFormatDesigner
        Public Sub New()
            InitializeComponent()
        End Sub

        Private Sub MainForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
            formatDesigner = New ViewFormatDesigner(pivotGridControl1)
            ' This line of code is generated by Data Source Configuration Wizard
            CountriesTableAdapter1.Fill(CountriesDBDataSet1.Countries)
        End Sub
    End Class
End Namespace
