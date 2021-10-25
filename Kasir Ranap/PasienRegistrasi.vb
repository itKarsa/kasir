Imports MySql.Data.MySqlClient
Public Class PasienRegistrasi

    Dim statusBayar As String
    Dim row2 As String()

    Sub totalUang()
        Dim totReg As Long = 0
        Dim totKonsul As Long = 0
        Dim totSelisih As Long = 0
        Dim totLunas As Long = 0
        For i As Integer = 0 To dgv.Rows.Count - 1
            totReg = totReg + Val(dgv.Rows(i).Cells(7).Value)
        Next
        txtUangReg.Text = totReg.ToString("#,##0")

        For i As Integer = 0 To dgv.Rows.Count - 1
            totKonsul = totKonsul + Val(dgv.Rows(i).Cells(8).Value)
        Next
        txtUangKonsul.Text = totKonsul.ToString("#,##0")

        For i As Integer = 0 To dgv.Rows.Count - 1
            totSelisih = totSelisih + Val(dgv.Rows(i).Cells(9).Value)
        Next
        txtUangSelisih.Text = totSelisih.ToString("#,##0")

        For i As Integer = 0 To dgv.Rows.Count - 1
            totLunas = totLunas + Val(dgv.Rows(i).Cells(10).Value)
        Next
        txtUangLunas.Text = totLunas.ToString("#,##0")
    End Sub

    Sub JmlPasien()
        Dim jml As Integer
        jml = dgv.Rows.Count
        txtJmlPasien.Text = jml
    End Sub

    Sub DaftarRegPerhari()
        Call koneksiServer()
        Dim query As String
        Dim cmd As MySqlCommand
        Dim dr As MySqlDataReader
        query = "SELECT tglMasukRawatJalan,noRegistrasiRawatJalan,noRekamedis,
                        nmPasien,unit,carabayar,namapetugasMedis,karciPendaftaran,
                        konsulDokter,(karciPendaftaran + konsulDokter) AS selisih,
                        IF(statusPembayaran IS NULL OR statusPembayaran != 'LUNAS',0, ( karciPendaftaran + konsulDokter )) AS bayar
                   FROM vw_pasienrawatjalan
                  WHERE (SUBSTR(tglDaftar,1,10) = '" & Format(DateTimePicker1.Value, "yyyy-MM-dd") & "') 
                    AND statusKeluar != 'Batal'
               ORDER BY unit ASC"
        Try
            cmd = New MySqlCommand(query, conn)
            dr = cmd.ExecuteReader
            dgv.Rows.Clear()
            Do While dr.Read
                dgv.Rows.Add(dr.Item("tglMasukRawatJalan"), dr.Item("noRegistrasiRawatJalan"), dr.Item("noRekamedis"),
                             dr.Item("nmPasien"), dr.Item("unit"), dr.Item("carabayar"), dr.Item("namapetugasMedis"),
                             dr.Item("karciPendaftaran"), dr.Item("konsulDokter"), dr.Item("selisih"), dr.Item("bayar"))
            Loop
            dr.Close()
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
        conn.Close()
    End Sub

    Sub DaftarFilterAll()
        Call koneksiServer()
        Dim query As String = ""
        Dim cmd As MySqlCommand
        Dim dr As MySqlDataReader

        If ComboBox1.Text <> "ALL" And ComboBox2.Text <> "ALL" And ComboBox3.Text <> "ALL" Then '111
            query = "SELECT tglMasukRawatJalan,noRegistrasiRawatJalan,noRekamedis,
                            nmPasien,unit,carabayar,namapetugasMedis,karciPendaftaran,
                            konsulDokter,(karciPendaftaran + konsulDokter) AS selisih,
                            IF(statusPembayaran IS NULL OR statusPembayaran != 'LUNAS',0, ( karciPendaftaran + konsulDokter )) AS bayar
                       FROM vw_pasienrawatjalan
                      WHERE (SUBSTR(tglDaftar,1,10) = '" & Format(DateTimePicker1.Value, "yyyy-MM-dd") & "')
                        AND unit = '" & ComboBox1.SelectedValue & "' 
                        AND carabayar = '" & ComboBox2.SelectedValue & "' 
                        AND (statusPembayaran = " & statusBayar & ")
                        AND statusKeluar != 'Batal'
                   ORDER BY unit ASC"
        ElseIf ComboBox1.Text <> "ALL" And ComboBox2.Text <> "ALL" And ComboBox3.Text = "ALL" Then ' 110
            query = "SELECT tglMasukRawatJalan,noRegistrasiRawatJalan,noRekamedis,
                            nmPasien,unit,carabayar,namapetugasMedis,karciPendaftaran,
                            konsulDokter,(karciPendaftaran + konsulDokter) AS selisih,
                            IF(statusPembayaran IS NULL OR statusPembayaran != 'LUNAS',0, ( karciPendaftaran + konsulDokter )) AS bayar
                       FROM vw_pasienrawatjalan
                      WHERE (SUBSTR(tglDaftar,1,10) = '" & Format(DateTimePicker1.Value, "yyyy-MM-dd") & "')
                        AND unit = '" & ComboBox1.SelectedValue & "' 
                        AND carabayar = '" & ComboBox2.SelectedValue & "'
                        AND statusKeluar != 'Batal'
                   ORDER BY unit ASC"
        ElseIf ComboBox1.Text <> "ALL" And ComboBox2.Text = "ALL" And ComboBox3.Text = "ALL" Then ' 100
            query = "SELECT tglMasukRawatJalan,noRegistrasiRawatJalan,noRekamedis,
                            nmPasien,unit,carabayar,namapetugasMedis,karciPendaftaran,
                            konsulDokter,(karciPendaftaran + konsulDokter) AS selisih,
                            IF(statusPembayaran IS NULL OR statusPembayaran != 'LUNAS',0, ( karciPendaftaran + konsulDokter )) AS bayar
                       FROM vw_pasienrawatjalan
                      WHERE (SUBSTR(tglDaftar,1,10) = '" & Format(DateTimePicker1.Value, "yyyy-MM-dd") & "')
                        AND unit = '" & ComboBox1.SelectedValue & "'
                        AND statusKeluar != 'Batal'
                   ORDER BY unit ASC"
        ElseIf ComboBox1.Text <> "ALL" And ComboBox2.Text = "ALL" And ComboBox3.Text <> "ALL" Then ' 101
            query = "SELECT tglMasukRawatJalan,noRegistrasiRawatJalan,noRekamedis,
                            nmPasien,unit,carabayar,namapetugasMedis,karciPendaftaran,
                            konsulDokter,(karciPendaftaran + konsulDokter) AS selisih,
                            IF(statusPembayaran IS NULL OR statusPembayaran != 'LUNAS',0, ( karciPendaftaran + konsulDokter )) AS bayar
                       FROM vw_pasienrawatjalan
                      WHERE (SUBSTR(tglDaftar,1,10) = '" & Format(DateTimePicker1.Value, "yyyy-MM-dd") & "')
                        AND unit = '" & ComboBox1.SelectedValue & "'
                        AND (statusPembayaran = " & statusBayar & ")
                        AND statusKeluar != 'Batal'
                   ORDER BY unit ASC"
        ElseIf ComboBox1.Text = "ALL" And ComboBox2.Text <> "ALL" And ComboBox3.Text = "ALL" Then '010 
            query = "SELECT tglMasukRawatJalan,noRegistrasiRawatJalan,noRekamedis,
                            nmPasien,unit,carabayar,namapetugasMedis,karciPendaftaran,
                            konsulDokter,(karciPendaftaran + konsulDokter) AS selisih,
                            IF(statusPembayaran IS NULL OR statusPembayaran != 'LUNAS',0, ( karciPendaftaran + konsulDokter )) AS bayar
                       FROM vw_pasienrawatjalan
                      WHERE (SUBSTR(tglDaftar,1,10) = '" & Format(DateTimePicker1.Value, "yyyy-MM-dd") & "')
                        AND carabayar = '" & ComboBox2.SelectedValue & "'
                        AND statusKeluar != 'Batal'
                   ORDER BY unit ASC"
        ElseIf ComboBox1.Text = "ALL" And ComboBox2.Text <> "ALL" And ComboBox3.Text <> "ALL" Then '011 
            query = "SELECT tglMasukRawatJalan,noRegistrasiRawatJalan,noRekamedis,
                            nmPasien,unit,carabayar,namapetugasMedis,karciPendaftaran,
                            konsulDokter,(karciPendaftaran + konsulDokter) AS selisih,
                            IF(statusPembayaran IS NULL OR statusPembayaran != 'LUNAS',0, ( karciPendaftaran + konsulDokter )) AS bayar
                       FROM vw_pasienrawatjalan
                      WHERE (SUBSTR(tglDaftar,1,10) = '" & Format(DateTimePicker1.Value, "yyyy-MM-dd") & "')
                        AND carabayar = '" & ComboBox2.SelectedValue & "'
                        AND (statusPembayaran = " & statusBayar & ")
                        AND statusKeluar != 'Batal'
                   ORDER BY unit ASC"
        ElseIf ComboBox1.Text = "ALL" And ComboBox2.Text = "ALL" And ComboBox3.Text <> "ALL" Then '001
            query = "SELECT tglMasukRawatJalan,noRegistrasiRawatJalan,noRekamedis,
                            nmPasien,unit,carabayar,namapetugasMedis,karciPendaftaran,
                            konsulDokter,(karciPendaftaran + konsulDokter) AS selisih,
                            IF(statusPembayaran IS NULL OR statusPembayaran != 'LUNAS',0, ( karciPendaftaran + konsulDokter )) AS bayar
                       FROM vw_pasienrawatjalan
                      WHERE (SUBSTR(tglDaftar,1,10) = '" & Format(DateTimePicker1.Value, "yyyy-MM-dd") & "')
                        AND (statusPembayaran = " & statusBayar & ")
                        AND statusKeluar != 'Batal'
                   ORDER BY unit ASC"
        End If

        Try
            cmd = New MySqlCommand(query, conn)
            dr = cmd.ExecuteReader
            dgv.Rows.Clear()
            Do While dr.Read
                dgv.Rows.Add(dr.Item("tglMasukRawatJalan"), dr.Item("noRegistrasiRawatJalan"), dr.Item("noRekamedis"),
                             dr.Item("nmPasien"), dr.Item("unit"), dr.Item("carabayar"), dr.Item("namapetugasMedis"),
                             dr.Item("karciPendaftaran"), dr.Item("konsulDokter"), dr.Item("selisih"), dr.Item("bayar"))
            Loop
            dr.Close()
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
        conn.Close()
    End Sub

    Sub autoUnit()
        Call koneksiServer()
        cmd = New MySqlCommand("SELECT 'ALL' AS unit UNION
                                SELECT unit AS unit FROM t_unit WHERE kdInstalasi IN ('ki1','ki4') ORDER BY unit ASC", conn)
        da = New MySqlDataAdapter(cmd)
        Dim dt As New DataTable
        da.Fill(dt)

        ComboBox1.DataSource = dt
        ComboBox1.DisplayMember = "unit"
        ComboBox1.ValueMember = "unit"
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

    Private Sub PasienRegistrasi_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Call autoUnit()
        Call autoCaraBayar()
        Call DaftarRegPerhari()
        ComboBox3.SelectedIndex = 0

        Call JmlPasien()
        Call totalUang()
    End Sub

    Private Sub DateTimePicker1_ValueChanged(sender As Object, e As EventArgs) Handles DateTimePicker1.ValueChanged
        'Call DaftarRegPerhari()
    End Sub

    Private Sub btnOK_Click(sender As Object, e As EventArgs) Handles btnOK.Click
        If ComboBox1.Text = "ALL" And ComboBox2.Text = "ALL" And ComboBox3.Text = "ALL" Then
            Call DaftarRegPerhari()
        Else
            Call DaftarFilterAll()
        End If
        Call JmlPasien()
        Call totalUang()
    End Sub

    Private Sub ComboBox3_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox3.SelectedIndexChanged
        If ComboBox3.SelectedItem.Equals("LUNAS") Then
            statusBayar = "'LUNAS'"
        ElseIf ComboBox3.SelectedItem.Equals("BELUM LUNAS") Then
            statusBayar = "'BELUM LUNAS' OR statusPembayaran IS NULL"
        End If
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

    Private Sub dgv_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles dgv.CellFormatting
        For i As Integer = 0 To dgv.Rows.Count - 1
            If i Mod 2 = 0 Then
                dgv.Rows(i).DefaultCellStyle.BackColor = Color.AliceBlue
            Else
                dgv.Rows(i).DefaultCellStyle.BackColor = Color.White
            End If
        Next

        For i As Integer = 0 To dgv.Rows.Count - 1
            If dgv.Rows(i).Cells(10).Value = "0" Then
                dgv.Rows(i).Cells(10).Style.BackColor = Color.Orange
                dgv.Rows(i).Cells(10).Style.ForeColor = Color.White
            ElseIf dgv.Rows(i).Cells(10).Value <> "0" Then
                dgv.Rows(i).Cells(10).Style.BackColor = Color.Green
                dgv.Rows(i).Cells(10).Style.ForeColor = Color.White
            End If
        Next
    End Sub

    Private Sub PasienRegistrasi_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        Home.Show()
    End Sub
End Class