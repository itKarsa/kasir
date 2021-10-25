Imports MySql.Data.MySqlClient
Imports Newtonsoft.Json
Public Class VABankJatim

    'Public noTagihan, noVa, totTagih As String

    Dim tgl As Date
    Dim noRegUnit, noTindakan As String
    Dim noReg, noTin, tglExp As String

    Sub tampilData()
        Call koneksiServer()
        Dim query As String = ""
        Dim query2 As String = ""
        Dim cmd As MySqlCommand
        Dim cmd2 As MySqlCommand
        Dim dr As MySqlDataReader
        Dim dr2 As MySqlDataReader

        Select Case Home.unit
            Case "IG"
                query = "CALL allnotindakankasir('" & Home.txtNoReg.Text & "')"
            Case "RJ"
                query = "CALL allnotindakankasir('" & Home.txtNoReg.Text & "')"
            Case "RI"
                query = "CALL totalbiayaranap('" & Home.txtNoReg.Text & "')"
        End Select

        query2 = "SELECT px.noRekamedis,
                      rrj.noRegistrasiRawatJalan,
                      rrj.tglMasukRawatJalan AS tglTindakan,
                      CONCAT('Pendaftaran dan Konsul ',u.unit) AS unit,
                      (rrj.karciPendaftaran + rrj.konsulDokter) AS totalTarifTindakan,
                      COALESCE(rrj.statusPembayaran,'BELUM LUNAS') AS statusPembayaran,
                      'RG' AS noTindakanPasienRajal
                    FROM t_pasien AS px
                      INNER JOIN t_registrasi AS reg ON px.noRekamedis = reg.noRekamedis
                      INNER JOIN t_registrasirawatjalan AS rrj ON reg.noDaftar = rrj.noDaftar
                      INNER JOIN t_unit AS u ON rrj.kdUnit = u.kdUnit
                      WHERE reg.noDaftar = '" & Home.txtNoReg.Text & "'
                      GROUP BY noTindakanPasienRajal"

        Try
            DataGridView1.Rows.Clear()

            cmd2 = New MySqlCommand(query2, conn)
            dr2 = cmd2.ExecuteReader
            Do While dr2.Read
                DataGridView1.Rows.Add(dr2.Item("noRegistrasiRawatJalan"), dr2.Item("tglTindakan"), dr2.Item("unit"),
                                     dr2.Item("totalTarifTindakan"), dr2.Item("statusPembayaran"), dr2.Item("noTindakanPasienRajal"))
            Loop
            dr2.Close()

            cmd = New MySqlCommand(query, conn)
            dr = cmd.ExecuteReader
            Do While dr.Read
                DataGridView1.Rows.Add(dr.Item("noRegistrasiRawatJalan"), dr.Item("tglTindakan"), dr.Item("unit"),
                                     dr.Item("totalTarifTindakan"), dr.Item("statusPembayaran"), dr.Item("noTindakanPasienRajal"))
            Loop
            dr.Close()
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
        conn.Close()
    End Sub

    Sub tampilDataDetail()
        Dim query As String = ""

        Select Case noRegUnit
            Case "IG"
                query = "CALL alldetailrajalkasir('" & noTindakan & "')"
            Case "RJ"
                query = "CALL alldetailrajalkasir('" & noTindakan & "')"
            Case "RI"
                query = "CALL alldetailranapkasir('" & noTindakan & "')"
        End Select

        If noTin = "RG" Then
            query = "SELECT 'Pendaftaran' AS tindakan,
	                            1 AS jumlahTindakan,
	                            rrj.karcipendaftaran AS tarif, 
	                            rrj.karcipendaftaran AS totalTarif
                           FROM t_registrasirawatjalan AS rrj 
                          WHERE rrj.noRegistrasiRawatJalan = '" & Home.txtRegUnit.Text & "' 
                          UNION ALL
                         SELECT 'Konsul' AS tindakan,
                                1 AS jumlahTindakan,	
	                            rrj.konsulDokter AS tarif,
	                            rrj.konsulDokter AS totalTarif
                           FROM t_registrasirawatjalan AS rrj 
                          WHERE rrj.noRegistrasiRawatJalan = '" & Home.txtRegUnit.Text & "'"
        End If

        Try
            Call koneksiServer()
            cmd = New MySqlCommand(query, conn)
            dr = cmd.ExecuteReader
            DataGridView2.Rows.Clear()

            Do While dr.Read
                DataGridView2.Rows.Add(dr.Item("tindakan"), dr.Item("jumlahTindakan"), dr.Item("tarif"), dr.Item("totalTarif"))
            Loop
            dr.Close()
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
        conn.Close()
    End Sub

    Sub totalTagihan()
        Dim totBayar As Long
        totBayar = 0
        For i As Integer = 0 To DataGridView1.Rows.Count - 1
            totBayar = totBayar + Val(DataGridView1.Rows(i).Cells(3).Value)
        Next
        txtTotal.Text = (Math.Ceiling(totBayar / 100) * 100).ToString("#,##0")
    End Sub

    Sub totalDetailTagihan()
        Dim totBayar As Long
        totBayar = 0
        For i As Integer = 0 To DataGridView2.Rows.Count - 1
            totBayar = totBayar + Val(DataGridView2.Rows(i).Cells(3).Value)
        Next
        txtDetailTotal.Text = (Math.Ceiling(totBayar / 100) * 100).ToString("#,##0")
        btnBayar.Text = "Bayar Rp. " & (Math.Ceiling(totBayar / 100) * 100).ToString("#,##0")
    End Sub

    Sub autoNoTagihan()
        Select Case Home.unit
            Case "IG"
                noTagihan = "IG" + Format(Now, "ddMMyyHHmmss")
            Case "RJ"
                noTagihan = "RJ" + Format(Now, "ddMMyyHHmmss")
            Case "RI"
                noTagihan = "RI" + Format(Now, "ddMMyyHHmmss")
        End Select
        'MsgBox(noTagihan)
    End Sub

    Sub addRegistrasi()
        Call koneksiJatim()
        Try
            Dim str As String
            Dim cmd As MySqlCommand
            str = "INSERT INTO registrasi(notagihan,nodaftar,norm,virtualakun,nama,
                                          totaltagihan,tglexp,berita1,berita2,berita3,
                                          berita4,berita5,flagproses) 
                                  VALUES ('" & noTagihan & "','" & Home.txtNoReg.Text & "','" & txtNoRM.Text & "',
                                          '" & noVa & "','" & txtPx.Text & "','" & CInt(txtDetailTotal.Text) & "',
                                          '" & tglExp & "','" & txtUnitTrans.Text & "','" & txtnoTrans.Text & "',
                                          'RSUKH','-','-',1)"
            cmd = New MySqlCommand(str, conn)
            cmd.ExecuteNonQuery()
            MsgBox("Insert data registrasi berhasil dilakukan", MsgBoxStyle.Information)
        Catch ex As Exception
            MsgBox("Insert data registrasi gagal dilakukan.", MsgBoxStyle.Critical)
        End Try
        conn.Close()
    End Sub

    Private Sub VABankJatim_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        TableLayoutPanel3.ColumnStyles(2).SizeType = SizeType.Percent
        TableLayoutPanel3.ColumnStyles(2).Width = 0

        txtPx.Text = Home.txtNamaPasien.Text
        txtNoRM.Text = Home.txtRekMed.Text
        txtUnit.Text = Home.txtUnit.Text
        txtDokter.Text = "-"
        txtTglDaftar.Text = Home.txtTglMasuk.Text
        tgl = Home.txtTglMasuk.Text

        Call autoNoTagihan()
        Call tampilData()
        Call totalTagihan()
    End Sub

    Private Sub DataGridView1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick
        If e.RowIndex = -1 Then
            Return
        End If

        TableLayoutPanel3.ColumnStyles(2).SizeType = SizeType.Percent
        TableLayoutPanel3.ColumnStyles(2).Width = 30

        txtnoTrans.Text = DataGridView1.Rows(e.RowIndex).Cells(5).Value.ToString
        txtUnitTrans.Text = DataGridView1.Rows(e.RowIndex).Cells(2).Value.ToString
        txtTglTrans.Text = DataGridView1.Rows(e.RowIndex).Cells(1).Value.ToString
        noReg = DataGridView1.Rows(e.RowIndex).Cells(0).Value.ToString
        noTindakan = DataGridView1.Rows(e.RowIndex).Cells(5).Value.ToString
        noTin = DataGridView1.Rows(e.RowIndex).Cells(5).Value.Substring(0, 2)
        noRegUnit = DataGridView1.Rows(e.RowIndex).Cells(0).Value.Substring(0, 2)

        Select Case Home.unit
            Case "IG"
                Call tampilDataDetail()
                Call totalDetailTagihan()
            Case "RJ"
                Call tampilDataDetail()
                Call totalDetailTagihan()
            Case "RI"
                DataGridView2.Rows.Clear()
                DataGridView2.Rows.Add(1)
                DataGridView2.Rows(0).Cells(0).Value = txtUnitTrans.Text
                DataGridView2.Rows(0).Cells(1).Value = 1
                DataGridView2.Rows(0).Cells(2).Value = CInt(DataGridView1.Rows(e.RowIndex).Cells(3).Value.ToString)
                DataGridView2.Rows(0).Cells(3).Value = CInt(DataGridView1.Rows(e.RowIndex).Cells(3).Value.ToString)
                DataGridView2.Update()
                Call totalDetailTagihan()
        End Select

    End Sub

    Private Sub DataGridView1_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellClick
        If e.RowIndex = -1 Then
            Return
        End If

        TableLayoutPanel3.ColumnStyles(2).SizeType = SizeType.Percent
        TableLayoutPanel3.ColumnStyles(2).Width = 30

        txtnoTrans.Text = DataGridView1.Rows(e.RowIndex).Cells(5).Value.ToString
        txtUnitTrans.Text = DataGridView1.Rows(e.RowIndex).Cells(2).Value.ToString
        txtTglTrans.Text = DataGridView1.Rows(e.RowIndex).Cells(1).Value.ToString
        noReg = DataGridView1.Rows(e.RowIndex).Cells(0).Value.ToString
        noTindakan = DataGridView1.Rows(e.RowIndex).Cells(5).Value.ToString
        noTin = DataGridView1.Rows(e.RowIndex).Cells(5).Value.Substring(0, 2)
        noRegUnit = DataGridView1.Rows(e.RowIndex).Cells(0).Value.Substring(0, 2)

        Select Case Home.unit
            Case "IG"
                Call tampilDataDetail()
                Call totalDetailTagihan()
            Case "RJ"
                Call tampilDataDetail()
                Call totalDetailTagihan()
            Case "RI"
                DataGridView2.Rows.Clear()
                DataGridView2.Rows.Add(1)
                DataGridView2.Rows(0).Cells(0).Value = txtUnitTrans.Text
                DataGridView2.Rows(0).Cells(1).Value = 1
                DataGridView2.Rows(0).Cells(2).Value = CInt(DataGridView1.Rows(e.RowIndex).Cells(3).Value.ToString)
                DataGridView2.Rows(0).Cells(3).Value = CInt(DataGridView1.Rows(e.RowIndex).Cells(3).Value.ToString)
                DataGridView2.Update()
                Call totalDetailTagihan()
        End Select
    End Sub

    Private Sub DataGridView1_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles DataGridView1.CellFormatting
        DataGridView1.Columns(1).DefaultCellStyle.Format = "G"
        DataGridView1.Columns(4).DefaultCellStyle.Font = New Font("Tahoma", 9, FontStyle.Bold)
        DataGridView1.ColumnHeadersDefaultCellStyle.Font = New Font("Tahoma", 9, FontStyle.Bold)
        DataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.LightCoral
        DataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.White
        DataGridView1.DefaultCellStyle.Font = New Font("Tahoma", 9, FontStyle.Regular)
        DataGridView1.DefaultCellStyle.SelectionBackColor = Color.Coral
        DataGridView1.DefaultCellStyle.SelectionForeColor = Color.Black

        For i As Integer = 0 To DataGridView1.Rows.Count - 1
            If i Mod 2 = 0 Then
                DataGridView1.Rows(i).DefaultCellStyle.BackColor = Color.MistyRose
            Else
                DataGridView1.Rows(i).DefaultCellStyle.BackColor = Color.White
            End If
        Next

        For i As Integer = 0 To DataGridView1.Rows.Count - 1
            If DataGridView1.Rows(i).Cells(4).Value = "BELUM LUNAS" Then
                DataGridView1.Rows(i).Cells(4).Style.BackColor = Color.Orange
                DataGridView1.Rows(i).Cells(4).Style.ForeColor = Color.White
            ElseIf DataGridView1.Rows(i).Cells(4).Value = "LUNAS" Then
                DataGridView1.Rows(i).Cells(4).Style.BackColor = Color.Green
                DataGridView1.Rows(i).Cells(4).Style.ForeColor = Color.White
            End If
        Next
    End Sub
    Private Sub DataGridView1_RowPostPaint(sender As Object, e As DataGridViewRowPostPaintEventArgs) Handles DataGridView1.RowPostPaint
        Dim dg As DataGridView = DirectCast(sender, DataGridView)
        Dim rowNumber As String = (e.RowIndex + 1).ToString()

        Dim size As SizeF = e.Graphics.MeasureString(rowNumber, Me.Font)
        If dg.RowHeadersWidth < CInt(size.Width + 20) Then
            dg.RowHeadersWidth = CInt(size.Width + 20)
        End If

        Dim b As Brush = SystemBrushes.ControlText

        e.Graphics.DrawString(rowNumber, dg.Font, b, e.RowBounds.Location.X + 15, e.RowBounds.Location.Y + ((e.RowBounds.Height - size.Height) / 2))
    End Sub

    Private Sub DataGridView2_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles DataGridView2.CellFormatting
        DataGridView2.ColumnHeadersDefaultCellStyle.Font = New Font("Tahoma", 9, FontStyle.Bold)
        DataGridView2.ColumnHeadersDefaultCellStyle.BackColor = Color.LightCoral
        DataGridView2.ColumnHeadersDefaultCellStyle.ForeColor = Color.White
        DataGridView2.DefaultCellStyle.Font = New Font("Tahoma", 9, FontStyle.Regular)
        DataGridView2.DefaultCellStyle.SelectionBackColor = Color.Coral
        DataGridView2.DefaultCellStyle.SelectionForeColor = Color.Black

        For i As Integer = 0 To DataGridView2.Rows.Count - 1
            If i Mod 2 = 0 Then
                DataGridView2.Rows(i).DefaultCellStyle.BackColor = Color.MistyRose
            Else
                DataGridView2.Rows(i).DefaultCellStyle.BackColor = Color.White
            End If
        Next
    End Sub
    Private Sub DataGridView2_RowPostPaint(sender As Object, e As DataGridViewRowPostPaintEventArgs) Handles DataGridView2.RowPostPaint
        Dim dg As DataGridView = DirectCast(sender, DataGridView)
        Dim rowNumber As String = (e.RowIndex + 1).ToString()

        Dim size As SizeF = e.Graphics.MeasureString(rowNumber, Me.Font)
        If dg.RowHeadersWidth < CInt(size.Width + 20) Then
            dg.RowHeadersWidth = CInt(size.Width + 20)
        End If

        Dim b As Brush = SystemBrushes.ControlText

        e.Graphics.DrawString(rowNumber, dg.Font, b, e.RowBounds.Location.X + 15, e.RowBounds.Location.Y + ((e.RowBounds.Height - size.Height) / 2))
    End Sub

    Private Sub PicClose1_Click(sender As Object, e As EventArgs) Handles PicClose1.Click
        If TableLayoutPanel3.ColumnStyles(2).Width = 30 Then
            TableLayoutPanel3.ColumnStyles(2).SizeType = SizeType.Percent
            TableLayoutPanel3.ColumnStyles(2).Width = 0
        Else
            TableLayoutPanel3.ColumnStyles(2).SizeType = SizeType.Percent
            TableLayoutPanel3.ColumnStyles(2).Width = 30
        End If
    End Sub

    Private Sub btnBayar_MouseEnter(sender As Object, e As EventArgs) Handles btnBayar.MouseEnter
        btnBayar.BackgroundImage = My.Resources.btn_redv21
        btnBayar.ForeColor = Color.DarkRed
    End Sub

    Private Sub btnLogin_MouseLeave(sender As Object, e As EventArgs) Handles btnBayar.MouseLeave
        btnBayar.BackgroundImage = My.Resources.btn_red
        btnBayar.ForeColor = Color.White
    End Sub



    Private Sub btnBayar_Click(sender As Object, e As EventArgs) Handles btnBayar.Click
        Dim pm As New preferenceModel.Post
        Dim x As String = ""
        Dim jsonQuery As String = ""
        Dim req As String = ""
        Dim response As String = ""
        Dim tglReg As String
        tglReg = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")

        Select Case True
            Case noTin.Contains("RG")
                x = "10056056"
            Case noReg.Contains("PIGD")
                x = "10056056"
            Case noReg.Contains("P1RJ")
                x = "10056056"
            Case noReg.Contains("P1RI")
                x = "10056056"
            Case noReg.Contains("RJLAB")
                x = "10056056"
            Case noReg.Contains("RILAB")
                x = "10056056"
            Case noReg.Contains("RJRAD")
                x = "10056056"
            Case noReg.Contains("RIRAD")
                x = "10056056"
            Case noReg.Contains("RJHD")
                x = "10056056"
            Case noReg.Contains("RIHD")
                x = "10056056"
            Case noReg.Contains("RJ")
                x = "10056056"
            Case noReg.Contains("RI")
                x = "10056056"
            Case noReg.Contains("IGD")
                x = "10056056"
            Case Home.unit.Contains("RI")
                x = "10056056"
        End Select

        namaVA = txtPx.Text
        noVA = x & Home.txtRekMed.Text
        totTagih = txtDetailTotal.Text
        'MsgBox(noVA & " - " & noTagihan & " - " & txtDetailTotal.Text)
        tglExp = Format(DateAdd(DateInterval.Day, 1, DateTime.Now), "yyyyMMdd")
        expDateVA = Format(DateAdd(DateInterval.Day, 1, DateTime.Now), "dd/MM/yyyy")

        'Try
        '    pm.VirtualAccount = noVA
        '    pm.Nama = txtPx.Text
        '    pm.TotalTagihan = CInt(totTagih)
        '    pm.TanggalExp = tglExp
        '    pm.Berita1 = "RSUKH"
        '    pm.Berita2 = txtUnitTrans.Text
        '    pm.Berita3 = txtnoTrans.Text
        '    pm.Berita4 = "TEST UAT"
        '    pm.Berita5 = "-"
        '    pm.FlagProses = "1"

        '    jsonQuery = JsonConvert.SerializeObject(pm)
        '    req = reqPost(jsonQuery)
        '    response = req
        '    MsgBox(response)
        '    Dim result As preferenceModel.ResRegistrasi = JsonConvert.DeserializeObject(Of preferenceModel.ResRegistrasi)(response)
        '    MsgBox(result.Status.ErrorDesc, MsgBoxStyle.Information)
        'Catch ex As Exception
        '    MessageBox.Show(ex.ToString & vbNewLine & response, "Error JSON QUERY", MessageBoxButtons.OK, MessageBoxIcon.Error)
        'End Try

        'Call addRegistrasi()
        Dim frmNota As New ViewNotaVA
        frmNota.ShowDialog()
    End Sub

    Property ParamTotTagih As String
    Property ParamNoVA As String
    Property ParamNoTagih As String
End Class