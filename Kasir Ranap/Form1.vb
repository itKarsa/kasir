Imports System.ComponentModel
Imports MySql.Data.MySqlClient
Public Class Form1

    Public Ambil_Data As String
    Public Form_Ambil_Data As String

    Dim noRekamedis, nmPasien, tempatLahir, tglLahir, jenisKelamin, alamat, notlp, noDaftar, tglDaftar,
        carabayar, noDaftarRawatInap, rawatInap, kelas As String

    Sub tampilData()
        Call koneksiServer()
        da = New MySqlDataAdapter("SELECT noRekamedis,nmPasien,tempatLahir,tglLahir,
                                          jenisKelamin,alamat,noTelepone,noDaftar,tglDaftar,
                                          tglPulang,carabayar,penjamin,noDaftarRawatInap,
                                          rawatInap,kelas,statusKeluar,caraKeluar
                                     FROM vw_pasienrawatinap
                                    WHERE tglKeluarRawatInap IS NULL AND statusKeluar NOT IN ('batal')
                                 ORDER BY tglMasukRawatInap ASC", conn)
        ds = New DataSet
        da.Fill(ds, "vw_pasienrawatinap")
        DataGridView1.DataSource = ds.Tables("vw_pasienrawatinap")
        Call aturDGV()
    End Sub

    Sub aturDGV()
        'DataGridView1.Columns(0).Width = 150
        DataGridView1.Columns(0).HeaderText = "NO.RM"
        DataGridView1.Columns(1).HeaderText = "NAMA PASIEN"
        DataGridView1.Columns(2).HeaderText = "TEMPAT LAHIR"
        DataGridView1.Columns(3).HeaderText = "TGL.LAHIR"
        DataGridView1.Columns(4).HeaderText = "JK"
        DataGridView1.Columns(5).HeaderText = "ALAMAT"
        DataGridView1.Columns(6).HeaderText = "NO.TELP"
        DataGridView1.Columns(7).HeaderText = "NO.DAFTAR"
        DataGridView1.Columns(8).HeaderText = "TGL.DAFTAR"
        DataGridView1.Columns(9).HeaderText = "TGL.PULANG"
        DataGridView1.Columns(10).HeaderText = "CARA BAYAR"
        DataGridView1.Columns(11).HeaderText = "PENJAMIN"
        DataGridView1.Columns(12).HeaderText = "NO.REG RANAP"
        DataGridView1.Columns(13).HeaderText = "RAWAT INAP"
        DataGridView1.Columns(14).HeaderText = "KELAS"
        DataGridView1.Columns(15).HeaderText = "STATUS KELUAR"
        DataGridView1.Columns(16).HeaderText = "CARA KELUAR"

        DataGridView1.Columns(15).Visible = False
        DataGridView1.Columns(16).Visible = False

        DataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        DataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.DodgerBlue
        DataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.White
        DataGridView1.ColumnHeadersDefaultCellStyle.Font = New Font("Tahoma", 8.25, FontStyle.Bold)
        DataGridView1.DefaultCellStyle.SelectionBackColor = Color.PaleTurquoise
        DataGridView1.DefaultCellStyle.SelectionForeColor = Color.Black
        DataGridView1.RowHeadersVisible = False
        DataGridView1.AllowUserToResizeRows = False
        DataGridView1.EnableHeadersVisualStyles = False
    End Sub

    Sub caridata()
        conn.Close()
        Dim query As String
        query = "SELECT *  
                   FROM vw_pasienkasir
                  WHERE (nmPasien Like '%" & TextBox1.Text & "%' Or noRekamedis Like '%" & TextBox1.Text & "%')
                    AND statusKeluar NOT IN ('batal')
                  ORDER BY tglDaftar DESC LIMIT 1"
        da = New MySqlDataAdapter(query, conn)

        Dim str As New DataTable
        str.Clear()
        da.Fill(str)
        DataGridView1.DataSource = str
    End Sub

    Sub autoUnit()
        koneksiServer()

        Using cmd As New MySqlCommand("SELECT noRekamedis,nmPasien 
                                         FROM vw_pasienrawatinap", conn)
            Using rd As MySqlDataReader = cmd.ExecuteReader
                While rd.Read
                    With TextBox1
                        .AutoCompleteMode = AutoCompleteMode.Suggest
                        .AutoCompleteCustomSource.Add(rd.Item(0))
                        .AutoCompleteCustomSource.Add(rd.Item(1))
                        .AutoCompleteSource = AutoCompleteSource.CustomSource
                    End With
                End While
                rd.Close()
            End Using
        End Using
        conn.Close()
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'Call autoUnit()
        'Call tampilData()
        TextBox1.Select()

        btnTindakan.Enabled = False
        btnLaboratorium.Enabled = False
        btnRadiologi.Enabled = False
        btnObat.Enabled = False
        btnTotalPembayaran.Enabled = False
    End Sub

    Private Sub btnCari_Click(sender As Object, e As EventArgs) Handles btnCari.Click
        Call caridata()
    End Sub

    Private Sub TextBox1_KeyDown(sender As Object, e As KeyEventArgs) Handles TextBox1.KeyDown
        If e.KeyCode = Keys.Enter Then
            If TextBox1.Text = "" Then
                'Call tampilData()
                DataGridView1.Rows.Clear()
            Else
                Call caridata()
            End If
        End If
    End Sub

    Private Sub btnTindakan_Click(sender As Object, e As EventArgs) Handles btnTindakan.Click
        Tindakan.txtNoRM.Text = noRekamedis
        Tindakan.txtNama.Text = nmPasien
        Tindakan.txtTTL.Text = tempatLahir
        Tindakan.txtTTL2.Text = tglLahir
        Tindakan.txtJK.Text = jenisKelamin
        Tindakan.txtAlamat.Text = alamat
        Tindakan.txtTelp.Text = notlp
        Tindakan.txtNoDaftar.Text = noDaftar
        Tindakan.txtTglDaftar.Text = tglDaftar
        Tindakan.txtCaraBayar.Text = carabayar
        Tindakan.txtRanap.Text = rawatInap
        Tindakan.txtKelas.Text = kelas
        Tindakan.txtNoReg.Text = noDaftarRawatInap
        Tindakan.txtUser.Text = txtUser.Text
        Tindakan.Show()
        Me.Hide()
    End Sub

    Private Sub btnLaboratorium_Click(sender As Object, e As EventArgs) Handles btnLaboratorium.Click
        Laboratorium.txtNoRM.Text = noRekamedis
        Laboratorium.txtNama.Text = nmPasien
        Laboratorium.txtTTL.Text = tempatLahir
        Laboratorium.txtTTL2.Text = tglLahir
        Laboratorium.txtJK.Text = jenisKelamin
        Laboratorium.txtAlamat.Text = alamat
        Laboratorium.txtTelp.Text = notlp
        Laboratorium.txtNoDaftar.Text = noDaftar
        Laboratorium.txtTglDaftar.Text = tglDaftar
        Laboratorium.txtCaraBayar.Text = carabayar
        Laboratorium.txtRanap.Text = rawatInap
        Laboratorium.txtKelas.Text = kelas
        Laboratorium.txtNoReg.Text = noDaftarRawatInap
        Laboratorium.txtUser.Text = txtUser.Text
        Laboratorium.Show()
        Me.Hide()
    End Sub

    Private Sub btnRadiologi_Click(sender As Object, e As EventArgs) Handles btnRadiologi.Click
        Radiologi.txtNoRM.Text = noRekamedis
        Radiologi.txtNama.Text = nmPasien
        Radiologi.txtTTL.Text = tempatLahir
        Radiologi.txtTTL2.Text = tglLahir
        Radiologi.txtJK.Text = jenisKelamin
        Radiologi.txtAlamat.Text = alamat
        Radiologi.txtTelp.Text = notlp
        Radiologi.txtNoDaftar.Text = noDaftar
        Radiologi.txtTglDaftar.Text = tglDaftar
        Radiologi.txtCaraBayar.Text = carabayar
        Radiologi.txtRanap.Text = rawatInap
        Radiologi.txtKelas.Text = kelas
        Radiologi.txtNoReg.Text = noDaftarRawatInap
        Radiologi.txtUser.Text = txtUser.Text
        Radiologi.Show()
        Me.Hide()
    End Sub

    Private Sub btnObat_Click(sender As Object, e As EventArgs) Handles btnObat.Click
        Obat.txtNoRM.Text = noRekamedis
        Obat.txtNama.Text = nmPasien
        Obat.txtTTL.Text = tempatLahir
        Obat.txtTTL2.Text = tglLahir
        Obat.txtJK.Text = jenisKelamin
        Obat.txtAlamat.Text = alamat
        Obat.txtTelp.Text = notlp
        Obat.txtNoDaftar.Text = noDaftar
        Obat.txtTglDaftar.Text = tglDaftar
        Obat.txtCaraBayar.Text = carabayar
        Obat.txtRanap.Text = rawatInap
        Obat.txtKelas.Text = kelas
        Obat.txtNoReg.Text = noDaftarRawatInap
        Obat.txtUser.Text = txtUser.Text
        Obat.Show()
        Me.Hide()
    End Sub

    Private Sub btnTotalPembayaran_Click(sender As Object, e As EventArgs) Handles btnTotalPembayaran.Click
        TotalPembayaran.txtNoRM.Text = noRekamedis
        TotalPembayaran.txtNama.Text = nmPasien
        TotalPembayaran.txtTTL.Text = tempatLahir
        TotalPembayaran.txtTTL2.Text = tglLahir
        TotalPembayaran.txtJK.Text = jenisKelamin
        TotalPembayaran.txtAlamat.Text = alamat
        TotalPembayaran.txtTelp.Text = notlp
        TotalPembayaran.txtNoDaftar.Text = noDaftar
        TotalPembayaran.txtTglDaftar.Text = tglDaftar
        TotalPembayaran.txtCaraBayar.Text = carabayar
        TotalPembayaran.txtRanap.Text = rawatInap
        TotalPembayaran.txtKelas.Text = kelas
        TotalPembayaran.txtNoReg.Text = noDaftarRawatInap
        TotalPembayaran.txtUser.Text = txtUser.Text
        TotalPembayaran.Show()
        Me.Hide()
    End Sub

    Private Sub DataGridView1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick
        If e.RowIndex = -1 Then
            Return
        End If

        'If Ambil_Data = True Then
        '    Select Case Form_Ambil_Data
        '        Case "Tindakan"
        noRekamedis = DataGridView1.Rows(e.RowIndex).Cells(0).Value
        nmPasien = DataGridView1.Rows(e.RowIndex).Cells(1).Value
        tempatLahir = DataGridView1.Rows(e.RowIndex).Cells(2).Value
        tglLahir = DataGridView1.Rows(e.RowIndex).Cells(3).Value
        jenisKelamin = DataGridView1.Rows(e.RowIndex).Cells(4).Value.ToString
        alamat = DataGridView1.Rows(e.RowIndex).Cells(5).Value
        notlp = DataGridView1.Rows(e.RowIndex).Cells(6).Value.ToString
        noDaftar = DataGridView1.Rows(e.RowIndex).Cells(7).Value
        tglDaftar = DataGridView1.Rows(e.RowIndex).Cells(8).Value
        carabayar = DataGridView1.Rows(e.RowIndex).Cells(11).Value
        noDaftarRawatInap = DataGridView1.Rows(e.RowIndex).Cells(12).Value
        rawatInap = DataGridView1.Rows(e.RowIndex).Cells(13).Value
        kelas = DataGridView1.Rows(e.RowIndex).Cells(14).Value
        '    End Select
        'End If

        btnTindakan.Enabled = True
        btnLaboratorium.Enabled = True
        btnRadiologi.Enabled = True
        btnObat.Enabled = True
        btnTotalPembayaran.Enabled = True
    End Sub

    Private Sub DataGridView1_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellClick
        If e.RowIndex = -1 Then
            Return
        End If

        'If Ambil_Data = True Then
        '    Select Case Form_Ambil_Data
        '        Case "Tindakan"
        noRekamedis = DataGridView1.Rows(e.RowIndex).Cells(0).Value
        nmPasien = DataGridView1.Rows(e.RowIndex).Cells(1).Value
        tempatLahir = DataGridView1.Rows(e.RowIndex).Cells(2).Value
        tglLahir = DataGridView1.Rows(e.RowIndex).Cells(3).Value
        jenisKelamin = DataGridView1.Rows(e.RowIndex).Cells(4).Value
        alamat = DataGridView1.Rows(e.RowIndex).Cells(5).Value
        notlp = DataGridView1.Rows(e.RowIndex).Cells(6).Value.ToString
        noDaftar = DataGridView1.Rows(e.RowIndex).Cells(7).Value
        tglDaftar = DataGridView1.Rows(e.RowIndex).Cells(8).Value
        carabayar = DataGridView1.Rows(e.RowIndex).Cells(11).Value
        noDaftarRawatInap = DataGridView1.Rows(e.RowIndex).Cells(12).Value
        rawatInap = DataGridView1.Rows(e.RowIndex).Cells(13).Value
        kelas = DataGridView1.Rows(e.RowIndex).Cells(14).Value
        '    End Select
        'End If

        btnTindakan.Enabled = True
        btnLaboratorium.Enabled = True
        btnRadiologi.Enabled = True
        btnObat.Enabled = True
        btnTotalPembayaran.Enabled = True
    End Sub
    Private Sub btnCari_MouseLeave(sender As Object, e As EventArgs) Handles btnCari.MouseLeave
        Me.btnCari.BackColor = Color.Navy
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        'Call autoUnit()
        'Call tampilData()
        'MsgBox("Refresh")
    End Sub

    Private Sub btnCari_MouseEnter(sender As Object, e As EventArgs) Handles btnCari.MouseEnter
        Me.btnCari.BackColor = Color.Blue
    End Sub

    Private Sub btnTindakan_MouseLeave(sender As Object, e As EventArgs) Handles btnTindakan.MouseLeave
        Me.btnTindakan.BackColor = Color.Navy
    End Sub

    Private Sub btnTindakan_MouseEnter(sender As Object, e As EventArgs) Handles btnTindakan.MouseEnter
        Me.btnTindakan.BackColor = Color.Blue
    End Sub

    Private Sub btnLaboratorium_MouseLeave(sender As Object, e As EventArgs) Handles btnLaboratorium.MouseLeave
        Me.btnLaboratorium.BackColor = Color.Navy
    End Sub

    Private Sub btnLaboratorium_MouseEnter(sender As Object, e As EventArgs) Handles btnLaboratorium.MouseEnter
        Me.btnLaboratorium.BackColor = Color.Blue
    End Sub

    Private Sub btnRadiologi_MouseLeave(sender As Object, e As EventArgs) Handles btnRadiologi.MouseLeave
        Me.btnRadiologi.BackColor = Color.Navy
    End Sub

    Private Sub btnRadiologi_MouseEnter(sender As Object, e As EventArgs) Handles btnRadiologi.MouseEnter
        Me.btnRadiologi.BackColor = Color.Blue
    End Sub

    Private Sub btnObat_MouseLeave(sender As Object, e As EventArgs) Handles btnObat.MouseLeave
        Me.btnObat.BackColor = Color.Navy
    End Sub

    Private Sub btnObat_MouseEnter(sender As Object, e As EventArgs) Handles btnObat.MouseEnter
        Me.btnObat.BackColor = Color.Blue
    End Sub

    Private Sub btnTotalPembayaran_MouseLeave(sender As Object, e As EventArgs) Handles btnTotalPembayaran.MouseLeave
        Me.btnTotalPembayaran.BackColor = Color.Navy
    End Sub

    Private Sub btnTotalPembayaran_MouseEnter(sender As Object, e As EventArgs) Handles btnTotalPembayaran.MouseEnter
        Me.btnTotalPembayaran.BackColor = Color.Blue
    End Sub

    Private Sub Form1_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        LoginForm.Show()
    End Sub
End Class
