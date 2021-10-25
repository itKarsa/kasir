Imports MySql.Data.MySqlClient
Imports Microsoft.Reporting.WinForms
Public Class ViewNotaVA

    Public Ambil_Data As String
    Public Form_Ambil_Data As String

    Private Sub ViewNotaVA_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim namaParam As New ReportParameter("nama", namaVA)
        Dim expdateParam As New ReportParameter("expDate", expDateVA)
        Dim totalParam As New ReportParameter("totalTagihan", totTagih)
        Dim noVAParam As New ReportParameter("noVA", noVA)
        Dim noTagihParam As New ReportParameter("noTagihan", noTagihan)

        ReportViewer1.LocalReport.SetParameters(namaParam)
        ReportViewer1.LocalReport.SetParameters(expdateParam)
        ReportViewer1.LocalReport.SetParameters(noVAParam)
        ReportViewer1.LocalReport.SetParameters(noTagihParam)
        ReportViewer1.LocalReport.SetParameters(totalParam)
        ReportViewer1.LocalReport.Refresh()

        Me.ReportViewer1.SetDisplayMode(DisplayMode.PrintLayout)
    End Sub
End Class