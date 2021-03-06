﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using Itop.Domain.Graphics;
using Itop.Client.Common;
using System.Collections;
using Itop.Common;
using DevExpress.XtraGrid.Views.BandedGrid;
using System.IO;
using Itop.Client.Stutistics;
using System.Xml;
using Itop.Client.Base;
using DevExpress.XtraTreeList.Columns;
using DevExpress.XtraTreeList.Nodes;
using Itop.TLPSP.DEVICE;
using System.Reflection;
using Excel = Microsoft.Office.Interop.Excel;
using Office = Microsoft.Office.Core;
namespace Itop.TLPSP.DEVICE
{
    public partial class UcPdtypenode : DevExpress.XtraEditors.XtraUserControl
    {
        public UcPdtypenode()
        {
            InitializeComponent();
            barButtonItem6.Glyph = Itop.ICON.Resource.打回重新编;
            barSubItem1.Glyph = Itop.ICON.Resource.修改;
            barButtonItem10.Glyph = Itop.ICON.Resource.作废;

        }
        private Ps_pdreltype parentobj=new Ps_pdreltype();
        private string pdreltypeid;
        DataTable dt = new DataTable();
        DataTable resulttb = new DataTable();
        DataTable resultzbtb = new DataTable();
        public Ps_pdreltype ParentObj
        {
            get
            {
                return parentobj;
            }
            set
            {
                parentobj = value;
                pdreltypeid = parentobj.ID;
                Init();
            }
        }
        private void Init()
        {
           // TreeListColumn column1 = new TreeListColumn();
           // column1.Caption = "样式";
           // column1.FieldName = "devicetype";
           // column1.VisibleIndex = -1;
           // column1.OptionsColumn.AllowEdit = false;
           // column1.OptionsColumn.AllowSort = false;

           // TreeListColumn column = new TreeListColumn();
           // column.Caption = "元件";
           // column.FieldName = "title";
           // column.VisibleIndex =0;
           // column.Width = 40;
           // column.OptionsColumn.AllowEdit = false;
           // column.OptionsColumn.AllowSort = false;
           // treeList1.Columns.AddRange(new TreeListColumn[] {
           //column1, column});
            treeList1.KeyFieldName = "ID";
            treeList1.ParentFieldName = "ParentID";
            IList<Ps_pdtypenode> list1 = Services.BaseService.GetList<Ps_pdtypenode>("SelectPs_pdtypenodeByCon", "pdreltypeid='" + pdreltypeid + "' ORDER BY SUBSTRING(Code, 3, 1)");
            dt = Itop.Common.DataConverter.ToDataTable((IList)list1, typeof(Ps_pdtypenode));
            this.treeList1.DataSource = dt;
        }
        #region 私有方法
        /// <summary>
        /// 实例化类接口
        /// </summary>
        /// <param name="classname"></param>
        /// <returns></returns>
        private UCDeviceBase createInstance(string classname)
        {
            //return Assembly.GetExecutingAssembly().CreateInstance(classname, false) as UCDeviceBase;
            return Assembly.Load("Itop.TLPSP.DEVICE").CreateInstance(classname, false) as UCDeviceBase;
        }
        private void showDevice(UCDeviceBase device)
        {
            if (device == null) return;
            device.Dock = DockStyle.Fill;
            splitContainerControl1.Panel2.Controls.Add(device);
        }
        #endregion
        #region 字段
        UCDeviceBase curDevice;
        Dictionary<string, UCDeviceBase> devicTypes = new Dictionary<string, UCDeviceBase>();

        #endregion
       //添加元件所关联的设备
        private void adducdevice(string Devicetype)
        {

            string dtype = DeviceTypeHelper.DeviceClassbyType(Devicetype);
            if (string.IsNullOrEmpty(dtype))
            {
                if (curDevice != null)
                {
                    curDevice.Hide();
                }
                return;
            }

            UCDeviceBase device = null;
            if (devicTypes.ContainsKey(dtype))
            {
                device = devicTypes[dtype];
                device.ID = Devicetype;
                try
                {
                    device.Show();
                }
                catch { }
            }
            else
            {
                device = createInstance(dtype);
                device.ID = Devicetype;
                device.ProjectID = Itop.Client.MIS.ProgUID;
                devicTypes.Add(dtype, device);
                showDevice(device);
            }

            if (curDevice != null && curDevice != device) curDevice.Hide();
            curDevice = device;
            if (curDevice != null)
            {
                
                //给一个空的选择
                curDevice.strCon = " where 1=1 and suid='1111' and";
                curDevice.Init();
            }
        }
        private void treeList1_MouseClick(object sender, MouseEventArgs e)
        {
            //if (e.Button == MouseButtons.Right) return;
            //TreeListNode node = treeList1.FocusedNode;
            //if (node == null) return;
            //string deviceid = node["DeviceID"].ToString();
            //string strID = node["devicetype"].ToString();
            //string dtype = DeviceTypeHelper.DeviceClassbyType(strID);
            //if (string.IsNullOrEmpty(dtype))
            //{
            //    if (curDevice != null)
            //    {
            //        curDevice.Hide();
            //    }
            //    return;
            //}

            //UCDeviceBase device = null;
            //if (devicTypes.ContainsKey(dtype))
            //{
            //    device = devicTypes[dtype];
            //    device.ID = strID;
            //    try
            //    {
            //        device.Show();
            //    }
            //    catch { }
            //}
            //else
            //{
            //    device = createInstance(dtype);
            //    device.ID = strID;
            //    device.ProjectID = Itop.Client.MIS.ProgUID;
            //    devicTypes.Add(dtype, device);
            //    showDevice(device);
            //}

            //if (curDevice != null && curDevice != device) curDevice.Hide();
            //curDevice = device;
            //if (curDevice != null)
            //{
            //    curDevice.strCon = " where 1=1 and suid='" + deviceid + "'and ";
            //    curDevice.Init();
            //}
        }

        private void treeList1_GetStateImage(object sender, DevExpress.XtraTreeList.GetStateImageEventArgs e)
        {
            string stateimage = e.Node.GetValue("devicetype").ToString();
            switch (stateimage)
            {
                case "01":
                    e.NodeImageIndex = 33;
                    break;
                case "73":
                    e.NodeImageIndex = 28;
                    break;
                case "74":
                    e.NodeImageIndex = 29;
                    break;
                case "55":
                    e.NodeImageIndex = 31;
                    break;
                case "06":
                    e.NodeImageIndex = 26;
                    break;
                case "80":
                    e.NodeImageIndex = 30;
                    break;
                case "75":
                    e.NodeImageIndex = 32;
                    break;
                default:
                    e.NodeImageIndex = 28;
                    break;
            }
        }
#region  添加元件
        //添加馈线
        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

            TreeListNode tln = treeList1.FocusedNode;

            if (tln != null)
            {
                DataRow row = (tln.TreeList.GetDataRecordByNode(tln) as DataRowView).Row;
                Ps_pdtypenode v = DataConverter.RowToObject<Ps_pdtypenode>(row);
                if (tln.GetValue("devicetype").ToString() == "01" || tln.GetValue("devicetype").ToString() == "73")
                {
                    adducdevice("73");
                    curDevice.ParentID = v.DeviceID;
                    curDevice.Add();
                    PSPDEV pd = curDevice.SelectedDevice as PSPDEV;
                    //馈线段记录
                    if (pd == null)
                        return;
                    Ps_pdtypenode pn = new Ps_pdtypenode();
                    pn.title = pd.Name;
                    pn.pdreltypeid = pdreltypeid;
                    pn.devicetype = "73";
                    pn.DeviceID = pd.SUID;
                    pn.ParentID = tln.GetValue("ID").ToString();
                    pn.Code = (tln.Level + 1).ToString() + "1" + (tln.Nodes.Count + 1).ToString();
                    Services.BaseService.Create<Ps_pdtypenode>(pn);
                    dt.Rows.Add(Itop.Common.DataConverter.ObjectToRow(pn, dt.NewRow()));
                }
                else
                {
                    MsgBox.Show("请选择电源后，再操作！");
                    return;
                }
            }
        }
        //添加馈线段
        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            TreeListNode tln = treeList1.FocusedNode;

            if (tln != null)
            {
                DataRow row = (tln.TreeList.GetDataRecordByNode(tln) as DataRowView).Row;
                Ps_pdtypenode v = DataConverter.RowToObject<Ps_pdtypenode>(row);
                if (tln.GetValue("devicetype").ToString() == "73")
                {
                    adducdevice("74");
                    curDevice.ParentID = v.DeviceID;
                    curDevice.Add();

                    PSPDEV pd = curDevice.SelectedDevice as PSPDEV;
                    //馈线段记录
                    if (pd == null)
                        return;
                    Ps_pdtypenode pn = new Ps_pdtypenode();
                    pn.title = pd.Name;
                    pn.pdreltypeid = pdreltypeid;
                    pn.devicetype = "74";
                    pn.DeviceID = pd.SUID;
                    pn.ParentID = tln.GetValue("ID").ToString();
                    pn.Code = (tln.Level + 1).ToString() + "2" + (tln.Nodes.Count + 1).ToString();
                    Services.BaseService.Create<Ps_pdtypenode>(pn);
                    dt.Rows.Add(Itop.Common.DataConverter.ObjectToRow(pn, dt.NewRow()));
                }
                else
                {
                    MsgBox.Show("请选择馈线，再操作！");
                    return;
                }
            }
        }
        //添加负荷之路
        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            TreeListNode tln = treeList1.FocusedNode;

            if (tln != null)
            {
                DataRow row = (tln.TreeList.GetDataRecordByNode(tln) as DataRowView).Row;
                Ps_pdtypenode v = DataConverter.RowToObject<Ps_pdtypenode>(row);
                if (tln.GetValue("devicetype").ToString() == "73")
                {
                    adducdevice("80");
                    curDevice.ParentID = v.DeviceID;
                    curDevice.Add();

                    PSPDEV pd = curDevice.SelectedDevice as PSPDEV;
                    //馈线段记录
                    if (pd == null)
                        return;
                    Ps_pdtypenode pn = new Ps_pdtypenode();
                    pn.title = pd.Name;
                    pn.pdreltypeid = pdreltypeid;
                    pn.devicetype = "80";
                    pn.DeviceID = pd.SUID;
                    pn.ParentID = tln.GetValue("ID").ToString();
                    pn.Code = (tln.Level + 1).ToString() + "3" + (tln.Nodes.Count + 1).ToString();
                    Services.BaseService.Create<Ps_pdtypenode>(pn);
                    dt.Rows.Add(Itop.Common.DataConverter.ObjectToRow(pn, dt.NewRow()));
                }
                else
                {
                    MsgBox.Show("请选择馈线，再操作！");
                    return;
                }
            }
        }
        //添加联络线
        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            TreeListNode tln = treeList1.FocusedNode;

            if (tln != null)
            {
                DataRow row = (tln.TreeList.GetDataRecordByNode(tln) as DataRowView).Row;
                Ps_pdtypenode v = DataConverter.RowToObject<Ps_pdtypenode>(row);
                if (tln.GetValue("devicetype").ToString() == "73")
                {
                    adducdevice("75");
                    curDevice.ParentID = v.DeviceID;
                    curDevice.Add();

                    PSPDEV pd = curDevice.SelectedDevice as PSPDEV;
                    //馈线段记录
                    if (pd == null)
                        return;
                    Ps_pdtypenode pn = new Ps_pdtypenode();
                    pn.title = pd.Name;
                    pn.pdreltypeid = pdreltypeid;
                    pn.devicetype = "75";
                    pn.DeviceID = pd.SUID;
                    pn.ParentID = tln.GetValue("ID").ToString();
                    pn.Code = (tln.Level + 1).ToString() + "5" + (tln.Nodes.Count + 1).ToString();
                    Services.BaseService.Create<Ps_pdtypenode>(pn);
                    dt.Rows.Add(Itop.Common.DataConverter.ObjectToRow(pn, dt.NewRow()));
                }
                else
                {
                    MsgBox.Show("请选择馈线，再操作！");
                    return;
                }
            }
        }
        //添加断路器
        private void barButtonItem8_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            TreeListNode tln = treeList1.FocusedNode;

            if (tln != null)
            {
                DataRow row = (tln.TreeList.GetDataRecordByNode(tln) as DataRowView).Row;
                Ps_pdtypenode v = DataConverter.RowToObject<Ps_pdtypenode>(row);
                if (tln.GetValue("devicetype").ToString() == "74")
                {
                    adducdevice("06");
                    curDevice.ParentID = v.DeviceID;
                    curDevice.Add();

                    PSPDEV pd = curDevice.SelectedDevice as PSPDEV;
                    //馈线段记录
                    if (pd == null)
                        return;
                    Ps_pdtypenode pn = new Ps_pdtypenode();
                    pn.title = pd.Name;
                    pn.pdreltypeid = pdreltypeid;
                    pn.devicetype = "06";
                    pn.DeviceID = pd.SUID;
                    pn.ParentID = tln.GetValue("ID").ToString();
                    pn.Code = (tln.Level + 1).ToString() + "4" + (tln.Nodes.Count + 1).ToString();
                    Services.BaseService.Create<Ps_pdtypenode>(pn);
                    dt.Rows.Add(Itop.Common.DataConverter.ObjectToRow(pn, dt.NewRow()));
                }
                else
                {
                    MsgBox.Show("请选择馈线段，再操作！");
                    return;
                }
            }
        }
        //添加隔离开关
        private void barButtonItem9_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            TreeListNode tln = treeList1.FocusedNode;

            if (tln != null)
            {
                DataRow row = (tln.TreeList.GetDataRecordByNode(tln) as DataRowView).Row;
                Ps_pdtypenode v = DataConverter.RowToObject<Ps_pdtypenode>(row);
                if (tln.GetValue("devicetype").ToString() == "74")
                {
                    adducdevice("55");
                    curDevice.ParentID = v.DeviceID;
                    curDevice.Add();

                    PSPDEV pd = curDevice.SelectedDevice as PSPDEV;
                    //馈线段记录
                    if (pd == null)
                        return;
                    Ps_pdtypenode pn = new Ps_pdtypenode();
                    pn.title = pd.Name;
                    pn.pdreltypeid = pdreltypeid;
                    pn.devicetype = "55";
                    pn.DeviceID = pd.SUID;
                    pn.ParentID = tln.GetValue("ID").ToString();
                    pn.Code = (tln.Level + 1).ToString() + "6" + (tln.Nodes.Count + 1).ToString();
                    Services.BaseService.Create<Ps_pdtypenode>(pn);
                    dt.Rows.Add(Itop.Common.DataConverter.ObjectToRow(pn, dt.NewRow()));
                }
                else
                {
                    MsgBox.Show("请选择馈线段，再操作！");
                    return;
                }
            }
        }
