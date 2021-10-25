Imports System.Globalization
Imports MySql.Data.MySqlClient
Public Class TotalPembayaran

    Public Ambil_Data As String
    Public Form_Ambil_Data As String

    Dim totBiayaRuang As Integer
    Dim totTindakan, totLab, totRad As Integer
    Dim totObat As Integer
    Dim totBayar As Integer


    Sub totalTarif()
        totBiayaRuang = 0
        totTindakan = 0
        totLab = 0
        totRad = 0
        totObat = 0
        totBayar = 0
        For i As Integer = 0 To DataGridView1.Rows.Count - 1
            totTindakan = totTindakan + Val(DataGridView1.Rows(i).Cells(2).Value)
        Next

        For i As Integer = 0 To DataGridView2.Rows.Count - 1
            totLab = totLab + Val(DataGridView2.Rows(i).Cells(2).Value)
        Next

        For i As Integer = 0 To DataGridView3.Rows.Count - 1
            totRad = totRad + Val(DataGridView3.Rows(i).Cells(2).Value)
        Next

        'Ruang
        totBiayaRuang = Val(txtTarifRuang2.Text * txtTotInap.Text) + Val(txtTarifDPJP.Text)
        txtTotalBiayaRuang.Text = "Rp " & totBiayaRuang.ToString("#,##0")
        txtTotalBiayaRuang2.Text = totBiayaRuang
        'Tindakan
        txtTotalTindakan.Text = "Rp " & totTindakan.ToString("#,##0")
        txtTotTindak2.Text = totTindakan
        'Penunjang
        txtTotalPenunjang.Text = "Rp " & (totLab + totRad).ToString("#,##0")
        txtTotPenunjang2.Text = totLab + totRad

        totBayar = totBiayaRuang + totTindakan + totLab + totRad + (Math.Ceiling(totObat / 100) * 100)
        txtPembayaran.Text = "Rp " & (Math.Ceiling(totBayar / 100) * 100).ToString("#,##0")
        txtTotal2.Text = (Math.Ceiling(totBayar / 100) * 100)
    End Sub

    Sub hitungInap()
        Call koneksiServer()
        Try
            Dim query As String
            Dim dr As MySqlDataReader
            query = "SELECT tarifKmr,DATEDIFF(NOW(), tglMasukRawatInap) AS jumlah 
                       FROM vw_pasienrawatinap 
                      WHERE noRekamedis = '" & txtNoRM.Text & "' 
                        AND noDaftarRawatInap = '" & txtNoReg.Text & "'"
            cmd = New MySqlCommand(query, conn)
            dr = cmd.ExecuteReader

            While dr.Read
                txtTarifRuang2.Text = dr.GetString("tarifKmr")
                txtTotInap.Text = Val(dr.GetString("jumlah") + 1)
            End While
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Sub tampilDataTindakan()
        Call koneksiServer()
        da = New MySqlDataAdapter("SELECT noDaftarRawatInap,tglTindakan,
                                          totalTarifTindakan,statusPembayaran,
                                          noTindakanPasienRanap
                                     FROM vw_tindakanpasienranapkasir
                                    WHERE noDaftarRawatInap = '" & txtNoReg.Text & "'
                                 ORDER BY CAST(noTindakanPasienRanap AS UNSIGNED) ASC", conn)
        ds = New DataSet
        da.Fill(ds, "vw_detailtindakanpasienranap")
        DataGridView1.DataSource = ds.Tables("vw_detailtindakanpasienranap")
        Call aturDGVTindakan()
    End Sub

    Sub aturDGVTindakan()
        'DataGridView1.Columns(0).Width = 150
        DataGridView1.Columns(0).HeaderText = "NO.DAFTAR RANAP"
        DataGridView1.Columns(1).HeaderText = "TGL.TINDAKAN"
        DataGridView1.Columns(2).HeaderText = "TOTAL TARIF"
        DataGridView1.Columns(3).HeaderText = "STATUS PEMBAYARAN"
        DataGridView1.Columns(4).HeaderText = "No.TINDAKAN"

        DataGridView1.Columns(4).Visible = False

        DataGridView1.Columns(2).DefaultCellStyle.Format = "###,###,###"
        DataGridView1.Columns(1).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        DataGridView1.Columns(2).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        DataGridView1.Columns(3).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        DataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        DataGridView1.ColumnHeadersDefaultCellStyle.Font = New Font("Tahoma", 10, FontStyle.Bold)
        DataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.DodgerBlue
        DataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.White
        DataGridView1.DefaultCellStyle.SelectionBackColor = Color.PaleTurquoise
        DataGridView1.DefaultCellStyle.SelectionForeColor = Color.Black
        DataGridView1.RowHeadersVisible = False
        DataGridView1.AllowUserToResizeRows = False
        DataGridView1.EnableHeadersVisualStyles = False

    End Sub

    Sub tampilDataLab()
        Call koneksiServer()
        da = New MySqlDataAdapter("SELECT noRegistrasiPenunjangRanap,tglMasukPenunjangRanap,
                                          totalTindakanPenunjangRanap,statusPembayaran,
                                          noTindakanPenunjangRanap
                                     FROM vw_datalabpasienranap
                                    WHERE noDaftar = '" & txtNoDaftar.Text & "' AND noDaftarRawatInap = '" & txtNoReg.Text & "'
                                 GROUP BY noRegistrasiPenunjangRanap                                 
                                 ORDER BY CAST(noTindakanPenunjangRanap AS UNSIGNED) ASC", conn)
        ds = New DataSet
        da.Fill(ds, "vw_datalabpasienranap")
        DataGridView2.DataSource = ds.Tables("vw_datalabpasienranap")
        Call aturDGVLab()
    End Sub

    Sub aturDGVLab()
        'DataGridView1.Columns(0).Width = 150
        DataGridView2.Columns(0).HeaderText = "NO.DAFTAR LAB"
        DataGridView2.Columns(1).HeaderText = "TGL.TINDAKAN"
        DataGridView2.Columns(2).HeaderText = "TOTAL TARIF"
        DataGridView2.Columns(3).HeaderText = "STATUS PEMBAYARAN"
        DataGridView2.Columns(4).HeaderText = "No.TINDAKAN LAB"

        DataGridView2.Columns(4).Visible = False

        DataGridView2.Columns(2).DefaultCellStyle.Format = "###,###,###"
        DataGridView2.Columns(1).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        DataGridView2.Columns(2).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        DataGridView2.Columns(3).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        DataGridView2.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        DataGridView2.ColumnHeadersDefaultCellStyle.Font = New Font("Tahoma", 10, FontStyle.Bold)
        DataGridView2.ColumnHeadersDefaultCellStyle.BackColor = Color.DodgerBlue
        DataGridView2.ColumnHeadersDefaultCellStyle.ForeColor = Color.White
        DataGridView2.DefaultCellStyle.SelectionBackColor = Color.PaleTurquoise
        DataGridView2.DefaultCellStyle.SelectionForeColor = Color.Black
        DataGridView2.RowHeadersVisible = False
        DataGridView2.AllowUserToResizeRows = False
        DataGridView2.EnableHeadersVisualStyles = False
    End Sub

    Sub tampilDataRad()
        Call koneksiServer()
        da = New MySqlDataAdapter("Select noRegistrasiRadiologiRanap,tglMasukRadiologiRanap,
                                          totalTindakanRadiologiRanap, statusPembayaran,
                                          noTindakanRadiologiRanap
                                     From vw_dataradpasienranap
                                    Where noDaftar = '" & txtNoDaftar.Text & "' AND noDaftarRawatInap = '" & txtNoReg.Text & "'
                                 GROUP BY noRegistrasiRadiologiRanap                                 
                                 ORDER BY CAST(noTindakanRadiologiRanap AS UNSIGNED) ASC", conn)


        ds = New DataSet
        da.Fill(ds, "vw_dataradpasienranap")
        DataGridView3.DataSource = ds.Tables("vw_dataradpasienranap")
        Call aturDGVRad()
    End Sub

    Sub aturDGVRad()
        'DataGridView1.Columns(0).Width = 150
        DataGridView3.Columns(0).HeaderText = "NO.DAFTAR RADIOLOGI"
        DataGridView3.Columns(1).HeaderText = "TGL.TINDAKAN"
        DataGridView3.Columns(2).HeaderText = "TOTAL TARIF"
        DataGridView3.Columns(3).HeaderText = "STATUS PEMBAYARAN"
        DataGridView3.Columns(4).HeaderText = "No.TINDAKAN LAB"

        DataGridView3.Columns(4).Visible = False

        DataGridView3.Columns(2).DefaultCellStyle.Format = "###,###,###"
        DataGridView3.Columns(1).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        DataGridView3.Columns(2).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        DataGridView3.Columns(3).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        DataGridView3.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        DataGridView3.ColumnHeadersDefaultCellStyle.Font = New Font("Tahoma", 10, FontStyle.Bold)
        DataGridView3.ColumnHeadersDefaultCellStyle.BackColor = Color.DodgerBlue
        DataGridView3.ColumnHeadersDefaultCellStyle.ForeColor = Color.White
        DataGridView3.DefaultCellStyle.SelectionBackColor = Color.PaleTurquoise
        DataGridView3.DefaultCellStyle.SelectionForeColor = Color.Black
        DataGridView3.RowHeadersVisible = False
        DataGridView3.AllowUserToResizeRows = False
        DataGridView3.EnableHeadersVisualStyles = False
    End Sub

    Sub tampilDataObat()
        'Call koneksiServer()
        'da = New MySqlDataAdapter("SELECT * 
        '                             FROM vw_penjualanobatranap
        '                            WHERE noDaftar = '" & txtNoDaftar.Text & "'", conn)

        'ds = New DataSet
        'da.Fill(ds, "vw_penjualanobatranap")
        'DataGridView4.DataSource = ds.Tables("vw_penjualanobatranap")
        'Call aturDGVObat()
    End Sub

    Sub aturDGVObat()
        'DataGridView1.Columns(0).Width = 150
        'DataGridView4.Columns(0).HeaderText = "NO.DAFTAR"
        'DataGridView4.Columns(1).HeaderText = "NO.TRANSAKSI"
        'DataGridView4.Columns(2).HeaderText = "TGL.PEMBELIAN"
        'DataGridView4.Columns(3).HeaderText = "TOTAL"
        'DataGridView4.Columns(4).HeaderText = "STATUS PEMBAYARAN"

        'DataGridView4.Columns(0).Visible = False

        'DataGridView4.Columns(3).DefaultCellStyle.Format = "N2"
        'DataGridView4.Columns(2).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        'DataGridView4.Columns(3).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        'DataGridView4.Columns(4).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        'DataGridView4.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        'DataGridView4.ColumnHeadersDefaultCellStyle.Font = New Font("Tahoma", 10, FontStyle.Bold)
        'DataGridView4.ColumnHeadersDefaultCellStyle.BackColor = Color.DodgerBlue
        'DataGridView4.ColumnHeadersDefaultCellStyle.ForeColor = Color.White
        'DataGridView4.DefaultCellStyle.SelectionBackColor = Color.PaleTurquoise
        'DataGridView4.DefaultCellStyle.SelectionForeColor = Color.Black
        'DataGridView4.RowHeadersVisible = False
        'DataGridView4.AllowUserToResizeRows = False
        'DataGridView4.EnableHeadersVisualStyles = False
    End Sub

    Private Sub TotalPembayaran_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Call hitungInap()
        Call tampilDataTindakan()
        Call tampilDataLab()
        Call tampilDataRad()
        Call tampilDataObat()

        txtTarifRuang.Text = txtTarifRuang2.Text
        Call totalTarif()
    End Sub

    Private Sub DataGridView1_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellClick
        Dim noTindakan, status As String

        If e.RowIndex = -1 Then
            Return
        End If

        noTindakan = DataGridView1.Rows(e.RowIndex).Cells(4).Value.ToString
        status = DataGridView1.Rows(e.RowIndex).Cells(3).Value.ToString
        txtNoTindakan.Text = noTindakan
        'txtStatus.Text = status

        'Call tampilDataDetail()
        'Call totalTarif()
    End Sub

    Private Sub DataGridView2_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView2.CellClick
        Dim noTindakan, status As String

        If e.RowIndex = -1 Then
            Return
        End If

        noTindakan = DataGridView2.Rows(e.RowIndex).Cells(4).Value.ToString
        status = DataGridView2.Rows(e.RowIndex).Cells(3).Value.ToString
        txtNoTindakan.Text = noTindakan
        'txtStatus.Text = status

        'Call tampilDataDetail()
        'Call totalTarif()
    End Sub

    Private Sub DataGridView3_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView3.CellClick
        Dim noTindakan, status As String

        If e.RowIndex = -1 Then
            Return
        End If

        noTindakan = DataGridView3.Rows(e.RowIndex).Cells(4).Value.ToString
        status = DataGridView3.Rows(e.RowIndex).Cells(3).Value.ToString
        txtNoTindakan.Text = noTindakan
        'txtStatus.Text = status

        'Call tampilDataDetail()
        'Call totalTarif()
    End Sub

    Private Sub DataGridView4_CellClick(sender As Object, e As DataGridViewCellEventArgs)
        'Dim noTindakan, status As String

        'If e.RowIndex = -1 Then
        '    Return
        'End If

        'noTindakan = DataGridView4.Rows(e.RowIndex).Cells(1).Value.ToString
        'status = DataGridView4.Rows(e.RowIndex).Cells(4).Value.ToString
        'txtNoTindakan.Text = noTindakan
        'txtStatus.Text = status

        'Call tampilDataDetail()
        'Call totalTarif()
    End Sub

    Private Sub btnPrintAll_Click(sender As Object, e As EventArgs) Handles btnPrintAll.Click
        ViewNotaAllTotal.Ambil_Data = True
        ViewNotaAllTotal.Form_Ambil_Data = "TotalAll"
        ViewNotaAllTotal.Show()
    End Sub

    Private Sub btnPrintPer_Click(sender As Object, e As EventArgs) Handles btnPrintPer.Click
        ViewNotaAllTotal.Ambil_Data = True
        ViewNotaAllTotal.Form_Ambil_Data = "TotalPernota"
        ViewNotaAllTotal.Show()
    End Sub

    Private Sub btnPrint_Click(sender As Object, e As EventArgs) Handles btnPrint.Click
        ViewNotaTotal.Show()
    End Sub

    Private Sub btnProses_Click(sender As Object, e As EventArgs) Handles btnProses.Click
        'Call updatePembayaran()
        'Call tampilData()
    End Sub

    Private Sub btnProses_MouseLeave(sender As Object, e As EventArgs) Handles btnProses.MouseLeave
        Me.btnProses.BackColor = Color.Navy
    End Sub

    Private Sub btnProses_MouseEnter(sender As Object, e As EventArgs) Handles btnProses.MouseEnter
        Me.btnProses.BackColor = Color.Blue
    End Sub

    Private Sub btnPrintAll_MouseLeave(sender As Object, e As EventArgs) Handles btnPrintAll.MouseLeave
        Me.btnPrintAll.BackColor = Color.Navy
    End Sub

    Private Sub btnPrintAll_MouseEnter(sender As Object, e As EventArgs) Handles btnPrintAll.MouseEnter
        Me.btnPrintAll.BackColor = Color.Blue
    End Sub

    Private Sub btnPrintPer_MouseLeave(sender As Object, e As EventArgs) Handles btnPrintPer.MouseLeave
        Me.btnPrintPer.BackColor = Color.Navy
    End Sub

    Private Sub btnPrintPer_MouseEnter(sender As Object, e As EventArgs) Handles btnPrintPer.MouseEnter
        Me.btnPrintPer.BackColor = Color.Blue
    End Sub

    Private Sub txtKelas_TextChanged(sender As Object, e As EventArgs) Handles txtKelas.TextChanged
        If txtKelas.Text.Equals("EXECUTIVE") Then
            txtTarifDPJP.Text = "150000"
        ElseIf txtKelas.Text.Equals("VVIP") Then
            txtTarifDPJP.Text = "140000"
        ElseIf txtKelas.Text.Equals("VIP") Then
            txtTarifDPJP.Text = "120000"
        ElseIf txtKelas.Text.Equals("UTAMA") Then
            txtTarifDPJP.Text = "100000"
        ElseIf txtKelas.Text.Equals("KELAS I") Then
            txtTarifDPJP.Text = "50000"
        ElseIf txtKelas.Text.Equals("KELAS II") Then
            txtTarifDPJP.Text = "30000"
        ElseIf txtKelas.Text.Equals("KELAS III") Then
            txtTarifDPJP.Text = "20000"
        End If
    End Sub

    Private Sub btnPrint_MouseLeave(sender As Object, e As EventArgs) Handles btnPrint.MouseLeave
        Me.btnPrintPer.BackColor = Color.Navy
    End Sub

    Private Sub btnPrint_MouseEnter(sender As Object, e As EventArgs) Handles btnPrint.MouseEnter
        Me.btnPrintPer.BackColor = Color.Blue
    End Sub

    Private Sub TotalPembayaran_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        Form1.Show()
    End Sub
End Class