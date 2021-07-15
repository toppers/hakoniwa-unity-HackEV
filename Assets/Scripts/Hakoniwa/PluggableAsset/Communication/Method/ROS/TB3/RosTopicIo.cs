using Hakoniwa.PluggableAsset.Communication.Pdu.ROS;
using Hakoniwa.PluggableAsset.Communication.Method.ROS;
using Hakoniwa.PluggableAsset.Communication.Pdu;
using System.Collections;
using System.Collections.Generic;
using Unity.Robotics.ROSTCPConnector;
using UnityEngine;
using System;
using RosMessageTypes.Ev3;
using Unity.Robotics.ROSTCPConnector.MessageGeneration;
using RosMessageTypes.Geometry;
using RosMessageTypes.Sensor;
using Hakoniwa.PluggableAsset.Communication.Pdu.ROS.TB3;
using System.IO;
using Newtonsoft.Json;
using Hakoniwa.Core.Utils.Logger;

namespace Hakoniwa.PluggableAsset.Communication.Method.ROS.TB3
{
    [System.Serializable]
    public class UnityRosParameter
    {
        public bool connection_startup;
        public string ip_address;
        public int portno;
        public bool show_hud;
        public int keep_alive_time;
        public int network_timeout;
        public float sleep_time;
    }
    public class RosTopicIo : IRosTopicIo
    {
        private ROSConnection ros;
        private Dictionary<string, Message> topic_data_table = new Dictionary<string, Message>();
        private UnityRosParameter parameters;
        private void LoadParameters(string filepath)
        {
            ros = ROSConnection.instance;
            if (filepath == null)
            {
                return;
            }
            string jsonString = File.ReadAllText(filepath);
            parameters = JsonConvert.DeserializeObject<UnityRosParameter>(jsonString);
            ros.ShowHud = parameters.show_hud;
            ros.RosIPAddress = parameters.ip_address;
            ros.RosPort = parameters.portno;
            ros.ConnectOnStart = parameters.connection_startup;
            ros.KeepaliveTime = parameters.keep_alive_time;
            ros.NetworkTimeoutSeconds = parameters.network_timeout;
            ros.SleepTimeSeconds = parameters.sleep_time;
            SimpleLogger.Get().Log(Level.INFO, "ShowHud=" + ros.ShowHud);
            SimpleLogger.Get().Log(Level.INFO, "RosIPAddress=" + ros.RosIPAddress);
            SimpleLogger.Get().Log(Level.INFO, "RosPort=" + ros.RosPort);
            SimpleLogger.Get().Log(Level.INFO, "ConnectOnStart=" + ros.ConnectOnStart);
            SimpleLogger.Get().Log(Level.INFO, "KeepaliveTime=" + ros.KeepaliveTime);
            SimpleLogger.Get().Log(Level.INFO, "NetworkTimeoutSeconds=" + ros.NetworkTimeoutSeconds);
            SimpleLogger.Get().Log(Level.INFO, "SleepTimeSeconds=" + ros.SleepTimeSeconds);
        }

        public RosTopicIo()
        {
            LoadParameters(AssetConfigLoader.core_config.ros_topic_method.parameters);

            foreach (var e in AssetConfigLoader.core_config.ros_topics)
            {
                topic_data_table[e.topic_message_name] = null;
            }


            ros.Subscribe<MLaserScan>("scan", MLaserScanChange);
            ros.Subscribe<MImu>("imu", MImuChange);
            ros.Subscribe<MTwist>("cmd_vel", MTwistChange);

        }


        private void MLaserScanChange(MLaserScan obj)
        {
            this.topic_data_table["scan"] = obj;
        }
        private void MImuChange(MImu obj)
        {
            this.topic_data_table["imu"] = obj;
        }
        private void MTwistChange(MTwist obj)
        {
            this.topic_data_table["cmd_vel"] = obj;
        }

        public void Publish(IPduCommTypedData data)
        {
            RosTopicPduCommTypedData typed_data = data as RosTopicPduCommTypedData;
            ros.Send(typed_data.GetDataName(), typed_data.GetTopicData());
        }

        public IPduCommTypedData Recv(string topic_name)
        {
            var cfg = AssetConfigLoader.GetRosTopic(topic_name);
            if (cfg == null)
            {
                throw new System.NotImplementedException();
            }
            else if (this.topic_data_table[topic_name] == null)
            {
                return null;
            }
            return new RosTopicPduCommTypedData(topic_name, cfg.topic_type_name, this.topic_data_table[topic_name]);
        }
    }

}