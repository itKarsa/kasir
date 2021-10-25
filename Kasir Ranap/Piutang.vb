Imports MySql.Data.MySqlClient
Public Class Piutang

    Sub dataPiutang()
        Call koneksiServer()
        Dim query As String
        Dim cmd As MySqlCommand
        Dim dr As MySqlDataReader
        query = "CALL piutang('" & Format(DateTimePicker1.Value, "yyyy-MM-dd") & "','" & Format(DateTimePicker1.Value, "yyyy-MM-dd") & "','" & ComboBox2.SelectedValue & "')"
        Try
            cmd = New MySqlCommand(query, conn)
            dr = cmd.ExecuteReader
            dgv.Rows.Clear()
            Do While dr.Read
                dgv.Rows.Add(dr.Item("No.RM"), dr.Item("No.REG"), dr.Item("TGL.MASUK"),
                             dr.Item("TGL.KELUAR"), dr.Item("PASIEN"), dr.Item("RUANG"), dr.Item("KELAS"),
                             dr.Item("JML_INAP"), dr.Item("AKOMODASI"), dr.Item("JASA_VISITE"), dr.Item("NUTRISIONIST"),
                             dr.Item("PERAWATAN"), dr.Item("ASKEP"), dr.Item("LAB_PK"), dr.Item("RAD"),
                             dr.Item("OBAT"), dr.Item("FARKLIN"), dr.Item("OXY"))
            Loop
            dr.Close()
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try
        conn.Close()
    End Sub

    Sub autoCaraBayar()
        Call koneksiServer()
        cmd = New MySqlCommand("SELECT 'ALL' AS cara UNION
                                SELECT carabayar AS cara FROM t_carabayar 
                                 WHERE carabayar IN ('JKN','Taspen','BPJS Jamkesmas','KIS','Jamsostek') 
                              ORDER BY cara ASC", conn)
        da = New MySqlDataAdapter(cmd)
        Dim dt As New DataTable
        da.Fill(dt)

        ComboBox2.DataSource = dt
        ComboBox2.DisplayMember = "cara"
        ComboBox2.ValueMember = "cara"
        ComboBox2.AutoCompleteSource = AutoCompleteSource.ListItems
    End Sub

    Private Sub Piutang_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Call autoCaraBayar()
        'Call autoUnit()
        'Call autoCaraBayar()
        'ComboBox3.SelectedIndex = 0

        'Call JmlPasien()
        'Call totalUang()
    End Sub

    Private Sub btnOK_Click(sender As Object, e As EventArgs) Handles btnOK.Click
        Call dataPiutang()
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

    Private Sub Piutang_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        Home.Show()
    End Sub
End Class