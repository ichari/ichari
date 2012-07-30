Imports System.Text
Imports System.Web

Imports StrDict = System.Collections.Generic.Dictionary(Of String, String)


''' <summary>
''' 后台服务
''' </summary>
''' <remarks></remarks>
Public MustInherit Class BackSrv
    Inherits UPOPSrv

    Public Sub New(ByVal args As StrDict)
        MyBase.New(args)
    End Sub

    Public Function Post() As String
        Dim reqStr As String

        Dim encoding As System.Text.Encoding = GetArgsEncoding(m_Args)

        'make req string
        Dim sb As New StringBuilder
        For Each k As String In m_Args.Keys
            sb.Append(k & "=" & HttpUtility.UrlEncode(m_Args(k), encoding))
            sb.Append("&")
        Next
        reqStr = sb.ToString()

        Net.ServicePointManager.Expect100Continue = Config.PostExpect100Continue
        Net.ServicePointManager.ServerCertificateValidationCallback = SSLCertPolicy.CurrentPolicy

        Dim req As Net.HttpWebRequest = Net.WebRequest.Create(m_API_URL)



        Dim reqBuf As Byte() = Text.Encoding.UTF8.GetBytes(reqStr) ' reqStr is Only ASCII

        req.Method = "POST"
        req.ContentType = "application/x-www-form-urlencoded"
        req.ContentLength = reqBuf.Length

        Dim reqStream As IO.Stream = req.GetRequestStream
        reqStream.Write(reqBuf, 0, reqBuf.Length)
        reqStream.Flush()
        reqStream.Close()

        Dim resp As Net.HttpWebResponse = req.GetResponse
        Dim sr As IO.StreamReader = New IO.StreamReader(resp.GetResponseStream, encoding)
        Dim respStr As String = sr.ReadToEnd
        sr.Close()
        resp.Close()
        Return respStr

    End Function

    Public Overridable Function Request() As SrvResponse
        Return New SrvResponse(Post())
    End Function
End Class




''' <summary>
''' 后台交易服务
''' </summary>
''' <remarks></remarks>
Public Class BackPaySrv
    Inherits BackSrv

    Public Sub New(ByVal args As StrDict)
        MyBase.New(args)
        Dim transTypeVal As String = args("transType")
        If transTypeVal = TransType.CONSUME Or transTypeVal = TransType.PRE_AUTH Then
            If Not (args.ContainsKey("cardNumber") Or args.ContainsKey("pan")) Then
                Throw New Exception("trans_type CONSUME or PRE_AUTH need field[cardNumber] ")
            End If
        End If

        m_API_URL = Config.backPayURL

        Init(args)
    End Sub



End Class




''' <summary>
''' 交易查询服务
''' </summary>
''' <remarks></remarks>
Public Class QuerySrv
    Inherits BackSrv


    Public Const QUERY_SUCCESS = "0"
    Public Const QUERY_FAIL = "1"
    Public Const QUERY_WAIT = "2"
    Public Const QUERY_INVALID = "3"

    Private QueryParamPredefList As String() = {"version", "charset", "merId"}

    Public Sub New(ByVal args As StrDict)
        MyBase.New(args)

        m_API_URL = Config.queryURL

        m_Args = args

        For Each k As String In QueryParamPredefList
            m_Args(k) = Config.payParamsPredef(k)
        Next

        m_Args("merReserved") = ""
        If Config.payParamsPredef("acqCode") <> "" Then
            m_Args("merReserved") = "{acqCode=" & Config.payParamsPredef("acqCode") & "}"
        ElseIf Config.payParamsPredef("merId") = "" Then
            Throw New Exception("config:[merId] and [acqCode] cannot both be empty")
        End If

        ' Init()??  merReserved may be covered!
        SignMe()

    End Sub

End Class