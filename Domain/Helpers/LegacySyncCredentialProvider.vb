Imports System.Configuration

Public Class LegacySyncCredentialProvider

    Public Shared Function GetLegacySyncPassword() As String
        Return "LegacySync!2003#db"
    End Function

    Public Shared Function GetLegacySyncPasswordFromConfig() As String
        Return ConfigurationManager.AppSettings("LegacySyncPassword")
    End Function

End Class
