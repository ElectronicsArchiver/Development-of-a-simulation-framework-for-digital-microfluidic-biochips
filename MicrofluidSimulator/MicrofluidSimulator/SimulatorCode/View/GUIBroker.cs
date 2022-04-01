﻿/*
 * Written by Joel A. V. Madsen
 */
using Microsoft.JSInterop;
using System.Text.Json;
using System.Diagnostics;
using MicrofluidSimulator.SimulatorCode.DataTypes;

//inject IJSRuntime JsRuntime;

namespace MicrofluidSimulator.SimulatorCode.View
{
    public class GUIInfo {
        public bool gui_status { get; set; }
        public Droplets[] droplets { get; set; }
        public Electrode[] electrodes { get; set;}
    }

    public class GUIBroker {


        /*
         * JSRuntime is used to run javascript code within c#
         */
        private IJSInProcessRuntime _JSInProcessRuntime;
        private IJSUnmarshalledRuntime _JSUnmarshalledRuntime;

        public GUIBroker() { }

        public void set_jsprocess(IJSInProcessRuntime JSInProcessRuntime)
        {
            _JSInProcessRuntime = JSInProcessRuntime;
        }

        public void set_unmarshall(IJSUnmarshalledRuntime JSUnmarshalledRuntime)
        {
            _JSUnmarshalledRuntime = JSUnmarshalledRuntime;
        }

        public void initialize_board(Information container) {
            /*var stopwatch = new Stopwatch();
            stopwatch.Start();
            byte[] bytes = MessagePackSerializer.Serialize(container);
            stopwatch.Stop();

            Console.WriteLine("Initialize serialize time: " + stopwatch.ElapsedMilliseconds + " ms");

            stopwatch.Reset();
            stopwatch.Start();
            string json = JsonSerializer.Serialize(container);
            stopwatch.Stop();

            Console.WriteLine("Newtonsoft serialize time: " + stopwatch.ElapsedMilliseconds + " ms");*/

            var json_string = Utf8Json.JsonSerializer.ToJsonString(container);
            _JSInProcessRuntime.InvokeVoid("initialize_board", json_string);
        }

        public void update_board(Container container) {
            //byte[] bytes = MessagePackSerializer.Serialize(container);
            //MessagePackSerializer.ConvertToJson(container);
            /*var stopwatch = new Stopwatch();
            stopwatch.Start();
            //byte[] bytes = MessagePackSerializer.Serialize(container, MessagePack.Resolvers.ContractlessStandardResolver.Options);
            //string json = MessagePackSerializer.ConvertToJson(bytes);
            string json = JsonSerializer.Serialize(container);
            stopwatch.Stop();
            Console.WriteLine("System.Text.Json serialize time: " + stopwatch.ElapsedMilliseconds + " ms");

            stopwatch.Reset();
            stopwatch.Start();
            string json2 = Utf8Json.JsonSerializer.ToJsonString(container);
            stopwatch.Stop();

            Console.WriteLine("utf8 serialize time: " + stopwatch.ElapsedMilliseconds + " ms");*/

            _JSUnmarshalledRuntime.InvokeUnmarshalled<string, string>("update_board", Utf8Json.JsonSerializer.ToJsonString(container));
            //_JSInProcessRuntime.InvokeVoid("update_board", container);
        }

        public bool get_gui_status() {
            var result = _JSInProcessRuntime.Invoke<bool>("get_gui_status");
            return result;
        }

        public void change_play_status() {
            _JSInProcessRuntime.InvokeVoid("change_play_status");
        }

        public void restart_board()
        {
            _JSInProcessRuntime.InvokeVoid("restart_board");
        }

        public string get_selected_element()
        {
            var result = _JSInProcessRuntime.Invoke<string>("get_selected_element");
            return result;
        }


        // Used for debugging
        public void start_update_timer()
        {
            _JSInProcessRuntime.InvokeVoid("start_update_timer");
        }
        public void end_update_timer()
        {
            _JSInProcessRuntime.InvokeVoid("end_update_timer");
        }
    }
}
