Imports MySql.Data.MySqlClient
Imports Microsoft.Reporting.WinForms
Public Class ViewNotaKarcis

    Public Ambil_Data As String
    Public Form_Ambil_Data As String

    Private Sub ViewNotaKarcis_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim noDaftarParam As New ReportParameter("noDaftar", Home.txtKarReg.Text)
        Dim tglDaftarParam As New ReportParameter("tglDaftar", Home.txtKarTglReg.Text)
        Dim noRmParam As New ReportParameter("noRM", Home.txtKarRM.Text)
        Dim namaParam As New ReportParameter("nama", Home.txtKarNama.Text)
        Dim ttlParam As New ReportParameter("ttl", Home.txtKarTtl.Text)
        Dim jkParam As New ReportParameter("jk", Home.txtKarJk.Text)
        Dim alamatParam As New ReportParameter("alamat", Home.txtKarAlamat.Text)
        Dim caraBayarParam As New ReportParameter("caraBayar", Home.txtCaraBayar.Text)
        Dim unitParam As New ReportParameter("unit", Home.txtKarPoli.Text)
        Dim karcisParam As New ReportParameter("karcis", Home.txtKarBiaya.Text)
        Dim konsulParam As New ReportParameter("konsul", Home.txtKarKonsul.Text)
        Dim totalParam As New ReportParameter("total", Home.txtKarTotal.Text)
        Dim userParam As New ReportParameter("user", Home.txtUser.Text)

        ReportViewer1.LocalReport.SetParameters(noDaftarParam)
        ReportViewer1.LocalReport.SetParameters(tglDaftarParam)
        ReportViewer1.LocalReport.SetParameters(noRmParam)
        ReportViewer1.LocalReport.SetParameters(namaParam)
        ReportViewer1.LocalReport.SetParameters(ttlParam)
        ReportViewer1.LocalReport.SetParameters(jkParam)
        ReportViewer1.LocalReport.SetParameters(alamatParam)
        ReportViewer1.LocalReport.SetParameters(caraBayarParam)
        ReportViewer1.LocalReport.SetParameters(unitParam)
        ReportViewer1.LocalReport.SetParameters(karcisParam)
        ReportViewer1.LocalReport.SetParameters(konsulParam)
        ReportViewer1.LocalReport.SetParameters(totalParam)
        ReportViewer1.LocalReport.SetParameters(userParam)
        ReportViewer1.LocalReport.Refresh()

        Me.ReportViewer1.SetDisplayMode(DisplayMode.PrintLayout)
    End Sub
End Class