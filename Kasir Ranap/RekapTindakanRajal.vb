Imports MySql.Data.MySqlClient
Public Class RekapTindakanRajal

    Sub autoUnit()
        Call koneksiServer()
        cmd = New MySqlCommand("SELECT unit FROM t_unit WHERE kdInstalasi IN ('ki1','ki4') ORDER BY unit ASC", conn)
        da = New MySqlDataAdapter(cmd)
        Dim dt As New DataTable
        da.Fill(dt)

        ComboBox2.DataSource = dt
        ComboBox2.DisplayMember = "unit"
        ComboBox2.ValueMember = "unit"
        'ComboBox2.AutoCompleteMode = AutoCompleteMode.SuggestAppend
        ComboBox2.AutoCompleteSource = AutoCompleteSource.ListItems
    End Sub

    Sub DaftarRekapAll()
        Call koneksiServer()
        Dim query As String
        Dim cmd As MySqlCommand
        Dim dr As MySqlDataReader
        query = "SELECT tpj.tglTindakan,
                        dtpj.tindakan AS nama_tindakan,
                        COUNT(dtpj.jumlahTindakan) AS jumlah_tindakan,
                        prj.unit
                   FROM t_tindakanpasienrajal AS tpj
             INNER JOIN t_detailtindakanpasienrajal AS dtpj ON dtpj.noTindakanPasienRajal = tpj.noTindakanPasienRajal
             INNER JOIN vw_pasienrawatjalan AS prj ON tpj.noRegistrasiRawatJalan = prj.noRegistrasiRawatJalan
                  WHERE (SUBSTR(tglTindakan,1,10) BETWEEN '" & Format(DateTimePicker1.Value, "yyyy-MM-dd") & "' AND '" & Format(DateTimePicker2.Value, "yyyy-MM-dd") & "')
               GROUP BY dtpj.tindakan
               ORDER BY tpj.tglTindakan ASC"
        Try
            cmd = New MySqlCommand(query, conn)
            dr = cmd.ExecuteReader
            dgv.Rows.Clear()
            Do While dr.Read
                dgv.Rows.Add(dr.Item("tglTindakan"), dr.Item("nama_tindakan"), dr.Item("jumlah_tindakan"), dr.Item("unit"))
            Loop
            dr.Close()
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
        conn.Close()
    End Sub

    Sub DaftarRekapUnit()
        Call koneksiServer()
        Dim query As String
        Dim cmd As MySqlCommand
        Dim dr As MySqlDataReader
        query = "SELECT tpj.tglTindakan,
                        dtpj.tindakan AS nama_tindakan,
                        COUNT(dtpj.jumlahTindakan) AS jumlah_tindakan,
                        prj.unit
                   FROM t_tindakanpasienrajal AS tpj
             INNER JOIN t_detailtindakanpasienrajal AS dtpj ON dtpj.noTindakanPasienRajal = tpj.noTindakanPasienRajal
             INNER JOIN vw_pasienrawatjalan AS prj ON tpj.noRegistrasiRawatJalan = prj.noRegistrasiRawatJalan
                  WHERE prj.unit = '" & ComboBox2.SelectedValue & "' 
                    AND (SUBSTR(tglTindakan,1,10) BETWEEN '" & Format(DateTimePicker1.Value, "yyyy-MM-dd") & "' AND '" & Format(DateTimePicker2.Value, "yyyy-MM-dd") & "')
               GROUP BY dtpj.tindakan
               ORDER BY tpj.tglTindakan ASC"
        Try
            cmd = New MySqlCommand(query, conn)
            dr = cmd.ExecuteReader
            dgv.Rows.Clear()
            Do While dr.Read
                dgv.Rows.Add(dr.Item("tglTindakan"), dr.Item("nama_tindakan"), dr.Item("jumlah_tindakan"), dr.Item("unit"))
            Loop
            dr.Close()
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
        conn.Close()
    End Sub

    Private Sub RekapTindakanRajal_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Call autoUnit()
        ComboBox1.SelectedIndex = 0
    End Sub

    Private Sub btnOK_Click(sender As Object, e As EventArgs) Handles btnOK.Click
        If ComboBox1.SelectedItem.Equals("SEMUA UNIT") Then
            ComboBox2.Enabled = False
            dgv.Rows.Clear()
            Call DaftarRekapAll()
        ElseIf ComboBox1.SelectedItem.Equals("PER UNIT") Then
            ComboBox2.Enabled = True
            If ComboBox2.SelectedValue.Equals(Nothing) Then
                MsgBox("Pilih Unit terlebih dahulu")
            Else
                dgv.Rows.Clear()
                Call DaftarRekapUnit()
            End If
        End If
    End Sub

    Private Sub dgv_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles dgv.CellFormatting
        For i As Integer = 0 To dgv.Rows.Count - 1
            If i Mod 2 = 0 Then
                dgv.Rows(i).DefaultCellStyle.BackColor = Color.AliceBlue
            Else
                dgv.Rows(i).DefaultCellStyle.BackColor = Color.White
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

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        If ComboBox1.SelectedItem.Equals("SEMUA UNIT") Then
            ComboBox2.SelectedValue = -1
            ComboBox2.Enabled = False
        ElseIf ComboBox1.SelectedItem.Equals("PER UNIT") Then
            ComboBox2.SelectedValue = 0
            ComboBox2.Enabled = True
        End If
    End Sub

    Private Sub RekapTindakanRajal_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        Home.Show()
    End Sub
End Class