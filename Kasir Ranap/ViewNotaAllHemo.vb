Imports MySql.Data.MySqlClient
Imports Microsoft.Reporting.WinForms
Public Class ViewNotaAllHemo

    Public Ambil_Data As String
    Public Form_Ambil_Data As String

    Private Sub ViewNotaAllHemo_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim noDaftarParam As New ReportParameter("noDaftar", Home.txtNoReg.Text)
        Dim noRmParam As New ReportParameter("noRm", Home.txtRekMed.Text)
        Dim namaParam As New ReportParameter("nama", Home.txtNamaPasien.Text)
        Dim ttlParam As New ReportParameter("ttl", Home.txtUmur.Text)
        Dim alamatParam As New ReportParameter("alamat", Home.txtAlamat.Text)
        Dim tglDaftarParam As New ReportParameter("tglDaftar", Home.txtTglMasuk.Text)
        Dim caraBayarParam As New ReportParameter("caraBayar", Home.txtCaraBayar.Text)
        Dim user As New ReportParameter("user", Home.txtUser.Text)

        ReportViewer1.LocalReport.SetParameters(noDaftarParam)
        ReportViewer1.LocalReport.SetParameters(noRmParam)
        ReportViewer1.LocalReport.SetParameters(namaParam)
        ReportViewer1.LocalReport.SetParameters(ttlParam)
        ReportViewer1.LocalReport.SetParameters(alamatParam)
        ReportViewer1.LocalReport.SetParameters(tglDaftarParam)
        ReportViewer1.LocalReport.SetParameters(caraBayarParam)
        ReportViewer1.LocalReport.SetParameters(user)

        If Ambil_Data = True Then
            Select Case Form_Ambil_Data
                Case "HemoAll"
                    Dim dt As New DataTable
                    da = New MySqlDataAdapter("SELECT
	                                                noTindakanHD,
	                                                tindakan,
	                                                jumlahTindakan,
	                                                tarifTindakan,
	                                                subtotal 
                                                FROM
	                                                vw_pasienhddetail
                                               WHERE 
                                                    noRegistrasi = '" & Home.txtNoReg.Text & "'", conn)
                    ds = New DataSet
                    da.Fill(dt)
                    ReportViewer1.LocalReport.DataSources.Clear()
                    Dim rpt As New ReportDataSource("CetakNotaHemodialisa", dt)
                    ReportViewer1.LocalReport.DataSources.Add(rpt)
                Case "HemoPernota"
                    Dim dt As New DataTable
                    da = New MySqlDataAdapter("SELECT
	                                                noTindakanHD,
	                                                tindakan,
	                                                jumlahTindakan,
	                                                tarifTindakan,
	                                                subtotal 
                                                FROM
	                                                vw_pasienhddetail
                                                WHERE
	                                                noTindakanHD = '" & Home.txtNoTindakan.Text & "'", conn)
                    ds = New DataSet
                    da.Fill(dt)
                    ReportViewer1.LocalReport.DataSources.Clear()
                    Dim rpt As New ReportDataSource("CetakNotaHemodialisa", dt)
                    ReportViewer1.LocalReport.DataSources.Add(rpt)
            End Select
        End If

        Me.ReportViewer1.SetDisplayMode(DisplayMode.PrintLayout)
    End Sub
End Class