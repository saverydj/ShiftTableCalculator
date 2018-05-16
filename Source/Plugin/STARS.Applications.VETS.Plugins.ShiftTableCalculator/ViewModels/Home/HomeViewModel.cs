using System.Collections.Generic;
using System.ComponentModel.Composition;
using Caliburn.PresentationFramework;
using STARS.Applications.Interfaces;
using STARS.Applications.Interfaces.Navigation;
using STARS.Applications.Interfaces.ViewModels;
using STARS.Applications.VETS.Interfaces;
using STARS.Applications.VETS.Plugins.ShiftTableCalculator.Properties;
using STARS.Applications.VETS.Plugins.ShiftTableCalculator.Command;
using System.Windows;
using STARS.Applications.VETS.Interfaces.Entities;
using STARS.Applications.Interfaces.EntityManager;
using System.Linq;
using System.ComponentModel;
using System;
using STARS.Applications.Interfaces.EntityProperties.CustomFields;
using STARS.Applications.VETS.Interfaces.Logging;
using STARS.Applications.Interfaces.Dialogs;

namespace STARS.Applications.VETS.Plugins.ShiftTableCalculator.ViewModels.Home
{

    [Export(typeof(HomeViewModel))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    internal class HomeViewModel : PropertyChangedBase, IReadOnlyNameNavigableItem, INotifyPropertyChanged
    {
        private readonly IShellViewModel _shellViewModel;
        private readonly IEntityQuery _entityQuery;
        private readonly IVETSEntityManagerView _entityManager;

        [ImportingConstructor]
        public HomeViewModel(IDialogService dialogService, IImageManager imageManager, IShellViewModel shellViewModel, IEntityQuery entityQuery, IVETSEntityManagerView entityManager, ISystemLogManager logger, ShiftTableCalculator shiftTableCalculator)
        {
            _shellViewModel = shellViewModel;
            _entityQuery = entityQuery;
            _entityManager = entityManager;
            SystemLogService.Logger = logger;
            SystemLogService.DialogService = dialogService;

            CalculateShiftTableCommand = new CalculateShiftTableCommand(this, imageManager, shiftTableCalculator);

            var imagePathPattern = "/STARS.Applications.VETS.Plugins.ShiftTableCalculator;component/Images/{0}.png";
            DisplayInfo = new ExplorerDisplayInfo
            {
                Description = Resources.DisplayName,
                Image16 = string.Format(imagePathPattern, "color_image_16"),
                ExplorerImage16 = string.Format(imagePathPattern, "white_image_16"),
            };
            DisplayName = Resources.DisplayName;
        }

        public void Display()
        {
            Visibilities = new BoundVisibilities();
            BoundVehicleObject = new BoundVehicle(this, _entityManager);
            ShiftNames = new BoundShiftNames();

            SelectedVehicle = null;
            SelectedTrace = null;

            VehicleList = _entityQuery.Where<Vehicle>().ToList().OrderBy(x => x.Name).ToList();
            TraceList = _entityQuery.Where<Trace>().ToList().OrderBy(x => x.Name).ToList();
          
            _shellViewModel.ActivateItem(this);
        }

        new public virtual event PropertyChangedEventHandler PropertyChanged;
        protected virtual void NotifyPropertyChanged(params string[] propertyNames)
        {
            if (PropertyChanged != null)
            {
                foreach (string propertyName in propertyNames) PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                PropertyChanged(this, new PropertyChangedEventArgs("HasError"));
            }
        }

        public void PopulateVehicleBoxes()
        {
            if (SelectedVehicle == null) return;
            try
            {
                BoundVehicleObject = new BoundVehicle
                (
                    this,
                    _entityManager,
                    GetCustomFieldValue("EngineCapacity"),
                    GetCustomFieldValue("RatedPower"),
                    GetCustomFieldValue("KerbMass"),
                    GetCustomFieldValue("MaxSpeed"),
                    GetCustomFieldValue("RatedEngineSpeed"),
                    GetCustomFieldValue("IdlingSpeed"),
                    GetCustomFieldValue("NumberOfGears"),
                    GetCustomFieldValue("Gear1"),
                    GetCustomFieldValue("Gear2"),
                    GetCustomFieldValue("Gear3"),
                    GetCustomFieldValue("Gear4"),
                    GetCustomFieldValue("Gear5"),
                    GetCustomFieldValue("Gear6")
                );
                UpdateVisibilities();
            }
            catch(Exception e)
            {
                SelectedVehicle = null;
                SystemLogService.DisplayErrorInVETSLog(e);
            }           
        }

        private void UpdateVisibilities()
        {
            if (Config.WmtcMode) Visibilities.NormalMode = Visibility.Collapsed;
            else
            {
                Visibilities.NormalMode = Visibility.Visible;
                Visibilities.SetVisibilityOfWmtcClass(String.Empty);
            }

            if (SelectedVehicle != null)
            {
                Visibilities.ShiftInput = Visibility.Visible;
                if (BoundVehicleObject == null) Visibilities.SetVisibilityOfGears(0);
                else
                {
                    Visibilities.SetVisibilityOfGears(TypeCast.ToInt(BoundVehicleObject.NumberOfGears));
                    if (Config.WmtcMode) Visibilities.SetVisibilityOfWmtcClass(BoundVehicleObject.Class);
                }
            }
            Visibilities = new BoundVisibilities(Visibilities);
        }

        private string GetCustomFieldValue(string fieldName)
        {
            CustomFieldValue customField = SelectedVehicle.CustomFieldValues.FirstOrDefault(p => p.CustomFieldID == fieldName);
            if (customField == null)
            {
                Visibilities = new BoundVisibilities();
                SelectedTrace = null;
                ShiftNames = new BoundShiftNames();
                throw new Exception(String.Format(Resources.CustomFeildDoesNotExistInVehicleResourceErrorMessage, fieldName, SelectedVehicle.Name));            
            }
            return customField.Value;
        }

        private void AutoGenerateShiftNames()
        {
            if (SelectedVehicle != null)
            {
                ShiftNames.Part1 = SelectedVehicle.Name + " " + Config.Part1;
                ShiftNames.Part1Reduced = SelectedVehicle.Name + " " + Config.Part1Reduced;
                ShiftNames.Part2 = SelectedVehicle.Name + " " + Config.Part2;
                ShiftNames.Part2Reduced = SelectedVehicle.Name + " " + Config.Part2Reduced;
                ShiftNames.Part3 = SelectedVehicle.Name + " " + Config.Part3;
                ShiftNames.Part3Reduced = SelectedVehicle.Name + " " + Config.Part3Reduced;
                if (SelectedTrace != null) ShiftNames.ShiftName = SelectedVehicle.Name + " " + SelectedTrace.Name;
                ShiftNames = new BoundShiftNames(ShiftNames);
            }
        }

        #region Bound Properties

        public DisplayInfo DisplayInfo { get; private set; }
        public string DisplayName { get; private set; }
        public ICommandViewModel CalculateShiftTableCommand { get; private set; }
        public List<Vehicle> VehicleList { get; private set; }
        public List<Trace> TraceList { get; private set; }

        public bool WmtcMode
        {
            get { return Config.WmtcMode; }
            private set { Config.WmtcMode = value; NotifyPropertyChanged("WmtcMode"); UpdateVisibilities(); }
        }

        private BoundVisibilities visibilities;
        public BoundVisibilities Visibilities
        {
            get { return visibilities; }
            private set { visibilities = value; NotifyPropertyChanged("Visibilities"); }
        }

        private BoundShiftNames shiftNames;
        public BoundShiftNames ShiftNames
        {
            get { return shiftNames; }
            set { shiftNames = value; NotifyPropertyChanged("ShiftNames"); }
        }

        private BoundVehicle boundVehicleObject;
        public BoundVehicle BoundVehicleObject
        {
            get { return boundVehicleObject; }
            set { boundVehicleObject = value; NotifyPropertyChanged("BoundVehicleObject"); }
        }

        private Vehicle selectedVehicle;
        public Vehicle SelectedVehicle
        {
            get { return selectedVehicle; }
            set { selectedVehicle = value; NotifyPropertyChanged("SelectedVehicle"); PopulateVehicleBoxes(); AutoGenerateShiftNames(); }
        }

        private Trace selectedTrace;
        public Trace SelectedTrace
        {
            get { return selectedTrace; }
            set { selectedTrace = value; NotifyPropertyChanged("SelectedTrace"); AutoGenerateShiftNames(); }
        }

        #endregion

    }
}
