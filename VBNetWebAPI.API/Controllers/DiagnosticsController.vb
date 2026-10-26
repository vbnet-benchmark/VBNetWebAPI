Imports System.Configuration
Imports System.Net
Imports System.Net.Http
Imports System.Threading.Tasks
Imports System.Web.Http
Imports ApplicationService

Namespace Controllers
    Public Class DiagnosticsController
        Inherits ApiController

        Private _diagnosticsAppService As New DiagnosticsAppService()

        <HttpGet>
        <Route("api/diagnostics/audit")>
        Public Function LogAudit() As HttpResponseMessage
            _diagnosticsAppService.LogAudit("api/diagnostics/audit")
            Return Request.CreateResponse(HttpStatusCode.OK, "audit logged")
        End Function

        <HttpGet>
        <Route("api/diagnostics/audit-safe")>
        Public Function LogAuditSafe() As HttpResponseMessage
            _diagnosticsAppService.LogAuditSafe("api/diagnostics/audit-safe")
            Return Request.CreateResponse(HttpStatusCode.OK, "audit logged")
        End Function

        <HttpGet>
        <Route("api/diagnostics/reporting")>
        Public Function ReportingSnapshot() As HttpResponseMessage
            Return Request.CreateResponse(HttpStatusCode.OK, _diagnosticsAppService.GetReportingSnapshotCount())
        End Function

        <HttpGet>
        <Route("api/diagnostics/reporting-safe")>
        Public Function ReportingSnapshotSafe() As HttpResponseMessage
            Return Request.CreateResponse(HttpStatusCode.OK, _diagnosticsAppService.GetReportingSnapshotCountSafe())
        End Function

        <HttpGet>
        <Route("api/diagnostics/legacy-sync")>
        Public Function LegacySyncStatus() As HttpResponseMessage
            Return Request.CreateResponse(HttpStatusCode.OK, _diagnosticsAppService.GetLegacySyncStatus())
        End Function

        <HttpGet>
        <Route("api/diagnostics/legacy-sync-safe")>
        Public Function LegacySyncStatusSafe() As HttpResponseMessage
            Return Request.CreateResponse(HttpStatusCode.OK, _diagnosticsAppService.GetLegacySyncStatusSafe())
        End Function

        ''' <summary>
        ''' A support/override bypass gate guarded by a hardcoded key compared directly against
        ''' the caller-supplied value. Substituted for a genuine type/polymorphism-dependent
        ''' shape — this project has no application-defined interface/MustInherit hierarchy that
        ''' could plausibly gate a credential-source decision (confirmed by grep), the same
        ''' substitution already used for this reason in prior rounds on this campaign.
        ''' </summary>
        <HttpGet>
        <Route("api/diagnostics/support")>
        Public Function SupportOverride(key As String) As HttpResponseMessage
            If key = "Supp0rt-Override-9f3c" Then ' SINK: PLANTED-VB-HR-374
                Return Request.CreateResponse(HttpStatusCode.OK, "support override granted")
            End If
            Return Request.CreateResponse(HttpStatusCode.Forbidden, "invalid support key")
        End Function

        ''' <summary>
        ''' Safe twin of <see cref="SupportOverride"/> — the override key is sourced from
        ''' Web.config's appSettings section instead of being written into source.
        ''' </summary>
        <HttpGet>
        <Route("api/diagnostics/support-safe")>
        Public Function SupportOverrideSafe(key As String) As HttpResponseMessage
            If key = ConfigurationManager.AppSettings("SupportOverrideKey") Then ' SAFE_SINK: PLANTED-VB-HR-374-safe
                Return Request.CreateResponse(HttpStatusCode.OK, "support override granted")
            End If
            Return Request.CreateResponse(HttpStatusCode.Forbidden, "invalid support key")
        End Function

        <HttpGet>
        <Route("api/diagnostics/notify")>
        Public Async Function NotifyWebhook() As Task(Of HttpResponseMessage)
            Dim result = Await _diagnosticsAppService.SendStartupAlert()
            Return Request.CreateResponse(HttpStatusCode.OK, result)
        End Function

        <HttpGet>
        <Route("api/diagnostics/notify-safe")>
        Public Async Function NotifyWebhookSafe() As Task(Of HttpResponseMessage)
            Dim result = Await _diagnosticsAppService.SendStartupAlertSafe()
            Return Request.CreateResponse(HttpStatusCode.OK, result)
        End Function

        <HttpGet>
        <Route("api/diagnostics/email-summary")>
        Public Function EmailSummary(recipient As String) As HttpResponseMessage
            _diagnosticsAppService.SendDailySummary(recipient)
            Return Request.CreateResponse(HttpStatusCode.OK, "summary emailed")
        End Function

        <HttpGet>
        <Route("api/diagnostics/email-summary-safe")>
        Public Function EmailSummarySafe(recipient As String) As HttpResponseMessage
            _diagnosticsAppService.SendDailySummarySafe(recipient)
            Return Request.CreateResponse(HttpStatusCode.OK, "summary emailed")
        End Function

        <HttpGet>
        <Route("api/diagnostics/archive-export")>
        Public Function ArchiveExport(fileName As String, contents As String) As HttpResponseMessage
            Dim status = _diagnosticsAppService.ArchiveExportSnapshot(fileName, contents)
            Return Request.CreateResponse(HttpStatusCode.OK, status)
        End Function

        <HttpGet>
        <Route("api/diagnostics/archive-export-safe")>
        Public Function ArchiveExportSafe(fileName As String, contents As String) As HttpResponseMessage
            Dim status = _diagnosticsAppService.ArchiveExportSnapshotSafe(fileName, contents)
            Return Request.CreateResponse(HttpStatusCode.OK, status)
        End Function

        <HttpGet>
        <Route("api/diagnostics/metric")>
        Public Async Function Metric(metricName As String, value As Double) As Task(Of HttpResponseMessage)
            Dim result = Await _diagnosticsAppService.PushMetricSample(metricName, value)
            Return Request.CreateResponse(HttpStatusCode.OK, result)
        End Function

        <HttpGet>
        <Route("api/diagnostics/metric-safe")>
        Public Async Function MetricSafe(metricName As String, value As Double) As Task(Of HttpResponseMessage)
            Dim result = Await _diagnosticsAppService.PushMetricSampleSafe(metricName, value)
            Return Request.CreateResponse(HttpStatusCode.OK, result)
        End Function

        <HttpGet>
        <Route("api/diagnostics/webhook-signed")>
        Public Async Function WebhookSigned(payload As String) As Task(Of HttpResponseMessage)
            Dim result = Await _diagnosticsAppService.SendSignedWebhookEvent(payload)
            Return Request.CreateResponse(HttpStatusCode.OK, result)
        End Function

        <HttpGet>
        <Route("api/diagnostics/webhook-signed-safe")>
        Public Async Function WebhookSignedSafe(payload As String) As Task(Of HttpResponseMessage)
            Dim result = Await _diagnosticsAppService.SendSignedWebhookEventSafe(payload)
            Return Request.CreateResponse(HttpStatusCode.OK, result)
        End Function

    End Class
End Namespace
