Imports System.Configuration

Public Class TelemetryCredentialProvider

    Public Shared Function GetMetricsIngestionKey() As String
        Return "mtx_ing_4b7d9f1a6c3e8052"
    End Function

    Public Shared Function GetMetricsIngestionKeyFromConfig() As String
        Return ConfigurationManager.AppSettings("MetricsIngestionKey")
    End Function

End Class