#endregion
        //修改元件
        private void barButtonItem6_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            TreeListNode tln = treeList1.FocusedNode;

            if (tln != null)
            {
                DataRow row = (tln.TreeList.GetDataRecordByNode(tln) as DataRowView).Row;
                Ps_pdtypenode v = DataConverter.RowToObject<Ps_pdtypenode>(row);

                curDevice.Edit();
                if (curDevice.SelectedDevice!=null)
                {
                    PSPDEV pd = curDevice.SelectedDevice as PSPDEV;
                    v.title = pd.Name;
                    Services.BaseService.Update<Ps_pdtypenode>(v);
                    tln.SetValue("title", v.title);    
                }
                   
            }
        }
        //删除元件
        public void DeleteNode(TreeListNode tln)
        {

            
            if (tln.HasChildren)
            {
                for (int i = 0; i < tln.Nodes.Count; i++)
                {
                    DeleteNode(tln.Nodes[i]);
                }
                DeleteNode(tln);
            }
            else
            {
                Ps_pdtypenode pf = new Ps_pdtypenode();
                pf.ID = tln["ID"].ToString();
                string nodestr = tln["AreaName"].ToString();
                try
                {
                    TreeListNode node = tln.TreeList.FindNodeByKeyID(pf.ID);
                    if (node != null)
                        tln.TreeList.DeleteNode(node);
                    RemoveDataTableRow(dt, pf.ID);
                    Services.BaseService.Delete<Ps_pdtypenode>(pf);


                }
                catch (Exception e)
                {

                    MessageBox.Show(e.Message + "删除结点出错！");
                }

            }


        }
        public void RemoveDataTableRow(DataTable dt, string ID)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["ID"].ToString() == ID)
                {
                    dt.Rows.RemoveAt(i);
                    break;
                }
            }
        }
      
      

        private void barButtonItem7_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            TreeListNode tln = treeList1.FocusedNode;
            if (tln == null)
            {
                return;
            }

           
            if (MsgBox.ShowYesNo("是否删除？") != DialogResult.Yes)
            {
                return;
            }
            DeleteNode(tln);
        }

        private void barButtonItem10_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            TreeListNode tln = treeList1.FocusedNode;
            frmDeviceSelect dlg = new frmDeviceSelect();
            if (tln != null)
            {
                DataRow row = (tln.TreeList.GetDataRecordByNode(tln) as DataRowView).Row;
                Ps_pdtypenode v = DataConverter.RowToObject<Ps_pdtypenode>(row);
                if (tln.GetValue("devicetype").ToString() == "01")
                {
                    //adducdevice("73");
                    //curDevice.Add();
                    //PSPDEV pd = curDevice.SelectedDevice as PSPDEV;
                    //馈线段记录
                    dlg.InitDeviceType("01", "73");
                }
                else if (tln.GetValue("devicetype").ToString() == "73")
                {
                    dlg.InitDeviceType("73", "74", "80", "75");

                }
                else if (tln.GetValue("devicetype").ToString() == "74")
                {
                    dlg.InitDeviceType("06", "55");

                }
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    Dictionary<string, object> dic = dlg.GetSelectedDevice();
                    PSPDEV devzx = dic["device"] as PSPDEV;
                    //S1 = devzx.SUID;
                    Ps_pdtypenode pn = new Ps_pdtypenode();
                    pn.title = devzx.Name;
                    pn.pdreltypeid = pdreltypeid;
                    pn.devicetype = devzx.Type;
                    pn.DeviceID = devzx.SUID;
                    if (devzx.Type != "01")
                    {
                        pn.ParentID = tln.GetValue("ID").ToString();
                    }
                    switch (devzx.Type)
                    {
                        case "73":
                            pn.Code = (tln.Level + 1).ToString() + "1" + (tln.Nodes.Count + 1).ToString();
                            break;
                        case "74":
                            pn.Code = (tln.Level + 1).ToString() + "2" + (tln.Nodes.Count + 1).ToString();
                            break;
                        case "80":
                            pn.Code = (tln.Level + 1).ToString() + "3" + (tln.Nodes.Count + 1).ToString();
                            break;
                        case "75":
                            pn.Code = (tln.Level + 1).ToString() + "5" + (tln.Nodes.Count + 1).ToString();
                            break;
                        case "06":
                            pn.Code = (tln.Level + 1).ToString() + "4" + (tln.Nodes.Count + 1).ToString();
                            break;
                        case "55":
                            pn.Code = (tln.Level + 1).ToString() + "6" + (tln.Nodes.Count + 1).ToString();
                            break;
                        case "01":
                            pn.Code = treeList1.Nodes.Count.ToString();
                            break;
                    }

                    Services.BaseService.Create<Ps_pdtypenode>(pn);
                    dt.Rows.Add(Itop.Common.DataConverter.ObjectToRow(pn, dt.NewRow()));
                }
            }
        }

        private void barButtonItem12_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            PSPDEV psd = new PSPDEV();
            psd.SUID = parentobj.ID;
            psd = Services.BaseService.GetOneByKey<PSPDEV>(psd);
            if (psd!=null)
            {
                XtraPDrelfrm xf = new XtraPDrelfrm();
                xf.ParentObj = psd;
                xf.init();
                xf.ShowDialog();
            }

          
        }
        int order = 0;
        private void barButtonItem13_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Init();  //刷新元件树
            resulttb = new DataTable();
            resulttb.Columns.Add("fhname", typeof(string));
            resulttb.Columns.Add("zbname", typeof(string));
            resulttb.Columns.Add("result", typeof(double));
            resulttb.Columns.Add("pdfs", typeof(string));
            resulttb.Columns.Add("A", typeof(string));
            resulttb.Columns.Add("B", typeof(string));
            order = 0;
            resultzbtb = new DataTable();
            resultzbtb.Columns.Add("fhname", typeof(string));
            resultzbtb.Columns.Add("zbname", typeof(string));
            resultzbtb.Columns.Add("result", typeof(double));
            resultzbtb.Columns.Add("pdfs", typeof(string));
            resultzbtb.Columns.Add("A", typeof(string));
            resultzbtb.Columns.Add("B", typeof(string));
           

             ExcelAccess ex = new ExcelAccess();
            try
            {
              
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                string fname = Application.StartupPath + "\\xls\\tempt.xls";
                ex.Open(fname);
                //ex.ActiveSheet(1);
                ex.SetCellValue("指标", 1, 1);
                ex.SetCellValue("不同的配点方式", 1, 3);
                ex.AlignmentCells(1, 1, 1, 1, ExcelStyle.ExcelHAlign.居中, ExcelStyle.ExcelVAlign.居中);
                ex.SetFontStyle(1, 1, 1, 1, true, false, ExcelStyle.UnderlineStyle.无下划线);
                ex.CellsBackColor(1, 1, 1, 1, ExcelStyle.ColorIndex.黄色);
                ex.UnitCells(1, 1, 2, 2);
                DataTable dt = new DataTable();
                frmfxlx fm = new frmfxlx();
                if (fm.ShowDialog() == DialogResult.OK)
                {
                    dt = fm.DT1;
                    int columnscount = 0;
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (Convert.ToInt32(dr["B"]) == 1)
                        {

                            TreeListNode tln = treeList1.FindNodeByKeyID(pdreltypeid);
                            bool flag = relanalsy(tln, Convert.ToInt32(dr["D"]), columnscount, ex);
                            columnscount++;
                            if (!flag)
                            {
                                ex.DisPoseExcel();
                                return;
                            }
                        }
                    }
                    //FrmResult FR = new FrmResult();
                    //FR.DT = resulttb;
                    //FR.DT1 = resultzbtb;
                    //FR.ShowDialog();
                    if (columnscount==0)
                    {
                        return;
                    }
                    ex.UnitCells(1, 3, 1, 2 + columnscount);
                    ex.AlignmentCells(1, 3, 1, 2 + columnscount, ExcelStyle.ExcelHAlign.居中, ExcelStyle.ExcelVAlign.居中);
                    ex.SetFontStyle(1, 3, 1, 2 + columnscount, true, false, ExcelStyle.UnderlineStyle.无下划线);
                    ex.CellsBackColor(1, 3, 1, 2 + columnscount, ExcelStyle.ColorIndex.黄色);
                    ex.ShowExcel();
                }
                else
                {
                    ex.DisPoseExcel();
                }
               
            }
            catch (System.Exception exe)
            {
                ex.DisPoseExcel();
                MessageBox.Show("存在问题！请检查数据后再计算");
            }
           
        }
       
        //以报表的形式输出结果
        //private void ExportExcel(string name)
        //{

        //    //if (File.Exists(System.Windows.Forms.Application.StartupPath + "\\temp.xls"))
        //    //{
        //    //    File.Delete(System.Windows.Forms.Application.StartupPath + "\\temp.xls");
        //    //}
        //    //FileStream fs = new FileStream(System.Windows.Forms.Application.StartupPath + "\\temp.xls", FileMode.CreateNew);
        //    //fs.Dispose();
        //   ex = new ExcelAccess();
        //    SaveFileDialog saveFileDialog1 = new SaveFileDialog();
        //    string fname = Application.StartupPath + "\\xls\\tempt.xls";
        //    ex.Open(fname);
        //    //ex.ActiveSheet(1);
        //    try
        //    {
        //        ex.SetCellValue(name, 1, 1);
        //        ex.SetCellValue("不同的配点方式", 1, 3);
        //        ex.AlignmentCells(1, 1, 1, 1, ExcelStyle.ExcelHAlign.居中, ExcelStyle.ExcelVAlign.居中);
        //        ex.SetFontStyle(1, 1, 1, 1, true, false, ExcelStyle.UnderlineStyle.无下划线);
        //        ex.CellsBackColor(1, 1, 1, 1, ExcelStyle.ColorIndex.黄色);
        //        ex.UnitCells(1, 1, 2, 2);
                
        //    }
        //    catch (System.Exception e)
        //    {
            	
        //    }
            
            
            
           
        //}
        //XL为分析的线路 
        private bool relanalsy(TreeListNode xl, int fxtype,int columncounts,ExcelAccess ex)
        {
            foreach (TreeListNode tln in xl.Nodes)
            {
                if (tln.GetValue("devicetype").ToString() == "73" && string.IsNullOrEmpty(tln.GetValue("S1").ToString()) && !tln.GetValue("title").ToString().Contains("节点有问题"))
                {
                    //对子线路进行等值分析
                    dzanalsy(tln, fxtype);
                }
            }
            
            //求取主线相关联的负荷节点的 停电率 停电时间等
            //各个线路段对该层负荷点的 故障率和停运时间的影响
            Dictionary<PSPDEV ,List<rresult>> rescol=new Dictionary<PSPDEV,List<rresult>>();
            //主线路各负荷点的影响分析
            //下行等值化
            xxanaly(xl);
            
            //负荷分析
           if (everyxlanalyst(xl, ref rescol, fxtype))
           {
               //对其结果进行处理分析
               Dictionary<string, rresult> fhjg = new Dictionary<string, rresult>();
               foreach (KeyValuePair<PSPDEV, List<rresult>> kp in rescol)
               {
                   foreach (rresult re in kp.Value)
                   {
                       if (!fhjg.ContainsKey(re.deviceid.SUID))
                       {
                           fhjg.Add(re.deviceid.SUID, re);
                       }
                       else
                       {
                           rresult r = fhjg[re.deviceid.SUID];
                           r.gzl += re.gzl;
                           r.ntysj += re.ntysj;

                           fhjg[re.deviceid.SUID] = r;

                       }
                   }


               }
               Dictionary<string, rresult> fhjg1 = new Dictionary<string, rresult>();
               foreach (KeyValuePair<string, rresult> kp in fhjg)
               {
                   rresult r = kp.Value;
                   r.tysj = r.ntysj / r.gzl;
                   //重新赋其值
                   fhjg1[kp.Key] = r;
               }

               //输出结果

               int rownum = 0;
               foreach (KeyValuePair<string, rresult> kp in fhjg1)
               {
                   order++;

                   //DataRow row = resulttb.NewRow();
                   //row["fhname"] = kp.Value.deviceid.Name;
                   //row["zbname"] = "λ（次/年）";
                   //row["result"] = kp.Value.gzl;
                   //row["pdfs"] = "方式" + fxtype.ToString();
                   //row["A"] = order;
                   //resulttb.Rows.Add(row);
                   //row = resulttb.NewRow();
                   //row["fhname"] = kp.Value.deviceid.Name;
                   //row["zbname"] = "t（h）";
                   //row["result"] = kp.Value.tysj;
                   //row["pdfs"] = "方式" + fxtype.ToString();
                   //row["A"] = order;
                   //resulttb.Rows.Add(row);
                   //row = resulttb.NewRow();
                   //row["fhname"] = kp.Value.deviceid.Name;
                   //row["zbname"] = "T（h/年）";
                   //row["result"] = kp.Value.ntysj;
                   //row["pdfs"] = "方式" + fxtype.ToString();
                   //row["A"] = order;
                   //resulttb.Rows.Add(row);
                   //写到excel
                   if (columncounts == 0)
                   {
                       ex.SetCellValue(kp.Value.deviceid.Name, 3 + 3 * rownum, 1);

                       ex.UnitCells(3 + 3 * rownum, 1, 5 + 3 * rownum, 1);
                       ex.SetCellValue("λ（次/年）", 3 + 3 * rownum, 2);
                       ex.SetCellValue("t（h）", 4 + 3 * rownum, 2);
                       ex.SetCellValue("T（h/年）", 5 + 3 * rownum, 2);
                       ex.SetFontStyle(3 + 3 * rownum, 1, 5 + 3 * rownum, 1, true, true, ExcelStyle.UnderlineStyle.无下划线);
                       ex.AlignmentCells(3 + 3 * rownum, 1, 5 + 3 * rownum, 1, ExcelStyle.ExcelHAlign.居中, ExcelStyle.ExcelVAlign.居中);
                       ex.CellsBackColor(3 + 3 * rownum, 1, 5 + 3 * rownum, 1, ExcelStyle.ColorIndex.绿色);
                       ex.SetCellValue("方式" + fxtype.ToString(), 2, columncounts + 3);
                       ex.SetFontStyle(2, columncounts + 3, 2, columncounts + 3, true, true, ExcelStyle.UnderlineStyle.无下划线);
                       ex.CellsBackColor(2, columncounts + 3, 2, columncounts + 3, ExcelStyle.ColorIndex.黄色);
                       ex.SetCellValue(kp.Value.gzl.ToString(), 3 + 3 * rownum, columncounts + 3);
                       ex.SetCellValue(kp.Value.tysj.ToString(), 4 + 3 * rownum, columncounts + 3);
                       ex.SetCellValue(kp.Value.ntysj.ToString(), 5 + 3 * rownum, columncounts + 3);
                   }
                   else
                   {
                       ex.SetCellValue("方式" + fxtype.ToString(), 2, columncounts + 3);
                       ex.SetFontStyle(2, columncounts + 3, 2, columncounts + 3, true, true, ExcelStyle.UnderlineStyle.无下划线);
                       ex.CellsBackColor(2, columncounts + 3, 2, columncounts + 3, ExcelStyle.ColorIndex.黄色);
                       ex.SetCellValue(kp.Value.gzl.ToString(), 3 + 3 * rownum, columncounts + 3);
                       ex.SetCellValue(kp.Value.tysj.ToString(), 4 + 3 * rownum, columncounts + 3);
                       ex.SetCellValue(kp.Value.ntysj.ToString(), 5 + 3 * rownum, columncounts + 3);
                   }
                   rownum++;
               }

               //求结果
               double ACI = 0, CID = 0, SAIFI = 0, SAIDI = 0, CAIDI = 0, ASAI = 0, ASUI = 0, ASCI = 0, sumyh = 0;
               foreach (KeyValuePair<string, rresult> kp in fhjg1)
               {
                   ACI += kp.Value.deviceid.Num1 * kp.Value.gzl;
                   sumyh += kp.Value.deviceid.Num1;
                   CID += kp.Value.deviceid.Num1 * kp.Value.ntysj;
                   ASCI += kp.Value.deviceid.HuganTQ4 * kp.Value.ntysj;
               }
               SAIFI = ACI / sumyh;
               SAIDI = CID / sumyh;
               CAIDI = CID / ACI;
               ASAI = ((sumyh * 8760) - 605) / (sumyh * 8760);
               ASUI = 1 - ASAI;
               ASCI = ASCI / sumyh;
               order++;
               //DataRow row1 = resultzbtb.NewRow();

               //row1["zbname"] = "用户全年总停电次数ACI(次/年)";
               //row1["result"] = ACI;
               //row1["pdfs"] = "方式" + fxtype.ToString();
               //row1["A"] = order;
               //resultzbtb.Rows.Add(row1);
               //row1 = resultzbtb.NewRow();

               //row1["zbname"] = "用户总全年停电时间CID（h）";
               //row1["result"] = CID;
               //row1["pdfs"] = "方式" + fxtype.ToString();
               //row1["A"] = order;
               //resultzbtb.Rows.Add(row1);
               //row1 = resultzbtb.NewRow();

               //row1["zbname"] = "系统平均停电频率SAIFI（次/户·年）";
               //row1["result"] =SAIFI;
               //row1["pdfs"] = "方式" + fxtype.ToString();
               //row1["A"] = order;
               //resultzbtb.Rows.Add(row1);

               //row1 = resultzbtb.NewRow();
               //row1["zbname"] = "系统平均停电持续时间SAIDI（h/户·年）";
               //row1["result"] = SAIDI;
               //row1["pdfs"] = "方式" + fxtype.ToString();
               //row1["A"] = order;
               //resultzbtb.Rows.Add(row1);

               //row1 = resultzbtb.NewRow();
               //row1["zbname"] = "用户平均停电时间CAIDI（h/户·年）";
               //row1["result"] = CAIDI;
               //row1["pdfs"] = "方式" + fxtype.ToString();
               //row1["A"] = order;
               //resultzbtb.Rows.Add(row1);

               //row1 = resultzbtb.NewRow();
               //row1["zbname"] = "平均供电可用率ASAI";
               //row1["result"] = ASAI;
               //row1["pdfs"] = "方式" + fxtype.ToString();
               //row1["A"] = order;
               //resultzbtb.Rows.Add(row1);

               //row1 = resultzbtb.NewRow();
               //row1["zbname"] = "平均供电不可用率ASUI";
               //row1["result"] = ASUI;
               //row1["pdfs"] = "方式" + fxtype.ToString();
               //row1["A"] = order;
               //resultzbtb.Rows.Add(row1);

               //row1 = resultzbtb.NewRow();
               //row1["zbname"] = "平均系统缺电指标ASCI";
               //row1["result"] = ASCI;
               //row1["pdfs"] = "方式" + fxtype.ToString();
               //row1["A"] = order;
               //resultzbtb.Rows.Add(row1);

               //写到excel中
               if (columncounts == 0)
               {
                   ex.SetCellValue("系统", 3 + 3 * rownum, 1);
                   ex.UnitCells(3 + 3 * rownum, 1, 10 + 3 * rownum, 1);
                   ex.SetFontStyle(3 + 3 * rownum, 1, 10 + 3 * rownum, 1, true, true, ExcelStyle.UnderlineStyle.无下划线);

                   ex.AlignmentCells(3 + 3 * rownum, 1, 10 + 3 * rownum, 1, ExcelStyle.ExcelHAlign.居中, ExcelStyle.ExcelVAlign.居中);
                   ex.CellsBackColor(3 + 3 * rownum, 1, 10 + 3 * rownum, 1, ExcelStyle.ColorIndex.绿色);
                   ex.SetCellValue("用户全年总停电次数ACI(次/年)", 3 + 3 * rownum, 2);
                   ex.SetCellValue("用户总全年停电时间CID（h）", 4 + 3 * rownum, 2);
                   ex.SetCellValue("系统平均停电频率SAIFI（次/户·年）", 5 + 3 * rownum, 2);
                   ex.SetCellValue("系统平均停电持续时间SAIDI（h/户·年）", 6 + 3 * rownum, 2);
                   ex.SetCellValue("系用户平均停电时间CAIDI（h/户·年）", 7 + 3 * rownum, 2);
                   ex.SetCellValue("平均供电可用率ASAI", 8 + 3 * rownum, 2);
                   ex.SetCellValue("平均供电不可用率ASUI", 9 + 3 * rownum, 2);
                   ex.SetCellValue("平均系统缺电指标ASCI", 10 + 3 * rownum, 2);
                   // ex.SetCellValue("方式" + fxtype.ToString(), 1, columncounts + 2);
                   ex.SetCellValue(ACI.ToString(), 3 + 3 * rownum, columncounts + 3);
                   ex.SetCellValue(CID.ToString(), 4 + 3 * rownum, columncounts + 3);
                   ex.SetCellValue(SAIFI.ToString(), 5 + 3 * rownum, columncounts + 3);
                   ex.SetCellValue(SAIDI.ToString(), 6 + 3 * rownum, columncounts + 3);
                   ex.SetCellValue(CAIDI.ToString(), 7 + 3 * rownum, columncounts + 3);
                   ex.SetCellValue(ASAI.ToString(), 8 + 3 * rownum, columncounts + 3);
                   ex.SetCellValue(ASUI.ToString(), 9 + 3 * rownum, columncounts + 3);
                   ex.SetCellValue(ASCI.ToString(), 10 + 3 * rownum, columncounts + 3);

               }
               else
               {
                   ex.SetCellValue(ACI.ToString(), 3 + 3 * rownum, columncounts + 3);
                   ex.SetCellValue(CID.ToString(), 4 + 3 * rownum, columncounts + 3);
                   ex.SetCellValue(SAIFI.ToString(), 5 + 3 * rownum, columncounts + 3);
                   ex.SetCellValue(SAIDI.ToString(), 6 + 3 * rownum, columncounts + 3);
                   ex.SetCellValue(CAIDI.ToString(), 7 + 3 * rownum, columncounts + 3);
                   ex.SetCellValue(ASAI.ToString(), 8 + 3 * rownum, columncounts + 3);
                   ex.SetCellValue(ASUI.ToString(), 9 + 3 * rownum, columncounts + 3);
                   ex.SetCellValue(ASCI.ToString(), 10 + 3 * rownum, columncounts + 3);
               }
               return true;
           }
            else
           {
               return false;
           }

          

        }

       //对各个线路进行负荷分析
        private bool everyxlanalyst(TreeListNode xl,ref  Dictionary<PSPDEV ,List<rresult>> rescol,int fxtype)
        {
            //对本支路负荷分析
            if (yjfx(xl, ref rescol, fxtype))
            {
                foreach (TreeListNode tln in xl.Nodes)
                {
                    if (tln.GetValue("devicetype").ToString() == "73" && tln.GetValue("S1").ToString() == "1")
                    {
                        //对下一层线路的负荷进行分析
                       if (!everyxlanalyst(tln, ref rescol, fxtype))
                       {
                           return false;
                       }
                        
                    }

                }
                return true;
            }
            else
                return false;
          
        }
        //各个元件对负荷点的影响分析
        private bool yjfx(TreeListNode xl,ref  Dictionary<PSPDEV ,List<rresult>> rescol,int fxtype)
        {
           
            List<yjandjd> xldcol = new List<yjandjd>();  //线路段集合
            List<yjandjd> fhzlcol = new List<yjandjd>();  //负荷支路集合
            List<yjandjd> xlcol = new List<yjandjd>();    //分支线路
            List<yjandjd> luxcol = new List<yjandjd>();//联络线集合
            List<yjandjd> fxlcol = new List<yjandjd>(); //如果有父线路看成 为此线路的首节点 的影响所有的都会受到影响 除非存在备用电源找到第一个线路段的开关
            
            for (int i = 0; i < xl.Nodes.Count;i++ )
            {

                if (xl.Nodes[i].GetValue("devicetype").ToString() == "74" && !xl.Nodes[i].GetValue("title").ToString().Contains("节点有问题"))
                {
                    PSPDEV pd = new PSPDEV();
                    pd.SUID = xl.Nodes[i].GetValue("DeviceID").ToString();
                    pd = Services.BaseService.GetOneByKey<PSPDEV>(pd);
                    if (pd!=null)
                    {
                        string sql = "where SUID='" + pd.IName + "'and AreaID='" + xl.GetValue("DeviceID").ToString() + "'and type='70'";
                        PSPDEV firstnode = UCDeviceBase.DataService.GetObject("SelectPSPDEVByCondition", sql) as PSPDEV;
                        sql = "where SUID='" + pd.JName + "'and AreaID='" + xl.GetValue("DeviceID").ToString() + "'and type='70'";
                        PSPDEV lastnode = UCDeviceBase.DataService.GetObject("SelectPSPDEVByCondition", sql) as PSPDEV;
                        yjandjd jd = new yjandjd(pd, firstnode, lastnode);
                        xldcol.Add(jd);
                        if (firstnode==null)
                        {
                            MessageBox.Show("线路段" + pd.Name + "首节点有问题！请检查后再操作。");
                            return false;
                        }
                        if (lastnode == null)
                        {
                            MessageBox.Show("线路段" + pd.Name + "末节点有问题！请检查后再操作。");
                            return false;
                        }
                    }
                    
                }
                else if (xl.Nodes[i].GetValue("devicetype").ToString() == "80" && !xl.Nodes[i].GetValue("title").ToString().Contains("节点有问题"))
                {
                    PSPDEV pd = new PSPDEV();
                    pd.SUID = xl.Nodes[i].GetValue("DeviceID").ToString();
                    pd = Services.BaseService.GetOneByKey<PSPDEV>(pd);
                    if (pd != null)
                    {
                        string sql = "where SUID='" + pd.IName + "'and AreaID='" + xl.GetValue("DeviceID").ToString() + "'and type='70'";
                        PSPDEV firstnode = UCDeviceBase.DataService.GetObject("SelectPSPDEVByCondition", sql) as PSPDEV;
                       
                        yjandjd jd = new yjandjd(pd, firstnode, null);
                        fhzlcol.Add(jd);
                        if (firstnode == null)
                        {
                            MessageBox.Show("负荷支路" + pd.Name + "所在节点有问题！请检查后再操作。");
                            return false;
                        }
                    }
                }
                else if (xl.Nodes[i].GetValue("devicetype").ToString() == "75" && !xl.Nodes[i].GetValue("title").ToString().Contains("节点有问题"))
                {
                    PSPDEV pd = new PSPDEV();
                    pd.SUID = xl.Nodes[i].GetValue("DeviceID").ToString();
                    pd = Services.BaseService.GetOneByKey<PSPDEV>(pd);
                    if (pd != null)
                    {
                        PSPDEV firstnode=new PSPDEV();
                        if (pd.IName == xl.GetValue("DeviceID").ToString())
                        {
                            string sql = "where SUID='" + pd.HuganLine1 + "'and AreaID='" + xl.GetValue("DeviceID").ToString() + "'and type='70'";
                            firstnode = UCDeviceBase.DataService.GetObject("SelectPSPDEVByCondition", sql) as PSPDEV;
                        }
                        if (pd.JName == xl.GetValue("DeviceID").ToString())
                        {
                            string sql = "where SUID='" + pd.HuganLine2 + "'and AreaID='" + xl.GetValue("DeviceID").ToString() + "'and type='70'";
                            firstnode = UCDeviceBase.DataService.GetObject("SelectPSPDEVByCondition", sql) as PSPDEV;
                        }
                        if (firstnode == null)
                        {
                            MessageBox.Show("联络线" + pd.Name + "节点有问题！请检查后再操作。");
                            return false;
                        }
                       
                        yjandjd jd = new yjandjd(pd, firstnode, null);
                        luxcol.Add(jd);
                    }
                }
                else if (xl.Nodes[i].GetValue("devicetype").ToString() == "73" )
                {
                    PSPDEV pd = new PSPDEV();
                    pd.SUID = xl.Nodes[i].GetValue("DeviceID").ToString();
                    pd = Services.BaseService.GetOneByKey<PSPDEV>(pd);
                    if (pd != null)
                    {
                        string sql = "where SUID='" + pd.HuganLine1 + "'and AreaID='" + xl.GetValue("DeviceID").ToString() + "'and type='70'";
                        PSPDEV firstnode = UCDeviceBase.DataService.GetObject("SelectPSPDEVByCondition", sql) as PSPDEV;

                        yjandjd jd = new yjandjd(pd, firstnode, null);
                        xlcol.Add(jd);
                        if (firstnode == null)
                        {
                            MessageBox.Show("支线路" + pd.Name + "连接父节点有问题！请检查后再操作。");
                            return false;
                        }
                    }
                }


            }
            if (xl.ParentNode.GetValue("devicetype").ToString() == "73")   //存在父支路
            {
                PSPDEV ps = new PSPDEV();
                ps.SUID = xl.ParentNode.GetValue("DeviceID").ToString();
                ps = Services.BaseService.GetOneByKey<PSPDEV>(ps);
                PSPDEV xld=new PSPDEV();
                PSPDEV firstjd=new PSPDEV();
                //  
                for (int i = 0; i < xldcol.Count;i++ )
                {
                    string sql = "where IName='" + xldcol[i].YJ.SUID + "'and (type='06'or type='55')";
                    IList<PSPDEV> listkg = UCDeviceBase.DataService.GetList<PSPDEV>("SelectPSPDEVByCondition", sql);
                    if (listkg.Count>0)
                    {
                        xld = xldcol[i].YJ;
                        firstjd = xldcol[i].FirstNode;
                        yjandjd jd = new yjandjd(ps, listkg[0], firstjd);
                        fxlcol.Add(jd);
                        break;
                    }
                }
              
            }
            //然后依次的分析 
#region  线路段的对负荷的影响

            for (int i = 0; i < xldcol.Count;i++ )
            {
                //判断该线路段是否有开关 如果有则直接进行
                yjandjd ps = xldcol[i];
                List<rresult> listfhtyl=new List<rresult>();   //记录元件对本线路各个负荷的影响
                rresult result=new rresult();
               string sql = "where IName='" + ps.YJ.SUID + "'and (type='06'or type='55')"; 
                IList<PSPDEV> listkg = UCDeviceBase.DataService.GetList<PSPDEV>("SelectPSPDEVByCondition", sql);
               
                if (listkg.Count>0)    //线路段有开关
                {
                  for (int j=0;j<fhzlcol.Count;j++)
                  {
                      if (xldcol[i].FirstNode.Number>=fhzlcol[j].FirstNode.Number)          //负荷支路比起元件更靠近电源  停电方式都是一个样
                      {
                          result.deviceid=fhzlcol[j].YJ;
                          result.gzl=xldcol[i].YJ.HuganTQ3;
                          if (listkg[0].Type=="06")
                          {
                                 result.tysj=listkg[0].HuganTQ2;
                          }
                          else
                             result.tysj=listkg[0].HuganTQ4;
                          result.ntysj=result.gzl*result.tysj; 
                          listfhtyl.Add(result);

                         
                      }
                      else if (xldcol[i].FirstNode.Number<fhzlcol[j].FirstNode.Number)
                      {
                         switch (fxtype)
                          {
                          case 1 :  //第一种方式
                                 result.deviceid=fhzlcol[j].YJ;
                                  result.gzl=xldcol[i].YJ.HuganTQ3;
                                 result.tysj=xldcol[i].YJ.HuganTQ4;
                                 result.ntysj=result.gzl*result.tysj; 
                                 listfhtyl.Add(result);
                            break;
                              case 2:  //第二种方式  判断联络线的节点的编号和firstnode的比较 如果大于它则影响不了前面 如果小于的话
                                    if (fhzlcol[j].FirstNode.Number <= xldcol[i].LastNode.Number)
                                    {
                                        result.deviceid = fhzlcol[j].YJ;
                                        result.gzl = xldcol[i].YJ.HuganTQ3;
                                        result.tysj = xldcol[i].YJ.HuganTQ4;
                                        result.ntysj = result.gzl * result.tysj;
                                        listfhtyl.Add(result);
                                    }
                                    else
                                    {
                                        if (luxcol.Count > 0)
                                        {
                                            if (luxcol[0].FirstNode.Number < xldcol[i].LastNode.Number)
                                            {
                                                result.deviceid = fhzlcol[j].YJ;
                                                result.gzl = xldcol[i].YJ.HuganTQ3;
                                                result.tysj = xldcol[i].YJ.HuganTQ4;
                                                result.ntysj = result.gzl * result.tysj;
                                                listfhtyl.Add(result);
                                            }
                                            else
                                            {
                                                result.deviceid = fhzlcol[j].YJ;
                                                result.gzl = xldcol[i].YJ.HuganTQ3;
                                                if (listkg[0].Type == "06")
                                                {
                                                    result.tysj = listkg[0].HuganTQ2 >= luxcol[0].YJ.HuganTQ3 ? listkg[0].HuganTQ2 : luxcol[0].YJ.HuganTQ3;
                                                }
                                                else
                                                    result.tysj = listkg[0].HuganTQ4 >= luxcol[0].YJ.HuganTQ3 ? listkg[0].HuganTQ4 : luxcol[0].YJ.HuganTQ3;
                                                result.ntysj = result.gzl * result.tysj;
                                                listfhtyl.Add(result);
                                            }
                                        }
                                        else
                                        {
                                            result.deviceid = fhzlcol[j].YJ;
                                            result.gzl = xldcol[i].YJ.HuganTQ3;
                                            if (listkg[0].Type == "06")
                                            {
                                                result.tysj = listkg[0].HuganTQ2;
                                            }
                                            else
                                                result.tysj = listkg[0].HuganTQ4;
                                            result.ntysj = result.gzl * result.tysj;
                                            listfhtyl.Add(result);
                                        }
                                    }
                            
                                  
                                
                                  break;
                               case 3:  //第三种方式  判断联络线的节点的编号和firstnode的比较 如果大于它则影响不了前面 如果小于的话
                                  if (luxcol.Count > 0)
                                  {
                                      if (luxcol[0].FirstNode.Number <= xldcol[i].LastNode.Number)
                                      {
                                          result.deviceid = fhzlcol[j].YJ;
                                          result.gzl = xldcol[i].YJ.HuganTQ3;
                                          result.tysj = xldcol[i].YJ.HuganTQ4;
                                          result.ntysj = result.gzl * result.tysj;
                                          listfhtyl.Add(result);
                                      }
                                      else
                                      {
                                          result.deviceid = fhzlcol[j].YJ;
                                          result.gzl = xldcol[i].YJ.HuganTQ3;
                                          result.tysj = fhzlcol[j].YJ.HuganTQ2 * 2;
                                          result.ntysj = result.gzl * result.tysj;
                                          listfhtyl.Add(result);
                                      }
                                  }
                                  else
                                  {
                                      result.deviceid = fhzlcol[j].YJ;
                                      result.gzl = xldcol[i].YJ.HuganTQ3;
                                      result.tysj = fhzlcol[j].YJ.HuganTQ2 * 2;
                                      result.ntysj = result.gzl * result.tysj;
                                      listfhtyl.Add(result);
                                  }
                                   
                                   
                            
                              break;
                             case 4:       //第四种方式
                                      if (fhzlcol[j].FirstNode.Number <= xldcol[i].LastNode.Number)
                                      {
                                          result.deviceid = fhzlcol[j].YJ;
                                          result.gzl = xldcol[i].YJ.HuganTQ3;
                                          result.tysj = xldcol[i].YJ.HuganTQ4;
                                          result.ntysj = result.gzl * result.tysj;
                                          listfhtyl.Add(result);
                                      }
                                      else
                                      {
                                          if (luxcol.Count > 0)
                                          {
                                              if (luxcol[0].FirstNode.Number < xldcol[i].LastNode.Number)
                                              {
                                                  result.deviceid = fhzlcol[j].YJ;
                                                  result.gzl = xldcol[i].YJ.HuganTQ3;
                                                  result.tysj = xldcol[i].YJ.HuganTQ4;
                                                  result.ntysj = result.gzl * result.tysj;
                                                  listfhtyl.Add(result);
                                              }
                                              else
                                              {
                                                  result.deviceid = fhzlcol[j].YJ;
                                                  result.gzl = xldcol[i].YJ.HuganTQ3;
                                                  if (listkg[0].Type == "06")
                                                  {
                                                      result.tysj = listkg[0].HuganTQ2;
                                                  }
                                                  else
                                                      result.tysj = listkg[0].HuganTQ4;
                                                  result.ntysj = result.gzl * result.tysj;
                                                  listfhtyl.Add(result);
                                              }
                                          }
                                          else
                                          {
                                              result.deviceid = fhzlcol[j].YJ;
                                              result.gzl = xldcol[i].YJ.HuganTQ3;
                                              if (listkg[0].Type == "06")
                                              {
                                                  result.tysj = listkg[0].HuganTQ2;
                                              }
                                              else
                                                  result.tysj = listkg[0].HuganTQ4;
                                              result.ntysj = result.gzl * result.tysj;
                                              listfhtyl.Add(result);
                                          }
                                      }
                                  
                                 break;
                             case 5:      //第五种方式
                                 if (fhzlcol[j].FirstNode.Number <= xldcol[i].LastNode.Number)
                                 {
                                     result.deviceid = fhzlcol[j].YJ;
                                     result.gzl = xldcol[i].YJ.HuganTQ3;
                                     result.tysj = xldcol[i].YJ.HuganTQ4;
                                     result.ntysj = result.gzl * result.tysj;
                                     listfhtyl.Add(result);
                                 }
                                 else
                                 {
                                     if (luxcol.Count > 0)
                                     {
                                         if (luxcol[0].FirstNode.Number < xldcol[i].LastNode.Number)
                                         {
                                             result.deviceid = fhzlcol[j].YJ;
                                             result.gzl = xldcol[i].YJ.HuganTQ3;
                                             result.tysj = xldcol[i].YJ.HuganTQ4;
                                             result.ntysj = result.gzl * result.tysj;
                                             listfhtyl.Add(result);
                                         }
                                         else
                                         {
                                             result.deviceid = fhzlcol[j].YJ;
                                             result.gzl = xldcol[i].YJ.HuganTQ3;
                                             if (listkg[0].Type == "06")
                                             {
                                                 result.tysj = listkg[0].HuganTQ2;
                                             }
                                             else
                                                 result.tysj = listkg[0].HuganTQ4;
                                             result.ntysj = result.gzl * result.tysj;
                                             listfhtyl.Add(result);
                                         }
                                     }
                                     else
                                     {
                                         result.deviceid = fhzlcol[j].YJ;
                                         result.gzl = xldcol[i].YJ.HuganTQ3;
                                         if (listkg[0].Type == "06")
                                         {
                                             result.tysj = listkg[0].HuganTQ2;
                                         }
                                         else
                                             result.tysj = listkg[0].HuganTQ4;
                                         result.ntysj = result.gzl * result.tysj;
                                         listfhtyl.Add(result);
                                     }
                                 }
                                 break;
                          }
                      }
                  }
                }
                else  //如果没有 找到前后线路段具有开关的
                {
                    yjandjd curpsp = xldcol[i];  //当前的线路段
                    yjandjd prepsp = xldcol[i]; //前面的线路段
                    yjandjd lastpsp = xldcol[i]; //后面的线路段
                    IList<PSPDEV> listkg1 = new List<PSPDEV>();  //前段线路段的开关
                    IList<PSPDEV> listkg2 = new List<PSPDEV>();  //后段线路段的开关
                    if (i<xldcol.Count-1)
                    {
                       
                        for (int j = i + 1; j < xldcol.Count; j++)
                        {
                        
                         sql = "where IName='" + xldcol[j].YJ.SUID + "'and (type='06'or type='55')"; 
                        listkg2 = UCDeviceBase.DataService.GetList<PSPDEV>("SelectPSPDEVByCondition", sql);
               
                         if (listkg2.Count>0)    //线路段有开关
                         {
                             lastpsp = xldcol[j];
                             break;
                         }
                        }
                        
                    }
                    if (i>0)
                    {
                        for (int j = 0; j < i;j++ )
                        {
                            sql = "where IName='" + xldcol[j].YJ.SUID + "'and (type='06'or type='55')";
                            listkg1 = UCDeviceBase.DataService.GetList<PSPDEV>("SelectPSPDEVByCondition", sql);

                            if (listkg1.Count > 0)    //线路段有开关
                            {
                                prepsp = xldcol[j];
                                break;
                            }
                        }
                    }
                    //进行几个指标的判断
                    for (int j = 0; j < fhzlcol.Count; j++)
                    {
                        if (fhzlcol[j].FirstNode.Number <= prepsp.FirstNode.Number)          //负荷支路比起元件更靠近电源  停电方式都是一个样
                        {
                            if (prepsp.Equals(curpsp))
                            {
                                result.deviceid = fhzlcol[j].YJ;
                                result.gzl = curpsp.YJ.HuganTQ3;
                                result.tysj = curpsp.YJ.HuganTQ4;
                                result.ntysj = result.gzl * result.tysj;
                                listfhtyl.Add(result);
                            }
                            else
                            {
                                result.deviceid = fhzlcol[j].YJ;
                           
                                result.gzl = curpsp.YJ.HuganTQ3;

                                if (listkg1[0].Type == "06")
                                {
                                    result.tysj = listkg1[0].HuganTQ2;
                                }
                                else
                                    result.tysj = listkg1[0].HuganTQ4;
                                result.ntysj = result.gzl * result.tysj;
                                listfhtyl.Add(result);
                            }
                         


                        }

                        if (prepsp.LastNode.Number <= fhzlcol[j].FirstNode.Number && fhzlcol[j].FirstNode.Number <= lastpsp.FirstNode.Number)  //在中间
                        {
                            result.deviceid = fhzlcol[j].YJ;
                            result.gzl = curpsp.YJ.HuganTQ3;
                            result.tysj = curpsp.YJ.HuganTQ4;
                            result.ntysj = result.gzl * result.tysj;
                            listfhtyl.Add(result);
                        }
                        if (fhzlcol[j].FirstNode.Number>lastpsp.FirstNode.Number)
                        {
                            if (lastpsp.Equals(curpsp))
                            {
                                result.deviceid = fhzlcol[j].YJ;
                                result.gzl = curpsp.YJ.HuganTQ3;
                                result.tysj = curpsp.YJ.HuganTQ4;
                                result.ntysj = result.gzl * result.tysj;
                                listfhtyl.Add(result);
                            }
                            else
                            {
                                switch (fxtype)
                                {
                                    case 1:  //第一种方式
                                        result.deviceid = fhzlcol[j].YJ;
                                        result.gzl = curpsp.YJ.HuganTQ3;
                                        result.tysj = curpsp.YJ.HuganTQ4;
                                        result.ntysj = result.gzl * result.tysj;
                                        listfhtyl.Add(result);
                                        break;
                                    case 2:  //第二种方式  判断联络线的节点的编号和firstnode的比较 如果大于它则影响不了前面 如果小于的话
                                        if (fhzlcol[j].FirstNode.Number <= xldcol[i].LastNode.Number)
                                        {
                                            result.deviceid = fhzlcol[j].YJ;
                                            result.gzl = xldcol[i].YJ.HuganTQ3;
                                            result.tysj = xldcol[i].YJ.HuganTQ4;
                                            result.ntysj = result.gzl * result.tysj;
                                            listfhtyl.Add(result);
                                        }
                                        else
                                        {
                                            if (luxcol.Count > 0)
                                            {
                                                if (luxcol[0].FirstNode.Number < curpsp.LastNode.Number)
                                                {
                                                    result.deviceid = fhzlcol[j].YJ;
                                                    result.gzl = curpsp.YJ.HuganTQ3;
                                                    result.tysj = curpsp.YJ.HuganTQ4;
                                                    result.ntysj = result.gzl * result.tysj;
                                                    listfhtyl.Add(result);
                                                }
                                                else
                                                {
                                                    result.deviceid = fhzlcol[j].YJ;
                                                    result.gzl = curpsp.YJ.HuganTQ3;
                                                    if (listkg2[0].Type == "06")
                                                    {
                                                        result.tysj = listkg2[0].HuganTQ2 >= luxcol[0].YJ.HuganTQ3 ? listkg2[0].HuganTQ2 : luxcol[0].YJ.HuganTQ3;
                                                    }
                                                    else
                                                        result.tysj = listkg2[0].HuganTQ4 >= luxcol[0].YJ.HuganTQ3 ? listkg2[0].HuganTQ4 : luxcol[0].YJ.HuganTQ3;
                                                    result.ntysj = result.gzl * result.tysj;
                                                    listfhtyl.Add(result);
                                                }
                                            }
                                            else
                                            {
                                                result.deviceid = fhzlcol[j].YJ;
                                                result.gzl = curpsp.YJ.HuganTQ3;
                                                if (listkg2[0].Type == "06")
                                                {
                                                    result.tysj = listkg2[0].HuganTQ2;
                                                }
                                                else
                                                    result.tysj = listkg2[0].HuganTQ4;
                                                result.ntysj = result.gzl * result.tysj;
                                                listfhtyl.Add(result);
                                            }

                                        }
                                        

                                        break;
                                    case 3:  //第三种方式  判断联络线的节点的编号和firstnode的比较 如果大于它则影响不了前面 如果小于的话
                                        if (luxcol.Count>0)
                                        {
                                            if (luxcol[0].FirstNode.Number <= curpsp.LastNode.Number)
                                            {
                                                result.deviceid = fhzlcol[j].YJ;
                                                result.gzl = curpsp.YJ.HuganTQ3;
                                                result.tysj = curpsp.YJ.HuganTQ4;
                                                result.ntysj = result.gzl * result.tysj;
                                                listfhtyl.Add(result);
                                            }
                                            else
                                            {
                                                result.deviceid = fhzlcol[j].YJ;
                                                result.gzl = curpsp.YJ.HuganTQ3;
                                                result.tysj = fhzlcol[j].YJ.HuganTQ2 * 2;
                                                result.ntysj = result.gzl * result.tysj;
                                                listfhtyl.Add(result);
                                            }
                                        }
                                        else
                                        {
                                            result.deviceid = fhzlcol[j].YJ;
                                            result.gzl = curpsp.YJ.HuganTQ3;
                                            result.tysj = fhzlcol[j].YJ.HuganTQ2 * 2;
                                            result.ntysj = result.gzl * result.tysj;
                                            listfhtyl.Add(result);
                                        }
                                       

                                        break;
                                    case 4:       //第四种方式
                                        if (fhzlcol[j].FirstNode.Number <= curpsp.LastNode.Number)
                                        {
                                            result.deviceid = fhzlcol[j].YJ;
                                            result.gzl = curpsp.YJ.HuganTQ3;
                                            result.tysj = curpsp.YJ.HuganTQ4;
                                            result.ntysj = result.gzl * result.tysj;
                                            listfhtyl.Add(result);
                                        }
                                        else
                                        {
                                            if (luxcol.Count > 0)
                                            {
                                                if (luxcol[0].FirstNode.Number < curpsp.LastNode.Number)
                                                {
                                                    result.deviceid = fhzlcol[j].YJ;
                                                    result.gzl = curpsp.YJ.HuganTQ3;
                                                    result.tysj = curpsp.YJ.HuganTQ4;
                                                    result.ntysj = result.gzl * result.tysj;
                                                    listfhtyl.Add(result);
                                                }
                                                else
                                                {
                                                    result.deviceid = fhzlcol[j].YJ;
                                                    result.gzl = xldcol[i].YJ.HuganTQ3;
                                                    if (listkg2[0].Type == "06")
                                                    {
                                                        result.tysj = listkg2[0].HuganTQ2;
                                                    }
                                                    else
                                                        result.tysj = listkg2[0].HuganTQ4;
                                                    result.ntysj = result.gzl * result.tysj;
                                                    listfhtyl.Add(result);
                                                }
                                            }
                                            else
                                            {
                                                result.deviceid = fhzlcol[j].YJ;
                                                result.gzl = curpsp.YJ.HuganTQ3;
                                                if (listkg2[0].Type == "06")
                                                {
                                                    result.tysj = listkg2[0].HuganTQ2;
                                                }
                                                else
                                                    result.tysj = listkg2[0].HuganTQ4;
                                                result.ntysj = result.gzl * result.tysj;
                                                listfhtyl.Add(result);
                                            }
                                        }
                                       
                                        break;
                                    case 5:      //第五种方式
                                        if (fhzlcol[j].FirstNode.Number <= curpsp.LastNode.Number)
                                        {
                                            result.deviceid = fhzlcol[j].YJ;
                                            result.gzl = curpsp.YJ.HuganTQ3;
                                            result.tysj = curpsp.YJ.HuganTQ4;
                                            result.ntysj = result.gzl * result.tysj;
                                            listfhtyl.Add(result);
                                        }
                                        else
                                        {
                                            if (luxcol.Count > 0)
                                            {
                                                if (luxcol[0].FirstNode.Number < curpsp.LastNode.Number)
                                                {
                                                    result.deviceid = fhzlcol[j].YJ;
                                                    result.gzl = curpsp.YJ.HuganTQ3;
                                                    result.tysj = curpsp.YJ.HuganTQ4;
                                                    result.ntysj = result.gzl * result.tysj;
                                                    listfhtyl.Add(result);
                                                }
                                                else
                                                {
                                                    result.deviceid = fhzlcol[j].YJ;
                                                    result.gzl = xldcol[i].YJ.HuganTQ3;
                                                    if (listkg2[0].Type == "06")
                                                    {
                                                        result.tysj = listkg2[0].HuganTQ2;
                                                    }
                                                    else
                                                        result.tysj = listkg2[0].HuganTQ4;
                                                    result.ntysj = result.gzl * result.tysj;
                                                    listfhtyl.Add(result);
                                                }
                                            }
                                            else
                                            {
                                                result.deviceid = fhzlcol[j].YJ;
                                                result.gzl = curpsp.YJ.HuganTQ3;
                                                if (listkg2[0].Type == "06")
                                                {
                                                    result.tysj = listkg2[0].HuganTQ2;
                                                }
                                                else
                                                    result.tysj = listkg2[0].HuganTQ4;
                                                result.ntysj = result.gzl * result.tysj;
                                                listfhtyl.Add(result);
                                            }
                                        }
                                        break;
                                }
                            }
                        }
                    
                    }
                    
                    
                }
                rescol[xldcol[i].YJ] = listfhtyl;  //添加到容器中

            }
#endregion
#region   负荷支路对负荷的影响
            for (int i = 0; i < fhzlcol.Count;i++ )
            {
                List<rresult> listfhtyl = new List<rresult>();   //记录元件对本线路各个负荷的影响
                rresult result = new rresult();
                PSPDEV ps=fhzlcol[i].YJ;
                //找到负荷支路前面的线路段和后面的线路段
               
                int prenum = 0;
                int lastnum = 0;
                yjandjd prexld = new yjandjd();
                yjandjd lastxld =new yjandjd();
                for (int j = 0; j < xldcol.Count;j++ )
                {
                    if (xldcol[j].YJ.IName == fhzlcol[i].YJ.IName && xldcol[j].YJ.AreaID == fhzlcol[i].YJ.AreaID)
                    {
                        lastxld = xldcol[j];
                        lastnum =lastxld.FirstNode.Number;
                        continue;
                    }
                    if (xldcol[j].YJ.JName == fhzlcol[i].YJ.IName && xldcol[j].YJ.AreaID == fhzlcol[i].YJ.AreaID)
                    {
                        prexld = xldcol[j];
                        prenum = prexld.LastNode.Number;
                        continue;
                    }

                }
                if (prenum==xldcol.Count)
                {
                    lastxld = prexld;
                }
                //需找前后第一个带隔离开关的线路段
                IList<PSPDEV> listkg1 = new List<PSPDEV>();  //前段线路段的开关
                IList<PSPDEV> listkg2 = new List<PSPDEV>();  //后段线路段的开关
               string sql = "where IName='" + prexld.YJ.SUID + "'and (type='06'or type='55')";
                listkg1 = UCDeviceBase.DataService.GetList<PSPDEV>("SelectPSPDEVByCondition", sql);
                if (listkg1.Count==0)
                {
                    for (int j = 0; j< prenum;j++ )
                    {
                        sql = "where IName='" + xldcol[j].YJ.SUID + "'and (type='06'or type='55')";
                        listkg1 = UCDeviceBase.DataService.GetList<PSPDEV>("SelectPSPDEVByCondition", sql);

                        if (listkg1.Count > 0)    //线路段有开关
                        {
                            prexld = xldcol[j];
                            break;
                        }
                    }
                }
                sql = "where IName='" + lastxld.YJ.SUID + "'and (type='06'or type='55')";
                listkg2= UCDeviceBase.DataService.GetList<PSPDEV>("SelectPSPDEVByCondition", sql);
                if (listkg2.Count == 0)
                {
                    for (int j = lastnum; j < xldcol.Count; j++)
                    {
                        sql = "where IName='" + xldcol[j].YJ.SUID + "'and (type='06'or type='55')";
                        listkg2 = UCDeviceBase.DataService.GetList<PSPDEV>("SelectPSPDEVByCondition", sql);

                        if (listkg2.Count > 0)    //线路段有开关
                        {
                            lastxld = xldcol[j];
                            break;
                        }
                    }
                }
               

                for (int j = 0; j < fhzlcol.Count;j++ )
                {
                    if (i==j)   //自己本身的负荷
                    {
                        result.deviceid = fhzlcol[j].YJ;
                        result.gzl = fhzlcol[i].YJ.HuganTQ1;
                        result.tysj = fhzlcol[i].YJ.HuganTQ2;
                        result.ntysj = result.gzl * result.tysj;
                        listfhtyl.Add(result);

                    }
                    else
                    {
                        
                            switch (fxtype)   //一二三种类型不会影响其他负荷支路 所以不用计算
                            {
                         
                                
                                case 4:       //第四种方式  死接在负荷支路上
                                    if (listkg1.Count>0)
                                    {
                                        
                                      
                                            if (fhzlcol[j].FirstNode.Number<=prexld.FirstNode.Number)
                                           {
                                                 result.deviceid = fhzlcol[j].YJ;
                                                result.gzl = fhzlcol[i].YJ.HuganTQ1;
                                                if (listkg1[0].Type == "06")
                                                {
                                                    result.tysj = listkg1[0].HuganTQ2;
                                                }
                                                else
                                                    result.tysj = listkg1[0].HuganTQ4;
                                                result.ntysj = result.gzl * result.tysj;
                                                listfhtyl.Add(result);
                                            }

                                           if (fhzlcol[j].FirstNode.Number<=fhzlcol[i].FirstNode.Number&&fhzlcol[j].FirstNode.Number>prexld.FirstNode.Number)
                                           {
                                                 result.deviceid = fhzlcol[j].YJ;
                                                result.gzl = fhzlcol[i].YJ.HuganTQ1;
                                             
                                                result.tysj = fhzlcol[i].YJ.HuganTQ2;
                                                result.ntysj = result.gzl * result.tysj;
                                                listfhtyl.Add(result);

                                           }
                                   
                                        
                                    }
                                    else
                                    {
                                       if (fhzlcol[j].FirstNode.Number<=fhzlcol[i].FirstNode.Number)
                                       {
                                                result.deviceid = fhzlcol[j].YJ;
                                                result.gzl = fhzlcol[i].YJ.HuganTQ1;
                                             
                                                result.tysj = fhzlcol[i].YJ.HuganTQ2;
                                                result.ntysj = result.gzl * result.tysj;
                                                listfhtyl.Add(result);
                                       }
                                    }
                                    if (listkg2.Count>0)
                                    {
                                          if (fhzlcol[j].FirstNode.Number>=lastxld.LastNode.Number&&!lastxld.Equals(prexld))
                                           {
                                                 result.deviceid = fhzlcol[j].YJ;
                                                result.gzl = fhzlcol[i].YJ.HuganTQ1;
                                                if (listkg2[0].Type == "06")
                                                {
                                                    result.tysj = listkg2[0].HuganTQ2;
                                                }
                                                else
                                                    result.tysj = listkg2[0].HuganTQ4;
                                                result.ntysj = result.gzl * result.tysj;
                                                listfhtyl.Add(result);
                                            }

                                          if (fhzlcol[j].FirstNode.Number >= fhzlcol[i].FirstNode.Number && fhzlcol[j].FirstNode.Number < lastxld.LastNode.Number && !lastxld.Equals(prexld))
                                           {
                                                 result.deviceid = fhzlcol[j].YJ;
                                                result.gzl = fhzlcol[i].YJ.HuganTQ1;
                                             
                                                result.tysj = fhzlcol[i].YJ.HuganTQ2;
                                                result.ntysj = result.gzl * result.tysj;
                                                listfhtyl.Add(result);

                                           }
                                    }
                                    else
                                    {
                                         if (fhzlcol[j].FirstNode.Number>fhzlcol[i].FirstNode.Number)
                                       {
                                                result.deviceid = fhzlcol[j].YJ;
                                                result.gzl = fhzlcol[i].YJ.HuganTQ1;
                                             
                                                result.tysj = fhzlcol[i].YJ.HuganTQ2;
                                                result.ntysj = result.gzl * result.tysj;
                                                listfhtyl.Add(result);
                                       }
                                    }
                                    break;
                                case 5:      //第五种方式
                                    if (listkg1.Count > 0)
                                    {


                                        if (fhzlcol[j].FirstNode.Number <= prexld.FirstNode.Number)
                                        {
                                            result.deviceid = fhzlcol[j].YJ;
                                            result.gzl = fhzlcol[i].YJ.HuganTQ1*0.1;
                                            if (listkg1[0].Type == "06")
                                            {
                                                result.tysj = listkg1[0].HuganTQ2;
                                            }
                                            else
                                                result.tysj = listkg1[0].HuganTQ4;
                                            result.ntysj = result.gzl * result.tysj;
                                            listfhtyl.Add(result);
                                        }

                                        if (fhzlcol[j].FirstNode.Number <= fhzlcol[i].FirstNode.Number && fhzlcol[j].FirstNode.Number > prexld.FirstNode.Number)
                                        {
                                            result.deviceid = fhzlcol[j].YJ;
                                            result.gzl = fhzlcol[i].YJ.HuganTQ1;

                                            result.tysj = fhzlcol[i].YJ.HuganTQ2;
                                            result.ntysj = result.gzl * result.tysj;
                                            listfhtyl.Add(result);

                                        }


                                    }
                                    else
                                    {
                                        if (fhzlcol[j].FirstNode.Number <= fhzlcol[i].FirstNode.Number)
                                        {
                                            result.deviceid = fhzlcol[j].YJ;
                                            result.gzl = fhzlcol[i].YJ.HuganTQ1;

                                            result.tysj = fhzlcol[i].YJ.HuganTQ2;
                                            result.ntysj = result.gzl * result.tysj;
                                            listfhtyl.Add(result);
                                        }
                                    }
                                    if (listkg2.Count > 0)
                                    {
                                        if (fhzlcol[j].FirstNode.Number >= lastxld.LastNode.Number && !lastxld.Equals(prexld))
                                        {
                                            result.deviceid = fhzlcol[j].YJ;
                                            result.gzl = fhzlcol[i].YJ.HuganTQ1*0.1;
                                            if (listkg2[0].Type == "06")
                                            {
                                                result.tysj = listkg2[0].HuganTQ2;
                                            }
                                            else
                                                result.tysj = listkg2[0].HuganTQ4;
                                            result.ntysj = result.gzl * result.tysj;
                                            listfhtyl.Add(result);
                                        }

                                        if (fhzlcol[j].FirstNode.Number >= fhzlcol[i].FirstNode.Number && fhzlcol[j].FirstNode.Number < lastxld.LastNode.Number && !lastxld.Equals(prexld))
                                        {
                                            result.deviceid = fhzlcol[j].YJ;
                                            result.gzl = fhzlcol[i].YJ.HuganTQ1;

                                            result.tysj = fhzlcol[i].YJ.HuganTQ2;
                                            result.ntysj = result.gzl * result.tysj;
                                            listfhtyl.Add(result);

                                        }
                                    }
                                    else
                                    {
                                        if (fhzlcol[j].FirstNode.Number > fhzlcol[i].FirstNode.Number)
                                        {
                                            result.deviceid = fhzlcol[j].YJ;
                                            result.gzl = fhzlcol[i].YJ.HuganTQ1;

                                            result.tysj = fhzlcol[i].YJ.HuganTQ2;
                                            result.ntysj = result.gzl * result.tysj;
                                            listfhtyl.Add(result);
                                        }
                                    }
                                    break;
                            }
                       
                    }
                }
                rescol[fhzlcol[i].YJ] = listfhtyl;  //添加到容器中
            }
#endregion
#region //如果存在支路的话要看做一个元件来等值化处理
            for (int i = 0; i < xlcol.Count; i++)
            {
                List<rresult> listfhtyl = new List<rresult>();   //记录元件对本线路各个负荷的影响
                rresult result = new rresult();
                PSPDEV ps = xlcol[i].YJ;
                TreeListNode tln = null;
                  for (int j= 0; j < xl.Nodes.Count;j++ )
                  {
                      if (xl.Nodes[j].GetValue("DeviceID").ToString()==ps.SUID)
                      {
                          tln = xl.Nodes[j];
                          break;
                      }
                      
                  }
               
                //找到负荷支路前面的线路段和后面的线路段

                int prenum = 0;
                int lastnum = 0;
                yjandjd prexld = new yjandjd();
                yjandjd lastxld = new yjandjd();
                for (int j = 0; j < xldcol.Count; j++)
                {
                    if (xldcol[j].YJ.IName == xlcol[i].YJ.HuganLine1 && xldcol[j].YJ.AreaID == xlcol[i].YJ.JName)
                    {
                        lastxld = xldcol[j];
                        lastnum = lastxld.FirstNode.Number;
                        continue;
                    }
                    if (xldcol[j].YJ.JName == xlcol[i].YJ.HuganLine1 && xldcol[j].YJ.AreaID == xlcol[i].YJ.JName)
                    {
                        prexld = xldcol[j];
                        prenum = prexld.LastNode.Number;
                        continue;
                    }

                }
               if (prenum==xldcol.Count)
               {
                   lastxld = prexld;
               }
                //需找前后第一个带隔离开关的线路段
                IList<PSPDEV> listkg1 = new List<PSPDEV>();  //前段线路段的开关
                IList<PSPDEV> listkg2 = new List<PSPDEV>();  //后段线路段的开关
                string sql = "where IName='" + prexld.YJ.SUID + "'and (type='06'or type='55')";
                listkg1 = UCDeviceBase.DataService.GetList<PSPDEV>("SelectPSPDEVByCondition", sql);
                if (listkg1.Count == 0)
                {
                    for (int j = 0; j < prenum; j++)
                    {
                        sql = "where IName='" + xldcol[j].YJ.SUID + "'and (type='06'or type='55')";
                        listkg1 = UCDeviceBase.DataService.GetList<PSPDEV>("SelectPSPDEVByCondition", sql);

                        if (listkg1.Count > 0)    //线路段有开关
                        {
                            prexld = xldcol[j];
                            break;
                        }
                    }
                }
                sql = "where IName='" + lastxld.YJ.SUID + "'and (type='06'or type='55')";
                listkg2 = UCDeviceBase.DataService.GetList<PSPDEV>("SelectPSPDEVByCondition", sql);
                if (listkg1.Count == 0)
                {
                    for (int j = lastnum; j < xldcol.Count; j++)
                    {
                        sql = "where IName='" + xldcol[j].YJ.SUID + "'and (type='06'or type='55')";
                        listkg2 = UCDeviceBase.DataService.GetList<PSPDEV>("SelectPSPDEVByCondition", sql);

                        if (listkg2.Count > 0)    //线路段有开关
                        {
                            lastxld = xldcol[j];
                            break;
                        }
                    }
                }
                //进行判断参照负荷的过程
                for (int j = 0; j < fhzlcol.Count;j++ )
                {
                 switch(fxtype)
                 {
                     case 1:
                         if (fhzlcol[j].FirstNode.Number<=prexld.FirstNode.Number)
                         {
                             if (listkg1.Count>0)   //存在开关
                             {
                                 result.deviceid = fhzlcol[j].YJ;
                                 result.gzl =Convert.ToDouble(tln.GetValue("D1")) ;
                                 if (listkg1[0].Type == "06")
                                 {
                                     result.tysj = listkg1[0].HuganTQ2;
                                 }
                                 else
                                     result.tysj = listkg1[0].HuganTQ4;
                                 result.ntysj = result.gzl * result.tysj;
                                 listfhtyl.Add(result);
                             }
                             else
                             {
                                 result.deviceid = fhzlcol[j].YJ;
                                 result.gzl = Convert.ToDouble(tln.GetValue("D1"));
                                 result.tysj = Convert.ToDouble(tln.GetValue("D2"));
                                
                                 result.ntysj = result.gzl * result.tysj;
                                 listfhtyl.Add(result);
                             }
                         }
                         else
                         {
                             result.deviceid = fhzlcol[j].YJ;
                             result.gzl = Convert.ToDouble(tln.GetValue("D1"));
                             result.tysj = Convert.ToDouble(tln.GetValue("D2"));

                             result.ntysj = result.gzl * result.tysj;
                             listfhtyl.Add(result);
                         }
                         break;
                     case 2:
                         if (fhzlcol[j].FirstNode.Number <= prexld.FirstNode.Number)
                         {
                             if (listkg1.Count > 0)   //存在开关
                             {
                                 result.deviceid = fhzlcol[j].YJ;
                                 result.gzl = Convert.ToDouble(tln.GetValue("D1"));
                                 if (listkg1[0].Type == "06")
                                 {
                                     result.tysj = listkg1[0].HuganTQ2;
                                 }
                                 else
                                     result.tysj = listkg1[0].HuganTQ4;
                                 result.ntysj = result.gzl * result.tysj;
                                 listfhtyl.Add(result);
                             }
                             else
                             {
                                 result.deviceid = fhzlcol[j].YJ;
                                 result.gzl = Convert.ToDouble(tln.GetValue("D1"));
                                 result.tysj = Convert.ToDouble(tln.GetValue("D2"));

                                 result.ntysj = result.gzl * result.tysj;
                                 listfhtyl.Add(result);
                             }
                         }
                         if (fhzlcol[j].FirstNode.Number > prexld.FirstNode.Number && fhzlcol[j].FirstNode.Number <= lastxld.FirstNode.Number && !lastxld.Equals(prexld))
                         {
                             result.deviceid = fhzlcol[j].YJ;
                             result.gzl = Convert.ToDouble(tln.GetValue("D1"));
                             result.tysj = Convert.ToDouble(tln.GetValue("D2"));

                             result.ntysj = result.gzl * result.tysj;
                             listfhtyl.Add(result);
                         }
                         if (fhzlcol[j].FirstNode.Number >= lastxld.FirstNode.Number && !lastxld.Equals(prexld))
                         {
                             if (listkg2.Count>0)
                             {
                                 if (luxcol.Count>0)
                                 {
                                     if (luxcol[0].FirstNode.Number < lastxld.FirstNode.Number )
                                     {
                                         result.deviceid = fhzlcol[j].YJ;
                                         result.gzl = Convert.ToDouble(tln.GetValue("D1"));
                                         result.tysj = Convert.ToDouble(tln.GetValue("D2"));

                                         result.ntysj = result.gzl * result.tysj;
                                         listfhtyl.Add(result);
                                     }
                                     else
                                     {
                                         result.deviceid = fhzlcol[j].YJ;
                                         result.gzl = Convert.ToDouble(tln.GetValue("D1"));
                                         if (listkg2[0].Type == "06")
                                         {
                                             result.tysj = listkg2[0].HuganTQ2 >= luxcol[0].YJ.HuganTQ3 ? listkg2[0].HuganTQ2 : luxcol[0].YJ.HuganTQ3;
                                         }
                                         else
                                             result.tysj = listkg2[0].HuganTQ4 >= luxcol[0].YJ.HuganTQ3 ? listkg2[0].HuganTQ4 : luxcol[0].YJ.HuganTQ3;
                                         result.ntysj = result.gzl * result.tysj;
                                         listfhtyl.Add(result);
                                     }
                                 }
                                 else
                                 {
                                     result.deviceid = fhzlcol[j].YJ;
                                     result.gzl = Convert.ToDouble(tln.GetValue("D1"));
                                     if (listkg2[0].Type == "06")
                                     {
                                         result.tysj = listkg2[0].HuganTQ2;
                                     }
                                     else
                                         result.tysj = listkg2[0].HuganTQ4;
                                     result.ntysj = result.gzl * result.tysj;
                                     listfhtyl.Add(result);
                                 }
                                
                             }
                             else
                             {
                                 result.deviceid = fhzlcol[j].YJ;
                                 result.gzl = Convert.ToDouble(tln.GetValue("D1"));
                                 result.tysj = Convert.ToDouble(tln.GetValue("D2"));

                                 result.ntysj = result.gzl * result.tysj;
                                 listfhtyl.Add(result);
                             }
                         }
                      
                         break;
                     case 3:
                         if (fhzlcol[j].FirstNode.Number<=prexld.FirstNode.Number)
                         {
                             if (listkg1.Count>0)
                             {
                                 result.deviceid = fhzlcol[j].YJ;
                                 result.gzl = Convert.ToDouble(tln.GetValue("D1"));
                                 if (listkg1[0].Type == "06")
                                 {
                                     result.tysj = listkg1[0].HuganTQ2;
                                 }
                                 else
                                     result.tysj = listkg1[0].HuganTQ4;
                                 result.ntysj = result.gzl * result.tysj;
                                 listfhtyl.Add(result);
                             }
                             else
                             {
                                 result.deviceid = fhzlcol[j].YJ;
                                 result.gzl = Convert.ToDouble(tln.GetValue("D1"));
                                 result.tysj = Convert.ToDouble(tln.GetValue("D2"));

                                 result.ntysj = result.gzl * result.tysj;
                                 listfhtyl.Add(result);
                             }
                         }
                         if (fhzlcol[j].FirstNode.Number > prexld.FirstNode.Number && fhzlcol[j].FirstNode.Number <= lastxld.FirstNode.Number && !lastxld.Equals(prexld))
                         {
                             result.deviceid = fhzlcol[j].YJ;
                             result.gzl = Convert.ToDouble(tln.GetValue("D1"));
                             result.tysj = Convert.ToDouble(tln.GetValue("D2"));

                             result.ntysj = result.gzl * result.tysj;
                             listfhtyl.Add(result);
                         }
                         if (fhzlcol[j].FirstNode.Number > lastxld.FirstNode.Number && !lastxld.Equals(prexld))
                         {
                             if (listkg2.Count>0)
                             {
                                 result.deviceid = fhzlcol[j].YJ;
                                 result.gzl = fhzlcol[j].YJ .HuganTQ2*2;
                                 result.tysj = Convert.ToDouble(tln.GetValue("D2"));

                                 result.ntysj = result.gzl * result.tysj;
                                 listfhtyl.Add(result);
                             }
                             else
                             {
                                 result.deviceid = fhzlcol[j].YJ;
                                 result.gzl = Convert.ToDouble(tln.GetValue("D1"));
                                 result.tysj = Convert.ToDouble(tln.GetValue("D2"));

                                 result.ntysj = result.gzl * result.tysj;
                                 listfhtyl.Add(result);
                             }
                         }
                         break;
                     case 4:
                         if (fhzlcol[j].FirstNode.Number <= prexld.FirstNode.Number)
                         {
                             if (listkg1.Count > 0)   //存在开关
                             {
                                 result.deviceid = fhzlcol[j].YJ;
                                 result.gzl = Convert.ToDouble(tln.GetValue("D1"));
                                 if (listkg1[0].Type == "06")
                                 {
                                     result.tysj = listkg1[0].HuganTQ2;
                                 }
                                 else
                                     result.tysj = listkg1[0].HuganTQ4;
                                 result.ntysj = result.gzl * result.tysj;
                                 listfhtyl.Add(result);
                             }
                             else
                             {
                                 result.deviceid = fhzlcol[j].YJ;
                                 result.gzl = Convert.ToDouble(tln.GetValue("D1"));
                                 result.tysj = Convert.ToDouble(tln.GetValue("D2"));

                                 result.ntysj = result.gzl * result.tysj;
                                 listfhtyl.Add(result);
                             }
                         }
                         if (fhzlcol[j].FirstNode.Number > prexld.FirstNode.Number && fhzlcol[j].FirstNode.Number <= lastxld.FirstNode.Number && !lastxld.Equals(prexld))
                         {
                             result.deviceid = fhzlcol[j].YJ;
                             result.gzl = Convert.ToDouble(tln.GetValue("D1"));
                             result.tysj = Convert.ToDouble(tln.GetValue("D2"));

                             result.ntysj = result.gzl * result.tysj;
                             listfhtyl.Add(result);
                         }
                         if (fhzlcol[j].FirstNode.Number >= lastxld.FirstNode.Number && !lastxld.Equals(prexld))
                         {
                             if (listkg2.Count > 0)
                             {
                                 if (luxcol.Count > 0)
                                 {
                                     if (luxcol[0].FirstNode.Number < lastxld.FirstNode.Number)
                                     {
                                         result.deviceid = fhzlcol[j].YJ;
                                         result.gzl = Convert.ToDouble(tln.GetValue("D1"));
                                         result.tysj = Convert.ToDouble(tln.GetValue("D2"));

                                         result.ntysj = result.gzl * result.tysj;
                                         listfhtyl.Add(result);
                                     }
                                     else
                                     {
                                         result.deviceid = fhzlcol[j].YJ;
                                         result.gzl = Convert.ToDouble(tln.GetValue("D1"));
                                         if (listkg2[0].Type == "06")
                                         {
                                             result.tysj = listkg2[0].HuganTQ2 >= luxcol[0].YJ.HuganTQ3 ? listkg2[0].HuganTQ2 : luxcol[0].YJ.HuganTQ3;
                                         }
                                         else
                                             result.tysj = listkg2[0].HuganTQ4 >= luxcol[0].YJ.HuganTQ3 ? listkg2[0].HuganTQ4 : luxcol[0].YJ.HuganTQ3;
                                         result.ntysj = result.gzl * result.tysj;
                                         listfhtyl.Add(result);
                                     }
                                 }
                                 else
                                 {
                                     result.deviceid = fhzlcol[j].YJ;
                                     result.gzl = Convert.ToDouble(tln.GetValue("D1"));
                                     if (listkg2[0].Type == "06")
                                     {
                                         result.tysj = listkg2[0].HuganTQ2;
                                     }
                                     else
                                         result.tysj = listkg2[0].HuganTQ4;
                                     result.ntysj = result.gzl * result.tysj;
                                     listfhtyl.Add(result);
                                 }

                             }
                             else
                             {
                                 result.deviceid = fhzlcol[j].YJ;
                                 result.gzl = Convert.ToDouble(tln.GetValue("D1"));
                                 result.tysj = Convert.ToDouble(tln.GetValue("D2"));

                                 result.ntysj = result.gzl * result.tysj;
                                 listfhtyl.Add(result);
                             }
                         }
                         break;
                     case 5:
                         if (fhzlcol[j].FirstNode.Number <= prexld.FirstNode.Number)
                         {
                             if (listkg1.Count > 0)   //存在开关
                             {
                                 result.deviceid = fhzlcol[j].YJ;
                                 result.gzl = Convert.ToDouble(tln.GetValue("D1"));
                                 if (listkg1[0].Type == "06")
                                 {
                                     result.tysj = listkg1[0].HuganTQ2;
                                 }
                                 else
                                     result.tysj = listkg1[0].HuganTQ4;
                                 result.ntysj = result.gzl * result.tysj;
                                 listfhtyl.Add(result);
                             }
                             else
                             {
                                 result.deviceid = fhzlcol[j].YJ;
                                 result.gzl = Convert.ToDouble(tln.GetValue("D1"));
                                 result.tysj = Convert.ToDouble(tln.GetValue("D2"));

                                 result.ntysj = result.gzl * result.tysj;
                                 listfhtyl.Add(result);
                             }
                         }
                         if (fhzlcol[j].FirstNode.Number > prexld.FirstNode.Number && fhzlcol[j].FirstNode.Number <= lastxld.FirstNode.Number && !lastxld.Equals(prexld))
                         {
                             result.deviceid = fhzlcol[j].YJ;
                             result.gzl = Convert.ToDouble(tln.GetValue("D1"));
                             result.tysj = Convert.ToDouble(tln.GetValue("D2"));

                             result.ntysj = result.gzl * result.tysj;
                             listfhtyl.Add(result);
                         }
                         if (fhzlcol[j].FirstNode.Number >= lastxld.FirstNode.Number && !lastxld.Equals(prexld))
                         {
                             if (listkg2.Count > 0)
                             {
                                 if (luxcol.Count > 0)
                                 {
                                     if (luxcol[0].FirstNode.Number < lastxld.FirstNode.Number)
                                     {
                                         result.deviceid = fhzlcol[j].YJ;
                                         result.gzl = Convert.ToDouble(tln.GetValue("D1"));
                                         result.tysj = Convert.ToDouble(tln.GetValue("D2"));

                                         result.ntysj = result.gzl * result.tysj;
                                         listfhtyl.Add(result);
                                     }
                                     else
                                     {
                                         result.deviceid = fhzlcol[j].YJ;
                                         result.gzl = Convert.ToDouble(tln.GetValue("D1"));
                                         if (listkg2[0].Type == "06")
                                         {
                                             result.tysj = listkg2[0].HuganTQ2 >= luxcol[0].YJ.HuganTQ3 ? listkg2[0].HuganTQ2 : luxcol[0].YJ.HuganTQ3;
                                         }
                                         else
                                             result.tysj = listkg2[0].HuganTQ4>= luxcol[0].YJ.HuganTQ3 ? listkg2[0].HuganTQ4 : luxcol[0].YJ.HuganTQ3;
                                         result.ntysj = result.gzl * result.tysj;
                                         listfhtyl.Add(result);
                                     }
                                 }
                                 else
                                 {
                                     result.deviceid = fhzlcol[j].YJ;
                                     result.gzl = Convert.ToDouble(tln.GetValue("D1"));
                                     if (listkg2[0].Type == "06")
                                     {
                                         result.tysj = listkg2[0].HuganTQ2;
                                     }
                                     else
                                         result.tysj = listkg2[0].HuganTQ4;
                                     result.ntysj = result.gzl * result.tysj;
                                     listfhtyl.Add(result);
                                 }

                             }
                             else
                             {
                                 result.deviceid = fhzlcol[j].YJ;
                                 result.gzl = Convert.ToDouble(tln.GetValue("D1"));
                                 result.tysj = Convert.ToDouble(tln.GetValue("D2"));

                                 result.ntysj = result.gzl * result.tysj;
                                 listfhtyl.Add(result);
                             }
                         }
                         break;
                 }
                }
                rescol[xlcol[i].YJ] = listfhtyl;  //添加到容器中
            }
#endregion
#region  父线路对本线路负荷的影响
            for (int i = 0; i < fxlcol.Count;i++ )
            {
                List<rresult> listfhtyl = new List<rresult>();   //记录元件对本线路各个负荷的影响
                rresult result = new rresult();
                for (int j = 0; j < fhzlcol.Count;j++ )
                {
                    if (fhzlcol[j].FirstNode.Number<=fxlcol[i].LastNode.Number)
                    {
                       
                            result.deviceid = fhzlcol[j].YJ;
                            result.gzl =Convert.ToDouble(xl.GetValue("D3"));
                            result.tysj = Convert.ToDouble(xl.GetValue("D4"));

                            result.ntysj = result.gzl * result.tysj;
                            listfhtyl.Add(result);
                      
                    
                    }
                    else
                    {
                        
                            result.deviceid = fhzlcol[j].YJ;
                            result.gzl = Convert.ToDouble(xl.GetValue("D3")); ;
                            if (fxlcol[i].FirstNode.Type == "06")
                            {
                                result.tysj = fxlcol[i].FirstNode.HuganTQ2 ;
                            }
                            else
                                result.tysj = fxlcol[i].FirstNode.HuganTQ4 ;
                            result.ntysj = result.gzl * result.tysj;
                            listfhtyl.Add(result);
                     
                    }
                }
                rescol[fxlcol[i].YJ] = listfhtyl;  //添加到容器中
            }
#endregion
            return true;
        }
        //上行等值化分析
     
