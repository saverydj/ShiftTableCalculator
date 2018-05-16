using System.Windows.Input;
using STARS.Applications.Interfaces;
using STARS.Applications.Interfaces.ViewModels;
using STARS.Applications.UI.Common;
using STARS.Applications.VETS.Interfaces;
using System;
using STARS.Applications.VETS.Plugins.ShiftTableCalculator.ViewModels.Home;

namespace STARS.Applications.VETS.Plugins.ShiftTableCalculator.Command
{
    /// <summary>
    /// Implementation of StartTest button
    /// </summary>
    internal class CalculateShiftTableCommand : ICommandViewModel
    { 

        public DisplayInfo DisplayInfo { get; private set; }       
        public ICommand Command { get; private set; }
        public string DisplayName { get { return Properties.Resources.CalculateShiftTable; } }

        public CalculateShiftTableCommand(HomeViewModel homeViewModel, IImageManager imageManager, ShiftTableCalculator shiftTableCalculator)
        {
            Command = new RelayCommand(_ => DoCalculateShiftTable(homeViewModel, shiftTableCalculator),               
                                       _ => (homeViewModel.SelectedVehicle != null && (homeViewModel.SelectedTrace != null || homeViewModel.WmtcMode)));
            DisplayInfo = new DisplayInfo
            {
                Description = Properties.Resources.CalculateShiftTable,
                Image16 = imageManager.GetImage16Path("Import")
            };
        }

        private void DoCalculateShiftTable(HomeViewModel homeViewModel, ShiftTableCalculator shiftTableCalculator)
        {
            try
            {
                shiftTableCalculator.PerformCalculation(homeViewModel);
            }
            catch(Exception e)
            {
                shiftTableCalculator.IsPerformingCalculation = false;
                throw e;
            }

            
        }

    }
}
