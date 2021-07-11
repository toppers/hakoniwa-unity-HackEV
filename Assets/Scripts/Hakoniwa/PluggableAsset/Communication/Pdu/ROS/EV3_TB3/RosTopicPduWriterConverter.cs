using Hakoniwa.PluggableAsset.Communication.Pdu;
using Hakoniwa.PluggableAsset.Communication.Pdu.ROS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RosMessageTypes.Std;
using RosMessageTypes.Ev3;
using Unity.Robotics.ROSTCPConnector.MessageGeneration;
using RosMessageTypes.Geometry;
using RosMessageTypes.Sensor;
using Hakoniwa.PluggableAsset.Communication.Pdu.ROS.EV3_TB3;

namespace Hakoniwa.PluggableAsset.Communication.Pdu.ROS.EV3_TB3
{
    class RosTopicPduWriterConverter : IPduWriterConverter
    {
        public IPduCommData ConvertToIoData(IPduWriter src)
        {
            RosTopicPduWriter pdu_writer = src as RosTopicPduWriter;
            return new RosTopicPduCommTypedData(pdu_writer);
        }
        

        static private void ConvertToMessage(IPduReadOperation src, MEv3PduActuator dst)
        {
            ConvertToMessage(src.Ref("header").GetPduReadOps(), dst.header);
			dst.leds = src.GetDataUInt8Array("leds");
            foreach (var e in src.Refs("motors"))
            {
                int index = Array.IndexOf(src.Refs("motors"), e);
                if (dst.motors[index] == null) {
                    dst.motors[index] = new MEv3PduMotor();
                }
                ConvertToMessage(e.GetPduReadOps(), dst.motors[index]);
            }
			dst.gyro_reset = src.GetDataUInt32("gyro_reset");
        }
        static private void ConvertToMessage(IPduReadOperation src, MEv3PduActuatorHeader dst)
        {
			dst.name = src.GetDataString("name");
			dst.version = src.GetDataUInt32("version");
			dst.asset_time = src.GetDataInt64("asset_time");
			dst.ext_off = src.GetDataUInt32("ext_off");
			dst.ext_size = src.GetDataUInt32("ext_size");
        }
        static private void ConvertToMessage(IPduReadOperation src, MEv3PduColorSensor dst)
        {
			dst.color = src.GetDataUInt32("color");
			dst.reflect = src.GetDataUInt32("reflect");
			dst.rgb_r = src.GetDataUInt32("rgb_r");
			dst.rgb_g = src.GetDataUInt32("rgb_g");
			dst.rgb_b = src.GetDataUInt32("rgb_b");
        }
        static private void ConvertToMessage(IPduReadOperation src, MEv3PduMotor dst)
        {
			dst.power = src.GetDataInt32("power");
			dst.stop = src.GetDataUInt32("stop");
			dst.reset_angle = src.GetDataUInt32("reset_angle");
        }
        static private void ConvertToMessage(IPduReadOperation src, MEv3PduSensor dst)
        {
            ConvertToMessage(src.Ref("header").GetPduReadOps(), dst.header);
			dst.buttons = src.GetDataUInt8Array("buttons");
            foreach (var e in src.Refs("color_sensors"))
            {
                int index = Array.IndexOf(src.Refs("color_sensors"), e);
                if (dst.color_sensors[index] == null) {
                    dst.color_sensors[index] = new MEv3PduColorSensor();
                }
                ConvertToMessage(e.GetPduReadOps(), dst.color_sensors[index]);
            }
            foreach (var e in src.Refs("touch_sensors"))
            {
                int index = Array.IndexOf(src.Refs("touch_sensors"), e);
                if (dst.touch_sensors[index] == null) {
                    dst.touch_sensors[index] = new MEv3PduTouchSensor();
                }
                ConvertToMessage(e.GetPduReadOps(), dst.touch_sensors[index]);
            }
			dst.motor_angle = src.GetDataUInt32Array("motor_angle");
			dst.gyro_degree = src.GetDataInt32("gyro_degree");
			dst.gyro_degree_rate = src.GetDataInt32("gyro_degree_rate");
			dst.sensor_ultrasonic = src.GetDataUInt32("sensor_ultrasonic");
			dst.gps_lat = src.GetDataFloat64("gps_lat");
			dst.gps_lon = src.GetDataFloat64("gps_lon");
        }
        static private void ConvertToMessage(IPduReadOperation src, MEv3PduSensorHeader dst)
        {
			dst.name = src.GetDataString("name");
			dst.version = src.GetDataUInt32("version");
			dst.hakoniwa_time = src.GetDataInt64("hakoniwa_time");
			dst.ext_off = src.GetDataUInt32("ext_off");
			dst.ext_size = src.GetDataUInt32("ext_size");
        }
        static private void ConvertToMessage(IPduReadOperation src, MEv3PduTouchSensor dst)
        {
			dst.value = src.GetDataUInt32("value");
        }
        static private void ConvertToMessage(IPduReadOperation src, MHeader dst)
        {
			dst.seq = src.GetDataUInt32("seq");
            ConvertToMessage(src.Ref("stamp").GetPduReadOps(), dst.stamp);
			dst.frame_id = src.GetDataString("frame_id");
        }
        static private void ConvertToMessage(IPduReadOperation src, MImu dst)
        {
            ConvertToMessage(src.Ref("header").GetPduReadOps(), dst.header);
            ConvertToMessage(src.Ref("orientation").GetPduReadOps(), dst.orientation);
			dst.orientation_covariance = src.GetDataFloat64Array("orientation_covariance");
            ConvertToMessage(src.Ref("angular_velocity").GetPduReadOps(), dst.angular_velocity);
			dst.angular_velocity_covariance = src.GetDataFloat64Array("angular_velocity_covariance");
            ConvertToMessage(src.Ref("linear_acceleration").GetPduReadOps(), dst.linear_acceleration);
			dst.linear_acceleration_covariance = src.GetDataFloat64Array("linear_acceleration_covariance");
        }
        static private void ConvertToMessage(IPduReadOperation src, MLaserScan dst)
        {
            ConvertToMessage(src.Ref("header").GetPduReadOps(), dst.header);
			dst.angle_min = src.GetDataFloat32("angle_min");
			dst.angle_max = src.GetDataFloat32("angle_max");
			dst.angle_increment = src.GetDataFloat32("angle_increment");
			dst.time_increment = src.GetDataFloat32("time_increment");
			dst.scan_time = src.GetDataFloat32("scan_time");
			dst.range_min = src.GetDataFloat32("range_min");
			dst.range_max = src.GetDataFloat32("range_max");
			dst.ranges = src.GetDataFloat32Array("ranges");
			dst.intensities = src.GetDataFloat32Array("intensities");
        }
        static private void ConvertToMessage(IPduReadOperation src, MQuaternion dst)
        {
			dst.x = src.GetDataFloat64("x");
			dst.y = src.GetDataFloat64("y");
			dst.z = src.GetDataFloat64("z");
			dst.w = src.GetDataFloat64("w");
        }
        static private void ConvertToMessage(IPduReadOperation src, MTime dst)
        {
			dst.secs = src.GetDataUInt32("secs");
			dst.nsecs = src.GetDataUInt32("nsecs");
        }
        static private void ConvertToMessage(IPduReadOperation src, MTwist dst)
        {
            ConvertToMessage(src.Ref("linear").GetPduReadOps(), dst.linear);
            ConvertToMessage(src.Ref("angular").GetPduReadOps(), dst.angular);
        }
        static private void ConvertToMessage(IPduReadOperation src, MVector3 dst)
        {
			dst.x = src.GetDataFloat64("x");
			dst.y = src.GetDataFloat64("y");
			dst.z = src.GetDataFloat64("z");
        }
        
        
        static public Message ConvertToMessage(IPduReadOperation src, string type)
        {

            if (type.Equals("Ev3PduSensor"))
            {
            	MEv3PduSensor ros_topic = new MEv3PduSensor();
                ConvertToMessage(src, ros_topic);
                return ros_topic;
            }
            if (type.Equals("Ev3PduActuator"))
            {
            	MEv3PduActuator ros_topic = new MEv3PduActuator();
                ConvertToMessage(src, ros_topic);
                return ros_topic;
            }
            if (type.Equals("LaserScan"))
            {
            	MLaserScan ros_topic = new MLaserScan();
                ConvertToMessage(src, ros_topic);
                return ros_topic;
            }
            if (type.Equals("Imu"))
            {
            	MImu ros_topic = new MImu();
                ConvertToMessage(src, ros_topic);
                return ros_topic;
            }
            if (type.Equals("Twist"))
            {
            	MTwist ros_topic = new MTwist();
                ConvertToMessage(src, ros_topic);
                return ros_topic;
            }
            throw new InvalidCastException("Can not find ros message type:" + type);
        }
        
        static public Message ConvertToMessage(RosTopicPduWriter pdu_writer)
        {
            return ConvertToMessage(pdu_writer.GetReadOps(), pdu_writer.GetTypeName());
        }
    }

}
