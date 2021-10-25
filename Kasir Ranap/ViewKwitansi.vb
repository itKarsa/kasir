Imports MySql.Data.MySqlClient
Imports Microsoft.Reporting.WinForms
Public Class ViewKwitansi
    Private Sub ViewKwitansi_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim noParam As New ReportParameter("noKwitansi", Home.txtNoKwintasi.Text)
        Dim noRMParam As New ReportParameter("noRM", Home.txtRekMed.Text)
        Dim namaParam As New ReportParameter("namaPasien", Home.txtNamaPasien.Text)
        Dim jkParam As New ReportParameter("jk", Home.txtJk.Text)
        Dim terbilangParam As New ReportParameter("angkaTerbilang", Home.txtTerbilangKwitansi.Text)
        Dim nominalParam As New ReportParameter("angkaNominal", Home.txtNominalKwitansi.Text)
        Dim ruangParam As New ReportParameter("ruang", Home.txtUnit.Text)
        Dim tglMasukParam As New ReportParameter("tglMasuk", Home.txtTglMasuk.Text)
        Dim tglKeluarParam As New ReportParameter("tglKeluar", Home.txtTglKeluar.Text)
        Dim user As New ReportParameter("user", Home.txtUser.Text)

        ReportViewer1.LocalReport.SetParameters(noParam)
        ReportViewer1.LocalReport.SetParameters(noRMParam)
        ReportViewer1.LocalReport.SetParameters(namaParam)
        ReportViewer1.LocalReport.SetParameters(jkParam)
        ReportViewer1.LocalReport.SetParameters(terbilangParam)
        ReportViewer1.LocalReport.SetParameters(nominalParam)
        ReportViewer1.LocalReport.SetParameters(user)
        ReportViewer1.LocalReport.SetParameters(ruangParam)
        ReportViewer1.LocalReport.SetParameters(tglMasukParam)
        ReportViewer1.LocalReport.SetParameters(tglKeluarParam)
        ReportViewer1.LocalReport.Refresh()

        Me.ReportViewer1.SetDisplayMode(DisplayMode.PrintLayout)
    End Sub
End Class