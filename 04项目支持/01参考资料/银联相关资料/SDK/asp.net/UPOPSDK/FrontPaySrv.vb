Imports System.Text

Imports StrDict = System.Collections.Generic.Dictionary(Of String, String)

''' <summary>
''' 前台交易服务
''' </summary>
''' <remarks></remarks>
Public Class FrontPaySrv
    Inherits UPOPSrv

    Private Shared ReadOnly ms_supportedTrade As String() = {TransType.CONSUME, TransType.PRE_AUTH}

    Protected Shared Function IsTradeSupport(ByVal tradeType As String) As Boolean
        Return Array.BinarySearch(Of String)(ms_supportedTrade, tradeType) >= 0
    End Function


    Public Sub New(ByVal args As StrDict)
        MyBase.New(args)
        Dim transTypeVal As String = args("transType")
        If Not IsTradeSupport(transTypeVal) Then
            Throw New Exception("front pay cannot support this trans_type")
        End If

        m_API_URL = Config.frontPayURL
        Init(args)

    End Sub

    Public Function CreateHtml() As String
        Dim html As New StringBuilder
        html.AppendLine("<html>").AppendLine("<head>")
        html.AppendFormat("<meta http-equiv=""Content-Type"" content=""text/html; charset={0}"" />", m_Args("charset")).AppendLine()
        html.AppendLine("</head>")

        html.AppendLine("<body onload=""javascript:document.pay_form.submit();"">")
        html.AppendFormat("<form id=""pay_form"" name=""pay_form"" action=""{0}"" method=""POST"">", m_API_URL).AppendLine()

        For Each k As String In m_Args.Keys
            html.AppendFormat("<input type=""hidden"" name=""{0}"" id=""{0}"" value=""{1}"" />", k, m_Args(k)).AppendLine()
        Next

        html.AppendLine("<input type=""submit"" style=""display:none;"" />")
        html.AppendLine("</form>").AppendLine("</body>").AppendLine("</html>")

        Return html.ToString()
    End Function

    Public ReadOnly Property Charset() As Encoding
        Get
            Dim e As Encoding = Util.GetArgsEncoding(m_Args)
            Return e
        End Get
    End Property
End Class
