Imports System.ComponentModel
Imports MySql.Data.MySqlClient
Public Class Tindakan

    Public Ambil_Data As String
    Public Form_Ambil_Data As String

    Sub totalTarif()
        Dim totBayar As Long
        totBayar = 0
        For i As Integer = 0 To DataGridView2.Rows.Count - 1
            totBayar = totBayar + Val(DataGridView2.Rows(i).Cells(6).Value)
        Next
        txtTotal.Text = "Rp " & totBayar.ToString("#,##0")
        txtTotal2.Text = totBayar
    End Sub

    Sub totalAll()
        Dim totBayar As Long
        totBayar = 0
        For i As Integer = 0 To DataGridView1.Rows.Count - 1
            totBayar = totBayar + Val(DataGridView1.Rows(i).Cells(2).Value)
        Next
        txtTotalAll.Text = "Rp " & totBayar.ToString("#,##0")
    End Sub

    Sub tampilData()
        Call koneksiServer()
        da = New MySqlDataAdapter("SELECT noDaftarRawatInap,tglTindakan,
                                          rawatInap,totalTarifTindakan,
                                          statusPembayaran,noTindakanPasienRanap
                                     FROM vw_tindakanpasienranapkasir
                                    WHERE noDaftarRawatInap = '" & txtNoReg.Text & "'
                                      AND (totalTarifTindakan IS NOT NULL AND totalTarifTindakan != 0)
                                 ORDER BY tglTindakan ASC", conn)
        ds = New DataSet
        da.Fill(ds, "vw_detailtindakanpasienranap")
        DataGridView1.DataSource = ds.Tables("vw_detailtindakanpasienranap")
        Call aturDGV()
    End Sub

    Sub aturDGV()
        'DataGridView1.Columns(0).Width = 150
        DataGridView1.Columns(0).HeaderText = "NO.DAFTAR RANAP"
        DataGridView1.Columns(1).HeaderText = "TGL.TINDAKAN"
        DataGridView1.Columns(2).HeaderText = "RUANG"
        DataGridView1.Columns(3).HeaderText = "TOTAL TARIF"
        DataGridView1.Columns(4).HeaderText = "STATUS PEMBAYARAN"
        DataGridView1.Columns(5).HeaderText = "No.TINDAKAN"

        DataGridView1.Columns(5).Visible = False

        DataGridView1.Columns(3).DefaultCellStyle.Format = "###,###,###"
        DataGridView1.Columns(1).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        DataGridView1.Columns(2).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        DataGridView1.Columns(3).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        DataGridView1.Columns(4).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        DataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        DataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        DataGridView1.ColumnHeadersDefaultCellStyle.Font = New Font("Tahoma", 10, FontStyle.Bold)
        DataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.DodgerBlue
        DataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.White
        DataGridView1.Columns(4).DefaultCellStyle.Font = New Font("Tahoma", 10, FontStyle.Bold)
        DataGridView1.DefaultCellStyle.SelectionBackColor = Color.PaleTurquoise
        DataGridView1.DefaultCellStyle.SelectionForeColor = Color.Black
        DataGridView1.AllowUserToResizeRows = False
        DataGridView1.EnableHeadersVisualStyles = False
    End Sub

    Sub tampilDataDetail()
        Call koneksiServer()
        da = New MySqlDataAdapter("SELECT kdTarif, tindakan, jumlahTindakan, tarif, DPJP, PPA, totalTarif 
                                     FROM vw_tindakanmedisranap WHERE noTindakanPasienRanap = '" & txtNoTindakan.Text & "'", conn)
        ds = New DataSet
        da.Fill(ds, "vw_tindakanmedisranap")
        DataGridView2.DataSource = ds.Tables("vw_tindakanmedisranap")
        Call aturDGVDetail()
    End Sub

    Sub aturDGVDetail()
        'DataGridView2.Columns(0).Width = 150
        DataGridView2.Columns(0).HeaderText = "KODE"
        DataGridView2.Columns(1).HeaderText = "TINDAKAN"
        DataGridView2.Columns(2).HeaderText = "QTY"
        DataGridView2.Columns(3).HeaderText = "TARIF"
        DataGridView2.Columns(4).HeaderText = "DPJP"
        DataGridView2.Columns(5).HeaderText = "PPA"
        DataGridView2.Columns(6).HeaderText = "TOTAL TARIF"

        DataGridView2.Columns(3).DefaultCellStyle.Format = "###,###,###"
        DataGridView2.Columns(6).DefaultCellStyle.Format = "###,###,###"
        DataGridView2.Columns(2).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        DataGridView2.Columns(3).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        DataGridView2.Columns(6).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        DataGridView2.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        DataGridView2.ColumnHeadersDefaultCellStyle.Font = New Font("Tahoma", 10, FontStyle.Bold)
        DataGridView2.ColumnHeadersDefaultCellStyle.BackColor = Color.DodgerBlue
        DataGridView2.ColumnHeadersDefaultCellStyle.ForeColor = Color.White
        DataGridView2.Columns(6).DefaultCellStyle.Font = New Font("Tahoma", 10, FontStyle.Bold)
        DataGridView2.DefaultCellStyle.SelectionBackColor = Color.PaleTurquoise
        DataGridView2.DefaultCellStyle.SelectionForeColor = Color.Black
        DataGridView2.AllowUserToResizeRows = False
        DataGridView2.EnableHeadersVisualStyles = False

    End Sub

    Sub updatePembayaran()
        Dim dt As String
        dt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")

        Dim str As String = ""

        str = "UPDATE t_tindakanpasienranap 
                  SET statusPembayaran = '" & txtStatus.Text & "',
                      tglPembayaran = '" & dt & "'
                WHERE noTindakanPasienRanap = '" & txtNoTindakan.Text & "'"

        Call koneksiServer()
        Try
            cmd = New MySqlCommand(str, conn)
            cmd.ExecuteNonQuery()
            MsgBox("Pembayaran Tindakan berhasil dilakukan", MessageBoxIcon.Information)
        Catch ex As Exception
            MsgBox("Pembayaran gagal dilakukan.", MessageBoxIcon.Error)
        End Try

        conn.Close()
    End Sub

    Private Sub Tindakan_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Call tampilData()
        Call totalAll()
    End Sub

    Private Sub DataGridView1_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellClick
        Dim bar, noTindakan, status As String

        If e.RowIndex = -1 Then
            Return
        End If

        bar = Val(DataGridView1.CurrentCell.RowIndex) + 1
        noTindakan = DataGridView1.Rows(e.RowIndex).Cells(5).Value.ToString
        status = DataGridView1.Rows(e.RowIndex).Cells(4).Value.ToString
        txtNoTindakan.Text = noTindakan
        txtStatus.Text = status
        Label16.Text = "TOTAL NOTA-" & bar

        Call tampilDataDetail()
        Call totalTarif()
    End Sub

    Private Sub DataGridView1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick
        Dim bar, noTindakan, status As String

        If e.RowIndex = -1 Then
            Return
        End If

        bar = Val(DataGridView1.CurrentCell.RowIndex) + 1
        noTindakan = DataGridView1.Rows(e.RowIndex).Cells(5).Value.ToString
        status = DataGridView1.Rows(e.RowIndex).Cells(4).Value.ToString
        txtNoTindakan.Text = noTindakan
        txtStatus.Text = status
        Label16.Text = "TOTAL NOTA-" & bar

        Call tampilDataDetail()
        Call totalTarif()
    End Sub

    Private Sub btnPrintAll_Click(sender As Object, e As EventArgs) Handles btnPrintAll.Click
        ViewNotaAllTindakan.Ambil_Data = True
        ViewNotaAllTindakan.Form_Ambil_Data = "TindakanAll"
        ViewNotaAllTindakan.Show()
    End Sub

    Private Sub btnPrintPer_Click(sender As Object, e As EventArgs) Handles btnPrintPer.Click
        ViewNotaAllTindakan.Ambil_Data = True
        ViewNotaAllTindakan.Form_Ambil_Data = "TindakanPernota"
        ViewNotaAllTindakan.Show()
    End Sub

    Private Sub btnProses_Click(sender As Object, e As EventArgs) Handles btnProses.Click
        Call updatePembayaran()
        Call tampilData()
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

    Private Sub Tindakan_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        DataGridView2.DataSource = Nothing
        txtTotal.Text = Nothing
        txtTotal2.Text = Nothing
        txtTotalAll.Text = Nothing
        txtStatus.Text = Nothing
    End Sub

    Private Sub DataGridView1_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles DataGridView1.CellFormatting
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

    Private Sub Tindakan_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        Form1.Show()
    End Sub
End Class