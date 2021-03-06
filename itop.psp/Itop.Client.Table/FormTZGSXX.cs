using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Itop.Domain.HistoryValue;
using Itop.Common;
using Itop.Client.Base;
using System.Collections;
using System.Reflection;
using DevExpress.XtraTreeList.Columns;
using DevExpress.XtraTreeList.Nodes;
using Itop.Client.Chen;
using Itop.Domain.Table;
using Itop.Client.Table;
using Itop.Client.Forecast;
using Itop.Client.Stutistics;
using Itop.Domain.Stutistics;
using Itop.Client.Common;

namespace Itop.Client.Table
{
    public partial class FormTZGSXX : FormBase
    {
        public string type;          //类型
        public string pid;            //投资估算项目ID;
        public int devicenum;         //设备个数
        public double volt = 0;        //基准电压
        public string buildyear = "2010";//开工时间
        public string buildend = "2020";//完工时间
        public FormTZGSXX()
        {
            InitializeComponent();
        }
        #region 公共属性
        /// <summary>
        /// 获取或设置"双击允许修改"标志
        /// </summary>
        //public bool AllowUpdate
        //{
        //    get { return _bAllowUpdate; }
        //    set { _bAllowUpdate = value; }
        //}

        ///// <summary>
        ///// 获取GridControl对象
        ///// </summary>
        //public GridControl GridControl
        //{
        //    get { return gridControl; }
        //}

        ///// <summary>
        ///// 获取bandedGridView1对象
        ///// </summary>
        //public BandedGridView GridView
        //{
        //    get { return this.gridView1; }
        //}

        /// <summary>
        /// 获取GridControl的数据源，即对象的列表
        /// </summary>
        public IList ObjectList
        {
            get { return this.gridControl1.DataSource as IList; }
        }

