using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraGrid.Columns;
using System.Collections;
using Itop.Client.Projects;
using Itop.Domain.Graphics;
using System.IO;
using System.Xml;
using System.Reflection;

namespace Itop.TLPSP.DEVICE
{
    /// <summary>
    /// 母线
    /// </summary>
    public partial class UCDevicePWFHZL : Itop.TLPSP.DEVICE.UCDeviceBase
    {
        public UCDevicePWFHZL()
        {
            InitializeComponent();            
        }        
        #region 初始设置
        /// <summary>
        /// 设备初始化
        /// </summary>
        public override void Init() {
            if (ID!=null)
            {
                con = " Type='" + ID + "'";
            }
            if (ID == "54")
            {
                con = " Type='54'";
            }
            if (ID == "55")
            {
                con = " Type='55'";
            }
            if (ID == "56")
            {
                con = " Type='56'";
            }
            if (ID == "57")
            {
                con = " Type='57'";
            }
            if (ID == "58")
            {
                con = " Type='58'";
            }
            if (ID == "59")
            {
                con = " Type='59'";
            }
            if (ID == "61")
            {
                con = " Type='61'";
            }
            if (ID == "80")
            {
                con = " Type='80'";
            }
            base.Init();         
        }
        /// <summary>
        /// 检索设备
        /// </summary>
        public override void SelDevices()
        {
            if (ID != null)
            {
                con = " and Type='" + ID + "'";
            }
            if (ID == "54")
            {
                con = " and Type='54'";
            }
            if (ID == "55")
            {
                con = " and Type='55'";
            }
            if (ID == "56")
            {
                con = " and Type='56'";
            }
            if (ID == "57")
            {
                con = " and Type='57'";
            }
            if (ID == "58")
            {
                con = " and Type='58'";
            }
            if (ID == "59")
            {
                con = " and Type='59'";
            }
            if (ID == "80")
            {
                con = " and Type='80'";
            }
            base.SelDevices();
        }
        /// <summary>
        /// 设置设备显示列
        /// </summary>
        public override void InitColumns() {
            GridColumn column = gridView1.Columns.Add();
            column.Caption = "所在线路";
            column.FieldName = "AreaID";
            column.SortOrder = DevExpress.Data.ColumnSortOrder.Ascending;
            column.Width = 100;
            column.VisibleIndex = 1;
            column.OptionsColumn.AllowEdit = false;
            DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit comboBox = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            column.ColumnEdit = comboBox;
            string sql = "where Type in('05','73')";
            IList<PSPDEV> xl_list = Services.BaseService.GetList<PSPDEV>("SelectPSPDEVByCondition", sql);
            comboBox.DataSource = xl_list;
            comboBox.DisplayMember = "Name";
            comboBox.ValueMember = "SUID";

            column = gridView1.Columns.Add();
            column.Caption = "编号";
            column.FieldName = "Number";
            column.Width = 100;
            column.VisibleIndex = 1;
            column.OptionsColumn.AllowEdit = false;
            column = gridView1.Columns.Add();
            column.Caption = "名称";
            column.FieldName = "Name";
            column.Width = 100;
            column.VisibleIndex = 2;
            column.OptionsColumn.AllowEdit = false;
            column = gridView1.Columns.Add();
 
            //column.Caption = "开关数";
            //column.FieldName = "Flag";
            //column.Width = 100;
            //column.VisibleIndex = 3;
            //column.OptionsColumn.AllowEdit = false;
           

            column = gridView1.Columns.Add();
            column.Caption = "额定电压（KV）";
            column.FieldName = "RateVolt";
            column.Width = 150;
            column.VisibleIndex = 4;
            column.OptionsColumn.AllowEdit = false;
            column = gridView1.Columns.Add();
            column.Caption = "投产时间";
            column.FieldName = "OperationYear";
            column.Width = 150;
            column.VisibleIndex = 5;
            column.OptionsColumn.AllowEdit = false;
            column = gridView1.Columns.Add();
            column.Caption = "故障率";
            column.FieldName = "HuganTQ2";
            column.Width = 100;
            column.VisibleIndex = 6;
            column.OptionsColumn.AllowEdit = false;
           
            column = gridView1.Columns.Add();
            column.Caption = "修复时间";
            column.FieldName = "HuganTQ3";
            column.Width = 100;
            column.VisibleIndex = 7;
            column.OptionsColumn.AllowEdit = false;
            column = gridView1.Columns.Add();
            column.Caption = "是否有熔断器";
            column.FieldName = "ISwitch";
            column.Width = 100;
            column.VisibleIndex = 8;
            column.OptionsColumn.AllowEdit = false;
            column = gridView1.Columns.Add();
            column.Caption = "是否有变压器";
            column.FieldName = "JSwitch";
            column.Width = 100;
            column.VisibleIndex =9;
            column.OptionsColumn.AllowEdit = false;
            column = gridView1.Columns.Add();
            column.Caption = "用户数";
            column.FieldName = "Num1";
            column.Width = 100;
            column.VisibleIndex = 10;
            column.OptionsColumn.AllowEdit = false;
            column = gridView1.Columns.Add();
            column.Caption = "连接负荷（kW）";
            column.FieldName = "HuganTQ4";
            column.Width = 100;
            column.VisibleIndex = 11;
            column.OptionsColumn.AllowEdit = false;
        }
        #endregion
        public override string GetClassName()
        {
            return "PSPDEV";
        }
        public override string GetType()
        {
            return ID;
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
        public override void Add() {
            frmFHZLdlg dlg = new frmFHZLdlg();
            if (ID == "63" || ID == "64")
            {
                dlg.SetEnable();
            }
            dlg.ParentID = ParentID;
            dlg.Name = "";
            dlg.ProjectID = this.ProjectID;
            PSPDEV pd = new PSPDEV();
            pd.AreaID = ParentID;
            pd.ProjectID = ProjectID;
            
            Stream fs = Assembly.GetExecutingAssembly().GetManifestResourceStream("Itop.TLPSP.DEVICE.devicetypes.xml");
            //Assembly.GetExecutingAssembly().GetManifestResourceStream
            XmlDocument xml = new XmlDocument();
            xml.Load(fs);            
            XmlNode node = xml.SelectSingleNode("devicetype/*[@id='" + ID + "']");
            if (ID!=null)
            {
                dlg.Text = node.Attributes["name"].Value.ToString() + "信息";
            }
            dlg.DeviceMx = pd;
            //if(ID=="55"){
            //    dlg.Text = "开关站信息";
            //}
            //if (ID == "56")
            //{
            //    dlg.Text = "环网柜信息";
            //}
            //if (ID == "57")
            //{
            //    dlg.Text = "柱上开关信息";
            //}
            //if (ID == "58")
            //{
            //    dlg.Text = "电缆分支箱信息";
            //}
            //if (ID == "59")
            //{
            //    dlg.Text = "负荷开关信息";
            //}
            if (dlg.ShowDialog() == DialogResult.OK) {
                //增加记录 
                PSPDEV dev = dlg.DeviceMx;
                dev.Type = ID;
                dev.ProjectID = this.ProjectID;
                UCDeviceBase.DataService.Create("InsertPSPDEV", dev);
                DataRow row=datatable1.NewRow();
             
                Itop.Common.DataConverter.ObjectToRow(dev, row);
                datatable1.Rows.Add(row); 
            }
        }
        public override void Delete() {
            //删除记录     
            base.Delete();
        }
        public override void Edit() {
            frmFHZLdlg dlg = new frmFHZLdlg();
            if (ID == "63" || ID == "64")
            {
                dlg.SetEnable();
            }
            DataRow row = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            if (row != null)
            {
                PSPDEV dev = Itop.Common.DataConverter.RowToObject<PSPDEV>(row);
             
              
                dlg.DeviceMx = dev;
                dlg.ProjectID = dev.ProjectID;
                Stream fs = Assembly.GetExecutingAssembly().GetManifestResourceStream("Itop.TLPSP.DEVICE.devicetypes.xml");
                //Assembly.GetExecutingAssembly().GetManifestResourceStream
                XmlDocument xml = new XmlDocument();
                xml.Load(fs);
                XmlNode node = xml.SelectSingleNode("devicetype/*[@id='" + ID + "']");
                if (node != null)
                {
                    dlg.Text = node.Attributes["name"].Value.ToString()+"信息";
                }
                if (ID == "55")
                {
                    dlg.Text = "开关站信息";
                }
                if (ID == "56")
                {
                    dlg.Text = "环网柜信息";
                }
                if (ID == "57")
                {
                    dlg.Text = "柱上开关信息";
                }
                if (ID == "58")
                {
                    dlg.Text = "电缆分支箱信息";
                }
                if (ID == "59")
                {
                    dlg.Text = "负荷开关信息";
                }
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    //更新记录
                    dev = dlg.DeviceMx;
                    dev.ProjectID = this.ProjectID;
                    dev.Type = ID;
                    UCDeviceBase.DataService.Update<PSPDEV>(dev);
                  
                    Itop.Common.DataConverter.ObjectToRow(dev, row);
                }
            }
        }
        
        #endregion
    }
}

