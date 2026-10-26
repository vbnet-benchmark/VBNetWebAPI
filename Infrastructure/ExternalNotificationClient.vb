Imports System.Configuration
Imports System.Net.Http
Imports System.Net.Http.Headers
Imports System.Threading.Tasks

Public Class ExternalNotificationClient

    ''' <summary>
    ''' Posts a startup-health alert to the external monitoring webhook, authenticating with a
    ''' bearer API key. Distinct construction shape from the connection-string instances above:
    ''' the credential is embedded directly into an HTTP Authorization header, not a DB sink.
    ''' </summary>
    Public Async Function SendStartupAlert() As Task(Of String)
        Using client As New HttpClient()
            client.DefaultRequestHeaders.Authorization = New AuthenticationHeaderValue("Bearer", "sk_live_7f1c9a2b4e6d8f0a1b3c5d7e9f") ' SINK: PLANTED-VB-HR-375
            Dim response = Await client.PostAsync("https://notify.vbnetwebapi-monitoring.example.com/api/alerts", New StringContent("{""event"":""startup""}"))
            Return response.StatusCode.ToString()
        End Using
    End Function

    ''' <summary>
    ''' Safe twin of <see cref="SendStartupAlert"/> — the bearer API key is sourced from
    ''' Web.config's appSettings section instead of being written into source.
    ''' </summary>
    Public Async Function SendStartupAlertSafe() As Task(Of String)
        Using client As New HttpClient()
            Dim apiKey = ConfigurationManager.AppSettings("NotificationApiKey")
            client.DefaultRequestHeaders.Authorization = New AuthenticationHeaderValue("Bearer", apiKey) ' SAFE_SINK: PLANTED-VB-HR-375-safe
            Dim response = Await client.PostAsync("https://notify.vbnetwebapi-monitoring.example.com/api/alerts", New StringContent("{""event"":""startup""}"))
            Return response.StatusCode.ToString()
        End Using
    End Function

End Class
