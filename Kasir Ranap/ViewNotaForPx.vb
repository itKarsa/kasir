Imports MySql.Data.MySqlClient
Imports Microsoft.Reporting.WinForms
Public Class ViewNotaForPx

    Function tampilRegIGD() As DataTable
        Call koneksiServer()
        Dim query As String = ""
        Dim cmd As MySqlCommand
        Dim dr As MySqlDataReader
        Dim dt As New DataTable

        query = "SELECT * 
                   FROM vw_pasienrawatjalan 
                  WHERE noDaftar = '" & Home.txtNoReg.Text & "'"
        cmd = New MySqlCommand(query, conn)
        dr = cmd.ExecuteReader
        dt.Load(dr)
        ReportViewer1.LocalReport.DataSources.Clear()
        Return dt
    End Function

    Function tampilTindakanIGD() As DataTable
        Call koneksiServer()
        Dim query2 As String = ""
        Dim cmd2 As MySqlCommand
        Dim dr2 As MySqlDataReader
        Dim dt2 As New DataTable

        query2 = "SELECT tindakan, totaltarif
                    FROM t_detailtindakanpasienrajal
                   WHERE noTindakanPasienRajal IN (SELECT noTindakanPasienRajal 
                                                     FROM t_tindakanpasienrajal 
                                                    WHERE noRegistrasiRawatJalan IN (SELECT noRegistrasiRawatJalan 
                                                                                       FROM t_registrasirawatjalan
                                                                                      WHERE noDaftar = '" & Home.txtNoReg.Text & "'))
                     AND (tindakan NOT LIKE 'OKSIGEN%' AND tindakan NOT LIKE '%KONSULTASI%')"
        cmd2 = New MySqlCommand(query2, conn)
        dr2 = cmd2.ExecuteReader
        dt2.Load(dr2)
        ReportViewer1.LocalReport.DataSources.Clear()
        Return dt2
    End Function

    Function tampilKonsulIGD() As DataTable
        Call koneksiServer()
        Dim query3 As String = ""
        Dim cmd3 As MySqlCommand
        Dim dr3 As MySqlDataReader
        Dim dt3 As New DataTable

        query3 = "SELECT trj.noTindakanPasienRajal,
                        trj.noRegistrasiRawatJalan,
                        trj.tglTindakan,
                        trj.totalTarifTindakan,
                        trj.statusPembayaran,
                        trj.tglPembayaran,
                        dtrj.kdTarif,
                        dtrj.tindakan,
                        dtrj.tarif,
                        dtrj.jumlahTindakan,
                        dtrj.totalTarif,
                        DPJP.namapetugasMedis AS DPJP,
                        PPA.namapetugasMedis AS PPA,
                        reg.noDaftar,
                        unit.unit
                   FROM t_tindakanpasienrajal AS trj
                        INNER JOIN t_detailtindakanpasienrajal AS dtrj ON trj.noTindakanPasienRajal = dtrj.noTindakanPasienRajal
                        INNER JOIN t_registrasirawatjalan AS rj ON rj.noRegistrasiRawatJalan = trj.noRegistrasiRawatJalan
                        INNER JOIN t_registrasi AS reg ON reg.noDaftar = rj.noDaftar
                        INNER JOIN t_tenagamedis2 AS DPJP ON reg.kdTenagaMedis = DPJP.kdPetugasMedis
                        INNER JOIN t_tenagamedis2 AS PPA ON dtrj.kdTenagaMedis = PPA.kdPetugasMedis
                        INNER JOIN t_unit AS unit ON rj.kdUnit = unit.kdUnit
                  WHERE reg.noDaftar = '" & Home.txtNoReg.Text & "'
                    AND dtrj.tindakan LIKE '%KONSULTASI%'"
        cmd3 = New MySqlCommand(query3, conn)
        dr3 = cmd3.ExecuteReader
        dt3.Load(dr3)
        ReportViewer1.LocalReport.DataSources.Clear()
        Return dt3
    End Function

    Function tampilLabIGD() As DataTable
        Call koneksiServer()
        Dim query4 As String = ""
        Dim cmd4 As MySqlCommand
        Dim dr4 As MySqlDataReader
        Dim dt4 As New DataTable

        query4 = "SELECT * 
                   FROM vw_tindakanpxlabrajal
                  WHERE noDaftar = '" & Home.txtNoReg.Text & "'"
        cmd4 = New MySqlCommand(query4, conn)
        dr4 = cmd4.ExecuteReader
        dt4.Load(dr4)
        ReportViewer1.LocalReport.DataSources.Clear()
        Return dt4
    End Function

    Function tampilRadIGD() As DataTable
        Call koneksiServer()
        Dim query As String = ""
        Dim cmd As MySqlCommand
        Dim dr As MySqlDataReader
        Dim dt As New DataTable

        query = "SELECT * 
                   FROM vw_tindakanpxradrajal
                  WHERE noDaftar = '" & Home.txtNoReg.Text & "'"
        cmd = New MySqlCommand(query, conn)
        dr = cmd.ExecuteReader
        dt.Load(dr)
        ReportViewer1.LocalReport.DataSources.Clear()
        Return dt
    End Function

    Function tampilObatIGD() As DataTable
        Call koneksiFarmasi()
        Dim query As String = ""
        Dim cmd As MySqlCommand
        Dim dr As MySqlDataReader
        Dim dt As New DataTable
        query = "SELECT * 
                   FROM t_penjualanobatigd
                  WHERE noDaftar = '" & Home.txtNoReg.Text & "'"

        cmd = New MySqlCommand(query, conn)
        dr = cmd.ExecuteReader
        dt.Load(dr)
        ReportViewer1.LocalReport.DataSources.Clear()
        Return dt
    End Function

    Function tampilObatPinere() As DataTable
        Call koneksiFarmasi()
        Dim query As String = ""
        Dim cmd As MySqlCommand
        Dim dr As MySqlDataReader
        Dim dt As New DataTable

        query = "SELECT * 
                   FROM t_penjualanobatranap
                  WHERE noDaftar = '" & Home.txtNoReg.Text & "'"

        cmd = New MySqlCommand(query, conn)
        dr = cmd.ExecuteReader
        dt.Load(dr)
        ReportViewer1.LocalReport.DataSources.Clear()
        Return dt
    End Function

    Function tampilOxyIGD() As DataTable
        Call koneksiServer()
        Dim query As String = ""
        Dim cmd As MySqlCommand
        Dim dr As MySqlDataReader
        Dim dt As New DataTable

        query = "SELECT trj.noTindakanPasienRajal,
                        trj.noRegistrasiRawatJalan,
                        trj.tglTindakan,
                        trj.totalTarifTindakan,
                        trj.statusPembayaran,
                        trj.tglPembayaran,
                        dtrj.kdTarif,
                        dtrj.tindakan,
                        dtrj.tarif,
                        dtrj.jumlahTindakan,
                        dtrj.totalTarif,
                        DPJP.namapetugasMedis AS DPJP,
                        PPA.namapetugasMedis AS PPA,
                        reg.noDaftar,
                        unit.unit
                   FROM t_tindakanpasienrajal AS trj
                        INNER JOIN t_detailtindakanpasienrajal AS dtrj ON trj.noTindakanPasienRajal = dtrj.noTindakanPasienRajal
                        INNER JOIN t_registrasirawatjalan AS rj ON rj.noRegistrasiRawatJalan = trj.noRegistrasiRawatJalan
                        INNER JOIN t_registrasi AS reg ON reg.noDaftar = rj.noDaftar
                        INNER JOIN t_tenagamedis2 AS DPJP ON reg.kdTenagaMedis = DPJP.kdPetugasMedis
                        INNER JOIN t_tenagamedis2 AS PPA ON dtrj.kdTenagaMedis = PPA.kdPetugasMedis
                        INNER JOIN t_unit AS unit ON rj.kdUnit = unit.kdUnit
                  WHERE reg.noDaftar = '" & Home.txtNoReg.Text & "'
                    AND dtrj.tindakan LIKE 'OKSIGEN%'"
        cmd = New MySqlCommand(query, conn)
        dr = cmd.ExecuteReader
        dt.Load(dr)
        ReportViewer1.LocalReport.DataSources.Clear()
        Return dt
    End Function

    Function tampilHemoIGD() As DataTable
        Call koneksiServer()
        Dim query As String = ""
        Dim cmd As MySqlCommand
        Dim dr As MySqlDataReader
        Dim dt As New DataTable

        query = "CALL notadetailhemo('" & Home.txtNoReg.Text & "')"
        cmd = New MySqlCommand(query, conn)
        dr = cmd.ExecuteReader
        dt.Load(dr)
        ReportViewer1.LocalReport.DataSources.Clear()
        Return dt
    End Function

    Function tampilOperasiIGD() As DataTable
        Call koneksiServer()
        Dim query As String = ""
        Dim cmd As MySqlCommand
        Dim dr As MySqlDataReader
        Dim dt As New DataTable

        query = "CALL notadetailoperasi('" & Home.txtNoReg.Text & "')"
        cmd = New MySqlCommand(query, conn)
        dr = cmd.ExecuteReader
        dt.Load(dr)
        ReportViewer1.LocalReport.DataSources.Clear()
        Return dt
    End Function

    Function tampilObatOpeIGD() As DataTable
        Call koneksiFarmasi()
        Dim query As String = ""
        Dim cmd As MySqlCommand
        Dim dr As MySqlDataReader
        Dim dt As New DataTable

        query = "SELECT * 
                   FROM t_penjualanobatok
                  WHERE noDaftar = '" & Home.txtNoReg.Text & "'"
        cmd = New MySqlCommand(query, conn)
        dr = cmd.ExecuteReader
        dt.Load(dr)
        ReportViewer1.LocalReport.DataSources.Clear()
        Return dt
    End Function

    Private Sub ViewNotaForPx_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'Dim noDaftarParam As New ReportParameter("noDaftar", Home.txtNoReg.Text)
        Dim noRmParam As New ReportParameter("noRm", Home.txtRekMed.Text)
        Dim namaParam As New ReportParameter("nama", Home.txtNamaPasien.Text)
        Dim ttlParam As New ReportParameter("ttl", Home.txtUmur.Text)
        Dim alamatParam As New ReportParameter("alamat", Home.txtAlamat.Text)
        Dim ruangParam As New ReportParameter("ruang", Home.txtUnit.Text)
        Dim tglDaftarParam As New ReportParameter("tglDaftar", Home.txtTglMasuk.Text)
        Dim tglKeluarParam As New ReportParameter("tglKeluar", Home.txtTglKeluar.Text)
        Dim user As New ReportParameter("user", Home.txtUser.Text)

        'ReportViewer1.LocalReport.SetParameters(noDaftarParam)
        ReportViewer1.LocalReport.SetParameters(noRmParam)
        ReportViewer1.LocalReport.SetParameters(namaParam)
        ReportViewer1.LocalReport.SetParameters(ttlParam)
        ReportViewer1.LocalReport.SetParameters(alamatParam)
        ReportViewer1.LocalReport.SetParameters(ruangParam)
        ReportViewer1.LocalReport.SetParameters(tglDaftarParam)
        ReportViewer1.LocalReport.SetParameters(tglKeluarParam)
        ReportViewer1.LocalReport.SetParameters(user)

        Dim rptReg As New ReportDataSource("CetakTarifIGD", tampilRegIGD)
        Dim rptTindakan As New ReportDataSource("CetakTarifTindakanIGD", tampilTindakanIGD)
        Dim rptJasa As New ReportDataSource("CetakKonsulIGD", tampilKonsulIGD)
        'Dim rptVisite As New ReportDataSource("CetakVisite", tampilVisite)
        Dim rptOxy As New ReportDataSource("CetakOxyIGD", tampilOxyIGD)
        Dim rptLab As New ReportDataSource("CetakLabIGD", tampilLabIGD)
        Dim rptRad As New ReportDataSource("CetakRadIGD", tampilRadIGD)
        Dim rptObat As New ReportDataSource("CetakObatIGD", tampilObatIGD)
        Dim rptObatPinere As New ReportDataSource("CetakObatPinere", tampilObatPinere)
        Dim rptHemo As New ReportDataSource("CetakHDIGD", tampilHemoIGD)
        Dim rptOpe As New ReportDataSource("CetakOperasiIGD", tampilOperasiIGD)
        Dim rptObatOpe As New ReportDataSource("CetakObatOpeIGD", tampilObatOpeIGD)

        ReportViewer1.LocalReport.DataSources.Add(rptReg)
        ReportViewer1.LocalReport.DataSources.Add(rptTindakan)
        ReportViewer1.LocalReport.DataSources.Add(rptJasa)
        'ReportViewer1.LocalReport.DataSources.Add(rptVisite)
        ReportViewer1.LocalReport.DataSources.Add(rptOxy)
        ReportViewer1.LocalReport.DataSources.Add(rptLab)
        ReportViewer1.LocalReport.DataSources.Add(rptRad)
        ReportViewer1.LocalReport.DataSources.Add(rptObat)
        ReportViewer1.LocalReport.DataSources.Add(rptObatPinere)
        ReportViewer1.LocalReport.DataSources.Add(rptHemo)
        ReportViewer1.LocalReport.DataSources.Add(rptOpe)
        ReportViewer1.LocalReport.DataSources.Add(rptObatOpe)
        ReportViewer1.LocalReport.Refresh()

        Me.ReportViewer1.SetDisplayMode(DisplayMode.PrintLayout)
    End Sub
End Class