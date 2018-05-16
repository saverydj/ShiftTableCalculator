using System;
using STARS.Applications.VETS.Interfaces.Constants;
using STARS.Applications.VETS.Interfaces.Logging;
using STARS.Applications.VETS.Plugins.ShiftTableCalculator.Properties;
using STARS.Applications.Interfaces.Dialogs;

namespace STARS.Applications.VETS.Plugins.ShiftTableCalculator
{
    public static class SystemLogService
    {
        private static ISystemLogManager _logger;
        public static ISystemLogManager Logger { get { return _logger; } set { _logger = value; } }

        private static IDialogService _dialogService;
        public static IDialogService DialogService { get { return _dialogService; } set { _dialogService = value; } }

        public static void DisplayErrorInVETSLog(Exception e)
        {
            _logger.AddLogEntry(System.Diagnostics.TraceEventType.Error, SystemLogSources.DataAccess, String.Format(Resources.ErrorMessageHeader, e.Message) + "\r\n" + e.StackTrace);
            throw e;
        }

        public static void DisplayErrorInVETSLog(string message, string title = null)
        {
            if (title != null) DisplayErrorInPopup(title, message);
            throw new Exception(String.Format(Resources.ErrorMessageHeader, message));
        }

        public static void DisplayErrorInPopup(string title, string message)
        {
            _dialogService.PromptUser(title, message, DialogIcon.Alert, DialogButton.OK, DialogButton.OK);
        }

        public static void DisplayMessageInVETSLog(string message, string title = null)
        {
            if (title != null) DisplayMessageInPopup(title, message);
            _logger.AddLogEntry(System.Diagnostics.TraceEventType.Information, SystemLogSources.DataAccess, message);
        }

        public static void DisplayMessageInPopup(string title, string message)
        {
            _dialogService.PromptUser(title, message, DialogIcon.Information, DialogButton.OK, DialogButton.OK);
        }

        public static void DisplayErrorInVETSLogNoReturn(string message, string title = null)
        {
            if (title != null) DisplayErrorInPopup(title, message);
            _logger.AddLogEntry(System.Diagnostics.TraceEventType.Error, SystemLogSources.DataAccess, String.Format(Resources.ErrorMessageHeader, message));
        }
    }

}
