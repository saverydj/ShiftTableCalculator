using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace STARS.Applications.VETS.Plugins.ShiftTableCalculator
{
    public class BoundShiftNames
    {
        public string ShiftName { get; set; }
        public string Part1 { get; set; }
        public string Part1Reduced { get; set; }
        public string Part2 { get; set; }
        public string Part2Reduced { get; set; }
        public string Part3 { get; set; }
        public string Part3Reduced { get; set; }

        public BoundShiftNames()
        {
            ShiftName = null;
            Part1 = null;
            Part1Reduced = null;
            Part2 = null;
            Part2Reduced = null;
            Part3 = null;
            Part3Reduced = null;
        }    

        public BoundShiftNames(BoundShiftNames boundShiftNames)
        {
            ShiftName = boundShiftNames.ShiftName;
            Part1 = boundShiftNames.Part1;
            Part1Reduced = boundShiftNames.Part1Reduced;
            Part2 = boundShiftNames.Part2;
            Part2Reduced = boundShiftNames.Part2Reduced;
            Part3 = boundShiftNames.Part3;
            Part3Reduced = boundShiftNames.Part3Reduced;
        }
        
        public BoundShiftNames(string shiftName, string Part1, string Part1Reduced, string Part2, string Part2Reduced, string Part3, string Part3Reduced)
        {
            ShiftName = shiftName;
            Part1 = Part1;
            Part1Reduced = Part1Reduced;
            Part2 = Part2;
            Part2Reduced = Part2Reduced;
            Part3 = Part3;
            Part3Reduced = Part3Reduced;
        }
    }
}
