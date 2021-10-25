Imports MySql.Data.MySqlClient
Imports Microsoft.Reporting.WinForms
Public Class ViewNotaTotal

    Private Sub ViewNotaTotal_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim noDaftarParam As New ReportParameter("noDaftar", TotalPembayaran.txtNoDaftar.Text)
        Dim noRmParam As New ReportParameter("noRm", TotalPembayaran.txtNoRM.Text)
        Dim namaParam As New ReportParameter("nama", TotalPembayaran.txtNama.Text)
        Dim ttlParam As New ReportParameter("ttl", TotalPembayaran.txtTTL.Text & ", " & TotalPembayaran.txtTTL2.Text)
        Dim alamatParam As New ReportParameter("alamat", TotalPembayaran.txtAlamat.Text)
        Dim tglDaftarParam As New ReportParameter("tglDaftar", TotalPembayaran.txtTglDaftar.Text)
        Dim caraBayarParam As New ReportParameter("caraBayar", TotalPembayaran.txtCaraBayar.Text)
        Dim rawatInapParam As New ReportParameter("rawatInap", TotalPembayaran.txtRanap.Text)
        Dim kelasParam As New ReportParameter("kelas", TotalPembayaran.txtKelas.Text)
        Dim lamaInapParam As New ReportParameter("lamaInap", TotalPembayaran.txtTotInap.Text)
        Dim totBiayaRuang As New ReportParameter("totalBiayaRuang", TotalPembayaran.txtTotalBiayaRuang.Text)
        Dim totTindakanParam As New ReportParameter("totalTindakan", TotalPembayaran.txtTotTindak2.Text)
        Dim totPenunjangParam As New ReportParameter("totalPenunjang", TotalPembayaran.txtTotPenunjang2.Text)
        Dim totPembayaranParam As New ReportParameter("totalPembayaran", TotalPembayaran.txtTotal2.Text)

        ReportViewer1.LocalReport.SetParameters(noDaftarParam)
        ReportViewer1.LocalReport.SetParameters(noRmParam)
        ReportViewer1.LocalReport.SetParameters(namaParam)
        ReportViewer1.LocalReport.SetParameters(ttlParam)
        ReportViewer1.LocalReport.SetParameters(alamatParam)
        ReportViewer1.LocalReport.SetParameters(tglDaftarParam)
        ReportViewer1.LocalReport.SetParameters(caraBayarParam)
        ReportViewer1.LocalReport.SetParameters(rawatInapParam)
        ReportViewer1.LocalReport.SetParameters(kelasParam)
        ReportViewer1.LocalReport.SetParameters(lamaInapParam)
        ReportViewer1.LocalReport.SetParameters(totBiayaRuang)
        ReportViewer1.LocalReport.SetParameters(totTindakanParam)
        ReportViewer1.LocalReport.SetParameters(totPenunjangParam)
        ReportViewer1.LocalReport.SetParameters(totPembayaranParam)

        Me.ReportViewer1.SetDisplayMode(DisplayMode.PrintLayout)
    End Sub
End Class