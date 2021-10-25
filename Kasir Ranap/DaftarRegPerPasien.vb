Imports MySql.Data.MySqlClient
Public Class DaftarRegPerPasien

    Public Ambil_Data As String
    Public Form_Ambil_Data As String

    Dim noRm As String = ""

    Dim tglMasuk, noReg, nmPasien, tglKeluar, unit, kelas, carabayar, penjamin, tmpLahir, tglLahir, jk,
            alamat, telp, regUnit, tglCO, karRM, karTglReg, KarReg, KarNama, KarPoli, KarJk, KarAlamat As String

    Sub DaftarReg(noRm)
        Call koneksiServer()
        Dim query As String
        Dim cmd As MySqlCommand
        Dim dr As MySqlDataReader
        query = "CALL listpasienregistrasi('" & noRm & "')"
        Try
            cmd = New MySqlCommand(query, conn)
            dr = cmd.ExecuteReader
            dgvReg.Rows.Clear()
            Do While dr.Read
                dgvReg.Rows.Add(dr.Item("noRekamedis"), dr.Item("tglMasukRawatJalan"), dr.Item("noDaftar"),
                                dr.Item("nmPasien"), dr.Item("tglPulang"), dr.Item("unit"),
                                dr.Item("kelas"), dr.Item("statusKeluar"), dr.Item("carabayar"),
                                dr.Item("penjamin"), dr.Item("tempatLahir"), dr.Item("tglLahir"),
                                dr.Item("jenisKelamin"), dr.Item("alamat"), dr.Item("noTelepone"),
                                dr.Item("noRegistrasiRawatJalan"), dr.Item("tglDaftar"))
            Loop
            dr.Close()
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
        conn.Close()
    End Sub

    Private Sub DaftarRegPerPasien_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If Ambil_Data = True Then
            Select Case Form_Ambil_Data
                Case "Reg"
                    noRm = Home.txtRekMed.Text
                    Call DaftarReg(noRm)
            End Select
        End If
    End Sub

    Private Sub dgvReg_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvReg.CellClick
        If e.RowIndex = -1 Then
            Return
        End If

        tglMasuk = dgvReg.Rows(e.RowIndex).Cells(16).Value.ToString
        noReg = dgvReg.Rows(e.RowIndex).Cells(2).Value.ToString
        nmPasien = dgvReg.Rows(e.RowIndex).Cells(3).Value.ToString
        tglKeluar = dgvReg.Rows(e.RowIndex).Cells(4).Value.ToString
        unit = dgvReg.Rows(e.RowIndex).Cells(5).Value.ToString
        kelas = dgvReg.Rows(e.RowIndex).Cells(6).Value.ToString
        carabayar = dgvReg.Rows(e.RowIndex).Cells(8).Value.ToString
        penjamin = dgvReg.Rows(e.RowIndex).Cells(9).Value.ToString
        tmpLahir = dgvReg.Rows(e.RowIndex).Cells(10).Value.ToString
        tglLahir = dgvReg.Rows(e.RowIndex).Cells(11).Value.ToString
        jk = dgvReg.Rows(e.RowIndex).Cells(12).Value.ToString
        alamat = dgvReg.Rows(e.RowIndex).Cells(13).Value.ToString
        telp = dgvReg.Rows(e.RowIndex).Cells(14).Value.ToString
        regUnit = dgvReg.Rows(e.RowIndex).Cells(15).Value.ToString
        tglCO = dgvReg.Rows(e.RowIndex).Cells(4).Value.ToString

        karRM = dgvReg.Rows(e.RowIndex).Cells(0).Value.ToString
        karTglReg = dgvReg.Rows(e.RowIndex).Cells(1).Value.ToString
        KarReg = dgvReg.Rows(e.RowIndex).Cells(2).Value.ToString
        KarNama = dgvReg.Rows(e.RowIndex).Cells(3).Value.ToString
        KarPoli = dgvReg.Rows(e.RowIndex).Cells(5).Value.ToString
        'Home.txtKarTtl.Text = dgvReg.Rows(e.RowIndex).Cells(10).Value.ToString & ", " & dgvReg.Rows(e.RowIndex).Cells(11).Value.ToString
        KarJk = dgvReg.Rows(e.RowIndex).Cells(12).Value.ToString
        KarAlamat = dgvReg.Rows(e.RowIndex).Cells(13).Value.ToString

    End Sub

    Private Sub dgvReg_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvReg.CellContentClick
        If e.RowIndex = -1 Then
            Return
        End If

        tglMasuk = dgvReg.Rows(e.RowIndex).Cells(16).Value.ToString
        noReg = dgvReg.Rows(e.RowIndex).Cells(2).Value.ToString
        nmPasien = dgvReg.Rows(e.RowIndex).Cells(3).Value.ToString
        tglKeluar = dgvReg.Rows(e.RowIndex).Cells(4).Value.ToString
        unit = dgvReg.Rows(e.RowIndex).Cells(5).Value.ToString
        kelas = dgvReg.Rows(e.RowIndex).Cells(6).Value.ToString
        carabayar = dgvReg.Rows(e.RowIndex).Cells(8).Value.ToString
        penjamin = dgvReg.Rows(e.RowIndex).Cells(9).Value.ToString
        tmpLahir = dgvReg.Rows(e.RowIndex).Cells(10).Value.ToString
        tglLahir = dgvReg.Rows(e.RowIndex).Cells(11).Value.ToString
        jk = dgvReg.Rows(e.RowIndex).Cells(12).Value.ToString
        alamat = dgvReg.Rows(e.RowIndex).Cells(13).Value.ToString
        telp = dgvReg.Rows(e.RowIndex).Cells(14).Value.ToString
        regUnit = dgvReg.Rows(e.RowIndex).Cells(15).Value.ToString
        tglCO = dgvReg.Rows(e.RowIndex).Cells(4).Value.ToString

        karRM = dgvReg.Rows(e.RowIndex).Cells(0).Value.ToString
        karTglReg = dgvReg.Rows(e.RowIndex).Cells(1).Value.ToString
        KarReg = dgvReg.Rows(e.RowIndex).Cells(2).Value.ToString
        KarNama = dgvReg.Rows(e.RowIndex).Cells(3).Value.ToString
        KarPoli = dgvReg.Rows(e.RowIndex).Cells(5).Value.ToString
        'Home.txtKarTtl.Text = dgvReg.Rows(e.RowIndex).Cells(10).Value.ToString & ", " & dgvReg.Rows(e.RowIndex).Cells(11).Value.ToString
        KarJk = dgvReg.Rows(e.RowIndex).Cells(12).Value.ToString
        KarAlamat = dgvReg.Rows(e.RowIndex).Cells(13).Value.ToString
    End Sub

    Private Sub dgvReg_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvReg.CellDoubleClick
        If e.RowIndex = -1 Then
            Return
        End If

        Home.txtTglMasuk.Text = dgvReg.Rows(e.RowIndex).Cells(16).Value.ToString
        Home.txtNoReg.Text = dgvReg.Rows(e.RowIndex).Cells(2).Value.ToString
        Home.txtNamaPasien.Text = dgvReg.Rows(e.RowIndex).Cells(3).Value.ToString
        Home.txtTglKeluar.Text = dgvReg.Rows(e.RowIndex).Cells(4).Value.ToString
        Home.txtUnit.Text = dgvReg.Rows(e.RowIndex).Cells(5).Value.ToString
        Home.txtKelas.Text = dgvReg.Rows(e.RowIndex).Cells(6).Value.ToString
        Home.txtCaraBayar.Text = dgvReg.Rows(e.RowIndex).Cells(8).Value.ToString
        Home.txtPenjamin.Text = dgvReg.Rows(e.RowIndex).Cells(9).Value.ToString
        Home.txtTmpLahir.Text = dgvReg.Rows(e.RowIndex).Cells(10).Value.ToString
        Home.txtTglLahir.Text = dgvReg.Rows(e.RowIndex).Cells(11).Value.ToString
        Home.txtJk.Text = dgvReg.Rows(e.RowIndex).Cells(12).Value.ToString
        Home.txtAlamat.Text = dgvReg.Rows(e.RowIndex).Cells(13).Value.ToString
        Home.txtTelp.Text = dgvReg.Rows(e.RowIndex).Cells(14).Value.ToString
        Home.txtRegUnit.Text = dgvReg.Rows(e.RowIndex).Cells(15).Value.ToString
        Home.txtTglCO.Text = dgvReg.Rows(e.RowIndex).Cells(4).Value.ToString

        Home.txtKarRM.Text = dgvReg.Rows(e.RowIndex).Cells(0).Value.ToString
        Home.txtKarTglReg.Text = dgvReg.Rows(e.RowIndex).Cells(1).Value.ToString
        Home.txtKarReg.Text = dgvReg.Rows(e.RowIndex).Cells(2).Value.ToString
        Home.txtKarNama.Text = dgvReg.Rows(e.RowIndex).Cells(3).Value.ToString
        Home.txtKarPoli.Text = dgvReg.Rows(e.RowIndex).Cells(5).Value.ToString
        'Home.txtKarTtl.Text = dgvReg.Rows(e.RowIndex).Cells(10).Value.ToString & ", " & dgvReg.Rows(e.RowIndex).Cells(11).Value.ToString
        Home.txtKarJk.Text = dgvReg.Rows(e.RowIndex).Cells(12).Value.ToString
        Home.txtKarAlamat.Text = dgvReg.Rows(e.RowIndex).Cells(13).Value.ToString

        Me.Close()
        dgvReg.Rows.Clear()
    End Sub

    Private Sub dgvReg_CellContentDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvReg.CellContentDoubleClick
        If e.RowIndex = -1 Then
            Return
        End If

        Home.txtTglMasuk.Text = dgvReg.Rows(e.RowIndex).Cells(16).Value.ToString
        Home.txtNoReg.Text = dgvReg.Rows(e.RowIndex).Cells(2).Value.ToString
        Home.txtNamaPasien.Text = dgvReg.Rows(e.RowIndex).Cells(3).Value.ToString
        Home.txtTglKeluar.Text = dgvReg.Rows(e.RowIndex).Cells(4).Value.ToString
        Home.txtUnit.Text = dgvReg.Rows(e.RowIndex).Cells(5).Value.ToString
        Home.txtKelas.Text = dgvReg.Rows(e.RowIndex).Cells(6).Value.ToString
        Home.txtCaraBayar.Text = dgvReg.Rows(e.RowIndex).Cells(8).Value.ToString
        Home.txtPenjamin.Text = dgvReg.Rows(e.RowIndex).Cells(9).Value.ToString
        Home.txtTmpLahir.Text = dgvReg.Rows(e.RowIndex).Cells(10).Value.ToString
        Home.txtTglLahir.Text = dgvReg.Rows(e.RowIndex).Cells(11).Value.ToString
        Home.txtJk.Text = dgvReg.Rows(e.RowIndex).Cells(12).Value.ToString
        Home.txtAlamat.Text = dgvReg.Rows(e.RowIndex).Cells(13).Value.ToString
        Home.txtTelp.Text = dgvReg.Rows(e.RowIndex).Cells(14).Value.ToString
        Home.txtRegUnit.Text = dgvReg.Rows(e.RowIndex).Cells(15).Value.ToString
        Home.txtTglCO.Text = dgvReg.Rows(e.RowIndex).Cells(4).Value.ToString

        Home.txtKarRM.Text = dgvReg.Rows(e.RowIndex).Cells(0).Value.ToString
        Home.txtKarTglReg.Text = dgvReg.Rows(e.RowIndex).Cells(1).Value.ToString
        Home.txtKarReg.Text = dgvReg.Rows(e.RowIndex).Cells(2).Value.ToString
        Home.txtKarNama.Text = dgvReg.Rows(e.RowIndex).Cells(3).Value.ToString
        Home.txtKarPoli.Text = dgvReg.Rows(e.RowIndex).Cells(5).Value.ToString
        'Home.txtKarTtl.Text = dgvReg.Rows(e.RowIndex).Cells(10).Value.ToString & ", " & dgvReg.Rows(e.RowIndex).Cells(11).Value.ToString
        Home.txtKarJk.Text = dgvReg.Rows(e.RowIndex).Cells(12).Value.ToString
        Home.txtKarAlamat.Text = dgvReg.Rows(e.RowIndex).Cells(13).Value.ToString

        Me.Close()
        dgvReg.Rows.Clear()
    End Sub

    Private Sub dgvReg_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles dgvReg.CellFormatting
        dgvReg.ColumnHeadersDefaultCellStyle.Font = New Font("Tahoma", 9, FontStyle.Bold)
        dgvReg.ColumnHeadersDefaultCellStyle.BackColor = Color.DodgerBlue
        dgvReg.ColumnHeadersDefaultCellStyle.ForeColor = Color.White
        dgvReg.DefaultCellStyle.Font = New Font("Tahoma", 10, FontStyle.Bold)
        dgvReg.DefaultCellStyle.SelectionBackColor = Color.PaleTurquoise
        dgvReg.DefaultCellStyle.SelectionForeColor = Color.Black

        For i As Integer = 0 To dgvReg.Rows.Count - 1
            If i Mod 2 = 0 Then
                dgvReg.Rows(i).DefaultCellStyle.BackColor = Color.AliceBlue
            Else
                dgvReg.Rows(i).DefaultCellStyle.BackColor = Color.White
            End If
        Next
    End Sub
    Private Sub dgvReg_RowPostPaint(sender As Object, e As DataGridViewRowPostPaintEventArgs) Handles dgvReg.RowPostPaint
        Dim dg As DataGridView = DirectCast(sender, DataGridView)
        Dim rowNumber As String = (e.RowIndex + 1).ToString()

        Dim size As SizeF = e.Graphics.MeasureString(rowNumber, Me.Font)
        If dg.RowHeadersWidth < CInt(size.Width + 20) Then
            dg.RowHeadersWidth = CInt(size.Width + 20)
        End If

        Dim b As Brush = SystemBrushes.ControlText

        e.Graphics.DrawString(rowNumber, dg.Font, b, e.RowBounds.Location.X + 15, e.RowBounds.Location.Y + ((e.RowBounds.Height - size.Height) / 2))
    End Sub

    Private Sub btnOK_Click(sender As Object, e As EventArgs) Handles btnOK.Click
        If (dgvReg.SelectedRows.Count = 1 AndAlso dgvReg.SelectedRows(0).Index = dgvReg.NewRowIndex) Then
            MessageBox.Show("Pilih No.Daftar terlebih dahulu !!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Else
            Home.txtTglMasuk.Text = tglMasuk
            Home.txtNoReg.Text = noReg
            Home.txtNamaPasien.Text = nmPasien
            Home.txtTglKeluar.Text = tglKeluar
            Home.txtUnit.Text = unit
            Home.txtKelas.Text = kelas
            Home.txtCaraBayar.Text = carabayar
            Home.txtPenjamin.Text = penjamin
            Home.txtTmpLahir.Text = tmpLahir
            Home.txtTglLahir.Text = tglLahir
            Home.txtJk.Text = jk
            Home.txtAlamat.Text = alamat
            Home.txtTelp.Text = telp
            Home.txtRegUnit.Text = regUnit
            Home.txtTglCO.Text = tglCO

            Home.txtKarRM.Text = karRM
            Home.txtKarTglReg.Text = karTglReg
            Home.txtKarReg.Text = KarReg
            Home.txtKarNama.Text = KarNama
            Home.txtKarPoli.Text = KarPoli
            'Home.txtKarTtl.Text = dgvReg.Rows(e.RowIndex).Cells(10).Value.ToString & ", " & dgvReg.Rows(e.RowIndex).Cells(11).Value.ToString
            Home.txtKarJk.Text = KarJk
            Home.txtKarAlamat.Text = KarAlamat
            Me.Close()
            dgvReg.Rows.Clear()
        End If
    End Sub
End Class