Imports MySql.Data.MySqlClient
Imports Microsoft.Reporting.WinForms
Public Class ViewNotaAllObatOK

    Public Ambil_Data As String
    Public Form_Ambil_Data As String

    Private Sub ViewNotaAllObatOK_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim noDaftarParam As New ReportParameter("noDaftar", Home.txtNoReg.Text)
        Dim noRmParam As New ReportParameter("noRm", Home.txtRekMed.Text)
        Dim namaParam As New ReportParameter("nama", Home.txtNamaPasien.Text)
        Dim ttlParam As New ReportParameter("ttl", Home.txtUmur.Text)
        Dim alamatParam As New ReportParameter("alamat", Home.txtAlamat.Text)
        Dim tglDaftarParam As New ReportParameter("tglDaftar", Home.txtTglMasuk.Text)
        Dim caraBayarParam As New ReportParameter("caraBayar", Home.txtCaraBayar.Text)
        Dim rawatInapParam As New ReportParameter("rawatInap", Home.txtUnit.Text)
        Dim kelasParam As New ReportParameter("kelas", Home.txtKelas.Text)
        Dim user As New ReportParameter("user", Home.txtUser.Text)

        ReportViewer1.LocalReport.SetParameters(noDaftarParam)
        ReportViewer1.LocalReport.SetParameters(noRmParam)
        ReportViewer1.LocalReport.SetParameters(namaParam)
        ReportViewer1.LocalReport.SetParameters(ttlParam)
        ReportViewer1.LocalReport.SetParameters(alamatParam)
        ReportViewer1.LocalReport.SetParameters(tglDaftarParam)
        ReportViewer1.LocalReport.SetParameters(caraBayarParam)
        ReportViewer1.LocalReport.SetParameters(rawatInapParam)
        ReportViewer1.LocalReport.SetParameters(kelasParam)
        ReportViewer1.LocalReport.SetParameters(user)

        If Ambil_Data = True Then
            Select Case Form_Ambil_Data
                Case "OkAll"
                    Dim dt As New DataTable
                    da = New MySqlDataAdapter("SELECT
	                                                noPenjualanObatOK,
	                                                namaObat,
	                                                JumlahItem,
	                                                harga,
	                                                subTotalPenjualan 
                                                FROM
	                                                vw_detailpenjualanobatok
                                               WHERE 
                                                    noDaftar = '" & Home.txtNoReg.Text & "'", conn)
                    ds = New DataSet
                    da.Fill(dt)
                    ReportViewer1.LocalReport.DataSources.Clear()
                    Dim rpt As New ReportDataSource("CetakNotaObatOK", dt)
                    ReportViewer1.LocalReport.DataSources.Add(rpt)
                Case "OkPernota"
                    Dim dt As New DataTable
                    da = New MySqlDataAdapter("SELECT
	                                                noPenjualanObatOK,
	                                                namaObat,
	                                                JumlahItem,
	                                                harga,
	                                                subTotalPenjualan 
                                                FROM
	                                                vw_detailpenjualanobatok
                                                WHERE
	                                                noPenjualanObatOK = '" & Home.txtNoJualOk.Text & "'", conn)
                    ds = New DataSet
                    da.Fill(dt)
                    ReportViewer1.LocalReport.DataSources.Clear()
                    Dim rpt As New ReportDataSource("CetakNotaObatOK", dt)
                    ReportViewer1.LocalReport.DataSources.Add(rpt)
            End Select
        End If

        Me.ReportViewer1.SetDisplayMode(DisplayMode.PrintLayout)
    End Sub
End Class