﻿using System.Collections;
using MicrofluidSimulator.SimulatorCode.DataTypes;
using System.Linq;
using System.Text.Json;
using System.Numerics;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using MicrofluidSimulator.SimulatorCode.Models;
using MicrofluidSimulator.SimulatorCode.DataTypes.JsonDataTypes;
using MicrofluidSimulator.SimulatorCode.DataTypes;

namespace MicrofluidSimulator.SimulatorCode.Initialize
{

    public class Initialize
    {

        public Initialize()
        {

        }

        public Container initialize(JsonContainer jsonContainer, ElectrodesWithNeighbours[] electrodesWithNeighbours)
        {

            //Electrodes[] electrodes = new Electrodes[jsonContainer.electrodes.Count];
           
            Electrodes[] electrodeBoard = initializeBoard(jsonContainer.electrodes, electrodesWithNeighbours);



            ArrayList droplets = initializeDroplets(jsonContainer.droplets);
            Console.WriteLine("actuatorname: " + jsonContainer.actuators[0].name);
            DataTypes.Actuators[] actuatorsInitial = initializeActuators(jsonContainer.actuators);
            DataTypes.Sensors[] sensorsInitial = initializeSensors(jsonContainer.sensors, electrodeBoard);
            Information information = initializeInformation(jsonContainer.information);
            Container container = new Container(electrodeBoard, droplets, actuatorsInitial, sensorsInitial, information, 0);
            foreach(Droplets droplet in droplets)
            {
                container.SubscribedDroplets.Add(droplet.ID1);
            }
            foreach (DataTypes.Actuators actuators in actuatorsInitial)
            {
                container.SubscribedActuators.Add(actuators.ID1);
            }
            if (electrodesWithNeighbours == null)
            {
                NeighbourFinder neighbourFinder = new NeighbourFinder();
                NeighbourFinder.findNeighbours(container);
            }

            initializeSubscriptions(container);


            return container;
        }

        private Information initializeInformation(DataTypes.JsonDataTypes.Information jsonInformation)
        {
            Information information = new Information(jsonInformation.platform_name, jsonInformation.platform_type, jsonInformation.platform_ID, jsonInformation.sizeX, jsonInformation.sizeY);
            return information;
        }

        private DataTypes.Sensors[] initializeSensors(List<DataTypes.JsonDataTypes.Sensors> sensors, Electrodes[] electrodeBoard)
        {
            DataTypes.Sensors[] sensorsInitial = new DataTypes.Sensors[sensors.Count];

            for (int i = 0; i < sensors.Count; i++)
            {
                switch (sensors[i].type)
                {
                    case "RGB_color":



                        sensorsInitial[i] = new ColorSensor(sensors[i].name, sensors[i].ID, sensors[i].sensorID, sensors[i].type, sensors[i].positionX, sensors[i].positionY,
                            sensors[i].sizeX, sensors[i].sizeY, sensors[i].valueRed, sensors[i].valueGreen, sensors[i].valueBlue, HelpfullRetreiveFunctions.getIDofElectrodeByPosition(sensors[i].positionX, sensors[i].positionY, electrodeBoard));

                        break;
                    case "temperature":

                        sensorsInitial[i] = new TemperatureSensor(sensors[i].name, sensors[i].ID, sensors[i].sensorID, sensors[i].type, sensors[i].positionX, sensors[i].positionY,
                            sensors[i].sizeX, sensors[i].sizeY, sensors[i].valueTemperature, HelpfullRetreiveFunctions.getIDofElectrodeByPosition(sensors[i].positionX, sensors[i].positionY, electrodeBoard));

                        break;

                }

            }
            return sensorsInitial;
        }

        private DataTypes.Actuators[] initializeActuators(List<DataTypes.JsonDataTypes.Actuators> actuators)
        {
            //List<MicrofluidSimulator.SimulatorCode.DataTypes.Actuators> actuatorsInitial = new List<MicrofluidSimulator.SimulatorCode.DataTypes.Actuators>();
            DataTypes.Actuators[] actuatorsInitial = new DataTypes.Actuators[actuators.Count];
            
            for(int i = 0; i < actuators.Count; i++)
            {
                switch (actuators[i].type)
                {
                    case "heater":
                        
                        

                        actuatorsInitial[i]= (new Heater(actuators[i].name, actuators[i].ID, actuators[i].actuatorID, actuators[i].type, actuators[i].positionX,
                            actuators[i].positionY, actuators[i].sizeX, actuators[i].sizeY, actuators[i].valueActualTemperature, actuators[i].valueDesiredTemperature,
                            actuators[i].valuePowerStatus));

                        break;
                    
                }
                
            }

            return actuatorsInitial;

        }

