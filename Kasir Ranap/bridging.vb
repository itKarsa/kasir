Imports System.IO
Imports System.Net
Imports System.Text
Module bridging
    Function reqPost(jsonQuery As String)
        Dim myReq As HttpWebRequest
        Dim myResp As HttpWebResponse
        'Dim myResponse As String
        'myReq = HttpWebRequest.Create("https://apps.bankjatim.co.id/Api/Registrasi")
        myReq = HttpWebRequest.Create("https://jatimva.bankjatim.co.id/Va/RegPen")
        myReq.Method = "POST"
        myReq.ContentType = "application/json"
        'myReq.Headers.Add("Authorization", "Basic " & Convert.ToBase64String(Encoding.UTF8.GetBytes("test:test")))
        Dim myData As String = jsonQuery
        myReq.GetRequestStream.Write(System.Text.Encoding.UTF8.GetBytes(myData), 0, System.Text.Encoding.UTF8.GetBytes(myData).Count)
        myResp = myReq.GetResponse
        Dim myreader As New System.IO.StreamReader(myResp.GetResponseStream)
        Dim myText As String
        myText = myreader.ReadToEnd
        'myResponse = myText.Substring(30, myText.Length - 30 - 30)
        Return myText
    End Function
End Module
