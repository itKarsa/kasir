Imports MySql.Data.MySqlClient
Public Class RekapObatRajal

    Dim statusBayar As String
    Dim noTrans As String

    Sub CariPasien()
        Call koneksiFarmasi()
        Dim query As String
        Dim cmd As MySqlCommand
        Dim dr As MySqlDataReader
        query = "SELECT noPenjualanObatRajal,tglPenjualanObatRajal,noRekamedis,noDaftar,
	                    namapasien,alamat,caraPembayaran,unit,statusPembayaran 
                   FROM t_penjualanobatrajal
                  WHERE (namapasien Like '%" & TextBox1.Text & "%' Or noRekamedis Like '%" & TextBox1.Text & "%')"
        Try
            cmd = New MySqlCommand(query, conn)
            dr = cmd.ExecuteReader
            dgv.Rows.Clear()
            Do While dr.Read
                dgv.Rows.Add(dr.Item("noPenjualanObatRajal"), dr.Item("tglPenjualanObatRajal"), dr.Item("noRekamedis"),
                             dr.Item("noDaftar"), dr.Item("namapasien"), dr.Item("alamat"),
                             dr.Item("caraPembayaran"), dr.Item("unit"), dr.Item("statusPembayaran"))
            Loop
            dr.Close()
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
        conn.Close()
    End Sub

    Sub DaftarPasien()
        Call koneksiFarmasi()
        Dim query As String
        Dim cmd As MySqlCommand
        Dim dr As MySqlDataReader
        query = "SELECT noPenjualanObatRajal,tglPenjualanObatRajal,noRekamedis,noDaftar,
	                    namapasien,alamat,caraPembayaran,unit,statusPembayaran 
                   FROM t_penjualanobatrajal
                  WHERE (SUBSTR(tglPenjualanObatRajal,1,10) = '" & Format(DateTimePicker1.Value, "yyyy-MM-dd") & "')
               ORDER BY tglPenjualanObatRajal DESC, caraPembayaran ASC"
        Try
            cmd = New MySqlCommand(query, conn)
            dr = cmd.ExecuteReader
            dgv.Rows.Clear()
            Do While dr.Read
                dgv.Rows.Add(dr.Item("noPenjualanObatRajal"), dr.Item("tglPenjualanObatRajal"), dr.Item("noRekamedis"),
                             dr.Item("noDaftar"), dr.Item("namapasien"), dr.Item("alamat"),
                             dr.Item("caraPembayaran"), dr.Item("unit"), dr.Item("statusPembayaran"))
            Loop
            dr.Close()
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
        conn.Close()
    End Sub

    Sub DaftarFilterAll()
        Call koneksiFarmasi()
        Dim query As String = ""
        Dim cmd As MySqlCommand
        Dim dr As MySqlDataReader

        If ComboBox2.Text <> "ALL" And ComboBox3.Text <> "ALL" Then '11
            query = "SELECT noPenjualanObatRajal,tglPenjualanObatRajal,noRekamedis,noDaftar,
	                        namapasien,alamat,caraPembayaran,unit,statusPembayaran 
                       FROM t_penjualanobatrajal
                      WHERE (SUBSTR(tglPenjualanObatRajal,1,10) = '" & Format(DateTimePicker1.Value, "yyyy-MM-dd") & "')
                        AND caraPembayaran = '" & ComboBox2.SelectedValue & "' 
                        AND (statusPembayaran = " & statusBayar & ")
                   ORDER BY tglPenjualanObatRajal DESC, caraPembayaran ASC"
        ElseIf ComboBox2.Text <> "ALL" And ComboBox3.Text = "ALL" Then ' 10
            query = "SELECT noPenjualanObatRajal,tglPenjualanObatRajal,noRekamedis,noDaftar,
	                        namapasien,alamat,caraPembayaran,unit,statusPembayaran 
                       FROM t_penjualanobatrajal
                      WHERE (SUBSTR(tglPenjualanObatRajal,1,10) = '" & Format(DateTimePicker1.Value, "yyyy-MM-dd") & "')
                        AND caraPembayaran = '" & ComboBox2.SelectedValue & "'
                   ORDER BY tglPenjualanObatRajal DESC, caraPembayaran ASC"
        ElseIf ComboBox2.Text = "ALL" And ComboBox3.Text <> "ALL" Then '01
            query = "SELECT noPenjualanObatRajal,tglPenjualanObatRajal,noRekamedis,noDaftar,
	                        namapasien,alamat,caraPembayaran,unit,statusPembayaran 
                       FROM t_penjualanobatrajal
                      WHERE (SUBSTR(tglPenjualanObatRajal,1,10) = '" & Format(DateTimePicker1.Value, "yyyy-MM-dd") & "')
                        AND (statusPembayaran = " & statusBayar & ")
                   ORDER BY tglPenjualanObatRajal DESC, caraPembayaran ASC"
        ElseIf ComboBox2.Text = "ALL" And ComboBox3.Text = "ALL" Then ' 00
            query = "SELECT noPenjualanObatRajal,tglPenjualanObatRajal,noRekamedis,noDaftar,
	                        namapasien,alamat,caraPembayaran,unit,statusPembayaran 
                       FROM t_penjualanobatrajal
                      WHERE (SUBSTR(tglPenjualanObatRajal,1,10) = '" & Format(DateTimePicker1.Value, "yyyy-MM-dd") & "')
                   ORDER BY tglPenjualanObatRajal DESC, caraPembayaran ASC"
        End If


        Try
            cmd = New MySqlCommand(query, conn)
            dr = cmd.ExecuteReader
            dgv.Rows.Clear()
            Do While dr.Read
                dgv.Rows.Add(dr.Item("noPenjualanObatRajal"), dr.Item("tglPenjualanObatRajal"), dr.Item("noRekamedis"),
                             dr.Item("noDaftar"), dr.Item("namapasien"), dr.Item("alamat"),
                             dr.Item("caraPembayaran"), dr.Item("unit"), dr.Item("statusPembayaran"))
            Loop
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
        conn.Close()
    End Sub

    Sub DetailObatKronis(noTransaksi As String)
        Call koneksiFarmasi()
        Dim query As String = ""
        Dim cmd As MySqlCommand
        Dim dr As MySqlDataReader

        query = "SELECT noPenjualanObatRajal,kdObat,namaObat,harga,
	                    diberikanKronis AS jml, (harga * diberikanKronis) AS total
                   FROM t_detailpenjualanobatrajal  
                  WHERE noPenjualanObatRajal = '" & noTransaksi & "'"

        Try
            cmd = New MySqlCommand(query, conn)
            dr = cmd.ExecuteReader
            DataGridView1.Rows.Clear()

            Do While dr.Read
                DataGridView1.Rows.Add(dr.Item("noPenjualanObatRajal"), dr.Item("kdObat"),
                                        dr.Item("namaObat"), dr.Item("harga"), dr.Item("jml"),
                                        dr.Item("total"))
            Loop

            dr.Close()
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
        conn.Close()
    End Sub

    Sub DetailObatNon(noTransaksi As String)
        Call koneksiFarmasi()
        Dim query As String = ""
        Dim cmd As MySqlCommand
        Dim dr As MySqlDataReader
        query = "SELECT noPenjualanObatRajal,kdObat,namaObat,harga,
	                    diberikanNonKronis AS jml, (harga * diberikanNonKronis) AS total
                   FROM t_detailpenjualanobatrajal  
                  WHERE noPenjualanObatRajal = '" & noTransaksi & "'"

        Try
            cmd = New MySqlCommand(query, conn)
            dr = cmd.ExecuteReader
            DataGridView2.Rows.Clear()

            Do While dr.Read
                DataGridView2.Rows.Add(dr.Item("noPenjualanObatRajal"), dr.Item("kdObat"),
                                    dr.Item("namaObat"), dr.Item("harga"), dr.Item("jml"),
                                    dr.Item("total"))
            Loop

            dr.Close()
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
        conn.Close()
    End Sub

    Sub totalTarifDetailKronis()
        Dim totBayar As Long
        totBayar = 0
        For i As Integer = 0 To DataGridView1.Rows.Count - 1
            totBayar = totBayar + Val(DataGridView1.Rows(i).Cells(5).Value)
        Next
        TextBox2.Text = (Math.Ceiling(totBayar / 100) * 100).ToString("#,##0")
    End Sub

    Sub totalTarifDetailNon()
        Dim totBayar As Long
        totBayar = 0
        For i As Integer = 0 To DataGridView2.Rows.Count - 1
            totBayar = totBayar + Val(DataGridView2.Rows(i).Cells(5).Value)
        Next
        TextBox3.Text = (Math.Ceiling(totBayar / 100) * 100).ToString("#,##0")
    End Sub

    Sub autoCaraBayar()
        Call koneksiServer()
        cmd = New MySqlCommand("SELECT 'ALL' AS cara UNION
                                SELECT carabayar AS cara FROM t_carabayar ORDER BY cara ASC", conn)
        da = New MySqlDataAdapter(cmd)
        Dim dt As New DataTable
        da.Fill(dt)

        ComboBox2.DataSource = dt
        ComboBox2.DisplayMember = "cara"
        ComboBox2.ValueMember = "cara"
        ComboBox2.AutoCompleteSource = AutoCompleteSource.ListItems
    End Sub

    Sub JmlPasien()
        Dim jml As Integer
        jml = dgv.Rows.Count
        txtJmlTrans.Text = jml
    End Sub

    Private Sub RekapObatRajal_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.FormBorderStyle = FormBorderStyle.None
        Me.WindowState = FormWindowState.Normal
        Me.StartPosition = FormStartPosition.Manual
        With Screen.PrimaryScreen.WorkingArea
            Me.SetBounds(.Left, .Top, .Width, .Height)
        End With

        Call autoCaraBayar()
        ComboBox3.SelectedIndex = 0
        Call DaftarPasien()
        Call JmlPasien()
    End Sub

    Private Sub btnCari_Click(sender As Object, e As EventArgs) Handles btnCari.Click
        Call CariPasien()
    End Sub

    Private Sub DateTimePicker1_ValueChanged(sender As Object, e As EventArgs) Handles DateTimePicker1.ValueChanged
        Call DaftarPasien()
    End Sub

    Private Sub TextBox1_KeyDown(sender As Object, e As KeyEventArgs) Handles TextBox1.KeyDown
        If e.KeyCode = Keys.Enter Then
            If TextBox1.Text = "" Then
                dgv.Rows.Clear()
                Call DaftarPasien()
            Else
                Call CariPasien()
            End If
        End If
    End Sub
    Private Sub btnOK_Click(sender As Object, e As EventArgs) Handles btnOK.Click
        If ComboBox2.Text = "ALL" And ComboBox3.Text = "ALL" Then
            Call DaftarPasien()
        Else
            Call DaftarFilterAll()
        End If
        Call JmlPasien()
    End Sub

    Private Sub ComboBox3_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox3.SelectedIndexChanged
        If ComboBox3.SelectedItem.Equals("LUNAS") Then
            statusBayar = "'LUNAS'"
        ElseIf ComboBox3.SelectedItem.Equals("BELUM LUNAS") Then
            statusBayar = "'BELUM LUNAS'"
        End If
    End Sub

    Private Sub picKeluar_Click(sender As Object, e As EventArgs) Handles picKeluar.Click
        Dim konfirmasi As MsgBoxResult

        konfirmasi = MsgBox("Apakah anda yakin ingin keluar..?", vbQuestion + vbYesNo, "Konfirmasi")
        If konfirmasi = vbYes Then
            Me.Close()
        End If
    End Sub

    Private Sub dgv_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgv.CellClick
        Dim noTransaksi As String

        If e.RowIndex = -1 Then
            Return
        End If

        noTransaksi = dgv.Rows(e.RowIndex).Cells(0).Value.ToString
        noTrans = noTransaksi

        Call DetailObatKronis(noTrans)
        Call DetailObatNon(noTrans)
        Call totalTarifDetailKronis()
        Call totalTarifDetailNon()
    End Sub

    Private Sub dgv_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgv.CellContentClick
        Dim noTransaksi As String

        If e.RowIndex = -1 Then
            Return
        End If

        noTransaksi = dgv.Rows(e.RowIndex).Cells(0).Value.ToString
        noTrans = noTransaksi
        Call DetailObatKronis(noTrans)
        Call DetailObatNon(noTrans)
        Call totalTarifDetailKronis()
        Call totalTarifDetailNon()
    End Sub

    Private Sub dgv_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles dgv.CellFormatting

        For i As Integer = 0 To dgv.Rows.Count - 1
            If i Mod 2 = 0 Then
                dgv.Rows(i).DefaultCellStyle.BackColor = Color.AliceBlue
            Else
                dgv.Rows(i).DefaultCellStyle.BackColor = Color.White
            End If
        Next

        For i As Integer = 0 To dgv.Rows.Count - 1
            If dgv.Rows(i).Cells(8).Value = "BELUM LUNAS" Then
                dgv.Rows(i).Cells(8).Style.BackColor = Color.Orange
                dgv.Rows(i).Cells(8).Style.ForeColor = Color.White
            ElseIf dgv.Rows(i).Cells(8).Value = "LUNAS" Then
                dgv.Rows(i).Cells(8).Style.BackColor = Color.Green
                dgv.Rows(i).Cells(8).Style.ForeColor = Color.White
            End If
        Next

    End Sub

    Private Sub dgv_RowPostPaint(sender As Object, e As DataGridViewRowPostPaintEventArgs) Handles dgv.RowPostPaint
        Dim dg As DataGridView = DirectCast(sender, DataGridView)
        Dim rowNumber As String = (e.RowIndex + 1).ToString()

        Dim size As SizeF = e.Graphics.MeasureString(rowNumber, Me.Font)
        If dg.RowHeadersWidth < CInt(size.Width + 20) Then
            dg.RowHeadersWidth = CInt(size.Width + 20)
        End If

        Dim b As Brush = SystemBrushes.ControlText

        e.Graphics.DrawString(rowNumber, dg.Font, b, e.RowBounds.Location.X + 15, e.RowBounds.Location.Y + ((e.RowBounds.Height - size.Height) / 2))
    End Sub
    Private Sub DataGridView1_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles DataGridView1.CellFormatting

        For i As Integer = 0 To DataGridView1.Rows.Count - 1
            If i Mod 2 = 0 Then
                DataGridView1.Rows(i).DefaultCellStyle.BackColor = Color.AliceBlue
            Else
                DataGridView1.Rows(i).DefaultCellStyle.BackColor = Color.White
            End If
        Next
    End Sub

    Private Sub DataGridView2_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles DataGridView2.CellFormatting

        For i As Integer = 0 To DataGridView2.Rows.Count - 1
            If i Mod 2 = 0 Then
                DataGridView2.Rows(i).DefaultCellStyle.BackColor = Color.AliceBlue
            Else
                DataGridView2.Rows(i).DefaultCellStyle.BackColor = Color.White
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

    Private Sub RekapObatRajal_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        Home.Show()
    End Sub
End Class