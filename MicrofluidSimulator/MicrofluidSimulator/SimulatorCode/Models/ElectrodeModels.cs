﻿using System.Collections;
using MicrofluidSimulator.SimulatorCode.DataTypes;
namespace MicrofluidSimulator.SimulatorCode.Models

{
    public class ElectrodeModels
    {
        public static ArrayList electrodeOnOff(Container values, Electrodes electrode, DataTypes.SimulatorAction action)
        {
            electrode.Status = action.ActionChange;
            return electrode.Subscriptions;
        }
    }
}