#region //上行分析
        private void dzanalsy(TreeListNode xl, int fxtype)
        {
            //判断是否有分支线 替代等值过程 将其分支线路的 停电率和故障时间等效到此节点上类型 关系表的S1=‘1’表示上行 S1=‘2’代表下行 D1 D2 D3 等效的值
            foreach (TreeListNode tln in xl.Nodes)
            {
                if (tln.GetValue("devicetype").ToString() == "73" && string.IsNullOrEmpty(tln.GetValue("S1").ToString()) && !tln.GetValue("title").ToString().Contains("节点有问题"))
                {
                    

                    dzanalsy(tln, fxtype);

                }

            }
            //判断支路信息将其等值化
            xl.SetValue("S1", "1");
            Zxdzh(xl, fxtype);
            
        }
        private void Zxdzh(TreeListNode zxl, int fxtype)
        {
            double gzl = 0, U = 0, gztime = 0, dklv = 0;
            //找到第一个线路段判断是否有断路器
            bool glkgflag = false;
            for (int i = 0; i < zxl.Nodes.Count; i++)
            {
                if (zxl.Nodes[i].GetValue("devicetype").ToString() == "74")
                {
                    if (zxl.Nodes[i].Nodes.Count > 0)
                    {
                        glkgflag = true;
                        PSPDEV pd = new PSPDEV();
                        pd.SUID = zxl.Nodes[i].Nodes[0].GetValue("DeviceID").ToString();
                        pd = Services.BaseService.GetOneByKey<PSPDEV>(pd);
                        if (pd != null)
                        {
                            if (pd.Type == "06")
                            {
                                dklv = Convert.ToDouble(pd.HuganFirst);
                                gztime = pd.HuganTQ2;
                            }
                            else if (pd.Type == "55")
                            {
                                dklv = pd.HuganTQ4;
                                gztime = pd.HuganTQ1;
                            }

                        }

                    }
                    break;
                }
            }
            //如果首段存在断路器进行等值 第一种情况
            foreach (TreeListNode tln in zxl.Nodes)
            {
                if (tln.GetValue("devicetype").ToString() == "74")
                {
                    PSPDEV pd = new PSPDEV();
                    pd.SUID = tln.GetValue("DeviceID").ToString();
                    pd = Services.BaseService.GetOneByKey<PSPDEV>(pd);
                    if (pd != null)
                    {
                        gzl += pd.HuganTQ3;
                        U += (pd.HuganTQ3 * pd.HuganTQ4);
                    }

                }
                if (tln.GetValue("devicetype").ToString() == "80")
                {
                    PSPDEV pd = new PSPDEV();
                    pd.SUID = tln.GetValue("DeviceID").ToString();
                    pd = Services.BaseService.GetOneByKey<PSPDEV>(pd);
                    if (pd != null)
                    {
                        gzl += pd.HuganTQ2;
                        U += (pd.HuganTQ2 * pd.HuganTQ3);
                    }
                }
                if (tln.GetValue("devicetype").ToString() == "73" && tln.GetValue("S1").ToString() == "1")
                {
                    gzl += Convert.ToDouble(tln.GetValue("D1"));
                    U += Convert.ToDouble(tln.GetValue("D1")) * Convert.ToDouble(tln.GetValue("D2"));
                }
            }
            //等值的过程
            if (glkgflag)
            {

                zxl.SetValue("D1", (1 - dklv) * gzl);
                zxl.SetValue("D2", gztime);
            }
            else
            {
                zxl.SetValue("D1", gzl);
                zxl.SetValue("D2", U / gzl);
            }

        }
