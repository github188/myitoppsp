using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Itop.Client.Common;
using Itop.Domain.Layouts;
using Itop.Domain.Table;
using Itop.Common;
using Itop.Domain.Graphics;
using Itop.Domain.Stutistic;

using System.Reflection;
using System.Diagnostics;
using DevExpress.Utils;
using Itop.Domain.RightManager;
using Itop.Client.Base;
using FarPoint.Win;
using Itop.Domain.Forecast;
using Microsoft.Office.Interop.Excel;

namespace Itop.Client.GYGH
{
    public partial class FrmGYDWGH : FormBase
    {
        

        fpcommon fc = new fpcommon();
        
        //定义一个边框线
        LineBorder border = new LineBorder(Color.Black);
        //工程号
        string ProjID = Itop.Client.MIS.ProgUID;
        public FrmGYDWGH()
        {
            InitializeComponent();
        }
        private void FrmGYDWGH_Load(object sender, EventArgs e)
        {

            //根据窗口变化全部添满
            fpSpread1.Dock = System.Windows.Forms.DockStyle.Fill;
            //添加表
            fpSpread1_addsheet();
            
            
        }
        private void SpreadRemoveEmptyCells(FarPoint.Win.Spread.FpSpread tempspread)
        {
            //本方法用于去掉多表格中多余的行和列（空）
            //定义无空单元格模式
           FarPoint.Win.Spread.Model.INonEmptyCells nec;
            //计算spread有多少个表格
           int sheetscount = tempspread.Sheets.Count;
            //定义行数
           int rowcount = 0;
            //定义列数
           int colcount = 0;
           for (int m = 0; m < sheetscount;m++ )
           {
               nec = (FarPoint.Win.Spread.Model.INonEmptyCells)tempspread.Sheets[m].Models.Data;
               //计算无空单元格列数
               colcount= nec.NonEmptyColumnCount;
               //计算无空单元格行数
               rowcount = nec.NonEmptyRowCount;
               tempspread.Sheets[m].RowCount = rowcount;
               tempspread.Sheets[m].ColumnCount = colcount;
           }
        }
        private void fpSpread1_addsheet()
        {
            WaitDialogForm wait = null;
            wait = new WaitDialogForm("", "正在加载数据, 请稍候...");
            try
            {
                //打开Excel表格
                fpSpread1.Sheets.Clear();
                fpSpread1.OpenExcel(System.Windows.Forms.Application.StartupPath + "\\xls\\GYDWGH.xls");
                SpreadRemoveEmptyCells(fpSpread1);
            }
            catch (System.Exception e)
            {
                //如果打开出错则重新生成并保存
                fpSpread1.Sheets.Clear();
                Firstadddata();
                //判断文件夹是否存在，不存在则创建
                if (!Directory.Exists(System.Windows.Forms.Application.StartupPath + "\\xls"))
                {
                    Directory.CreateDirectory(System.Windows.Forms.Application.StartupPath + "\\xls");
                }
                fpSpread1.SaveExcel(System.Windows.Forms.Application.StartupPath + "\\xls\\GYDWGH.xls");
            }
            wait.Close();
        }   
        private  void Firstadddata()
        {
            //如果没有文件，则全部表格重写
            //新建表7-1
            FarPoint.Win.Spread.SheetView Sheet71 = new FarPoint.Win.Spread.SheetView();
            //新建表7-2
            FarPoint.Win.Spread.SheetView Sheet72 = new FarPoint.Win.Spread.SheetView();
            //新建表7-2附表36
            FarPoint.Win.Spread.SheetView Sheet72_36 = new FarPoint.Win.Spread.SheetView();
            //新建表7-3
            FarPoint.Win.Spread.SheetView Sheet73 = new FarPoint.Win.Spread.SheetView();
            //新建表7-3附表37
            FarPoint.Win.Spread.SheetView Sheet73_37 = new FarPoint.Win.Spread.SheetView();
            //新建表7-4
            FarPoint.Win.Spread.SheetView Sheet74 = new FarPoint.Win.Spread.SheetView();
            //新建表7-4附表38
            FarPoint.Win.Spread.SheetView Sheet74_38 = new FarPoint.Win.Spread.SheetView();
            //新建表7-5
            FarPoint.Win.Spread.SheetView Sheet75 = new FarPoint.Win.Spread.SheetView();
            //新建表7-5附表39
            FarPoint.Win.Spread.SheetView Sheet75_39 = new FarPoint.Win.Spread.SheetView();
            //新建表7-6
            FarPoint.Win.Spread.SheetView Sheet76 = new FarPoint.Win.Spread.SheetView();
            //新建表7-6附表40
            FarPoint.Win.Spread.SheetView Sheet76_40 = new FarPoint.Win.Spread.SheetView();
            //新建表7-7
            FarPoint.Win.Spread.SheetView Sheet77 = new FarPoint.Win.Spread.SheetView();
            //新建表7-8
            FarPoint.Win.Spread.SheetView Sheet78 = new FarPoint.Win.Spread.SheetView();
            //新建表7-9
            FarPoint.Win.Spread.SheetView Sheet79 = new FarPoint.Win.Spread.SheetView();

            //根据窗口变化全部添满
            fpSpread1.Dock = System.Windows.Forms.DockStyle.Fill;


            Sheet71.SheetName = "表7-1";
            fpSpread1.Sheets.Add(Sheet71);

            Sheet72.SheetName = "表7-2";
            fpSpread1.Sheets.Add(Sheet72);

            Sheet72_36.SheetName = "表7-2 附表36";
            fpSpread1.Sheets.Add(Sheet72_36);

            Sheet73.SheetName = "表7-3";
            fpSpread1.Sheets.Add(Sheet73);

            Sheet73_37.SheetName = "表7-3 附表37";
            fpSpread1.Sheets.Add(Sheet73_37);

            Sheet74.SheetName = "表7-4 ";
            fpSpread1.Sheets.Add(Sheet74);

            Sheet74_38.SheetName = "表7-4 附表38";
            fpSpread1.Sheets.Add(Sheet74_38);

            Sheet75.SheetName = "表7-5";
            fpSpread1.Sheets.Add(Sheet75);

            Sheet75_39.SheetName = "表7-5 附表39";
            fpSpread1.Sheets.Add(Sheet75_39);

            Sheet76.SheetName = "表7-6";
            fpSpread1.Sheets.Add(Sheet76);

            Sheet76_40.SheetName = "表7-6 附表40";
            fpSpread1.Sheets.Add(Sheet76_40);

            Sheet77.SheetName = "表7-7";
            fpSpread1.Sheets.Add(Sheet77);

            Sheet78.SheetName = "表7-8";
            fpSpread1.Sheets.Add(Sheet78);

            Sheet79.SheetName = "表7-9";
            fpSpread1.Sheets.Add(Sheet79);

            build_sheet71(fpSpread1.Sheets[0]);
            build_Sheet72(fpSpread1.Sheets[1]);
            build_Sheet72_36(fpSpread1.Sheets[2]);
            build_Sheet73(fpSpread1.Sheets[3]);
            build_Sheet73_37(fpSpread1.Sheets[4]);
            build_Sheet74(fpSpread1.Sheets[5]);
            build_Sheet74_38(fpSpread1.Sheets[6]);
            build_Sheet75(fpSpread1.Sheets[7]);
            build_Sheet75_39(fpSpread1.Sheets[8]);
            build_Sheet76(fpSpread1.Sheets[9]);
            build_Sheet76_40(fpSpread1.Sheets[10]);
            build_Sheet77(fpSpread1.Sheets[11]);
            build_Sheet78(fpSpread1.Sheets[12]);
            build_Sheet79(fpSpread1.Sheets[13]);
        }
        private void Sheet_WriteZero(FarPoint.Win.Spread.SheetView obj_sheet, int startrow, int startcol, int rowcount, int colcount)
        {
            for (int row = 0; row < rowcount; row++)
            {
                for (int col = 0; col < colcount; col++)
                {
                    obj_sheet.SetValue(startrow + row, startcol + col, 0);
                }
            }
        }
        private string Sheet_find_notnull(FarPoint.Win.Spread.SheetView obj_sheet, int row, int col)
        {
            //对于合并单元格的表格，除合并列的第一行能找到数据外，其余行为null
            //本方法用于返回合并列的数据，递归向上找。返回值为“错误”表示没找到数据
            if (obj_sheet.Cells[row, col].Value != null)
            {
                return obj_sheet.Cells[row, col].Value.ToString();
            }
            else
            {
                if (row != 0)
                {
                    return Sheet_find_notnull(obj_sheet, row - 1, col);
                }
                else
                {
                    return "错误";
                }
            }
        }
        private void build_sheet71(FarPoint.Win.Spread.SheetView Sheet71)
        {
           
           // Sheet71.SheetName = "规划情况";     
            
            //工作表为9列
            Sheet71.Columns.Count = 9;
            //查询变电站电压有多少等级条件(与线路等级相同)
            string tiaojian = " ProjectID='"+ProjID+"' and BianInfo!='' group by substring(BianInfo,1,charindex('@',BianInfo,0)-1)";
            //记录电压等级
            IList ptz = Services.BaseService.GetList("SelectBianInfo", tiaojian);
            #region //修改情况：原表设计为查询电压等级，现修改为固定电线，220kv和330kv
            //修改部分,直接添加330，220，电压
            ptz.Clear();
            ptz.Add("330");
            ptz.Add("220");
            #endregion            
            int m = ptz.Count;//记录电压等级计数
            Sheet71.Rows.Count = 2 + 6 * m;//根据电压等级确定表格的行数，每增加一个电压等级增加6行
            //设表格线和居中
            fc.Sheet_GridandCenter(Sheet71);
            fc.Sheet_Referen_R1C1(Sheet71);
            //设定表格中第1列表列和第9列宽度
            Sheet71.Columns[0].Width = 92;
            Sheet71.Columns[1].Width = 100;

            Sheet71.Columns[8].Width = 95;
            Sheet71.Rows[0].Height = 35;
            //合并第一行的所有列做为表头
            Sheet71.AddSpanCell(0, 0, 1, 9);
            //向表中写入相关的标题
            Sheet71.SetValue(0, 0, "表7-1 铜陵市220kV、330kV电网“十二五”规划情况");
            Sheet71.SetValue(1, 0, "电压等级（kV）");
            Sheet71.SetValue(1, 1, "项目名称");
            //循环写入年份
            for (int i = 2; i < 8; i++)
            {
                Sheet71.SetValue(1, i, i + 2008 + "年");
            }
            Sheet71.SetValue(1, 8, "“十二五”合计");
            //循环写入电压等级和相应的六个项目
            for (int j = 0; j < m; j++)
            {
                Sheet71add(Sheet71, 2 + j * 6);
                Sheet71.AddSpanCell(2 + j * 6, 0, 6, 1);
                Sheet71.SetValue(2 + j * 6, 0, ptz[j]);
            }
            //从数据库提取数据写入表格
            //用电压等级计数做外循环控制
            for (int j = 0; j < m; j++)
            {
                //用需要添加内容的列数做内循环控制
                for (int i = 2; i < 8; i++)
                {
                    //用年份Col4和电压等级做条件，BianInfo格式为"220@新建220kV变电站",220为电压
                    string yeartiaojian = " ProjectID='" + ProjID + "' and BuildEd='" + (i + 2008) + "' and Col4='bian'  and BianInfo like '" + Sheet71.Cells[2 + 6 * j, 0].Value + "@%'";
                    //计算变电站数量
                    double DZnum = 0;
                    if (Services.BaseService.GetObject("SelectTZGSSUMnum5", yeartiaojian)!=null)
                    {
                        DZnum = (double)Services.BaseService.GetObject("SelectTZGSSUMnum5", yeartiaojian);
                    }
                    //向表格写入变电站数量
                    Sheet71.SetValue(2 + 6 * j, i, DZnum);
                    //记录主变总和
                    double ZBnum = 0;
                    if (Services.BaseService.GetObject("SelectTZGSSUMnum1", yeartiaojian) != null)
                    {
                        ZBnum = (double)Services.BaseService.GetObject("SelectTZGSSUMnum1", yeartiaojian);
                    }
                    //向表格写入主变总和
                    Sheet71.SetValue(3 + 6 * j, i, ZBnum);
                    //记录主变容量和
                    double ZBRLnum = 0;

                    if (Services.BaseService.GetObject("SelectTZGSSUMVolumn", yeartiaojian) != null)
                    {
                        ZBRLnum = (double)Services.BaseService.GetObject("SelectTZGSSUMVolumn", yeartiaojian);
                    }
                    //向表格写入主变容量和
                    Sheet71.SetValue(4 + 6 * j, i, ZBRLnum);
                    //记录架空线长度和
                    double JKlinenum = 0;
                    string linetiaojian = " ProjectID='" + ProjID + "' and BuildEd='" + (i + 2008) + "' and Col4='line'  and LineInfo like '" + Sheet71.Cells[2 + 6 * j, 0].Value + "@%'";
                    if (Services.BaseService.GetObject("SelectTZGSSUMLength", linetiaojian) != null)
                    {
                        JKlinenum = (double)Services.BaseService.GetObject("SelectTZGSSUMLength", linetiaojian);
                    }
                    //向表格写入架空线长度和
                    Sheet71.SetValue(5 + 6 * j, i, JKlinenum);
                    //记录电缆线长度和
                    double DLlinenum = 0;
                    string linedltiaojian = " ProjectID='" + ProjID + "' and BuildEd='" + (i + 2008) + "' and Col4='line'  and LineInfo like '" + Sheet71.Cells[2 + 6 * j, 0].Value + "@%'";
                    if (Services.BaseService.GetObject("SelectTZGSSUMLength2", linedltiaojian) != null)
                    {
                        DLlinenum = (double)Services.BaseService.GetObject("SelectTZGSSUMLength2", linedltiaojian);
                    }
                    //向表格写入电缆线长度和
                    Sheet71.SetValue(6 + 6 * j, i, DLlinenum);
                    //记录架空线和电缆线的长度和公式
                    string formulastr="";
                    //通过表格获取架空线长和电缆线长来写入公式
                    formulastr = "SUM(R" + (5 + 6 * j + 1) + "C" + (i + 1) + ", R" + (6 + 6 * j + 1) + "C" + (i + 1) + ")";
                    //向表格写入总线长度公式
                    Sheet71.Cells[7 + 6 * j, i].Formula = formulastr;
                }
            }
            //通过表格计算最后一列和值
             fc.Sheet_WriteFormula_ColSum_WritOne(Sheet71, 2, 3, m * 6, 5, 8);
           
            //使表格只读
            fc.Sheet_Locked(Sheet71);
        }
        private void Sheet71add(FarPoint.Win.Spread.SheetView sheet1, int i)
        {
            //对表7-1每增加一个电压等级时以下六个项目是固定的
            sheet1.SetValue(i, 1, "变电站（座）");
            sheet1.SetValue(i + 1, 1, "主变（台）");
            sheet1.SetValue(i + 2, 1, "变电容量（MVA）");
            sheet1.SetValue(i + 3, 1, "架空（km）");
            sheet1.SetValue(i + 4, 1, "电缆（km）");
            sheet1.SetValue(i + 5, 1, "线路合计（km）");
        }
        private void build_Sheet72(FarPoint.Win.Spread.SheetView Sheet72)
        {
            //工作表为19列59行
            Sheet72.Columns.Count = 19;
            Sheet72.Rows.Count = 59;
            //设表格线和居中
            fc.Sheet_GridandCenter(Sheet72);
            //设表格模式为R1C1
            fc.Sheet_Referen_R1C1(Sheet72);
            //设定表格中第1列表列和第9列宽度
            Sheet72.Columns[0].Width = 50;
            Sheet72.Columns[1].Width = 100;
            Sheet72.Columns[2].Width = 85;
            Sheet72.Columns[3].Width = 130;
            Sheet72.Rows[0].Height = 35;
            //合并第一行的所有列做为表头
            Sheet72.AddSpanCell(0, 0, 1, 19);
            //向表中写入相关的标题
            Sheet72.SetValue(0, 0, "表7-2  铜陵市高压配电网规划新、扩建变电工程规模");
            Sheet72.AddSpanCell(1, 0, 2, 1);
            Sheet72.SetValue(1, 0, "编号");
            Sheet72.AddSpanCell(1, 1, 2, 1);
            Sheet72.SetValue(1, 1, "类型");
            Sheet72.AddSpanCell(1, 2, 2, 1);
            Sheet72.SetValue(1, 2, "电压等级(KV)");
            Sheet72.AddSpanCell(1, 3, 2, 1);
            Sheet72.SetValue(1, 3, "项  目");
            //循环写入年份
            for (int i = 4; i < 16; i++)
            {
                Sheet72.AddSpanCell(1, i, 1, 2);
                Sheet72.SetValue(1, i, (2010+(i-4)/2) + "年");
                Sheet72.SetValue(2, i, "新建");
                Sheet72.SetValue(2, ++i, "扩建");
            }
            Sheet72.AddSpanCell(1, 16, 1, 3);
            Sheet72.SetValue(1, 16, "“十二五”合计");
            Sheet72.SetValue(2, 16, "新建");
            Sheet72.SetValue(2, 17, "扩建");
            Sheet72.SetValue(2, 18, "小计");
            //写入市辖供电区表格部分
            int jishu = 0;
            Sheet72_36add(Sheet72, 3 + jishu * 8);
            Sheet72.AddSpanCell(3 + jishu * 8, 1, 8, 1);
            Sheet72.SetValue(3 + jishu * 8, 1, "市辖供电区");
            Sheet72.AddSpanCell(3 + jishu * 8, 0, 8, 1);
            Sheet72.SetValue(3 + jishu * 8, 0, "1");
            //县级供电区
            jishu++;
            Sheet72_36add(Sheet72, 3 + jishu * 8);
            Sheet72.AddSpanCell(3 + jishu * 8, 1, 8, 1);
            Sheet72.SetValue(3 + jishu * 8, 1, "县级供电区");
            Sheet72.AddSpanCell(3 + jishu * 8, 0, 8, 1);
            Sheet72.SetValue(3 + jishu * 8, 0, "2");
            //其中：直供直管
            jishu++;
            Sheet72_36add(Sheet72, 3 + jishu * 8);
            Sheet72.AddSpanCell(3 + jishu * 8, 1, 8, 1);
            Sheet72.SetValue(3 + jishu * 8, 1, "其中：直供直管");
            Sheet72.AddSpanCell(3 + jishu * 8, 0, 8, 1);
            Sheet72.SetValue(3 + jishu * 8, 0, "2.1");
            //县级控股
            jishu++;
            Sheet72_36add(Sheet72, 3 + jishu * 8);
            Sheet72.AddSpanCell(3 + jishu * 8, 1, 8, 1);
            Sheet72.SetValue(3 + jishu * 8, 1, "控股");
            Sheet72.AddSpanCell(3 + jishu * 8, 0, 8, 1);
            Sheet72.SetValue(3 + jishu * 8, 0, "2.2");
            //县级参股
            jishu++;
            Sheet72_36add(Sheet72, 3 + jishu * 8);
            Sheet72.AddSpanCell(3 + jishu * 8, 1, 8, 1);
            Sheet72.SetValue(3 + jishu * 8, 1, "参股");
            Sheet72.AddSpanCell(3 + jishu * 8, 0, 8, 1);
            Sheet72.SetValue(3 + jishu * 8, 0, "2.3");
            //县级代管
            jishu++;
            Sheet72_36add(Sheet72, 3 + jishu * 8);
            Sheet72.AddSpanCell(3 + jishu * 8, 1, 8, 1);
            Sheet72.SetValue(3 + jishu * 8, 1, "代管");
            Sheet72.AddSpanCell(3 + jishu * 8, 0, 8, 1);
            Sheet72.SetValue(3 + jishu * 8, 0, "2.4");
            //全地区合计
            jishu++;
            Sheet72_36add(Sheet72, 3 + jishu * 8);
            Sheet72.AddSpanCell(3 + jishu * 8, 1, 8, 1);
            Sheet72.SetValue(3 + jishu * 8, 1, "全地区合计");
            Sheet72.AddSpanCell(3 + jishu * 8, 0, 8, 1);
            Sheet72.SetValue(3 + jishu * 8, 0, "3");
            adddata_sheet72(Sheet72);
            //锁定表格
            fc.Sheet_Locked(Sheet72);
        }
        private void adddata_sheet72(FarPoint.Win.Spread.SheetView obj_sheet)
        {
            //Sheet72即为表7-2
            //向表7-3写入市辖供电区数据
            adddata_sheet72_diqu(obj_sheet,"市辖供电区", 3);
            //向表7-3写入县级直供直管数据
            adddata_sheet72_diqu(obj_sheet, "县级直供直管", 19);
            //向表7-3写入县级控股数据
            adddata_sheet72_diqu(obj_sheet, "县级控股", 27);
            //向表7-3写入县级参股数据
            adddata_sheet72_diqu(obj_sheet, "县级参股", 35);
            //向表7-3写入县级代管数据
            adddata_sheet72_diqu(obj_sheet, "县级代管", 43);
            //写入计算县级供电区数据公式（求和）
            fc.Sheet_WriteFormula_RowSum(obj_sheet, 19, 4, 4, 8, 11, 4, 12);
            //写入计算全地区合计数据公式（求和）
            fc.Sheet_WriteFormula_RowSum(obj_sheet, 3, 4, 2, 8, 51, 4, 12);
            //写入计算十二五新建扩建数据公式（求和）
            fc.Sheet_WriteFormula_MoreColsum(obj_sheet, 3, 6, 56, 5, 3, 16, 2);
            //写入合计部分公式
            fc.Sheet_WriteFormula_ColSum_WritOne(obj_sheet, 3, 16, 56, 2, 18);
           

        }
        private void adddata_sheet72_diqu(FarPoint.Win.Spread.SheetView obj_sheet,string diqu, int row)
        {

            //根据地区不同向表7-2写入数据
            //diqu参数为表7-2的类型，可以为市辖供电区，县级直供直管等
            //row参数为要写入数据的地区开始行号
            for (int i = 4; i < 16; i++)
            {
                #region  新建数据
                //向表写入diqu110（66）变电站台数 新建
                string BDZtiaojian1 = " ProjectID='"+ProjID+"' and BuildEd='" + (2010 + (i - 4) / 2) + "' and Col3='新建' and Col4='bian' and  DQ='" + diqu + "' and (BianInfo like '110@%' or BianInfo like '66@%')";
                double BDZsum1 = 0;
                if (Services.BaseService.GetObject("SelectTZGSSUMnum5", BDZtiaojian1) == null)
                {
                    BDZsum1 = 0;
                }
                else
                {
                    BDZsum1 = (double)Services.BaseService.GetObject("SelectTZGSSUMnum5", BDZtiaojian1);
                }
                obj_sheet.SetValue(row + 0, i, BDZsum1);
                //向表写入diqu110（66）主变台数
                double ZBsum1 = 0;
                if (Services.BaseService.GetObject("SelectTZGSSUMnum1", BDZtiaojian1) == null)
                {
                    ZBsum1 = 0;
                }
                else
                {
                    ZBsum1 = (double)Services.BaseService.GetObject("SelectTZGSSUMnum1", BDZtiaojian1);
                }
                obj_sheet.SetValue(row + 1, i, ZBsum1);
                //向表写入diqu110（66）变电容量
                double BDRLsum1 = 0;
                if (Services.BaseService.GetObject("SelectTZGSSUMVolumn", BDZtiaojian1) == null)
                {
                    BDRLsum1 = 0;
                }
                else
                {
                    BDRLsum1 = (double)Services.BaseService.GetObject("SelectTZGSSUMVolumn", BDZtiaojian1);
                }
                obj_sheet.SetValue(row + 2, i, BDRLsum1);
                //向表写入diqu110（66）无功补尝容量
                double WGBCsum1 = 0;
                if (Services.BaseService.GetObject("SelectTZGSSUMWGNum", BDZtiaojian1) == null)
                {
                    WGBCsum1 = 0;
                }
                else
                {
                    WGBCsum1 = (double)Services.BaseService.GetObject("SelectTZGSSUMWGNum", BDZtiaojian1);
                }
                obj_sheet.SetValue(row + 3, i, WGBCsum1);

                //向表写入diqu35变电站台数 新建
                string BDZtiaojian2 = " ProjectID='"+ProjID+"' and BuildEd='" + (2010 + (i - 4) / 2) + "' and Col3='新建' and Col4='bian' and  DQ='" + diqu + "' and BianInfo like '35@%'";
                double BDZsum2 = 0;
                if (Services.BaseService.GetObject("SelectTZGSSUMnum5", BDZtiaojian2) == null)
                {
                    BDZsum2 = 0;
                }
                else
                {
                    BDZsum2 = (double)Services.BaseService.GetObject("SelectTZGSSUMnum5", BDZtiaojian2);
                }
                obj_sheet.SetValue(row + 4, i, BDZsum2);
                //向表写入diqu35主变台数
                double ZBsum2 = 0;
                if (Services.BaseService.GetObject("SelectTZGSSUMnum1", BDZtiaojian2) == null)
                {
                    ZBsum2 = 0;
                }
                else
                {
                    ZBsum2 = (double)Services.BaseService.GetObject("SelectTZGSSUMnum1", BDZtiaojian2);
                }
                obj_sheet.SetValue(row + 5, i, ZBsum2);
                //向表写入diqu35变电容量
                double BDRLsum2 = 0;
                if (Services.BaseService.GetObject("SelectTZGSSUMVolumn", BDZtiaojian2) == null)
                {
                    BDRLsum2 = 0;
                }
                else
                {
                    BDRLsum2 = (double)Services.BaseService.GetObject("SelectTZGSSUMVolumn", BDZtiaojian2);
                }
                obj_sheet.SetValue(row + 6, i, BDRLsum2);
                //向表写入diqu35无功补尝容量
                double WGBCsum2 = 0;
                if (Services.BaseService.GetObject("SelectTZGSSUMWGNum", BDZtiaojian2) == null)
                {
                    WGBCsum2 = 0;
                }
                else
                {
                    WGBCsum2 = (double)Services.BaseService.GetObject("SelectTZGSSUMWGNum", BDZtiaojian2);
                }
                obj_sheet.SetValue(row + 7, i, WGBCsum2);
                #endregion
                #region 扩建数据
                //向表写入diqu110（66）变电站台数  扩建
                string BDZtiaojian3 = " ProjectID='"+ProjID+"' and BuildEd='" + (2010 + (i - 4) / 2) + "' and Col3='扩建' and Col4='bian' and  DQ='" + diqu + "' and (BianInfo like '110@%' or BianInfo like '66@%')";
                double BDZsum3 = 0;
                if (Services.BaseService.GetObject("SelectTZGSSUMnum5", BDZtiaojian3) == null)
                {
                    BDZsum3 = 0;
                }
                else
                {
                    BDZsum3 = (double)Services.BaseService.GetObject("SelectTZGSSUMnum5", BDZtiaojian3);
                }
                obj_sheet.SetValue(row + 0, i + 1, BDZsum3);
                //向表写入diqu110（66）主变台数
                double ZBsum3 = 0;
                if (Services.BaseService.GetObject("SelectTZGSSUMnum1", BDZtiaojian3 ) == null)
                {
                    ZBsum3 = 0;
                }
                else
                {
                    ZBsum3 = (double)Services.BaseService.GetObject("SelectTZGSSUMnum1", BDZtiaojian3 );
                }
                obj_sheet.SetValue(row + 1, i + 1, ZBsum3);
                //向表写入diqu110（66）变电容量
                double BDRLsum3 = 0;
                if (Services.BaseService.GetObject("SelectTZGSSUMVolumn", BDZtiaojian3 ) == null)
                {
                    BDRLsum3 = 0;
                }
                else
                {
                    BDRLsum3 = (double)Services.BaseService.GetObject("SelectTZGSSUMVolumn", BDZtiaojian3 );
                }
                obj_sheet.SetValue(row + 2, i + 1, BDRLsum3);
                //向表写入diqu110（66）无功补尝容量
                double WGBCsum3 = 0;
                if (Services.BaseService.GetObject("SelectTZGSSUMWGNum", BDZtiaojian3 ) == null)
                {
                    WGBCsum3 = 0;
                }
                else
                {
                    WGBCsum3 = (double)Services.BaseService.GetObject("SelectTZGSSUMWGNum", BDZtiaojian3 );
                }
                obj_sheet.SetValue(row + 3, i + 1, WGBCsum3);

                //向表写入diqu35变电站台数 新建
                string BDZtiaojian4 = " ProjectID='"+ProjID+"' and BuildEd='" + (2010 + (i - 4) / 2) + "' and Col3='扩建' and Col4='bian' and  DQ='" + diqu + "' and BianInfo like '35@%'";
                double BDZsum4= 0;
                if (Services.BaseService.GetObject("SelectTZGSSUMnum5", BDZtiaojian4) == null)
                {
                    BDZsum4= 0;
                }
                else
                {
                    BDZsum4= (double)Services.BaseService.GetObject("SelectTZGSSUMnum5", BDZtiaojian4);
                }
                obj_sheet.SetValue(row + 4, i + 1, BDZsum4);
                //向表写入diqu35主变台数
                double ZBsum4 = 0;
                if (Services.BaseService.GetObject("SelectTZGSSUMnum1", BDZtiaojian4) == null)
                {
                    ZBsum4 = 0;
                }
                else
                {
                    ZBsum4 = (double)Services.BaseService.GetObject("SelectTZGSSUMnum1", BDZtiaojian4);
                }
                obj_sheet.SetValue(row + 5, i + 1, ZBsum4);
                //向表写入diqu35变电容量
                double BDRLsum4 = 0;
                if (Services.BaseService.GetObject("SelectTZGSSUMVolumn", BDZtiaojian4) == null)
                {
                    BDRLsum4 = 0;
                }
                else
                {
                    BDRLsum4 = (double)Services.BaseService.GetObject("SelectTZGSSUMVolumn", BDZtiaojian4);
                }
                obj_sheet.SetValue(row + 6, i + 1, BDRLsum4);
                //向表写入diqu35无功补尝容量
                double WGBCsum4 = 0;
                if (Services.BaseService.GetObject("SelectTZGSSUMWGNum", BDZtiaojian4) == null)
                {
                    WGBCsum4 = 0;
                }
                else
                {
                    WGBCsum4 = (double)Services.BaseService.GetObject("SelectTZGSSUMWGNum", BDZtiaojian4);
                }
                obj_sheet.SetValue(row + 7, i + 1, WGBCsum4);
                #endregion
                i++;
                                
            }

        }
        private void build_Sheet72_36(FarPoint.Win.Spread.SheetView Sheet72_36)
        {
            //工作表为19列
            Sheet72_36.Columns.Count = 19;
            ////查询市辖供电区分区
            string SXtiaojian = " ProjectID='"+ProjID+"' and BuildEd between 2010 and 2015 and AreaName!='' and (Col3='新建' or Col3='扩建') and (BianInfo like '110@%' or  BianInfo like '66@%' or BianInfo like '35@%') and  Col4='bian' and DQ='市辖供电区' group by AreaName";
            IList SXlist = Services.BaseService.GetList("SelectTZGSAreaName", SXtiaojian);
            ////查询县级供电分区
            string XJtiaojian = " ProjectID='"+ProjID+"' and BuildEd between 2010 and 2015 and AreaName!='' and (Col3='新建' or Col3='扩建') and (BianInfo like '110@%' or  BianInfo like '66@%' or BianInfo like '35@%')  and  Col4='bian' and DQ!='市辖供电区' group by AreaName";
            IList XJlist = Services.BaseService.GetList("SelectTZGSAreaName", XJtiaojian);
            ////记录分区数
            int SXSUM = SXlist.Count;
            int XJSUM = XJlist.Count;
            //根据分区数据确定表格的行数，每增加一个分区增加8行
            Sheet72_36.Rows.Count = 3 + 8 * 2 + 8 * (SXSUM + XJSUM);
            //设表格线和居中
            fc.Sheet_GridandCenter(Sheet72_36);
            //设表格模式为R1C1
            fc.Sheet_Referen_R1C1(Sheet72_36);
            //设定表格中第1列表列和第9列宽度
            Sheet72_36.Columns[0].Width = 80;
            Sheet72_36.Columns[1].Width = 100;
            Sheet72_36.Columns[2].Width = 85;
            Sheet72_36.Columns[3].Width = 130;
            Sheet72_36.Rows[0].Height = 35;
            //合并第一行的所有列做为表头
            Sheet72_36.AddSpanCell(0, 0, 1, 19);
            //向表中写入相关的标题
            Sheet72_36.SetValue(0, 0, "附表36  铜陵市高压配电网规划方案变电工程量统计（新、扩建部分）");
            Sheet72_36.AddSpanCell(1, 0, 2, 1);
            Sheet72_36.SetValue(1, 0, "分区类型");
            Sheet72_36.AddSpanCell(1, 1, 2, 1);
            Sheet72_36.SetValue(1, 1, "分区名称");
            Sheet72_36.AddSpanCell(1, 2, 2, 1);
            Sheet72_36.SetValue(1, 2, "电压等级(KV)");
            Sheet72_36.AddSpanCell(1, 3, 2, 1);
            Sheet72_36.SetValue(1, 3, "项  目");
            //循环写入年份
            for (int i = 4; i < 16; i++)
            {
                Sheet72_36.AddSpanCell(1, i, 1, 2);
                Sheet72_36.SetValue(1, i, (2010 + (i - 4) / 2) + "年");
                Sheet72_36.SetValue(2, i, "新建");
                Sheet72_36.SetValue(2, ++i, "扩建");
            }
            Sheet72_36.AddSpanCell(1, 16, 1, 3);
            Sheet72_36.SetValue(1, 16, "“十二五”合计");
            Sheet72_36.SetValue(2, 16, "新建");
            Sheet72_36.SetValue(2, 17, "扩建");
            Sheet72_36.SetValue(2, 18, "小计");
            
            //写入市辖供电区合计表格部分
            Sheet72_36add(Sheet72_36, 3);
            Sheet72_36.AddSpanCell(3, 1, 8, 1);
            Sheet72_36.SetValue(3, 1, "合计");
            //循环写入市辖供电区分区和相应的项目
            if (SXSUM != 0)
            {
                for (int j = 1; j <= SXSUM; j++)
                {
                    Sheet72_36add(Sheet72_36, 3 + j * 8);
                    Sheet72_36.AddSpanCell(3 + j * 8, 1, 8, 1);
                    Sheet72_36.SetValue(3 + j * 8, 1, SXlist[j - 1]);
                }
            }
            Sheet72_36.AddSpanCell(3, 0, 8 * (SXSUM + 1), 1);
            Sheet72_36.SetValue(3, 0, "市辖供电区");
            //写入县级供电区合计表格部分
            Sheet72_36add(Sheet72_36, 3 + 8 * (SXSUM + 1));
            Sheet72_36.AddSpanCell(3 + 8 * (SXSUM + 1), 1, 8, 1);
            Sheet72_36.SetValue(3 + 8 * (SXSUM + 1), 1, "合计");
            //循环写入县级供电区分区和相应的项目
            if (XJSUM != 0)
            {
                for (int j = 1; j <= XJSUM; j++)
                {
                    Sheet72_36add(Sheet72_36, 3 + (j + SXSUM + 1) * 8);
                    Sheet72_36.AddSpanCell(3 + (j + SXSUM + 1) * 8, 1, 8, 1);
                    Sheet72_36.SetValue(3 + (j + SXSUM + 1) * 8, 1, XJlist[j - 1]);
                }
            }
            Sheet72_36.AddSpanCell(3 + 8 * (SXSUM + 1), 0, 8 * (XJSUM + 1), 1);
            Sheet72_36.SetValue(3 + 8 * (SXSUM + 1), 0, "县级供电区");
            //从数据库提取数据写入表格   

            if (SXSUM != 0)
            {
                for (int sum = 1; sum <= SXSUM; sum++)
                {
                    //向表中写入市辖分区数据
                    Sheet72_36adddata( Sheet72_36, 3 + sum * 8, " and DQ='市辖供电区' ");

                }
                //向表中写入市辖分区合计部分公式
                fc.Sheet_WriteFormula_RowSum(Sheet72_36, 11, 4, SXSUM, 8, 3,4,12);
            }
            else
            {
                ////如果无市辖地区，合计部分直接写0
                Sheet_WriteZero( Sheet72_36, 3, 4, 8, 12);
            }

            if (XJSUM != 0)
            {
                for (int sum = SXSUM + 2; sum <= SXSUM + XJSUM + 1; sum++)
                {
                    //向表中写入县级分区数据
                    Sheet72_36adddata( Sheet72_36, 3 + sum * 8, " and DQ!='市辖供电区' ");

                }
                //向表中写入县级分区合计部分

                fc.Sheet_WriteFormula_RowSum(Sheet72_36, (SXSUM + 2) * 8+3, 4, XJSUM, 8, 3 + (SXSUM + 1) * 8, 4, 12);
                
            }
            else
            {
                //如果无县级分区，合计部分直接写0
                Sheet_WriteZero( Sheet72_36, 3 + (SXSUM + 1) * 8, 4, 8, 12);
            }
            //写入计算十二五新建扩建数据公式（求和）
            fc.Sheet_WriteFormula_MoreColsum(Sheet72_36, 3, 6, (SXSUM + XJSUM + 2) * 8, 5, 3, 16, 2);
            //写入合计部分公式
            fc.Sheet_WriteFormula_ColSum_WritOne(Sheet72_36, 3, 16, 8 * (SXSUM + XJSUM + 2), 2, 18);
            //锁定表格
            fc.Sheet_Locked(Sheet72_36);

        }
        private void Sheet72_36add(FarPoint.Win.Spread.SheetView obj_sheet, int i)
        {
            obj_sheet.SetValue(i, 3, "变电站(座)");
            obj_sheet.SetValue(i + 1, 3, "变压器（台）");
            obj_sheet.SetValue(i + 2, 3, "变电容量（MVA）");
            obj_sheet.SetValue(i + 3, 3, "无功补偿容量（Mvar）");
            obj_sheet.SetValue(i + 4, 3, "变电站(座)");
            obj_sheet.SetValue(i + 5, 3, "变压器（台）");
            obj_sheet.SetValue(i + 6, 3, "变电容量（MVA）");
            obj_sheet.SetValue(i + 7, 3, "无功补偿容量（Mvar）");
            obj_sheet.AddSpanCell(i, 2, 4, 1);
            obj_sheet.SetValue(i, 2, "110(66)");
            obj_sheet.AddSpanCell(i + 4, 2, 4, 1);
            obj_sheet.SetValue(i + 4, 2, "35");
        }
        private void Sheet72_36adddata( FarPoint.Win.Spread.SheetView obj_sheet,int row,string DQtiaojian)
        {
            string diqu = obj_sheet.Cells[row, 1].Value.ToString();
            for (int i = 4; i < 16; i++)
            {
                #region  新建数据
                //向表写入diqu110（66）变电站台数 新建
                string BDZtiaojian1 = " ProjectID='"+ProjID+"' and BuildEd='" + (2010 + (i - 4) / 2) + "' and Col3='新建' and Col4='bian' and  AreaName='" + diqu +"'"+DQtiaojian+ "  and (BianInfo like '110@%' or BianInfo like '66@%')";
                double BDZsum1 = 0;
                if (Services.BaseService.GetObject("SelectTZGSSUMnum5", BDZtiaojian1) == null)
                {
                    BDZsum1 = 0;
                }
                else
                {
                    BDZsum1 = (double)Services.BaseService.GetObject("SelectTZGSSUMnum5", BDZtiaojian1);
                }
                obj_sheet.SetValue(row + 0, i, BDZsum1);
                //向表写入diqu110（66）主变台数
                double ZBsum1 = 0;
                if (Services.BaseService.GetObject("SelectTZGSSUMnum1", BDZtiaojian1) == null)
                {
                    ZBsum1 = 0;
                }
                else
                {
                    ZBsum1 = (double)Services.BaseService.GetObject("SelectTZGSSUMnum1", BDZtiaojian1);
                }
                obj_sheet.SetValue(row + 1, i, ZBsum1);
                //向表写入diqu110（66）变电容量
                double BDRLsum1 = 0;
                if (Services.BaseService.GetObject("SelectTZGSSUMVolumn", BDZtiaojian1) == null)
                {
                    BDRLsum1 = 0;
                }
                else
                {
                    BDRLsum1 = (double)Services.BaseService.GetObject("SelectTZGSSUMVolumn", BDZtiaojian1);
                }
                obj_sheet.SetValue(row + 2, i, BDRLsum1);
                //向表写入diqu110（66）无功补尝容量
                double WGBCsum1 = 0;
                if (Services.BaseService.GetObject("SelectTZGSSUMWGNum", BDZtiaojian1) == null)
                {
                    WGBCsum1 = 0;
                }
                else
                {
                    WGBCsum1 = (double)Services.BaseService.GetObject("SelectTZGSSUMWGNum", BDZtiaojian1);
                }
                obj_sheet.SetValue(row + 3, i, WGBCsum1);

                //向表写入diqu35变电站台数 新建
                string BDZtiaojian2 = " ProjectID='"+ProjID+"' and BuildEd='" + (2010 + (i - 4) / 2) + "' and Col3='新建' and Col4='bian' and  AreaName='" + diqu + "'" + DQtiaojian + "  and BianInfo like '35@%'";
                double BDZsum2 = 0;
                if (Services.BaseService.GetObject("SelectTZGSSUMnum5", BDZtiaojian2) == null)
                {
                    BDZsum2 = 0;
                }
                else
                {
                    BDZsum2 = (double)Services.BaseService.GetObject("SelectTZGSSUMnum5", BDZtiaojian2);
                }
                obj_sheet.SetValue(row + 4, i, BDZsum2);
                //向表写入diqu35主变台数
                double ZBsum2 = 0;
                if (Services.BaseService.GetObject("SelectTZGSSUMnum1", BDZtiaojian2) == null)
                {
                    ZBsum2 = 0;
                }
                else
                {
                    ZBsum2 = (double)Services.BaseService.GetObject("SelectTZGSSUMnum1", BDZtiaojian2);
                }
                obj_sheet.SetValue(row + 5, i, ZBsum2);
                //向表写入diqu35变电容量
                double BDRLsum2 = 0;
                if (Services.BaseService.GetObject("SelectTZGSSUMVolumn", BDZtiaojian2) == null)
                {
                    BDRLsum2 = 0;
                }
                else
                {
                    BDRLsum2 = (double)Services.BaseService.GetObject("SelectTZGSSUMVolumn", BDZtiaojian2);
                }
                obj_sheet.SetValue(row + 6, i, BDRLsum2);
                //向表写入diqu35无功补尝容量
                double WGBCsum2 = 0;
                if (Services.BaseService.GetObject("SelectTZGSSUMWGNum", BDZtiaojian2) == null)
                {
                    WGBCsum2 = 0;
                }
                else
                {
                    WGBCsum2 = (double)Services.BaseService.GetObject("SelectTZGSSUMWGNum", BDZtiaojian2);
                }
                obj_sheet.SetValue(row + 7, i, WGBCsum2);
                #endregion
                #region 扩建数据
                //向表写入diqu110（66）变电站台数  扩建
                string BDZtiaojian3 = " ProjectID='"+ProjID+"' and BuildEd='" + (2010 + (i - 4) / 2) + "' and Col3='扩建' and Col4='bian' and  AreaName='" + diqu + "'" + DQtiaojian + "  and (BianInfo like '110@%' or BianInfo like '66@%')";
                double BDZsum3 = 0;
                if (Services.BaseService.GetObject("SelectTZGSSUMnum5", BDZtiaojian3) == null)
                {
                    BDZsum3 = 0;
                }
                else
                {
                    BDZsum3 = (double)Services.BaseService.GetObject("SelectTZGSSUMnum5", BDZtiaojian3);
                }
                obj_sheet.SetValue(row + 0, i + 1, BDZsum3);
                //向表写入diqu110（66）主变台数
                double ZBsum3 = 0;
                if (Services.BaseService.GetObject("SelectTZGSSUMnum1", BDZtiaojian3) == null)
                {
                    ZBsum3 = 0;
                }
                else
                {
                    ZBsum3 = (double)Services.BaseService.GetObject("SelectTZGSSUMnum1", BDZtiaojian3);
                }
                obj_sheet.SetValue(row + 1, i + 1, ZBsum3);
                //向表写入diqu110（66）变电容量
                double BDRLsum3 = 0;
                if (Services.BaseService.GetObject("SelectTZGSSUMVolumn", BDZtiaojian3) == null)
                {
                    BDRLsum3 = 0;
                }
                else
                {
                    BDRLsum3 = (double)Services.BaseService.GetObject("SelectTZGSSUMVolumn", BDZtiaojian3);
                }
                obj_sheet.SetValue(row + 2, i + 1, BDRLsum3);
                //向表写入diqu110（66）无功补尝容量
                double WGBCsum3 = 0;
                if (Services.BaseService.GetObject("SelectTZGSSUMWGNum", BDZtiaojian3) == null)
                {
                    WGBCsum3 = 0;
                }
                else
                {
                    WGBCsum3 = (double)Services.BaseService.GetObject("SelectTZGSSUMWGNum", BDZtiaojian3);
                }
                obj_sheet.SetValue(row + 3, i + 1, WGBCsum3);

                //向表写入diqu35变电站台数 新建
                string BDZtiaojian4 = " ProjectID='"+ProjID+"' and BuildEd='" + (2010 + (i - 4) / 2) + "' and Col3='扩建' and Col4='bian' and  AreaName='" + diqu + "'" + DQtiaojian + "  and BianInfo like '35@%'";
                double BDZsum4 = 0;
                if (Services.BaseService.GetObject("SelectTZGSSUMnum5", BDZtiaojian4) == null)
                {
                    BDZsum4 = 0;
                }
                else
                {
                    BDZsum4 = (double)Services.BaseService.GetObject("SelectTZGSSUMnum5", BDZtiaojian4);
                }
                obj_sheet.SetValue(row + 4, i + 1, BDZsum4);
                //向表写入diqu35主变台数
                double ZBsum4 = 0;
                if (Services.BaseService.GetObject("SelectTZGSSUMnum1", BDZtiaojian4) == null)
                {
                    ZBsum4 = 0;
                }
                else
                {
                    ZBsum4 = (double)Services.BaseService.GetObject("SelectTZGSSUMnum1", BDZtiaojian4);
                }
                obj_sheet.SetValue(row + 5, i + 1, ZBsum4);
                //向表写入diqu35变电容量
                double BDRLsum4 = 0;
                if (Services.BaseService.GetObject("SelectTZGSSUMVolumn", BDZtiaojian4) == null)
                {
                    BDRLsum4 = 0;
                }
                else
                {
                    BDRLsum4 = (double)Services.BaseService.GetObject("SelectTZGSSUMVolumn", BDZtiaojian4);
                }
                obj_sheet.SetValue(row + 6, i + 1, BDRLsum4);
                //向表写入diqu35无功补尝容量
                double WGBCsum4 = 0;
                if (Services.BaseService.GetObject("SelectTZGSSUMWGNum", BDZtiaojian4) == null)
                {
                    WGBCsum4 = 0;
                }
                else
                {
                    WGBCsum4 = (double)Services.BaseService.GetObject("SelectTZGSSUMWGNum", BDZtiaojian4);
                }
                obj_sheet.SetValue(row + 7, i + 1, WGBCsum4);
                #endregion
                i++;

            }
        }
        private void build_Sheet73(FarPoint.Win.Spread.SheetView Sheet73)
        {
            //工作表为11列44行
            Sheet73.Columns.Count = 11;
            Sheet73.Rows.Count = 44;
            //设表格线和居中
            fc.Sheet_GridandCenter(Sheet73);
            fc.Sheet_Referen_R1C1(Sheet73);
         
            //设定表格中第1列表列和第9列宽度
            Sheet73.Columns[0].Width = 50;
            Sheet73.Columns[1].Width = 100;
            Sheet73.Columns[2].Width = 85;
            Sheet73.Columns[3].Width = 90;
            Sheet73.Columns[10].Width = 95;
            Sheet73.Rows[0].Height = 35;
            //合并第一行的所有列做为表头
            Sheet73.AddSpanCell(0, 0, 1, 11);
            //向表中写入相关的标题
            Sheet73.SetValue(0, 0, "表7-3  铜陵市高压配电网规划改造工程主要规模");
            Sheet73.SetValue(1, 0, "编号");
            Sheet73.SetValue(1, 1, "类型");
            Sheet73.SetValue(1, 2, "电压等级(KV)");
            Sheet73.SetValue(1, 3, "项  目");
            //循环写入年份
            for (int i = 4; i < 11; i++)
            {
                Sheet73.SetValue(1, i, i + 2006 + "年");
            }
            Sheet73.SetValue(1, 10, "“十二五”合计");
            //写入市辖供电区表格部分
            int jishu = 0;
            Sheet73_37add(Sheet73, 2 + jishu*6);
            Sheet73.AddSpanCell(2 + jishu * 6, 1, 6, 1);
            Sheet73.SetValue(2 + jishu * 6, 1, "市辖供电区");
            Sheet73.AddSpanCell(2 + jishu * 6, 0, 6, 1);
            Sheet73.SetValue(2 + jishu * 6, 0, "1");
            //县级供电区
            jishu++;
            Sheet73_37add(Sheet73, 2 + jishu * 6);
            Sheet73.AddSpanCell(2 + jishu * 6, 1, 6, 1);
            Sheet73.SetValue(2 + jishu * 6, 1, "县级供电区");
            Sheet73.AddSpanCell(2 + jishu * 6, 0, 6, 1);
            Sheet73.SetValue(2 + jishu * 6, 0, "2");
            //其中：直供直管
            jishu++;
            Sheet73_37add(Sheet73, 2 + jishu * 6);
            Sheet73.AddSpanCell(2 + jishu * 6, 1, 6, 1);
            Sheet73.SetValue(2 + jishu * 6, 1, "其中：直供直管");
            Sheet73.AddSpanCell(2 + jishu * 6, 0, 6, 1);
            Sheet73.SetValue(2 + jishu * 6, 0, "2.1");
            //县级控股
            jishu++;
            Sheet73_37add(Sheet73, 2 + jishu * 6);
            Sheet73.AddSpanCell(2 + jishu * 6, 1, 6, 1);
            Sheet73.SetValue(2 + jishu * 6, 1, "控股");
            Sheet73.AddSpanCell(2 + jishu * 6, 0, 6, 1);
            Sheet73.SetValue(2 + jishu * 6, 0, "2.2");
            //县级参股
            jishu++;
            Sheet73_37add(Sheet73, 2 + jishu * 6);
            Sheet73.AddSpanCell(2 + jishu * 6, 1, 6, 1);
            Sheet73.SetValue(2 + jishu * 6, 1, "参股");
            Sheet73.AddSpanCell(2 + jishu * 6, 0, 6, 1);
            Sheet73.SetValue(2 + jishu * 6, 0, "2.3");
            //县级代管
            jishu++;
            Sheet73_37add(Sheet73, 2 + jishu * 6);
            Sheet73.AddSpanCell(2 + jishu * 6, 1, 6, 1);
            Sheet73.SetValue(2 + jishu * 6, 1, "代管");
            Sheet73.AddSpanCell(2 + jishu * 6, 0, 6, 1);
            Sheet73.SetValue(2 + jishu * 6, 0, "2.4");
            //全地区合计
            jishu++;
            Sheet73_37add(Sheet73, 2 + jishu * 6);
            Sheet73.AddSpanCell(2 + jishu * 6, 1, 6, 1);
            Sheet73.SetValue(2 + jishu * 6, 1, "全地区合计");
            Sheet73.AddSpanCell(2 + jishu * 6, 0, 6, 1);
            Sheet73.SetValue(2 + jishu * 6, 0, "3");
            adddata_sheet73(Sheet73);
            //锁定表格
            fc.Sheet_Locked(Sheet73);
        }
        private void adddata_sheet73(FarPoint.Win.Spread.SheetView obj_sheet)
        {
            //Sheet73即为表7-3
            //向表7-3写入市辖供电区数据
            adddata_sheet73_diqu(obj_sheet,"市辖供电区", 2);
            //向表7-3写入县级直供直管数据
            adddata_sheet73_diqu(obj_sheet, "县级直供直管", 14);
            //向表7-3写入县级控股数据
            adddata_sheet73_diqu(obj_sheet, "县级控股", 20);
            //向表7-3写入县级参股数据
            adddata_sheet73_diqu(obj_sheet, "县级参股", 26);
            //向表7-3写入县级代管数据
            adddata_sheet73_diqu(obj_sheet, "县级代管", 32);
            //写入计算县级供电区数据（求和）公式
            fc.Sheet_WriteFormula_RowSum(obj_sheet, 14, 4, 4, 6, 8, 4, 6);
            //写入计算全地区合计数据（求和）公式
            fc.Sheet_WriteFormula_RowSum(obj_sheet, 2, 4, 2, 6, 38, 4, 6);
            //写入计算十二五合计公式
            fc.Sheet_WriteFormula_ColSum_WritOne(obj_sheet, 2, 5, 42, 5, 10);
           
            
        }
        private void adddata_sheet73_diqu(FarPoint.Win.Spread.SheetView obj_sheet,string diqu, int row)
        {   //根据地区不同向表7-3写入数据
            //diqu参数为表7-3的类型，可以为市辖供电区，县级直供直管等
            //row参数为要写入数据的地区开始行号
            for (int i = 4; i < 10; i++)
            {   //向表写入diqu110（66）变电站台数
                string BDZtiaojian1 = " ProjectID='"+ProjID+"' and BuildEd='" + (i + 2006) + "'and Col3='改造' and Col4='bian' and  DQ='" + diqu + "' and (BianInfo like '110@%' or BianInfo like '66@%')";
                double BDZsum1 = 0;
                if (Services.BaseService.GetObject("SelectTZGSSUMnum5", BDZtiaojian1)==null)
                {
                    BDZsum1 = 0;
                }
                else
                {
                    BDZsum1 = (double)Services.BaseService.GetObject("SelectTZGSSUMnum5", BDZtiaojian1);
                }
                obj_sheet.SetValue(row + 0, i, BDZsum1);
                //向表写入diqu110（66）主变台数
                double ZBsum1 = 0;
                if (Services.BaseService.GetObject("SelectTZGSSUMnum1", BDZtiaojian1) == null)
                {
                    ZBsum1 = 0;
                }
                else
                {
                    ZBsum1 = (double)Services.BaseService.GetObject("SelectTZGSSUMnum1", BDZtiaojian1);
                }
                obj_sheet.SetValue(row + 1, i, ZBsum1);
                //向表写入diqu110（66）断路器台数
                double DLQsum1 = 0;
                if (Services.BaseService.GetObject("SelectTZGSSUMnum2", BDZtiaojian1) == null)
                {
                    DLQsum1 = 0;
                }
                else
                {
                    DLQsum1 = (double)Services.BaseService.GetObject("SelectTZGSSUMnum2", BDZtiaojian1);
                }
                obj_sheet.SetValue(row + 2, i, DLQsum1);

                string BDZtiaojian2 = " ProjectID='"+ProjID+"' and BuildEd='" + (i + 2006) + "'and Col3='改造'  and Col4='bian' and  DQ='" + diqu + "' and BianInfo like '35@%'";
                //向表写入diqu35变电站台数
                double BDZsum2 = 0;
                if (Services.BaseService.GetObject("SelectTZGSSUMnum5", BDZtiaojian2) == null)
                {
                    BDZsum2 = 0;
                }
                else
                {
                    BDZsum2 = (double)Services.BaseService.GetObject("SelectTZGSSUMnum5", BDZtiaojian2);
                }
                obj_sheet.SetValue(row + 3, i, BDZsum2);
                //向表写入diqu35主变台数
                double ZBsum2 = 0;
                if (Services.BaseService.GetObject("SelectTZGSSUMnum1", BDZtiaojian2) == null)
                {
                    ZBsum2 = 0;
                }
                else
                {
                    ZBsum2 = (double)Services.BaseService.GetObject("SelectTZGSSUMnum1", BDZtiaojian2);
                }
                obj_sheet.SetValue(row + 4, i, ZBsum2);
                //向表写入diqu35断路器台数
                double DLQsum2 = 0;
                if (Services.BaseService.GetObject("SelectTZGSSUMnum2", BDZtiaojian2) == null)
                {
                    DLQsum2 = 0;
                }
                else
                {
                    DLQsum2 = (double)Services.BaseService.GetObject("SelectTZGSSUMnum2", BDZtiaojian2);
                }
                obj_sheet.SetValue(row + 5, i, DLQsum2);


            }
        }
        private void build_Sheet73_37(FarPoint.Win.Spread.SheetView Sheet73_37)
        {
            //工作表为11列
            Sheet73_37.Columns.Count = 11;
            ////查询市辖供电区分区
            string SXtiaojian = " ProjectID='"+ProjID+"' and BuildEd between 2010 and 2015 and AreaName!='' and Col3='改造' and (BianInfo like '110@%' or  BianInfo like '66@%' or BianInfo like '35@%') and  Col4='bian' and DQ='市辖供电区' group by AreaName";
            IList SXlist = Services.BaseService.GetList("SelectTZGSAreaName", SXtiaojian);
            ////查询县级供电分区
            string XJtiaojian = " ProjectID='"+ProjID+"' and BuildEd between 2010 and 2015 and AreaName!='' and Col3='改造' and (BianInfo like '110@%' or  BianInfo like '66@%' or BianInfo like '35@%')  and  Col4='bian' and DQ!='市辖供电区' group by AreaName";
            IList XJlist = Services.BaseService.GetList("SelectTZGSAreaName", XJtiaojian);
            ////记录分区数
            int SXSUM = SXlist.Count;
            int XJSUM = XJlist.Count;
            //根据分区数据确定表格的行数，每增加一个分区增加6行
            Sheet73_37.Rows.Count = 2 + 6 * 2 + 6 * (SXSUM + XJSUM);
            //设表格线和居中
            fc.Sheet_GridandCenter(Sheet73_37);
            fc.Sheet_Referen_R1C1(Sheet73_37);
            //设定表格中第1列表列和第9列宽度
            Sheet73_37.Columns[0].Width = 92;
            Sheet73_37.Columns[1].Width = 100;
            Sheet73_37.Columns[2].Width = 85;
            Sheet73_37.Columns[3].Width = 90;
            Sheet73_37.Columns[10].Width = 95;
            Sheet73_37.Rows[0].Height = 35;
            //合并第一行的所有列做为表头
            Sheet73_37.AddSpanCell(0, 0, 1, 11);
            //向表中写入相关的标题
            Sheet73_37.SetValue(0, 0, "附表37  铜陵市高压配电网规划方案变电工程量统计（改造部分）");
            Sheet73_37.SetValue(1, 0, "分区类型");
            Sheet73_37.SetValue(1, 1, "分区名称");
            Sheet73_37.SetValue(1, 2, "电压等级(KV)");
            Sheet73_37.SetValue(1, 3, "项  目");
            //循环写入年份
            for (int i = 4; i < 11; i++)
            {
                Sheet73_37.SetValue(1, i, i + 2006 + "年");
            }
            Sheet73_37.SetValue(1, 10, "“十二五”合计");
            //写入市辖供电区合计表格部分
            Sheet73_37add(Sheet73_37, 2);
            Sheet73_37.AddSpanCell(2, 1, 6, 1);
            Sheet73_37.SetValue(2, 1, "合计");
            //循环写入市辖供电区分区和相应的项目
            if (SXSUM != 0)
            {
                for (int j = 1; j <= SXSUM; j++)
                {
                    Sheet73_37add(Sheet73_37, 2 + j * 6);
                    Sheet73_37.AddSpanCell(2 + j * 6, 1, 6, 1);
                    Sheet73_37.SetValue(2 + j * 6, 1, SXlist[j - 1]);
                }
            }

            Sheet73_37.AddSpanCell(2, 0, 6 * (SXSUM + 1), 1);
            Sheet73_37.SetValue(2, 0, "市辖供电区");
            //写入县级供电区合计表格部分
            Sheet73_37add(Sheet73_37, 2 + 6 * (SXSUM + 1));
            Sheet73_37.AddSpanCell(2 + 6 * (SXSUM + 1), 1, 6, 1);
            Sheet73_37.SetValue(2 + 6 * (SXSUM + 1), 1, "合计");
            //循环写入县级供电区分区和相应的项目
            if (XJSUM != 0)
            {
                for (int j = 1; j <= XJSUM; j++)
                {
                    Sheet73_37add(Sheet73_37, 2 + (j + SXSUM + 1) * 6);
                    Sheet73_37.AddSpanCell(2 + (j + SXSUM + 1) * 6, 1, 6, 1);
                    Sheet73_37.SetValue(2 + (j + SXSUM + 1) * 6, 1, XJlist[j - 1]);
                }
            }
            Sheet73_37.AddSpanCell(2 + 6 * (SXSUM + 1), 0, 6 * (XJSUM + 1), 1);
            Sheet73_37.SetValue(2 + 6 * (SXSUM + 1), 0, "县级供电区");
            //从数据库提取数据写入表格   
            
            if (SXSUM != 0)
            {
                for (int sum = 1; sum <= SXSUM ; sum++)
                {
                    //向表中写入市辖分区数据
                    Sheet73_37adddata(Sheet73_37, 2 + sum * 6, " and DQ='市辖供电区' ");

                }
                //写入市辖分区合计部分公式
                fc.Sheet_WriteFormula_RowSum(Sheet73_37, 8, 4, SXSUM, 6, 2, 4, 6);
            }
            else
            {
                ////如果无市辖地区，合计部分直接写0
                Sheet_WriteZero(Sheet73_37, 2, 4, 6, 6);
            }

            if (XJSUM != 0)
            {
                for (int sum = SXSUM+2; sum <= SXSUM+XJSUM+1; sum++)
                {
                    //向表中写入县级分区数据
                    Sheet73_37adddata(Sheet73_37, 2 + sum * 6, " and DQ!='市辖供电区' ");

                }
                //写入县级分区合计部分公式
                fc.Sheet_WriteFormula_RowSum(Sheet73_37, 2 + (SXSUM + 2) * 6, 4, XJSUM, 6, 2 + (SXSUM + 1) * 6, 4, 6);
            }
            else
            {
                //如果无县级分区，合计部分直接写0
                Sheet_WriteZero(Sheet73_37, 2 + (SXSUM + 1) * 6, 4, 6, 6);
            }  
            //写十二五合计公式
            fc.Sheet_WriteFormula_ColSum_WritOne(Sheet73_37, 2, 5, 6 * (SXSUM + XJSUM + 2), 5, 10);
            //锁定表格
            fc.Sheet_Locked(Sheet73_37);

        }
        private void Sheet73_37add(FarPoint.Win.Spread.SheetView obj_sheet, int i)
        {
            obj_sheet.SetValue(i, 3, "变电站(座)");
            obj_sheet.SetValue(i + 1, 3, "主变压器(台)");
            obj_sheet.SetValue(i + 2, 3, "断路器(台)");
            obj_sheet.SetValue(i + 3, 3, "变电站(座)");
            obj_sheet.SetValue(i + 4, 3, "主变压器(台)");
            obj_sheet.SetValue(i + 5, 3, "断路器(台)");
            obj_sheet.AddSpanCell(i, 2, 3, 1);
            obj_sheet.SetValue(i, 2, "110(66)");
            obj_sheet.AddSpanCell(i + 3, 2, 3, 1);
            obj_sheet.SetValue(i + 3, 2, "35");
        }
        private void Sheet73_37adddata(FarPoint.Win.Spread.SheetView obj_sheet, int row,String DQtiaojian)
        {
            string diqu = obj_sheet.Cells[row, 1].Value.ToString();
            for (int i = 4; i < 10; i++)
            {   //向表写入diqu110（66）变电站台数
                string BDZtiaojian1 = " ProjectID='"+ProjID+"' and BuildEd='" + (i + 2006) + "'and Col3='改造'  and Col4='bian' and  AreaName='" + diqu+ "'"+DQtiaojian+" and (BianInfo like '110@%' or BianInfo like '66@%')";
                double BDZsum1 = 0;
                if (Services.BaseService.GetObject("SelectTZGSSUMnum5", BDZtiaojian1) == null)
                {
                    BDZsum1 = 0;
                }
                else
                {
                    BDZsum1 = (double)Services.BaseService.GetObject("SelectTZGSSUMnum5", BDZtiaojian1);
                }
                obj_sheet.SetValue(row + 0, i, BDZsum1);
                //向表写入diqu110（66）主变台数
                double ZBsum1 = 0;
                if (Services.BaseService.GetObject("SelectTZGSSUMnum1", BDZtiaojian1) == null)
                {
                    ZBsum1 = 0;
                }
                else
                {
                    ZBsum1 = (double)Services.BaseService.GetObject("SelectTZGSSUMnum1", BDZtiaojian1);
                }
                obj_sheet.SetValue(row + 1, i, ZBsum1);
                //向表写入diqu110（66）断路器台数
                double DLQsum1 = 0;
                if (Services.BaseService.GetObject("SelectTZGSSUMnum2", BDZtiaojian1) == null)
                {
                    DLQsum1 = 0;
                }
                else
                {
                    DLQsum1 = (double)Services.BaseService.GetObject("SelectTZGSSUMnum2", BDZtiaojian1);
                }
                obj_sheet.SetValue(row + 2, i, DLQsum1);

                string BDZtiaojian2 = " ProjectID='"+ProjID+"' and BuildEd='" + (i + 2006) + "'and Col3='改造'  and Col4='bian' and  AreaName='" + diqu +"'"+DQtiaojian+ " and BianInfo like '35@%'";
                //向表写入diqu35变电站台数
                double BDZsum2 = 0;
                if (Services.BaseService.GetObject("SelectTZGSSUMnum5", BDZtiaojian2) == null)
                {
                    BDZsum2 = 0;
                }
                else
                {
                    BDZsum2 = (double)Services.BaseService.GetObject("SelectTZGSSUMnum5", BDZtiaojian2);
                }
                obj_sheet.SetValue(row + 3, i, BDZsum2);
                //向表写入diqu35主变台数
                double ZBsum2 = 0;
                if (Services.BaseService.GetObject("SelectTZGSSUMnum1", BDZtiaojian2) == null)
                {
                    ZBsum2 = 0;
                }
                else
                {
                    ZBsum2 = (double)Services.BaseService.GetObject("SelectTZGSSUMnum1", BDZtiaojian2);
                }
                obj_sheet.SetValue(row + 4, i, ZBsum2);
                //向表写入diqu35断路器台数
                double DLQsum2 = 0;
                if (Services.BaseService.GetObject("SelectTZGSSUMnum2", BDZtiaojian2) == null)
                {
                    DLQsum2 = 0;
                }
                else
                {
                    DLQsum2 = (double)Services.BaseService.GetObject("SelectTZGSSUMnum2", BDZtiaojian2);
                }
                obj_sheet.SetValue(row + 5, i, DLQsum2);
            }
        }
        private void build_Sheet74(FarPoint.Win.Spread.SheetView Sheet74)
        {
            //工作表为11列44行
            Sheet74.Columns.Count = 11;
            Sheet74.Rows.Count = 44;
            //设表格线和居中
            fc.Sheet_GridandCenter(Sheet74);
            //模式为R1C1
            fc.Sheet_Referen_R1C1(Sheet74);
            //设定表格中第1列表列和第9列宽度
            Sheet74.Columns[0].Width = 50;
            Sheet74.Columns[1].Width = 100;
            Sheet74.Columns[2].Width = 85;
            Sheet74.Columns[3].Width = 90;
            Sheet74.Columns[10].Width = 95;
            Sheet74.Rows[0].Height = 35;
            //合并第一行的所有列做为表头
            Sheet74.AddSpanCell(0, 0, 1, 11);
            //向表中写入相关的标题
            Sheet74.SetValue(0, 0, "表7-4  铜陵市高压配电网规划方案新建线路工程量汇总");
            Sheet74.SetValue(1, 0, "编号");
            Sheet74.SetValue(1, 1, "类型");
            Sheet74.SetValue(1, 2, "电压等级(KV)");
            Sheet74.SetValue(1, 3, "项  目");
            //循环写入年份
            for (int i = 4; i < 11; i++)
            {
                Sheet74.SetValue(1, i, i + 2006 + "年");
            }
            Sheet74.SetValue(1, 10, "“十二五”合计");
            //写入市辖供电区表格部分
            int jishu = 0;
           Sheet74_38add(Sheet74, 2 + jishu * 6);
            Sheet74.AddSpanCell(2 + jishu * 6, 1, 6, 1);
            Sheet74.SetValue(2 + jishu * 6, 1, "市辖供电区");
            Sheet74.AddSpanCell(2 + jishu * 6, 0, 6, 1);
            Sheet74.SetValue(2 + jishu * 6, 0, "1");
            //县级供电区
            jishu++;
           Sheet74_38add(Sheet74, 2 + jishu * 6);
            Sheet74.AddSpanCell(2 + jishu * 6, 1, 6, 1);
            Sheet74.SetValue(2 + jishu * 6, 1, "县级供电区");
            Sheet74.AddSpanCell(2 + jishu * 6, 0, 6, 1);
            Sheet74.SetValue(2 + jishu * 6, 0, "2");
            //其中：直供直管
            jishu++;
           Sheet74_38add(Sheet74, 2 + jishu * 6);
            Sheet74.AddSpanCell(2 + jishu * 6, 1, 6, 1);
            Sheet74.SetValue(2 + jishu * 6, 1, "其中：直供直管");
            Sheet74.AddSpanCell(2 + jishu * 6, 0, 6, 1);
            Sheet74.SetValue(2 + jishu * 6, 0, "2.1");
            //县级控股
            jishu++;
           Sheet74_38add(Sheet74, 2 + jishu * 6);
            Sheet74.AddSpanCell(2 + jishu * 6, 1, 6, 1);
            Sheet74.SetValue(2 + jishu * 6, 1, "控股");
            Sheet74.AddSpanCell(2 + jishu * 6, 0, 6, 1);
            Sheet74.SetValue(2 + jishu * 6, 0, "2.2");
            //县级参股
            jishu++;
           Sheet74_38add(Sheet74, 2 + jishu * 6);
            Sheet74.AddSpanCell(2 + jishu * 6, 1, 6, 1);
            Sheet74.SetValue(2 + jishu * 6, 1, "参股");
            Sheet74.AddSpanCell(2 + jishu * 6, 0, 6, 1);
            Sheet74.SetValue(2 + jishu * 6, 0, "2.3");
            //县级代管
            jishu++;
           Sheet74_38add(Sheet74, 2 + jishu * 6);
            Sheet74.AddSpanCell(2 + jishu * 6, 1, 6, 1);
            Sheet74.SetValue(2 + jishu * 6, 1, "代管");
            Sheet74.AddSpanCell(2 + jishu * 6, 0, 6, 1);
            Sheet74.SetValue(2 + jishu * 6, 0, "2.4");
            //全地区合计
            jishu++;
           Sheet74_38add(Sheet74, 2 + jishu * 6);
            Sheet74.AddSpanCell(2 + jishu * 6, 1, 6, 1);
            Sheet74.SetValue(2 + jishu * 6, 1, "全地区合计");
            Sheet74.AddSpanCell(2 + jishu * 6, 0, 6, 1);
            Sheet74.SetValue(2 + jishu * 6, 0, "3");
            adddata_sheet74(Sheet74);
            //锁定表格
            fc.Sheet_Locked(Sheet74);
           
        }
        private void adddata_sheet74(FarPoint.Win.Spread.SheetView obj_sheet)
        {
            //fpSpread1_Sheet74即为表7-4
            //向表7-4写入市辖供电区数据
            adddata_sheet74_diqu(obj_sheet,"市辖供电区", 2);
            //向表7-4写入县级直供直管数据
            adddata_sheet74_diqu(obj_sheet, "县级直供直管", 14);
            //向表7-4写入县级控股数据
            adddata_sheet74_diqu(obj_sheet, "县级控股", 20);
            //向表7-4写入县级参股数据
            adddata_sheet74_diqu(obj_sheet, "县级参股", 26);
            //向表7-4写入县级代管数据
            adddata_sheet74_diqu(obj_sheet, "县级代管", 32);
            //写入计算县极供电区数据公式（求和）
            fc.Sheet_WriteFormula_RowSum(obj_sheet, 14, 4, 4, 6, 8, 4, 6);
            //写入计算全地区合计数据公式（求和）
            fc.Sheet_WriteFormula_RowSum(obj_sheet, 2, 4, 2, 6, 38, 4, 6);
            //写入计算十二五合计公式
            fc.Sheet_WriteFormula_ColSum_WritOne(obj_sheet, 2, 5, 42, 5, 10);
           
        }
        private void adddata_sheet74_diqu(FarPoint.Win.Spread.SheetView obj_sheet,string diqu, int row)
        {   //根据地区不同向表7-4写入数据
            //diqu参数为表7-4的类型，可以为市辖供电区，县级直供直管等
            //row参数为要写入数据的地区开始行号
            for (int i = 4; i < 10; i++)
            {   //向表写入diqu110（66）线路条数
                string XLtiaojian1 = " ProjectID='"+ProjID+"' and BuildEd='" + (i + 2006) + "' and Col3='新建' and Col4='line' and  DQ='" + diqu + "' and (LineInfo like '110@%' or LineInfo like '66@%')";
                double XLsum1 = 0;
                if (Services.BaseService.GetObject("SelectTZGSSUMnum5", XLtiaojian1) == null)
                {
                    XLsum1 = 0;
                }
                else
                {
                    XLsum1 = (double)Services.BaseService.GetObject("SelectTZGSSUMnum5", XLtiaojian1);
                }
                obj_sheet.SetValue(row + 0, i, XLsum1);
                //向表写入diqu110（66）架空线长
                string JKXtiaojian1 = " ProjectID='"+ProjID+"' and BuildEd='" + (i + 2006) + "'and Col3='新建'  and Col4='line' and  DQ='" + diqu + "' and (LineInfo like '110@%' or LineInfo like '66@%')";
                double JKlenth1 = 0;
                if (Services.BaseService.GetObject("SelectTZGSSUMLength", JKXtiaojian1) == null)
                {
                    JKlenth1 = 0;
                }
                else
                {
                    JKlenth1 = (double)Services.BaseService.GetObject("SelectTZGSSUMLength", JKXtiaojian1);
                }
                obj_sheet.SetValue(row + 1, i, JKlenth1);
                //向表写入diqu110（66）电缆线长度
                string DLXtiaojian1 = " ProjectID='"+ProjID+"' and BuildEd='" + (i + 2006) + "'and Col3='新建'  and Col4='line' and  DQ='" + diqu + "' and (LineInfo like '110@%' or LineInfo like '66@%')";
                double DLXlenth1 = 0;
                if (Services.BaseService.GetObject("SelectTZGSSUMLength2", DLXtiaojian1) == null)
                {
                    DLXlenth1 = 0;
                }
                else
                {
                    DLXlenth1 = (double)Services.BaseService.GetObject("SelectTZGSSUMLength2", DLXtiaojian1);
                }
                obj_sheet.SetValue(row + 2, i, DLXlenth1);

                //向表写入diqu35线路条数
                string XLtiaojian2 = " ProjectID='"+ProjID+"' and BuildEd='" + (i + 2006) + "'and Col3='新建'  and Col4='line' and  DQ='" + diqu + "' and LineInfo like '35@%'";
                double XLsum2 = 0;
                if (Services.BaseService.GetObject("SelectTZGSSUMnum5", XLtiaojian2) == null)
                {
                    XLsum2 = 0;
                }
                else
                {
                    XLsum2 = (double)Services.BaseService.GetObject("SelectTZGSSUMnum5", XLtiaojian2);
                }
                obj_sheet.SetValue(row + 3, i, XLsum2);

                //向表写入diqu35架空线长
                string JKXtiaojian2 = " ProjectID='"+ProjID+"' and BuildEd='" + (i + 2006) + "'and Col3='新建'  and Col4='line' and  DQ='" + diqu + "' and LineInfo like '35@%' ";
                double JKlenth2 = 0;
                if (Services.BaseService.GetObject("SelectTZGSSUMLength", JKXtiaojian2) == null)
                {
                    JKlenth2 = 0;
                }
                else
                {
                    JKlenth2 = (double)Services.BaseService.GetObject("SelectTZGSSUMLength", JKXtiaojian2);
                }
                obj_sheet.SetValue(row + 4, i, JKlenth2);

                //向表写入diqu35电缆线长
                string DLXtiaojian2 = " ProjectID='"+ProjID+"' and BuildEd='" + (i + 2006) + "'and Col3='新建'  and Col4='line' and  DQ='" + diqu + "' and LineInfo like '35@%'";
                double DLXlenth2 = 0;
                if (Services.BaseService.GetObject("SelectTZGSSUMLength2", DLXtiaojian2) == null)
                {
                    DLXlenth2 = 0;
                }
                else
                {
                    DLXlenth2 = (double)Services.BaseService.GetObject("SelectTZGSSUMLength2", DLXtiaojian2);
                }
                obj_sheet.SetValue(row + 5, i, DLXlenth2);


            }
        }
        private void build_Sheet74_38(FarPoint.Win.Spread.SheetView Sheet74_38 )
        {
            //工作表为11列
            Sheet74_38.Columns.Count = 11;
            //查询市辖供电区分区
            string SXtiaojian = " ProjectID='"+ProjID+"' and BuildEd between 2010 and 2015 and AreaName!='' and Col3='新建' and  Col4='line' and (LineInfo like '110@%' or  LineInfo like '66@%' or LineInfo like '35@%') and DQ='市辖供电区'  group by AreaName";
            IList SXlist = Services.BaseService.GetList("SelectTZGSAreaName", SXtiaojian);
            //查询县级供电分区
            string XJtiaojian = " ProjectID='"+ProjID+"' and BuildEd between 2010 and 2015 and AreaName!='' and Col3='新建' and  Col4='line' and (LineInfo like '110@%' or  LineInfo like '66@%' or LineInfo like '35@%') and DQ!='市辖供电区' group by AreaName";
            IList XJlist = Services.BaseService.GetList("SelectTZGSAreaName", XJtiaojian);
            //记录分区数
            int SXSUM = SXlist.Count;
            int XJSUM = XJlist.Count;
            //根据分区数据确定表格的行数，每增加一个分区增加6行
            Sheet74_38.Rows.Count = 2 + 6 * 2 + 6 * (SXSUM + XJSUM);
            //设表格线和居中
            fc.Sheet_GridandCenter(Sheet74_38);
            //模式为R1C1
            fc.Sheet_Referen_R1C1(Sheet74_38);
            //设定表格中第1列表列和第9列宽度
            Sheet74_38.Columns[0].Width = 92;
            Sheet74_38.Columns[1].Width = 100;
            Sheet74_38.Columns[2].Width = 85;
            Sheet74_38.Columns[3].Width = 90;
            Sheet74_38.Columns[10].Width = 95;
            Sheet74_38.Rows[0].Height = 35;
            //合并第一行的所有列做为表头
            Sheet74_38.AddSpanCell(0, 0, 1, 11);
            //向表中写入相关的标题
            Sheet74_38.SetValue(0, 0, "附表38  铜陵市高压配电网规划方案新建线路工程量统计");
            Sheet74_38.SetValue(1, 0, "分区类型");
            Sheet74_38.SetValue(1, 1, "分区名称");
            Sheet74_38.SetValue(1, 2, "电压等级(KV)");
            Sheet74_38.SetValue(1, 3, "项  目");
            //循环写入年份
            for (int i = 4; i < 10; i++)
            {
                Sheet74_38.SetValue(1, i, i + 2006 + "年");
            }
            Sheet74_38.SetValue(1, 10, "“十二五”合计");
            //写入市辖供电区合计表格部分
            Sheet74_38add(Sheet74_38, 2);
            Sheet74_38.AddSpanCell(2, 1, 6, 1);
            Sheet74_38.SetValue(2, 1, "合计");
            //循环写入市辖供电区分区和相应的项目
            if (SXSUM != 0)
            {
                for (int j = 1; j <= SXSUM; j++)
                {
                    Sheet74_38add(Sheet74_38, 2 + j * 6);
                    Sheet74_38.AddSpanCell(2 + j * 6, 1, 6, 1);
                    Sheet74_38.SetValue(2 + j * 6, 1, SXlist[j - 1]);
                }
            }
            Sheet74_38.AddSpanCell(2, 0, 6 * (SXSUM + 1), 1);
            Sheet74_38.SetValue(2, 0, "市辖供电区");
            //写入县级供电区合计表格部分
            Sheet74_38add(Sheet74_38, 2 + 6 * (SXSUM + 1));
            Sheet74_38.AddSpanCell(2 + 6 * (SXSUM + 1), 1, 6, 1);
            Sheet74_38.SetValue(2 + 6 * (SXSUM + 1), 1, "合计");
            //循环写入县级供电区分区和相应的项目
            if (XJSUM != 0)
            {
                for (int j = 1; j <= XJSUM; j++)
                {
                    Sheet74_38add(Sheet74_38, 2 + (j + SXSUM + 1) * 6);
                    Sheet74_38.AddSpanCell(2 + (j + SXSUM + 1) * 6, 1, 6, 1);
                    Sheet74_38.SetValue(2 + (j + SXSUM + 1) * 6, 1, XJlist[j - 1]);
                }
            }
            Sheet74_38.AddSpanCell(2 + 6 * (SXSUM + 1), 0, 6 * (XJSUM + 1), 1);
            Sheet74_38.SetValue(2 + 6 * (SXSUM + 1), 0, "县级供电区");
            //从数据库提取数据写入表格
            if (SXSUM != 0)
            {
                for (int sum = 1; sum <= SXSUM; sum++)
                {
                   // 向表中写入市辖分区数据
                    Sheet74_38adddata(Sheet74_38, 2 + sum * 6, " and DQ='市辖供电区' ");

                }
                //向表中写入市辖分区合计部分公式
                fc.Sheet_WriteFormula_RowSum(Sheet74_38, 8, 4, SXSUM, 6, 2, 4, 6);
            }
            else
            {
                //如果无市辖地区，合计部分直接写0
                Sheet_WriteZero(Sheet74_38, 2, 4, 6, 6);
            }

            if (XJSUM != 0)
            {
                for (int sum = SXSUM + 2; sum <= SXSUM + XJSUM + 1; sum++)
                {
                   // 向表中写入县级分区数据
                    Sheet74_38adddata(Sheet74_38, 2 + sum * 6, " and DQ!='市辖供电区' ");

                }
                //向表中写入县级分区合计部分公式
                fc.Sheet_WriteFormula_RowSum(Sheet74_38, 2 + (SXSUM + 2) * 6, 4, XJSUM, 6, 2 + (SXSUM + 1) * 6, 4, 6);
            }
            else
            {
                //如果无县级分区，合计部分直接写0
                Sheet_WriteZero(Sheet74_38, 2 + (SXSUM + 1) * 6, 4, 6, 6);
            }  
            fc.Sheet_WriteFormula_ColSum_WritOne(Sheet74_38, 2, 5, 6 * (SXSUM + XJSUM + 2), 5, 10);
            //锁定表格
            fc.Sheet_Locked(Sheet74_38);

        }
        private void Sheet74_38add(FarPoint.Win.Spread.SheetView obj_sheet, int i)
        {
            obj_sheet.SetValue(i, 3, "线路(回)");
            obj_sheet.SetValue(i + 1, 3, "架空线(km)");
            obj_sheet.SetValue(i + 2, 3, "电缆(km)");
            obj_sheet.SetValue(i + 3, 3, "线路(回)");
            obj_sheet.SetValue(i + 4, 3, "架空线(km)");
            obj_sheet.SetValue(i + 5, 3, "电缆(km)");
            obj_sheet.AddSpanCell(i, 2, 3, 1);
            obj_sheet.SetValue(i, 2, "110(66)");
            obj_sheet.AddSpanCell(i + 3, 2, 3, 1);
            obj_sheet.SetValue(i + 3, 2, "35");
        }
        private void Sheet74_38adddata(FarPoint.Win.Spread.SheetView obj_sheet, int row,String DQtiaojian)
        {
            string diqu = obj_sheet.Cells[row, 1].Value.ToString();
            for (int i = 4; i < 10; i++)
            {   //向表写入diqu110（66）线路条数
                string XLtiaojian1 = " ProjectID='"+ProjID+"' and BuildEd='" + (i + 2006) + "' and Col3='新建' and Col4='line' and  AreaName='" + diqu + "'"+ DQtiaojian + " and (LineInfo like '110@%' or LineInfo like '66@%')";
                double XLsum1 = 0;
                if (Services.BaseService.GetObject("SelectTZGSSUMnum5", XLtiaojian1) == null)
                {
                    XLsum1 = 0;
                }
                else
                {
                    XLsum1 = (double)Services.BaseService.GetObject("SelectTZGSSUMnum5", XLtiaojian1);
                }
                obj_sheet.SetValue(row + 0, i, XLsum1);
                //向表写入diqu110（66）架空线长
                string JKXtiaojian1 = " ProjectID='"+ProjID+"' and BuildEd='" + (i + 2006) + "'and Col3='新建'  and Col4='line' and  AreaName='" + diqu + "'" + DQtiaojian + "  and (LineInfo like '110@%' or LineInfo like '66@%')";
                double JKlenth1 = 0;
                if (Services.BaseService.GetObject("SelectTZGSSUMLength", JKXtiaojian1) == null)
                {
                    JKlenth1 = 0;
                }
                else
                {
                    JKlenth1 = (double)Services.BaseService.GetObject("SelectTZGSSUMLength", JKXtiaojian1);
                }
                obj_sheet.SetValue(row + 1, i, JKlenth1);
                //向表写入diqu110（66）电缆线长度
                string DLXtiaojian1 = " ProjectID='"+ProjID+"' and BuildEd='" + (i + 2006) + "'and Col3='新建'  and Col4='line' and  AreaName='" + diqu + "'" + DQtiaojian + "  and (LineInfo like '110@%' or LineInfo like '66@%')";
                double DLXlenth1 = 0;
                if (Services.BaseService.GetObject("SelectTZGSSUMLength2", DLXtiaojian1) == null)
                {
                    DLXlenth1 = 0;
                }
                else
                {
                    DLXlenth1 = (double)Services.BaseService.GetObject("SelectTZGSSUMLength2", DLXtiaojian1);
                }
                obj_sheet.SetValue(row + 2, i, DLXlenth1);
                //向表写入diqu35线路条数
                string XLtiaojian2 = " ProjectID='"+ProjID+"' and BuildEd='" + (i + 2006) + "'and Col3='新建'  and Col4='line' and  AreaName='" + diqu + "'" + DQtiaojian + "  and LineInfo like '35@%'";
                double XLsum2 = 0;
                if (Services.BaseService.GetObject("SelectTZGSSUMnum5", XLtiaojian2) == null)
                {
                    XLsum2 = 0;
                }
                else
                {
                    XLsum2 = (double)Services.BaseService.GetObject("SelectTZGSSUMnum5", XLtiaojian2);
                }              
                obj_sheet.SetValue(row + 3, i, XLsum2);

                //向表写入diqu35架空线长
                string JKXtiaojian2 = " ProjectID='"+ProjID+"' and BuildEd='" + (i + 2006) + "'and Col3='新建'  and Col4='line' and  AreaName='" + diqu  +"'" + DQtiaojian + "  and LineInfo like '35@%' ";
                double JKlenth2 = 0;
                if (Services.BaseService.GetObject("SelectTZGSSUMLength", JKXtiaojian2) == null)
                {
                    JKlenth2 = 0;
                }
                else
                {
                    JKlenth2 = (double)Services.BaseService.GetObject("SelectTZGSSUMLength", JKXtiaojian2);
                }
                obj_sheet.SetValue(row + 4, i, JKlenth2);

                //向表写入diqu35电缆线长
                string DLXtiaojian2 = " ProjectID='"+ProjID+"' and BuildEd='" + (i + 2006) + "'and Col3='新建'  and Col4='line' and  AreaName='" + diqu + "'" + DQtiaojian + "  and LineInfo like '35@%'";
                double DLXlenth2 = 0;
                if (Services.BaseService.GetObject("SelectTZGSSUMLength2", DLXtiaojian2) == null)
                {
                    DLXlenth2 = 0;
                }
                else
                {
                    DLXlenth2 = (double)Services.BaseService.GetObject("SelectTZGSSUMLength2", DLXtiaojian2);
                }
                obj_sheet.SetValue(row + 5, i, DLXlenth2);


            }
        }
        private void build_Sheet75(FarPoint.Win.Spread.SheetView Sheet75 )
        {
            //工作表为11列44行
            Sheet75.Columns.Count = 11;
            Sheet75.Rows.Count = 44;
            //设表格线和居中
            fc.Sheet_GridandCenter(Sheet75);
            //模式为R1C1
            fc.Sheet_Referen_R1C1(Sheet75);
            //设定表格中第1列表列和第9列宽度
            Sheet75.Columns[0].Width = 50;
            Sheet75.Columns[1].Width = 100;
            Sheet75.Columns[2].Width = 85;
            Sheet75.Columns[3].Width = 130;
            Sheet75.Columns[10].Width = 95;
            Sheet75.Rows[0].Height = 35;
            //合并第一行的所有列做为表头
            Sheet75.AddSpanCell(0, 0, 1, 11);
            //向表中写入相关的标题
            Sheet75.SetValue(0, 0, "表7-5  铜陵市高压配电网规划方案改造架空线路工程量");
            Sheet75.SetValue(1, 0, "编号");
            Sheet75.SetValue(1, 1, "类型");
            Sheet75.SetValue(1, 2, "电压等级(KV)");
            Sheet75.SetValue(1, 3, "项  目");
            //循环写入年份
            for (int i = 4; i < 11; i++)
            {
                Sheet75.SetValue(1, i, i + 2006 + "年");
            }
            Sheet75.SetValue(1, 10, "“十二五”合计");
            //写入市辖供电区表格部分
            int jishu = 0;
            Sheet75_39add(Sheet75, 2 + jishu * 6);
            Sheet75.AddSpanCell(2 + jishu * 6, 1, 6, 1);
            Sheet75.SetValue(2 + jishu * 6, 1, "市辖供电区");
            Sheet75.AddSpanCell(2 + jishu * 6, 0, 6, 1);
            Sheet75.SetValue(2 + jishu * 6, 0, "1");
            //县级供电区
            jishu++;
            Sheet75_39add(Sheet75, 2 + jishu * 6);
            Sheet75.AddSpanCell(2 + jishu * 6, 1, 6, 1);
            Sheet75.SetValue(2 + jishu * 6, 1, "县级供电区");
            Sheet75.AddSpanCell(2 + jishu * 6, 0, 6, 1);
            Sheet75.SetValue(2 + jishu * 6, 0, "2");
            //其中：直供直管
            jishu++;
            Sheet75_39add(Sheet75, 2 + jishu * 6);
            Sheet75.AddSpanCell(2 + jishu * 6, 1, 6, 1);
            Sheet75.SetValue(2 + jishu * 6, 1, "其中：直供直管");
            Sheet75.AddSpanCell(2 + jishu * 6, 0, 6, 1);
            Sheet75.SetValue(2 + jishu * 6, 0, "2.1");
            //县级控股
            jishu++;
            Sheet75_39add(Sheet75, 2 + jishu * 6);
            Sheet75.AddSpanCell(2 + jishu * 6, 1, 6, 1);
            Sheet75.SetValue(2 + jishu * 6, 1, "控股");
            Sheet75.AddSpanCell(2 + jishu * 6, 0, 6, 1);
            Sheet75.SetValue(2 + jishu * 6, 0, "2.2");
            //县级参股
            jishu++;
            Sheet75_39add(Sheet75, 2 + jishu * 6);
            Sheet75.AddSpanCell(2 + jishu * 6, 1, 6, 1);
            Sheet75.SetValue(2 + jishu * 6, 1, "参股");
            Sheet75.AddSpanCell(2 + jishu * 6, 0, 6, 1);
            Sheet75.SetValue(2 + jishu * 6, 0, "2.3");
            //县级代管
            jishu++;
            Sheet75_39add(Sheet75, 2 + jishu * 6);
            Sheet75.AddSpanCell(2 + jishu * 6, 1, 6, 1);
            Sheet75.SetValue(2 + jishu * 6, 1, "代管");
            Sheet75.AddSpanCell(2 + jishu * 6, 0, 6, 1);
            Sheet75.SetValue(2 + jishu * 6, 0, "2.4");
            //全地区合计
            jishu++;
            Sheet75_39add(Sheet75, 2 + jishu * 6);
            Sheet75.AddSpanCell(2 + jishu * 6, 1, 6, 1);
            Sheet75.SetValue(2 + jishu * 6, 1, "全地区合计");
            Sheet75.AddSpanCell(2 + jishu * 6, 0, 6, 1);
            Sheet75.SetValue(2 + jishu * 6, 0, "3");
            adddata_sheet75(Sheet75);
            //锁定表格
            fc.Sheet_Locked(Sheet75);
        }
        private void adddata_sheet75(FarPoint.Win.Spread.SheetView obj_sheet)
        {
            //Sheet75即为表7-5
            //向表7-5写入市辖供电区数据
            adddata_sheet75_diqu(obj_sheet,"市辖供电区", 2);
            //向表7-5写入县级直供直管数据
            adddata_sheet75_diqu(obj_sheet,"县级直供直管", 14);
            //向表7-5写入县级控股数据
            adddata_sheet75_diqu(obj_sheet,"县级控股", 20);
            //向表7-5写入县级参股数据
            adddata_sheet75_diqu(obj_sheet,"县级参股", 26);
            //向表7-5写入县级代管数据
            adddata_sheet75_diqu(obj_sheet,"县级代管", 32);
            //计算县极供电区数据公式（求和）
            fc.Sheet_WriteFormula_RowSum(obj_sheet, 14, 4, 4, 6, 8, 4, 6);
            //计算全地区合计数据公式（求和）
            fc.Sheet_WriteFormula_RowSum(obj_sheet, 2, 4, 2, 6, 38, 4, 6);
            //计算十二五合计公式
            fc.Sheet_WriteFormula_ColSum_WritOne(obj_sheet, 2, 5, 42, 5, 10);
           
        }
        private void adddata_sheet75_diqu(FarPoint.Win.Spread.SheetView obj_sheet,string diqu, int row)
        {   //根据地区不同向表7-5写入数据
            //diqu参数为表7-5的类型，可以为市辖供电区，县级直供直管等
            //row参数为要写入数据的地区开始行号
            for (int i = 4; i < 10; i++)
            {   //向表写入diqu110（66）架空线长
                string JKXtiaojian1 = " ProjectID='"+ProjID+"' and BuildEd='" + (i + 2006) + "' and Col3='改造' and Col4='line' and  DQ='" + diqu + "' and (LineInfo like '110@%' or LineInfo like '66@%')";
                double JKlenth1 = 0;
                if (Services.BaseService.GetObject("SelectTZGSSUMLength", JKXtiaojian1) == null)
                {
                    JKlenth1 = 0;
                }
                else
                {
                    JKlenth1 = (double)Services.BaseService.GetObject("SelectTZGSSUMLength", JKXtiaojian1);
                }
                obj_sheet.SetValue(row + 0, i, JKlenth1);
                //向表写入diqu110（66）架空耐热导线长
                double NRDLenth1 = 0;
                if (Services.BaseService.GetObject("SelectTZGSSUMnum1", JKXtiaojian1) == null)
                {
                    NRDLenth1 = 0;
                }
                else
                {
                    NRDLenth1 = (double)Services.BaseService.GetObject("SelectTZGSSUMnum1", JKXtiaojian1);
                }
                obj_sheet.SetValue(row + 1, i, NRDLenth1);
                //向表写入diqu110（66）杆塔数 
                double TGlenth1 = 0;
                if (Services.BaseService.GetObject("SelectTZGSSUMnum2", JKXtiaojian1) == null)
                {
                    TGlenth1 = 0;
                }
                else
                {
                    TGlenth1 = (double)Services.BaseService.GetObject("SelectTZGSSUMnum2", JKXtiaojian1);
                }
                obj_sheet.SetValue(row + 2, i, TGlenth1);

                //向表写入diqu35架空线长
                string JKXtiaojian2 = " ProjectID='"+ProjID+"' and BuildEd='" + (i + 2006) + "' and Col3='改造' and Col4='line' and  DQ='" + diqu + "' and LineInfo like '35@%'";
                double JKlenth2 = 0;
                if (Services.BaseService.GetObject("SelectTZGSSUMLength", JKXtiaojian2) == null)
                {
                    JKlenth2 = 0;
                }
                else
                {
                    JKlenth2 = (double)Services.BaseService.GetObject("SelectTZGSSUMLength", JKXtiaojian2);
                }
                obj_sheet.SetValue(row + 3, i, JKlenth2);
                //向表写入diqu35架空耐热导线长
                double NRDLenth2 = 0;
                if (Services.BaseService.GetObject("SelectTZGSSUMnum1", JKXtiaojian2) == null)
                {
                    NRDLenth2  = 0;
                }
                else
                {
                    NRDLenth2  = (double)Services.BaseService.GetObject("SelectTZGSSUMnum1", JKXtiaojian2);
                }
                obj_sheet.SetValue(row + 4, i, NRDLenth2 );
                //向表写入diqu35杆塔数 
                double TGlenth2 = 0;
                if (Services.BaseService.GetObject("SelectTZGSSUMnum2", JKXtiaojian2) == null)
                {
                    TGlenth2 = 0;
                }
                else
                {
                    TGlenth2 = (double)Services.BaseService.GetObject("SelectTZGSSUMnum2", JKXtiaojian2);
                }
                obj_sheet.SetValue(row + 5, i, TGlenth2);
            }
        }
        private void build_Sheet75_39(FarPoint.Win.Spread.SheetView Sheet75_39)
        {
            //工作表为11列
            Sheet75_39.Columns.Count = 11;
            //查询市辖供电区分区
            string SXtiaojian = " ProjectID='"+ProjID+"' and BuildEd between 2010 and 2015 and AreaName!='' and Col3='改造' and  Col4='line' and (LineInfo like '110@%' or  LineInfo like '66@%' or LineInfo like '35@%') and DQ='市辖供电区'  group by AreaName";
            IList SXlist = Services.BaseService.GetList("SelectTZGSAreaName", SXtiaojian);
            //查询县级供电分区
            string XJtiaojian = " ProjectID='"+ProjID+"' and BuildEd between 2010 and 2015 and AreaName!='' and Col3='改造' and  Col4='line' and (LineInfo like '110@%' or  LineInfo like '66@%' or LineInfo like '35@%') and DQ!='市辖供电区' group by AreaName";
            IList XJlist = Services.BaseService.GetList("SelectTZGSAreaName", XJtiaojian);
            //记录分区数
            int SXSUM = SXlist.Count;
            int XJSUM = XJlist.Count;
            //根据分区数据确定表格的行数，每增加一个分区增加6行
            Sheet75_39.Rows.Count = 2 + 6 * 2 + 6 * (SXSUM + XJSUM);
            //设表格线和居中
            fc.Sheet_GridandCenter(Sheet75_39);
            //模式为R1C1
            fc.Sheet_Referen_R1C1(Sheet75_39);
            //设定表格中第1列表列和第9列宽度
            Sheet75_39.Columns[0].Width = 80;
            Sheet75_39.Columns[1].Width = 100;
            Sheet75_39.Columns[2].Width = 85;
            Sheet75_39.Columns[3].Width = 130;
            Sheet75_39.Columns[10].Width = 95;
            Sheet75_39.Rows[0].Height = 35;
            //合并第一行的所有列做为表头
            Sheet75_39.AddSpanCell(0, 0, 1, 11);
            //向表中写入相关的标题
            Sheet75_39.SetValue(0, 0, "附表39  铜陵市高压配电网规划方案改造架空线路工程量统计");
            Sheet75_39.SetValue(1, 0, "分区类型");
            Sheet75_39.SetValue(1, 1, "分区名称");
            Sheet75_39.SetValue(1, 2, "电压等级(KV)");
            Sheet75_39.SetValue(1, 3, "项  目");
            //循环写入年份
            for (int i = 4; i < 10; i++)
            {
                Sheet75_39.SetValue(1, i, i + 2006 + "年");
            }
            Sheet75_39.SetValue(1, 10, "“十二五”合计");
            //写入市辖供电区合计表格部分
            Sheet75_39add(Sheet75_39, 2);
            Sheet75_39.AddSpanCell(2, 1, 6, 1);
            Sheet75_39.SetValue(2, 1, "合计");
            //循环写入市辖供电区分区和相应的项目
            if (SXSUM != 0)
            {
                for (int j = 1; j <= SXSUM; j++)
                {
                    Sheet75_39add(Sheet75_39, 2 + j * 6);
                    Sheet75_39.AddSpanCell(2 + j * 6, 1, 6, 1);
                    Sheet75_39.SetValue(2 + j * 6, 1, SXlist[j - 1]);
                }
            }
            Sheet75_39.AddSpanCell(2, 0, 6 * (SXSUM + 1), 1);
            Sheet75_39.SetValue(2, 0, "市辖供电区");
            //写入县级供电区合计表格部分
            Sheet75_39add(Sheet75_39, 2 + 6 * (SXSUM + 1));
            Sheet75_39.AddSpanCell(2 + 6 * (SXSUM + 1), 1, 6, 1);
            Sheet75_39.SetValue(2 + 6 * (SXSUM + 1), 1, "合计");
            //循环写入县级供电区分区和相应的项目
            if (XJSUM != 0)
            {
                for (int j = 1; j <= XJSUM; j++)
                {
                    Sheet75_39add(Sheet75_39, 2 + (j + SXSUM + 1) * 6);
                    Sheet75_39.AddSpanCell(2 + (j + SXSUM + 1) * 6, 1, 6, 1);
                    Sheet75_39.SetValue(2 + (j + SXSUM + 1) * 6, 1, XJlist[j - 1]);
                }
            }
            Sheet75_39.AddSpanCell(2 + 6 * (SXSUM + 1), 0, 6 * (XJSUM + 1), 1);
            Sheet75_39.SetValue(2 + 6 * (SXSUM + 1), 0, "县级供电区");
            //从数据库提取数据写入表格
            if (SXSUM != 0)
            {
                for (int sum = 1; sum <= SXSUM; sum++)
                {
                    //向表中写入市辖分区数据
                    Sheet75_39adddata(Sheet75_39, 2 + sum * 6, " and DQ='市辖供电区' ");

                }
                //向表中写入市辖分区合计部分公式
                fc.Sheet_WriteFormula_RowSum(Sheet75_39, 8, 4, SXSUM, 6, 2, 4, 6);
            }
            else
            {
                //如果无市辖地区，合计部分直接写0
                Sheet_WriteZero(Sheet75_39, 2, 4, 6, 6);
            }

            if (XJSUM != 0)
            {
                for (int sum = SXSUM + 2; sum <= SXSUM + XJSUM + 1; sum++)
                {
                    //向表中写入县级分区数据
                    Sheet75_39adddata(Sheet75_39, 2 + sum * 6, " and DQ!='市辖供电区' ");

                }
                //向表中写入县级分区合计部分
                fc.Sheet_WriteFormula_RowSum(Sheet75_39, 2 + (SXSUM + 2) * 6, 4, XJSUM, 6, 2 + (SXSUM + 1) * 6, 4, 6);
            }
            else
            {
                //如果无县级分区，合计部分直接写0
                Sheet_WriteZero(Sheet75_39, 2 + (SXSUM + 1) * 6, 4, 6, 6);
            }  
            //写入十二五合计部分公式
            fc.Sheet_WriteFormula_ColSum_WritOne(Sheet75_39, 2, 5, 6 * (SXSUM + XJSUM + 2), 5, 10);
            //锁定表格
            fc.Sheet_Locked(Sheet75_39);

        }
        private void Sheet75_39add(FarPoint.Win.Spread.SheetView obj_sheet, int i)
        {
            obj_sheet.SetValue(i, 3, "导线长度（km）");
            obj_sheet.SetValue(i + 1, 3, "其中：耐热导线（km）");
            obj_sheet.SetValue(i + 2, 3, "杆塔（基）");
            obj_sheet.SetValue(i + 3, 3, "导线长度（km）");
            obj_sheet.SetValue(i + 4, 3, "其中：耐热导线（km）");
            obj_sheet.SetValue(i + 5, 3, "杆塔（基）");
            obj_sheet.AddSpanCell(i, 2, 3, 1);
            obj_sheet.SetValue(i, 2, "110(66)");
            obj_sheet.AddSpanCell(i + 3, 2, 3, 1);
            obj_sheet.SetValue(i + 3, 2, "35");
        }
        private void Sheet75_39adddata(FarPoint.Win.Spread.SheetView obj_sheet, int row,String DQtiaojian)
        {
            string diqu = obj_sheet.Cells[row, 1].Value.ToString();
            for (int i = 4; i < 10; i++)
            {   //向表写入diqu110（66）架空线长
                string JKXtiaojian1 = " ProjectID='"+ProjID+"' and BuildEd='" + (i + 2006) + "' and Col3='改造' and Col4='line' and  AreaName='" + diqu +"'"+DQtiaojian+ " and (LineInfo like '110@%' or LineInfo like '66@%')";
                double JKlenth1 = 0;
                if (Services.BaseService.GetObject("SelectTZGSSUMLength", JKXtiaojian1) == null)
                {
                    JKlenth1 = 0;
                }
                else
                {
                    JKlenth1 = (double)Services.BaseService.GetObject("SelectTZGSSUMLength", JKXtiaojian1);
                }
                obj_sheet.SetValue(row + 0, i, JKlenth1);
                //向表写入diqu110（66）架空耐热导线长
                double NRDLenth1 = 0;
                if (Services.BaseService.GetObject("SelectTZGSSUMnum1", JKXtiaojian1) == null)
                {
                    NRDLenth1 = 0;
                }
                else
                {
                    NRDLenth1 = (double)Services.BaseService.GetObject("SelectTZGSSUMnum1", JKXtiaojian1);
                }
                obj_sheet.SetValue(row + 1, i, NRDLenth1);
                //向表写入diqu110（66）杆塔数 
                double TGlenth1 = 0;
                if (Services.BaseService.GetObject("SelectTZGSSUMnum2", JKXtiaojian1) == null)
                {
                    TGlenth1 = 0;
                }
                else
                {
                    TGlenth1 = (double)Services.BaseService.GetObject("SelectTZGSSUMnum2", JKXtiaojian1);
                }
                obj_sheet.SetValue(row + 2, i, TGlenth1);


                //向表写入diqu35架空线长
                string JKXtiaojian2 = " ProjectID='"+ProjID+"' and BuildEd='" + (i + 2006) + "' and Col3='改造' and Col4='line' and AreaName='" + diqu + "'" + DQtiaojian + "  and LineInfo like '35@%'";
                double JKlenth2 = 0;
                if (Services.BaseService.GetObject("SelectTZGSSUMLength", JKXtiaojian2) == null)
                {
                    JKlenth2 = 0;
                }
                else
                {
                    JKlenth2 = (double)Services.BaseService.GetObject("SelectTZGSSUMLength", JKXtiaojian2);
                }
                obj_sheet.SetValue(row + 3, i, JKlenth2);
                //向表写入diqu35架空耐热导线长
                double NRDLenth2 = 0;
                if (Services.BaseService.GetObject("SelectTZGSSUMnum1", JKXtiaojian2) == null)
                {
                    NRDLenth2  = 0;
                }
                else
                {
                    NRDLenth2  = (double)Services.BaseService.GetObject("SelectTZGSSUMnum1", JKXtiaojian2);
                }
                obj_sheet.SetValue(row + 4, i, NRDLenth2);
                //向表写入diqu35杆塔数 
                double TGlenth2 = 0;
                if (Services.BaseService.GetObject("SelectTZGSSUMnum2", JKXtiaojian2) == null)
                {
                    TGlenth2 = 0;
                }
                else
                {
                    TGlenth2 = (double)Services.BaseService.GetObject("SelectTZGSSUMnum2", JKXtiaojian2);
                }
                obj_sheet.SetValue(row + 5, i, TGlenth2);
            }

            }
        private void build_Sheet76(FarPoint.Win.Spread.SheetView Sheet76)
        {
            //工作表为11列30行
            Sheet76.Columns.Count = 11;
            Sheet76.Rows.Count = 30;
            //设表格线和居中
            fc.Sheet_GridandCenter(Sheet76);
            //模式为R1C1
            fc.Sheet_Referen_R1C1(Sheet76);
            //设定表格中第1列表列和第9列宽度
            Sheet76.Columns[0].Width = 50;
            Sheet76.Columns[1].Width = 100;
            Sheet76.Columns[2].Width = 85;
            Sheet76.Columns[3].Width = 130;
            Sheet76.Columns[10].Width = 95;
            Sheet76.Rows[0].Height = 35;
            //合并第一行的所有列做为表头
            Sheet76.AddSpanCell(0, 0, 1, 11);
            //向表中写入相关的标题
            Sheet76.SetValue(0, 0, "表7-6 铜陵市高压配电网规划方案改造电缆线路工程量");
            Sheet76.SetValue(1, 0, "编号");
            Sheet76.SetValue(1, 1, "类型");
            Sheet76.SetValue(1, 2, "电压等级(KV)");
            Sheet76.SetValue(1, 3, "项  目");
            //循环写入年份
            for (int i = 4; i < 11; i++)
            {
                Sheet76.SetValue(1, i, i + 2006 + "年");
            }
            Sheet76.SetValue(1, 10, "“十二五”合计");
            //写入市辖供电区表格部分
            int jishu = 0;
            Sheet76_40add(Sheet76, 2 + jishu * 4);
            Sheet76.AddSpanCell(2 + jishu * 4, 1,4, 1);
            Sheet76.SetValue(2 + jishu * 4, 1, "市辖供电区");
            Sheet76.AddSpanCell(2 + jishu * 4, 0, 4, 1);
            Sheet76.SetValue(2 + jishu * 4, 0, "1");
            //县级供电区
            jishu++;
            Sheet76_40add(Sheet76, 2 + jishu * 4);
            Sheet76.AddSpanCell(2 + jishu * 4, 1,4, 1);
            Sheet76.SetValue(2 + jishu * 4, 1, "县级供电区");
            Sheet76.AddSpanCell(2 + jishu * 4, 0, 4, 1);
            Sheet76.SetValue(2 + jishu * 4, 0, "2");
            //其中：直供直管
            jishu++;
            Sheet76_40add(Sheet76, 2 + jishu * 4);
            Sheet76.AddSpanCell(2 + jishu * 4, 1,4, 1);
            Sheet76.SetValue(2 + jishu * 4, 1, "其中：直供直管");
            Sheet76.AddSpanCell(2 + jishu * 4, 0, 4, 1);
            Sheet76.SetValue(2 + jishu * 4, 0, "2.1");
            //县级控股
            jishu++;
            Sheet76_40add(Sheet76, 2 + jishu * 4);
            Sheet76.AddSpanCell(2 + jishu * 4, 1,4, 1);
            Sheet76.SetValue(2 + jishu * 4, 1, "控股");
            Sheet76.AddSpanCell(2 + jishu * 4, 0, 4, 1);
            Sheet76.SetValue(2 + jishu * 4, 0, "2.2");
            //县级参股
            jishu++;
            Sheet76_40add(Sheet76, 2 + jishu * 4);
            Sheet76.AddSpanCell(2 + jishu * 4, 1,4, 1);
            Sheet76.SetValue(2 + jishu * 4, 1, "参股");
            Sheet76.AddSpanCell(2 + jishu * 4, 0, 4, 1);
            Sheet76.SetValue(2 + jishu * 4, 0, "2.3");
            //县级代管
            jishu++;
            Sheet76_40add(Sheet76, 2 + jishu * 4);
            Sheet76.AddSpanCell(2 + jishu * 4, 1,4, 1);
            Sheet76.SetValue(2 + jishu * 4, 1, "代管");
            Sheet76.AddSpanCell(2 + jishu * 4, 0, 4, 1);
            Sheet76.SetValue(2 + jishu * 4, 0, "2.4");
            //全地区合计
            jishu++;
            Sheet76_40add(Sheet76, 2 + jishu * 4);
            Sheet76.AddSpanCell(2 + jishu * 4, 1,4, 1);
            Sheet76.SetValue(2 + jishu * 4, 1, "全地区合计");
            Sheet76.AddSpanCell(2 + jishu * 4, 0, 4, 1);
            Sheet76.SetValue(2 + jishu * 4, 0, "3");
            adddata_sheet76(Sheet76);
            //锁定表格
            fc.Sheet_Locked(Sheet76);

        }
        private void adddata_sheet76(FarPoint.Win.Spread.SheetView obj_sheet)
        {
            //Sheet76即为表7-6
            //向表7-6写入市辖供电区数据
            adddata_sheet76_diqu(obj_sheet,"市辖供电区", 2);
            //向表7-6写入县级直供直管数据
            adddata_sheet76_diqu(obj_sheet,"县级直供直管", 10);
            //向表7-6写入县级控股数据
            adddata_sheet76_diqu(obj_sheet,"县级控股", 14);
            //向表7-6写入县级参股数据
            adddata_sheet76_diqu(obj_sheet,"县级参股", 18);
            //向表7-6写入县级代管数据
            adddata_sheet76_diqu(obj_sheet,"县级代管", 22);
            //计算县极供电区数据公式（求和）
            fc.Sheet_WriteFormula_RowSum(obj_sheet, 10, 4, 4, 4, 6, 4, 6);
            //计算全地区合计数据公式（求和）
            fc.Sheet_WriteFormula_RowSum(obj_sheet, 2, 4, 2, 4, 26, 4, 6);
            //计算十二五合计公式
            fc.Sheet_WriteFormula_ColSum_WritOne(obj_sheet, 2, 5, 28, 5, 10);
          
        }
        private void adddata_sheet76_diqu(FarPoint.Win.Spread.SheetView obj_sheet,string diqu, int row)
        {   //根据地区不同向表7-6写入数据
            //diqu参数为表7-6的类型，可以为市辖供电区，县级直供直管等
            //row参数为要写入数据的地区开始行号
            for (int i = 4; i < 10; i++)
            {   //向表写入diqu110（66）电缆线长
                string DLXtiaojian1 = " ProjectID='"+ProjID+"' and BuildEd='" + (i + 2006) + "' and Col3='改造' and Col4='line' and  DQ='" + diqu + "' and (LineInfo like '110@%' or LineInfo like '66@%')";
                double DLLlenth1 = 0;
                if (Services.BaseService.GetObject("SelectTZGSSUMLength2", DLXtiaojian1) == null)
                {
                    DLLlenth1 = 0;
                }
                else
                {
                    DLLlenth1 = (double)Services.BaseService.GetObject("SelectTZGSSUMLength2", DLXtiaojian1);
                }
                obj_sheet.SetValue(row + 0, i, DLLlenth1);
                //向表写入diqu110（66）沟道长
                double GDLenth1 = 0;
                if (Services.BaseService.GetObject("SelectTZGSSUMnum6", DLXtiaojian1) == null)
                {
                    GDLenth1 = 0;
                }
                else
                {
                    GDLenth1 = (double)Services.BaseService.GetObject("SelectTZGSSUMnum6", DLXtiaojian1);
                }
                obj_sheet.SetValue(row + 1, i, GDLenth1);

                //向表写入diqu35电缆线长
                string DLXtiaojian2 = " ProjectID='"+ProjID+"' and BuildEd='" + (i + 2006) + "' and Col3='改造' and Col4='line' and  DQ='" + diqu + "' and LineInfo like '35@%'";
                double JKlenth2 = 0;
                if (Services.BaseService.GetObject("SelectTZGSSUMLength2", DLXtiaojian2) == null)
                {
                    JKlenth2 = 0;
                }
                else
                {
                    JKlenth2 = (double)Services.BaseService.GetObject("SelectTZGSSUMLength2", DLXtiaojian2);
                }
                obj_sheet.SetValue(row + 2, i, JKlenth2);
                //向表写入diqu35沟道长
                double GDXLenth2 = 0;
                if (Services.BaseService.GetObject("SelectTZGSSUMnum6", DLXtiaojian2) == null)
                {
                    GDXLenth2 = 0;
                }
                else
                {
                    GDXLenth2 = (double)Services.BaseService.GetObject("SelectTZGSSUMnum6", DLXtiaojian2);
                }
                obj_sheet.SetValue(row + 3, i, GDXLenth2);
               
            }
        }
        private void build_Sheet76_40(FarPoint.Win.Spread.SheetView Sheet76_40)
        {
            //工作表为11列
            Sheet76_40.Columns.Count = 11;
            //查询市辖供电区分区
            string SXtiaojian = " ProjectID='"+ProjID+"' and BuildEd between 2010 and 2015 and AreaName!='' and Col3='改造' and  Col4='line' and (LineInfo like '110@%' or  LineInfo like '66@%' or LineInfo like '35@%') and DQ='市辖供电区'  group by AreaName";
            IList SXlist = Services.BaseService.GetList("SelectTZGSAreaName", SXtiaojian);
            //查询县级供电分区
            string XJtiaojian = " ProjectID='"+ProjID+"' and BuildEd between 2010 and 2015 and AreaName!='' and Col3='改造' and  Col4='line' and (LineInfo like '110@%' or  LineInfo like '66@%' or LineInfo like '35@%') and DQ!='市辖供电区' group by AreaName";
            IList XJlist = Services.BaseService.GetList("SelectTZGSAreaName", XJtiaojian);
            ////记录分区数
            int SXSUM = SXlist.Count;
            int XJSUM = XJlist.Count;
            //根据分区数据确定表格的行数，每增加一个分区增加4行
            Sheet76_40.Rows.Count = 2 + 4 * 2 + 4 * (SXSUM + XJSUM);
            //设表格线和居中
            fc.Sheet_GridandCenter(Sheet76_40);
            //模式为R1C1
            fc.Sheet_Referen_R1C1(Sheet76_40);
            //设定表格中第1列表列和第9列宽度
            Sheet76_40.Columns[0].Width = 80;
            Sheet76_40.Columns[1].Width = 100;
            Sheet76_40.Columns[2].Width = 85;
            Sheet76_40.Columns[3].Width = 130;
            Sheet76_40.Columns[10].Width = 95;
            Sheet76_40.Rows[0].Height = 35;
            //合并第一行的所有列做为表头
            Sheet76_40.AddSpanCell(0, 0, 1, 11);
            //向表中写入相关的标题
            Sheet76_40.SetValue(0, 0, "附表40  铜陵市高压配电网规划方案改造电缆线路工程量统计");
            Sheet76_40.SetValue(1, 0, "分区类型");
            Sheet76_40.SetValue(1, 1, "分区名称");
            Sheet76_40.SetValue(1, 2, "电压等级(KV)");
            Sheet76_40.SetValue(1, 3, "项  目");
            //循环写入年份
            for (int i = 4; i < 10; i++)
            {
                Sheet76_40.SetValue(1, i, i + 2006 + "年");
            }
            Sheet76_40.SetValue(1, 10, "“十二五”合计");
            //写入市辖供电区合计表格部分
            Sheet76_40add(Sheet76_40, 2);
            Sheet76_40.AddSpanCell(2, 1, 4, 1);
            Sheet76_40.SetValue(2, 1, "合计");
            //循环写入市辖供电区分区和相应的项目
            if (SXSUM != 0)
            {
                for (int j = 1; j <= SXSUM; j++)
                {
                    Sheet76_40add(Sheet76_40, 2 + j * 4);
                    Sheet76_40.AddSpanCell(2 + j * 4, 1,4, 1);
                    Sheet76_40.SetValue(2 + j * 4, 1, SXlist[j - 1]);
                }
            }
            Sheet76_40.AddSpanCell(2, 0, 4 * (SXSUM + 1), 1);
            Sheet76_40.SetValue(2, 0, "市辖供电区");
            //写入县级供电区合计表格部分
            Sheet76_40add(Sheet76_40, 2 + 4 * (SXSUM + 1));
            Sheet76_40.AddSpanCell(2 + 4 * (SXSUM + 1), 1, 4, 1);
            Sheet76_40.SetValue(2 + 4 * (SXSUM + 1), 1, "合计");
            //循环写入县级供电区分区和相应的项目
            if (XJSUM != 0)
            {
                for (int j = 1; j <= XJSUM; j++)
                {
                    Sheet76_40add(Sheet76_40, 2 + (j + SXSUM + 1) * 4);
                    Sheet76_40.AddSpanCell(2 + (j + SXSUM + 1) * 4, 1, 4, 1);
                    Sheet76_40.SetValue(2 + (j + SXSUM + 1) * 4, 1, XJlist[j - 1]);
                }
            }
            Sheet76_40.AddSpanCell(2 + 4 * (SXSUM + 1), 0, 4 * (XJSUM + 1), 1);
            Sheet76_40.SetValue(2 + 4 * (SXSUM + 1), 0, "县级供电区");
            //从数据库提取数据写入表格
            if (SXSUM != 0)
            {
                for (int sum = 1; sum <= SXSUM; sum++)
                {
                    //向表中写入市辖分区数据
                     Sheet76_40adddata( Sheet76_40, 2 + sum * 4, " and DQ='市辖供电区' ");

                }
                //向表中写入市辖分区合计部分公式
                fc.Sheet_WriteFormula_RowSum(Sheet76_40, 6, 4, SXSUM, 4, 2, 4, 6);
            }
            else
            {
                ////如果无市辖地区，合计部分直接写0
                Sheet_WriteZero(Sheet76_40, 2, 4, 4, 6);
            }

            if (XJSUM != 0)
            {
                for (int sum = SXSUM + 2; sum <= SXSUM + XJSUM + 1; sum++)
                {
                    //向表中写入县级分区数据
                     Sheet76_40adddata( Sheet76_40, 2 + sum * 4, " and DQ!='市辖供电区' ");

                }
                //向表中写入县级分区合计部分公式
                fc.Sheet_WriteFormula_RowSum(Sheet76_40, 2 + (SXSUM + 2) * 4, 4, XJSUM, 4, 2 + (SXSUM + 1) * 4, 4, 6);
            }
            else
            {
                //如果无县级分区，合计部分直接写0
                Sheet_WriteZero( Sheet76_40, 2 + (SXSUM + 1) * 4, 4, 4, 6);
            }  
            //写入十二五合计公式
            fc.Sheet_WriteFormula_ColSum_WritOne(Sheet76_40, 2, 5, 4 * (SXSUM + XJSUM + 2), 5, 10);
            //锁定表格
            fc.Sheet_Locked(Sheet76_40);

        }
        private void Sheet76_40add(FarPoint.Win.Spread.SheetView obj_sheet, int i)
        {
            obj_sheet.SetValue(i, 3, "电缆长度（km）");
            obj_sheet.SetValue(i + 1, 3, "沟道（km）");
            obj_sheet.SetValue(i + 2, 3, "电缆长度（km）");
            obj_sheet.SetValue(i + 3, 3, "沟道（km）");
            obj_sheet.AddSpanCell(i, 2, 2, 1);
            obj_sheet.SetValue(i, 2, "110(66)");
            obj_sheet.AddSpanCell(i + 2, 2, 2, 1);
            obj_sheet.SetValue(i + 2, 2, "35");
        }
        private void Sheet76_40adddata(FarPoint.Win.Spread.SheetView obj_sheet, int row,String DQdiaojian)
        {
            string diqu = obj_sheet.Cells[row, 1].Value.ToString();
           
            for (int i = 4; i < 10; i++)
            {   //向表写入diqu110（66）电缆线长
                string DLXtiaojian1 = " ProjectID='"+ProjID+"' and BuildEd='" + (i + 2006) + "' and Col3='改造' and Col4='line' and  AreaName='" + diqu +"'"+DQdiaojian+ " and (LineInfo like '110@%' or LineInfo like '66@%')";
                double DLLlenth1 = 0;
                if (Services.BaseService.GetObject("SelectTZGSSUMLength2", DLXtiaojian1) == null)
                {
                    DLLlenth1 = 0;
                }
                else
                {
                    DLLlenth1 = (double)Services.BaseService.GetObject("SelectTZGSSUMLength2", DLXtiaojian1);
                }
                obj_sheet.SetValue(row + 0, i, DLLlenth1);
                //向表写入diqu110（66）沟道长
                double GDLenth1 = 0;
                if (Services.BaseService.GetObject("SelectTZGSSUMnum6", DLXtiaojian1) == null)
                {
                    GDLenth1 = 0;
                }
                else
                {
                    GDLenth1 = (double)Services.BaseService.GetObject("SelectTZGSSUMnum6", DLXtiaojian1);
                }
                obj_sheet.SetValue(row + 1, i, GDLenth1);

                //向表写入diqu35电缆线长
                string DLXtiaojian2 = " ProjectID='"+ProjID+"' and BuildEd='" + (i + 2006) + "' and Col3='改造' and Col4='line' and  AreaName='" + diqu + "'" + DQdiaojian + " and LineInfo like '35@%'";
                double JKlenth2 = 0;
                if (Services.BaseService.GetObject("SelectTZGSSUMLength2", DLXtiaojian2) == null)
                {
                    JKlenth2 = 0;
                }
                else
                {
                    JKlenth2 = (double)Services.BaseService.GetObject("SelectTZGSSUMLength2", DLXtiaojian2);
                }
                obj_sheet.SetValue(row + 2, i, JKlenth2);
                //向表写入diqu35沟道长
                double GDXLenth2 = 0;
                if (Services.BaseService.GetObject("SelectTZGSSUMnum6", DLXtiaojian2) == null)
                {
                    GDXLenth2 = 0;
                }
                else
                {
                    GDXLenth2 = (double)Services.BaseService.GetObject("SelectTZGSSUMnum6", DLXtiaojian2);
                }
                obj_sheet.SetValue(row + 3, i, GDXLenth2);

            }

        }
        private void build_Sheet77(FarPoint.Win.Spread.SheetView Sheet77)
        {
            //工作表为11列54行
            Sheet77.Columns.Count = 11;
            Sheet77.Rows.Count = 54;
            //设表格线和居中
            fc.Sheet_GridandCenter(Sheet77);
            //模式为R1C1
            fc.Sheet_Referen_R1C1(Sheet77);
            //设定表格中第1列表列和第9列宽度
            Sheet77.Columns[0].Width = 95;
            Sheet77.Columns[1].Width = 88;
            Sheet77.Columns[2].Width = 45;
            Sheet77.Columns[3].Width = 110;
            Sheet77.Columns[10].Width = 95;
            Sheet77.Rows[0].Height = 35;
            //合并第一行的所有列做为表头
            Sheet77.AddSpanCell(0, 0, 1, 11);
            //向表中写入相关的标题
            Sheet77.SetValue(0, 0, "表7-7 铜陵市市辖供电区高压配电网新扩建工程分类规模");
            Sheet77.SetValue(1, 0, "工程类别");
            Sheet77.SetValue(1, 1, "电压等级(kV)");
            Sheet77.AddSpanCell(1, 2, 1, 2);
            Sheet77.SetValue(1, 2, "项  目");
            //循环写入年份
            for (int i = 4; i < 10; i++)
            {
                Sheet77.SetValue(1, i, i + 2006 + "年");
            }
            Sheet77.SetValue(1, 10, "“十二五”合计");
            //写入提高供电能力部分
            Sheet77or78add(Sheet77, 2, "变线");
            Sheet77.AddSpanCell(2, 0, 14, 1);
            Sheet77.SetValue(2, 0, "提高供电能力");

            //网架结构优化
            Sheet77or78add(Sheet77, 16, "线线");
            Sheet77.AddSpanCell(16, 0, 8, 1);
            Sheet77.SetValue(16, 0, "网架结构优化");

            //电厂接入
            Sheet77or78add(Sheet77, 24, "线线");
            Sheet77.AddSpanCell(24, 0, 8, 1);
            Sheet77.SetValue(24, 0, "电厂接入");

            //电铁供电
            Sheet77or78add(Sheet77, 32, "线线");
            Sheet77.AddSpanCell(32, 0, 8, 1);
            Sheet77.SetValue(32, 0, "电铁供电");

            //其它类别
            Sheet77or78add(Sheet77, 40, "变线");
            Sheet77.AddSpanCell(40, 0, 14, 1);
            Sheet77.SetValue(40, 0, "其他类别");

            //写入数据
            Sheet77or78adddata(Sheet77,2,54);
            //写入合计公式
            fc.Sheet_WriteFormula_ColSum_WritOne(Sheet77, 2, 5, 52, 5, 10);
            //锁定表格
            fc.Sheet_Locked(Sheet77);
        }
        private void Sheet77or78adddata(FarPoint.Win.Spread.SheetView obj_sheet,int startrow, int rowcount)
        {
            //根据表7-7,7-8的特点，把项目部分做为一个突破口，对应“变电”“电源侧间隔数(个)”和“线路”三个基本部分
            //用行做外循环控制，列（年份）做内循环控制，取工程类别和电压等级做判断（合并单元格有空能值为空，调用Sheet_find_notnull方法），
            for (int row = startrow; row < rowcount;row++ )
            {
                string tiaojian = "";
                //记录工程序类别
                string type = Sheet_find_notnull(obj_sheet, row, 0);
                //记录电压等级
                string dianya = Sheet_find_notnull(obj_sheet, row, 1);
                for (int i = 4; i < 10; i++)
                {
                    if (obj_sheet.Cells[row, 2].Value.ToString() == "变电")
                    {
                        //判断电压
                        if (dianya == "110（66）")
                        {
                            tiaojian = " ProjectID='"+ProjID+"' and BuildEd='" + (i + 2006) + "' and (Col3='新建' or Col3='扩建') and Col4='bian' and  ProgType='" + type + "' and (BianInfo like '110@%' or BianInfo  like '66@%')";
                           
                        }
                        if (dianya == "35")
                        {
                            tiaojian = " ProjectID='"+ProjID+"' and BuildEd='" + (i + 2006) + "' and (Col3='新建' or Col3='扩建') and Col4='bian' and  ProgType='" + type + "' and BianInfo  like '35@%'";
                        }
                        //向表写入变电站台数 
                        double BDZsum1 = 0;
                        if (Services.BaseService.GetObject("SelectTZGSSUMnum5", tiaojian) == null)
                        {
                            BDZsum1 = 0;
                        }
                        else
                        {
                            BDZsum1 = (double)Services.BaseService.GetObject("SelectTZGSSUMnum5", tiaojian);
                        }
                        obj_sheet.SetValue(row + 0, i, BDZsum1);
                        //向表写入主变台数
                        double ZBsum1 = 0;
                        if (Services.BaseService.GetObject("SelectTZGSSUMnum1", tiaojian) == null)
                        {
                            ZBsum1 = 0;
                        }
                        else
                        {
                            ZBsum1 = (double)Services.BaseService.GetObject("SelectTZGSSUMnum1", tiaojian);
                        }
                        obj_sheet.SetValue(row + 1, i, ZBsum1);
                        //向表写入变电容量
                        double BDRLsum1 = 0;
                        if (Services.BaseService.GetObject("SelectTZGSSUMVolumn", tiaojian) == null)
                        {
                            BDRLsum1 = 0;
                        }
                        else
                        {
                            BDRLsum1 = (double)Services.BaseService.GetObject("SelectTZGSSUMVolumn", tiaojian);
                        }
                        obj_sheet.SetValue(row + 2, i, BDRLsum1);
                        
                    }
                    if (obj_sheet.Cells[row, 2].Value.ToString() == "电源侧间隔数(个)")
                    {
                        if (dianya == "110（66）")
                        {
                            tiaojian = " ProjectID='"+ProjID+"' and BuildEd='" + (i + 2006) + "' and (Col3='新建' or Col3='扩建') and Col4='' and  ProgType='" + type + "' and (BianInfo like '110@%' or BianInfo  like '66@%')";

                        }
                        if (dianya == "35")
                        {
                            tiaojian = " ProjectID='"+ProjID+"' and BuildEd='" + (i + 2006) + "' and (Col3='新建' or Col3='扩建') and Col4='' and  ProgType='" + type + "' and BianInfo  like '35@%'";
                        }
                        //向表写入电源侧间隔数(个)
                        int JGsum1 = 0;
                        if (Services.BaseService.GetObject("SelectTZGSSUMJGNum", tiaojian) == null)
                        {
                            JGsum1 = 0;
                        }
                        else
                        {
                            JGsum1 = (int)Services.BaseService.GetObject("SelectTZGSSUMJGNum", tiaojian);
                        }
                        obj_sheet.SetValue(row + 0, i, JGsum1);
                        

                    }
                    if (obj_sheet.Cells[row, 2].Value.ToString() == "线路")
                    {
                        if (dianya == "110（66）")
                        {
                            tiaojian = " ProjectID='"+ProjID+"' and BuildEd='" + (i + 2006) + "' and (Col3='新建' or Col3='扩建') and Col4='line' and  ProgType='" + type + "' and (LineInfo like '110@%' or LineInfo  like '66@%')";
                           
                        }
                        if (dianya == "35")
                        {
                            tiaojian = " ProjectID='"+ProjID+"' and BuildEd='" + (i + 2006) + "' and (Col3='新建' or Col3='扩建') and Col4='line' and  ProgType='" + type + "' and LineInfo  like '35@%'";
                        }
                        //向表写入线路条数
                         double XLsum1 = 0;
                         if (Services.BaseService.GetObject("SelectTZGSSUMnum5", tiaojian) == null)
                        {
                            XLsum1 = 0;
                        }
                        else
                        {
                            XLsum1 = (double)Services.BaseService.GetObject("SelectTZGSSUMnum5", tiaojian);
                        }
                        obj_sheet.SetValue(row + 0, i, XLsum1);
                        //向表写入架空线长
                        double JKlenth1 = 0;
                        if (Services.BaseService.GetObject("SelectTZGSSUMLength", tiaojian) == null)
                        {
                            JKlenth1 = 0;
                        }
                        else
                        {
                            JKlenth1 = (double)Services.BaseService.GetObject("SelectTZGSSUMLength", tiaojian);
                        }
                        obj_sheet.SetValue(row + 1, i, JKlenth1);
                        //向表写入电缆线长度
                        double DLXlenth1 = 0;
                        if (Services.BaseService.GetObject("SelectTZGSSUMLength2", tiaojian) == null)
                        {
                            DLXlenth1 = 0;
                        }
                        else
                        {
                            DLXlenth1 = (double)Services.BaseService.GetObject("SelectTZGSSUMLength2", tiaojian);
                        }
                        obj_sheet.SetValue(row + 2, i, DLXlenth1);
                        
                        
                    }
                    
                }
                //如果不是“电源侧间隔数(个)”，必然是“变电”或者“线路”，
                //每个对应三行内容，所以行号加2，循环本身再加1，指向下一个内容
                if (obj_sheet.Cells[row, 2].Value.ToString() != "电源侧间隔数(个)")
                {
                    row = row + 2;
                }
            }
            
        }
        private void build_Sheet78(FarPoint.Win.Spread.SheetView Sheet78)
        {
            //工作表为11列82行
            Sheet78.Columns.Count = 11;
            Sheet78.Rows.Count = 82;
            //设表格线和居中
            fc.Sheet_GridandCenter(Sheet78);
            //模式为R1C1
            fc.Sheet_Referen_R1C1(Sheet78);
            //设定表格中第1列表列和第9列宽度
            Sheet78.Columns[0].Width = 95;
            Sheet78.Columns[1].Width = 88;
            Sheet78.Columns[2].Width = 45;
            Sheet78.Columns[3].Width = 110;
            Sheet78.Columns[10].Width = 95;
            Sheet78.Rows[0].Height = 35;
            //合并第一行的所有列做为表头
            Sheet78.AddSpanCell(0, 0, 1, 11);
            //向表中写入相关的标题
            Sheet78.SetValue(0, 0, "表7-8 铜陵市县级供电区高压配电网新扩建工程分类规模");
            Sheet78.SetValue(1, 0, "工程类别");
            Sheet78.SetValue(1, 1, "电压等级(kV)");
            Sheet78.AddSpanCell(1, 2, 1, 2);
            Sheet78.SetValue(1, 2, "项  目");
            //循环写入年份
            for (int i = 4; i < 10; i++)
            {
                Sheet78.SetValue(1, i, i + 2006 + "年");
            }
            Sheet78.SetValue(1, 10, "“十二五”合计");
            //写入提高供电能力部分
            Sheet77or78add(Sheet78, 2, "变线");
            Sheet78.AddSpanCell(2, 0, 14, 1);
            Sheet78.SetValue(2, 0, "提高供电能力");

            //网架结构优化
            Sheet77or78add(Sheet78, 16, "线线");
            Sheet78.AddSpanCell(16, 0, 8, 1);
            Sheet78.SetValue(16, 0, "网架结构优化");

            //电厂接入
            Sheet77or78add(Sheet78, 24, "线线");
            Sheet78.AddSpanCell(24, 0, 8, 1);
            Sheet78.SetValue(24, 0, "电厂接入");

            //电铁供电
            Sheet77or78add(Sheet78, 32, "线线");
            Sheet78.AddSpanCell(32, 0, 8, 1);
            Sheet78.SetValue(32, 0, "电铁供电");

            //新农村电气化
            Sheet77or78add(Sheet78, 40, "变线");
            Sheet78.AddSpanCell(40, 0, 14, 1);
            Sheet78.SetValue(40, 0, "新农村电气化");


            //无电地区电力建设
            Sheet77or78add(Sheet78, 54, "变线");
            Sheet78.AddSpanCell(54, 0, 14, 1);
            Sheet78.SetValue(54, 0, "无电地区电力建设");

            //其它类别
            Sheet77or78add(Sheet78, 68, "变线");
            Sheet78.AddSpanCell(68, 0, 14, 1);
            Sheet78.SetValue(68, 0, "其他类别");
            
            //写入数据
            Sheet77or78adddata(Sheet78, 2, 82);
            //写入合计公式
            fc.Sheet_WriteFormula_ColSum_WritOne(Sheet78, 2, 5, 80, 5, 10);
            fc.Sheet_Locked(Sheet78);
        }
        private void Sheet77or78add(FarPoint.Win.Spread.SheetView obj_sheet, int row, string liebie)
        {
            //类别有两种，一种是“变线”，另和种是"线线"
            //分别对应“提高供电能力”和“优化网架结构”的电压和项目部分的写入
            if (liebie == "变线")
            {
                //110（66）部分
                obj_sheet.SetValue(row, 3, "变电站座数(座)");
                obj_sheet.SetValue(row + 1, 3, "主变台数(台)");
                obj_sheet.SetValue(row + 2, 3, "变电容量(MVA)");
                obj_sheet.AddSpanCell(row, 2, 3, 1);
                obj_sheet.SetValue(row, 2, "变电");

                obj_sheet.AddSpanCell(row + 3, 2, 1, 2);
                obj_sheet.SetValue(row + 3, 2, "电源侧间隔数(个)");

                obj_sheet.SetValue(row + 4, 3, "线路条数(条)");
                obj_sheet.SetValue(row + 5, 3, "架空长度(km)");
                obj_sheet.SetValue(row + 6, 3, "电缆长度(km)");
                obj_sheet.AddSpanCell(row + 4, 2, 3, 1);
                obj_sheet.SetValue(row + 4, 2, "线路");

                obj_sheet.AddSpanCell(row, 1, 7, 1);
                obj_sheet.SetValue(row, 1, "110（66）");
                //35部分
                int newrow = row + 7;
                obj_sheet.SetValue(newrow, 3, "变电站座数(座)");
                obj_sheet.SetValue(newrow + 1, 3, "主变台数(台)");
                obj_sheet.SetValue(newrow + 2, 3, "变电容量(MVA)");
                obj_sheet.AddSpanCell(newrow, 2, 3, 1);
                obj_sheet.SetValue(newrow, 2, "变电");

                obj_sheet.AddSpanCell(newrow + 3, 2, 1, 2);
                obj_sheet.SetValue(newrow + 3, 2, "电源侧间隔数(个)");

                obj_sheet.SetValue(newrow + 4, 3, "线路条数(条)");
                obj_sheet.SetValue(newrow + 5, 3, "架空长度(km)");
                obj_sheet.SetValue(newrow + 6, 3, "电缆长度(km)");
                obj_sheet.AddSpanCell(newrow + 4, 2, 3, 1);
                obj_sheet.SetValue(newrow + 4, 2, "线路");

                obj_sheet.AddSpanCell(newrow, 1, 7, 1);
                obj_sheet.SetValue(newrow, 1, "35");
            }
            if (liebie == "线线")
            {
                 //110（66）部分
                obj_sheet.AddSpanCell(row, 2, 1, 2);
                obj_sheet.SetValue(row, 2, "电源侧间隔数(个)");

                obj_sheet.SetValue(row + 1, 3, "线路条数(条)");
                obj_sheet.SetValue(row + 2, 3, "架空长度(km)");
                obj_sheet.SetValue(row + 3, 3, "电缆长度(km)");
                obj_sheet.AddSpanCell(row + 1, 2, 3, 1);
                obj_sheet.SetValue(row + 1, 2, "线路");

                obj_sheet.AddSpanCell(row, 1, 4, 1);
                obj_sheet.SetValue(row, 1, "110（66）");

                //35部分
                int newrow=row+4;
                obj_sheet.AddSpanCell(newrow, 2, 1, 2);
                obj_sheet.SetValue(newrow, 2, "电源侧间隔数(个)");

                obj_sheet.SetValue(newrow + 1, 3, "线路条数(条)");
                obj_sheet.SetValue(newrow + 2, 3, "架空长度(km)");
                obj_sheet.SetValue(newrow + 3, 3, "电缆长度(km)");
                obj_sheet.AddSpanCell(newrow + 1, 2, 3, 1);
                obj_sheet.SetValue(newrow + 1, 2, "线路");

                obj_sheet.AddSpanCell(newrow, 1, 4, 1);
                obj_sheet.SetValue(newrow, 1, "35");

            }
          

        }
        private void build_Sheet79(FarPoint.Win.Spread.SheetView Sheet79)
        {
            //工作表为11列30行
            Sheet79.Columns.Count = 11;
            Sheet79.Rows.Count = 30;
            //设表格线和居中
            fc.Sheet_GridandCenter(Sheet79);
            //模式为R1C1
            fc.Sheet_Referen_R1C1(Sheet79);
            //设定表格中第1列表列和第9列宽度
            Sheet79.Columns[0].Width = 95;
            Sheet79.Columns[1].Width = 88;
            Sheet79.Columns[2].Width = 60;
            Sheet79.Columns[3].Width = 135;
            Sheet79.Columns[10].Width = 95;
            Sheet79.Rows[0].Height = 35;
            //合并第一行的所有列做为表头
            Sheet79.AddSpanCell(0, 0, 1, 11);
            //向表中写入相关的标题
            Sheet79.SetValue(0, 0, "表7-9 铜陵市高压配电网改造工程规模");
            Sheet79.SetValue(1, 0, "区域");
            Sheet79.SetValue(1, 1, "电压等级(kV)");
            Sheet79.AddSpanCell(1, 2, 1, 2);
            Sheet79.SetValue(1, 2, "项  目");
            //循环写入年份
            for (int i = 4; i < 10; i++)
            {
                Sheet79.SetValue(1, i, i + 2006 + "年");
            }
            Sheet79.SetValue(1, 10, "“十二五”合计");
            //写入市辖供电区部分
            Sheet79add(Sheet79, 2, "110（66）");
            Sheet79add(Sheet79, 9, "35");
            Sheet79.AddSpanCell(2, 0, 14, 1);
            Sheet79.SetValue(2, 0, "市辖供电区");

            //写入县级供电区部分
            Sheet79add(Sheet79, 16, "110（66）");
            Sheet79add(Sheet79, 23, "35");
            Sheet79.AddSpanCell(16, 0, 14, 1);
            Sheet79.SetValue(16, 0, "县级供电区");
            //写入数据
            Sheet79adddata(Sheet79, 2, 30);
            //写入合计公式
            fc.Sheet_WriteFormula_ColSum_WritOne(Sheet79, 2, 5, 28, 5, 10);
            fc.Sheet_Locked(Sheet79);
        }
        private void Sheet79add(FarPoint.Win.Spread.SheetView obj_sheet, int row, string dianya)
        {

            obj_sheet.SetValue(row, 3, "变压器台数（台）");
            obj_sheet.SetValue(row + 1, 3, "断路器台数（台）");
            obj_sheet.AddSpanCell(row, 2, 2, 1);
            obj_sheet.SetValue(row, 2, "变电");

            obj_sheet.SetValue(row + 2, 3, "长度（km）");
            obj_sheet.SetValue(row + 3, 3, "其中：耐热导线（km）");
            obj_sheet.SetValue(row + 4, 3, "杆塔（基）");
            obj_sheet.AddSpanCell(row + 2, 2, 3, 1);
            obj_sheet.SetValue(row + 2, 2, "架空线路");

            obj_sheet.SetValue(row + 5, 3, "长度（km）");
            obj_sheet.SetValue(row + 6, 3, "沟道（km）");
            obj_sheet.AddSpanCell(row + 5, 2, 2, 1);
            obj_sheet.SetValue(row + 5, 2, "电缆线路");

            obj_sheet.AddSpanCell(row, 1, 7, 1);
            obj_sheet.SetValue(row, 1, dianya);
        }
        private void Sheet79adddata(FarPoint.Win.Spread.SheetView obj_sheet, int startrow, int rowcount)
        {
            //根据表79的特点，把项目部分做为一个突破口，对应“变电”“架空线路”和“电缆线路”三个基本部分
            //用行做外循环控制，列（年份）做内循环控制，取区域和电压等级做判断（合并单元格有中能值为空，调用Sheet_find_notnull方法），
            for (int row = startrow; row < rowcount; row++)
            {
                string tiaojian = "";
                string diqutiaojian = "";
                string dydztiaojian = "";
                string dylinetiaojian="";
                //记录地区
                string diqu = Sheet_find_notnull(obj_sheet, row, 0);
                if (diqu == "市辖供电区")
                {
                    diqutiaojian = " and DQ='市辖供电区' ";
                }
                if (diqu == "县级供电区")
                {
                    diqutiaojian = " and DQ!='市辖供电区' ";
                }
                //记录电压等级
                string dianya = Sheet_find_notnull(obj_sheet, row, 1);
                //判断电压
                if (dianya == "110（66）")
                {
                    dydztiaojian=" and (BianInfo like '110@%' or BianInfo  like '66@%')";
                    dylinetiaojian=" and (LineInfo like '110@%' or LineInfo  like '66@%')";

                }
                if (dianya == "35")
                {
                    dydztiaojian=" and BianInfo  like '35@%'";
                    dylinetiaojian = " and LineInfo  like '35@%'";
                }
                for (int i = 4; i < 10; i++)
                {
                    if (obj_sheet.Cells[row, 2].Value.ToString() == "变电")
                    {
                        tiaojian = " ProjectID='"+ProjID+"' and BuildEd='" + (i + 2006) + "' and Col3='改造'  and Col4='bian'" + diqutiaojian + dydztiaojian;
                    
                        //向表写入主变台数
                        double ZBsum1 = 0;
                        if (Services.BaseService.GetObject("SelectTZGSSUMnum1", tiaojian) == null)
                        {
                            ZBsum1 = 0;
                        }
                        else
                        {
                            ZBsum1 = (double)Services.BaseService.GetObject("SelectTZGSSUMnum1", tiaojian);
                        }
                        obj_sheet.SetValue(row , i, ZBsum1);
                        //向表写入断路器台数
                        double DLQsum1 = 0;
                        if (Services.BaseService.GetObject("SelectTZGSSUMnum2", tiaojian) == null)
                        {
                            DLQsum1 = 0;
                        }
                        else
                        {
                            DLQsum1 = (double)Services.BaseService.GetObject("SelectTZGSSUMnum2", tiaojian);
                        }
                        obj_sheet.SetValue(row + 1, i, DLQsum1);

                    }
                    if (obj_sheet.Cells[row, 2].Value.ToString() == "架空线路")
                    {

                        tiaojian = " ProjectID='"+ProjID+"' and BuildEd='" + (i + 2006) + "' and Col3='改造' and Col4='line'"+ diqutiaojian + dylinetiaojian;
                        //向表写入架空线长度
                        double JKXsum1 = 0;
                        if (Services.BaseService.GetObject("SelectTZGSSUMLength", tiaojian) == null)
                        {
                            JKXsum1 = 0;
                        }
                        else
                        {
                            JKXsum1 = (double)Services.BaseService.GetObject("SelectTZGSSUMLength", tiaojian);
                        }
                        obj_sheet.SetValue(row , i, JKXsum1);
                        //向表写入耐热导线长
                        double NRDsum1 = 0;
                        if (Services.BaseService.GetObject("SelectTZGSSUMnum1", tiaojian) == null)
                        {
                            NRDsum1 = 0;
                        }
                        else
                        {
                            NRDsum1 = (double)Services.BaseService.GetObject("SelectTZGSSUMnum1", tiaojian);
                        }
                        obj_sheet.SetValue(row + 1, i, NRDsum1);
                        //向表写入杆塔（基）
                        double GTsum1 = 0;
                        if (Services.BaseService.GetObject("SelectTZGSSUMnum2", tiaojian) == null)
                        {
                            GTsum1 = 0;
                        }
                        else
                        {
                            GTsum1 = (double)Services.BaseService.GetObject("SelectTZGSSUMnum2", tiaojian);
                        }
                        obj_sheet.SetValue(row + 2, i, GTsum1);

                    }
                    if (obj_sheet.Cells[row, 2].Value.ToString() == "电缆线路")
                    {
                        tiaojian = " ProjectID='"+ProjID+"' and BuildEd='" + (i + 2006) + "' and Col3='改造' and Col4='line' " + diqutiaojian + dylinetiaojian;
                        //向表写入电缆长度（km)
                        double DLsum1 = 0;
                        if (Services.BaseService.GetObject("SelectTZGSSUMLength2", tiaojian) == null)
                        {
                            DLsum1 = 0;
                        }
                        else
                        {
                            DLsum1 = (double)Services.BaseService.GetObject("SelectTZGSSUMLength2", tiaojian);
                        }
                        obj_sheet.SetValue(row, i, DLsum1);
                        //向表写入沟道（km）长度
                        double GDsum1 = 0;
                        if (Services.BaseService.GetObject("SelectTZGSSUMnum6", tiaojian) == null)
                        {
                            GDsum1 = 0;
                        }
                        else
                        {
                            GDsum1 = (double)Services.BaseService.GetObject("SelectTZGSSUMnum6", tiaojian);
                        }
                        obj_sheet.SetValue(row + 1, i, GDsum1);

                    }

                }
                //如果不是“架空线路”，必然是“变电”或者“电缆线路”，
                //架空线路对应三行内容，“变电”或者“电缆线路” 对应二行内容。
                if (obj_sheet.Cells[row, 2].Value.ToString() == "架空线路")
                {
                    row = row + 2;
                }
                else
                {
                    row = row + 1;
                }
            }

        }

