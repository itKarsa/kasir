Imports System.ComponentModel
Imports System.Deployment.Application
Imports Newtonsoft.Json
Imports MySql.Data.MySqlClient

Public Class Home

    Public unit As String
    Public lab As String
    Public obt As String
    Dim jatim As New BankJatim
    Dim va As String
    Dim Status As String
    Dim dtBank As String = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")

    Sub cariPasien()
        Call koneksiServer()
        Dim query As String
        Dim cmd As MySqlCommand
        Dim da As MySqlDataAdapter
        query = "SELECT noRekamedis
                   FROM t_pasien
                  WHERE ( nmPasien LIKE '%" & txtRekMed.Text & "%' OR noRekamedis LIKE '%" & txtRekMed.Text & "%' )
               ORDER BY noRekamedis ASC LIMIT 1"
        Try
            cmd = New MySqlCommand(query, conn)
            da = New MySqlDataAdapter(cmd)

            Dim str As New DataTable
            str.Clear()
            da.Fill(str)
            If str.Rows.Count() > 0 Then
                txtRekMed.Text = str.Rows(0)(0).ToString
                Dim fReg As DaftarRegPerPasien = New DaftarRegPerPasien
                fReg.Ambil_Data = True
                fReg.Form_Ambil_Data = "Reg"
                fReg.ShowDialog()
            Else
                MessageBox.Show("Pasien Tidak Ada / Belum Terdaftar", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End Try
        conn.Close()
    End Sub

    Sub cariPasienPeg()
        Call koneksiServer()
        Dim query As String
        Dim cmd As MySqlCommand
        Dim da As MySqlDataAdapter
        query = "SELECT noRekamedis
                       FROM t_pasien
                      WHERE noRekamedis = '00000000'"
        Try
            cmd = New MySqlCommand(query, conn)
            da = New MySqlDataAdapter(cmd)

            Dim str As New DataTable
            str.Clear()
            da.Fill(str)
            If str.Rows.Count() > 0 Then
                txtRekMed.Text = str.Rows(0)(0).ToString
                Dim fReg As DaftarRegPerPasien = New DaftarRegPerPasien
                fReg.Ambil_Data = True
                fReg.Form_Ambil_Data = "Reg"
                fReg.ShowDialog()
            Else
                MessageBox.Show("Pasien Tidak Ada / Belum Terdaftar", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End Try
        conn.Close()
    End Sub

    Sub tampilTarifKamar()
        Call koneksiServer()

        Dim query As String
        Dim cmd As MySqlCommand
        Dim total As Integer
        query = "SELECT * FROM vw_daftarruangakomodasi
                  WHERE noDaftar = '" & txtNoReg.Text & "'"
        Try
            cmd = New MySqlCommand(query, conn)
            dr = cmd.ExecuteReader
            dgvRuang.Rows.Clear()
            Do While dr.Read
                If dr.IsDBNull(6) Then
                    total = 0
                Else
                    total = dr.GetString(6).ToString
                End If
                dgvRuang.Rows.Add(dr.Item("noDaftar"), dr.Item("noDaftarRawatInap"),
                                  dr.Item("rawatInap"), dr.Item("kelas"), dr.Item("tarifKmr"),
                                  dr.Item("jumlahHariMenginap"), total)
            Loop
            dr.Close()
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
        conn.Close()
    End Sub

    Sub tampilNominal()
        Call koneksiServer()

        Dim query As String
        Dim cmd As MySqlCommand
        Dim total As Double = 0

        query = "CALL angkaTerbilang('" & txtNoReg.Text & "','" & txtRegUnit.Text & "','" & txtUnit.Text & "','" & txtKelas.Text & "')"

        Try
            cmd = New MySqlCommand(query, conn)
            dr = cmd.ExecuteReader
            Do While dr.Read
                total = dr.GetString(0)
            Loop
            dr.Close()
            txtTerbilangKwitansi.Text = Terbilang(total) & " Rupiah"
            txtNominalKwitansi.Text = total.ToString("#,##0")
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
        conn.Close()
    End Sub

    Sub tampilNominalIGD()
        Call koneksiServer()

        Dim query As String
        Dim cmd As MySqlCommand
        Dim total As Integer
        query = "CALL angkaTerbilangIGD( '" & txtNoReg.Text & "')"
        Try
            cmd = New MySqlCommand(query, conn)
            dr = cmd.ExecuteReader
            Do While dr.Read
                total = dr.GetString(0)
            Loop
            dr.Close()
            txtTerbilangKwitansi.Text = Terbilang(total) & " Rupiah"
            txtNominalKwitansi.Text = total.ToString("#,##0")
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
        conn.Close()
    End Sub

    Sub cekRIK()
        Call koneksiServer()

        Dim query As String
        Dim cmd As MySqlCommand
        query = "CALL cekRIK( '" & txtUnit.Text & "')"
        Try
            cmd = New MySqlCommand(query, conn)
            dr = cmd.ExecuteReader
            Do While dr.Read
                txtRIK.Text = dr.Item("Ruang").ToString
            Loop
            dr.Close()
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
        conn.Close()
    End Sub

    Sub totalTarifKamar()
        Dim totBayar As Long
        totBayar = 0
        For i As Integer = 0 To dgvRuang.Rows.Count - 1
            totBayar = totBayar + Val(dgvRuang.Rows(i).Cells(6).Value)
        Next
        txtTotalTarifKmr.Text = totBayar.ToString("#,##0")
        txtTotalTarifKmr2.Text = totBayar
    End Sub
#Region "Clear"
    Sub clearText()
        txtRekMed.Text = ""
        txtNoReg.Text = ""
        txtTglMasuk.Text = ""
        txtNamaPasien.Text = ""
        txtTglLahir.Text = ""
        txtUmur.Text = ""
        txtJk.Text = ""
        txtAlamat.Text = ""
        txtTelp.Text = ""
        txtCaraBayar.Text = ""
        txtPenjamin.Text = ""
        txtUnit.Text = ""
        txtKelas.Text = ""
        txtTarifKmr.Text = 0
        txtTarifKmr2.Text = 0
        txtCaraBayar.Text = ""
        txtPenjamin.Text = ""
        txtTglMasuk.Text = ""
        txtJumInap.Text = 0
        txtRegUnit.Text = ""
        txtTarifDPJP.Text = 0
        txtTmpLahir.Text = ""
        txtTglKeluar.Text = ""
        txtTotalTindakan.Text = 0
        txtPerTotalTindakan.Text = 0
        txtTotalLab.Text = 0
        txtPerTotalLab.Text = 0
        txtTotalRad.Text = 0
        txtPerTotalRad.Text = 0
        txtTotalObat.Text = 0
        txtPerTotalObat.Text = 0
        txtTotalAll.Text = 0
        txtPertotalAll.Text = 0
        txtBiayaRuang.Text = 0
        txtBiayaRuang2.Text = 0
        txtBiayaTindakan.Text = 0
        txtTerbilang.Text = ""
        txtTerbilangKwitansi.Text = ""
        txtKarRM.Text = ""
        txtKarNama.Text = ""
        txtKarReg.Text = ""
        txtKarTglReg.Text = ""
        txtKarAlamat.Text = ""
        txtKarTtl.Text = ""
        txtKarJk.Text = ""
        txtKarUmur.Text = ""
        txtKarPoli.Text = ""
        txtKarBiaya.Text = 0
        txtKarBiaya2.Text = 0
        txtKarKonsul.Text = 0
        txtKarKonsul2.Text = 0
        txtKarTotal.Text = 0
        txtKarStatus.Text = ""
        dgvTindakan.Rows.Clear()
        dgvDetailTindakan.Rows.Clear()
        dgvLab.Rows.Clear()
        dgvDetailLab.Rows.Clear()
        dgvRad.Rows.Clear()
        dgvDetailRad.Rows.Clear()
        dgvObat.Rows.Clear()
        dgvDetailObat.Rows.Clear()
        dgvAll.Rows.Clear()
        dgvDetailAll.Rows.Clear()
    End Sub
#End Region
    Sub DisBackColor()
        txtNoReg.BackColor = Color.White
        txtTglMasuk.BackColor = Color.White
        txtNamaPasien.BackColor = Color.White
        txtUmur.BackColor = Color.White
        txtJk.BackColor = Color.White
        txtAlamat.BackColor = Color.White
        txtTelp.BackColor = Color.White
        txtCaraBayar.BackColor = Color.White
        txtPenjamin.BackColor = Color.White
        txtUnit.BackColor = Color.White
        txtKelas.BackColor = Color.White
    End Sub

    Sub JmlPasien()
        Dim dt As String
        dt = DateTime.Now.ToString("yyyy-MM-dd")

        Call koneksiServer()
        Dim query As String
        Dim cmd As MySqlCommand
        Dim da As MySqlDataAdapter
        Dim jml As String = ""
        query = "SELECT COUNT(noRegistrasiRawatJalan) as jumlah
                   FROM t_registrasirawatjalan 
                  WHERE tglMasukRawatJalan LIKE '" & dt & "%'"
        Try
            cmd = New MySqlCommand(query, conn)
            da = New MySqlDataAdapter(cmd)

            Dim str As New DataTable
            str.Clear()
            da.Fill(str)
            If str.Rows.Count() > 0 Then
                jml = str.Rows(0)(0).ToString
            End If
            MessageBox.Show("Jumlah pasien rawat jalan hari ini : " & jml, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End Try
        conn.Close()
    End Sub

    Sub hitungInap()
        Call koneksiServer()
        Try
            Dim query As String
            Dim cmd As MySqlCommand
            Dim dr As MySqlDataReader
            Dim jml As String
            query = "SELECT tarifKmr, COALESCE(DATEDIFF(DATE_FORMAT(tglKeluarRawatInap,'%Y-%m-%d'), DATE_FORMAT(tglMasukRawatInap,'%Y-%m-%d'))+1,
							                   DATEDIFF(DATE_FORMAT(NOW(),'%Y-%m-%d'), DATE_FORMAT(tglMasukRawatInap,'%Y-%m-%d'))) AS jumlah 
	                   FROM t_registrasirawatinap  
                      WHERE noDaftarRawatInap = 'RI050921143310-11448'"
            cmd = New MySqlCommand(query, conn)
            dr = cmd.ExecuteReader

            While dr.Read
                txtTarifKmr.Text = dr.GetString("tarifKmr")
                txtTarifKmr2.Text = dr.GetString("tarifKmr")
                If dr.IsDBNull(1) Then
                    jml = 1
                Else
                    jml = dr.GetString(1).ToString
                End If
                txtJumInap.Text = jml
            End While
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Sub hitungInapSampaiDgn()
        Call koneksiServer()
        Try
            Dim query As String
            Dim cmd As MySqlCommand
            Dim dr As MySqlDataReader
            Dim jml As String
            query = "SELECT tarifKmr,DATEDIFF('" & Format(txtTglKeluar.Value, "yyyy-MM-dd") & "', DATE_FORMAT( tglDaftar, '%Y-%m-%d' )) AS jumlah 
                       FROM vw_pasienrawatinap
                      WHERE noDaftarRawatInap = '" & txtRegUnit.Text & "'"
            cmd = New MySqlCommand(query, conn)
            dr = cmd.ExecuteReader

            While dr.Read
                txtTarifKmr.Text = dr.GetString("tarifKmr")
                txtTarifKmr2.Text = dr.GetString("tarifKmr")
                If dr.IsDBNull(1) Then
                    jml = 1
                Else
                    jml = dr.GetString(1).ToString
                End If
                txtJumInap.Text = CInt(jml) + 1
            End While
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Sub tarifDPJP()
        Call koneksiServer()
        Try
            Dim query As String = ""
            Dim cmd As MySqlCommand
            Dim dr As MySqlDataReader

            Select Case unit
                Case "IG"
                    query = "SELECT karciPendaftaran,konsuldokter,statusPembayaran
                               FROM t_registrasirawatjalan 
                              WHERE noDaftar = '" & txtNoReg.Text & "'"
                Case "RJ"
                    query = "SELECT karciPendaftaran,konsuldokter,statusPembayaran
                               FROM t_registrasirawatjalan 
                              WHERE noDaftar = '" & txtNoReg.Text & "'"
                Case "RI"
                    If txtUnit.Text.Contains("PERINATOLOGI") Then
                        query = "SELECT tarif
                               FROM vw_caritindakan 
                              WHERE kelas = 'UTAMA'
                                AND tindakan = 'DOKTER PENANGGUNG JAWAB PASIEN'
                                AND kdKelompokTindakan = '02'"
                    Else
                        query = "SELECT tarif
                               FROM vw_caritindakan 
                              WHERE kelas = '" & txtKelas.Text & "'
                                AND tindakan = 'DOKTER PENANGGUNG JAWAB PASIEN'
                                AND kdKelompokTindakan = '02'"
                    End If

            End Select
                    cmd = New MySqlCommand(query, conn)
            dr = cmd.ExecuteReader
            Select Case unit
                Case "IG"
                    While dr.Read
                        txtKarcis.Text = dr.GetString("karciPendaftaran")
                        txtTarifDPJP.Text = dr.GetString("konsuldokter")

                        txtKarBiaya.Text = dr.GetString("karciPendaftaran")
                        txtKarBiaya2.Text = dr.GetString("karciPendaftaran")
                        txtKarKonsul.Text = dr.GetString("konsuldokter")
                        txtKarKonsul2.Text = dr.GetString("konsuldokter")
                        If dr.IsDBNull(2) Then
                            txtKarStatus.Text = "BELUM LUNAS"
                        Else
                            txtKarStatus.Text = dr.GetString(2).ToString
                        End If
                    End While
                Case "RJ"
                    While dr.Read
                        txtKarcis.Text = dr.GetString("karciPendaftaran")
                        txtTarifDPJP.Text = dr.GetString("konsuldokter")

                        txtKarBiaya.Text = dr.GetString("karciPendaftaran")
                        txtKarBiaya2.Text = dr.GetString("karciPendaftaran")
                        txtKarKonsul.Text = dr.GetString("konsuldokter")
                        txtKarKonsul2.Text = dr.GetString("konsuldokter")
                        If dr.IsDBNull(2) Then
                            txtKarStatus.Text = "BELUM LUNAS"
                        Else
                            txtKarStatus.Text = dr.GetString(2).ToString
                        End If
                    End While
                Case "RI"
                    If txtUnit.Text.Contains("LAVENDER") Or txtUnit.Text.Contains("ICU") Or txtUnit.Text.Contains("HCU") Or txtUnit.Text.Contains("PICU") Then
                        txtTarifDPJP.Text = 0
                        txtKarBiaya.Text = 0
                        txtKarKonsul.Text = 0
                        txtKarTotal.Text = 0
                        txtKarStatus.Text = "LUNAS"
                    Else
                        While dr.Read
                            txtTarifDPJP.Text = dr.GetString("tarif")
                            txtKarBiaya.Text = 0
                            txtKarKonsul.Text = 0
                            txtKarTotal.Text = 0
                            txtKarStatus.Text = "LUNAS"
                            'Tampilkan karcis igdnya
                        End While
                    End If
            End Select
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

#Region "Tindakan"
    Sub totalTarifTindakan()
        Dim totBayar As Long
        totBayar = 0
        For i As Integer = 0 To dgvTindakan.Rows.Count - 1
            totBayar = totBayar + Val(dgvTindakan.Rows(i).Cells(3).Value)
        Next
        txtTotalTindakan.Text = totBayar.ToString("#,##0")
        'txtTotal2.Text = totBayar
    End Sub

    Sub totalTarifDetailTin()
        Dim totBayar As Long
        totBayar = 0
        For i As Integer = 0 To dgvDetailTindakan.Rows.Count - 1
            totBayar = totBayar + Val(dgvDetailTindakan.Rows(i).Cells(6).Value)
        Next
        txtPerTotalTindakan.Text = totBayar.ToString("#,##0")
    End Sub

    Sub DaftarTindakan()
        Call koneksiServer()
        Dim query As String = ""
        Dim cmd As MySqlCommand
        Dim dr As MySqlDataReader

        Select Case unit
            Case "IG"
                query = "SELECT *
                           FROM vw_tindakanpasienrajalkasir
                          WHERE noDaftar = '" & txtNoReg.Text & "'
                            AND (totalTarifTindakan IS NOT NULL AND totalTarifTindakan != 0)
                       ORDER BY tglTindakan ASC"
            Case "RJ"
                query = "SELECT *
                           FROM vw_tindakanpasienrajalkasir
                          WHERE noDaftar = '" & txtNoReg.Text & "'
                            AND (totalTarifTindakan IS NOT NULL AND totalTarifTindakan != 0)
                       ORDER BY tglTindakan ASC"
            Case "RI"
                query = "SELECT *
                           FROM vw_tindakanpasienranapkasir
                          WHERE noDaftar = '" & txtNoReg.Text & "'
                            AND (totalTarifTindakan IS NOT NULL AND totalTarifTindakan != 0)
                       ORDER BY tglTindakan ASC"
        End Select

        Try
            cmd = New MySqlCommand(query, conn)
            dr = cmd.ExecuteReader
            dgvTindakan.Rows.Clear()
            Select Case unit
                Case "IG"
                    Do While dr.Read
                        dgvTindakan.Rows.Add(dr.Item("noRegistrasiRawatJalan"), dr.Item("tglTindakan"), dr.Item("unit"),
                               dr.Item("totalTarifTindakan"), dr.Item("statusPembayaran"), dr.Item("noTindakanPasienRajal"))
                    Loop
                Case "RJ"
                    Do While dr.Read
                        dgvTindakan.Rows.Add(dr.Item("noRegistrasiRawatJalan"), dr.Item("tglTindakan"), dr.Item("unit"),
                               dr.Item("totalTarifTindakan"), dr.Item("statusPembayaran"), dr.Item("noTindakanPasienRajal"))
                    Loop
                Case "RI"
                    Do While dr.Read
                        dgvTindakan.Rows.Add(dr.Item("noDaftarRawatInap"), dr.Item("tglTindakan"), dr.Item("rawatInap"),
                               dr.Item("totalTarifTindakan"), dr.Item("statusPembayaran"), dr.Item("noTindakanPasienRanap"))
                    Loop
            End Select

            dr.Close()
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
        conn.Close()
    End Sub

    Sub DetailTindakan(noTindakan As String)
        Call koneksiServer()
        Dim query As String = ""
        Dim cmd As MySqlCommand
        Dim dr As MySqlDataReader

        Select Case unit
            Case "IG"
                query = "SELECT kdTarif, tindakan, jumlahTindakan, tarif, DPJP, PPA, totalTarif 
                           FROM vw_tindakanpasienrajaldetail
                          WHERE noTindakanPasienRajal = '" & noTindakan & "'"
            Case "RJ"
                query = "SELECT kdTarif, tindakan, jumlahTindakan, tarif, DPJP, PPA, totalTarif 
                           FROM vw_tindakanpasienrajaldetail 
                          WHERE noTindakanPasienRajal = '" & noTindakan & "'"
            Case "RI"
                query = "SELECT kdTarif, tindakan, jumlahTindakan, tarif, DPJP, PPA, totalTarif 
                           FROM vw_tindakanpasienranapdetail 
                          WHERE noTindakanPasienRanap = '" & noTindakan & "'"
        End Select

        Try
            cmd = New MySqlCommand(query, conn)
            dr = cmd.ExecuteReader
            dgvDetailTindakan.Rows.Clear()
            Do While dr.Read
                dgvDetailTindakan.Rows.Add(dr.Item("kdTarif"), dr.Item("tindakan"), dr.Item("jumlahTindakan"),
                               dr.Item("tarif"), dr.Item("DPJP"), dr.Item("PPA"), dr.Item("totalTarif"))
            Loop
            dr.Close()
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
        conn.Close()
    End Sub

    Sub LunasTindakanAll(noTindakan As String)
        Dim dt As String
        dt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")

        Dim str As String = ""
        Select Case unit
            Case "IG"
                str = "UPDATE t_tindakanpasienrajal 
                          SET statusPembayaran = 'LUNAS',
                              tglPembayaran = '" & dt & "'
                        WHERE noTindakanPasienRajal IN (" & noTindakan & ")"
            Case "RJ"
                str = "UPDATE t_tindakanpasienrajal 
                          SET statusPembayaran = 'LUNAS',
                              tglPembayaran = '" & dt & "'
                        WHERE noTindakanPasienRajal IN (" & noTindakan & ")"
            Case "RI"
                str = "UPDATE t_tindakanpasienranap 
                          SET statusPembayaran = 'LUNAS',
                              tglPembayaran = '" & dt & "'
                        WHERE noTindakanPasienRanap IN (" & noTindakan & ")"
        End Select

        Call koneksiServer()
        Dim cmd As MySqlCommand
        Try
            cmd = New MySqlCommand(str, conn)
            cmd.ExecuteNonQuery()
            MsgBox("Pembayaran Tindakan berhasil dilakukan", MessageBoxIcon.Information)
        Catch ex As Exception
            MsgBox("Pembayaran Tindakan All gagal dilakukan.", MessageBoxIcon.Error)
        End Try

        conn.Close()
    End Sub

    Sub LunasTindakan(noTindakan As String)
        Dim dt As String
        dt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")

        Dim str As String = ""
        Select Case unit
            Case "IG"
                str = "UPDATE t_tindakanpasienrajal 
                          SET statusPembayaran = 'LUNAS',
                              tglPembayaran = '" & dt & "'
                        WHERE noTindakanPasienRajal = '" & noTindakan & "'"
            Case "RJ"
                str = "UPDATE t_tindakanpasienrajal 
                          SET statusPembayaran = 'LUNAS',
                              tglPembayaran = '" & dt & "'
                        WHERE noTindakanPasienRajal = '" & noTindakan & "'"
            Case "RI"
                str = "UPDATE t_tindakanpasienranap 
                          SET statusPembayaran = 'LUNAS',
                              tglPembayaran = '" & dt & "'
                        WHERE noTindakanPasienRanap = '" & noTindakan & "'"
        End Select

        Call koneksiServer()
        Dim cmd As MySqlCommand
        Try
            cmd = New MySqlCommand(str, conn)
            cmd.ExecuteNonQuery()
            MsgBox("Pembayaran Tindakan berhasil dilakukan", MessageBoxIcon.Information)
        Catch ex As Exception
            MsgBox("Pembayaran gagal dilakukan.", MessageBoxIcon.Error)
        End Try

        conn.Close()
    End Sub
#End Region
#Region "Laboratorium"
    Sub totalTarifLab()
        Dim totBayar As Long
        totBayar = 0
        For i As Integer = 0 To dgvLab.Rows.Count - 1
            totBayar = totBayar + Val(dgvLab.Rows(i).Cells(2).Value)
        Next
        txtTotalLab.Text = totBayar.ToString("#,##0")
    End Sub

    Sub totalTarifDetailLab()
        Dim totBayar As Long
        totBayar = 0
        For i As Integer = 0 To dgvDetailLab.Rows.Count - 1
            totBayar = totBayar + Val(dgvDetailLab.Rows(i).Cells(4).Value)
        Next
        txtPerTotalLab.Text = totBayar.ToString("#,##0")
    End Sub

    Sub DaftarLab()
        Call koneksiServer()
        Dim query As String = ""
        Dim cmd As MySqlCommand
        Dim dr As MySqlDataReader

        Select Case unit
            Case "IG"
                query = "SELECT noRegistrasiPenunjangRajal,tglMasukPenunjangRajal,
                                totalTindakanPenunjangRajal,statusPembayaran,
                                noTindakanPenunjangRajal
                           FROM vw_pasienpenunjangrajal
                          WHERE noDaftar = '" & txtNoReg.Text & "'
                            AND (totalTindakanPenunjangRajal IS NOT NULL AND totalTindakanPenunjangRajal != 0)
                          UNION ALL
                         SELECT noRegistrasiPARajal,tglMasukPARajal,
                                totalTindakanPA,statusPembayaran,
                                noTindakanPARajal
                           FROM vw_pasienpatologirajal
                          WHERE noDaftar = '" & txtNoReg.Text & "'
                            AND (totalTindakanPA IS NOT NULL AND totalTindakanPA != 0)  
                          UNION ALL
                        SELECT noRegistrasiBDRS,tglMasukBDRS,
			                   totalTindakanBDRS,statusPembayaran,
			                   noTindakanBDRS
                          FROM vw_pasienbdrsrajal
                         WHERE noDaftar = '" & txtNoReg.Text & "'
	                       AND (totalTindakanBDRS IS NOT NULL AND totalTindakanBDRS != 0)
                       ORDER BY tglMasukPenunjangRajal ASC"
            Case "RJ"
                query = "SELECT noRegistrasiPenunjangRajal,tglMasukPenunjangRajal,
                                totalTindakanPenunjangRajal,statusPembayaran,
                                noTindakanPenunjangRajal
                           FROM vw_pasienpenunjangrajal
                          WHERE noDaftar = '" & txtNoReg.Text & "'
                            AND (totalTindakanPenunjangRajal IS NOT NULL AND totalTindakanPenunjangRajal != 0)
                          UNION ALL
                         SELECT noRegistrasiPARajal,tglMasukPARajal,
                                totalTindakanPA,statusPembayaran,
                                noTindakanPARajal
                           FROM vw_pasienpatologirajal
                          WHERE noDaftar = '" & txtNoReg.Text & "'
                            AND (totalTindakanPA IS NOT NULL AND totalTindakanPA != 0)   
                          UNION ALL
                         SELECT noRegistrasiBDRS,tglMasukBDRS,
			                    totalTindakanBDRS,statusPembayaran,
			                    noTindakanBDRS
                           FROM vw_pasienbdrsrajal
                          WHERE noDaftar = '" & txtNoReg.Text & "'
	                        AND (totalTindakanBDRS IS NOT NULL AND totalTindakanBDRS != 0)
                       ORDER BY tglMasukPenunjangRajal ASC"
            Case "RI"
                query = "SELECT noRegistrasiPenunjangRanap,tglMasukPenunjangRanap,
                                totalTindakanPenunjangRanap,statusPembayaran,
                                noTindakanPenunjangRanap
                           FROM vw_pasienpenunjangranap
                          WHERE noDaftar = '" & txtNoReg.Text & "'
                            AND (totalTindakanPenunjangRanap IS NOT NULL AND totalTindakanPenunjangRanap != 0)
                          UNION ALL
                         SELECT noRegistrasiPARanap,tglMasukPARanap,
                                totalTindakanPA,statusPembayaran,
                                noTindakanPARanap
                           FROM vw_pasienpatologiranap
                          WHERE noDaftar = '" & txtNoReg.Text & "'
                            AND (totalTindakanPA IS NOT NULL AND totalTindakanPA != 0)
                          UNION ALL
                         SELECT noRegistrasiBDRS,tglMasukBDRS,
			                    totalTindakanBDRS,statusPembayaran,
			                    noTindakanBDRS
                           FROM vw_pasienbdrsranap
                          WHERE noDaftar = '" & txtNoReg.Text & "'
	                        AND (totalTindakanBDRS IS NOT NULL AND totalTindakanBDRS != 0)
                       ORDER BY tglMasukPenunjangRanap ASC"
        End Select

        Try
            cmd = New MySqlCommand(query, conn)
            dr = cmd.ExecuteReader
            dgvLab.Rows.Clear()
            Select Case unit
                Case "IG"
                    Do While dr.Read
                        dgvLab.Rows.Add(dr.Item("noRegistrasiPenunjangRajal"), dr.Item("tglMasukPenunjangRajal"),
                            dr.Item("totalTindakanPenunjangRajal"), dr.Item("statusPembayaran"), dr.Item("noTindakanPenunjangRajal"))
                    Loop
                Case "RJ"
                    Do While dr.Read
                        dgvLab.Rows.Add(dr.Item("noRegistrasiPenunjangRajal"), dr.Item("tglMasukPenunjangRajal"),
                            dr.Item("totalTindakanPenunjangRajal"), dr.Item("statusPembayaran"), dr.Item("noTindakanPenunjangRajal"))
                    Loop
                Case "RI"
                    Do While dr.Read
                        dgvLab.Rows.Add(dr.Item("noRegistrasiPenunjangRanap"), dr.Item("tglMasukPenunjangRanap"),
                            dr.Item("totalTindakanPenunjangRanap"), dr.Item("statusPembayaran"), dr.Item("noTindakanPenunjangRanap"))
                    Loop
            End Select

            dr.Close()
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
        conn.Close()
    End Sub

    Sub DetailLab(noTindakan As String)
        Call koneksiServer()
        Dim query As String = ""
        Dim cmd As MySqlCommand
        Dim dr As MySqlDataReader

        Select Case unit
            Case "IG"
                query = "CALL datadetaillabrajal('" & noTindakan & "')"
            Case "RJ"
                query = "CALL datadetaillabrajal('" & noTindakan & "')"
            Case "RI"
                query = "CALL datadetaillabranap('" & noTindakan & "')"
        End Select

        Try
            cmd = New MySqlCommand(query, conn)
            dr = cmd.ExecuteReader
            dgvDetailLab.Rows.Clear()
            Do While dr.Read
                dgvDetailLab.Rows.Add(dr.Item("kdTarif"), dr.Item("tindakan"), dr.Item("tarif"),
                                      dr.Item("jumlahTindakan"), dr.Item("totalTarif"))

            Loop
            dr.Close()
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
        conn.Close()
    End Sub

    Sub LunasLabAll(noTindakan As String)
        Dim dt As String
        dt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")

        Dim str As String = ""
        Select Case unit
            Case "IG"
                str = "UPDATE t_tindakanpenunjangrajal 
                          SET statusPembayaran = 'LUNAS',
                              tglPembayaran = '" & dt & "'
                        WHERE noTindakanPenunjangRajal IN (" & noTindakan & ")"
            Case "RJ"
                str = "UPDATE t_tindakanpenunjangrajal 
                          SET statusPembayaran = 'LUNAS',
                              tglPembayaran = '" & dt & "'
                        WHERE noTindakanPenunjangRajal IN (" & noTindakan & ")"
            Case "RI"
                str = "UPDATE t_tindakanpenunjangranap 
                          SET statusPembayaran = 'LUNAS',
                              tglPembayaran = '" & dt & "'
                        WHERE noTindakanPenunjangRanap IN (" & noTindakan & ")"
        End Select
        Call koneksiServer()
        Dim cmd As MySqlCommand
        Try
            cmd = New MySqlCommand(str, conn)
            cmd.ExecuteNonQuery()
            MsgBox("Pembayaran Laboratorium berhasil dilakukan", MessageBoxIcon.Information)
        Catch ex As Exception
            MsgBox("Pembayaran Laboratorium All gagal dilakukan.", MessageBoxIcon.Error)
        End Try

        conn.Close()
    End Sub

    Sub LunasLab(noTindakan As String)
        Dim dt As String
        dt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")

        Dim str As String = ""
        Select Case unit
            Case "IG"
                Select Case lab
                    Case "TLA"
                        str = "UPDATE t_tindakanpenunjangrajal 
                                  SET statusPembayaran = 'LUNAS',
                                      tglPembayaran = '" & dt & "'
                                WHERE noTindakanPenunjangRajal = '" & noTindakan & "'"
                    Case "TPA"
                        str = "UPDATE t_tindakanpatologirajal 
                                  SET statusPembayaran = 'LUNAS',
                                      tglPembayaran = '" & dt & "'
                                WHERE noTindakanPARajal = '" & noTindakan & "'"
                    Case "TBD"
                        str = "UPDATE t_tindakanbdrs 
                                  SET statusPembayaran = 'LUNAS',
                                      tglPembayaran = '" & dt & "'
                                WHERE noTindakanBDRS = '" & noTindakan & "'"
                End Select
            Case "RJ"
                Select Case lab
                    Case "TLA"
                        str = "UPDATE t_tindakanpenunjangrajal 
                                  SET statusPembayaran = 'LUNAS',
                                      tglPembayaran = '" & dt & "'
                                WHERE noTindakanPenunjangRajal = '" & noTindakan & "'"
                    Case "TPA"
                        str = "UPDATE t_tindakanpatologirajal 
                                  SET statusPembayaran = 'LUNAS',
                                      tglPembayaran = '" & dt & "'
                                WHERE noTindakanPARajal = '" & noTindakan & "'"
                    Case "TBD"
                        str = "UPDATE t_tindakanbdrs 
                                  SET statusPembayaran = 'LUNAS',
                                      tglPembayaran = '" & dt & "'
                                WHERE noTindakanBDRS = '" & noTindakan & "'"
                End Select
            Case "RI"
                Select Case lab
                    Case "TLA"
                        str = "UPDATE t_tindakanpenunjangranap 
                                  SET statusPembayaran = 'LUNAS',
                                      tglPembayaran = '" & dt & "'
                                WHERE noTindakanPenunjangRanap = '" & noTindakan & "'"
                    Case "TPA"
                        str = "UPDATE t_tindakanpatologiranap 
                                  SET statusPembayaran = 'LUNAS',
                                      tglPembayaran = '" & dt & "'
                                WHERE noTindakanPARanap = '" & noTindakan & "'"
                    Case "TBD"
                        str = "UPDATE t_tindakanbdrs 
                                  SET statusPembayaran = 'LUNAS',
                                      tglPembayaran = '" & dt & "'
                                WHERE noTindakanBDRS = '" & noTindakan & "'"
                End Select
        End Select

        Call koneksiServer()
        Dim cmd As MySqlCommand
        Try
            cmd = New MySqlCommand(str, conn)
            cmd.ExecuteNonQuery()
            MsgBox("Pembayaran Laboratorium berhasil dilakukan", MessageBoxIcon.Information)
        Catch ex As Exception
            MsgBox("Pembayaran gagal dilakukan.", MessageBoxIcon.Error)
        End Try

        conn.Close()
    End Sub

#End Region
#Region "Radiologi"
    Sub totalTarifRad()
        Dim totBayar As Long
        totBayar = 0
        For i As Integer = 0 To dgvRad.Rows.Count - 1
            totBayar = totBayar + Val(dgvRad.Rows(i).Cells(2).Value)
        Next
        txtTotalRad.Text = totBayar.ToString("#,##0")
    End Sub

    Sub totalTarifDetailRad()
        Dim totBayar As Long
        totBayar = 0
        For i As Integer = 0 To dgvDetailRad.Rows.Count - 1
            totBayar = totBayar + Val(dgvDetailRad.Rows(i).Cells(4).Value)
        Next
        txtPerTotalRad.Text = totBayar.ToString("#,##0")
    End Sub

    Sub DaftarRad()
        Call koneksiServer()
        Dim query As String = ""
        Dim cmd As MySqlCommand
        Dim dr As MySqlDataReader
        Select Case unit
            Case "IG"
                query = "CALL tindakanpasienradrajal('" & txtNoReg.Text & "')"
            Case "RJ"
                query = "CALL tindakanpasienradrajal('" & txtNoReg.Text & "')"
            Case "RI"
                query = "CALL tindakanpasienradranap('" & txtNoReg.Text & "')"
        End Select

        Try
            cmd = New MySqlCommand(query, conn)
            dr = cmd.ExecuteReader
            dgvRad.Rows.Clear()
            Select Case unit
                Case "IG"
                    Do While dr.Read
                        dgvRad.Rows.Add(dr.Item("noRegistrasiRadiologiRajal"), dr.Item("tglMasukRadiologiRajal"),
                               dr.Item("totalTindakanRadiologiRajal"), dr.Item("statusPembayaran"), dr.Item("noTindakanRadiologiRajal"))
                    Loop
                Case "RJ"
                    Do While dr.Read
                        dgvRad.Rows.Add(dr.Item("noRegistrasiRadiologiRajal"), dr.Item("tglMasukRadiologiRajal"),
                               dr.Item("totalTindakanRadiologiRajal"), dr.Item("statusPembayaran"), dr.Item("noTindakanRadiologiRajal"))
                    Loop
                Case "RI"
                    Do While dr.Read
                        dgvRad.Rows.Add(dr.Item("noRegistrasiRadiologiRanap"), dr.Item("tglMasukRadiologiRanap"),
                               dr.Item("totalTindakanRadiologiRanap"), dr.Item("statusPembayaran"), dr.Item("noTindakanRadiologiRanap"))
                    Loop
            End Select

            dr.Close()
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
        conn.Close()
    End Sub

    Sub DetailRad(noTindakan As String)
        Call koneksiServer()
        Dim query As String = ""
        Dim cmd As MySqlCommand
        Dim dr As MySqlDataReader
        Select Case unit
            Case "IG"
                query = "CALL datadetailradrajal('" & noTindakan & "')"
            Case "RJ"
                query = "CALL datadetailradrajal('" & noTindakan & "')"
            Case "RI"
                query = "CALL datadetailradranap('" & noTindakan & "')"
        End Select

        Try
            cmd = New MySqlCommand(query, conn)
            dr = cmd.ExecuteReader
            dgvDetailRad.Rows.Clear()
            Do While dr.Read
                dgvDetailRad.Rows.Add(dr.Item("kdTarif"), dr.Item("tindakan"), dr.Item("tarif"),
                                      dr.Item("jumlahTindakan"), dr.Item("totalTarif"))

            Loop
            dr.Close()
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
        conn.Close()
    End Sub

    Sub LunasRadAll(noTindakan As String)
        Dim dt As String
        dt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")

        Dim str As String = ""
        Select Case unit
            Case "IG"
                str = "UPDATE t_tindakanradiologirajal 
                          SET statusPembayaran = 'LUNAS',
                              tglPembayaran = '" & dt & "'
                        WHERE noTindakanRadiologiRajal IN (" & noTindakan & ")"
            Case "RJ"
                str = "UPDATE t_tindakanradiologirajal 
                          SET statusPembayaran = 'LUNAS',
                              tglPembayaran = '" & dt & "'
                        WHERE noTindakanRadiologiRajal IN (" & noTindakan & ")"
            Case "RI"
                str = "UPDATE t_tindakanradiologiranap 
                          SET statusPembayaran = 'LUNAS',
                              tglPembayaran = '" & dt & "'
                        WHERE noTindakanRadiologiRanap IN (" & noTindakan & ")"
        End Select
        Call koneksiServer()
        Dim cmd As MySqlCommand
        Try
            cmd = New MySqlCommand(str, conn)
            cmd.ExecuteNonQuery()
            MsgBox("Pembayaran Radiologi berhasil dilakukan", MessageBoxIcon.Information)
        Catch ex As Exception
            MsgBox("Pembayaran Radiologi All gagal dilakukan.", MessageBoxIcon.Error)
        End Try

        conn.Close()
    End Sub

    Sub LunasRad(noTindakan As String)
        Dim dt As String
        dt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")

        Dim str As String = ""
        Select Case unit
            Case "IG"
                str = "UPDATE t_tindakanradiologirajal 
                          SET statusPembayaran = 'LUNAS',
                              tglPembayaran = '" & dt & "'
                        WHERE noTindakanRadiologiRajal = '" & noTindakan & "'"
            Case "RJ"
                str = "UPDATE t_tindakanradiologirajal 
                          SET statusPembayaran = 'LUNAS',
                              tglPembayaran = '" & dt & "'
                        WHERE noTindakanRadiologiRajal = '" & noTindakan & "'"
            Case "RI"
                str = "UPDATE t_tindakanradiologiranap 
                          SET statusPembayaran = 'LUNAS',
                              tglPembayaran = '" & dt & "'
                        WHERE noTindakanRadiologiRanap = '" & noTindakan & "'"
        End Select

        Call koneksiServer()
        Dim cmd As MySqlCommand
        Try
            cmd = New MySqlCommand(str, conn)
            cmd.ExecuteNonQuery()
            MsgBox("Pembayaran Radiologi berhasil dilakukan", MessageBoxIcon.Information)
        Catch ex As Exception
            MsgBox("Pembayaran gagal dilakukan.", MessageBoxIcon.Error)
        End Try

        conn.Close()
    End Sub
#End Region
#Region "Obat"
    Sub totalTarifObat()
        Dim totBayar As Long
        totBayar = 0
        For i As Integer = 0 To dgvObat.Rows.Count - 1
            totBayar = totBayar + Val(dgvObat.Rows(i).Cells(3).Value)
        Next
        txtTotalObat.Text = (Math.Ceiling(totBayar / 100) * 100).ToString("#,##0")
    End Sub

    Sub totalTarifDetailObat()
        Dim totBayar As Long
        totBayar = 0
        For i As Integer = 0 To dgvDetailObat.Rows.Count - 1
            totBayar = totBayar + Val(dgvDetailObat.Rows(i).Cells(5).Value)
        Next
        txtPerTotalObat.Text = (Math.Ceiling(totBayar / 100) * 100).ToString("#,##0")
    End Sub

    Sub DaftarObat()
        Call koneksiServer()
        Dim query As String = ""
        Dim cmd As MySqlCommand
        Dim dr As MySqlDataReader

        Select Case unit
            Case "IG"
                query = "SELECT * FROM vw_penjualanobatigd WHERE noDaftar = '" & txtNoReg.Text & "' ORDER BY tglPenjualanObatIGD ASC"
            Case "RJ"
                query = "SELECT * FROM vw_penjualanobatrajal WHERE noDaftar = '" & txtNoReg.Text & "' ORDER BY tglPenjualanObatRajal ASC"
            Case "RI"
                query = "SELECT * FROM vw_penjualanobatranap WHERE noDaftar = '" & txtNoReg.Text & "' ORDER BY tglPenjualanObatRanap ASC"
        End Select

        Try
            cmd = New MySqlCommand(query, conn)
            dr = cmd.ExecuteReader
            dgvObat.Rows.Clear()
            Select Case unit
                Case "IG"
                    Do While dr.Read
                        dgvObat.Rows.Add(dr.Item("noDaftar"), dr.Item("noPenjualanObatIGD"),
                               dr.Item("tglPenjualanObatIGD"), dr.Item("totalAkhirPenjualanIGD"), dr.Item("statusPembayaran"))
                    Loop
                Case "RJ"
                    Do While dr.Read
                        dgvObat.Rows.Add(dr.Item("noDaftar"), dr.Item("noPenjualanObatRajal"),
                               dr.Item("tglPenjualanObatRajal"), dr.Item("totalAkhirPenjualanRajal"), dr.Item("statusPembayaran"))
                    Loop
                Case "RI"
                    Do While dr.Read
                        dgvObat.Rows.Add(dr.Item("noDaftar"), dr.Item("noPenjualanObatRanap"),
                               dr.Item("tglPenjualanObatRanap"), dr.Item("totalAkhirPenjualanRanap"), dr.Item("statusPembayaran"))
                    Loop
            End Select

            dr.Close()
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
        conn.Close()
    End Sub

    Sub DetailObat(noTransaksi As String)
        Call koneksiServer()
        Dim query As String = ""
        Dim cmd As MySqlCommand
        Dim dr As MySqlDataReader

        query = "CALL detailpenjualanobat ('" & noTransaksi & "')"
        Try
            cmd = New MySqlCommand(query, conn)
            dr = cmd.ExecuteReader
            dgvDetailObat.Rows.Clear()

            Do While dr.Read
                dgvDetailObat.Rows.Add(dr.Item("noPenjualanObatIGD"), dr.Item("kdObat"),
                                       dr.Item("namaObat"), dr.Item("harga"), dr.Item("JumlahItem"),
                                       dr.Item("subTotalPenjualan"))
            Loop
            dr.Close()
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
        conn.Close()
    End Sub

    Sub LunasObatAll(noTransaksi As String)
        Dim dt As String
        dt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")

        Dim str As String = ""
        Select Case unit
            Case "IG"
                str = "UPDATE t_penjualanobatigd 
                          SET statusPembayaran = 'LUNAS',
                              tglPembayaran = '" & dt & "'
                        WHERE noPenjualanObatIGD IN (" & noTransaksi & ")"
            Case "RJ"
                str = "UPDATE t_penjualanobatrajal
                          SET statusPembayaran = 'LUNAS',
                              tglPembayaran = '" & dt & "'
                        WHERE noPenjualanObatRajal IN (" & noTransaksi & ")"
            Case "RI"
                str = "UPDATE t_penjualanobatranap 
                          SET statusPembayaran = 'LUNAS',
                              tglPembayaran = '" & dt & "'
                        WHERE noPenjualanObatRanap IN (" & noTransaksi & ")"
        End Select
        Call koneksiFarmasi()
        Dim cmd As MySqlCommand
        Try
            cmd = New MySqlCommand(str, conn)
            cmd.ExecuteNonQuery()
            MsgBox("Pembayaran Obat berhasil dilakukan", MessageBoxIcon.Information)
        Catch ex As Exception
            MsgBox("Pembayaran Obat All gagal dilakukan.", MessageBoxIcon.Error)
        End Try

        conn.Close()
    End Sub

    Sub LunasObat(noTransaksi As String)
        Dim dt As String
        dt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")

        Dim str As String = ""
        Select Case unit
            Case "IG"
                Select Case obt
                    Case "GD"
                        str = "UPDATE t_penjualanobatigd 
                                  SET statusPembayaran = 'LUNAS',
                                      tglPembayaran = '" & dt & "'
                                WHERE noPenjualanObatIGD = '" & noTransaksi & "'"
                    Case "RI"
                        str = "UPDATE t_penjualanobatranap 
                                  SET statusPembayaran = 'LUNAS',
                                      tglPembayaran = '" & dt & "'
                                WHERE noPenjualanObatRanap = '" & noTransaksi & "'"
                End Select
            Case "RJ"
                str = "UPDATE t_penjualanobatrajal
                          SET statusPembayaran = 'LUNAS',
                              tglPembayaran = '" & dt & "'
                        WHERE noPenjualanObatRajal = '" & noTransaksi & "'"
            Case "RI"
                str = "UPDATE t_penjualanobatranap 
                          SET statusPembayaran = 'LUNAS',
                              tglPembayaran = '" & dt & "'
                        WHERE noPenjualanObatRanap = '" & noTransaksi & "'"
        End Select

        Call koneksiFarmasi()
        Dim cmd As MySqlCommand
        Try
            cmd = New MySqlCommand(str, conn)
            cmd.ExecuteNonQuery()
            MsgBox("Pembayaran Obat berhasil dilakukan", MessageBoxIcon.Information)
        Catch ex As Exception
            MsgBox("Pembayaran gagal dilakukan.", MessageBoxIcon.Error)
        End Try

        conn.Close()
    End Sub
#End Region
#Region "ALL"
    Sub totalTarifAll()
        Dim totBiayaRuang As Double = 0
        Dim totTindakan As Double = 0
        Dim totBayar As Double = 0
        Select Case unit
            Case "IG"
                totTindakan = 0
                totBayar = 0
                For i As Integer = 0 To dgvAll.Rows.Count - 1
                    totTindakan = totTindakan + Val(dgvAll.Rows(i).Cells(3).Value)
                Next
                txtTotalAll.Text = totTindakan.ToString("#,##0")
                txtBiayaTindakan.Text = totTindakan.ToString("#,##0")

                totBiayaRuang = Val(CInt(txtKarBiaya.Text) + CInt(txtKarKonsul.Text))
                txtBiayaRuang.Text = totBiayaRuang.ToString("#,##0")
                txtBiayaRuang2.Text = totBiayaRuang

                totBayar = totBiayaRuang + totTindakan
                txtTotalBayar.Text = totBayar.ToString("#,##0")
            Case "RJ"
            Case "RI"
                totTindakan = 0
                totBayar = 0
                For i As Integer = 0 To dgvAll.Rows.Count - 1
                    totTindakan = totTindakan + Val(dgvAll.Rows(i).Cells(3).Value) 
                Next
                txtTotalAll.Text = totTindakan.ToString("#,##0")
                txtBiayaTindakan.Text = totTindakan.ToString("#,##0")

                'totBiayaRuang = Val(txtJumInap.Text * txtTarifKmr2.Text)
                totBiayaRuang = Val(txtTotalTarifKmr2.Text) + Val(txtTarifDPJP.Text)
                txtBiayaRuang.Text = totBiayaRuang.ToString("#,##0")
                'txtBiayaRuang2.Text = Val(txtJumInap.Text * txtTarifKmr2.Text)
                txtBiayaRuang2.Text = Val(txtTotalTarifKmr2.Text) + Val(txtTarifDPJP.Text)

                totBayar = totBiayaRuang + totTindakan
                txtTotalBayar.Text = totBayar.ToString("#,##0")
        End Select


        'txtTotal2.Text = totBayar.ToString("#,##0")
    End Sub

    Sub DaftarAll()
        Call koneksiServer()
        Dim query As String = ""
        Dim cmd As MySqlCommand
        Dim dr As MySqlDataReader

        query = "CALL allnotindakankasir('" & txtNoReg.Text & "')"

        Try
            cmd = New MySqlCommand(query, conn)
            dr = cmd.ExecuteReader
            dgvAll.Rows.Clear()
            Do While dr.Read
                dgvAll.Rows.Add(dr.Item("noRegistrasiRawatJalan"), dr.Item("tglTindakan"), dr.Item("unit"),
                                     dr.Item("totalTarifTindakan"), dr.Item("statusPembayaran"), dr.Item("noTindakanPasienRajal"))
            Loop

            dr.Close()
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
        conn.Close()
    End Sub

    Sub DetailAll(noTindakan As String)
        Call koneksiServer()
        Dim query As String = ""
        Dim cmd As MySqlCommand
        Dim dr As MySqlDataReader

        Select Case unit
            Case "IG"
                query = "CALL alldetailrajalkasir('" & noTindakan & "')"
            Case "RJ"
                query = "CALL alldetailrajalkasir('" & noTindakan & "')"
            Case "RI"
                query = "CALL alldetailranapkasir('" & noTindakan & "')"
        End Select

        Try
            cmd = New MySqlCommand(query, conn)
            dr = cmd.ExecuteReader
            dgvDetailAll.Rows.Clear()
            Do While dr.Read
                dgvDetailAll.Rows.Add(dr.Item("kdTarif"), dr.Item("tindakan"), dr.Item("jumlahTindakan"),
                               dr.Item("tarif"), dr.Item("DPJP"), dr.Item("PPA"), dr.Item("totalTarif"))
            Loop
            dr.Close()
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
        conn.Close()
    End Sub

    Sub LunasAllTindak(noTindakan As String)
        Dim dt As String
        dt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")

        Dim str As String = ""

        Select Case unit
            Case "IG"
                str = "UPDATE t_tindakanpasienrajal 
                          SET statusPembayaran = 'LUNAS',
                              tglPembayaran = '" & dt & "'
                        WHERE noTindakanPasienRajal IN (" & noTindakan & ")"
            Case "RJ"
            Case "RI"
                str = "UPDATE t_tindakanpasienranap 
                          SET statusPembayaran = 'LUNAS',
                              tglPembayaran = '" & dt & "'
                        WHERE noTindakanPasienRanap IN (" & noTindakan & ")"
        End Select

        Call koneksiServer()
        Dim cmd As MySqlCommand
        Try
            cmd = New MySqlCommand(str, conn)
            cmd.ExecuteNonQuery()
            'MsgBox("Pembayaran Tindakan berhasil dilakukan", MessageBoxIcon.Information)
        Catch ex As Exception
            MsgBox("Pembayaran Tindakan gagal dilakukan.", MessageBoxIcon.Error)
        End Try

        conn.Close()
    End Sub

    Sub LunasAllLab(noTindakan As String)
        Dim dt As String
        dt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")

        Dim str As String = ""

        Select Case unit
            Case "IG"
                Select Case lab
                    Case "TLA"
                        str = "UPDATE t_tindakanpenunjangrajal 
                                  SET statusPembayaran = 'LUNAS',
                                      tglPembayaran = '" & dt & "'
                                WHERE noTindakanPenunjangRajal IN (" & noTindakan & ")"
                    Case "TPA"
                        str = "UPDATE t_tindakanpatologirajal 
                                  SET statusPembayaran = 'LUNAS',
                                      tglPembayaran = '" & dt & "'
                                WHERE noTindakanPARajal IN (" & noTindakan & ")"
                    Case "TBD"
                        str = "UPDATE t_tindakanbdrs 
                                  SET statusPembayaran = 'LUNAS',
                                      tglPembayaran = '" & dt & "'
                                WHERE noTindakanBDRS IN (" & noTindakan & ")"
                End Select
            Case "RJ"
            Case "RI"
                Select Case lab
                    Case "TLA"
                        str = "UPDATE t_tindakanpenunjangranap 
                                  SET statusPembayaran = 'LUNAS',
                                      tglPembayaran = '" & dt & "'
                                WHERE noTindakanPenunjangRanap IN (" & noTindakan & ")"
                    Case "TPA"
                        str = "UPDATE t_tindakanpatologiranap 
                                  SET statusPembayaran = 'LUNAS',
                                      tglPembayaran = '" & dt & "'
                                WHERE noTindakanPARanap IN (" & noTindakan & ")"
                    Case "TBD"
                        str = "UPDATE t_tindakanbdrs 
                                  SET statusPembayaran = 'LUNAS',
                                      tglPembayaran = '" & dt & "'
                                WHERE noTindakanBDRS IN (" & noTindakan & ")"
                End Select
        End Select
        Call koneksiServer()
        Dim cmd As MySqlCommand
        Try
            cmd = New MySqlCommand(str, conn)
            cmd.ExecuteNonQuery()
            'MsgBox("Pembayaran Laboratorium berhasil dilakukan", MessageBoxIcon.Information)
        Catch ex As Exception
            MsgBox("Pembayaran Laboratorium gagal dilakukan.", MessageBoxIcon.Error)
        End Try

        conn.Close()
    End Sub

    Sub LunasAllRad(noTindakan As String)
        Dim dt As String
        dt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")

        Dim str As String = ""

        Select Case unit
            Case "IG"
                str = "UPDATE t_tindakanradiologirajal 
                          SET statusPembayaran = 'LUNAS',
                              tglPembayaran = '" & dt & "'
                        WHERE noTindakanRadiologiRajal IN (" & noTindakan & ")"
            Case "RJ"
            Case "RI"
                str = "UPDATE t_tindakanradiologiranap 
                          SET statusPembayaran = 'LUNAS',
                              tglPembayaran = '" & dt & "'
                        WHERE noTindakanRadiologiRanap IN (" & noTindakan & ")"
        End Select

        Call koneksiServer()
        Dim cmd As MySqlCommand
        Try
            cmd = New MySqlCommand(str, conn)
            cmd.ExecuteNonQuery()
            'MsgBox("Pembayaran Radiologi berhasil dilakukan", MessageBoxIcon.Information)
        Catch ex As Exception
            MsgBox("Pembayaran Radiologi gagal dilakukan.", MessageBoxIcon.Error)
        End Try

        conn.Close()
    End Sub

    Sub LunasAllOKnParu(noTindakan As String)
        Dim dt As String
        dt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
        Dim str As String = ""

        Select Case unit
            Case "IG"
                If noTindakan.Contains("TOP") Then
                    str = "UPDATE t_tindakanop 
                      SET statusPembayaran = 'LUNAS',
                          tglPembayaran = '" & dt & "'
                    WHERE noTindakanOP IN (" & noTindakan & ")"
                ElseIf noTindakan.Contains("TOKP") Then
                    str = "UPDATE t_tindakanokparu 
                      SET statusPembayaran = 'LUNAS',
                          tglPembayaran = '" & dt & "'
                    WHERE noTindakanOP IN (" & noTindakan & ")"
                End If
            Case "RJ"
            Case "RI"
                If noTindakan.Contains("TOP") Then
                    str = "UPDATE t_tindakanop 
                      SET statusPembayaran = 'LUNAS',
                          tglPembayaran = '" & dt & "'
                    WHERE noTindakanOP IN (" & noTindakan & ")"
                ElseIf noTindakan.Contains("TOKP") Then
                    str = "UPDATE t_tindakanokparu 
                      SET statusPembayaran = 'LUNAS',
                          tglPembayaran = '" & dt & "'
                    WHERE noTindakanOP IN (" & noTindakan & ")"
                End If
        End Select

        Call koneksiServer()
        Dim cmd As MySqlCommand
        Try
            cmd = New MySqlCommand(str, conn)
            cmd.ExecuteNonQuery()
            'MsgBox("Pembayaran Radiologi berhasil dilakukan", MessageBoxIcon.Information)
        Catch ex As Exception
            MsgBox("Pembayaran Operasi gagal dilakukan.", MessageBoxIcon.Error)
        End Try

        conn.Close()
    End Sub

    Sub LunasAllHemo(noTindakan As String)
        Dim dt As String
        dt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
        Dim str As String = ""

        Select Case unit
            Case "IG"
                str = "UPDATE t_tindakanhd
                          SET statusPembayaran = 'LUNAS',
                              tglPembayaran = '" & dt & "'
                        WHERE noTindakanHD IN (" & noTindakan & ")"
            Case "RJ"
            Case "RI"
                str = "UPDATE t_tindakanhd
                          SET statusPembayaran = 'LUNAS',
                              tglPembayaran = '" & dt & "'
                        WHERE noTindakanHD IN (" & noTindakan & ")"
        End Select

        Call koneksiServer()
        Dim cmd As MySqlCommand
        Try
            cmd = New MySqlCommand(str, conn)
            cmd.ExecuteNonQuery()
            'MsgBox("Pembayaran Radiologi berhasil dilakukan", MessageBoxIcon.Information)
        Catch ex As Exception
            MsgBox("Pembayaran Hemodialisa gagal dilakukan.", MessageBoxIcon.Error)
        End Try

        conn.Close()
    End Sub
#End Region
#Region "Karcis"
    Sub totalTarifKarcis()
        Dim karcis As Long = 0
        Dim konsul As Long = 0
        Dim totBayar As Long = 0
        karcis = txtKarBiaya.Text
        konsul = txtKarKonsul.Text
        totBayar = Val(CInt(karcis) + CInt(konsul))
        txtKarBiaya.Text = karcis.ToString("#,##0")
        txtKarKonsul.Text = konsul.ToString("#,##0")
        txtKarTotal.Text = totBayar.ToString("#,##0")
    End Sub

    Sub LunasKarcis(noRegRajal As String)
        Dim dt As String
        dt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")

        Dim str As String = ""
        Select Case unit
            Case "IG"
                str = "UPDATE t_registrasirawatjalan 
                          SET statusPembayaran = 'LUNAS',
                              tglPembayaran = '" & dt & "'
                        WHERE noRegistrasiRawatJalan = '" & noRegRajal & "'"
            Case "RJ"
                str = "UPDATE t_registrasirawatjalan 
                          SET statusPembayaran = 'LUNAS',
                              tglPembayaran = '" & dt & "'
                        WHERE noRegistrasiRawatJalan = '" & noRegRajal & "'"
            Case "RI"
        End Select


        Call koneksiServer()
        Dim cmd As MySqlCommand
        Try
            cmd = New MySqlCommand(str, conn)
            cmd.ExecuteNonQuery()
            MsgBox("Pembayaran Tindakan berhasil dilakukan", MessageBoxIcon.Information)
        Catch ex As Exception
            MsgBox("Pembayaran gagal dilakukan.", MessageBoxIcon.Error)
        End Try

        conn.Close()
    End Sub

    Sub LunasJatim(noVA As String, noRef As String)
        Call koneksiServer()
        Try
            Dim str As String
            str = "INSERT INTO t_vabankjatim(noDaftar,noRekamedis,noVASementara,totalBayar,noReferensi,tglBayar) 
                                     VALUES ('" & txtNoReg.Text & "','" & txtRekMed.Text & "','" & noVA & "',
                                            '" & txtTotalBayar.Text & "','" & noRef & "', '" & dtBank & "')"
            cmd = New MySqlCommand(str, conn)
            cmd.ExecuteNonQuery()
            MsgBox("Insert data VA berhasil dilakukan", MsgBoxStyle.Information, "Information")
        Catch ex As Exception
            MsgBox("Insert data VA gagal dilakukan.", MsgBoxStyle.Critical, "Error")
        End Try
        conn.Close()
    End Sub

    Sub LoadKarcis(noRegRajal As String)
        Dim dt As String
        dt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")

        Call koneksiServer()
        Dim query As String = ""
        Dim cmd As MySqlCommand
        Dim da As MySqlDataAdapter
        Select Case unit
            Case "IG"
                query = "SELECT statusPembayaran 
                         FROM t_registrasirawatjalan
                        WHERE noRegistrasiRawatJalan = '" & noRegRajal & "'"
            Case "RJ"
                query = "SELECT statusPembayaran 
                         FROM t_registrasirawatjalan
                        WHERE noRegistrasiRawatJalan = '" & noRegRajal & "'"
            Case "RI"
        End Select

        Try
            cmd = New MySqlCommand(query, conn)
            da = New MySqlDataAdapter(cmd)

            Dim str As New DataTable
            str.Clear()
            da.Fill(str)
            If str.Rows.Count() > 0 Then
                txtKarStatus.Text = str.Rows(0)(0).ToString
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

        conn.Close()
    End Sub

#End Region
#Region "Operasi"
    Sub totalTarifOp()
        Dim totBayar As Long
        totBayar = 0
        For i As Integer = 0 To dgvOp.Rows.Count - 1
            totBayar = totBayar + Val(dgvOp.Rows(i).Cells(3).Value)
        Next
        txtTotalOp.Text = totBayar.ToString("#,##0")
        'txtTotal2.Text = totBayar
    End Sub

    Sub totalTarifDetailOp()
        Dim totBayar As Long
        totBayar = 0
        For i As Integer = 0 To dgvDetailOp.Rows.Count - 1
            totBayar = totBayar + Val(dgvDetailOp.Rows(i).Cells(6).Value)
        Next
        txtPerTotalOp.Text = totBayar.ToString("#,##0")
    End Sub

    Sub DaftarOperasi()
        Call koneksiServer()
        Dim query As String = ""
        Dim cmd As MySqlCommand
        Dim dr As MySqlDataReader

        query = "SELECT *
                   FROM vw_tindakanpasienopkasir
                  WHERE noDaftarPasien = '" & txtNoReg.Text & "'
               ORDER BY tglTindakan ASC"

        Try
            cmd = New MySqlCommand(query, conn)
            dr = cmd.ExecuteReader
            dgvOp.Rows.Clear()
            Do While dr.Read
                dgvOp.Rows.Add(dr.Item("noRegistrasiPasien"), dr.Item("tglTindakan"), dr.Item("instalasi"),
                                     dr.Item("totalTarifTindakan"), dr.Item("statusPembayaran"), dr.Item("noTindakanOP"))
            Loop
            dr.Close()
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
        conn.Close()
    End Sub

    Sub DetailOperasi(noTindakan As String)
        Call koneksiServer()
        Dim query As String = ""
        Dim cmd As MySqlCommand
        Dim dr As MySqlDataReader

        query = "SELECT * 
                   FROM vw_tindakanpasienopdetail
                  WHERE noTindakanOP = '" & noTindakan & "' 
                    AND statusHapus = 0"

        Try
            cmd = New MySqlCommand(query, conn)
            dr = cmd.ExecuteReader
            dgvDetailOp.Rows.Clear()
            Do While dr.Read
                dgvDetailOp.Rows.Add(dr.Item("kdTarif"), dr.Item("tindakan"), dr.Item("jmlTindakan"),
                               dr.Item("tarif"), dr.Item("operator"), dr.Item("anestesi"), dr.Item("subTotal"))
            Loop
            dr.Close()
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
        conn.Close()
    End Sub

    Sub LunasOperasiAll(noTindakan As String)
        Dim dt As String
        dt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
        Dim str As String = ""

        If noTindakan.Contains("TOP") Then
            str = "UPDATE t_tindakanop 
                      SET statusPembayaran = 'LUNAS',
                          tglPembayaran = '" & dt & "'
                    WHERE noTindakanOP IN (" & noTindakan & ")"
        ElseIf noTindakan.Contains("TOKP") Then
            str = "UPDATE t_tindakanokparu 
                      SET statusPembayaran = 'LUNAS',
                          tglPembayaran = '" & dt & "'
                    WHERE noTindakanOP IN (" & noTindakan & ")"
        End If

        Call koneksiServer()
        Dim cmd As MySqlCommand
        Try
            cmd = New MySqlCommand(str, conn)
            cmd.ExecuteNonQuery()
            MsgBox("Pembayaran Tindakan berhasil dilakukan", MessageBoxIcon.Information)
        Catch ex As Exception
            MsgBox("Pembayaran Tindakan All gagal dilakukan.", MessageBoxIcon.Error)
        End Try

        conn.Close()
    End Sub

    Sub LunasOperasi(noTindakan As String)
        Dim dt As String
        dt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
        Dim str As String = ""

        If noTindakan.Contains("TOP") Then
            str = "UPDATE t_tindakanop 
                      SET statusPembayaran = 'LUNAS',
                          tglPembayaran = '" & dt & "'
                    WHERE noTindakanOP = '" & noTindakan & "'"
        ElseIf noTindakan.Contains("TOKP") Then
            str = "UPDATE t_tindakanokparu 
                      SET statusPembayaran = 'LUNAS',
                          tglPembayaran = '" & dt & "'
                    WHERE noTindakanOP = '" & noTindakan & "'"
        End If

        Call koneksiServer()
        Dim cmd As MySqlCommand
        Try
            cmd = New MySqlCommand(str, conn)
            cmd.ExecuteNonQuery()
            MsgBox("Pembayaran Tindakan berhasil dilakukan", MessageBoxIcon.Information)
        Catch ex As Exception
            MsgBox("Pembayaran gagal dilakukan.", MessageBoxIcon.Error)
        End Try

        conn.Close()
    End Sub
#End Region
#Region "Obat OK"
    Sub totalTarifOK()
        Dim totBayar As Long
        totBayar = 0
        For i As Integer = 0 To dgvOk.Rows.Count - 1
            totBayar = totBayar + Val(dgvOk.Rows(i).Cells(3).Value)
        Next
        txtTotalOk.Text = (Math.Ceiling(totBayar / 100) * 100).ToString("#,##0")
    End Sub

    Sub totalTarifDetailOk()
        Dim totBayar As Long
        totBayar = 0
        For i As Integer = 0 To dgvDetailOk.Rows.Count - 1
            totBayar = totBayar + Val(dgvDetailOk.Rows(i).Cells(5).Value)
        Next
        txtPerTotalOk.Text = (Math.Ceiling(totBayar / 100) * 100).ToString("#,##0")
    End Sub

    Sub DaftarOK()
        Call koneksiFarmasi()
        Dim query As String = ""
        Dim cmd As MySqlCommand
        Dim dr As MySqlDataReader

        query = "SELECT * FROM t_penjualanobatok WHERE noDaftar = '" & txtNoReg.Text & "' ORDER BY tglPenjualanObatOK ASC"

        Try
            cmd = New MySqlCommand(query, conn)
            dr = cmd.ExecuteReader
            dgvOk.Rows.Clear()

            Do While dr.Read
                dgvOk.Rows.Add(dr.Item("noDaftar"), dr.Item("noPenjualanObatOK"),
                                 dr.Item("tglPenjualanObatOK"), dr.Item("totalAkhirPenjualanOK"), dr.Item("statusPembayaran"))
            Loop

            dr.Close()
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
        conn.Close()
    End Sub

    Sub DetailOK(noTransaksi As String)
        Call koneksiServer()
        Dim query As String = ""
        Dim cmd As MySqlCommand
        Dim dr As MySqlDataReader

        query = "SELECT * FROM vw_detailpenjualanobatok WHERE noPenjualanObatOK = '" & noTransaksi & "'"

        Try
            cmd = New MySqlCommand(query, conn)
            dr = cmd.ExecuteReader
            dgvDetailOk.Rows.Clear()
            Do While dr.Read
                dgvDetailOk.Rows.Add(dr.Item("noPenjualanObatOK"), dr.Item("kdObat"),
                                     dr.Item("namaObat"), dr.Item("harga"), dr.Item("JumlahItem"),
                                     dr.Item("subTotalPenjualan"))
            Loop

            dr.Close()
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
        conn.Close()
    End Sub

    Sub LunasOkAll(noTransaksi As String)
        Dim dt As String
        dt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")

        Dim str As String = ""
        str = "UPDATE t_penjualanobatok
                  SET statusPembayaran = 'LUNAS',
                      tglPembayaran = '" & dt & "'
                WHERE noPenjualanObatOK IN (" & noTransaksi & ")"
        Call koneksiFarmasi()
        Dim cmd As MySqlCommand
        Try
            cmd = New MySqlCommand(str, conn)
            cmd.ExecuteNonQuery()
            MsgBox("Pembayaran Obat berhasil dilakukan", MessageBoxIcon.Information)
        Catch ex As Exception
            MsgBox("Pembayaran Obat All gagal dilakukan.", MessageBoxIcon.Error)
        End Try

        conn.Close()
    End Sub

    Sub LunasOk(noTransaksi As String)
        Dim dt As String
        dt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")

        Dim str As String = ""

        str = "UPDATE t_penjualanobatok 
                  SET statusPembayaran = 'LUNAS',
                      tglPembayaran = '" & dt & "'
                WHERE noPenjualanObatOK = '" & noTransaksi & "'"

        Call koneksiFarmasi()
        Dim cmd As MySqlCommand
        Try
            cmd = New MySqlCommand(str, conn)
            cmd.ExecuteNonQuery()
            MsgBox("Pembayaran Obat berhasil dilakukan", MessageBoxIcon.Information)
        Catch ex As Exception
            MsgBox("Pembayaran gagal dilakukan.", MessageBoxIcon.Error)
        End Try

        conn.Close()
    End Sub
#End Region
#Region "Hemodialisa"
    Sub totalTarifHemo()
        Dim totBayar As Long
        totBayar = 0
        For i As Integer = 0 To dgvHemo.Rows.Count - 1
            totBayar = totBayar + Val(dgvHemo.Rows(i).Cells(3).Value)
        Next
        txtTotalHemo.Text = totBayar.ToString("#,##0")
        'txtTotal2.Text = totBayar
    End Sub

    Sub totalTarifDetailHemo()
        Dim totBayar As Long
        totBayar = 0
        For i As Integer = 0 To dgvDetailHemo.Rows.Count - 1
            totBayar = totBayar + Val(dgvDetailHemo.Rows(i).Cells(6).Value)
        Next
        txtPerTotalHemo.Text = totBayar.ToString("#,##0")
    End Sub

    Sub DaftarTinHemo()
        Call koneksiServer()
        Dim query As String = ""
        Dim cmd As MySqlCommand
        Dim dr As MySqlDataReader

        Select Case unit
            Case "IG"
                query = "SELECT *
                           FROM vw_tindakanhdrajalkasir
                          WHERE noRegistrasi = '" & txtNoReg.Text & "'
                            AND (totalTarifTindakan IS NOT NULL AND totalTarifTindakan != 0)
                       ORDER BY tglTindakan ASC"
            Case "RJ"
                query = "SELECT *
                           FROM vw_tindakanhdrajalkasir
                          WHERE noRegistrasi = '" & txtNoReg.Text & "'
                            AND (totalTarifTindakan IS NOT NULL AND totalTarifTindakan != 0)
                       ORDER BY tglTindakan ASC"
            Case "RI"
                query = "SELECT *
                           FROM vw_tindakanhdranapkasir
                          WHERE noRegistrasi = '" & txtNoReg.Text & "'
                            AND (totalTarifTindakan IS NOT NULL AND totalTarifTindakan != 0)
                       ORDER BY tglTindakan ASC"
        End Select

        Try
            cmd = New MySqlCommand(query, conn)
            dr = cmd.ExecuteReader
            dgvHemo.Rows.Clear()
            Select Case unit
                Case "IG"
                    Do While dr.Read
                        dgvHemo.Rows.Add(dr.Item("noRegistrasiHD"), dr.Item("tglTindakan"), dr.Item("unit"),
                               dr.Item("totalTarifTindakan"), dr.Item("statusPembayaran"), dr.Item("noTindakanHD"))
                    Loop
                Case "RJ"
                    Do While dr.Read
                        dgvHemo.Rows.Add(dr.Item("noRegistrasiHD"), dr.Item("tglTindakan"), dr.Item("unit"),
                               dr.Item("totalTarifTindakan"), dr.Item("statusPembayaran"), dr.Item("noTindakanHD"))
                    Loop
                Case "RI"
                    Do While dr.Read
                        dgvHemo.Rows.Add(dr.Item("noRegistrasiHD"), dr.Item("tglTindakan"), dr.Item("unit"),
                               dr.Item("totalTarifTindakan"), dr.Item("statusPembayaran"), dr.Item("noTindakanHD"))
                    Loop
            End Select

            dr.Close()
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
        conn.Close()
    End Sub

    Sub DetailTinHemo(noTindakan As String)
        Call koneksiServer()
        Dim query As String = ""
        Dim cmd As MySqlCommand
        Dim dr As MySqlDataReader

        Select Case unit
            Case "IG"
                query = "SELECT * 
                           FROM vw_tindakanhdrajaldetail
                          WHERE noTindakanHD = '" & noTindakan & "'"
            Case "RJ"
                query = "SELECT * 
                           FROM vw_tindakanhdrajaldetail 
                          WHERE noTindakanHD = '" & noTindakan & "'"
            Case "RI"
                query = "SELECT *
                           FROM vw_tindakanhdranapdetail 
                          WHERE noTindakanHD = '" & noTindakan & "'"
        End Select

        Try
            cmd = New MySqlCommand(query, conn)
            dr = cmd.ExecuteReader
            dgvDetailHemo.Rows.Clear()
            Do While dr.Read
                dgvDetailHemo.Rows.Add(dr.Item("kdTindakan"), dr.Item("tindakan"), dr.Item("jumlahTindakan"),
                               dr.Item("tarifTindakan"), dr.Item("DPJP"), dr.Item("PPA"), dr.Item("subtotal"))
            Loop
            dr.Close()
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
        conn.Close()
    End Sub

    Sub LunasHemoAll(noTindakan As String)
        Dim dt As String
        dt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")

        Dim str As String = ""
        Select Case unit
            Case "IG"
                str = "UPDATE t_tindakanhd
                          SET statusPembayaran = 'LUNAS',
                              tglPembayaran = '" & dt & "'
                        WHERE noTindakanHD IN (" & noTindakan & ")"
            Case "RJ"
                str = "UPDATE t_tindakanhd
                          SET statusPembayaran = 'LUNAS',
                              tglPembayaran = '" & dt & "'
                        WHERE noTindakanHD IN (" & noTindakan & ")"
            Case "RI"
                str = "UPDATE t_tindakanhd
                          SET statusPembayaran = 'LUNAS',
                              tglPembayaran = '" & dt & "'
                        WHERE noTindakanHD IN (" & noTindakan & ")"
        End Select

        Call koneksiServer()
        Dim cmd As MySqlCommand
        Try
            cmd = New MySqlCommand(str, conn)
            cmd.ExecuteNonQuery()
            MsgBox("Pembayaran Tindakan berhasil dilakukan", MessageBoxIcon.Information)
        Catch ex As Exception
            MsgBox("Pembayaran Tindakan All gagal dilakukan.", MessageBoxIcon.Error)
        End Try

        conn.Close()
    End Sub

    Sub LunasHemo(noTindakan As String)
        Dim dt As String
        dt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")

        Dim str As String = ""
        Select Case unit
            Case "IG"
                str = "UPDATE t_tindakanhd
                          SET statusPembayaran = 'LUNAS',
                              tglPembayaran = '" & dt & "'
                        WHERE noTindakanHD IN (" & noTindakan & ")"
            Case "RJ"
                str = "UPDATE t_tindakanhd
                          SET statusPembayaran = 'LUNAS',
                              tglPembayaran = '" & dt & "'
                        WHERE noTindakanHD IN (" & noTindakan & ")"
            Case "RI"
                str = "UPDATE t_tindakanhd
                          SET statusPembayaran = 'LUNAS',
                              tglPembayaran = '" & dt & "'
                        WHERE noTindakanHD IN (" & noTindakan & ")"
        End Select

        Call koneksiServer()
        Dim cmd As MySqlCommand
        Try
            cmd = New MySqlCommand(str, conn)
            cmd.ExecuteNonQuery()
            MsgBox("Pembayaran Tindakan berhasil dilakukan", MessageBoxIcon.Information)
        Catch ex As Exception
            MsgBox("Pembayaran gagal dilakukan.", MessageBoxIcon.Error)
        End Try

        conn.Close()
    End Sub
#End Region

    Private Sub Home_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.FormBorderStyle = FormBorderStyle.None
        Me.WindowState = FormWindowState.Normal
        Me.StartPosition = FormStartPosition.Manual
        With Screen.PrimaryScreen.WorkingArea
            Me.SetBounds(.Left, .Top, .Width, .Height)
        End With

        If ApplicationDeployment.IsNetworkDeployed Then
            Dim ver As ApplicationDeployment = ApplicationDeployment.CurrentDeployment
            Label60.Text = "Version " & ver.CurrentVersion.ToString()
        End If

        TabControl1.SelectedTab = TabPage4
        txtRekMed.Select()
        txtTglCO.Text = DateTime.Now.ToString("dd/MM/yyyy")
    End Sub

    Private Sub TabControl1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles TabControl1.SelectedIndexChanged
        If TabControl1.SelectedTab Is TabPage1 Then
            dgvDetailTindakan.Rows.Clear()
            Select Case unit
                Case "IG"
                    'MsgBox("Tindakan IGD")
                    Call DaftarTindakan()
                    Call totalTarifTindakan()
                    pnlRanap.Visible = True
                Case "RJ"
                    'MsgBox("Tindakan RAJAL")
                    pnlRanap.Visible = False
                    Call DaftarTindakan()
                    Call totalTarifTindakan()
                Case "RI"
                    'MsgBox("Tindakan RANAP")
                    Call DaftarTindakan()
                    Call totalTarifTindakan()
                    pnlRanap.Visible = True
            End Select
            'For Each row As DataGridViewRow In dgvTindakan.Rows
            '    If row.Cells(4).Value.ToString.Equals("BELUM LUNAS") Then
            '        btnPrintTindakAll.BackColor = Color.Green
            '        btnPrintTindakAll.Text = "PROSES"
            '    ElseIf row.Cells(4).Value.ToString.Equals("LUNAS") Then
            '        btnPrintTindakAll.BackColor = Color.Navy
            '        btnPrintTindakAll.Text = "PRINT"
            '    End If
            'Next
        ElseIf TabControl1.SelectedTab Is TabPage2 Then 'Lab
            dgvDetailLab.Rows.Clear()
            Select Case unit
                Case "IG"
                    'MsgBox("Laboratorium IGD")
                    Call DaftarLab()
                    Call totalTarifLab()
                    pnlRanap.Visible = True
                Case "RJ"
                    'MsgBox("Laboratorium RAJAL")
                    pnlRanap.Visible = False
                    Call DaftarLab()
                    Call totalTarifLab()
                Case "RI"
                    'MsgBox("Laboratorium RANAP")
                    Call DaftarLab()
                    Call totalTarifLab()
                    pnlRanap.Visible = True
            End Select
            'For Each row As DataGridViewRow In dgvLab.Rows
            '    If row.Cells(3).Value.ToString.Equals("BELUM LUNAS") Then
            '        btnPrintLabAll.BackColor = Color.Green
            '        btnPrintLabAll.Text = "PROSES"
            '    ElseIf row.Cells(3).Value.ToString.Equals("LUNAS") Then
            '        btnPrintLabAll.BackColor = Color.Navy
            '        btnPrintLabAll.Text = "PRINT"
            '    End If
            'Next
        ElseIf TabControl1.SelectedTab Is TabPage3 Then
            dgvDetailRad.Rows.Clear()
            Select Case unit
                Case "IG"
                    'MsgBox("Radiologi IGD")
                    Call DaftarRad()
                    Call totalTarifRad()
                    pnlRanap.Visible = True
                Case "RJ"
                    'MsgBox("Radiologi RAJAL")
                    pnlRanap.Visible = False
                    Call DaftarRad()
                    Call totalTarifRad()
                Case "RI"
                    'MsgBox("Radiologi RANAP")
                    Call DaftarRad()
                    Call totalTarifRad()
                    pnlRanap.Visible = True

            End Select
            'For Each row As DataGridViewRow In dgvRad.Rows
            '    If row.Cells(3).Value.ToString.Equals("BELUM LUNAS") Then
            '        btnPrintRadAll.BackColor = Color.Green
            '        btnPrintRadAll.Text = "PROSES"
            '    ElseIf row.Cells(3).Value.ToString.Equals("LUNAS") Then
            '        btnPrintRadAll.BackColor = Color.Navy
            '        btnPrintRadAll.Text = "PRINT"
            '    End If
            'Next
        ElseIf TabControl1.SelectedTab Is TabPage4 Then
            dgvDetailObat.Rows.Clear()
            Select Case unit
                Case "IG"
                    'MsgBox("Obat IGD")
                    DaftarObat()
                    totalTarifObat()
                    pnlRanap.Visible = True
                Case "RJ"
                    'MsgBox("Obat RAJAL")
                    pnlRanap.Visible = False
                    Call DaftarObat()
                    Call totalTarifObat()
                Case "RI"
                    'MsgBox("Obat RANAP")
                    DaftarObat()
                    totalTarifObat()
                    pnlRanap.Visible = True
            End Select
            'For Each row As DataGridViewRow In dgvObat.Rows
            '    If row.Cells(4).Value.ToString.Equals("BELUM LUNAS") Then
            '        btnPrintObatAll.BackColor = Color.Green
            '        btnPrintObatAll.Text = "PROSES"
            '    ElseIf row.Cells(4).Value.ToString.Equals("LUNAS") Then
            '        btnPrintObatAll.BackColor = Color.Navy
            '        btnPrintObatAll.Text = "PRINT"
            '    End If
            'Next
        ElseIf TabControl1.SelectedTab Is TabPage5 Then
            dgvDetailAll.Rows.Clear()
            Select Case unit
                Case "IG"
                    pnlRanap.Visible = True
                    Call DaftarAll()
                    Call totalTarifAll()
                Case "RJ"
                    pnlRanap.Visible = False
                Case "RI"
                    pnlRanap.Visible = True
                    Call DaftarAll()
                    Call totalTarifAll()
            End Select

            If txtCaraBayar.Text.Equals("Umum", StringComparison.OrdinalIgnoreCase) Then
                Dim BLCount As Integer = 0
                For Each row As DataGridViewRow In dgvAll.Rows
                    If row.Cells(4).Value.ToString.Equals("BELUM LUNAS") Then
                        BLCount = BLCount + 1
                    End If
                Next

                If BLCount > 0 Then
                    btnBayarRanap.Enabled = True
                    btnBayarRanap.Text = "PROSES PEMBAYARAN"
                Else
                    btnBayarRanap.Enabled = False
                    btnBayarRanap.Text = "LUNAS"
                End If
            Else
                btnBayarRanap.Enabled = False
                btnBayarRanap.Text = "PROSES PEMBAYARAN"
            End If
        ElseIf TabControl1.SelectedTab Is TabPage6 Then
            dgvAll.Rows.Clear()
            Select Case unit
                Case "IG"
                    Call totalTarifKarcis()
                    pnlRanap.Visible = True
                Case "RJ"
                    pnlRanap.Visible = False
                    Call totalTarifKarcis()
                Case "RI"
                    pnlRanap.Visible = True
            End Select
        ElseIf TabControl1.SelectedTab Is TabPage7 Then
            dgvDetailOp.Rows.Clear()
            Select Case unit
                Case "IG"
                    'MsgBox("OP IGD")
                    pnlRanap.Visible = True
                    DaftarOperasi()
                    totalTarifOp()
                Case "RJ"
                    'MsgBox("OP RAJAL")
                    pnlRanap.Visible = False
                    DaftarOperasi()
                    totalTarifOp()
                Case "RI"
                    'MsgBox("OP RANAP")
                    pnlRanap.Visible = True
                    DaftarOperasi()
                    totalTarifOp()
            End Select
        ElseIf TabControl1.SelectedTab Is TabPage8 Then
            dgvDetailOk.Rows.Clear()
            Select Case unit
                Case "IG"
                    'MsgBox("OP IGD")
                    pnlRanap.Visible = True
                    DaftarOK()
                    totalTarifOK()
                Case "RJ"
                    'MsgBox("OP RAJAL")
                    pnlRanap.Visible = False
                    DaftarOK()
                    totalTarifOK()
                Case "RI"
                    'MsgBox("OP RANAP")
                    pnlRanap.Visible = True
                    DaftarOK()
                    totalTarifOK()
            End Select
        ElseIf TabControl1.SelectedTab Is TabPage9 Then
            dgvDetailHemo.Rows.Clear()
            Select Case unit
                Case "IG"
                    'MsgBox("HD IGD")
                    pnlRanap.Visible = True
                    DaftarTinHemo()
                    totalTarifHemo()
                Case "RJ"
                    'MsgBox("HD RAJAL")
                    pnlRanap.Visible = False
                    DaftarTinHemo()
                    totalTarifHemo()
                Case "RI"
                    'MsgBox("HD RANAP")
                    pnlRanap.Visible = True
                    DaftarTinHemo()
                    totalTarifHemo()
            End Select
        End If
    End Sub

    Private Sub picKeluar_Click(sender As Object, e As EventArgs) Handles picKeluar.Click
        Dim konfirmasi As MsgBoxResult

        konfirmasi = MsgBox("Apakah anda yakin ingin keluar..?", vbQuestion + vbYesNo, "Konfirmasi")
        If konfirmasi = vbYes Then
            Me.Close()
            LoginForm.Show()
        End If
    End Sub

    Private Sub txtTglLahir_TextChanged(sender As Object, e As EventArgs) Handles txtTglLahir.TextChanged
        If txtTglLahir.Text = "" Then
            Return
        Else
            Dim dt As DateTime = Convert.ToDateTime(txtTglLahir.Text)
            Dim cul As IFormatProvider = New System.Globalization.CultureInfo("id-ID", True)
            Dim dt1 As DateTime = DateTime.Parse(txtTglLahir.Text, cul, System.Globalization.DateTimeStyles.AssumeLocal)
            txtTglLahir.Text = dt1.ToShortDateString
            txtUmur.Text = hitungUmur(dt1.ToShortDateString)
            txtKarTtl.Text = txtTmpLahir.Text & ", " & txtTglLahir.Text
            txtKarUmur.Text = hitungUmur(dt1.ToShortDateString)
        End If
    End Sub

    Private Sub btnCari_Click(sender As Object, e As EventArgs) Handles btnCari.Click
        If txtRekMed.Text = "" Then
            Call clearText()
            'Call cariPasienPeg()
        Else
            Call cariPasien()
        End If
    End Sub

    Private Sub txtRekMed_KeyDown(sender As Object, e As KeyEventArgs) Handles txtRekMed.KeyDown
        If e.KeyCode = Keys.Enter Then
            If txtRekMed.Text = "" Then
                Call clearText()
                'Call cariPasienPeg()
            Else
                Call cariPasien()
            End If
        End If
    End Sub

    Private Sub txtRegUnit_TextChanged(sender As Object, e As EventArgs) Handles txtRegUnit.TextChanged
        If txtRegUnit.Text = "" Then
            Return
        ElseIf txtUnit.Text = "IGD" Or txtUnit.Text = "IGD PINERE" Then
            unit = "IG"
            Label28.Visible = False
            Label37.Visible = False
            'txtJumInap.Visible = False
            'txtTarifKmr.Visible = False
            pnlRuang.Visible = False
            Call tarifDPJP()
            Call tampilNominalIGD()
            txtNoKwintasi.Text = "00" & txtNoReg.Text.Substring(13)
        Else
            unit = txtRegUnit.Text.Substring(0, 2)
            If unit.Contains("RI") Then
                Label28.Visible = False
                Label37.Visible = False
                'txtJumInap.Visible = False
                'txtTarifKmr.Visible = False
                pnlRuang.Visible = True
                Call hitungInap()
                Call tarifDPJP()
                Call tampilTarifKamar()
                Call totalTarifKamar()
                Call tampilNominal()
                txtNoKwintasi.Text = "00" & txtNoReg.Text.Substring(13)
                dgvRuang.CurrentCell.Selected = False
            Else
                Label28.Visible = False
                Label37.Visible = False
                'txtJumInap.Visible = False
                'txtTarifKmr.Visible = False
                pnlRuang.Visible = False
                Call tarifDPJP()
            End If
        End If

        Dim BLCount As Integer = 0
        If TabControl1.SelectedTab Is TabPage1 Then
            dgvDetailTindakan.Rows.Clear()
            Select Case unit
                Case "IG"
                    'MsgBox("Tindakan IGD")
                    Call DaftarTindakan()
                    Call totalTarifTindakan()
                    pnlRanap.Visible = True
                Case "RJ"
                    'MsgBox("Tindakan RAJAL")
                    pnlRanap.Visible = False
                    Call DaftarTindakan()
                    Call totalTarifTindakan()
                Case "RI"
                    'MsgBox("Tindakan RANAP")
                    Call DaftarTindakan()
                    Call totalTarifTindakan()
                    pnlRanap.Visible = True
            End Select
            'For Each row As DataGridViewRow In dgvTindakan.Rows
            '    If row.Cells(4).Value.ToString.Equals("BELUM LUNAS") Then
            '        btnPrintTindakAll.BackColor = Color.Green
            '        btnPrintTindakAll.Text = "PROSES"
            '    ElseIf row.Cells(4).Value.ToString.Equals("LUNAS") Then
            '        btnPrintTindakAll.BackColor = Color.Navy
            '        btnPrintTindakAll.Text = "PRINT"
            '    End If
            'Next
        ElseIf TabControl1.SelectedTab Is TabPage2 Then
            dgvDetailLab.Rows.Clear()
            Select Case unit
                Case "IG"
                    'MsgBox("Laboratorium IGD")
                    Call DaftarLab()
                    Call totalTarifLab()
                    pnlRanap.Visible = True
                Case "RJ"
                    'MsgBox("Laboratorium RAJAL")
                    pnlRanap.Visible = False
                    Call DaftarLab()
                    Call totalTarifLab()
                Case "RI"
                    'MsgBox("Laboratorium RANAP")
                    Call DaftarLab()
                    Call totalTarifLab()
                    pnlRanap.Visible = True
            End Select
            'For Each row As DataGridViewRow In dgvLab.Rows
            '    If row.Cells(3).Value.ToString.Equals("BELUM LUNAS") Then
            '        btnPrintLabAll.BackColor = Color.Green
            '        btnPrintLabAll.Text = "PROSES"
            '    ElseIf row.Cells(3).Value.ToString.Equals("LUNAS") Then
            '        btnPrintLabAll.BackColor = Color.Navy
            '        btnPrintLabAll.Text = "PRINT"
            '    End If
            'Next
        ElseIf TabControl1.SelectedTab Is TabPage3 Then
            dgvDetailRad.Rows.Clear()
            Select Case unit
                Case "IG"
                    'MsgBox("Radiologi IGD")
                    Call DaftarRad()
                    Call totalTarifRad()
                    pnlRanap.Visible = True
                Case "RJ"
                    'MsgBox("Radiologi RAJAL")
                    pnlRanap.Visible = False
                    Call DaftarRad()
                    Call totalTarifRad()
                Case "RI"
                    'MsgBox("Radiologi RANAP")
                    Call DaftarRad()
                    Call totalTarifRad()
                    pnlRanap.Visible = True
            End Select
            'For Each row As DataGridViewRow In dgvRad.Rows
            '    If row.Cells(3).Value.ToString.Equals("BELUM LUNAS") Then
            '        btnPrintRadAll.BackColor = Color.Green
            '        btnPrintRadAll.Text = "PROSES"
            '    ElseIf row.Cells(3).Value.ToString.Equals("LUNAS") Then
            '        btnPrintRadAll.BackColor = Color.Navy
            '        btnPrintRadAll.Text = "PRINT"
            '    End If
            'Next
        ElseIf TabControl1.SelectedTab Is TabPage4 Then
            dgvDetailObat.Rows.Clear()
            Select Case unit
                Case "IG"
                    'MsgBox("Obat IGD")
                    DaftarObat()
                    totalTarifObat()
                    pnlRanap.Visible = True
                Case "RJ"
                    'MsgBox("Obat RAJAL")
                    pnlRanap.Visible = False
                    DaftarObat()
                    totalTarifObat()
                Case "RI"
                    'MsgBox("Obat RANAP")
                    DaftarObat()
                    totalTarifObat()
                    pnlRanap.Visible = True
            End Select
            'For Each row As DataGridViewRow In dgvObat.Rows
            '    If row.Cells(4).Value.ToString.Equals("BELUM LUNAS") Then
            '        btnPrintObatAll.BackColor = Color.Green
            '        btnPrintObatAll.Text = "PROSES"
            '    ElseIf row.Cells(4).Value.ToString.Equals("LUNAS") Then
            '        btnPrintObatAll.BackColor = Color.Navy
            '        btnPrintObatAll.Text = "PRINT"
            '    End If
            'Next
        ElseIf TabControl1.SelectedTab Is TabPage5 Then
            dgvAll.Rows.Clear()
            dgvDetailAll.Rows.Clear()
            Select Case unit
                Case "IG"
                    pnlRanap.Visible = True
                    Call DaftarAll()
                    Call totalTarifAll()
                Case "RJ"
                    pnlRanap.Visible = False
                Case "RI"
                    pnlRanap.Visible = True
                    Call DaftarAll()
                    Call totalTarifAll()
            End Select

            If txtCaraBayar.Text.Equals("Umum", StringComparison.OrdinalIgnoreCase) Then
                For Each row As DataGridViewRow In dgvAll.Rows
                    If row.Cells(4).Value.ToString.Equals("BELUM LUNAS") Then
                        BLCount = BLCount + 1
                    End If
                Next
            End If

            If BLCount > 0 Then
                btnBayarRanap.Enabled = True
                btnBayarRanap.Text = "PROSES PEMBAYARAN"
            Else
                btnBayarRanap.Enabled = False
                btnBayarRanap.Text = "LUNAS"
            End If

        ElseIf TabControl1.SelectedTab Is TabPage6 Then
            dgvAll.Rows.Clear()
            Select Case unit
                Case "IG"
                    Call totalTarifKarcis()
                    pnlRanap.Visible = True
                Case "RJ"
                    pnlRanap.Visible = False
                    Call totalTarifKarcis()
                Case "RI"
                    Call tampilTarifKamar()
                    pnlRanap.Visible = True
                    dgvRuang.CurrentCell.Selected = False
            End Select
        ElseIf TabControl1.SelectedTab Is TabPage7 Then
            dgvDetailOp.Rows.Clear()
            Select Case unit
                Case "IG"
                    'MsgBox("OP IGD")
                    pnlRanap.Visible = True
                    DaftarOperasi()
                    totalTarifOp()
                Case "RJ"
                    'MsgBox("OP RAJAL")
                    pnlRanap.Visible = False
                    DaftarOperasi()
                    totalTarifOp()
                Case "RI"
                    'MsgBox("OP RANAP")
                    pnlRanap.Visible = True
                    DaftarOperasi()
                    totalTarifOp()
            End Select
        ElseIf TabControl1.SelectedTab Is TabPage8 Then
            dgvDetailOk.Rows.Clear()
            Select Case unit
                Case "IG"
                    'MsgBox("OK IGD")
                    pnlRanap.Visible = True
                    DaftarOK()
                    totalTarifOK()
                Case "RJ"
                    'MsgBox("OK RAJAL")
                    pnlRanap.Visible = False
                    DaftarOK()
                    totalTarifOK()
                Case "RI"
                    'MsgBox("OK RANAP")
                    pnlRanap.Visible = True
                    DaftarOK()
                    totalTarifOK()
            End Select
        ElseIf TabControl1.SelectedTab Is TabPage9 Then
            dgvDetailHemo.Rows.Clear()
            Select Case unit
                Case "IG"
                    'MsgBox("HD IGD")
                    pnlRanap.Visible = True
                    DaftarTinHemo()
                    totalTarifHemo()
                Case "RJ"
                    'MsgBox("HD RAJAL")
                    pnlRanap.Visible = False
                    DaftarTinHemo()
                    totalTarifHemo()
                Case "RI"
                    'MsgBox("HD RANAP")
                    pnlRanap.Visible = True
                    DaftarTinHemo()
                    totalTarifHemo()
            End Select
        End If

        Select Case unit
            Case "IG"
                pnlRanap.Visible = True
                Call DaftarAll()
                Call totalTarifAll()
            Case "RJ"
                pnlRanap.Visible = False
            Case "RI"
                pnlRanap.Visible = True
                Call DaftarAll()
                Call totalTarifAll()
        End Select
    End Sub
    Private Sub txtTarifKmr_TextChanged(sender As Object, e As EventArgs) Handles txtTarifKmr.TextChanged
        If txtTarifKmr.Text = "" Then
            Return
        Else
            Dim g As Integer
            g = txtTarifKmr.Text
            txtTarifKmr.Text = Format(g, "###,###")
        End If
    End Sub

    Private Sub PictureBox2_Click(sender As Object, e As EventArgs) Handles PictureBox2.Click
        Dim FormInfo As Info = New Info
        FormInfo.ShowDialog()
    End Sub

    Private Sub PictureBox3_Click(sender As Object, e As EventArgs) Handles PictureBox3.Click
        Dim FormBJ As VABankJatim = New VABankJatim

        If txtNoReg.Text = "" Then
            MsgBox("Pilih data pasien terlebih dahulu !!", MsgBoxStyle.Exclamation)
        Else
            FormBJ.ShowDialog()
        End If
    End Sub

    Private Sub txtTglKeluar_ValueChanged(sender As Object, e As EventArgs) Handles txtTglKeluar.ValueChanged
        'Call hitungInapSampaiDgn()         -NOTHING HAPPEN
        'Call totalTarifAll()
    End Sub

    Private Sub txtUnit_TextChanged(sender As Object, e As EventArgs) Handles txtUnit.TextChanged
        Call koneksiServer()
        Dim query As String
        Dim cmd As MySqlCommand
        Dim dr As MySqlDataReader
        Try
            query = "SELECT kode
                       FROM t_vaunit 
                      WHERE unit = '" & txtUnit.Text & "'"
            cmd = New MySqlCommand(query, conn)
            dr = cmd.ExecuteReader

            While dr.Read
                txtVaUnit.Text = dr.GetString("kode")
            End While
            dr.Close()
            conn.Close()
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

        Call cekRIK()
    End Sub

    Private Sub dgvRuang_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvRuang.CellContentClick
        If e.RowIndex = -1 Then
            Return
        End If

        txtRegUnit.Text = dgvRuang.Rows(e.RowIndex).Cells(1).Value.ToString
        txtUnit.Text = dgvRuang.Rows(e.RowIndex).Cells(2).Value.ToString
        txtKelas.Text = dgvRuang.Rows(e.RowIndex).Cells(3).Value.ToString
    End Sub

    Private Sub dgvRuang_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvRuang.CellClick
        If e.RowIndex = -1 Then
            Return
        End If

        txtRegUnit.Text = dgvRuang.Rows(e.RowIndex).Cells(1).Value.ToString
        txtUnit.Text = dgvRuang.Rows(e.RowIndex).Cells(2).Value.ToString
        txtKelas.Text = dgvRuang.Rows(e.RowIndex).Cells(3).Value.ToString
    End Sub

    Private Sub dgvRuang_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles dgvRuang.CellFormatting
        dgvRuang.Columns(4).DefaultCellStyle.Format = "N0"

        For i As Integer = 0 To dgvRuang.Rows.Count - 1
            If i Mod 2 = 0 Then
                dgvRuang.Rows(i).DefaultCellStyle.BackColor = Color.AliceBlue
            Else
                dgvRuang.Rows(i).DefaultCellStyle.BackColor = Color.White
            End If
        Next
    End Sub

    Private Sub dgvRuang_RowPostPaint(sender As Object, e As DataGridViewRowPostPaintEventArgs) Handles dgvRuang.RowPostPaint
        Dim dg As DataGridView = DirectCast(sender, DataGridView)
        Dim rowNumber As String = (e.RowIndex + 1).ToString()

        Dim size As SizeF = e.Graphics.MeasureString(rowNumber, Me.Font)
        If dg.RowHeadersWidth < CInt(size.Width + 20) Then
            dg.RowHeadersWidth = CInt(size.Width + 20)
        End If

        Dim b As Brush = SystemBrushes.ControlText

        e.Graphics.DrawString(rowNumber, dg.Font, b, e.RowBounds.Location.X + 15, e.RowBounds.Location.Y + ((e.RowBounds.Height - size.Height) / 2))
    End Sub
#Region "Tindakan"
    Private Sub dgvTindakan_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvTindakan.CellClick
        Dim noTindakan, total, status As String

        If e.RowIndex = -1 Then
            Return
        End If

        noTindakan = dgvTindakan.Rows(e.RowIndex).Cells(5).Value.ToString
        total = dgvTindakan.Rows(e.RowIndex).Cells(3).Value.ToString
        status = dgvTindakan.Rows(e.RowIndex).Cells(4).Value.ToString
        txtNoTindakan.Text = noTindakan
        'txtStatus.Text = status
        'txtPerTotalTindakan.Text = "Rp " & CInt(total).ToString("#,##0")

        Call DetailTindakan(noTindakan)
        Call totalTarifDetailTin()
    End Sub

    Private Sub dgvTindakan_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvTindakan.CellContentClick
        Dim noTindakan, total, status As String

        If e.RowIndex = -1 Then
            Return
        End If

        noTindakan = dgvTindakan.Rows(e.RowIndex).Cells(5).Value.ToString
        total = dgvTindakan.Rows(e.RowIndex).Cells(3).Value.ToString
        status = dgvTindakan.Rows(e.RowIndex).Cells(4).Value.ToString
        txtNoTindakan.Text = noTindakan
        'txtStatus.Text = status
        'txtPerTotalTindakan.Text = "Rp " & CInt(total).ToString("#,##0")

        Call DetailTindakan(noTindakan)
        Call totalTarifDetailTin()

        Dim konfirmasi As MsgBoxResult
        If e.ColumnIndex = 6 Then
            Select Case dgvTindakan.Rows(e.RowIndex).Cells(4).Value.ToString
                Case "BELUM LUNAS"
                    konfirmasi = MsgBox("Apakah transaksi akan dibayar ?", vbQuestion + vbYesNo, "Tindakan")
                    If konfirmasi = vbYes Then
                        Call LunasTindakan(noTindakan)
                        Call DaftarTindakan()
                        'MsgBox(tindakan & " - Memulai tindakan", MsgBoxStyle.Information)
                    End If
                Case "LUNAS"
                    konfirmasi = MsgBox("Apakah ingin mencetak nota ?", vbQuestion + vbYesNo, "Tindakan")
                    If konfirmasi = vbYes Then
                        ViewNotaAllTindakan.Ambil_Data = True
                        ViewNotaAllTindakan.Form_Ambil_Data = "TindakanPernota"
                        ViewNotaAllTindakan.Show()
                    End If
            End Select
        End If

    End Sub

    Private Sub dgvTindakan_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles dgvTindakan.CellFormatting

        dgvTindakan.Columns(3).DefaultCellStyle.Format = "###,###,###"
        dgvTindakan.Columns(1).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        dgvTindakan.Columns(2).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        dgvTindakan.Columns(3).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        dgvTindakan.Columns(4).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        dgvTindakan.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        dgvTindakan.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        dgvTindakan.ColumnHeadersDefaultCellStyle.Font = New Font("Tahoma", 9, FontStyle.Bold)
        dgvTindakan.ColumnHeadersDefaultCellStyle.BackColor = Color.DodgerBlue
        dgvTindakan.ColumnHeadersDefaultCellStyle.ForeColor = Color.White
        dgvTindakan.DefaultCellStyle.Font = New Font("Tahoma", 9, FontStyle.Bold)
        dgvTindakan.DefaultCellStyle.SelectionBackColor = Color.PaleTurquoise
        dgvTindakan.DefaultCellStyle.SelectionForeColor = Color.Black
        dgvTindakan.AllowUserToResizeRows = False
        dgvTindakan.EnableHeadersVisualStyles = False

        For i As Integer = 0 To dgvTindakan.Rows.Count - 1
            If dgvTindakan.Rows(i).Cells(4).Value = "BELUM LUNAS" Then
                dgvTindakan.Rows(i).Cells(4).Style.BackColor = Color.Orange
                dgvTindakan.Rows(i).Cells(4).Style.ForeColor = Color.White
            ElseIf dgvTindakan.Rows(i).Cells(4).Value = "LUNAS" Then
                dgvTindakan.Rows(i).Cells(4).Style.BackColor = Color.Green
                dgvTindakan.Rows(i).Cells(4).Style.ForeColor = Color.White
            End If
        Next

        For i As Integer = 0 To dgvTindakan.RowCount - 1
            dgvTindakan.Rows(i).Cells(6).Style.BackColor = Color.DodgerBlue
            dgvTindakan.Rows(i).Cells(6).Style.ForeColor = Color.White
        Next

        For i As Integer = 0 To dgvTindakan.Rows.Count - 1
            If i Mod 2 = 0 Then
                dgvTindakan.Rows(i).DefaultCellStyle.BackColor = Color.AliceBlue
            Else
                dgvTindakan.Rows(i).DefaultCellStyle.BackColor = Color.White
            End If
        Next

        For Each column As DataGridViewColumn In dgvTindakan.Columns
            column.SortMode = DataGridViewColumnSortMode.NotSortable
        Next
    End Sub

    Private Sub dgvTindakan_RowPostPaint(sender As Object, e As DataGridViewRowPostPaintEventArgs) Handles dgvTindakan.RowPostPaint
        Dim dg As DataGridView = DirectCast(sender, DataGridView)
        Dim rowNumber As String = (e.RowIndex + 1).ToString()

        Dim size As SizeF = e.Graphics.MeasureString(rowNumber, Me.Font)
        If dg.RowHeadersWidth < CInt(size.Width + 20) Then
            dg.RowHeadersWidth = CInt(size.Width + 20)
        End If

        Dim b As Brush = SystemBrushes.ControlText

        e.Graphics.DrawString(rowNumber, dg.Font, b, e.RowBounds.Location.X + 15, e.RowBounds.Location.Y + ((e.RowBounds.Height - size.Height) / 2))
    End Sub

    Private Sub dgvDetailTindakan_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles dgvDetailTindakan.CellFormatting
        dgvDetailTindakan.Columns(3).DefaultCellStyle.Format = "###,###,###"
        dgvDetailTindakan.Columns(6).DefaultCellStyle.Format = "###,###,###"
        dgvDetailTindakan.Columns(2).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        dgvDetailTindakan.Columns(3).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        dgvDetailTindakan.Columns(6).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        dgvDetailTindakan.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        dgvDetailTindakan.ColumnHeadersDefaultCellStyle.Font = New Font("Tahoma", 9, FontStyle.Bold)
        dgvDetailTindakan.ColumnHeadersDefaultCellStyle.BackColor = Color.DodgerBlue
        dgvDetailTindakan.ColumnHeadersDefaultCellStyle.ForeColor = Color.White
        dgvDetailTindakan.DefaultCellStyle.Font = New Font("Tahoma", 9, FontStyle.Bold)
        dgvDetailTindakan.DefaultCellStyle.SelectionBackColor = Color.PaleTurquoise
        dgvDetailTindakan.DefaultCellStyle.SelectionForeColor = Color.Black
        dgvDetailTindakan.AllowUserToResizeRows = False
        dgvDetailTindakan.EnableHeadersVisualStyles = False

        For i As Integer = 0 To dgvDetailTindakan.Rows.Count - 1
            If i Mod 2 = 0 Then
                dgvDetailTindakan.Rows(i).DefaultCellStyle.BackColor = Color.AliceBlue
            Else
                dgvDetailTindakan.Rows(i).DefaultCellStyle.BackColor = Color.White
            End If
        Next
    End Sub

    Private Sub dgvDetailTindakan_RowPostPaint(sender As Object, e As DataGridViewRowPostPaintEventArgs) Handles dgvDetailTindakan.RowPostPaint
        Dim dg As DataGridView = DirectCast(sender, DataGridView)
        Dim rowNumber As String = (e.RowIndex + 1).ToString()

        Dim size As SizeF = e.Graphics.MeasureString(rowNumber, Me.Font)
        If dg.RowHeadersWidth < CInt(size.Width + 20) Then
            dg.RowHeadersWidth = CInt(size.Width + 20)
        End If

        Dim b As Brush = SystemBrushes.ControlText

        e.Graphics.DrawString(rowNumber, dg.Font, b, e.RowBounds.Location.X + 15, e.RowBounds.Location.Y + ((e.RowBounds.Height - size.Height) / 2))
    End Sub

    Private Sub btnPrintTindakAll_MouseLeave(sender As Object, e As EventArgs) Handles btnPrintTindakAll.MouseLeave
        'Me.btnPrintTindakAll.BackColor = Color.Navy
    End Sub

    Private Sub btnPrintTindakAll_MouseEnter(sender As Object, e As EventArgs) Handles btnPrintTindakAll.MouseEnter
        'Me.btnPrintTindakAll.BackColor = Color.Blue
    End Sub

    Private Sub btnPrintTindakAll_Click(sender As Object, e As EventArgs) Handles btnPrintTindakAll.Click
        If btnPrintTindakAll.Text.Equals("PROSES") Then
            Dim num As Integer = 0
            Dim ran As New List(Of String)
            Dim noRan As String
            For Each row As DataGridViewRow In dgvTindakan.Rows
                If row.Cells(4).Value.ToString.Equals("BELUM LUNAS") Then
                    ran.Add("'" & row.Cells(5).Value.ToString & "'")
                    num = num + 1
                End If
            Next
            noRan = String.Join(",", ran.ToArray)

            If num > 0 Then
                Call LunasTindakanAll(noRan)
                Call DaftarTindakan()
                btnPrintTindakAll.BackColor = Color.Navy
                btnPrintTindakAll.Text = "PRINT"
            End If
        ElseIf btnPrintTindakAll.Text.Equals("PRINT") Then
            ViewNotaAllTindakan.Ambil_Data = True
            ViewNotaAllTindakan.Form_Ambil_Data = "TindakanAll"
            ViewNotaAllTindakan.Show()
        End If
    End Sub
#End Region
#Region "Laboratorium"
    Private Sub dgvLab_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvLab.CellClick
        Dim noTindakan, total, status As String

        If e.RowIndex = -1 Then
            Return
        End If

        noTindakan = dgvLab.Rows(e.RowIndex).Cells(4).Value.ToString
        total = dgvLab.Rows(e.RowIndex).Cells(2).Value.ToString
        status = dgvLab.Rows(e.RowIndex).Cells(3).Value.ToString
        txtNoTinLab.Text = noTindakan
        lab = noTindakan.Substring(0, 3)
        'txtStatus.Text = status
        'txtPerTotalLab.Text = "Rp " & CInt(total).ToString("#,##0")

        Call DetailLab(noTindakan)
        Call totalTarifDetailLab()
    End Sub

    Private Sub dgvLab_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvLab.CellContentClick
        Dim noTindakan, total, status As String

        If e.RowIndex = -1 Then
            Return
        End If

        noTindakan = dgvLab.Rows(e.RowIndex).Cells(4).Value.ToString
        total = dgvLab.Rows(e.RowIndex).Cells(2).Value.ToString
        status = dgvLab.Rows(e.RowIndex).Cells(3).Value.ToString
        txtNoTinLab.Text = noTindakan
        lab = noTindakan.Substring(0, 3)
        'txtStatus.Text = status
        'txtPerTotalLab.Text = "Rp " & CInt(total).ToString("#,##0")

        Call DetailLab(noTindakan)
        Call totalTarifDetailLab()

        Dim konfirmasi As MsgBoxResult
        If e.ColumnIndex = 5 Then
            Select Case dgvLab.Rows(e.RowIndex).Cells(3).Value.ToString
                Case "BELUM LUNAS"
                    konfirmasi = MsgBox("Apakah transaksi akan dibayar ?", vbQuestion + vbYesNo, "Laboratorium")
                    If konfirmasi = vbYes Then
                        Call LunasLab(noTindakan)
                        Call DaftarLab()
                        'MsgBox(tindakan & " - Memulai tindakan", MsgBoxStyle.Information)
                    End If
                Case "LUNAS"
                    konfirmasi = MsgBox("Apakah ingin mencetak nota ?", vbQuestion + vbYesNo, "Laboratorium")
                    If konfirmasi = vbYes Then
                        ViewNotaAllLaboratorium.Ambil_Data = True
                        ViewNotaAllLaboratorium.Form_Ambil_Data = "LabPernota"
                        ViewNotaAllLaboratorium.Show()
                    End If
            End Select
        End If
    End Sub

    Private Sub dgvLab_RowPostPaint(sender As Object, e As DataGridViewRowPostPaintEventArgs) Handles dgvLab.RowPostPaint
        Dim dg As DataGridView = DirectCast(sender, DataGridView)
        Dim rowNumber As String = (e.RowIndex + 1).ToString()

        Dim size As SizeF = e.Graphics.MeasureString(rowNumber, Me.Font)
        If dg.RowHeadersWidth < CInt(size.Width + 20) Then
            dg.RowHeadersWidth = CInt(size.Width + 20)
        End If

        Dim b As Brush = SystemBrushes.ControlText

        e.Graphics.DrawString(rowNumber, dg.Font, b, e.RowBounds.Location.X + 15, e.RowBounds.Location.Y + ((e.RowBounds.Height - size.Height) / 2))
    End Sub

    Private Sub dgvDetailLab_RowPostPaint(sender As Object, e As DataGridViewRowPostPaintEventArgs) Handles dgvDetailLab.RowPostPaint
        Dim dg As DataGridView = DirectCast(sender, DataGridView)
        Dim rowNumber As String = (e.RowIndex + 1).ToString()

        Dim size As SizeF = e.Graphics.MeasureString(rowNumber, Me.Font)
        If dg.RowHeadersWidth < CInt(size.Width + 20) Then
            dg.RowHeadersWidth = CInt(size.Width + 20)
        End If

        Dim b As Brush = SystemBrushes.ControlText

        e.Graphics.DrawString(rowNumber, dg.Font, b, e.RowBounds.Location.X + 15, e.RowBounds.Location.Y + ((e.RowBounds.Height - size.Height) / 2))
    End Sub

    Private Sub dgvLab_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles dgvLab.CellFormatting
        dgvLab.Columns(2).DefaultCellStyle.Format = "###,###,###"
        dgvLab.Columns(1).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        dgvLab.Columns(2).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        dgvLab.Columns(3).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        dgvLab.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        dgvLab.ColumnHeadersDefaultCellStyle.Font = New Font("Tahoma", 9, FontStyle.Bold)
        dgvLab.ColumnHeadersDefaultCellStyle.BackColor = Color.DodgerBlue
        dgvLab.ColumnHeadersDefaultCellStyle.ForeColor = Color.White
        dgvLab.DefaultCellStyle.Font = New Font("Tahoma", 9, FontStyle.Bold)
        dgvLab.DefaultCellStyle.SelectionBackColor = Color.PaleTurquoise
        dgvLab.DefaultCellStyle.SelectionForeColor = Color.Black
        dgvLab.AllowUserToResizeRows = False
        dgvLab.EnableHeadersVisualStyles = False

        For i As Integer = 0 To dgvLab.Rows.Count - 1
            If dgvLab.Rows(i).Cells(3).Value = "BELUM LUNAS" Then
                dgvLab.Rows(i).Cells(3).Style.BackColor = Color.Orange
                dgvLab.Rows(i).Cells(3).Style.ForeColor = Color.White
            ElseIf dgvLab.Rows(i).Cells(3).Value = "LUNAS" Then
                dgvLab.Rows(i).Cells(3).Style.BackColor = Color.Green
                dgvLab.Rows(i).Cells(3).Style.ForeColor = Color.White
            End If
        Next

        For i As Integer = 0 To dgvLab.RowCount - 1
            dgvLab.Rows(i).Cells(5).Style.BackColor = Color.DodgerBlue
            dgvLab.Rows(i).Cells(5).Style.ForeColor = Color.White
        Next

        For i As Integer = 0 To dgvLab.Rows.Count - 1
            If i Mod 2 = 0 Then
                dgvLab.Rows(i).DefaultCellStyle.BackColor = Color.AliceBlue
            Else
                dgvLab.Rows(i).DefaultCellStyle.BackColor = Color.White
            End If
        Next

        For Each column As DataGridViewColumn In dgvLab.Columns
            column.SortMode = DataGridViewColumnSortMode.NotSortable
        Next
    End Sub

    Private Sub dgvDetailLab_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles dgvDetailLab.CellFormatting
        dgvDetailLab.Columns(2).DefaultCellStyle.Format = "###,###,###"
        dgvDetailLab.Columns(4).DefaultCellStyle.Format = "###,###,###"
        dgvDetailLab.Columns(2).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        dgvDetailLab.Columns(3).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        dgvDetailLab.Columns(4).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        dgvDetailLab.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        dgvDetailLab.ColumnHeadersDefaultCellStyle.Font = New Font("Tahoma", 9, FontStyle.Bold)
        dgvDetailLab.ColumnHeadersDefaultCellStyle.BackColor = Color.DodgerBlue
        dgvDetailLab.ColumnHeadersDefaultCellStyle.ForeColor = Color.White
        dgvDetailLab.DefaultCellStyle.Font = New Font("Tahoma", 9, FontStyle.Bold)
        dgvDetailLab.DefaultCellStyle.SelectionBackColor = Color.PaleTurquoise
        dgvDetailLab.DefaultCellStyle.SelectionForeColor = Color.Black
        dgvDetailLab.AllowUserToResizeRows = False
        dgvDetailLab.EnableHeadersVisualStyles = False

        For i As Integer = 0 To dgvDetailLab.Rows.Count - 1
            If i Mod 2 = 0 Then
                dgvDetailLab.Rows(i).DefaultCellStyle.BackColor = Color.AliceBlue
            Else
                dgvDetailLab.Rows(i).DefaultCellStyle.BackColor = Color.White
            End If
        Next
    End Sub

    Private Sub btnPrintLabAll_MouseLeave(sender As Object, e As EventArgs) Handles btnPrintLabAll.MouseLeave
        'Me.btnPrintLabAll.BackColor = Color.Navy
    End Sub

    Private Sub btnPrintLabAll_MouseEnter(sender As Object, e As EventArgs) Handles btnPrintLabAll.MouseEnter
        'Me.btnPrintLabAll.BackColor = Color.Blue
    End Sub

    Private Sub btnPrintLabAll_Click(sender As Object, e As EventArgs) Handles btnPrintLabAll.Click
        If btnPrintLabAll.Text.Equals("PROSES") Then
            Dim num As Integer = 0
            Dim lab As New List(Of String)
            Dim noLab As String
            For Each row As DataGridViewRow In dgvLab.Rows
                If row.Cells(3).Value.ToString.Equals("BELUM LUNAS") Then
                    lab.Add("'" & row.Cells(4).Value.ToString & "'")
                    num = num + 1
                End If
            Next
            noLab = String.Join(",", lab.ToArray)

            If num > 0 Then
                Call LunasLabAll(noLab)
                Call DaftarLab()
                btnPrintLabAll.BackColor = Color.Navy
                btnPrintLabAll.Text = "PRINT"
            End If
        ElseIf btnPrintLabAll.Text.Equals("PRINT") Then
            ViewNotaAllLaboratorium.Ambil_Data = True
            ViewNotaAllLaboratorium.Form_Ambil_Data = "LabAll"
            ViewNotaAllLaboratorium.Show()
        End If
    End Sub

#End Region
#Region "Radiologi"
    Private Sub dgvRad_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvRad.CellClick
        Dim noTindakan, total, status As String

        If e.RowIndex = -1 Then
            Return
        End If

        noTindakan = dgvRad.Rows(e.RowIndex).Cells(4).Value.ToString
        total = dgvRad.Rows(e.RowIndex).Cells(2).Value.ToString
        status = dgvRad.Rows(e.RowIndex).Cells(3).Value.ToString
        txtNoTinRad.Text = noTindakan
        'txtStatus.Text = status
        'txtPerTotalRad.Text = "Rp " & CInt(total).ToString("#,##0")

        Call DetailRad(noTindakan)
        Call totalTarifDetailRad()
    End Sub

    Private Sub dgvRad_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvRad.CellContentClick
        Dim noTindakan, total, status As String

        If e.RowIndex = -1 Then
            Return
        End If

        noTindakan = dgvRad.Rows(e.RowIndex).Cells(4).Value.ToString
        total = dgvRad.Rows(e.RowIndex).Cells(2).Value.ToString
        status = dgvRad.Rows(e.RowIndex).Cells(3).Value.ToString
        txtNoTinRad.Text = noTindakan
        'txtStatus.Text = status
        'txtPerTotalRad.Text = "Rp " & CInt(total).ToString("#,##0")

        Call DetailRad(noTindakan)
        Call totalTarifDetailRad()

        Dim konfirmasi As MsgBoxResult
        If e.ColumnIndex = 5 Then
            Select Case dgvRad.Rows(e.RowIndex).Cells(3).Value.ToString
                Case "BELUM LUNAS"
                    konfirmasi = MsgBox("Apakah transaksi akan dibayar ?", vbQuestion + vbYesNo, "Radiologi")
                    If konfirmasi = vbYes Then
                        Call LunasRad(noTindakan)
                        Call DaftarRad()
                        'MsgBox(tindakan & " - Memulai tindakan", MsgBoxStyle.Information)
                    End If
                Case "LUNAS"
                    konfirmasi = MsgBox("Apakah ingin mencetak nota ?", vbQuestion + vbYesNo, "Radiologi")
                    If konfirmasi = vbYes Then
                        ViewNotaAllRadiologi.Ambil_Data = True
                        ViewNotaAllRadiologi.Form_Ambil_Data = "RadPernota"
                        ViewNotaAllRadiologi.Show()
                    End If
            End Select
        End If
    End Sub

    Private Sub dgvRad_RowPostPaint(sender As Object, e As DataGridViewRowPostPaintEventArgs) Handles dgvRad.RowPostPaint
        Dim dg As DataGridView = DirectCast(sender, DataGridView)
        Dim rowNumber As String = (e.RowIndex + 1).ToString()

        Dim size As SizeF = e.Graphics.MeasureString(rowNumber, Me.Font)
        If dg.RowHeadersWidth < CInt(size.Width + 20) Then
            dg.RowHeadersWidth = CInt(size.Width + 20)
        End If

        Dim b As Brush = SystemBrushes.ControlText

        e.Graphics.DrawString(rowNumber, dg.Font, b, e.RowBounds.Location.X + 15, e.RowBounds.Location.Y + ((e.RowBounds.Height - size.Height) / 2))
    End Sub

    Private Sub dgvDetailRad_RowPostPaint(sender As Object, e As DataGridViewRowPostPaintEventArgs) Handles dgvDetailRad.RowPostPaint
        Dim dg As DataGridView = DirectCast(sender, DataGridView)
        Dim rowNumber As String = (e.RowIndex + 1).ToString()

        Dim size As SizeF = e.Graphics.MeasureString(rowNumber, Me.Font)
        If dg.RowHeadersWidth < CInt(size.Width + 20) Then
            dg.RowHeadersWidth = CInt(size.Width + 20)
        End If

        Dim b As Brush = SystemBrushes.ControlText

        e.Graphics.DrawString(rowNumber, dg.Font, b, e.RowBounds.Location.X + 15, e.RowBounds.Location.Y + ((e.RowBounds.Height - size.Height) / 2))
    End Sub

    Private Sub dgvRad_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles dgvRad.CellFormatting
        dgvRad.Columns(2).DefaultCellStyle.Format = "###,###,###"
        dgvRad.Columns(1).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        dgvRad.Columns(2).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        dgvRad.Columns(3).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        dgvRad.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        dgvRad.ColumnHeadersDefaultCellStyle.Font = New Font("Tahoma", 9, FontStyle.Bold)
        dgvRad.ColumnHeadersDefaultCellStyle.BackColor = Color.DodgerBlue
        dgvRad.ColumnHeadersDefaultCellStyle.ForeColor = Color.White
        dgvRad.DefaultCellStyle.Font = New Font("Tahoma", 9, FontStyle.Bold)
        dgvRad.DefaultCellStyle.SelectionBackColor = Color.PaleTurquoise
        dgvRad.DefaultCellStyle.SelectionForeColor = Color.Black
        dgvRad.AllowUserToResizeRows = False
        dgvRad.EnableHeadersVisualStyles = False

        For i As Integer = 0 To dgvRad.Rows.Count - 1
            If dgvRad.Rows(i).Cells(3).Value = "BELUM LUNAS" Then
                dgvRad.Rows(i).Cells(3).Style.BackColor = Color.Orange
                dgvRad.Rows(i).Cells(3).Style.ForeColor = Color.White
            ElseIf dgvRad.Rows(i).Cells(3).Value = "LUNAS" Then
                dgvRad.Rows(i).Cells(3).Style.BackColor = Color.Green
                dgvRad.Rows(i).Cells(3).Style.ForeColor = Color.White
            End If
        Next

        For i As Integer = 0 To dgvRad.RowCount - 1
            dgvRad.Rows(i).Cells(5).Style.BackColor = Color.DodgerBlue
            dgvRad.Rows(i).Cells(5).Style.ForeColor = Color.White
        Next

        For i As Integer = 0 To dgvRad.Rows.Count - 1
            If i Mod 2 = 0 Then
                dgvRad.Rows(i).DefaultCellStyle.BackColor = Color.AliceBlue
            Else
                dgvRad.Rows(i).DefaultCellStyle.BackColor = Color.White
            End If
        Next

        For Each column As DataGridViewColumn In dgvRad.Columns
            column.SortMode = DataGridViewColumnSortMode.NotSortable
        Next
    End Sub

    Private Sub dgvDetailRad_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles dgvDetailRad.CellFormatting
        dgvDetailRad.Columns(2).DefaultCellStyle.Format = "###,###,###"
        dgvDetailRad.Columns(4).DefaultCellStyle.Format = "###,###,###"
        dgvDetailRad.Columns(2).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        dgvDetailRad.Columns(3).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        dgvDetailRad.Columns(4).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        dgvDetailRad.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        dgvDetailRad.ColumnHeadersDefaultCellStyle.Font = New Font("Tahoma", 9, FontStyle.Bold)
        dgvDetailRad.ColumnHeadersDefaultCellStyle.BackColor = Color.DodgerBlue
        dgvDetailRad.ColumnHeadersDefaultCellStyle.ForeColor = Color.White
        dgvDetailRad.DefaultCellStyle.Font = New Font("Tahoma", 9, FontStyle.Bold)
        dgvDetailRad.DefaultCellStyle.SelectionBackColor = Color.PaleTurquoise
        dgvDetailRad.DefaultCellStyle.SelectionForeColor = Color.Black
        dgvDetailRad.AllowUserToResizeRows = False
        dgvDetailRad.EnableHeadersVisualStyles = False

        For i As Integer = 0 To dgvDetailRad.Rows.Count - 1
            If i Mod 2 = 0 Then
                dgvDetailRad.Rows(i).DefaultCellStyle.BackColor = Color.AliceBlue
            Else
                dgvDetailRad.Rows(i).DefaultCellStyle.BackColor = Color.White
            End If
        Next
    End Sub

    Private Sub btnPrintRadAll_MouseLeave(sender As Object, e As EventArgs) Handles btnPrintRadAll.MouseLeave
        'Me.btnPrintRadAll.BackColor = Color.Navy
    End Sub

    Private Sub btnPrintRadAll_MouseEnter(sender As Object, e As EventArgs) Handles btnPrintRadAll.MouseEnter
        'Me.btnPrintRadAll.BackColor = Color.Blue
    End Sub

    Private Sub btnPrintRadAll_Click(sender As Object, e As EventArgs) Handles btnPrintRadAll.Click
        If btnPrintRadAll.Text.Equals("PROSES") Then
            Dim num As Integer = 0
            Dim rad As New List(Of String)
            Dim noRad As String
            For Each row As DataGridViewRow In dgvRad.Rows
                If row.Cells(3).Value.ToString.Equals("BELUM LUNAS") Then
                    rad.Add("'" & row.Cells(4).Value.ToString & "'")
                    num = num + 1
                End If
            Next
            noRad = String.Join(",", rad.ToArray)

            If num > 0 Then
                Call LunasRadAll(noRad)
                Call DaftarRad()
                btnPrintRadAll.BackColor = Color.Navy
                btnPrintRadAll.Text = "PRINT"
            End If
        ElseIf btnPrintRadAll.Text.Equals("PRINT") Then
            ViewNotaAllRadiologi.Ambil_Data = True
            ViewNotaAllRadiologi.Form_Ambil_Data = "RadAll"
            ViewNotaAllRadiologi.Show()
        End If
    End Sub
#End Region
#Region "Obat"
    Private Sub dgvObat_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvObat.CellClick
        Dim noTransaksi, total, status As String

        If e.RowIndex = -1 Then
            Return
        End If

        noTransaksi = dgvObat.Rows(e.RowIndex).Cells(1).Value.ToString
        total = dgvObat.Rows(e.RowIndex).Cells(3).Value.ToString
        status = dgvObat.Rows(e.RowIndex).Cells(4).Value.ToString
        txtNoJualObat.Text = noTransaksi
        obt = noTransaksi.Substring(2, 2)
        'txtStatus.Text = status
        'txtPerTotalObat.Text = "Rp " & CInt((Math.Ceiling(total / 100) * 100)).ToString("#,##0")

        Call DetailObat(noTransaksi)
        Call totalTarifDetailObat()
    End Sub

    Private Sub dgvObat_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvObat.CellContentClick
        Dim noTransaksi, total, status As String

        If e.RowIndex = -1 Then
            Return
        End If

        noTransaksi = dgvObat.Rows(e.RowIndex).Cells(1).Value.ToString
        total = dgvObat.Rows(e.RowIndex).Cells(3).Value.ToString
        status = dgvObat.Rows(e.RowIndex).Cells(4).Value.ToString
        txtNoJualObat.Text = noTransaksi
        obt = noTransaksi.Substring(2, 2)
        'txtStatus.Text = status
        'txtPerTotalObat.Text = "Rp " & CInt((Math.Ceiling(total / 100) * 100)).ToString("#,##0")

        Call DetailObat(noTransaksi)
        Call totalTarifDetailObat()

        Dim konfirmasi As MsgBoxResult
        If e.ColumnIndex = 5 Then
            Select Case dgvObat.Rows(e.RowIndex).Cells(4).Value.ToString
                Case "BELUM LUNAS"
                    konfirmasi = MsgBox("Apakah transaksi akan dibayar ?", vbQuestion + vbYesNo, "Obat")
                    If konfirmasi = vbYes Then
                        Call LunasObat(noTransaksi)
                        Call DaftarObat()
                        'MsgBox(tindakan & " - Memulai tindakan", MsgBoxStyle.Information)
                    End If
                Case "LUNAS"
                    konfirmasi = MsgBox("Apakah ingin mencetak nota ?", vbQuestion + vbYesNo, "Obat")
                    If konfirmasi = vbYes Then
                        ViewNotaAllObat.Ambil_Data = True
                        ViewNotaAllObat.Form_Ambil_Data = "ObatPernota"
                        ViewNotaAllObat.Show()
                    End If
            End Select
        End If
    End Sub

    Private Sub dgvObat_RowPostPaint(sender As Object, e As DataGridViewRowPostPaintEventArgs) Handles dgvObat.RowPostPaint
        Dim dg As DataGridView = DirectCast(sender, DataGridView)
        Dim rowNumber As String = (e.RowIndex + 1).ToString()

        Dim size As SizeF = e.Graphics.MeasureString(rowNumber, Me.Font)
        If dg.RowHeadersWidth < CInt(size.Width + 20) Then
            dg.RowHeadersWidth = CInt(size.Width + 20)
        End If

        Dim b As Brush = SystemBrushes.ControlText

        e.Graphics.DrawString(rowNumber, dg.Font, b, e.RowBounds.Location.X + 15, e.RowBounds.Location.Y + ((e.RowBounds.Height - size.Height) / 2))
    End Sub

    Private Sub dgvDetailObat_RowPostPaint(sender As Object, e As DataGridViewRowPostPaintEventArgs) Handles dgvDetailObat.RowPostPaint
        Dim dg As DataGridView = DirectCast(sender, DataGridView)
        Dim rowNumber As String = (e.RowIndex + 1).ToString()

        Dim size As SizeF = e.Graphics.MeasureString(rowNumber, Me.Font)
        If dg.RowHeadersWidth < CInt(size.Width + 20) Then
            dg.RowHeadersWidth = CInt(size.Width + 20)
        End If

        Dim b As Brush = SystemBrushes.ControlText

        e.Graphics.DrawString(rowNumber, dg.Font, b, e.RowBounds.Location.X + 15, e.RowBounds.Location.Y + ((e.RowBounds.Height - size.Height) / 2))
    End Sub

    Private Sub dgvObat_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles dgvObat.CellFormatting
        dgvObat.Columns(3).DefaultCellStyle.Format = "N2"
        dgvObat.Columns(2).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        dgvObat.Columns(3).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        dgvObat.Columns(4).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        dgvObat.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        dgvObat.ColumnHeadersDefaultCellStyle.Font = New Font("Tahoma", 9, FontStyle.Bold)
        dgvObat.ColumnHeadersDefaultCellStyle.BackColor = Color.DodgerBlue
        dgvObat.ColumnHeadersDefaultCellStyle.ForeColor = Color.White
        dgvObat.DefaultCellStyle.Font = New Font("Tahoma", 9, FontStyle.Bold)
        dgvObat.DefaultCellStyle.SelectionBackColor = Color.PaleTurquoise
        dgvObat.DefaultCellStyle.SelectionForeColor = Color.Black
        dgvObat.AllowUserToResizeRows = False
        dgvObat.EnableHeadersVisualStyles = False

        For i As Integer = 0 To dgvObat.Rows.Count - 1
            If dgvObat.Rows(i).Cells(4).Value = "BELUM LUNAS" Then
                dgvObat.Rows(i).Cells(4).Style.BackColor = Color.Orange
                dgvObat.Rows(i).Cells(4).Style.ForeColor = Color.White
            ElseIf dgvObat.Rows(i).Cells(4).Value = "LUNAS" Then
                dgvObat.Rows(i).Cells(4).Style.BackColor = Color.Green
                dgvObat.Rows(i).Cells(4).Style.ForeColor = Color.White
            End If
        Next

        For i As Integer = 0 To dgvObat.RowCount - 1
            dgvObat.Rows(i).Cells(5).Style.BackColor = Color.DodgerBlue
            dgvObat.Rows(i).Cells(5).Style.ForeColor = Color.White
        Next

        For i As Integer = 0 To dgvObat.Rows.Count - 1
            If i Mod 2 = 0 Then
                dgvObat.Rows(i).DefaultCellStyle.BackColor = Color.AliceBlue
            Else
                dgvObat.Rows(i).DefaultCellStyle.BackColor = Color.White
            End If
        Next

        For Each column As DataGridViewColumn In dgvObat.Columns
            column.SortMode = DataGridViewColumnSortMode.NotSortable
        Next
    End Sub

    Private Sub dgvDetailObat_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles dgvDetailObat.CellFormatting
        dgvDetailObat.Columns(3).DefaultCellStyle.Format = "N2"
        dgvDetailObat.Columns(5).DefaultCellStyle.Format = "N2"
        dgvDetailObat.Columns(3).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        dgvDetailObat.Columns(4).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        dgvDetailObat.Columns(5).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        dgvDetailObat.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        dgvDetailObat.ColumnHeadersDefaultCellStyle.Font = New Font("Tahoma", 9, FontStyle.Bold)
        dgvDetailObat.ColumnHeadersDefaultCellStyle.BackColor = Color.DodgerBlue
        dgvDetailObat.ColumnHeadersDefaultCellStyle.ForeColor = Color.White
        dgvDetailObat.DefaultCellStyle.Font = New Font("Tahoma", 9, FontStyle.Bold)
        dgvDetailObat.DefaultCellStyle.SelectionBackColor = Color.PaleTurquoise
        dgvDetailObat.DefaultCellStyle.SelectionForeColor = Color.Black
        dgvDetailObat.AllowUserToResizeRows = False
        dgvDetailObat.EnableHeadersVisualStyles = False

        For i As Integer = 0 To dgvDetailObat.Rows.Count - 1
            If i Mod 2 = 0 Then
                dgvDetailObat.Rows(i).DefaultCellStyle.BackColor = Color.AliceBlue
            Else
                dgvDetailObat.Rows(i).DefaultCellStyle.BackColor = Color.White
            End If
        Next
    End Sub

    Private Sub btnPrintObatAll_MouseLeave(sender As Object, e As EventArgs) Handles btnPrintObatAll.MouseLeave
        'Me.btnPrintObatAll.BackColor = Color.Navy
    End Sub

    Private Sub btnPrintObatAll_MouseEnter(sender As Object, e As EventArgs) Handles btnPrintObatAll.MouseEnter
        'Me.btnPrintObatAll.BackColor = Color.Blue
    End Sub

    Private Sub btnPrintObatAll_Click(sender As Object, e As EventArgs) Handles btnPrintObatAll.Click
        If txtCaraBayar.Text.Equals("Umum", StringComparison.OrdinalIgnoreCase) Then
            If btnPrintObatAll.Text.Equals("PROSES") Then
                Dim num As Integer = 0
                Dim obt As New List(Of String)
                Dim noObt As String
                For Each row As DataGridViewRow In dgvObat.Rows
                    If row.Cells(4).Value.ToString.Equals("BELUM LUNAS") Then
                        obt.Add("'" & row.Cells(1).Value.ToString & "'")
                        num = num + 1
                    End If
                Next
                noObt = String.Join(",", obt.ToArray)

                If num > 0 Then
                    Call LunasObatAll(noObt)
                    Call DaftarObat()
                    btnPrintObatAll.BackColor = Color.Navy
                    btnPrintObatAll.Text = "PRINT"
                End If
            ElseIf btnPrintObatAll.Text.Equals("PRINT") Then
                ViewNotaAllObat.Ambil_Data = True
                ViewNotaAllObat.Form_Ambil_Data = "ObatAll"
                ViewNotaAllObat.Show()
            End If
        Else
            ViewNotaAllObat.Ambil_Data = True
            ViewNotaAllObat.Form_Ambil_Data = "ObatAll"
            ViewNotaAllObat.Show()
        End If
    End Sub
#End Region
#Region "All"
    Private Sub dgvAll_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvAll.CellClick
        Dim noTindakan, total, status As String

        If e.RowIndex = -1 Then
            Return
        End If

        noTindakan = dgvAll.Rows(e.RowIndex).Cells(5).Value.ToString
        total = dgvAll.Rows(e.RowIndex).Cells(3).Value.ToString
        status = dgvAll.Rows(e.RowIndex).Cells(4).Value.ToString
        txtNoTindakAll.Text = noTindakan
        'txtStatus.Text = status
        txtPertotalAll.Text = "Rp " & CInt(total).ToString("#,##0")
        Call DetailAll(noTindakan)
    End Sub

    Private Sub dgvAll_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvAll.CellContentClick
        Dim noTindakan, total, status As String

        If e.RowIndex = -1 Then
            Return
        End If

        noTindakan = dgvAll.Rows(e.RowIndex).Cells(5).Value.ToString
        total = dgvAll.Rows(e.RowIndex).Cells(3).Value.ToString
        status = dgvAll.Rows(e.RowIndex).Cells(4).Value.ToString
        txtNoTindakAll.Text = noTindakan
        'txtStatus.Text = status
        txtPertotalAll.Text = "Rp " & CInt(total).ToString("#,##0")

        Call DetailAll(noTindakan)

        'Dim konfirmasi As MsgBoxResult
        'If e.ColumnIndex = 6 Then
        '    Select Case dgvAll.Rows(e.RowIndex).Cells(4).Value.ToString
        '        Case "BELUM LUNAS"
        '            konfirmasi = MsgBox("Apakah transaksi akan dibayar ?", vbQuestion + vbYesNo, "Tindakan")
        '            If konfirmasi = vbYes Then
        '                'Call LunasAll(noTindakan)
        '                'Call DaftarAll()
        '                'MsgBox(tindakan & " - Memulai tindakan", MsgBoxStyle.Information)
        '            End If
        '        Case "LUNAS"
        '            konfirmasi = MsgBox("Apakah ingin mencetak nota ?", vbQuestion + vbYesNo, "Tindakan")
        '            If konfirmasi = vbYes Then

        '            End If
        '    End Select
        'End If

    End Sub

    Private Sub dgvAll_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles dgvAll.CellFormatting

        dgvAll.Columns(3).DefaultCellStyle.Format = "###,###,###"
        dgvAll.Columns(1).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        dgvAll.Columns(2).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        dgvAll.Columns(3).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        dgvAll.Columns(4).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        dgvAll.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        dgvAll.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        dgvAll.ColumnHeadersDefaultCellStyle.Font = New Font("Tahoma", 9, FontStyle.Bold)
        dgvAll.ColumnHeadersDefaultCellStyle.BackColor = Color.DodgerBlue
        dgvAll.ColumnHeadersDefaultCellStyle.ForeColor = Color.White
        dgvAll.DefaultCellStyle.Font = New Font("Tahoma", 9, FontStyle.Bold)
        dgvAll.DefaultCellStyle.SelectionBackColor = Color.PaleTurquoise
        dgvAll.DefaultCellStyle.SelectionForeColor = Color.Black
        dgvAll.AllowUserToResizeRows = False
        dgvAll.EnableHeadersVisualStyles = False

        For i As Integer = 0 To dgvAll.Rows.Count - 1
            If dgvAll.Rows(i).Cells(4).Value = "BELUM LUNAS" Then
                dgvAll.Rows(i).Cells(4).Style.BackColor = Color.Orange
                dgvAll.Rows(i).Cells(4).Style.ForeColor = Color.White
            ElseIf dgvAll.Rows(i).Cells(4).Value = "LUNAS" Then
                dgvAll.Rows(i).Cells(4).Style.BackColor = Color.Green
                dgvAll.Rows(i).Cells(4).Style.ForeColor = Color.White
            End If
        Next

        For i As Integer = 0 To dgvAll.RowCount - 1
            dgvAll.Rows(i).Cells(6).Style.BackColor = Color.DodgerBlue
            dgvAll.Rows(i).Cells(6).Style.ForeColor = Color.White
        Next

        For i As Integer = 0 To dgvAll.Rows.Count - 1
            If i Mod 2 = 0 Then
                dgvAll.Rows(i).DefaultCellStyle.BackColor = Color.AliceBlue
            Else
                dgvAll.Rows(i).DefaultCellStyle.BackColor = Color.White
            End If
        Next

        For Each column As DataGridViewColumn In dgvAll.Columns
            column.SortMode = DataGridViewColumnSortMode.NotSortable
        Next
    End Sub

    Private Sub dgvAll_RowPostPaint(sender As Object, e As DataGridViewRowPostPaintEventArgs) Handles dgvAll.RowPostPaint
        Dim dg As DataGridView = DirectCast(sender, DataGridView)
        Dim rowNumber As String = (e.RowIndex + 1).ToString()

        Dim size As SizeF = e.Graphics.MeasureString(rowNumber, Me.Font)
        If dg.RowHeadersWidth < CInt(size.Width + 20) Then
            dg.RowHeadersWidth = CInt(size.Width + 20)
        End If

        Dim b As Brush = SystemBrushes.ControlText

        e.Graphics.DrawString(rowNumber, dg.Font, b, e.RowBounds.Location.X + 15, e.RowBounds.Location.Y + ((e.RowBounds.Height - size.Height) / 2))
    End Sub

    Private Sub dgvDetailAll_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles dgvDetailAll.CellFormatting
        dgvDetailAll.Columns(3).DefaultCellStyle.Format = "###,###,###"
        dgvDetailAll.Columns(6).DefaultCellStyle.Format = "###,###,###"
        dgvDetailAll.Columns(2).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        dgvDetailAll.Columns(3).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        dgvDetailAll.Columns(6).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        dgvDetailAll.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        dgvDetailAll.ColumnHeadersDefaultCellStyle.Font = New Font("Tahoma", 9, FontStyle.Bold)
        dgvDetailAll.ColumnHeadersDefaultCellStyle.BackColor = Color.DodgerBlue
        dgvDetailAll.ColumnHeadersDefaultCellStyle.ForeColor = Color.White
        dgvDetailAll.DefaultCellStyle.Font = New Font("Tahoma", 9, FontStyle.Bold)
        dgvDetailAll.DefaultCellStyle.SelectionBackColor = Color.PaleTurquoise
        dgvDetailAll.DefaultCellStyle.SelectionForeColor = Color.Black
        dgvDetailAll.AllowUserToResizeRows = False
        dgvDetailAll.EnableHeadersVisualStyles = False

        For i As Integer = 0 To dgvDetailAll.Rows.Count - 1
            If i Mod 2 = 0 Then
                dgvDetailAll.Rows(i).DefaultCellStyle.BackColor = Color.AliceBlue
            Else
                dgvDetailAll.Rows(i).DefaultCellStyle.BackColor = Color.White
            End If
        Next
    End Sub

    Private Sub dgvDetailAll_RowPostPaint(sender As Object, e As DataGridViewRowPostPaintEventArgs) Handles dgvDetailAll.RowPostPaint
        Dim dg As DataGridView = DirectCast(sender, DataGridView)
        Dim rowNumber As String = (e.RowIndex + 1).ToString()

        Dim size As SizeF = e.Graphics.MeasureString(rowNumber, Me.Font)
        If dg.RowHeadersWidth < CInt(size.Width + 20) Then
            dg.RowHeadersWidth = CInt(size.Width + 20)
        End If

        Dim b As Brush = SystemBrushes.ControlText

        e.Graphics.DrawString(rowNumber, dg.Font, b, e.RowBounds.Location.X + 15, e.RowBounds.Location.Y + ((e.RowBounds.Height - size.Height) / 2))
    End Sub

    Private Sub btnPrintAll_MouseLeave(sender As Object, e As EventArgs) Handles btnPrintAll.MouseLeave
        Me.btnPrintAll.BackColor = Color.Navy
    End Sub

    Private Sub btnPrintAll_MouseEnter(sender As Object, e As EventArgs) Handles btnPrintAll.MouseEnter
        Me.btnPrintAll.BackColor = Color.Blue
    End Sub

    Private Sub btnPrintAll_Click(sender As Object, e As EventArgs) Handles btnPrintAll.Click
        Try
            Dim angka As Double = 0
            If Double.TryParse(txtTotalBayar.Text, angka) Then
                txtTerbilang.Text = Terbilang(angka) & " Rupiah"
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

        Select Case unit
            Case "IG"
                ViewNotaTotalIGD.Show()
            Case "RJ"
            Case "RI"
                ViewNotaAllTotal.Show()
        End Select
    End Sub

    Private Sub btnPrintForPx_MouseLeave(sender As Object, e As EventArgs) Handles btnPrintForPx.MouseLeave
        Me.btnPrintForPx.BackColor = Color.Navy
    End Sub

    Private Sub btnPrintForPx_MouseEnter(sender As Object, e As EventArgs) Handles btnPrintForPx.MouseEnter
        Me.btnPrintForPx.BackColor = Color.Blue
    End Sub

    Private Sub btnPrintForPx_Click(sender As Object, e As EventArgs) Handles btnPrintForPx.Click
        Select Case unit
            Case "IG"
                ViewNotaForPx.Show()
                ViewKwitansi.Show()
            Case "RI"
                If txtRIK.Text = "-" Then
                    ViewNotaForPxRanap.Show()
                    ViewKwitansi.Show()
                ElseIf txtRIK.Text = "RIK" Then
                    ViewNotaForPxRIK.Show()
                    ViewKwitansi.Show()
                End If
                'ViewNotaForPxRanap.Show()
                'ViewKwitansi.Show()
        End Select
    End Sub

    Private Sub btnBayarRanap_Click(sender As Object, e As EventArgs) Handles btnBayarRanap.Click
        Dim tin As New List(Of String)
        Dim lab As New List(Of String)
        Dim rad As New List(Of String)
        Dim had As New List(Of String)
        Dim oka As New List(Of String)
        Dim noTin As String
        Dim noLab As String
        Dim noRad As String
        Dim noHad As String
        Dim noOka As String
        Dim numTin As Integer = 0
        Dim numLab As Integer = 0
        Dim numRad As Integer = 0
        Dim numHad As Integer = 0
        Dim numOka As Integer = 0
        For Each row As DataGridViewRow In dgvAll.Rows
            If Not row.Cells(2).Value.ToString.Equals("Laboratorium") And
                Not row.Cells(2).Value.ToString.Equals("Radiologi") And
                Not row.Cells(2).Value.ToString.Equals("Hemodialisa") And
                Not (row.Cells(2).Value.ToString.Equals("Kamar Operasi") Or row.Cells(2).Value.ToString.Equals("OK Paru")) And
                row.Cells(4).Value.ToString.Equals("BELUM LUNAS") Then
                tin.Add("'" & row.Cells(5).Value.ToString & "'")
                numTin = numTin + 1
            End If
            If (row.Cells(2).Value.ToString.Equals("Laboratorium") Or
                row.Cells(2).Value.ToString.Equals("Patologi") Or
                row.Cells(2).Value.ToString.Equals("Bank Darah")) And row.Cells(4).Value.ToString.Equals("BELUM LUNAS") Then
                lab.Add("'" & row.Cells(5).Value.ToString & "'")
                numLab = numLab + 1
            End If
            If row.Cells(2).Value.ToString.Equals("Radiologi") And row.Cells(4).Value.ToString.Equals("BELUM LUNAS") Then
                rad.Add("'" & row.Cells(5).Value.ToString & "'")
                numRad = numRad + 1
            End If
            If row.Cells(2).Value.ToString.Equals("Hemodialisa") And row.Cells(4).Value.ToString.Equals("BELUM LUNAS") Then
                rad.Add("'" & row.Cells(5).Value.ToString & "'")
                numHad = numHad + 1
            End If
            If (row.Cells(2).Value.ToString.Equals("Kamar Operasi") Or row.Cells(2).Value.ToString.Equals("OK Paru")) And row.Cells(4).Value.ToString.Equals("BELUM LUNAS") Then
                rad.Add("'" & row.Cells(5).Value.ToString & "'")
                numOka = numOka + 1
            End If
        Next
        noTin = String.Join(",", tin.ToArray)
        noLab = String.Join(",", lab.ToArray)
        noRad = String.Join(",", rad.ToArray)
        noHad = String.Join(",", had.ToArray)
        noOka = String.Join(",", oka.ToArray)

        If numTin > 0 Then
            Call LunasAllTindak(noTin)
        End If

        If numLab > 0 Then
            Call LunasAllLab(noLab)
        End If

        If numRad > 0 Then
            Call LunasAllRad(noRad)
        End If

        If numHad > 0 Then
            Call LunasAllHemo(noHad)
        End If

        If numOka > 0 Then
            Call LunasAllOKnParu(noOka)
        End If

        Call DaftarAll()

        If txtCaraBayar.Text.Equals("Umum", StringComparison.OrdinalIgnoreCase) Then
            Dim BLCount As Integer = 0
            For Each row As DataGridViewRow In dgvAll.Rows
                If row.Cells(4).Value.ToString.Equals("BELUM LUNAS") Then
                    BLCount = BLCount + 1
                End If
            Next

            If BLCount > 0 Then
                btnBayarRanap.Enabled = True
                btnBayarRanap.Text = "PROSES PEMBAYARAN"
            Else
                btnBayarRanap.Enabled = False
                btnBayarRanap.Text = "LUNAS"
            End If
        Else
            btnBayarRanap.Enabled = False
            btnBayarRanap.Text = "PROSES PEMBAYARAN"
        End If
    End Sub
#End Region
#Region "Karcis"

    Private Sub txtKarStatus_TextChanged(sender As Object, e As EventArgs) Handles txtKarStatus.TextChanged
        If txtKarStatus.Text.Equals("LUNAS") Then
            btnKarcis.Text = "PRINT"
            btnKarcis.Enabled = True
            txtKarStatus.BackColor = Color.PaleGreen
            btnKarcis.BackColor = Color.Navy
        Else
            btnKarcis.Text = "PROSES"
            btnKarcis.Enabled = True
            txtKarStatus.BackColor = Color.LightCoral
            btnKarcis.BackColor = Color.Green
        End If
    End Sub

    Private Sub btnKarcis_Click(sender As Object, e As EventArgs) Handles btnKarcis.Click
        If btnKarcis.Text.Equals("PRINT") Then
            ViewNotaKarcis.Show()
        ElseIf btnKarcis.Text.Equals("PROSES") Then
            If txtCaraBayar.Text.Equals("UMUM", StringComparison.OrdinalIgnoreCase) Then
                'Dim konfirmasi As MsgBoxResult
                'konfirmasi = MsgBox("Apakah pembayaran tunai atau via Bank Jatim ? ", vbQuestion + vbYesNo, "Konfirmasi")
                'If konfirmasi = vbYes Then
                '    Call randUniqNum()
                '    jatim.VirtualAccount = txtVaUnit.Text & txtRekMed.Text & va
                '    jatim.Amount = txtTotalBayar.Text
                '    jatim.Reference = "000000000012"
                '    jatim.Tanggal = dtBank
                '    Dim result = JsonConvert.SerializeObject(jatim)
                '    Call LunasJatim(jatim.VirtualAccount, jatim.Reference)
                '    'MsgBox(result)
                'Else
                Call LunasKarcis(txtRegUnit.Text)
                Call LoadKarcis(txtRegUnit.Text)
                'End If
            Else
                Call LunasKarcis(txtRegUnit.Text)
                Call LoadKarcis(txtRegUnit.Text)
            End If

        End If
    End Sub
#End Region
#Region "Operasi"
    Private Sub dgvOp_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvOp.CellClick
        Dim noTindakan, total, status As String

        If e.RowIndex = -1 Then
            Return
        End If

        noTindakan = dgvOp.Rows(e.RowIndex).Cells(5).Value.ToString
        total = dgvOp.Rows(e.RowIndex).Cells(3).Value.ToString
        status = dgvOp.Rows(e.RowIndex).Cells(4).Value.ToString
        txtNoTinOp.Text = noTindakan
        'txtStatus.Text = status
        'txtPerTotalTindakan.Text = "Rp " & CInt(total).ToString("#,##0")

        Call DetailOperasi(noTindakan)
        Call totalTarifDetailOp()
    End Sub

    Private Sub dgvOp_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvOp.CellContentClick
        Dim noTindakan, total, status As String

        If e.RowIndex = -1 Then
            Return
        End If

        noTindakan = dgvOp.Rows(e.RowIndex).Cells(5).Value.ToString
        total = dgvOp.Rows(e.RowIndex).Cells(3).Value.ToString
        status = dgvOp.Rows(e.RowIndex).Cells(4).Value.ToString
        txtNoTinOp.Text = noTindakan
        'txtStatus.Text = status
        'txtPerTotalTindakan.Text = "Rp " & CInt(total).ToString("#,##0")

        Call DetailOperasi(noTindakan)
        Call totalTarifDetailOp()

        Dim konfirmasi As MsgBoxResult
        If e.ColumnIndex = 6 Then
            Select Case dgvOp.Rows(e.RowIndex).Cells(4).Value.ToString
                Case "BELUM LUNAS"
                    konfirmasi = MsgBox("Apakah transaksi akan dibayar ?", vbQuestion + vbYesNo, "Tindakan")
                    If konfirmasi = vbYes Then
                        Call LunasOperasi(noTindakan)
                        Call DaftarOperasi()
                        'MsgBox(tindakan & " - Memulai tindakan", MsgBoxStyle.Information)
                    End If
                Case "LUNAS"
                    konfirmasi = MsgBox("Apakah ingin mencetak nota ?", vbQuestion + vbYesNo, "Tindakan")
                    If konfirmasi = vbYes Then
                        ViewNotaAllOperasi.Ambil_Data = True
                        ViewNotaAllOperasi.Form_Ambil_Data = "OperasiPernota"
                        ViewNotaAllOperasi.Show()
                    End If
            End Select
        End If

    End Sub

    Private Sub dgvOp_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles dgvOp.CellFormatting

        dgvOp.Columns(3).DefaultCellStyle.Format = "###,###,###"
        dgvOp.Columns(1).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        dgvOp.Columns(2).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        dgvOp.Columns(3).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        dgvOp.Columns(4).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        dgvOp.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        dgvOp.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        dgvOp.ColumnHeadersDefaultCellStyle.Font = New Font("Tahoma", 9, FontStyle.Bold)
        dgvOp.ColumnHeadersDefaultCellStyle.BackColor = Color.DodgerBlue
        dgvOp.ColumnHeadersDefaultCellStyle.ForeColor = Color.White
        dgvOp.DefaultCellStyle.Font = New Font("Tahoma", 9, FontStyle.Bold)
        dgvOp.DefaultCellStyle.SelectionBackColor = Color.PaleTurquoise
        dgvOp.DefaultCellStyle.SelectionForeColor = Color.Black
        dgvOp.AllowUserToResizeRows = False
        dgvOp.EnableHeadersVisualStyles = False

        For i As Integer = 0 To dgvOp.Rows.Count - 1
            If dgvOp.Rows(i).Cells(4).Value = "BELUM LUNAS" Then
                dgvOp.Rows(i).Cells(4).Style.BackColor = Color.Orange
                dgvOp.Rows(i).Cells(4).Style.ForeColor = Color.White
            ElseIf dgvOp.Rows(i).Cells(4).Value = "LUNAS" Then
                dgvOp.Rows(i).Cells(4).Style.BackColor = Color.Green
                dgvOp.Rows(i).Cells(4).Style.ForeColor = Color.White
            End If
        Next

        For i As Integer = 0 To dgvOp.RowCount - 1
            dgvOp.Rows(i).Cells(6).Style.BackColor = Color.DodgerBlue
            dgvOp.Rows(i).Cells(6).Style.ForeColor = Color.White
        Next

        For i As Integer = 0 To dgvOp.Rows.Count - 1
            If i Mod 2 = 0 Then
                dgvOp.Rows(i).DefaultCellStyle.BackColor = Color.AliceBlue
            Else
                dgvOp.Rows(i).DefaultCellStyle.BackColor = Color.White
            End If
        Next

        For Each column As DataGridViewColumn In dgvOp.Columns
            column.SortMode = DataGridViewColumnSortMode.NotSortable
        Next
    End Sub

    Private Sub dgvOp_RowPostPaint(sender As Object, e As DataGridViewRowPostPaintEventArgs) Handles dgvOp.RowPostPaint
        Dim dg As DataGridView = DirectCast(sender, DataGridView)
        Dim rowNumber As String = (e.RowIndex + 1).ToString()

        Dim size As SizeF = e.Graphics.MeasureString(rowNumber, Me.Font)
        If dg.RowHeadersWidth < CInt(size.Width + 20) Then
            dg.RowHeadersWidth = CInt(size.Width + 20)
        End If

        Dim b As Brush = SystemBrushes.ControlText

        e.Graphics.DrawString(rowNumber, dg.Font, b, e.RowBounds.Location.X + 15, e.RowBounds.Location.Y + ((e.RowBounds.Height - size.Height) / 2))
    End Sub

    Private Sub dgvDetailOp_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles dgvDetailOp.CellFormatting
        dgvDetailOp.Columns(3).DefaultCellStyle.Format = "###,###,###"
        dgvDetailOp.Columns(6).DefaultCellStyle.Format = "###,###,###"
        dgvDetailOp.Columns(2).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        dgvDetailOp.Columns(3).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        dgvDetailOp.Columns(6).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        dgvDetailOp.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        dgvDetailOp.ColumnHeadersDefaultCellStyle.Font = New Font("Tahoma", 9, FontStyle.Bold)
        dgvDetailOp.ColumnHeadersDefaultCellStyle.BackColor = Color.DodgerBlue
        dgvDetailOp.ColumnHeadersDefaultCellStyle.ForeColor = Color.White
        dgvDetailOp.DefaultCellStyle.Font = New Font("Tahoma", 9, FontStyle.Bold)
        dgvDetailOp.DefaultCellStyle.SelectionBackColor = Color.PaleTurquoise
        dgvDetailOp.DefaultCellStyle.SelectionForeColor = Color.Black
        dgvDetailOp.AllowUserToResizeRows = False
        dgvDetailOp.EnableHeadersVisualStyles = False

        For i As Integer = 0 To dgvDetailOp.Rows.Count - 1
            If i Mod 2 = 0 Then
                dgvDetailOp.Rows(i).DefaultCellStyle.BackColor = Color.AliceBlue
            Else
                dgvDetailOp.Rows(i).DefaultCellStyle.BackColor = Color.White
            End If
        Next
    End Sub

    Private Sub dgvDetailOp_RowPostPaint(sender As Object, e As DataGridViewRowPostPaintEventArgs) Handles dgvDetailOp.RowPostPaint
        Dim dg As DataGridView = DirectCast(sender, DataGridView)
        Dim rowNumber As String = (e.RowIndex + 1).ToString()

        Dim size As SizeF = e.Graphics.MeasureString(rowNumber, Me.Font)
        If dg.RowHeadersWidth < CInt(size.Width + 20) Then
            dg.RowHeadersWidth = CInt(size.Width + 20)
        End If

        Dim b As Brush = SystemBrushes.ControlText

        e.Graphics.DrawString(rowNumber, dg.Font, b, e.RowBounds.Location.X + 15, e.RowBounds.Location.Y + ((e.RowBounds.Height - size.Height) / 2))
    End Sub

    Private Sub btnPrintOPAll_MouseLeave(sender As Object, e As EventArgs) Handles btnPrintOPAll.MouseLeave
        'Me.btnPrintTindakAll.BackColor = Color.Navy
    End Sub

    Private Sub btnPrintOPAll_MouseEnter(sender As Object, e As EventArgs) Handles btnPrintOPAll.MouseEnter
        'Me.btnPrintTindakAll.BackColor = Color.Blue
    End Sub

    Private Sub btnPrintOPAll_Click(sender As Object, e As EventArgs) Handles btnPrintOPAll.Click
        If btnPrintOPAll.Text.Equals("PROSES") Then
            Dim num As Integer = 0
            Dim ran As New List(Of String)
            Dim noRan As String
            For Each row As DataGridViewRow In dgvOp.Rows
                If row.Cells(4).Value.ToString.Equals("BELUM LUNAS") Then
                    ran.Add("'" & row.Cells(5).Value.ToString & "'")
                    num = num + 1
                End If
            Next
            noRan = String.Join(",", ran.ToArray)

            If num > 0 Then
                Call LunasOperasiAll(noRan)
                Call DaftarOperasi()
                btnPrintOPAll.BackColor = Color.Navy
                btnPrintOPAll.Text = "PRINT"
            End If
        ElseIf btnPrintOPAll.Text.Equals("PRINT") Then
            ViewNotaAllOperasi.Ambil_Data = True
            ViewNotaAllOperasi.Form_Ambil_Data = "OperasiAll"
            ViewNotaAllOperasi.Show()
        End If
    End Sub
#End Region
#Region "Obat OK"
    Private Sub dgvOk_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvOk.CellClick
        Dim noTransaksi, total, status As String

        If e.RowIndex = -1 Then
            Return
        End If

        noTransaksi = dgvOk.Rows(e.RowIndex).Cells(1).Value.ToString
        total = dgvOk.Rows(e.RowIndex).Cells(3).Value.ToString
        status = dgvOk.Rows(e.RowIndex).Cells(4).Value.ToString
        txtNoJualOk.Text = noTransaksi
        'txtStatus.Text = status
        'txtPerTotalObat.Text = "Rp " & CInt((Math.Ceiling(total / 100) * 100)).ToString("#,##0")

        Call DetailOK(noTransaksi)
        Call totalTarifDetailOk()
    End Sub

    Private Sub dgvOk_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvOk.CellContentClick
        Dim noTransaksi, total, status As String

        If e.RowIndex = -1 Then
            Return
        End If

        noTransaksi = dgvOk.Rows(e.RowIndex).Cells(1).Value.ToString
        total = dgvOk.Rows(e.RowIndex).Cells(3).Value.ToString
        status = dgvOk.Rows(e.RowIndex).Cells(4).Value.ToString
        txtNoJualOk.Text = noTransaksi
        'txtStatus.Text = status
        'txtPerTotalObat.Text = "Rp " & CInt((Math.Ceiling(total / 100) * 100)).ToString("#,##0")

        Call DetailOK(noTransaksi)
        Call totalTarifDetailOk()

        Dim konfirmasi As MsgBoxResult
        If e.ColumnIndex = 5 Then
            Select Case dgvOk.Rows(e.RowIndex).Cells(4).Value.ToString
                Case "BELUM LUNAS"
                    konfirmasi = MsgBox("Apakah transaksi akan dibayar ?", vbQuestion + vbYesNo, "Obat")
                    If konfirmasi = vbYes Then
                        Call LunasOk(noTransaksi)
                        Call DaftarOK()
                        'MsgBox(tindakan & " - Memulai tindakan", MsgBoxStyle.Information)
                    End If
                Case "LUNAS"
                    konfirmasi = MsgBox("Apakah ingin mencetak nota ?", vbQuestion + vbYesNo, "Obat")
                    If konfirmasi = vbYes Then
                        ViewNotaAllObatOK.Ambil_Data = True
                        ViewNotaAllObatOK.Form_Ambil_Data = "OkPernota"
                        ViewNotaAllObatOK.Show()
                    End If
            End Select
        End If

    End Sub

    Private Sub dgvOk_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles dgvOk.CellFormatting

        dgvOk.Columns(3).DefaultCellStyle.Format = "N2"
        dgvOk.Columns(2).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        dgvOk.Columns(3).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        dgvOk.Columns(4).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        dgvOk.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        dgvOk.ColumnHeadersDefaultCellStyle.Font = New Font("Tahoma", 9, FontStyle.Bold)
        dgvOk.ColumnHeadersDefaultCellStyle.BackColor = Color.DodgerBlue
        dgvOk.ColumnHeadersDefaultCellStyle.ForeColor = Color.White
        dgvOk.DefaultCellStyle.Font = New Font("Tahoma", 9, FontStyle.Bold)
        dgvOk.DefaultCellStyle.SelectionBackColor = Color.PaleTurquoise
        dgvOk.DefaultCellStyle.SelectionForeColor = Color.Black
        dgvOk.AllowUserToResizeRows = False
        dgvOk.EnableHeadersVisualStyles = False

        For i As Integer = 0 To dgvOk.Rows.Count - 1
            If dgvOk.Rows(i).Cells(4).Value = "BELUM LUNAS" Then
                dgvOk.Rows(i).Cells(4).Style.BackColor = Color.Orange
                dgvOk.Rows(i).Cells(4).Style.ForeColor = Color.White
            ElseIf dgvOk.Rows(i).Cells(4).Value = "LUNAS" Then
                dgvOk.Rows(i).Cells(4).Style.BackColor = Color.Green
                dgvOk.Rows(i).Cells(4).Style.ForeColor = Color.White
            End If
        Next

        For i As Integer = 0 To dgvOk.RowCount - 1
            dgvOk.Rows(i).Cells(5).Style.BackColor = Color.DodgerBlue
            dgvOk.Rows(i).Cells(5).Style.ForeColor = Color.White
        Next

        For i As Integer = 0 To dgvOk.Rows.Count - 1
            If i Mod 2 = 0 Then
                dgvOk.Rows(i).DefaultCellStyle.BackColor = Color.AliceBlue
            Else
                dgvOk.Rows(i).DefaultCellStyle.BackColor = Color.White
            End If
        Next

        For Each column As DataGridViewColumn In dgvOk.Columns
            column.SortMode = DataGridViewColumnSortMode.NotSortable
        Next
    End Sub

    Private Sub dgvOk_RowPostPaint(sender As Object, e As DataGridViewRowPostPaintEventArgs) Handles dgvOk.RowPostPaint
        Dim dg As DataGridView = DirectCast(sender, DataGridView)
        Dim rowNumber As String = (e.RowIndex + 1).ToString()

        Dim size As SizeF = e.Graphics.MeasureString(rowNumber, Me.Font)
        If dg.RowHeadersWidth < CInt(size.Width + 20) Then
            dg.RowHeadersWidth = CInt(size.Width + 20)
        End If

        Dim b As Brush = SystemBrushes.ControlText

        e.Graphics.DrawString(rowNumber, dg.Font, b, e.RowBounds.Location.X + 15, e.RowBounds.Location.Y + ((e.RowBounds.Height - size.Height) / 2))
    End Sub

    Private Sub dgvDetailOk_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles dgvDetailOk.CellFormatting
        dgvDetailOk.Columns(3).DefaultCellStyle.Format = "N2"
        dgvDetailOk.Columns(5).DefaultCellStyle.Format = "N2"
        dgvDetailOk.Columns(3).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        dgvDetailOk.Columns(4).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        dgvDetailOk.Columns(5).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        dgvDetailOk.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        dgvDetailOk.ColumnHeadersDefaultCellStyle.Font = New Font("Tahoma", 9, FontStyle.Bold)
        dgvDetailOk.ColumnHeadersDefaultCellStyle.BackColor = Color.DodgerBlue
        dgvDetailOk.ColumnHeadersDefaultCellStyle.ForeColor = Color.White
        dgvDetailOk.DefaultCellStyle.Font = New Font("Tahoma", 9, FontStyle.Bold)
        dgvDetailOk.DefaultCellStyle.SelectionBackColor = Color.PaleTurquoise
        dgvDetailOk.DefaultCellStyle.SelectionForeColor = Color.Black
        dgvDetailOk.AllowUserToResizeRows = False
        dgvDetailOk.EnableHeadersVisualStyles = False

        For i As Integer = 0 To dgvDetailOk.Rows.Count - 1
            If i Mod 2 = 0 Then
                dgvDetailOk.Rows(i).DefaultCellStyle.BackColor = Color.AliceBlue
            Else
                dgvDetailOk.Rows(i).DefaultCellStyle.BackColor = Color.White
            End If
        Next
    End Sub

    Private Sub dgvDetailOk_RowPostPaint(sender As Object, e As DataGridViewRowPostPaintEventArgs) Handles dgvDetailOk.RowPostPaint
        Dim dg As DataGridView = DirectCast(sender, DataGridView)
        Dim rowNumber As String = (e.RowIndex + 1).ToString()

        Dim size As SizeF = e.Graphics.MeasureString(rowNumber, Me.Font)
        If dg.RowHeadersWidth < CInt(size.Width + 20) Then
            dg.RowHeadersWidth = CInt(size.Width + 20)
        End If

        Dim b As Brush = SystemBrushes.ControlText

        e.Graphics.DrawString(rowNumber, dg.Font, b, e.RowBounds.Location.X + 15, e.RowBounds.Location.Y + ((e.RowBounds.Height - size.Height) / 2))
    End Sub

    Private Sub btnPrintOkAll_MouseLeave(sender As Object, e As EventArgs) Handles btnPrintOKAll.MouseLeave
        'Me.btnPrintTindakAll.BackColor = Color.Navy
    End Sub

    Private Sub btnPrintOkAll_MouseEnter(sender As Object, e As EventArgs) Handles btnPrintOKAll.MouseEnter
        'Me.btnPrintTindakAll.BackColor = Color.Blue
    End Sub

    Private Sub btnPrintOkAll_Click(sender As Object, e As EventArgs) Handles btnPrintOKAll.Click
        If txtCaraBayar.Text.Equals("Umum", StringComparison.OrdinalIgnoreCase) Then
            If btnPrintOKAll.Text.Equals("PROSES") Then
                Dim num As Integer = 0
                Dim obt As New List(Of String)
                Dim noObt As String
                For Each row As DataGridViewRow In dgvOk.Rows
                    If row.Cells(4).Value.ToString.Equals("BELUM LUNAS") Then
                        obt.Add("'" & row.Cells(1).Value.ToString & "'")
                        num = num + 1
                    End If
                Next
                noObt = String.Join(",", obt.ToArray)

                If num > 0 Then
                    Call LunasOkAll(noObt)
                    Call DaftarOK()
                    btnPrintOKAll.BackColor = Color.Navy
                    btnPrintOKAll.Text = "PRINT"
                End If
            ElseIf btnPrintOkAll.Text.Equals("PRINT") Then
                ViewNotaAllObatOK.Ambil_Data = True
                ViewNotaAllObatOK.Form_Ambil_Data = "OkAll"
                ViewNotaAllObatOK.Show()
            End If
        Else
            ViewNotaAllObatOK.Ambil_Data = True
            ViewNotaAllObatOK.Form_Ambil_Data = "OkAll"
            ViewNotaAllObatOK.Show()
        End If
    End Sub
#End Region
#Region "Hemodialisa"
    Private Sub dgvHemo_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvHemo.CellClick
        Dim noTindakan, total, status As String

        If e.RowIndex = -1 Then
            Return
        End If

        noTindakan = dgvHemo.Rows(e.RowIndex).Cells(5).Value.ToString
        total = dgvHemo.Rows(e.RowIndex).Cells(3).Value.ToString
        status = dgvHemo.Rows(e.RowIndex).Cells(4).Value.ToString
        txtNoTinHemo.Text = noTindakan
        'txtStatus.Text = status
        'txtPerTotalTindakan.Text = "Rp " & CInt(total).ToString("#,##0")

        Call DetailTinHemo(noTindakan)
        Call totalTarifDetailHemo()
    End Sub

    Private Sub dgvHemo_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvHemo.CellContentClick
        Dim noTindakan, total, status As String

        If e.RowIndex = -1 Then
            Return
        End If

        noTindakan = dgvHemo.Rows(e.RowIndex).Cells(5).Value.ToString
        total = dgvHemo.Rows(e.RowIndex).Cells(3).Value.ToString
        status = dgvHemo.Rows(e.RowIndex).Cells(4).Value.ToString
        txtNoTinHemo.Text = noTindakan
        'txtStatus.Text = status
        'txtPerTotalTindakan.Text = "Rp " & CInt(total).ToString("#,##0")

        Call DetailTinHemo(noTindakan)
        Call totalTarifDetailHemo()

        Dim konfirmasi As MsgBoxResult
        If e.ColumnIndex = 6 Then
            Select Case dgvTindakan.Rows(e.RowIndex).Cells(4).Value.ToString
                Case "BELUM LUNAS"
                    konfirmasi = MsgBox("Apakah transaksi akan dibayar ?", vbQuestion + vbYesNo, "Tindakan")
                    If konfirmasi = vbYes Then
                        Call LunasHemo(noTindakan)
                        Call DaftarTinHemo()
                        'MsgBox(tindakan & " - Memulai tindakan", MsgBoxStyle.Information)
                    End If
                Case "LUNAS"
                    konfirmasi = MsgBox("Apakah ingin mencetak nota ?", vbQuestion + vbYesNo, "Tindakan")
                    If konfirmasi = vbYes Then
                        ViewNotaAllHemo.Ambil_Data = True
                        ViewNotaAllHemo.Form_Ambil_Data = "HemoPernota"
                        ViewNotaAllHemo.Show()
                    End If
            End Select
        End If

    End Sub

    Private Sub dgvHemo_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles dgvHemo.CellFormatting

        dgvHemo.Columns(3).DefaultCellStyle.Format = "###,###,###"
        dgvHemo.Columns(1).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        dgvHemo.Columns(2).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        dgvHemo.Columns(3).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        dgvHemo.Columns(4).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        dgvHemo.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        dgvHemo.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        dgvHemo.ColumnHeadersDefaultCellStyle.Font = New Font("Tahoma", 9, FontStyle.Bold)
        dgvHemo.ColumnHeadersDefaultCellStyle.BackColor = Color.DodgerBlue
        dgvHemo.ColumnHeadersDefaultCellStyle.ForeColor = Color.White
        dgvHemo.DefaultCellStyle.Font = New Font("Tahoma", 9, FontStyle.Bold)
        dgvHemo.DefaultCellStyle.SelectionBackColor = Color.PaleTurquoise
        dgvHemo.DefaultCellStyle.SelectionForeColor = Color.Black
        dgvHemo.AllowUserToResizeRows = False
        dgvHemo.EnableHeadersVisualStyles = False

        For i As Integer = 0 To dgvHemo.Rows.Count - 1
            If dgvHemo.Rows(i).Cells(4).Value = "BELUM LUNAS" Then
                dgvHemo.Rows(i).Cells(4).Style.BackColor = Color.Orange
                dgvHemo.Rows(i).Cells(4).Style.ForeColor = Color.White
            ElseIf dgvHemo.Rows(i).Cells(4).Value = "LUNAS" Then
                dgvHemo.Rows(i).Cells(4).Style.BackColor = Color.Green
                dgvHemo.Rows(i).Cells(4).Style.ForeColor = Color.White
            End If
        Next

        For i As Integer = 0 To dgvHemo.RowCount - 1
            dgvHemo.Rows(i).Cells(6).Style.BackColor = Color.DodgerBlue
            dgvHemo.Rows(i).Cells(6).Style.ForeColor = Color.White
        Next

        For i As Integer = 0 To dgvHemo.Rows.Count - 1
            If i Mod 2 = 0 Then
                dgvHemo.Rows(i).DefaultCellStyle.BackColor = Color.AliceBlue
            Else
                dgvHemo.Rows(i).DefaultCellStyle.BackColor = Color.White
            End If
        Next

        For Each column As DataGridViewColumn In dgvHemo.Columns
            column.SortMode = DataGridViewColumnSortMode.NotSortable
        Next
    End Sub

    Private Sub dgvHemo_RowPostPaint(sender As Object, e As DataGridViewRowPostPaintEventArgs) Handles dgvHemo.RowPostPaint
        Dim dg As DataGridView = DirectCast(sender, DataGridView)
        Dim rowNumber As String = (e.RowIndex + 1).ToString()

        Dim size As SizeF = e.Graphics.MeasureString(rowNumber, Me.Font)
        If dg.RowHeadersWidth < CInt(size.Width + 20) Then
            dg.RowHeadersWidth = CInt(size.Width + 20)
        End If

        Dim b As Brush = SystemBrushes.ControlText

        e.Graphics.DrawString(rowNumber, dg.Font, b, e.RowBounds.Location.X + 15, e.RowBounds.Location.Y + ((e.RowBounds.Height - size.Height) / 2))
    End Sub

    Private Sub dgvDetailHemo_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles dgvDetailHemo.CellFormatting
        dgvDetailHemo.Columns(3).DefaultCellStyle.Format = "###,###,###"
        dgvDetailHemo.Columns(6).DefaultCellStyle.Format = "###,###,###"
        dgvDetailHemo.Columns(2).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        dgvDetailHemo.Columns(3).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        dgvDetailHemo.Columns(6).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        dgvDetailHemo.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        dgvDetailHemo.ColumnHeadersDefaultCellStyle.Font = New Font("Tahoma", 9, FontStyle.Bold)
        dgvDetailHemo.ColumnHeadersDefaultCellStyle.BackColor = Color.DodgerBlue
        dgvDetailHemo.ColumnHeadersDefaultCellStyle.ForeColor = Color.White
        dgvDetailHemo.DefaultCellStyle.Font = New Font("Tahoma", 9, FontStyle.Bold)
        dgvDetailHemo.DefaultCellStyle.SelectionBackColor = Color.PaleTurquoise
        dgvDetailHemo.DefaultCellStyle.SelectionForeColor = Color.Black
        dgvDetailHemo.AllowUserToResizeRows = False
        dgvDetailHemo.EnableHeadersVisualStyles = False

        For i As Integer = 0 To dgvDetailHemo.Rows.Count - 1
            If i Mod 2 = 0 Then
                dgvDetailHemo.Rows(i).DefaultCellStyle.BackColor = Color.AliceBlue
            Else
                dgvDetailHemo.Rows(i).DefaultCellStyle.BackColor = Color.White
            End If
        Next
    End Sub

    Private Sub dgvDetailHemo_RowPostPaint(sender As Object, e As DataGridViewRowPostPaintEventArgs) Handles dgvDetailHemo.RowPostPaint
        Dim dg As DataGridView = DirectCast(sender, DataGridView)
        Dim rowNumber As String = (e.RowIndex + 1).ToString()

        Dim size As SizeF = e.Graphics.MeasureString(rowNumber, Me.Font)
        If dg.RowHeadersWidth < CInt(size.Width + 20) Then
            dg.RowHeadersWidth = CInt(size.Width + 20)
        End If

        Dim b As Brush = SystemBrushes.ControlText

        e.Graphics.DrawString(rowNumber, dg.Font, b, e.RowBounds.Location.X + 15, e.RowBounds.Location.Y + ((e.RowBounds.Height - size.Height) / 2))
    End Sub

    Private Sub btnPrintHemoAll_MouseLeave(sender As Object, e As EventArgs) Handles btnPrintHemoAll.MouseLeave
        Me.btnPrintTindakAll.BackColor = Color.Navy
    End Sub

    Private Sub btnPrintHemoAll_MouseEnter(sender As Object, e As EventArgs) Handles btnPrintHemoAll.MouseEnter
        Me.btnPrintTindakAll.BackColor = Color.Blue
    End Sub

    Private Sub btnPrintHemoAll_Click(sender As Object, e As EventArgs) Handles btnPrintHemoAll.Click
        If btnPrintHemoAll.Text.Equals("PROSES") Then
            Dim num As Integer = 0
            Dim ran As New List(Of String)
            Dim noRan As String
            For Each row As DataGridViewRow In dgvHemo.Rows
                If row.Cells(4).Value.ToString.Equals("BELUM LUNAS") Then
                    ran.Add("'" & row.Cells(5).Value.ToString & "'")
                    num = num + 1
                End If
            Next
            noRan = String.Join(",", ran.ToArray)

            If num > 0 Then
                Call LunasHemoAll(noRan)
                Call DaftarTinHemo()
                btnPrintHemoAll.BackColor = Color.Navy
                btnPrintHemoAll.Text = "PRINT"
            End If
        ElseIf btnPrintHemoAll.Text.Equals("PRINT") Then
            ViewNotaAllHemo.Ambil_Data = True
            ViewNotaAllHemo.Form_Ambil_Data = "HemoAll"
            ViewNotaAllHemo.Show()
        End If
    End Sub
#End Region

End Class