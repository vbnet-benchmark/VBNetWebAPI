Imports System.Configuration
Imports System.Net.Http
Imports System.Security.Cryptography
Imports System.Text
Imports System.Threading.Tasks

Public Class WebhookSignatureService

    ''' <summary>
    ''' Posts a signed diagnostics webhook event, computing an HMAC-SHA256 signature over the
    ''' payload with a hardcoded shared secret and sending only the signature (never the secret
    ''' itself) in the X-Signature header. Diversity instance: the credential is consumed as an
    ''' HMAC key rather than placed directly into a connection string, an HTTP header value, or a
    ''' comparison — a different construction/consumption shape from every other instance in this
    ''' round and from Round 19's bearer-API-key-in-header shape.
    ''' </summary>
    Public Async Function SendSignedWebhookEvent(payload As String) As Task(Of String)
        Dim signature = ComputeSignature(payload, "whsec_7c3f9a1e5b2d8046a9c1f3e7b5d2a8f0") ' SINK: PLANTED-VB-HR-435
        Using client As New HttpClient()
            client.DefaultRequestHeaders.Add("X-Signature", signature)
            Dim response = Await client.PostAsync("https://notify.vbnetwebapi-monitoring.example.com/api/webhooks/diagnostics", New StringContent(payload))
            Return response.StatusCode.ToString()
        End Using
    End Function

    ''' <summary>
    ''' Safe twin of <see cref="SendSignedWebhookEvent"/> — the shared signing secret is sourced
    ''' from Web.config's appSettings section instead of being written into source.
    ''' </summary>
    Public Async Function SendSignedWebhookEventSafe(payload As String) As Task(Of String)
        Dim sharedSecret = ConfigurationManager.AppSettings("DiagnosticsWebhookSharedSecret")
        Dim signature = ComputeSignature(payload, sharedSecret) ' SAFE_SINK: PLANTED-VB-HR-435-safe
        Using client As New HttpClient()
            client.DefaultRequestHeaders.Add("X-Signature", signature)
            Dim response = Await client.PostAsync("https://notify.vbnetwebapi-monitoring.example.com/api/webhooks/diagnostics", New StringContent(payload))
            Return response.StatusCode.ToString()
        End Using
    End Function

    Private Function ComputeSignature(payload As String, secret As String) As String
        Using hmac As New HMACSHA256(Encoding.UTF8.GetBytes(secret))
            Dim hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(payload))
            Return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant()
        End Using
    End Function

End Class
