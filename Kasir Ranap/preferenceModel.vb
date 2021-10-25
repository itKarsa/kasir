Public Class preferenceModel
    Public Class Post
        Public VirtualAccount As String
        Public Nama As String
        Public TotalTagihan As String
        Public TanggalExp As String
        Public Berita1 As String
        Public Berita2 As String
        Public Berita3 As String
        Public Berita4 As String
        Public Berita5 As String
        Public FlagProses As String
    End Class

    Public Class ResRegistrasi
        Public VirtualAccount As String
        Public Nama As String
        Public TotalTagihan As String
        Public Status As Status
    End Class

    Public Class Status
        Public IsError As String
        Public ResponseCode As String
        Public ErrorDesc As String
    End Class

End Class
