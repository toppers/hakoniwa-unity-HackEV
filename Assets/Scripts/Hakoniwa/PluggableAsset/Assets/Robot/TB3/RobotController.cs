﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

using Hakoniwa.Core;
using Hakoniwa.PluggableAsset.Assets;
using Hakoniwa.PluggableAsset.Communication.Connector;
using Hakoniwa.PluggableAsset.Communication.Pdu;

namespace Hakoniwa.PluggableAsset.Assets.Robot.TB3
{
    public class RobotController : MonoBehaviour, IInsideAssetController
    {
        private GameObject root;
        private GameObject myObject;
        private ITB3Parts parts;
        private string my_name;
        private PduIoConnector pdu_io;
        private IPduWriter pdu_laser_scan;
        private IPduWriter pdu_imu;
        private IPduReader pdu_motor_control;
        private ILaserScan laser_scan;
        private IIMUSensor imu;
        private MotorController motor_controller;

        public void CopySensingDataToPdu()
        {
            //LaserSensor
            this.laser_scan.UpdateSensorValues();
            this.laser_scan.UpdateSensorData(pdu_laser_scan.GetWriteOps().Ref(null));

            //IMUSensor
            this.imu.UpdateSensorValues();
            this.imu.UpdateSensorData(pdu_imu.GetWriteOps().Ref(null));
        }

        public void DoActuation()
        {
            this.motor_controller.DoActuation();
        }

        public string GetName()
        {
            return this.my_name;
        }

        public void Initialize()
        {
            Debug.Log("TurtleBot3");
            this.root = GameObject.Find("Robot");
            this.myObject = GameObject.Find("Robot/" + this.transform.name);
            this.parts = myObject.GetComponentInChildren<ITB3Parts>();
            this.my_name = string.Copy(this.transform.name);
            this.pdu_io = PduIoConnector.Get(this.GetName());
            this.InitActuator();
            this.InitSensor();
        }

        private void InitSensor()
        {
            GameObject obj;
            string subParts = this.parts.GetLaserScan();
            if (subParts != null)
            {
                obj = root.transform.Find(this.transform.name + "/" + subParts).gameObject;
                Debug.Log("path=" + this.transform.name + "/" + subParts);
                laser_scan = obj.GetComponentInChildren<ILaserScan>();
                laser_scan.Initialize(obj);
            }
            subParts = this.parts.GetIMU();
            if (subParts != null)
            {
                obj = root.transform.Find(this.transform.name + "/" + subParts).gameObject;
                Debug.Log("path=" + this.transform.name + "/" + subParts);
                imu = obj.GetComponentInChildren<IIMUSensor>();
                imu.Initialize(obj);
            }
            this.pdu_laser_scan = this.pdu_io.GetWriter(this.GetName() + "_Tb3LaserSensorPdu");
            if (this.pdu_laser_scan == null)
            {
                throw new ArgumentException("can not found LaserScan pdu:" + this.GetName() + "_Tb3LaserSensorPdu");
            }
            this.pdu_imu = this.pdu_io.GetWriter(this.GetName() + "_Tb3ImuSensorPdu");
            if (this.pdu_imu == null)
            {
                throw new ArgumentException("can not found Imu pdu:" + this.GetName() + "_Tb3ImuSensorPdu");
            }
        }

        private void InitActuator()
        {
            motor_controller = new MotorController();
            this.pdu_motor_control = this.pdu_io.GetReader(this.GetName() + "_Tb3CmdVelPdu");
            if (this.pdu_motor_control == null)
            {
                throw new ArgumentException("can not found CmdVel pdu:" + this.GetName() + "_Tb3CmdVelPdu");
            }
            motor_controller.Initialize(this.root, this.transform, this.parts, this.pdu_motor_control);
        }
    }
}