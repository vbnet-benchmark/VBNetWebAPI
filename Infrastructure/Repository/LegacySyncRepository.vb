Imports System.Data.Odbc
Imports Domain

Public Class LegacySyncRepository

    ''' <summary>
    ''' Checks reachability of the legacy MySQL-backed sync replica via ODBC. The credential is
    ''' defined in a different class/file (<see cref="LegacySyncCredentialProvider"/>, Domain
    ''' layer) and reaches this sink through that cross-file call.
    ''' </summary>
    Public Function GetStatus() As String
        Using conn As New OdbcConnection("Driver={MySQL ODBC 8.0 Unicode Driver};Server=legacy-sync.internal;Database=vbnetwebapi_sync;Uid=sync_svc;Pwd=" & LegacySyncCredentialProvider.GetLegacySyncPassword() & ";") ' SINK: PLANTED-VB-HR-373
            conn.Open()
            Return "legacy sync reachable"
        End Using
    End Function

    ''' <summary>
    ''' Safe twin of <see cref="GetStatus"/> — the cross-file provider reads the credential from
    ''' Web.config's appSettings section instead of returning a hardcoded literal.
    ''' </summary>
    Public Function GetStatusSafe() As String
        Dim legacyPassword = LegacySyncCredentialProvider.GetLegacySyncPasswordFromConfig()
        Using conn As New OdbcConnection("Driver={MySQL ODBC 8.0 Unicode Driver};Server=legacy-sync.internal;Database=vbnetwebapi_sync;Uid=sync_svc;Pwd=" & legacyPassword & ";") ' SAFE_SINK: PLANTED-VB-HR-373-safe
            conn.Open()
            Return "legacy sync reachable"
        End Using
    End Function

End Class
