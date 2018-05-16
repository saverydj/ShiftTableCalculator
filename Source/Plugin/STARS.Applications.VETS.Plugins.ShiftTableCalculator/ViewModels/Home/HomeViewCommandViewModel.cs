using System.ComponentModel.Composition;
using System.Windows.Input;
using Caliburn.PresentationFramework.Views;
using STARS.Applications.Interfaces;
using STARS.Applications.Interfaces.Constants;
using STARS.Applications.Interfaces.ViewModels;
using STARS.Applications.UI.Common;
using STARS.Applications.VETS.Interfaces;
using STARS.Applications.VETS.Interfaces.Constants;
using STARS.Applications.VETS.Interfaces.ViewModels.Attributes;
using STARS.Applications.VETS.UI.Views.Commands.CommandBaseViews;

namespace STARS.Applications.VETS.Plugins.ShiftTableCalculator.ViewModels.Home
{
    [View(typeof(Explorer), Context = "Explorer")]
    [Command(CommandCategories.Utilities, "ShiftTableCalculator", Priority = Priorities.Fourth),
     PartCreationPolicy(CreationPolicy.Shared)]
    class HomeViewCommandViewModel : ICommandViewModel
    {
        #region Construction

        /// <summary>
        /// Default constructor
        /// </summary>
        [ImportingConstructor]
        public HomeViewCommandViewModel([Import(typeof(HomeViewModel))]HomeViewModel model, IImageManager imageManager)
        {
            DisplayName = Properties.Resources.DisplayName;
            DisplayInfo = new ExplorerDisplayInfo
            {
                Description = Properties.Resources.DisplayName,
                Image16 = "/STARS.Applications.VETS.Plugins.ShiftTableCalculator;component/Images/color_image_16.png",
                ExplorerImage16 = "/STARS.Applications.VETS.Plugins.ShiftTableCalculator;component/Images/white_image_16.png"
            };

            Command = new RelayCommand(p => model.Display());
        }

        public DisplayInfo DisplayInfo { get; private set; }
        public string DisplayName { get; private set; }
        public ICommand Command { get; private set; }

        #endregion

    }
}
