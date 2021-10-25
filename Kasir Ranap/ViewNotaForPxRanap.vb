Imports MySql.Data.MySqlClient
Imports Microsoft.Reporting.WinForms
Public Class ViewNotaForPxRanap

    Function tampilAko() As DataTable
        Call koneksiServer()
        Dim query As String = ""
        Dim cmd As MySqlCommand
        Dim dr As MySqlDataReader
        Dim dt As New DataTable

        query = "SELECT rgranap.tglMasukRawatInap, 
	                    COALESCE(rgranap.tglKeluarRawatInap,NOW()) AS tglKeluarRawatInap, 
	                    COALESCE(rgranap.jumlahHariMenginap,DATEDIFF( DATE_FORMAT( NOW(), '%Y-%m-%d %H:%m:%s' ), DATE_FORMAT( rgranap.tglMasukRawatInap, '%Y-%m-%d %H:%m:%s' ))+1) AS jumlahHariMenginap, 
	                    rgranap.tarifKmr,
	                    ppa.namapetugasMedis
                    FROM
	                    t_registrasirawatinap AS rgranap,
	                    t_registrasi AS reg,
	                    t_tenagamedis2 AS ppa
                    WHERE
	                    rgranap.noDaftarRawatInap = '" & Home.txtRegUnit.Text & "'
	                    AND reg.noDaftar = rgranap.noDaftar
	                    AND ppa.kdPetugasMedis = reg.kdTenagaMedis"
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

        query = "SELECT MAX( CASE WHEN dt.tindakan LIKE '%VISITE%' THEN ppa.namapetugasMedis END ) AS PPA,
			            COUNT(dt.jumlahTindakan) AS jumlahTindakan,
			            dt.tarif,
			            (COUNT(dt.jumlahTindakan) * dt.tarif) AS totalTarif 
                   FROM t_detailtindakanpasienranap AS dt,
			            t_tenagamedis2 AS ppa
                  WHERE dt.tindakan LIKE '%VISITE%'	 
	                AND dt.kdTenagaMedis LIKE '0%' 
                    AND (dt.kdTarif LIKE '02%' OR dt.kdTarif LIKE '13%' OR
			             dt.kdTarif LIKE '48%' OR dt.kdTarif LIKE '49%' OR 
			             dt.kdTarif LIKE '81%' OR dt.kdTarif LIKE '82%')
	                AND dt.kdTenagaMedis = ppa.kdPetugasMedis	
	                AND dt.noTindakanPasienRanap IN (SELECT td.noTindakanPasienRanap 
													   FROM t_tindakanpasienranap AS td
													  WHERE td.noDaftarRawatInap = '" & Home.txtRegUnit.Text & "'
												   GROUP BY td.noDaftarRawatInap)	 
               GROUP BY dt.kdTarif, ppa.namapetugasMedis"
        cmd = New MySqlCommand(query, conn)
        dr = cmd.ExecuteReader
        dt.Load(dr)
        ReportViewer1.LocalReport.DataSources.Clear()
        Return dt
    End Function

    Function tampilKonsul() As DataTable
        Call koneksiServer()
        Dim query As String = ""
        Dim cmd As MySqlCommand
        Dim dr As MySqlDataReader
        Dim dt As New DataTable

        query = "SELECT MAX( CASE WHEN dt.tindakan LIKE '%KONSULTASI%' THEN ppa.namapetugasMedis END ) AS PPA,
			            COUNT(dt.jumlahTindakan) AS jumlahTindakan,
			            dt.tarif,
			            (COUNT(dt.jumlahTindakan) * dt.tarif) AS totalTarif 
                   FROM t_detailtindakanpasienranap AS dt,
			            t_tenagamedis2 AS ppa
                  WHERE dt.tindakan LIKE '%KONSULTASI%'	 
	                AND dt.kdTenagaMedis LIKE '0%' 
                    AND (dt.kdTarif LIKE '02%' OR dt.kdTarif LIKE '13%' OR
			             dt.kdTarif LIKE '48%' OR dt.kdTarif LIKE '49%' OR 
			             dt.kdTarif LIKE '81%' OR dt.kdTarif LIKE '82%')
	                AND dt.kdTenagaMedis = ppa.kdPetugasMedis	
	                AND dt.noTindakanPasienRanap IN (SELECT td.noTindakanPasienRanap 
												       FROM t_tindakanpasienranap AS td
													  WHERE td.noDaftarRawatInap = '" & Home.txtRegUnit.Text & "'
											       GROUP BY td.noDaftarRawatInap)	 
            GROUP BY dt.kdTarif, ppa.namapetugasMedis"
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

        query = "SELECT COUNT(jumlahTindakan) AS jumlahTindakan,tarif,IF(SUM(totalTarif) IS NULL, 0, SUM(totalTarif)) AS totalTarif 
                   FROM t_detailtindakanpasienranap 
                  WHERE tindakan LIKE '%JASA ASUHAN KEPERAWATAN%' 
	                AND noTindakanPasienRanap IN (SELECT noTindakanPasienRanap 
									                FROM t_tindakanpasienranap
								                   WHERE noDaftarRawatInap = '" & Home.txtRegUnit.Text & "')"
        cmd = New MySqlCommand(query, conn)
        dr = cmd.ExecuteReader
        dt.Load(dr)
        ReportViewer1.LocalReport.DataSources.Clear()
        Return dt
    End Function

    Function tampilTindakan() As DataTable
        Call koneksiServer()
        Dim query As String = ""
        Dim cmd As MySqlCommand
        Dim dr As MySqlDataReader
        Dim dt As New DataTable

        query = "SELECT IF (SUM(totalTarif) IS NULL,0,SUM(totalTarif)) AS totalTarif
	               FROM t_detailtindakanpasienranap
                  WHERE (tindakan NOT LIKE 'JASA%' AND tindakan NOT LIKE 'OKSIGEN%')
	                AND (kdTarif NOT IN (020910,020920,020930,020940,020950,020960,020970))
	                AND noTindakanPasienRanap IN (SELECT noTindakanPasienRanap 
											        FROM t_tindakanpasienranap
											       WHERE noDaftarRawatInap = '" & Home.txtRegUnit.Text & "')"
        cmd = New MySqlCommand(query, conn)
        dr = cmd.ExecuteReader
        dt.Load(dr)
        ReportViewer1.LocalReport.DataSources.Clear()
        Return dt
    End Function

    Function tampilOxy() As DataTable
        Call koneksiServer()
        Dim query As String = ""
        Dim cmd As MySqlCommand
        Dim dr As MySqlDataReader
        Dim dt As New DataTable

        query = "SELECT COALESCE(SUM(totalTarif),0) AS totalTarif 
                   FROM t_detailtindakanpasienranap
                  WHERE tindakan LIKE 'OKSIGEN%'	
	                AND noTindakanPasienRanap IN (SELECT noTindakanPasienRanap 
													FROM t_tindakanpasienranap
												   WHERE noDaftarRawatInap = '" & Home.txtRegUnit.Text & "')"
        cmd = New MySqlCommand(query, conn)
        dr = cmd.ExecuteReader
        dt.Load(dr)
        ReportViewer1.LocalReport.DataSources.Clear()
        Return dt
    End Function

    Function tampilLab() As DataTable
        Call koneksiServer()
        Dim query As String = ""
        Dim cmd As MySqlCommand
        Dim dr As MySqlDataReader
        Dim dt As New DataTable

        query = "SELECT COALESCE(totalTindakanPenunjangRanap,0) AS totalTindakanPenunjangRanap
		           FROM t_tindakanpenunjangranap 
		          WHERE noRegistrasiPenunjangRanap IN (SELECT noRegistrasiPenunjangRanap 
												         FROM t_registrasipenunjangranap 
												        WHERE noDaftar = '" & Home.txtNoReg.Text & "'
														  AND unitAsal = '" & Home.txtUnit.Text & "')
                UNION ALL
                SELECT COALESCE(totalTindakanPA,0) AS totalTindakanPenunjangRanap
		          FROM t_tindakanpatologiranap 
		         WHERE noRegistrasiPARanap IN (SELECT noRegistrasiPARanap 
												                 FROM t_registrasipatologiranap 
											                    WHERE noDaftar = '" & Home.txtNoReg.Text & "'
										                          AND unitAsal = '" & Home.txtUnit.Text & "')
                UNION ALL																					
                SELECT COALESCE(totalTindakanBDRS,0) AS totalTindakanPenunjangRanap
		          FROM t_tindakanbdrs 
		         WHERE noRegistrasiBDRS IN (SELECT noRegistrasiBDRS 
										      FROM t_registrasibdrsranap 
										     WHERE noDaftar = '" & Home.txtNoReg.Text & "'
											   AND unitAsal = '" & Home.txtUnit.Text & "')"
        cmd = New MySqlCommand(query, conn)
        dr = cmd.ExecuteReader
        dt.Load(dr)
        ReportViewer1.LocalReport.DataSources.Clear()
        Return dt
    End Function

    Function tampilRad() As DataTable
        Call koneksiServer()
        Dim query As String = ""
        Dim cmd As MySqlCommand
        Dim dr As MySqlDataReader
        Dim dt As New DataTable

        query = "SELECT COALESCE(SUM(totalTindakanRadiologiRanap),0) AS totalTindakanRadiologiRanap
		           FROM t_tindakanradiologiranap
		          WHERE noRegistrasiRadiologiRanap IN (SELECT noRegistrasiRadiologiRanap 
												         FROM t_registrasiradiologiranap
												        WHERE noDaftar = '" & Home.txtNoReg.Text & "' 
														  AND unitAsal = '" & Home.txtUnit.Text & "')"
        cmd = New MySqlCommand(query, conn)
        dr = cmd.ExecuteReader
        dt.Load(dr)
        ReportViewer1.LocalReport.DataSources.Clear()
        Return dt
    End Function

    Function tampilObat() As DataTable
        Call koneksiFarmasi()
        Dim query As String = ""
        Dim cmd As MySqlCommand
        Dim dr As MySqlDataReader
        Dim dt As New DataTable

        query = "SELECT * 
                   FROM t_penjualanobatranap
                  WHERE noDaftar = '" & Home.txtNoReg.Text & "'
                    AND ruang LIKE '" & Home.txtUnit.Text & "%'"
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

        query = "SELECT COUNT(jumlahTindakan) AS jumlahTindakan,tarif,IF(SUM(totalTarif) IS NULL, 0, SUM(totalTarif)) AS totalTarif
	               FROM t_detailtindakanpasienranap
	              WHERE tindakan LIKE 'JASA ASUHAN FARMASI%'
	                AND noTindakanPasienRanap IN (SELECT noTindakanPasienRanap 
				                                    FROM t_tindakanpasienranap
				                                   WHERE noDaftarRawatInap = '" & Home.txtRegUnit.Text & "')"
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

        query = "SELECT COUNT(jumlahTindakan) AS jumlahTindakan,tarif,IF(SUM(totalTarif) IS NULL, 0, SUM(totalTarif)) AS totalTarif
	               FROM t_detailtindakanpasienranap
	              WHERE tindakan LIKE 'JASA ASUHAN GIZI%'
	                AND noTindakanPasienRanap IN (SELECT noTindakanPasienRanap 
				                                    FROM t_tindakanpasienranap
				                                   WHERE noDaftarRawatInap = '" & Home.txtRegUnit.Text & "')"
        cmd = New MySqlCommand(query, conn)
        dr = cmd.ExecuteReader
        dt.Load(dr)
        ReportViewer1.LocalReport.DataSources.Clear()
        Return dt
    End Function

    Function tampilTarifDpjp() As DataTable
        Call koneksiServer()
        Dim query As String = ""
        Dim cmd As MySqlCommand
        Dim dr As MySqlDataReader
        Dim dt As New DataTable

        If Home.txtUnit.Text.Contains("LAVENDER") Then
            query = "SELECT 0 AS tarif"
        ElseIf Home.txtUnit.Text.Contains("ICU") Then
            query = "SELECT 0 AS tarif"
        ElseIf Home.txtUnit.Text.Contains("HCU") Then
            query = "SELECT 0 AS tarif"
        ElseIf Home.txtUnit.Text.Contains("PICU") Then
            query = "SELECT 0 AS tarif"
        ElseIf Home.txtUnit.Text.Contains("NICU") Then
            query = "SELECT 0 AS tarif"
        Else
            query = "SELECT tarif
		               FROM vw_caritindakan
	                  WHERE kdTarif LIKE '0209%' AND kelas = '" & Home.txtKelas.Text & "'"
        End If

        cmd = New MySqlCommand(query, conn)
        dr = cmd.ExecuteReader
        dt.Load(dr)
        ReportViewer1.LocalReport.DataSources.Clear()
        Return dt
    End Function

    Function tampilHemo() As DataTable
        Call koneksiServer()
        Dim query As String = ""
        Dim cmd As MySqlCommand
        Dim dr As MySqlDataReader
        Dim dt As New DataTable

        query = "SELECT COALESCE(SUM(totalTarifTindakan),0) AS subTotal  
		           FROM t_tindakanhd
	              WHERE noRegistrasiHD IN (SELECT noRegistrasiHD 
											 FROM t_registrasihdranap 
										    WHERE noRegistrasi = '" & Home.txtNoReg.Text & "'
											  AND unitAsal = '" & Home.txtUnit.Text & "')"
        cmd = New MySqlCommand(query, conn)
        dr = cmd.ExecuteReader
        dt.Load(dr)
        ReportViewer1.LocalReport.DataSources.Clear()
        Return dt
    End Function

    Function tampilOperasi() As DataTable
        Call koneksiServer()
        Dim query As String = ""
        Dim cmd As MySqlCommand
        Dim dr As MySqlDataReader
        Dim dt As New DataTable

        query = "SELECT COALESCE(SUM(totalTarifTindakan),0) AS totalTarifTindakan  
		           FROM t_tindakanop
		          WHERE noRegistrasiOP IN (SELECT noRegistrasiOP 
											 FROM t_registrasiop 
											WHERE noRegistrasiPasien = '" & Home.txtRegUnit.Text & "')
                  UNION ALL
                 SELECT COALESCE(SUM(totalTarifTindakan),0) AS totalTarifTindakan  
		           FROM t_tindakanokparu
		          WHERE noRegistrasiOP IN (SELECT noRegistrasiOP 
											 FROM t_registrasiokparu 
											WHERE noRegistrasiPasien = '" & Home.txtRegUnit.Text & "')"
        cmd = New MySqlCommand(query, conn)
        dr = cmd.ExecuteReader
        dt.Load(dr)
        ReportViewer1.LocalReport.DataSources.Clear()
        Return dt
    End Function

    Function tampilObatOK() As DataTable
        Call koneksiFarmasi()
        Dim query As String = ""
        Dim cmd As MySqlCommand
        Dim dr As MySqlDataReader
        Dim dt As New DataTable

        query = "SELECT * 
                   FROM t_penjualanobatok
                  WHERE noDaftar = '" & Home.txtNoReg.Text & "'
                    AND unit = '" & Home.txtUnit.Text & "'"
        cmd = New MySqlCommand(query, conn)
        dr = cmd.ExecuteReader
        dt.Load(dr)
        ReportViewer1.LocalReport.DataSources.Clear()
        Return dt
    End Function

    Private Sub ViewNotaForPxRanap_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim noRmParam As New ReportParameter("noRm", Home.txtRekMed.Text)
        Dim namaParam As New ReportParameter("nama", Home.txtNamaPasien.Text)
        Dim ttlParam As New ReportParameter("ttl", Home.txtUmur.Text)
        Dim alamatParam As New ReportParameter("alamat", Home.txtAlamat.Text)
        Dim ruangParam As New ReportParameter("ruang", Home.txtUnit.Text)
        Dim kelasParam As New ReportParameter("kelas", Home.txtKelas.Text)
        Dim tglDaftarParam As New ReportParameter("tglDaftar", Home.txtTglMasuk.Text)
        Dim tglKeluarParam As New ReportParameter("tglKeluar", Home.txtTglKeluar.Text)
        Dim user As New ReportParameter("user", Home.txtUser.Text)
        Dim tarifDpjp As New ReportParameter("tarifDPJP", Home.txtTarifDPJP.Text)

        'ReportViewer1.LocalReport.SetParameters(noDaftarParam)
        ReportViewer1.LocalReport.SetParameters(noRmParam)
        ReportViewer1.LocalReport.SetParameters(namaParam)
        ReportViewer1.LocalReport.SetParameters(ttlParam)
        ReportViewer1.LocalReport.SetParameters(alamatParam)
        ReportViewer1.LocalReport.SetParameters(ruangParam)
        ReportViewer1.LocalReport.SetParameters(kelasParam)
        ReportViewer1.LocalReport.SetParameters(tglDaftarParam)
        ReportViewer1.LocalReport.SetParameters(tglKeluarParam)
        ReportViewer1.LocalReport.SetParameters(user)
        ReportViewer1.LocalReport.SetParameters(tarifDpjp)


        Dim rptAko As New ReportDataSource("CetakAkomodasiRanap", tampilAko)
        Dim rptVisite As New ReportDataSource("CetakVisite", tampilVisite)
        Dim rptKonsul As New ReportDataSource("CetakKonsul", tampilKonsul)
        Dim rptAskep As New ReportDataSource("CetakAskep", tampilAskep)
        Dim rptTindakan As New ReportDataSource("CetakTindakanRanap", tampilTindakan)
        Dim rptOxy As New ReportDataSource("CetakOxy", tampilOxy)
        Dim rptLab As New ReportDataSource("CetakLab", tampilLab)
        Dim rptRad As New ReportDataSource("CetakRad", tampilRad)
        Dim rptObat As New ReportDataSource("CetakObat", tampilObat)
        Dim rptFarklin As New ReportDataSource("CetakFarklin", tampilFarklin)
        Dim rptGizi As New ReportDataSource("CetakGizi", tampilGizi)
        Dim rptDPJP As New ReportDataSource("CetakTarifDPJP", tampilTarifDpjp)
        Dim rptHemo As New ReportDataSource("CetakHemo", tampilHemo)
        Dim rptOpe As New ReportDataSource("CetakOperasi", tampilOperasi)
        Dim rptObatOpe As New ReportDataSource("CetakObatOK", tampilObatOK)
        ReportViewer1.LocalReport.DataSources.Add(rptAko)
        ReportViewer1.LocalReport.DataSources.Add(rptVisite)
        ReportViewer1.LocalReport.DataSources.Add(rptKonsul)
        ReportViewer1.LocalReport.DataSources.Add(rptAskep)
        ReportViewer1.LocalReport.DataSources.Add(rptTindakan)
        ReportViewer1.LocalReport.DataSources.Add(rptOxy)
        ReportViewer1.LocalReport.DataSources.Add(rptLab)
        ReportViewer1.LocalReport.DataSources.Add(rptRad)
        ReportViewer1.LocalReport.DataSources.Add(rptObat)
        ReportViewer1.LocalReport.DataSources.Add(rptFarklin)
        ReportViewer1.LocalReport.DataSources.Add(rptGizi)
        ReportViewer1.LocalReport.DataSources.Add(rptDPJP)
        ReportViewer1.LocalReport.DataSources.Add(rptHemo)
        ReportViewer1.LocalReport.DataSources.Add(rptOpe)
        ReportViewer1.LocalReport.DataSources.Add(rptObatOpe)
        ReportViewer1.LocalReport.Refresh()


        Me.ReportViewer1.SetDisplayMode(DisplayMode.PrintLayout)
    End Sub
End Class