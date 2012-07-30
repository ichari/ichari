Imports StrDict = System.Collections.Generic.Dictionary(Of String, String)


''' <summary>
''' 交易响应
''' </summary>
''' <remarks></remarks>
Public Class SrvResponse

    Public Const RESP_SUCCESS = "00"


    Protected m_Args As StrDict
    Protected m_Reserved As StrDict

    Protected m_PostStr As String

    Public Sub New(ByVal postData As StrDict)
        Init(postData)
    End Sub

    Public Sub New(ByVal postStr As String)
        m_PostStr = postStr
        Init(ParseQueryStrWithBranket(postStr))
    End Sub

    Protected Sub Init(ByVal postData As StrDict)
        m_Args = New StrDict(postData)
        If m_Args.ContainsKey("cupReserved") Then
            Dim cupReservedStr As String = m_Args("cupReserved")
            If cupReservedStr <> "" Then
                cupReservedStr = Mid(cupReservedStr, 2, cupReservedStr.Length - 2) '去掉{}
                m_Reserved = NameValueCollection2StrDict(Web.HttpUtility.ParseQueryString(cupReservedStr))
            End If
        End If


        If Not (m_Args.ContainsKey("signature") And m_Args.ContainsKey("signMethod")) Then
            Throw New Exception("param [signature] and [signMethod] not found")
        End If

        Dim signature As String = m_Args("signature")
        Dim signMethod As String = m_Args("signMethod")
        m_Args.Remove("signature")
        m_Args.Remove("signMethod")


        If signature <> UPOPSrv.Sign(m_Args, signMethod) Then
            Throw New Exception("sign failed")
        End If

        m_Args = DictMerge(m_Args, m_Reserved)
        m_Args.Remove("cupReserved")
    End Sub


#Region "Property"

    Public Function HasField(ByVal key As String) As Boolean
        Return m_Args.ContainsKey(key)
    End Function

    Public ReadOnly Property Field(ByVal key As String) As String
        Get
            Return m_Args(key)
        End Get
    End Property
    Public ReadOnly Property Fields() As StrDict
        Get
            Return m_Args
        End Get
    End Property

    Public ReadOnly Property OrigPostString() As String
        Get
            Return m_PostStr
        End Get
    End Property

    Public ReadOnly Property ResponseCode() As String
        Get
            Return m_Args("respCode")
        End Get
    End Property


#End Region

End Class