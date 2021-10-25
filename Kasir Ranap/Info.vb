Public Class Info

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Me.Dispose()
        Home.Hide()
        PasienRegistrasi.Show()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Me.Dispose()
        Home.Hide()
        RekapTindakanRajal.Show()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Me.Dispose()
        Home.Hide()
        RekapObatRajal.Show()
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Me.Dispose()
        Home.Hide()
        Piutang.Show()
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) 
        Me.Dispose()
        Home.Hide()

    End Sub
End Class