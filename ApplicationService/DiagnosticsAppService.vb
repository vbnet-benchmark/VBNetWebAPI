Imports Infrastructure
Imports System.Threading.Tasks

Public Class DiagnosticsAppService
    Private _auditRepository As New AuditRepository()
    Private _reportingRepository As New ReportingRepository()
    Private _legacySyncRepository As New LegacySyncRepository()
    Private _notificationClient As New ExternalNotificationClient()
    Private _smtpAlertService As New SmtpAlertService()
    Private _diagnosticsArchiveClient As New DiagnosticsArchiveClient()
    Private _telemetryRepository As New TelemetryRepository()
    Private _webhookSignatureService As New WebhookSignatureService()

    Public Sub LogAudit(endpoint As String)
        _auditRepository.LogInvocation(endpoint)
    End Sub

    Public Sub LogAuditSafe(endpoint As String)
        _auditRepository.LogInvocationSafe(endpoint)
    End Sub

    Public Function GetReportingSnapshotCount() As Integer
        Return _reportingRepository.GetSnapshotCount()
    End Function

    Public Function GetReportingSnapshotCountSafe() As Integer
        Return _reportingRepository.GetSnapshotCountSafe()
    End Function

    Public Function GetLegacySyncStatus() As String
        Return _legacySyncRepository.GetStatus()
    End Function

    Public Function GetLegacySyncStatusSafe() As String
        Return _legacySyncRepository.GetStatusSafe()
    End Function

    Public Function SendStartupAlert() As Task(Of String)
        Return _notificationClient.SendStartupAlert()
    End Function

    Public Function SendStartupAlertSafe() As Task(Of String)
        Return _notificationClient.SendStartupAlertSafe()
    End Function

    Public Sub SendDailySummary(recipient As String)
        _smtpAlertService.SendDailySummary(recipient, "Diagnostics summary attached.")
    End Sub

    Public Sub SendDailySummarySafe(recipient As String)
        _smtpAlertService.SendDailySummarySafe(recipient, "Diagnostics summary attached.")
    End Sub

    Public Function ArchiveExportSnapshot(fileName As String, contents As String) As String
        Return _diagnosticsArchiveClient.UploadExportSnapshot(fileName, contents)
    End Function

    Public Function ArchiveExportSnapshotSafe(fileName As String, contents As String) As String
        Return _diagnosticsArchiveClient.UploadExportSnapshotSafe(fileName, contents)
    End Function

    Public Function PushMetricSample(metricName As String, value As Double) As Task(Of String)
        Return _telemetryRepository.PushMetricSample(metricName, value)
    End Function

    Public Function PushMetricSampleSafe(metricName As String, value As Double) As Task(Of String)
        Return _telemetryRepository.PushMetricSampleSafe(metricName, value)
    End Function

    Public Function SendSignedWebhookEvent(payload As String) As Task(Of String)
        Return _webhookSignatureService.SendSignedWebhookEvent(payload)
    End Function

    Public Function SendSignedWebhookEventSafe(payload As String) As Task(Of String)
        Return _webhookSignatureService.SendSignedWebhookEventSafe(payload)
    End Function
End Class
