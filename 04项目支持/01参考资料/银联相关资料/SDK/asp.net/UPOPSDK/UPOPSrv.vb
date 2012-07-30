
Imports StrDict = System.Collections.Generic.Dictionary(Of String, String)
Imports System.Text

Public MustInherit Class UPOPSrv

    Public Structure TransType
        ''' <summary>消费</summary>
        Public Const CONSUME = "01"

        ''' <summary>消费撤销</summary>
        Public Const CONSUME_VOID = "31"

        ''' <summary>预授权</summary>
        Public Const PRE_AUTH = "02"

        ''' <summary>预授权撤销</summary>
        Public Const PRE_AUTH_VOID = "32"

        ''' <summary>预授权完成</summary>
        Public Const PRE_AUTH_COMPLETE = "03"

        ''' <summary>预授权完成撤销</summary>
        Public Const PRE_AUTH_VOID_COMPLETE = "33"

        ''' <summary>退货</summary>
        Public Const REFUND = "04"

        ''' <summary>实名认证</summary>
        Public Const REGISTRATION = "71"

        Private none As Integer
    End Structure

    Public Const CURRENCY_CNY = "156"

    Public Structure ConfigInf
        Public signMethod As String
        Public securityKey As String

        Public frontPayURL As String
        Public backPayURL As String
        Public queryURL As String

        Public SSLCertPolicy As String
        Public SSLCertStorePath As String

        Public PostExpect100Continue As Boolean

        Public payParamsPredef As StrDictSerializable
        Public payParams As String()
        Public payParamsNotEmpty As String()
        Public queryParams As String()
        Public merReservedParams As String()
    End Structure

    Public Shared Config As ConfigInf

    ''' <summary>
    ''' 提供对https认证策略的多种支持
    ''' </summary>
    ''' <remarks></remarks>
    Protected Class SSLCertPolicy

        Protected Shared trustCerts As New List(Of System.Security.Cryptography.X509Certificates.X509Certificate)
        Public Shared CurrentPolicy As System.Net.Security.RemoteCertificateValidationCallback

        Public Shared Sub InitFromConfig()
            If UCase(Config.SSLCertPolicy) = "IGNORE" Then
                SSLCertPolicy.CurrentPolicy = AddressOf SSLCertPolicy.IgnoreAllValidate
            ElseIf UCase(Config.SSLCertPolicy) = "TRUSTSTORE" Then
                If Config.SSLCertStorePath Is Nothing OrElse Config.SSLCertPolicy = "" Then
                    Throw New Exception("Config: SSLCertPolicy=TrustStore, valid SSLCertStorePath needed!")
                End If
                SSLCertPolicy.LaodTrustCerts(Config.SSLCertStorePath)
                SSLCertPolicy.CurrentPolicy = AddressOf SSLCertPolicy.CheckTrustStoreValidate
            Else
                SSLCertPolicy.CurrentPolicy = Nothing
            End If
        End Sub

        Public Shared Sub LaodTrustCerts(ByVal trustStorePath As String)
            If trustStorePath.Trim = "" Then Return

            trustCerts.Clear()
            Dim strFiles = My.Computer.FileSystem.GetFiles(trustStorePath)
            For Each strFile In strFiles
                If Right(strFile, 4) <> ".cer" Then Continue For
                trustCerts.Add(System.Security.Cryptography.X509Certificates.X509Certificate.CreateFromCertFile(strFile))
            Next

        End Sub

        Public Shared Function IgnoreAllValidate(ByVal sender As Object, ByVal certificate As System.Security.Cryptography.X509Certificates.X509Certificate, _
                            ByVal chain As System.Security.Cryptography.X509Certificates.X509Chain, _
                            ByVal sslPolicyErrors As Net.Security.SslPolicyErrors) As Boolean
            Return True
        End Function

        Public Shared Function CheckTrustStoreValidate(ByVal sender As Object, ByVal certificate As System.Security.Cryptography.X509Certificates.X509Certificate, _
                            ByVal chain As System.Security.Cryptography.X509Certificates.X509Chain, _
                            ByVal sslPolicyErrors As Net.Security.SslPolicyErrors) As Boolean
            For Each cert In trustCerts
                If cert.Equals(certificate) Then
                    Return True
                End If
            Next
            Return False
        End Function

    End Class


    Protected m_API_URL As String
    Protected m_Args As StrDict


    Public Sub New(ByVal args As StrDict)

    End Sub

    Protected Function Init(ByVal args As StrDict) As Integer

        Me.m_Args = DictMerge(Config.payParamsPredef, args)
        DictInsertEmpty(Me.m_Args, Config.payParams, "")

        Debug.Assert(Me.m_Args.ContainsKey("commodityUrl"))
        Me.m_Args("commodityUrl") = System.Uri.EscapeUriString(args("commodityUrl"))

        'merReserverd field:
        Dim merReservedParamList As New List(Of String)

        For Each k As String In Config.merReservedParams
            If Me.m_Args.ContainsKey(k) Then
                Dim item As String = k & "=" & m_Args(k)
                merReservedParamList.Add(item)
                Me.m_Args.Remove(k)
            End If

        Next

        Dim merReservedStr As String = Join(merReservedParamList.ToArray(), "&")
        If merReservedStr <> "" Then merReservedStr = "{" & merReservedStr & "}"
        Me.m_Args("merReserved") = merReservedStr


        'param check:
        For Each k As String In Config.payParamsNotEmpty
            If m_Args(k) = "" Then
                Throw New Exception("key [" & k & "] cannot be empty")
            End If
        Next

        'signature
        SignMe()


    End Function


    Protected Sub SignMe()
        m_Args("signature") = Sign(m_Args, Config.signMethod)
        m_Args("signMethod") = Config.signMethod
    End Sub



    Friend Shared Function Sign(ByVal args As StrDict, ByVal Method As String) As String

        Dim signResult As String
        If UCase(Method) = "MD5" Then
            Dim signStr As New StringBuilder
            Dim enc As Encoding = GetArgsEncoding(args)

            Dim keys(0 To args.Keys.Count - 1) As String
            args.Keys.CopyTo(keys, 0)
            Array.Sort(keys, StringComparer.Ordinal)
            For Each k As String In keys
                signStr.AppendFormat("{0}={1}&", k, args(k))
            Next
            Console.WriteLine("Sign:" & signStr.ToString())
            signResult = signStr.ToString()
            signResult = MD5Hash(signResult & MD5Hash(Config.securityKey, enc), enc)
        Else
            Throw New Exception("unsupported signMethod")
        End If
        Return signResult

    End Function

    Public Shared Function LoadConf(ByVal xmlStream As IO.Stream) As Boolean
        Dim xs As System.Xml.Serialization.XmlSerializer = New System.Xml.Serialization.XmlSerializer(GetType(ConfigInf))
        Config = xs.Deserialize(xmlStream)

        SSLCertPolicy.InitFromConfig()

        Return True
    End Function
    Public Shared Function LoadConf(ByVal confFileName As String) As Boolean
        Dim FS As New IO.FileStream(confFileName, IO.FileMode.Open)
        Dim ret As Boolean = LoadConf(FS)
        FS.Close()
        Return ret
    End Function


#Region "Propertys"
    Public ReadOnly Property Param(ByVal key As String)
        Get
            Return Me.m_Args(key)
        End Get
    End Property

    Public ReadOnly Property Params()
        Get
            Return m_Args
        End Get
    End Property

    Public Function HasParam(ByVal key As String) As Boolean
        Return m_Args.ContainsKey(key)
    End Function
#End Region



End Class