#endregion
   
#region   下行等值化
        private void xxanaly(TreeListNode tln )
        {
            //在分析中将其值写到子支路D3 D4 中
            double gzl = 0, U = 0;
            List<TreeListNode> zxlcol = new List<TreeListNode>();
            for (int i = 0; i < tln.Nodes.Count;i++ )
            {
                if (tln.Nodes[i].GetValue("devicetype").ToString() == "73" && tln.Nodes[i].GetValue("S1").ToString() == "1")
                {
                    zxlcol.Add(tln.Nodes[i]);
                }

            }
            for (int i = 0; i < zxlcol.Count;i++ )
            {
                for (int j = 0; j < tln.Nodes.Count; j++)
                {
                    if (tln.Nodes[j].GetValue("devicetype").ToString() == "74")
                    {
                        PSPDEV pd = new PSPDEV();
                        pd.SUID = tln.Nodes[j].GetValue("DeviceID").ToString();
                        pd = Services.BaseService.GetOneByKey<PSPDEV>(pd);
                        if (pd != null)
                        {
                            gzl += pd.HuganTQ3;
                            U+= (pd.HuganTQ3 * pd.HuganTQ4);
                        }

                    }
                    if (tln.Nodes[j].GetValue("devicetype").ToString() == "80")
                    {
                        PSPDEV pd = new PSPDEV();
                        pd.SUID = tln.Nodes[j].GetValue("DeviceID").ToString();
                        pd = Services.BaseService.GetOneByKey<PSPDEV>(pd);
                        if (pd != null)
                        {
                            gzl += pd.HuganTQ2;
                            U += (pd.HuganTQ2 * pd.HuganTQ3);
                        }
                    }
                    if (tln.Nodes[j].GetValue("devicetype").ToString() == "73" && tln.GetValue("S1").ToString() == "1" && !tln.Nodes[j].Equals(zxlcol[i]))
                    {
                        gzl += Convert.ToDouble(tln.Nodes[j].GetValue("D1"));
                        U += Convert.ToDouble(tln.Nodes[j].GetValue("D1")) * Convert.ToDouble(tln.Nodes[j].GetValue("D2"));
                    }

                }
                if (tln.ParentNode.GetValue("devicetype").ToString() == "73")
                {
                    gzl += Convert.ToDouble(tln.GetValue("D3"));
                    U += Convert.ToDouble(tln.GetValue("D3")) * Convert.ToDouble(tln.GetValue("D4"));
                }
                //将其等值化的值写入其中
                zxlcol[i].SetValue("D3",gzl) ;
                zxlcol[i].SetValue("D4",U/gzl);
                //进入子路分析
                xxanaly(zxlcol[i]);
            }
        }
