Imports System.Configuration
Imports System.Net
Imports System.Text

Public Class DiagnosticsArchiveClient

    ''' <summary>
    ''' Hardcoded password for the legacy FTP archive service account. Held one hop away from the
    ''' sink (same class, private field) rather than inline in the FtpWebRequest construction.
    ''' </summary>
    Private Shared ReadOnly ArchiveFtpPassword As String = "Arch1veSvc#Ftp99"

    ''' <summary>
    ''' Uploads a diagnostics export snapshot to the legacy FTP archive server. Distinct
    ''' construction shape from the other Infrastructure credential sinks: an FTP
    ''' NetworkCredential sourced from a private class-level field, not a connection string, an
    ''' HTTP header, or a same-class private method's return value.
    ''' </summary>
    Public Function UploadExportSnapshot(fileName As String, contents As String) As String
        Dim request = CType(WebRequest.Create("ftp://archive.vbnetwebapi.internal/exports/" & fileName), FtpWebRequest)
        request.Method = WebRequestMethods.Ftp.UploadFile
        request.Credentials = New NetworkCredential("archive_svc", ArchiveFtpPassword) ' SINK: PLANTED-VB-HR-432
        Dim data = Encoding.UTF8.GetBytes(contents)
        Using requestStream = request.GetRequestStream()
            requestStream.Write(data, 0, data.Length)
        End Using
        Dim response = CType(request.GetResponse(), FtpWebResponse)
        Dim status = response.StatusDescription
        response.Close()
        Return status
    End Function

    ''' <summary>
    ''' Safe twin of <see cref="UploadExportSnapshot"/> — the FTP archive password is sourced from
    ''' Web.config's appSettings section instead of a private field's literal.
    ''' </summary>
    Public Function UploadExportSnapshotSafe(fileName As String, contents As String) As String
        Dim archivePassword = ConfigurationManager.AppSettings("DiagnosticsArchiveFtpPassword")
        Dim request = CType(WebRequest.Create("ftp://archive.vbnetwebapi.internal/exports/" & fileName), FtpWebRequest)
        request.Method = WebRequestMethods.Ftp.UploadFile
        request.Credentials = New NetworkCredential("archive_svc", archivePassword) ' SAFE_SINK: PLANTED-VB-HR-432-safe
        Dim data = Encoding.UTF8.GetBytes(contents)
        Using requestStream = request.GetRequestStream()
            requestStream.Write(data, 0, data.Length)
        End Using
        Dim response = CType(request.GetResponse(), FtpWebResponse)
        Dim status = response.StatusDescription
        response.Close()
        Return status
    End Function

End Class
