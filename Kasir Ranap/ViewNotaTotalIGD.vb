Imports MySql.Data.MySqlClient
Imports Microsoft.Reporting.WinForms
Public Class ViewNotaTotalIGD

    Function tampilDetail() As DataTable
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
                    AND dtrj.tindakan NOT LIKE 'JASA%' 
                    AND dtrj.kdTarif NOT IN (0500040,1300011,4800042,4900042,5500060,020910,020920,020930,020940,020950,020960,020970)
               ORDER BY tglTindakan ASC, tindakan ASC"
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
                    AND dtrj.tindakan LIKE 'JASA%'
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

    Function tampilLaboratorium() As DataTable
        Call koneksiServer()
        Dim dt As New DataTable
        cmd = New MySqlCommand("CALL notadetaillaboratigd('" & Home.txtNoReg.Text & "')", conn)
        dr = cmd.ExecuteReader
        dt.Load(dr)
        ReportViewer1.LocalReport.DataSources.Clear()
        Return dt
    End Function

    Function tampilRadiologi() As DataTable
        Call koneksiServer()
        Dim dt As New DataTable
        cmd = New MySqlCommand("CALL notadetailradiologiigd('" & Home.txtNoReg.Text & "')", conn)
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

    Private Sub ViewNotaTotalIGD_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim noDaftarParam As New ReportParameter("noDaftar", Home.txtNoReg.Text)
        Dim noRmParam As New ReportParameter("noRm", Home.txtRekMed.Text)
        Dim namaParam As New ReportParameter("nama", Home.txtNamaPasien.Text)
        Dim ttlParam As New ReportParameter("ttl", Home.txtUmur.Text)
        Dim alamatParam As New ReportParameter("alamat", Home.txtAlamat.Text)
        Dim tglDaftarParam As New ReportParameter("tglDaftar", Home.txtTglMasuk.Text)
        Dim caraBayarParam As New ReportParameter("caraBayar", Home.txtCaraBayar.Text)
        Dim unitParam As New ReportParameter("unit", Home.txtUnit.Text)
        Dim karcisParam As New ReportParameter("karcis", Home.txtKarBiaya2.Text)
        Dim tarifDPJPParam As New ReportParameter("tarifDPJP", Home.txtKarKonsul2.Text)
        Dim user As New ReportParameter("user", Home.txtUser.Text)
        Dim terbilangParam As New ReportParameter("terbilang", Home.txtTerbilang.Text)

        ReportViewer1.LocalReport.SetParameters(noDaftarParam)
        ReportViewer1.LocalReport.SetParameters(noRmParam)
        ReportViewer1.LocalReport.SetParameters(namaParam)
        ReportViewer1.LocalReport.SetParameters(ttlParam)
        ReportViewer1.LocalReport.SetParameters(alamatParam)
        ReportViewer1.LocalReport.SetParameters(tglDaftarParam)
        ReportViewer1.LocalReport.SetParameters(caraBayarParam)
        ReportViewer1.LocalReport.SetParameters(unitParam)
        ReportViewer1.LocalReport.SetParameters(karcisParam)
        ReportViewer1.LocalReport.SetParameters(tarifDPJPParam)
        ReportViewer1.LocalReport.SetParameters(user)
        ReportViewer1.LocalReport.SetParameters(terbilangParam)

        Dim rptAll As New ReportDataSource("CetakAllNotaIGD", tampilDetail)
        Dim rptJasa As New ReportDataSource("CetakJasa", tampilJasa)
        Dim rptOxy As New ReportDataSource("CetakOksigen", tampilOksigen)
        Dim rptLab As New ReportDataSource("CetakLaboratorium", tampilLaboratorium)
        Dim rptRad As New ReportDataSource("CetakRadiologi", tampilRadiologi)
        Dim rptOpe As New ReportDataSource("CetakOperasi", tampilOperasi)
        ReportViewer1.LocalReport.DataSources.Add(rptAll)
        ReportViewer1.LocalReport.DataSources.Add(rptJasa)
        ReportViewer1.LocalReport.DataSources.Add(rptOxy)
        ReportViewer1.LocalReport.DataSources.Add(rptLab)
        ReportViewer1.LocalReport.DataSources.Add(rptRad)
        ReportViewer1.LocalReport.DataSources.Add(rptOpe)
        ReportViewer1.LocalReport.Refresh()

        Me.ReportViewer1.SetDisplayMode(DisplayMode.PrintLayout)
    End Sub
End Class