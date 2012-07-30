Imports System.Text


Imports StrDict = System.Collections.Generic.Dictionary(Of String, String)

Public Module Util


    ''' <summary>
    ''' 将NameValueCollection转换为Dictionary(Of String,String)
    ''' </summary>
    ''' <param name="nvcoll"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function NameValueCollection2StrDict(ByVal nvcoll As Specialized.NameValueCollection) As StrDict
        Dim dict As New StrDict
        For Each k As String In nvcoll.AllKeys
            dict(k) = nvcoll(k)
        Next
        Return dict
    End Function


    ''' <summary>
    ''' 合并两个Dictionary，如果有重名key，则后一个覆盖前一个的值
    ''' </summary>
    ''' <typeparam name="T"></typeparam>
    ''' <typeparam name="T2"></typeparam>
    ''' <param name="dict1"></param>
    ''' <param name="dict2"></param>
    ''' <returns>合并后的Dictionary</returns>
    ''' <remarks></remarks>
    Public Function DictMerge(Of T, T2)(ByVal dict1 As Dictionary(Of T, T2), ByVal dict2 As Dictionary(Of T, T2)) As Dictionary(Of T, T2)
        If dict1 Is Nothing And dict2 Is Nothing Then
            Return Nothing
        ElseIf dict1 Is Nothing Then
            Return New Dictionary(Of T, T2)(dict2)
        End If

        Dim ret As Dictionary(Of T, T2) = New Dictionary(Of T, T2)(dict1)
        If Not dict2 Is Nothing Then
            For Each k As T In dict2.Keys
                ret(k) = dict2(k)
            Next
        End If
        Return ret
    End Function


    Friend Sub DictInsertEmpty(Of T, T2)(ByVal dict1 As Dictionary(Of T, T2), ByVal keyList As T(), ByVal emptyVal As T2)
        For Each k As T In keyList
            If dict1.ContainsKey(k) Then Continue For
            dict1(k) = emptyVal
        Next
    End Sub


    ''' <summary>
    ''' 计算input字符串的MD5值
    ''' </summary>
    ''' <param name="input">要计算的字符串</param>
    ''' <param name="enc">编码，默认是Encoding.Default</param>
    ''' <returns>小写16进制表示的md5值</returns>
    ''' <remarks></remarks>
    Friend Function MD5Hash(ByVal input As String, Optional ByVal enc As Encoding = Nothing) As String
        Dim md5Hasher As System.Security.Cryptography.MD5 = System.Security.Cryptography.MD5.Create()

        If enc Is Nothing Then enc = Encoding.Default

        Dim data As Byte() = md5Hasher.ComputeHash(enc.GetBytes(input))

        Dim sBuilder As New StringBuilder()

        Dim i As Integer
        For i = 0 To data.Length - 1
            sBuilder.Append(data(i).ToString("x2"))
        Next i

        Return sBuilder.ToString()

    End Function

    Friend Function GetArgsEncoding(ByVal args As StrDict) As Encoding
        If Not args.ContainsKey("charset") Then
            Throw New Exception("args does not contain [charset] field!")
        End If

        Dim strCharset As String = UCase(args("charset"))
        Select Case strCharset
            Case "UTF8", "UTF-8"
                Return Encoding.UTF8
            Case "UNICODE", "UTF-16"
                Return Encoding.Unicode
            Case "GBK", "CP936"
                Return Encoding.GetEncoding("gb2312")
            Case "ASCII"
                Return Encoding.ASCII
            Case Else
                Return Encoding.GetEncoding(strCharset)
        End Select

    End Function

    ''' <summary>
    ''' 解析QueryString，将{}内的串作为一个整体来处理，不拆分。
    ''' </summary>
    ''' <param name="queryStr"></param>
    ''' <returns>解析好的key/value对</returns>
    ''' <remarks></remarks>
    Friend Function ParseQueryStrWithBranket(ByVal queryStr As String) As StrDict
        If (Left(queryStr, 1) <> "&") Then queryStr = "&" & queryStr
        If (Right(queryStr, 1) <> "&") Then queryStr = queryStr & "&"

        Dim dict As New StrDict
        Dim re As New RegularExpressions.Regex("&(.*?)=((\{.*?\})*(.*?))(?=&)")
        For Each m As RegularExpressions.Match In re.Matches(queryStr)
            dict(m.Groups(1).Value) = m.Groups(2).Value
        Next
        Return dict
    End Function




    ''' <summary>
    ''' 可序列化的Dictionary(Of String,String)
    ''' </summary>
    ''' <remarks></remarks>
    Public Class StrDictSerializable
        Inherits Dictionary(Of String, String)
        Implements Xml.Serialization.IXmlSerializable

        Public Function Getschema() As Xml.Schema.XmlSchema Implements Xml.Serialization.IXmlSerializable.GetSchema
            Throw New NotImplementedException
        End Function

        Public Sub ReadXml(ByVal reader As Xml.XmlReader) Implements Xml.Serialization.IXmlSerializable.ReadXml
            If reader.IsEmptyElement Then
                Return
            End If

            reader.Read()
            Try
                Do While reader.NodeType <> Xml.XmlNodeType.EndElement
                    Dim k As String = reader.Name
                    If reader.IsEmptyElement Then
                        Me(k) = ""
                        reader.Read()
                    Else
                        Me(k) = reader.ReadElementString()
                    End If
                Loop
                reader.Read()
            Catch ex As Exception
                MsgBox(ex.Message)
            End Try
        End Sub


        Public Sub WriteXml(ByVal writer As Xml.XmlWriter) Implements Xml.Serialization.IXmlSerializable.WriteXml
            For Each k In Keys
                writer.WriteElementString(k, Me(k))
            Next
        End Sub

    End Class
End Module
