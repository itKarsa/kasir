Imports MySql.Data.MySqlClient
Imports Microsoft.Reporting.WinForms
Public Class ViewNotaAllTotal

    Public Ambil_Data As String
    Public Form_Ambil_Data As String

    Function tampilAkomodasiRuang() As DataTable
        Call koneksiServer()
        Dim query As String = ""
        Dim cmd As MySqlCommand
        Dim dr As MySqlDataReader
        Dim dt As New DataTable

        query = "SELECT * 
                   FROM vw_daftarruangakomodasi 
                  WHERE noDaftar = '" & Home.txtNoReg.Text & "'
                  ORDER BY CAST(SUBSTR(noDaftarRawatInap,15) AS UNSIGNED) DESC"
        cmd = New MySqlCommand(query, conn)
        dr = cmd.ExecuteReader
        dt.Load(dr)
        ReportViewer1.LocalReport.DataSources.Clear()
        Return dt
    End Function

    Function tampilDetail() As DataTable
        Call koneksiServer()
        Dim query As String = ""
        Dim cmd As MySqlCommand
        Dim dr As MySqlDataReader
        Dim dt As New DataTable

        query = "SELECT tri.noTindakanPasienRanap,
	                    tri.noDaftarRawatInap,
	                    tri.tglTindakan,
	                    tri.totalTarifTindakan,
	                    tri.statusPembayaran,
	                    tri.tglPembayaran,
	                    dtri.kdTarif,
	                    dtri.tindakan,
	                    dtri.tarif,
	                    dtri.jumlahTindakan,
	                    dtri.totalTarif,
	                    DPJP.namapetugasMedis AS DPJP,
	                    PPA.namapetugasMedis AS PPA,
	                    reg.noDaftar,
	                    ri.rawatInap
                   FROM
	                    t_tindakanpasienranap AS tri,
	                    t_detailtindakanpasienranap AS dtri,
	                    t_registrasirawatinap AS ri,
	                    t_registrasi AS reg,
	                    t_tenagamedis2 AS DPJP,
	                    t_tenagamedis2 AS PPA
                  WHERE
	                    reg.noDaftar = ri.noDaftar AND
	                    tri.noDaftarRawatInap = ri.noDaftarRawatInap AND
	                    tri.noTindakanPasienRanap = dtri.noTindakanPasienRanap AND
	                    reg.kdTenagaMedis = DPJP.kdPetugasMedis AND
	                    dtri.kdTenagaMedis = PPA.kdPetugasMedis AND
	                    reg.noDaftar = '" & Home.txtNoReg.Text & "'
                        AND dtri.tindakan NOT LIKE 'JASA%'
	                    AND dtri.tindakan NOT LIKE 'OKSIGEN%'
	                    AND dtri.kdTarif NOT IN (020910,020920,020930,020940,020950,020960,020970)
               ORDER BY tri.tglTindakan ASC, dtri.tindakan ASC"
        cmd = New MySqlCommand(query, conn)
        dr = cmd.ExecuteReader
        dt.Load(dr)
        ReportViewer1.LocalReport.DataSources.Clear()
        Return dt
    End Function

    Function tampilJasa() As DataTable
        Call koneksiServer()
        Dim query As String = ""
        Dim cmd As MySqlCommand
        Dim dr As MySqlDataReader
        Dim dt As New DataTable

        query = "SELECT tri.noTindakanPasienRanap,
	                    tri.noDaftarRawatInap,
	                    tri.tglTindakan,
	                    tri.totalTarifTindakan,
	                    tri.statusPembayaran,
	                    tri.tglPembayaran,
	                    dtri.kdTarif,
	                    dtri.tindakan,
	                    dtri.tarif,
	                    dtri.jumlahTindakan,
	                    dtri.totalTarif,
	                    DPJP.namapetugasMedis AS DPJP,
	                    PPA.namapetugasMedis AS PPA,
	                    reg.noDaftar,
	                    ri.rawatInap
                   FROM
	                    t_tindakanpasienranap AS tri,
	                    t_detailtindakanpasienranap AS dtri,
	                    t_registrasirawatinap AS ri,
	                    t_registrasi AS reg,
	                    t_tenagamedis2 AS DPJP,
	                    t_tenagamedis2 AS PPA
                  WHERE
	                    reg.noDaftar = ri.noDaftar AND
	                    tri.noDaftarRawatInap = ri.noDaftarRawatInap AND
	                    tri.noTindakanPasienRanap = dtri.noTindakanPasienRanap AND
	                    reg.kdTenagaMedis = DPJP.kdPetugasMedis AND
	                    dtri.kdTenagaMedis = PPA.kdPetugasMedis AND
	                    reg.noDaftar = '" & Home.txtNoReg.Text & "'
                        AND dtri.tindakan LIKE 'JASA%'
               ORDER BY tglTindakan ASC, tindakan ASC"
        cmd = New MySqlCommand(query, conn)
        dr = cmd.ExecuteReader
        dt.Load(dr)
        ReportViewer1.LocalReport.DataSources.Clear()
        Return dt
    End Function

    Function tampilVisite() As DataTable
        Call koneksiServer()
        Dim query As String = ""
        Dim cmd As MySqlCommand
        Dim dr As MySqlDataReader
        Dim dt As New DataTable

        query = "SELECT tri.noTindakanPasienRanap,
	                    tri.noDaftarRawatInap,
	                    tri.tglTindakan,
	                    tri.totalTarifTindakan,
	                    tri.statusPembayaran,
	                    tri.tglPembayaran,
	                    dtri.kdTarif,
	                    dtri.tindakan,
	                    dtri.tarif,
	                    dtri.jumlahTindakan,
	                    dtri.totalTarif,
	                    DPJP.namapetugasMedis AS DPJP,
	                    PPA.namapetugasMedis AS PPA,
	                    reg.noDaftar,
	                    ri.rawatInap
                   FROM
	                    t_tindakanpasienranap AS tri,
	                    t_detailtindakanpasienranap AS dtri,
	                    t_registrasirawatinap AS ri,
	                    t_registrasi AS reg,
	                    t_tenagamedis2 AS DPJP,
	                    t_tenagamedis2 AS PPA
                  WHERE
	                    reg.noDaftar = ri.noDaftar AND
	                    tri.noDaftarRawatInap = ri.noDaftarRawatInap AND
	                    tri.noTindakanPasienRanap = dtri.noTindakanPasienRanap AND
	                    reg.kdTenagaMedis = DPJP.kdPetugasMedis AND
	                    dtri.kdTenagaMedis = PPA.kdPetugasMedis AND
	                    reg.noDaftar = '" & Home.txtNoReg.Text & "'
                        AND (dtri.tindakan LIKE 'JASA VISITE%' OR dtri.tindakan LIKE 'JASA KONSULTASI%')
               ORDER BY tglTindakan ASC, tindakan ASC"
        cmd = New MySqlCommand(query, conn)
        dr = cmd.ExecuteReader
        dt.Load(dr)
        ReportViewer1.LocalReport.DataSources.Clear()
        Return dt
    End Function

    Function tampilAskep() As DataTable
        Call koneksiServer()
        Dim query As String = ""
        Dim cmd As MySqlCommand
        Dim dr As MySqlDataReader
        Dim dt As New DataTable

        query = "SELECT tri.noTindakanPasienRanap,
	                    tri.noDaftarRawatInap,
	                    tri.tglTindakan,
	                    tri.totalTarifTindakan,
	                    tri.statusPembayaran,
	                    tri.tglPembayaran,
	                    dtri.kdTarif,
	                    dtri.tindakan,
	                    dtri.tarif,
	                    dtri.jumlahTindakan,
	                    dtri.totalTarif,
	                    DPJP.namapetugasMedis AS DPJP,
	                    PPA.namapetugasMedis AS PPA,
	                    reg.noDaftar,
	                    ri.rawatInap
                   FROM
	                    t_tindakanpasienranap AS tri,
	                    t_detailtindakanpasienranap AS dtri,
	                    t_registrasirawatinap AS ri,
	                    t_registrasi AS reg,
	                    t_tenagamedis2 AS DPJP,
	                    t_tenagamedis2 AS PPA
                  WHERE
	                    reg.noDaftar = ri.noDaftar AND
	                    tri.noDaftarRawatInap = ri.noDaftarRawatInap AND
	                    tri.noTindakanPasienRanap = dtri.noTindakanPasienRanap AND
	                    reg.kdTenagaMedis = DPJP.kdPetugasMedis AND
	                    dtri.kdTenagaMedis = PPA.kdPetugasMedis AND
	                    reg.noDaftar = '" & Home.txtNoReg.Text & "'
                        AND dtri.tindakan LIKE 'JASA ASUHAN KEPERAWATAN%'
               ORDER BY tglTindakan ASC, tindakan ASC"
        cmd = New MySqlCommand(query, conn)
        dr = cmd.ExecuteReader
        dt.Load(dr)
        ReportViewer1.LocalReport.DataSources.Clear()
        Return dt
    End Function

    Function tampilFarklin() As DataTable
        Call koneksiServer()
        Dim query As String = ""
        Dim cmd As MySqlCommand
        Dim dr As MySqlDataReader
        Dim dt As New DataTable

        query = "SELECT tri.noTindakanPasienRanap,
	                    tri.noDaftarRawatInap,
	                    tri.tglTindakan,
	                    tri.totalTarifTindakan,
	                    tri.statusPembayaran,
	                    tri.tglPembayaran,
	                    dtri.kdTarif,
	                    dtri.tindakan,
	                    dtri.tarif,
	                    dtri.jumlahTindakan,
	                    dtri.totalTarif,
	                    DPJP.namapetugasMedis AS DPJP,
	                    PPA.namapetugasMedis AS PPA,
	                    reg.noDaftar,
	                    ri.rawatInap
                   FROM
	                    t_tindakanpasienranap AS tri,
	                    t_detailtindakanpasienranap AS dtri,
	                    t_registrasirawatinap AS ri,
	                    t_registrasi AS reg,
	                    t_tenagamedis2 AS DPJP,
	                    t_tenagamedis2 AS PPA
                  WHERE
	                    reg.noDaftar = ri.noDaftar AND
	                    tri.noDaftarRawatInap = ri.noDaftarRawatInap AND
	                    tri.noTindakanPasienRanap = dtri.noTindakanPasienRanap AND
	                    reg.kdTenagaMedis = DPJP.kdPetugasMedis AND
	                    dtri.kdTenagaMedis = PPA.kdPetugasMedis AND
	                    reg.noDaftar = '" & Home.txtNoReg.Text & "'
                        AND tindakan LIKE 'JASA ASUHAN FARMASI%'
               ORDER BY tglTindakan ASC, tindakan ASC"
        cmd = New MySqlCommand(query, conn)

        dr = cmd.ExecuteReader
        dt.Load(dr)
        ReportViewer1.LocalReport.DataSources.Clear()
        Return dt
    End Function

    Function tampilGizi() As DataTable
        Call koneksiServer()
        Dim query As String = ""
        Dim cmd As MySqlCommand
        Dim dr As MySqlDataReader
        Dim dt As New DataTable

        query = "SELECT tri.noTindakanPasienRanap,
	                    tri.noDaftarRawatInap,
	                    tri.tglTindakan,
	                    tri.totalTarifTindakan,
	                    tri.statusPembayaran,
	                    tri.tglPembayaran,
	                    dtri.kdTarif,
	                    dtri.tindakan,
	                    dtri.tarif,
	                    dtri.jumlahTindakan,
	                    dtri.totalTarif,
	                    DPJP.namapetugasMedis AS DPJP,
	                    PPA.namapetugasMedis AS PPA,
	                    reg.noDaftar,
	                    ri.rawatInap
                   FROM
	                    t_tindakanpasienranap AS tri,
	                    t_detailtindakanpasienranap AS dtri,
	                    t_registrasirawatinap AS ri,
	                    t_registrasi AS reg,
	                    t_tenagamedis2 AS DPJP,
	                    t_tenagamedis2 AS PPA
                  WHERE
	                    reg.noDaftar = ri.noDaftar AND
	                    tri.noDaftarRawatInap = ri.noDaftarRawatInap AND
	                    tri.noTindakanPasienRanap = dtri.noTindakanPasienRanap AND
	                    reg.kdTenagaMedis = DPJP.kdPetugasMedis AND
	                    dtri.kdTenagaMedis = PPA.kdPetugasMedis AND
	                    reg.noDaftar = '" & Home.txtNoReg.Text & "'
                        AND tindakan LIKE 'JASA ASUHAN GIZI%'
               ORDER BY tglTindakan ASC, tindakan ASC"
        cmd = New MySqlCommand(query, conn)
        dr = cmd.ExecuteReader
        dt.Load(dr)
        ReportViewer1.LocalReport.DataSources.Clear()
        Return dt
    End Function

    Function tampilOksigen() As DataTable
        Call koneksiServer()
        Dim query As String = ""
        Dim cmd As MySqlCommand
        Dim dr As MySqlDataReader
        Dim dt As New DataTable

        query = "SELECT tri.noTindakanPasienRanap,
	                    tri.noDaftarRawatInap,
	                    tri.tglTindakan,
	                    tri.totalTarifTindakan,
	                    tri.statusPembayaran,
	                    tri.tglPembayaran,
	                    dtri.kdTarif,
	                    dtri.tindakan,
	                    dtri.tarif,
	                    dtri.jumlahTindakan,
	                    dtri.totalTarif,
	                    DPJP.namapetugasMedis AS DPJP,
	                    PPA.namapetugasMedis AS PPA,
	                    reg.noDaftar,
	                    ri.rawatInap
                   FROM
	                    t_tindakanpasienranap AS tri,
	                    t_detailtindakanpasienranap AS dtri,
	                    t_registrasirawatinap AS ri,
	                    t_registrasi AS reg,
	                    t_tenagamedis2 AS DPJP,
	                    t_tenagamedis2 AS PPA
                  WHERE
	                    reg.noDaftar = ri.noDaftar AND
	                    tri.noDaftarRawatInap = ri.noDaftarRawatInap AND
	                    tri.noTindakanPasienRanap = dtri.noTindakanPasienRanap AND
	                    reg.kdTenagaMedis = DPJP.kdPetugasMedis AND
	                    dtri.kdTenagaMedis = PPA.kdPetugasMedis AND
	                    reg.noDaftar = '" & Home.txtNoReg.Text & "'
                        AND tindakan LIKE 'OKSIGEN%'
               ORDER BY tglTindakan ASC, tindakan ASC"
        cmd = New MySqlCommand(query, conn)
        dr = cmd.ExecuteReader
        dt.Load(dr)
        ReportViewer1.LocalReport.DataSources.Clear()
        Return dt
    End Function

    Function tampilLaboratorium() As DataTable
        Call koneksiServer()
        Dim cmd As MySqlCommand
        Dim dr As MySqlDataReader
        Dim dt As New DataTable
        cmd = New MySqlCommand("CALL notadetaillaborat('" & Home.txtNoReg.Text & "')", conn)
        dr = cmd.ExecuteReader
        dt.Load(dr)
        ReportViewer1.LocalReport.DataSources.Clear()
        Return dt
    End Function

    Function tampilRadiologi() As DataTable
        Call koneksiServer()
        Dim cmd As MySqlCommand
        Dim dr As MySqlDataReader
        Dim dt As New DataTable
        cmd = New MySqlCommand("CALL notadetailradiologi('" & Home.txtNoReg.Text & "')", conn)
        dr = cmd.ExecuteReader
        dt.Load(dr)
        ReportViewer1.LocalReport.DataSources.Clear()
        Return dt
    End Function

    Function tampilOperasi() As DataTable
        Call koneksiServer()
        Dim cmd As MySqlCommand
        Dim dr As MySqlDataReader
        Dim dt As New DataTable
        cmd = New MySqlCommand("CALL notadetailoperasi('" & Home.txtNoReg.Text & "')", conn)
        dr = cmd.ExecuteReader
        dt.Load(dr)
        ReportViewer1.LocalReport.DataSources.Clear()
        Return dt
    End Function

    Function tampilHemo() As DataTable
        Call koneksiServer()
        Dim cmd As MySqlCommand
        Dim dr As MySqlDataReader
        Dim dt As New DataTable
        cmd = New MySqlCommand("CALL notadetailhemo('" & Home.txtNoReg.Text & "')", conn)
        dr = cmd.ExecuteReader
        dt.Load(dr)
        ReportViewer1.LocalReport.DataSources.Clear()
        Return dt
    End Function

    Private Sub ViewNotaAllTotal_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim noDaftarParam As New ReportParameter("noDaftar", Home.txtNoReg.Text)
        Dim noRmParam As New ReportParameter("noRm", Home.txtRekMed.Text)
        Dim namaParam As New ReportParameter("nama", Home.txtNamaPasien.Text)
        Dim ttlParam As New ReportParameter("ttl", Home.txtUmur.Text)
        Dim alamatParam As New ReportParameter("alamat", Home.txtAlamat.Text)
        Dim tglDaftarParam As New ReportParameter("tglDaftar", Home.txtTglMasuk.Text)
        Dim tglKeluarParam As New ReportParameter("tglKeluar", Home.txtTglKeluar.Text)
        Dim caraBayarParam As New ReportParameter("caraBayar", Home.txtCaraBayar.Text)
        Dim rawatInapParam As New ReportParameter("rawatInap", Home.txtUnit.Text)
        Dim kelasParam As New ReportParameter("kelas", Home.txtKelas.Text)
        Dim totalTinParam As New ReportParameter("totalPembulatan", Home.txtTotalAll.Text)
        Dim lamaInap As New ReportParameter("lamaInap", Home.txtJumInap.Text)
        Dim tarifKamarParam As New ReportParameter("tarifKamar", Home.txtTarifKmr2.Text)
        Dim tarifDPJPParam As New ReportParameter("tarifDPJP", Home.txtTarifDPJP.Text)
        Dim totalBiayaRuang As New ReportParameter("totalBiayaRuang", Home.txtBiayaRuang2.Text)
        Dim user As New ReportParameter("user", Home.txtUser.Text)
        Dim terbilangParam As New ReportParameter("terbilang", Home.txtTerbilang.Text)

        ReportViewer1.LocalReport.SetParameters(noDaftarParam)
        ReportViewer1.LocalReport.SetParameters(noRmParam)
        ReportViewer1.LocalReport.SetParameters(namaParam)
        ReportViewer1.LocalReport.SetParameters(ttlParam)
        ReportViewer1.LocalReport.SetParameters(alamatParam)
        ReportViewer1.LocalReport.SetParameters(tglDaftarParam)
        ReportViewer1.LocalReport.SetParameters(tglKeluarParam)
        ReportViewer1.LocalReport.SetParameters(caraBayarParam)
        ReportViewer1.LocalReport.SetParameters(rawatInapParam)
        ReportViewer1.LocalReport.SetParameters(kelasParam)
        ReportViewer1.LocalReport.SetParameters(totalTinParam)
        ReportViewer1.LocalReport.SetParameters(lamaInap)
        ReportViewer1.LocalReport.SetParameters(tarifKamarParam)
        ReportViewer1.LocalReport.SetParameters(tarifDPJPParam)
        ReportViewer1.LocalReport.SetParameters(totalBiayaRuang)
        ReportViewer1.LocalReport.SetParameters(user)
        ReportViewer1.LocalReport.SetParameters(terbilangParam)

        Dim rptRuang As New ReportDataSource("CetakRuangAkomodasi", tampilAkomodasiRuang)
        Dim rptAll As New ReportDataSource("CetakNotaTotalKasirRanap", tampilDetail)
        Dim rptJasa As New ReportDataSource("CetakJasa", tampilJasa)
        Dim rptVisite As New ReportDataSource("CetakVisite", tampilVisite)
        Dim rptAskep As New ReportDataSource("CetakAskep", tampilAskep)
        Dim rptFarklin As New ReportDataSource("CetakFarklin", tampilFarklin)
        Dim rptGizi As New ReportDataSource("CetakGizi", tampilGizi)
        Dim rptOxy As New ReportDataSource("CetakOksigen", tampilOksigen)
        Dim rptLab As New ReportDataSource("CetakLaboratorium", tampilLaboratorium)
        Dim rptRad As New ReportDataSource("CetakRadiologi", tampilRadiologi)
        Dim rptOpe As New ReportDataSource("CetakOperasi", tampilOperasi)
        Dim rptHemo As New ReportDataSource("CetakHemo", tampilHemo)

        ReportViewer1.LocalReport.DataSources.Add(rptRuang)
        ReportViewer1.LocalReport.DataSources.Add(rptAll)
        ReportViewer1.LocalReport.DataSources.Add(rptJasa)
        ReportViewer1.LocalReport.DataSources.Add(rptVisite)
        ReportViewer1.LocalReport.DataSources.Add(rptAskep)
        ReportViewer1.LocalReport.DataSources.Add(rptFarklin)
        ReportViewer1.LocalReport.DataSources.Add(rptGizi)
        ReportViewer1.LocalReport.DataSources.Add(rptOxy)
        ReportViewer1.LocalReport.DataSources.Add(rptLab)
        ReportViewer1.LocalReport.DataSources.Add(rptRad)
        ReportViewer1.LocalReport.DataSources.Add(rptOpe)
        ReportViewer1.LocalReport.DataSources.Add(rptHemo)
        ReportViewer1.LocalReport.Refresh()

        Me.ReportViewer1.SetDisplayMode(DisplayMode.PrintLayout)
    End Sub
End Class