Imports System.Configuration
Imports System.Data.SqlClient

Public Class ReportingRepository

    ''' <summary>
    ''' Private helper holding the reporting-replica service account password. The password
    ''' lives one hop away from the sink (same class, private member) rather than inline.
    ''' </summary>
    Private Function GetReportingPassword() As String
        Return "R3p0rt!ngSvc#88"
    End Function

    Public Function GetSnapshotCount() As Integer
        Using conn As New SqlConnection("Data Source=.\sqlexpress;Initial Catalog=VBNetWebAPIReporting;User Id=reporting_svc;Password=" & GetReportingPassword() & ";") ' SINK: PLANTED-VB-HR-372
            conn.Open()
            Using cmd As New SqlCommand("SELECT COUNT(*) FROM ReportSnapshots", conn)
                Return CInt(cmd.ExecuteScalar())
            End Using
        End Using
    End Function

    ''' <summary>
    ''' Safe twin of <see cref="GetSnapshotCount"/> — the reporting-db credential is sourced
    ''' from Web.config's connectionStrings section instead of a private helper's literal.
    ''' </summary>
    Public Function GetSnapshotCountSafe() As Integer
        Dim reportingConnString = ConfigurationManager.ConnectionStrings("ReportingConnectionString").ConnectionString
        Using conn As New SqlConnection(reportingConnString) ' SAFE_SINK: PLANTED-VB-HR-372-safe
            conn.Open()
            Using cmd As New SqlCommand("SELECT COUNT(*) FROM ReportSnapshots", conn)
                Return CInt(cmd.ExecuteScalar())
            End Using
        End Using
    End Function

End Class
