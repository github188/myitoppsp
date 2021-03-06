using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraGrid.Columns;
using System.Collections;
using Itop.Client.Projects;
using Itop.Domain.Graphics;

namespace Itop.TLPSP.DEVICE
{
    public partial class UCDeviceML : Itop.TLPSP.DEVICE.UCDeviceBase
    {
        public UCDeviceML()
        {
            InitializeComponent();
        }
        public override void Init()
        {
            con = " Type='13'";
            base.Init();
        }
        /// <summary>
        /// 检索设备
        /// </summary>
        public override void SelDevices()
        {
            con = "and Type='13'";
            base.SelDevices();
        }
        /// <summary>
        ///短路图数据查询入口
        /// </summary>
        public override void SelshortDevices()
        {
            con = " and Type='13'";
            base.SelshortDevices();
        }
        /// <summary>
        /// 设置设备显示列
        /// </summary>
        public override void InitColumns()
        {
            GridColumn column = gridView1.Columns.Add();
            column.Caption = "1/2母联开关";
            column.FieldName = "Name";
            column.Width = 100;
            column.VisibleIndex = 1;
            column.OptionsColumn.AllowEdit = false;
            column = gridView1.Columns.Add();
            column.Caption = "1/2母联编号";
            column.FieldName = "Number";
            column.Width = 100;
            column.VisibleIndex = 2;
            column.OptionsColumn.AllowEdit = false;
            //column = gridView1.Columns.Add();
            //column.Caption = "节点类型";
            //column.FieldName = "NodeType";
            //column.Width = 100;
            //column.VisibleIndex = 3;
            //column.OptionsColumn.AllowEdit = false;
            column = gridView1.Columns.Add();
            column.Caption = "I侧母线名称";
            column.FieldName = "IName";
            column.Width = 100;
            column.VisibleIndex = 4;
            column.OptionsColumn.AllowEdit = false;
            column = gridView1.Columns.Add();
            column.Caption = "J侧母线名称";
            column.FieldName = "JName";
            column.Width = 100;
            column.VisibleIndex = 5;
            column.OptionsColumn.AllowEdit = false;
            column = gridView1.Columns.Add();
            column.Caption = "开断状态";
            column.FieldName = "KSwitchStatus";
            column.Width = 100;
            column.VisibleIndex = 9;
            column.OptionsColumn.AllowEdit = false;
        }
        public override string GetClassName()
        {
            return "PSPDEV";
        }
        public override string GetType()
        {
            return "13";
        }
        #region 记录操作
        public override object SelectedDevice
        {
            get
            {
                DataRow row = gridView1.GetDataRow(gridView1.FocusedRowHandle);

                if (row != null)
                {
                    return Itop.Common.DataConverter.RowToObject<PSPDEV>(row);
                }
                return base.SelectedDevice;
            }
        }
        public override void Add()
        {
            //frmDLQdlg dlg = new frmDLQdlg();
            //dlg.Name = "";
            //if (dlg.ShowDialog() == DialogResult.OK) {
            //    //增加记录 
            //    PSPDEV dev = dlg.DeviceMx;
            //    dev.ProjectID = this.ProjectID;
            //    UCDeviceBase.DataService.Create("InsertPSPDEV", dev);
            //    DataRow row=datatable1.NewRow();
            //    Itop.Common.DataConverter.ObjectToRow(dev, row);
            //    datatable1.Rows.Add(row); 
            //}
            frmMLdlg dlg = new frmMLdlg();
            dlg.ProjectID = this.ProjectID;
            dlg.Name = "";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                //增加记录 
                PSPDEV dev = dlg.DeviceMx;
                dev.ProjectID = this.ProjectID;
                UCDeviceBase.DataService.Create("InsertPSPDEV", dev);
                DataRow row = datatable1.NewRow();
                if (dev.NodeType == "0")
                {
                    dev.NodeType = "平衡节点";
                }
                else if (dev.NodeType == "1")
                {
                    dev.NodeType = "PQ节点";
                }
                else if (dev.NodeType == "2")
                {
                    dev.NodeType = "PV节点";
                }
                else
                {
                    dev.NodeType = null;
                }
                if (dev.KSwitchStatus == "1")
                {
                    dev.KSwitchStatus = "退出运行";
                }
                else
                {
                    dev.KSwitchStatus = "投入运行";
                }
                if (dev.UnitFlag == "0")
                {
                    dev.UnitFlag = "p.u.";
                }
                else
                {
                    if (dev.Type == "01" || dev.Type == "04" || dev.Type == "12")
                    {
                        dev.UnitFlag = "kV/MW/MVar";
                    }
                    else
                    {
                        dev.UnitFlag = "Ohm/10-6Siem";
                    }
                }
                Itop.Common.DataConverter.ObjectToRow(dev, row);
                datatable1.Rows.Add(row);
            }
        }
        public override void Delete()
        {
            //删除记录     
            base.Delete();
        }
        public override void Edit()
        {
            frmMLdlg dlg = new frmMLdlg();
            DataRow row = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            if (row != null)
            {
                PSPDEV dev = Itop.Common.DataConverter.RowToObject<PSPDEV>(row);
                if (dev.NodeType == "平衡节点")
                {
                    dev.NodeType = "0";
                }
                else if (dev.NodeType == "PQ节点")
                {
                    dev.NodeType = "1";
                }
                else if (dev.NodeType == "PV节点")
                {
                    dev.NodeType = "2";
                }
                if (dev.KSwitchStatus == "退出运行")
                {
                    dev.KSwitchStatus = "1";
                }
                else
                {
                    dev.KSwitchStatus = "0";
                }
                if (dev.UnitFlag == "p.u.")
                {
                    dev.UnitFlag = "0";
                }
                else
                {
                    dev.UnitFlag = "1";

                }
                dlg.DeviceMx = dev;
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    //更新记录
                    dev = dlg.DeviceMx;
                    dev.ProjectID = this.ProjectID;
                    UCDeviceBase.DataService.Update<PSPDEV>(dev);
                    if (dev.NodeType == "0")
                    {
                        dev.NodeType = "平衡节点";
                    }
                    else if (dev.NodeType == "1")
                    {
                        dev.NodeType = "PQ节点";
                    }
                    else if (dev.NodeType == "2")
                    {
                        dev.NodeType = "PV节点";
                    }
                    else
                    {
                        dev.NodeType = null;
                    }
                    if (dev.KSwitchStatus == "1")
                    {
                        dev.KSwitchStatus = "退出运行";
                    }
                    else
                    {
                        dev.KSwitchStatus = "投入运行";
                    }
                    if (dev.UnitFlag == "0")
                    {
                        dev.UnitFlag = "p.u.";
                    }
                    else
                    {
                        if (dev.Type == "01" || dev.Type == "04" || dev.Type == "12")
                        {
                            dev.UnitFlag = "kV/MW/MVar";
                        }
                        else
                        {
                            dev.UnitFlag = "Ohm/10-6Siem";
                        }
                    }
                    Itop.Common.DataConverter.ObjectToRow(dev, row);
                }
            }
        }

        #endregion
    }
}
