Module FuncTerbilang
    Private _Kamus As SortedDictionary(Of Integer, String)
    Private _ArGroup() As String = {"", " Ribu", " Juta", " Milyar", " Triliun"}

    Private Sub InitializeKamus()
        _Kamus = New SortedDictionary(Of Integer, String)
        _Kamus.Clear()
        _Kamus.Add(0, "")
        _Kamus.Add(1, " Satu")
        _Kamus.Add(2, " Dua")
        _Kamus.Add(3, " Tiga")
        _Kamus.Add(4, " Empat")
        _Kamus.Add(5, " Lima")
        _Kamus.Add(6, " Enam")
        _Kamus.Add(7, " Tujuh")
        _Kamus.Add(8, " Delapan")
        _Kamus.Add(9, " Sembilan")
        _Kamus.Add(10, " Sepuluh")
        _Kamus.Add(11, " Sebelas")
        _Kamus.Add(100, " Seratus")
    End Sub

    Public Function Terbilang(Bilangan As Double) As String
        Dim sRet As String = ""
        Dim sMinus As String = ""
        Dim BilCacah As Double = 0
        Dim BilPecahan As Integer = 0

        InitializeKamus()

        Try
            If Bilangan < 0 Then sMinus = "Minus "

            Dim grp() As String = Split(Math.Abs(Bilangan).ToString(System.Globalization.NumberFormatInfo.CurrentInfo), System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator)

            If grp.Length > 1 Then
                BilCacah = CDbl(grp(0))
                BilPecahan = CInt(grp(1))
            Else
                BilCacah = Bilangan
            End If

            Dim triple() As String = Split(BilCacah.ToString("#,##0", System.Globalization.NumberFormatInfo.CurrentInfo), System.Globalization.NumberFormatInfo.CurrentInfo.NumberGroupSeparator)
            Array.Reverse(triple)

            For i As Integer = triple.Length - 1 To 0 Step -1
                sRet = sRet & BacaGroupAngka(triple(i), False, IIf(i > 5, i - 5 + 1, i))
            Next

            If BilPecahan > 0 Then
                sRet = sRet & " Koma" & BacaGroupAngka(BilPecahan, True)
            End If

            sRet = sMinus & sRet

        Catch ex As Exception
            MsgBox(ex.Message, vbOKOnly, "Parsing Bilangan")
        End Try

        _Kamus.Clear()
        _Kamus = Nothing

        Return sRet.Trim
    End Function

    Private Function BacaGroupAngka(ByVal Angka As Integer,
                                    Optional IsPecahan As Boolean = False,
                                    Optional iGroup As Byte = 0) As String

        Dim sRet As String = ""
        Dim sAngka As String = Angka.ToString("000")

        Select Case IsPecahan
            Case True
                Try
                    For i As Integer = 0 To sAngka.Length - 1
                        If CInt(sAngka.Substring(i, 1)) = 0 Then
                            sRet = sRet & "Nol"
                        Else
                            sRet = sRet & _Kamus(CInt(sAngka.Substring(i, 1)))
                        End If
                    Next
                Catch ex As Exception
                    MsgBox(ex.Message, vbOKOnly, "Baca Pecahan")
                End Try
            Case Else
                Try
                    If Angka = 1 And iGroup = 1 Then
                        sRet = " Seribu"
                    ElseIf _Kamus.ContainsKey(Angka) Then
                        sRet = _Kamus(Angka) & _ArGroup(iGroup)
                    Else
                        Dim Satuan As String = _Kamus(CInt(sAngka.Substring(2, 1)))
                        Dim puluhan As String = ""
                        Dim ratusan As String = ""

                        If _Kamus.ContainsKey(CInt(sAngka.Substring(1, 2))) Then
                            puluhan = _Kamus(CInt(sAngka.Substring(1, 2)))
                        Else
                            If CInt(sAngka.Substring(1, 1)) = 0 Then
                                puluhan = Satuan
                            ElseIf CInt(sAngka.Substring(1, 1)) = 1 Then
                                puluhan = Satuan & " Belas"
                            Else
                                puluhan = _Kamus(CInt(sAngka.Substring(1, 1))) & " Puluh" & Satuan
                            End If
                        End If

                        If CInt(sAngka.Substring(0, 1)) = 0 Then
                            ratusan = puluhan
                        ElseIf CInt(sAngka.Substring(0, 1)) = 1 Then
                            ratusan = " Seratus" & puluhan
                        Else
                            ratusan = _Kamus(CInt(sAngka.Substring(0, 1))) & " Ratus" & puluhan
                        End If

                        sRet = ratusan & _ArGroup(iGroup)
                    End If
                Catch ex As Exception
                    MsgBox(ex.Message, vbOKOnly, "Baca Bilangan Bulat")
                End Try
        End Select

        Return sRet
    End Function
End Module