        private void barBtnDiaoChu_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            string fname = "";
            saveFileDialog1.Filter = "Microsoft Excel (*.xls)|*.xls";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                fname = saveFileDialog1.FileName;

                try
                {
                    fpSpread1.SaveExcel(fname);
                    //fps.SaveExcel(fname);
                    // 定义要使用的Excel 组件接口
                    // 定义Application 对象,此对象表示整个Excel 程序
                    Microsoft.Office.Interop.Excel.Application excelApp = null;
                    // 定义Workbook对象,此对象代表工作薄
                    Microsoft.Office.Interop.Excel.Workbook workBook;
                    // 定义Worksheet 对象,此对象表示Execel 中的一张工作表
                    Microsoft.Office.Interop.Excel.Worksheet ws = null;
                    Microsoft.Office.Interop.Excel.Range range = null;
                    excelApp = new Microsoft.Office.Interop.Excel.Application();
                    workBook = excelApp.Workbooks.Open(fname, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);
                    for (int i = 1; i <= workBook.Worksheets.Count; i++)
                    {

                        ws = (Microsoft.Office.Interop.Excel.Worksheet)workBook.Worksheets[i];
                        //取消保护工作表
                        ws.Unprotect(Missing.Value);
                        //有数据的行数
                        int row = ws.UsedRange.Rows.Count;
                        //有数据的列数
                        int col = ws.UsedRange.Columns.Count;
                        //创建一个区域
                        range = ws.get_Range(ws.Cells[1, 1], ws.Cells[row, col]);
                        //设区域内的单元格自动换行
                        range.WrapText = true;
                        //自动行高
                        //range.Cells.Rows.AutoFit();
                        //保护工作表
                        //ws.Protect(Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);
                    }
                    //保存工作簿
                    workBook.Save();
                    //关闭工作簿
                    excelApp.Workbooks.Close();
                    if (MsgBox.ShowYesNo("导出成功，是否打开该文档？") != DialogResult.Yes)
                        return;

                    System.Diagnostics.Process.Start(fname);
                }
                catch
                {
                    MsgBox.Show("无法保存" + fname + "。请用其他文件名保存文件，或将文件存至其他位置。");
                    return;
                }
            }
        }
        private void barBtnClose_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }
        private void barBtnRefrehData_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            WaitDialogForm newwait=new WaitDialogForm("", "正在更新数据, 请稍候...");
            //清空原有sheet的数据，但保留sheetname
            fc.SpreadClearSheet(fpSpread1);
            //生成一个空表用来保存当前表
            FarPoint.Win.Spread.SheetView obj_sheet = null;
            //生成一个空表，行列值都设为0用来做为程序处理时的当前表，这样可以提高处理速度
            FarPoint.Win.Spread.SheetView activesheet = new FarPoint.Win.Spread.SheetView();
            activesheet.RowCount = 0;
            activesheet.ColumnCount = 0;
            //添加空表
            fpSpread1.Sheets.Add(activesheet);
            //保留当前表，以备程序结束后还原当前表
            obj_sheet = fpSpread1.ActiveSheet;
            //将空表设为当前表
            fpSpread1.ActiveSheet = activesheet;


            build_sheet71(fpSpread1.Sheets[0]);
            build_Sheet72(fpSpread1.Sheets[1]);
            build_Sheet72_36(fpSpread1.Sheets[2]);
            build_Sheet73(fpSpread1.Sheets[3]);
            build_Sheet73_37(fpSpread1.Sheets[4]);
            build_Sheet74(fpSpread1.Sheets[5]);
            build_Sheet74_38(fpSpread1.Sheets[6]);
            build_Sheet75(fpSpread1.Sheets[7]);
            build_Sheet75_39(fpSpread1.Sheets[8]);
            build_Sheet76(fpSpread1.Sheets[9]);
            build_Sheet76_40(fpSpread1.Sheets[10]);
            build_Sheet77(fpSpread1.Sheets[11]);
            build_Sheet78(fpSpread1.Sheets[12]);
            build_Sheet79(fpSpread1.Sheets[13]);

            //移除空表
            fpSpread1.Sheets.Remove(activesheet);
            //还原当前表
            fpSpread1.ActiveSheet = obj_sheet;
            newwait.Close();
            MessageBox.Show("更新数据完成！");
        }
        private void barBtnSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            WaitDialogForm wait = null;
            wait = new WaitDialogForm("", "正在保存数据, 请稍候...");
            //判断文件夹xls是否存在，不存在则创建
            if (!Directory.Exists(System.Windows.Forms.Application.StartupPath + "\\xls"))
            {
                Directory.CreateDirectory(System.Windows.Forms.Application.StartupPath + "\\xls");
            }
            fpSpread1.SaveExcel(System.Windows.Forms.Application.StartupPath + "\\xls\\GYDWGH.xls");
            wait.Close();
            MsgBox.Show("保存成功");
          

        }
    }
}