#endregion

        private void treeList1_FocusedNodeChanged(object sender, DevExpress.XtraTreeList.FocusedNodeChangedEventArgs e)
        {
            //if (e.Button == MouseButtons.Right) return;
            TreeListNode node = treeList1.FocusedNode;
            if (node == null) return;
            string deviceid = node["DeviceID"].ToString();
            string strID = node["devicetype"].ToString();
            string dtype = DeviceTypeHelper.DeviceClassbyType(strID);
            if (string.IsNullOrEmpty(dtype))
            {
                if (curDevice != null)
                {
                    curDevice.Hide();
                }
                return;
            }

            UCDeviceBase device = null;
            if (devicTypes.ContainsKey(dtype))
            {
                device = devicTypes[dtype];
                device.ID = strID;
                try
                {
                    device.Show();
                }
                catch { }
            }
            else
            {
                device = createInstance(dtype);
                device.ID = strID;
                device.ProjectID = Itop.Client.MIS.ProgUID;
                devicTypes.Add(dtype, device);
                showDevice(device);
            }

            if (curDevice != null && curDevice != device) curDevice.Hide();
            curDevice = device;
            if (curDevice != null)
            {
                curDevice.strCon = " where 1=1 and suid='" + deviceid + "'and ";
                curDevice.Init();
            }
        }
       
    }
}
