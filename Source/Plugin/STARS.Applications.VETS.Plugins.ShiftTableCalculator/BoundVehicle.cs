using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using STARS.Applications.VETS.Plugins.ShiftTableCalculator.ViewModels.Home;
using STARS.Applications.VETS.Interfaces;

namespace STARS.Applications.VETS.Plugins.ShiftTableCalculator
{
    internal class BoundVehicle
    {

        private readonly HomeViewModel _homeView;
        private readonly IVETSEntityManagerView _entityManager;

        public List<string> Gears;
        public string Class { get; private set; }

        private string engineCapacity;
        public string EngineCapacity
        {
            get { return engineCapacity; }
            set { engineCapacity = value; SaveResource("EngineCapacity", value); }
        }

        private string ratedPower;
        public string RatedPower
        {
            get { return ratedPower; }
            set { ratedPower = value; SaveResource("RatedPower", value); }
        }

        private string kerbMass;
        public string KerbMass
        {
            get { return kerbMass; }
            set { kerbMass = value; SaveResource("KerbMass", value); }
        }

        private string maxSpeed;
        public string MaxSpeed
        {
            get { return maxSpeed; }
            set { maxSpeed = value; SaveResource("MaxSpeed", value); }
        }

        private string ratedEngineSpeed;
        public string RatedEngineSpeed
        {
            get { return ratedEngineSpeed; }
            set { ratedEngineSpeed = value; SaveResource("RatedEngineSpeed", value); }
        }

        private string idlingSpeed;
        public string IdlingSpeed
        {
            get { return idlingSpeed; }
            set { idlingSpeed = value; SaveResource("IdlingSpeed", value); }
        }

        private string numberOfGears;
        public string NumberOfGears
        {
            get { return numberOfGears; }
            set { numberOfGears = value; SaveResource("NumberOfGears", value); }
        }

        private string gear1;
        public string Gear1
        {
            get { return gear1; }
            set { gear1 = value; SaveResource("Gear1", value); }
        }

        private string gear2;
        public string Gear2
        {
            get { return gear2; }
            set { gear2 = value; SaveResource("Gear2", value); }
        }


        private string gear3;
        public string Gear3
        {
            get { return gear3; }
            set { gear3 = value; SaveResource("Gear3", value); }
        }


        private string gear4;
        public string Gear4
        {
            get { return gear4; }
            set { gear4 = value; SaveResource("Gear4", value); }
        }


        private string gear5;
        public string Gear5
        {
            get { return gear5; }
            set { gear5 = value; SaveResource("Gear5", value); }
        }

        private string gear6;
        public string Gear6
        {
            get { return gear6; }
            set { gear6 = value; SaveResource("Gear6", value); }
        }

        public BoundVehicle(HomeViewModel homeView, IVETSEntityManagerView entityManager)
        {
            _homeView = homeView;
            _entityManager = entityManager;
            Gears = new List<string>(new string[6]);
        }

        public BoundVehicle(HomeViewModel homeView, IVETSEntityManagerView entityManager, string engineCapacity, string ratedPower, string kerbMass, string maxSpeed,
                            string ratedEngineSpeed, string idlingSpeed, string numberOfGears, string gear1, string gear2, string gear3, string gear4, string gear5, string gear6)
        {
            _homeView = homeView;
            _entityManager = entityManager;
            this.engineCapacity = engineCapacity;
            this.ratedPower = ratedPower;
            this.kerbMass = kerbMass;
            this.maxSpeed = maxSpeed;
            this.ratedEngineSpeed = ratedEngineSpeed;
            this.idlingSpeed = idlingSpeed;
            this.numberOfGears = numberOfGears;
            this.gear1 = gear1;
            this.gear2 = gear2;
            this.gear3 = gear3;
            this.gear4 = gear4;
            this.gear5 = gear5;
            this.gear6 = gear6;
            Gears = new List<string>(new string[] { gear1, gear2, gear3, gear4, gear5, gear6 });
            SetClass();
        }

        private void SetClass()
        {
            if (!TypeCast.IsDouble(EngineCapacity) || !TypeCast.IsDouble(MaxSpeed))
            {
                Class = String.Empty;
            }
            else
            {
                double engineCapacity = TypeCast.ToDouble(EngineCapacity);
                double maxSpeed = TypeCast.ToDouble(MaxSpeed);

                if (engineCapacity < 150 && maxSpeed < 100) Class = "1";
                else if (maxSpeed < 115) Class = "2.1";
                else if (maxSpeed < 130) Class = "2.2";
                else if (maxSpeed < 140) Class = "3.1";
                else Class = "3.2";
            }
        }

        private void SaveResource(string fieldName, string fieldValue)
        {
            if (_homeView.SelectedVehicle == null) return;

            if (fieldValue != null && fieldValue != _homeView.SelectedVehicle.CustomFieldValues.FirstOrDefault(p => p.CustomFieldID == fieldName).Value)
            {
                _homeView.SelectedVehicle.CustomFieldValues.FirstOrDefault(p => p.CustomFieldID == fieldName).Value = fieldValue;
                _entityManager.Vehicles.Value.Update(_homeView.SelectedVehicle.ID, _homeView.SelectedVehicle.Properties);
                _homeView.PopulateVehicleBoxes();
            }
        }

    }
}
