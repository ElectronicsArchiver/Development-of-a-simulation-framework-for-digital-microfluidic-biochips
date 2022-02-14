﻿namespace MicrofluidSimulator.SimulatorCode.DataTypes
{
    public class Action
    {
        String actionName;
        int actionOnID, actionChange;

        public Action(string actionName, int actionOnID, int actionChange)
        {
            this.actionName = actionName;
            this.actionOnID = actionOnID;
            this.actionChange = actionChange;
        }

        public string ActionName { get => actionName; set => actionName = value; }
        public int ActionOnID { get => actionOnID; set => actionOnID = value; }
        public int ActionChange { get => actionChange; set => actionChange = value; }
    }
}