        /// <summary>
        /// 获取焦点对象，即FocusedRow
        /// </summary>
        public Ps_Table_TZMX FocusedObject
        {
            get { 
                return this.gridView1.GetRow(this.gridView1.FocusedRowHandle) as Ps_Table_TZMX; }
        }
        #endregion
        DataTable dataTable=new DataTable();
        private void initdata()
        {
            
            if (dataTable != null)
            {
                dataTable.Columns.Clear();
               gridView1.Columns.Clear();
            }
            DevExpress.XtraGrid.Columns.GridColumn column = new DevExpress.XtraGrid.Columns.GridColumn();
            if (type=="sub")
            {
                column = new DevExpress.XtraGrid.Columns.GridColumn();
                column.FieldName = "Title";
                column.Caption = "变电站名称";
                column.VisibleIndex = 0;
                column.Width = 180;
                column.OptionsColumn.AllowEdit = false;
                this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            column});
                column = new DevExpress.XtraGrid.Columns.GridColumn();
                column.FieldName = "Vol";
                column.Caption = "容量";
                column.VisibleIndex = 1;
                column.Width = 100;
                column.OptionsColumn.AllowEdit = false;
                this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            column});
                column = new DevExpress.XtraGrid.Columns.GridColumn();
                column.FieldName = "Num";
                column.Caption = "变电台数";
                column.VisibleIndex = 2;
                column.Width = 80;
                column.OptionsColumn.AllowEdit = false;
                this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            column});
            }
            else if (type=="line")
            {
                column = new DevExpress.XtraGrid.Columns.GridColumn();
                column.FieldName = "Title";
                column.Caption = "线路名称";
                column.VisibleIndex = 0;
                column.Width = 180;
                column.OptionsColumn.AllowEdit = false;
                this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            column});
                column = new DevExpress.XtraGrid.Columns.GridColumn();
                column.FieldName = "Vol";
                column.Caption = "长度";
                column.VisibleIndex = 1;
                column.Width = 100;
                column.OptionsColumn.AllowEdit = false;
                this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            column});
                column = new DevExpress.XtraGrid.Columns.GridColumn();
                column.FieldName = "Linetype";
                column.Caption = "导线型号";
                column.VisibleIndex = 2;
                column.Width = 180;
                column.OptionsColumn.AllowEdit = false;
                this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            column});

            }
            column = new DevExpress.XtraGrid.Columns.GridColumn();
            column.FieldName = "ID";
            column.Caption = "ID";
            column.VisibleIndex = -1;
            column.Width = 180;
            column.OptionsColumn.AllowEdit = false;
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            column});
            column = new DevExpress.XtraGrid.Columns.GridColumn();
            column.FieldName = "ProjectID";
            column.Caption = "ProjectID";
            column.VisibleIndex = -1;
            column.Width = 100;
            column.OptionsColumn.AllowEdit = false;
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            column});
            column = new DevExpress.XtraGrid.Columns.GridColumn();
            column.FieldName = "Typeqf";
            column.Caption = "Typeqf";
            column.VisibleIndex = -1;
            column.Width = 180;
            column.OptionsColumn.AllowEdit = false;
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            column});
            column = new DevExpress.XtraGrid.Columns.GridColumn();
            column.FieldName = "BuildYear";
            column.Caption = "开工时间";
            column.VisibleIndex =3;
            column.Width = 100;
            column.OptionsColumn.AllowEdit = false;
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            column});
            column = new DevExpress.XtraGrid.Columns.GridColumn();
            column.FieldName = "BuildEnd";
            column.Caption = "完工时间";
            column.VisibleIndex = 4;
            column.Width = 180;
            column.OptionsColumn.AllowEdit = false;
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            column});

            int firyear = Convert.ToInt32(buildyear);
            int endyear = Convert.ToInt32(buildend);
            for (int i = firyear; i <= endyear;i++ )
            {
                AddColumn(i);
            }

           
            column = new DevExpress.XtraGrid.Columns.GridColumn();
            column.FieldName = "LendRate";
            column.Caption = "贷款利率";
            column.VisibleIndex = 5;
            column.Width = 180;
            column.OptionsColumn.AllowEdit = false;
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            column});
            column = new DevExpress.XtraGrid.Columns.GridColumn();
            column.FieldName = "PrepChange";
            column.Caption = "预备费用";
            column.VisibleIndex = 6;
            column.Width = 100;
            column.OptionsColumn.AllowEdit = false;
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            column});
            column = new DevExpress.XtraGrid.Columns.GridColumn();
            column.FieldName = "DynInvest";
            column.Caption = "动态投资";
            column.VisibleIndex = 7;
            column.Width = 180;
            column.OptionsColumn.AllowEdit = false;
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            column});
            string con = "ProjectID='" + pid + "'and Typeqf='" + type + "'";
            IList listTypes = Common.Services.BaseService.GetList("SelectPs_Table_TZMXByValue", con);

            dataTable = Itop.Common.DataConverter.ToDataTable(listTypes, typeof(Ps_Table_TZMX));
            //dataTable = dc.GetSortTable(dataTable, "Flag", true);

            this.gridControl1.DataSource = listTypes;

        }
        //添加年份后，新增一列
        private void AddColumn(int year)
        {
            DevExpress.XtraGrid.Columns.GridColumn column = new DevExpress.XtraGrid.Columns.GridColumn();

            column.FieldName = "y" + year;
           // column.Tag = year;
            column.Caption ="静态投资" +year.ToString() + "年";
            column.Name = year.ToString();
            column.Width = 120;
            column.OptionsColumn.AllowEdit = false;
            //column.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            column.VisibleIndex = year;//有两列隐藏列
             this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            column});
           
        }
        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (devicenum>gridView1.RowCount||devicenum==0)
            {
                Ps_Table_TZMX px = new Ps_Table_TZMX();
                FormTZGSXXdialog fd = new FormTZGSXXdialog();

                fd.type = type;
                fd.volt = volt;
                fd.buildyear =Convert.ToInt32(buildyear);
                fd.buildend =Convert.ToInt32(buildend) ;
                fd.tzmx = px;
                fd.ShowDialog();
                if (fd.DialogResult==DialogResult.OK)
                {
                    Ps_Table_TZMX pt = fd.tzmx;
                    pt.ProjectID = pid;
                    pt.Typeqf = type;
                    pt.BuildYear = buildyear;
                    pt.BuildEnd = buildend;
                    Services.BaseService.Create<Ps_Table_TZMX>(pt);
                }
                initdata();
            }
            else
            {
                MessageBox.Show("变电站的数目已经达到!");
                return;
            }
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (FocusedObject!=null)
            {
                FormTZGSXXdialog ft = new FormTZGSXXdialog();
                ft.type = type;
                ft.volt = volt;
                ft.buildyear = Convert.ToInt32(buildyear);
                ft.buildend = Convert.ToInt32(buildend);
                ft.tzmx = FocusedObject;
                ft.ShowDialog();
                if (ft.DialogResult==DialogResult.OK)
                {
                    Services.BaseService.Update("UpdatePs_Table_TZMX",ft.tzmx);
                }
                initdata();
            }
            else
            {
                MessageBox.Show("请选择一行数据！");
                return;

            }
        }

        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (FocusedObject!=null)
            {
                if (MessageBox.Show("你确定要删除吗吗？", "删除", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    Services.BaseService.Delete<Ps_Table_TZMX>(FocusedObject);
                    initdata();
                }
            }
        }

        private void FormTZGSXX_Load(object sender, EventArgs e)
        {
            initdata();
        }

        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }
    }
}