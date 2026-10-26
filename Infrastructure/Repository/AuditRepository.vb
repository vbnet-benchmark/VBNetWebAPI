Imports System.Configuration
Imports System.Data.SqlClient

Public Class AuditRepository

    ''' <summary>
    ''' Records an API invocation into the dedicated audit-log database.
    ''' </summary>
    Public Sub LogInvocation(endpoint As String)
        Using conn As New SqlConnection("Data Source=.\sqlexpress;Initial Catalog=VBNetWebAPIAudit;User Id=audit_svc;Password=A6!auditSvc$2024;") ' SINK: PLANTED-VB-HR-371
            conn.Open()
            Using cmd As New SqlCommand("INSERT INTO AuditLog (Endpoint, InvokedAtUtc) VALUES (@endpoint, GETUTCDATE())", conn)
                cmd.Parameters.AddWithValue("@endpoint", endpoint)
                cmd.ExecuteNonQuery()
            End Using
        End Using
    End Sub

    ''' <summary>
    ''' Safe twin of <see cref="LogInvocation"/> — the audit-db credential is sourced from
    ''' Web.config's connectionStrings section instead of being written into source.
    ''' </summary>
    Public Sub LogInvocationSafe(endpoint As String)
        Dim auditConnString = ConfigurationManager.ConnectionStrings("AuditConnectionString").ConnectionString
        Using conn As New SqlConnection(auditConnString) ' SAFE_SINK: PLANTED-VB-HR-371-safe
            conn.Open()
            Using cmd As New SqlCommand("INSERT INTO AuditLog (Endpoint, InvokedAtUtc) VALUES (@endpoint, GETUTCDATE())", conn)
                cmd.Parameters.AddWithValue("@endpoint", endpoint)
                cmd.ExecuteNonQuery()
            End Using
        End Using
    End Sub

End Class
