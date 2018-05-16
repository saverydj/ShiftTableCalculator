using System;
using System.ComponentModel.Composition;
using System.Diagnostics;
using STARS.Applications.Interfaces.Dialogs;
using STARS.Applications.VETS.Interfaces.Constants;
using STARS.Applications.VETS.Interfaces.Logging;
using STARS.Applications.VETS.Interfaces.ViewModels;
using STARS.Applications.VETS.Plugins.Enterprise.Common;
using STARS.Applications.VETS.Plugins.Enterprise.Common.Repository;
using STARS.Applications.VETS.Plugins.Enterprise.UI.VTS.Properties;
using STARS.Applications.VETS.Plugins.Enterprise.UI.VTS.ViewModels.Home;

namespace STARS.Applications.VETS.Plugins.Enterprise.UI.VTS
{
    [Export("VTS", typeof(IUiLoginService)), PartCreationPolicy(CreationPolicy.Shared)]
    public class UiLoginService : IUiLoginService
    {
        [ImportingConstructor]
        public UiLoginService(IDialogService dialogService, ISystemLogManager logger)
        {
            _dialogService = dialogService;
            _logger = logger;
        }

        public bool Login(ILogonToRepository repository, bool firstAttempt = true)
        {
            ModalBlock.Value.ShowModalBlock();
            var autentificationModel = new AutentificationDialogViewModel(firstAttempt);
            if (_dialogService.PromptUser(Resources.Login, Resources.EnterCredentials,
                        null,
                        autentificationModel,
                        DialogButton.OK,
                        DialogButton.OK, DialogButton.Cancel) == DialogButton.OK)
            {
                var authentificationDetails = new AuthentificationDetails(repository)
                {
                    User = autentificationModel.User.Value,
                    Password = autentificationModel.Password.Value
                };

                if (authentificationDetails.Logon())
                    return true;

                _logger.AddLogEntry(DateTime.Now, TraceEventType.Error, SystemLogSources.System,
                    repository.LastException.Message, repository.LastException.StackTrace);
                return Login(repository, false);
            }

            _logger.AddLogEntry(TraceEventType.Information, SystemLogSources.System, Resources.LoginFailed);
            
            return false;
        }


        /// <summary>
        /// The window modal block
        /// </summary>
        [Import]
        internal Lazy<IModalBlock> ModalBlock;

        private readonly IDialogService _dialogService;
        private readonly ISystemLogManager _logger;
    }
}