        public class Corner
        {
            List<int> Coords { get; set; }
        }

        private Electrodes[] initializeBoard(List<Electrode> electrodes, ElectrodesWithNeighbours[] electrodesWithNeighbours)
        {


            Electrodes[] electrodeBoard = new Electrodes[electrodes.Count];
            for (int i = 0; i < electrodes.Count; i++)
            {
                //electrodeBoard[i] = new Electrodes("arrel", i, i, i, 0, (i % 32) * 20, (i / 32) * 20, 20, 20, 0, null);






                int[,] cornersGetter = new int[electrodes[i].corners.Count, 2];
                for (int j = 0; j < electrodes[i].corners.Count; j++)
                {

                    var res = System.Text.Json.JsonSerializer.Deserialize<List<int>>(electrodes[i].corners[j].ToString());
                    for (int k = 0; k < 2; k++)
                    {
                        cornersGetter[j, k] = res[k];
                    }
                }





                electrodeBoard[i] = new Electrodes(electrodes[i].name, electrodes[i].ID, electrodes[i].electrodeID, electrodes[i].driverID, electrodes[i].shape,
                electrodes[i].positionX, electrodes[i].positionY, electrodes[i].sizeX, electrodes[i].sizeY, electrodes[i].status, cornersGetter);

                if(electrodesWithNeighbours != null)
                {
                    
                    for(int j = 0; j < electrodesWithNeighbours[i].Neighbours.Count; j++)
                    {
                        electrodeBoard[i].Neighbours.Add(electrodesWithNeighbours[i].Neighbours[j]);
                    }
                    
                    //int[] neighboursGetter = new int[electrodesWithNeighbours[i].Neighbours.Count];
                    //for (int j = 0; j < electrodesWithNeighbours[i].Neighbours.Count; j++)
                    //{

                    //    var res = System.Text.Json.JsonSerializer.Deserialize<List<int>>(electrodesWithNeighbours[i].Neighbours[j].ToString());

                    //    for(int k = 0; k < res.Count; k++)
                    //    {
                    //        neighboursGetter[k] = res[k];
                    //    }

                    //    electrodeBoard[i].Neighbours.Add(neighboursGetter[j]);

                    //}

                }
                





            }

            return electrodeBoard;


        }

        private ArrayList initializeDroplets(List<MicrofluidSimulator.SimulatorCode.DataTypes.JsonDataTypes.Droplets> droplets)
        {
            ArrayList dropletsArray = new ArrayList();
            int i = 0;
            foreach (MicrofluidSimulator.SimulatorCode.DataTypes.JsonDataTypes.Droplets droplet in droplets)
            {
                dropletsArray.Add(new Droplets(droplet.name, droplet.ID, droplet.substance_name, droplet.positionX, droplet.positionY, droplet.sizeX, droplet.sizeY, droplet.color, droplet.temperature, DropletModels.getVolumeOfDroplet(droplet.sizeX, 1), droplet.electrodeID, i));
                i++;
            }
            //    droplets.Add(new Droplets("test droplet", 0, "h20", 120, 10, 15, 15, "blue", 20, DropletModels.getVolumeOfDroplet(15, 1), 0));
            //((Droplets)droplets[0]).ElectrodeID = 1;

            //droplets.Add(new Droplets("test droplet", 1, "h20", 30, 10, 15, 15, "blue", 20));
            //((Droplets)droplets[0]).ElectrodeID = 1;

            //droplets.Add(new Droplets("test droplet", 2, "h20", 50, 10, 15, 15, "blue", 20));
            //((Droplets)droplets[0]).ElectrodeID = 2;

            //droplets.Add(new Droplets("test droplet2", 3, "h20", 160, 70, 15, 15, "yellow", 20, DropletModels.getVolumeOfDroplet(15, 1), 1));
            //((Droplets) droplets[1]).ElectrodeID = 99;
            return dropletsArray;
        }

        

        private void initializeSubscriptions(Container container)
        {
            ArrayList droplets = container.Droplets;
            foreach (Droplets droplet in droplets)
            {
                Models.SubscriptionModels.dropletSubscriptions(container, droplet);
                foreach(int sub in droplet.Subscriptions)
                {
                    Console.WriteLine("droplet subs id: " + droplet.ID1 + " subs: " + sub);
                }
                
            }


        }

       

    }
}
