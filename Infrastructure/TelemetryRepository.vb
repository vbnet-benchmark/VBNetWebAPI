Imports System.Net.Http
Imports System.Threading.Tasks
Imports Domain

Public Class TelemetryRepository

    ''' <summary>
    ''' Pushes a diagnostics metric sample to the external metrics-ingestion endpoint. The
    ''' ingestion key is defined in a different class/file (<see cref="TelemetryCredentialProvider"/>,
    ''' Domain layer) and reaches this sink through that cross-file, cross-layer call — a genuinely
    ''' separate chain from the Domain-to-Infrastructure ODBC chain planted in Round 19, consuming
    ''' the key as a custom request header rather than a connection-string fragment.
    ''' </summary>
    Public Async Function PushMetricSample(metricName As String, value As Double) As Task(Of String)
        Using client As New HttpClient()
            client.DefaultRequestHeaders.Add("X-Metrics-Key", TelemetryCredentialProvider.GetMetricsIngestionKey()) ' SINK: PLANTED-VB-HR-433
            Dim payload = New StringContent("{""metric"":""" & metricName & """,""value"":" & value.ToString() & "}")
            Dim response = Await client.PostAsync("https://metrics.vbnetwebapi-monitoring.example.com/api/ingest", payload)
            Return response.StatusCode.ToString()
        End Using
    End Function

    ''' <summary>
    ''' Safe twin of <see cref="PushMetricSample"/> — the cross-file provider reads the ingestion
    ''' key from Web.config's appSettings section instead of returning a hardcoded literal.
    ''' </summary>
    Public Async Function PushMetricSampleSafe(metricName As String, value As Double) As Task(Of String)
        Using client As New HttpClient()
            Dim ingestionKey = TelemetryCredentialProvider.GetMetricsIngestionKeyFromConfig()
            client.DefaultRequestHeaders.Add("X-Metrics-Key", ingestionKey) ' SAFE_SINK: PLANTED-VB-HR-433-safe
            Dim payload = New StringContent("{""metric"":""" & metricName & """,""value"":" & value.ToString() & "}")
            Dim response = Await client.PostAsync("https://metrics.vbnetwebapi-monitoring.example.com/api/ingest", payload)
            Return response.StatusCode.ToString()
        End Using
    End Function

End Class
