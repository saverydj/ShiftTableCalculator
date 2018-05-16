using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace STARS.Applications.VETS.Plugins.ShiftTableCalculator
{
    public class BoundVisibilities
    {
        public Visibility ShiftInput { get; set; }
        public Visibility NormalMode { get; set; }
        public Visibility Part1 { get; set; }
        public Visibility Part1Reduced { get; set; }
        public Visibility Part2 { get; set; }
        public Visibility Part2Reduced { get; set; }
        public Visibility Part3 { get; set; }
        public Visibility Part3Reduced { get; set; }
        public List<Visibility> VisibleGears { get; set; }

        public BoundVisibilities()
        {
            ShiftInput = Visibility.Collapsed;
            if (Config.WmtcMode) NormalMode = Visibility.Collapsed;
            else NormalMode = Visibility.Visible;
            Part1 = Visibility.Collapsed;
            Part1Reduced = Visibility.Collapsed;
            Part2 = Visibility.Collapsed;
            Part2Reduced = Visibility.Collapsed;
            Part3 = Visibility.Collapsed;
            Part3Reduced = Visibility.Collapsed;
            VisibleGears = new List<Visibility>(new Visibility[6]);
            SetVisibilityOfGears(0);
        }

        public BoundVisibilities(BoundVisibilities boundVisibilities)
        {
            ShiftInput = boundVisibilities.ShiftInput;
            NormalMode = boundVisibilities.NormalMode;          
            Part1 = boundVisibilities.Part1;
            Part1Reduced = boundVisibilities.Part1Reduced;
            Part2 = boundVisibilities.Part2;
            Part2Reduced = boundVisibilities.Part2Reduced;
            Part3 = boundVisibilities.Part3;
            Part3Reduced = boundVisibilities.Part3Reduced;
            VisibleGears = boundVisibilities.VisibleGears;
        }

        public void SetVisibilityOfGears(int numberOfGears)
        {
            if (numberOfGears < 0) numberOfGears = 0;
            if (numberOfGears > 6) numberOfGears = 6;
            for (int i = 0; i < VisibleGears.Count; i++)
            {
                if (i + 1 <= numberOfGears) VisibleGears[i] = Visibility.Visible;
                else VisibleGears[i] = Visibility.Collapsed;
            }
        }

        public void SetVisibilityOfWmtcClass(string vehicleClass)
        {
            if (vehicleClass == "1")
            {
                Part1 = Visibility.Collapsed;
                Part1Reduced = Visibility.Visible;
                Part2 = Visibility.Collapsed;
                Part2Reduced = Visibility.Collapsed;
                Part3 = Visibility.Collapsed;
                Part3Reduced = Visibility.Collapsed;
            }
            else if (vehicleClass == "2.1")
            {
                Part1 = Visibility.Collapsed;
                Part1Reduced = Visibility.Visible;
                Part2 = Visibility.Collapsed;
                Part2Reduced = Visibility.Visible;
                Part3 = Visibility.Collapsed;
                Part3Reduced = Visibility.Collapsed;
            }
            else if (vehicleClass == "2.2")
            {
                Part1 = Visibility.Visible;
                Part1Reduced = Visibility.Collapsed;
                Part2 = Visibility.Visible;
                Part2Reduced = Visibility.Collapsed;
                Part3 = Visibility.Collapsed;
                Part3Reduced = Visibility.Collapsed;
            }
            else if (vehicleClass == "3.1")
            {
                Part1 = Visibility.Visible;
                Part1Reduced = Visibility.Collapsed;
                Part2 = Visibility.Visible;
                Part2Reduced = Visibility.Collapsed;
                Part3 = Visibility.Collapsed;
                Part3Reduced = Visibility.Visible;

            }
            else if (vehicleClass == "3.2")
            {
                Part1 = Visibility.Visible;
                Part1Reduced = Visibility.Collapsed;
                Part2 = Visibility.Visible;
                Part2Reduced = Visibility.Collapsed;
                Part3 = Visibility.Visible;
                Part3Reduced = Visibility.Collapsed;
            }
            else
            {
                Part1 = Visibility.Collapsed;
                Part1Reduced = Visibility.Collapsed;
                Part2 = Visibility.Collapsed;
                Part2Reduced = Visibility.Collapsed;
                Part3 = Visibility.Collapsed;
                Part3Reduced = Visibility.Collapsed;
            }
        }

    }
}
