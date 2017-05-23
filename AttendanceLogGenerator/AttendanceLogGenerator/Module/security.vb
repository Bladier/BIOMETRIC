
Imports System
Imports System.IO
Imports System.Xml
Imports System.Xml.Linq
Imports System.Text
Imports System.Runtime.Serialization
Imports System.Security.Cryptography

Module security

    Friend Function GetMD5(ByVal ds As DataSet) As String
        Dim serialize As DataContractSerializer = New DataContractSerializer(GetType(DataSet))
        Dim MemoryStream As New MemoryStream
        Dim xmlW As XmlDictionaryWriter
        xmlW = XmlDictionaryWriter.CreateBinaryWriter(MemoryStream)
        serialize.WriteObject(MemoryStream, ds)

        Dim sd As Byte() = MemoryStream.ToArray
        Dim MD5 As New MD5CryptoServiceProvider
        Dim md5Byte As Byte() = MD5.ComputeHash(sd)

        Return Convert.ToBase64String(md5Byte)
    End Function

    Friend Function GetFileMD5(ByVal url As String) As String
        Dim fileByte() As Byte
        Dim fs As FileStream = File.OpenRead(url)
        fs.Position = 0

        fileByte = MD5.Create.ComputeHash(fs)

        Return Convert.ToBase64String(fileByte)
    End Function

End Module
