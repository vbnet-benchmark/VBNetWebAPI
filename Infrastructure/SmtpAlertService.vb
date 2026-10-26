Imports System.Configuration
Imports System.Net
Imports System.Net.Mail

Public Class SmtpAlertService

    ''' <summary>
    ''' Sends a daily diagnostics-summary email through the corporate SMTP relay, authenticating
    ''' with a hardcoded mailbox account. Distinct construction shape from the other Infrastructure
    ''' credential sinks: the secret is passed directly into a NetworkCredential assigned to
    ''' SmtpClient.Credentials, not a connection string or an HTTP header.
    ''' </summary>
    Public Sub SendDailySummary(recipient As String, body As String)
        Using client As New SmtpClient("mail.vbnetwebapi.example.com", 587)
            client.EnableSsl = True
            client.Credentials = New NetworkCredential("alerts@vbnetwebapi.example.com", "Al3rtsSvc!2024#mail") ' SINK: PLANTED-VB-HR-431
            Using message As New MailMessage("alerts@vbnetwebapi.example.com", recipient, "VBNetWebAPI Diagnostics Summary", body)
                client.Send(message)
            End Using
        End Using
    End Sub

    ''' <summary>
    ''' Safe twin of <see cref="SendDailySummary"/> — the mailbox credential is sourced from
    ''' Web.config's appSettings section instead of being written into source.
    ''' </summary>
    Public Sub SendDailySummarySafe(recipient As String, body As String)
        Using client As New SmtpClient("mail.vbnetwebapi.example.com", 587)
            client.EnableSsl = True
            Dim mailPassword = ConfigurationManager.AppSettings("SmtpAlertMailboxPassword")
            client.Credentials = New NetworkCredential("alerts@vbnetwebapi.example.com", mailPassword) ' SAFE_SINK: PLANTED-VB-HR-431-safe
            Using message As New MailMessage("alerts@vbnetwebapi.example.com", recipient, "VBNetWebAPI Diagnostics Summary", body)
                client.Send(message)
            End Using
        End Using
    End Sub

End Class
