Imports System.ComponentModel
Imports MySql.Data.MySqlClient
Public Class Radiologi

    Public Ambil_Data As String
    Public Form_Ambil_Data As String

    Sub totalTarif()
        Dim totBayar As Long
        totBayar = 0
        For i As Integer = 0 To DataGridView2.Rows.Count - 1
            totBayar = totBayar + Val(DataGridView2.Rows(i).Cells(4).Value)
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
        da = New MySqlDataAdapter("Select noRegistrasiRadiologiRanap,tglMasukRadiologiRanap,
                                          totalTindakanRadiologiRanap, statusPembayaran,
                                          noTindakanRadiologiRanap
                                     From vw_dataradpasienranap
                                    Where noDaftar = '" & txtNoDaftar.Text & "' AND noDaftarRawatInap = '" & txtNoReg.Text & "'
                                      AND (totalTindakanRadiologiRanap IS NOT NULL AND totalTindakanRadiologiRanap != 0)
                                 GROUP BY noRegistrasiRadiologiRanap                                 
                                 ORDER BY tglMasukRadiologiRanap ASC", conn)


        ds = New DataSet
        da.Fill(ds, "vw_dataradpasienranap")
        DataGridView1.DataSource = ds.Tables("vw_dataradpasienranap")
        Call aturDGV()
    End Sub

    Sub aturDGV()
        'DataGridView1.Columns(0).Width = 150
        DataGridView1.Columns(0).HeaderText = "NO.DAFTAR RADIOLOGI"
        DataGridView1.Columns(1).HeaderText = "TGL.TINDAKAN"
        DataGridView1.Columns(2).HeaderText = "TOTAL TARIF"
        DataGridView1.Columns(3).HeaderText = "STATUS PEMBAYARAN"
        DataGridView1.Columns(4).HeaderText = "No.TINDAKAN LAB"

        DataGridView1.Columns(4).Visible = False

        DataGridView1.Columns(2).DefaultCellStyle.Format = "###,###,###"
        DataGridView1.Columns(1).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        DataGridView1.Columns(2).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        DataGridView1.Columns(3).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        DataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        DataGridView1.ColumnHeadersDefaultCellStyle.Font = New Font("Tahoma", 10, FontStyle.Bold)
        DataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.DodgerBlue
        DataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.White
        DataGridView1.Columns(3).DefaultCellStyle.Font = New Font("Tahoma", 10, FontStyle.Bold)
        DataGridView1.DefaultCellStyle.SelectionBackColor = Color.PaleTurquoise
        DataGridView1.DefaultCellStyle.SelectionForeColor = Color.Black
        DataGridView1.AllowUserToResizeRows = False
        DataGridView1.EnableHeadersVisualStyles = False
    End Sub

    Sub tampilDataDetail()
        Call koneksiServer()
        da = New MySqlDataAdapter("SELECT kdTarif, tindakan, jumlahTindakan, tarif, totalTarif 
                                     FROM vw_dataradpasienranap WHERE noTindakanRadiologiRanap = '" & txtNoTindakan.Text & "'", conn)
        ds = New DataSet
        da.Fill(ds, "vw_dataradpasienranap")
        DataGridView2.DataSource = ds.Tables("vw_dataradpasienranap")
        Call aturDGVDetail()
    End Sub

    Sub aturDGVDetail()
        'DataGridView2.Columns(0).Width = 150
        DataGridView2.Columns(0).HeaderText = "KODE"
        DataGridView2.Columns(1).HeaderText = "TINDAKAN"
        DataGridView2.Columns(2).HeaderText = "QTY"
        DataGridView2.Columns(3).HeaderText = "TARIF"
        DataGridView2.Columns(4).HeaderText = "TOTAL TARIF"

        DataGridView2.Columns(3).DefaultCellStyle.Format = "###,###,###"
        DataGridView2.Columns(4).DefaultCellStyle.Format = "###,###,###"
        DataGridView2.Columns(2).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        DataGridView2.Columns(3).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        DataGridView2.Columns(4).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        DataGridView2.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        DataGridView2.ColumnHeadersDefaultCellStyle.Font = New Font("Tahoma", 10, FontStyle.Bold)
        DataGridView2.ColumnHeadersDefaultCellStyle.BackColor = Color.DodgerBlue
        DataGridView2.ColumnHeadersDefaultCellStyle.ForeColor = Color.White
        DataGridView2.DefaultCellStyle.SelectionBackColor = Color.PaleTurquoise
        DataGridView2.DefaultCellStyle.SelectionForeColor = Color.Black
        DataGridView2.AllowUserToResizeRows = False
        DataGridView2.EnableHeadersVisualStyles = False
    End Sub

    Sub updatePembayaran()
        Dim dt As String
        dt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")

        Dim str As String = ""

        str = "UPDATE t_tindakanradiologiranap 
                  SET statusPembayaran = '" & txtStatus.Text & "',
                      tglPembayaran = '" & dt & "'
                WHERE noTindakanRadiologiRanap = '" & txtNoTindakan.Text & "'"

        Call koneksiServer()
        Try
            cmd = New MySqlCommand(str, conn)
            cmd.ExecuteNonQuery()
            MsgBox("Pembayaran Tindakan Radiologi berhasil dilakukan", MessageBoxIcon.Information)
        Catch ex As Exception
            MsgBox("Pembayaran gagal dilakukan.", MessageBoxIcon.Error)
        End Try

        conn.Close()
    End Sub

    Private Sub Radiologi_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Call tampilData()
        Call totalAll()
    End Sub

    Private Sub DataGridView1_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellClick
        Dim bar, noTindakan, status As String

        If e.RowIndex = -1 Then
            Return
        End If

        bar = Val(DataGridView1.CurrentCell.RowIndex) + 1
        noTindakan = DataGridView1.Rows(e.RowIndex).Cells(4).Value.ToString
        status = DataGridView1.Rows(e.RowIndex).Cells(3).Value.ToString
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
        noTindakan = DataGridView1.Rows(e.RowIndex).Cells(4).Value.ToString
        status = DataGridView1.Rows(e.RowIndex).Cells(3).Value.ToString
        txtNoTindakan.Text = noTindakan
        txtStatus.Text = status
        Label16.Text = "TOTAL NOTA-" & bar

        Call tampilDataDetail()
        Call totalTarif()
    End Sub

    Private Sub btnPrintAll_Click(sender As Object, e As EventArgs) Handles btnPrintAll.Click
        ViewNotaAllRadiologi.Ambil_Data = True
        ViewNotaAllRadiologi.Form_Ambil_Data = "RadAll"
        ViewNotaAllRadiologi.Show()
    End Sub

    Private Sub btnPrintPer_Click(sender As Object, e As EventArgs) Handles btnPrintPer.Click
        ViewNotaAllRadiologi.Ambil_Data = True
        ViewNotaAllRadiologi.Form_Ambil_Data = "RadPernota"
        ViewNotaAllRadiologi.Show()
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

    Private Sub Radiologi_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        DataGridView2.DataSource = Nothing
        txtTotal.Text = Nothing
        txtTotal2.Text = Nothing
        txtTotalAll.Text = Nothing
        txtStatus.Text = Nothing
    End Sub

    Private Sub DataGridView1_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles DataGridView1.CellFormatting
        For i As Integer = 0 To DataGridView1.Rows.Count - 1
            If DataGridView1.Rows(i).Cells(3).Value = "BELUM LUNAS" Then
                DataGridView1.Rows(i).Cells(3).Style.BackColor = Color.Orange
                DataGridView1.Rows(i).Cells(3).Style.ForeColor = Color.White
            ElseIf DataGridView1.Rows(i).Cells(3).Value = "LUNAS" Then
                DataGridView1.Rows(i).Cells(3).Style.BackColor = Color.Green
                DataGridView1.Rows(i).Cells(3).Style.ForeColor = Color.White
            End If
        Next
    End Sub

    Private Sub Radiologi_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        Form1.Show()
    End Sub
End Class