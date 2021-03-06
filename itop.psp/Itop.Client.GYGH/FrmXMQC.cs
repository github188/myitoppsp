using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
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
using System.IO;
using Microsoft.Office.Interop.Excel;

namespace Itop.Client.GYGH
{
    public partial class FrmXMQC : FormBase
    {

        //工程号
        string ProjID = Itop.Client.MIS.ProgUID;
        //为表中手写部分数据建的类
        private  class sdata
        {
            //存放工程名称
          public  string title="";
           //存放其他事项
          public  string str1="";
          public  string str2="";
            
        }
        //sd用于存放表格中那些手工写入的部分，每当更新数据时
        //先把表格中手写部分存起来，更新数据完成后再写回手写部分
        //每个有手写的表格（不固定）都有两个方法，一个在更新前写入手写数据，一个在更新后写回手写数据
        List<sdata> sd=new List<sdata>() ;
        //用自定义类创建对象
        fpcommon fc=new fpcommon() ;
        //工作表行数
        int rowcount = 0;
        //工作表列数据
        int colcount = 0;
        //工作表第一行的表名
        string title = "";
        //工作表标签名
        string sheetname = "";
        public FrmXMQC()
        {
            InitializeComponent();
        }

       
        private void FrmXMQC_Load(object sender, EventArgs e)
        {
            //根据窗口变化全部添满
            fpSpread1.Dock = System.Windows.Forms.DockStyle.Fill;
            fpSpread_addsheet();
        }
        private void fpSpread_addsheet()
        {
            WaitDialogForm wait = null;
            wait = new WaitDialogForm("", "正在加载数据, 请稍候...");
            try
            {
                //打开Excel表格
                //清空工作表
                fpSpread1.Sheets.Clear();
                fpSpread1.OpenExcel(System.Windows.Forms.Application.StartupPath + "\\xls\\XMQC.xls");
                fc.SpreadRemoveEmptyCells(fpSpread1);
                //设定表2-1中的手工输入部分的格式
                Sheet21_SetCellType(fpSpread1.Sheets[2]);
                //设定表3中的手工输入部分的格式
                Sheet3_SetCellType(fpSpread1.Sheets[5]);
                //设定表4中的手工输入部分的格式
                Sheet4_SetCellType(fpSpread1.Sheets[6]);

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
                //保存excel文件
                fpSpread1.SaveExcel(System.Windows.Forms.Application.StartupPath + "\\xls\\XMQC.xls");
                //以下是打开文件设表格自动换行

                // 定义要使用的Excel 组件接口
                // 定义Application 对象,此对象表示整个Excel 程序
                Microsoft.Office.Interop.Excel.Application excelApp = null;
                // 定义Workbook对象,此对象代表工作薄
                Microsoft.Office.Interop.Excel.Workbook workBook;
                // 定义Worksheet 对象,此对象表示Execel 中的一张工作表
                Microsoft.Office.Interop.Excel.Worksheet ws = null;
                Microsoft.Office.Interop.Excel.Range range = null;
                excelApp = new Microsoft.Office.Interop.Excel.Application();
                string filename = System.Windows.Forms.Application.StartupPath + "\\xls\\XMQC.xls";
                workBook = excelApp.Workbooks.Open(filename, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);
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
                    //保护工作表
                    ws.Protect(Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);

                }
                //保存工作簿
                workBook.Save();
                //关闭工作簿
                excelApp.Workbooks.Close();

            }
            wait.Close();
      
        }
        private void Firstadddata()
        {
            //生成表1-1
            FarPoint.Win.Spread.SheetView Sheet11 = new FarPoint.Win.Spread.SheetView();
            //生成表1-2
            FarPoint.Win.Spread.SheetView Sheet12 = new FarPoint.Win.Spread.SheetView();
            //生成表2-1
            FarPoint.Win.Spread.SheetView Sheet21 = new FarPoint.Win.Spread.SheetView();
            //生成表2-2
            FarPoint.Win.Spread.SheetView Sheet22 = new FarPoint.Win.Spread.SheetView();
            //生成表2-3
            FarPoint.Win.Spread.SheetView Sheet23 = new FarPoint.Win.Spread.SheetView();
            //生成表3
            FarPoint.Win.Spread.SheetView Sheet3 = new FarPoint.Win.Spread.SheetView();
            //生成表4
            FarPoint.Win.Spread.SheetView Sheet4 = new FarPoint.Win.Spread.SheetView();


            //创建表1-1
            Build_Sheet11(Sheet11);
            //创建表1-2
            Build_Sheet12(Sheet12);
            //创建表2-1
            Build_Sheet21(Sheet21);
            //创建表2-2
            Build_Sheet22(Sheet22);
            //创建表2-3
            Build_Sheet23(Sheet23);
            //创建表3
            Build_Sheet3(Sheet3);
            //创建表4
            Build_Sheet4(Sheet4);



            //添加表1-1
            fpSpread1.Sheets.Add(Sheet11);
            //添加表1-2
            fpSpread1.Sheets.Add(Sheet12);
            //添加2-1
            fpSpread1.Sheets.Add(Sheet21);
            //添加表2-2
            fpSpread1.Sheets.Add(Sheet22);
            //添加表2-3
            fpSpread1.Sheets.Add(Sheet23);
            //添加表3
            fpSpread1.Sheets.Add(Sheet3);
            //添加表4
            fpSpread1.Sheets.Add(Sheet4);

           fc.Sheet_Colautoenter(fpSpread1);
        }
        private void Build_Sheet11(FarPoint.Win.Spread.SheetView obj_sheet)
        {
            //91行18列
            rowcount = 91;
            colcount = 18;
            title = "项目清册表1-1 铜陵市高压配电网新建扩建工程规模及投资汇总表";
            sheetname = "项目清册表1-1";
            fc.Sheet_RowCol_Title_Name(obj_sheet, rowcount, colcount, title, sheetname);
            fc.Sheet_GridandCenter(obj_sheet);
            fc.Sheet_Referen_R1C1(obj_sheet);
            //设定表格列宽度
            obj_sheet.Columns[0].Width = 120;
            obj_sheet.Columns[1].Width = 90;
            obj_sheet.Columns[2].Width = 40;
            obj_sheet.Columns[3].Width = 135;
            //obj_sheet.Columns[3].Width = 60;
            //obj_sheet.Columns[4].Width = 110;
            obj_sheet.Columns[10].Width = 85;
            //向表中写入相关的标题
            obj_sheet.AddSpanCell(1, 0, 2, 1);
            obj_sheet.SetValue(1, 0, "工程类别");
            obj_sheet.AddSpanCell(1, 1, 2, 1);
            obj_sheet.SetValue(1, 1, "电压等级（kV）");
            obj_sheet.AddSpanCell(1, 2, 2, 2);
            obj_sheet.SetValue(1, 2, "项   目");
            //循环写入年份
            for (int i = 4; i < 16; i++)
            {
                obj_sheet.AddSpanCell(1, i, 1, 2);
                obj_sheet.SetValue(1, i, (2010 + (i - 4) / 2) + "年");
                obj_sheet.SetValue(2, i, "规模");
                obj_sheet.SetValue(2, ++i, "投资(万元)");
                obj_sheet.Columns[i].Width = 70;
            }
            obj_sheet.AddSpanCell(1, 16, 1, 2);
            obj_sheet.SetValue(1, 16, "“十二五”合计");
            obj_sheet.SetValue(2, 16, "规模");
            obj_sheet.SetValue(2, 17, "投资(万元)");
            obj_sheet.Columns[17].Width = 70;
            //除表头部分开始行号
            int startrow = 3;
            //变电站-线路类型所占行数
            int length1 = 16;
            int j1 = 0;
            //线路线路类型所占行数
            int length2 = 8;
            int j2 = 0;
            //写入提高供电能力部分
            Sheet11_AddItems(obj_sheet, startrow + length1 * j1 + length2 * j2, "变线");
            obj_sheet.AddSpanCell(startrow + length1 * j1 + length2 * j2, 0, length1, 1);
            obj_sheet.SetValue(startrow + length1 * j1 + length2 * j2, 0, "提高供电能力");
            j1++;
            //网架结构优化
            Sheet11_AddItems(obj_sheet, startrow + length1 * j1 + length2 * j2, "线线");
            obj_sheet.AddSpanCell(startrow + length1 * j1 + length2 * j2, 0,length2, 1);
            obj_sheet.SetValue(startrow + length1 * j1 + length2 * j2, 0, "网架结构优化");
            j2++;
            //电厂接入
            Sheet11_AddItems(obj_sheet, startrow + length1     * j1 + length2 * j2, "线线");
            obj_sheet.AddSpanCell(startrow + length1 * j1 + length2 * j2, 0, length2, 1);
            obj_sheet.SetValue(startrow + length1 * j1 + length2 * j2, 0, "电厂接入");
            j2++;
            //电铁供电
            Sheet11_AddItems(obj_sheet, startrow + length1 * j1 + length2 * j2, "线线");
            obj_sheet.AddSpanCell(startrow + length1 * j1 + length2 * j2, 0, length2, 1);
            obj_sheet.SetValue(startrow + length1 * j1 + length2 * j2, 0, "电铁供电");
            j2++;
            //新农村电气化
            Sheet11_AddItems(obj_sheet, startrow + length1 * j1 + length2 * j2, "变线");
            obj_sheet.AddSpanCell(startrow + length1 * j1 + length2 * j2, 0, length1, 1);
            obj_sheet.SetValue(startrow + length1 * j1 + length2 * j2, 0, "新农村电气化");
            j1++;

            //无电地区电力建设
            Sheet11_AddItems(obj_sheet, startrow + length1 * j1 + length2 * j2, "变线");
            obj_sheet.AddSpanCell(startrow + length1 * j1 + length2 * j2, 0, length1, 1);
            obj_sheet.SetValue(startrow + length1 * j1 + length2 * j2, 0, "无电地区电力建设");
            j1++;
            //其它类别
            Sheet11_AddItems(obj_sheet, startrow + length1 * j1 + length2 * j2, "变线");
            obj_sheet.AddSpanCell(startrow + length1 * j1 + length2 * j2, 0, length1, 1);
            obj_sheet.SetValue(startrow + length1 * j1 + length2 * j2, 0, "其他类别");

            //写入数据
            Sheet11_AddData(obj_sheet, 3, 91);
            //写入求和公式
            fc.Sheet_WriteFormula_MoreColsum(obj_sheet, 3, 6, 88, 5, 3, 16, 2);
            fc.Sheet_Locked(obj_sheet);
        }
        private void Sheet11_AddItems(FarPoint.Win.Spread.SheetView obj_sheet, int row, string liebie)
        {
            //类别有两种，一种是“变线”，另和种是"线线"
            //分别对应“提高供电能力”和“优化网架结构”的电压和项目部分的写入
            if (liebie == "变线")
            {
                //110（66）部分
                obj_sheet.SetValue(row, 3, "变电站座数(座)");
                obj_sheet.SetValue(row + 1, 3, "主变台数(台)");
                obj_sheet.SetValue(row + 2, 3, "变电容量(MVA)");
                obj_sheet.SetValue(row + 3, 3, "无功补偿容量（Mvar）");
                obj_sheet.AddSpanCell(row, 2, 4, 1);
                obj_sheet.SetValue(row, 2, "变电");
                obj_sheet.AddSpanCell(row, 17, 4, 1);

                obj_sheet.AddSpanCell(row + 4, 2, 1, 2);
                obj_sheet.SetValue(row + 4, 2, "电源侧间隔数(个)");

                obj_sheet.SetValue(row + 5, 3, "线路条数(条)");
                obj_sheet.SetValue(row + 6, 3, "架空长度(km)");
                obj_sheet.SetValue(row + 7, 3, "电缆长度(km)");
                obj_sheet.AddSpanCell(row + 5, 2, 3, 1);
                obj_sheet.SetValue(row + 5, 2, "线路");
                obj_sheet.AddSpanCell(row + 5, 17, 3, 1);

                obj_sheet.AddSpanCell(row, 1, 8, 1);
                obj_sheet.SetValue(row, 1, "110（66）");
                //35部分
                row = row + 8;
                obj_sheet.SetValue(row, 3, "变电站座数(座)");
                obj_sheet.SetValue(row + 1, 3, "主变台数(台)");
                obj_sheet.SetValue(row + 2, 3, "变电容量(MVA)");
                obj_sheet.SetValue(row + 3, 3, "无功补偿容量（Mvar）");
                obj_sheet.AddSpanCell(row, 2, 4, 1);
                obj_sheet.SetValue(row, 2, "变电");
                obj_sheet.AddSpanCell(row, 17, 4, 1);

                obj_sheet.AddSpanCell(row + 4, 2, 1, 2);
                obj_sheet.SetValue(row + 4, 2, "电源侧间隔数(个)");

                obj_sheet.SetValue(row + 5, 3, "线路条数(条)");
                obj_sheet.SetValue(row + 6, 3, "架空长度(km)");
                obj_sheet.SetValue(row + 7, 3, "电缆长度(km)");
                obj_sheet.AddSpanCell(row + 5, 2, 3, 1);
                obj_sheet.SetValue(row + 5, 2, "线路");
                obj_sheet.AddSpanCell(row + 5, 17, 3, 1);

                obj_sheet.AddSpanCell(row, 1, 8, 1);
                obj_sheet.SetValue(row, 1, "35");
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
                obj_sheet.AddSpanCell(row + 1, 17, 3, 1);

                obj_sheet.AddSpanCell(row, 1, 4, 1);
                obj_sheet.SetValue(row, 1, "110（66）");

                //35部分
                row   = row + 4;
                obj_sheet.AddSpanCell(row, 2, 1, 2);
                obj_sheet.SetValue(row, 2, "电源侧间隔数(个)");

                obj_sheet.SetValue(row + 1, 3, "线路条数(条)");
                obj_sheet.SetValue(row + 2, 3, "架空长度(km)");
                obj_sheet.SetValue(row + 3, 3, "电缆长度(km)");
                obj_sheet.AddSpanCell(row + 1, 2, 3, 1);
                obj_sheet.SetValue(row + 1, 2, "线路");
                obj_sheet.AddSpanCell(row + 1, 17, 3, 1);

                obj_sheet.AddSpanCell(row, 1, 4, 1);
                obj_sheet.SetValue(row, 1, "35");

            }


        }
        private void Sheet11_AddData(FarPoint.Win.Spread.SheetView obj_sheet, int rowstart,   int countrow)
        {
            //根据表1-1特点，把项目部分做为一个突破口，对应“变电”“电源侧间隔数(个)”和“线路”三个基本部分
            //用行做外循环控制，列（年份）做内循环控制，取工程类别和电压等级做判断（合并单元格有空能值为空，调用Sheet_find_notnull方法），
            for (int row = rowstart; row < countrow; row++)
            {
                string tiaojian = "";
                //查电源侧间隔数单价的条件
                string danjiaditojian     = "";
                //记录工程序类别
                string type = fc.Sheet_find_Rownotemptycell(obj_sheet, row, 0);
                //记录电压等级
                string dianya = fc.Sheet_find_Rownotemptycell(obj_sheet, row, 1);
                for (int i = 4; i < 16; i++)
                {

                    if (obj_sheet.Cells[row, 2].Value.ToString() == "变电")
                    {
                        //判断电压
                        if (dianya == "110（66）")
                        {
                            tiaojian = " ProjectID='"+ProjID+"' and BuildEd='" + (2010 + (i - 4) / 2)+ "' and (Col3='新建' or Col3='扩建') and Col4='bian' and  ProgType='" + type + "' and (BianInfo like '110@%' or BianInfo  like '66@%')";

                        }
                        if (dianya == "35")
                        {
                            tiaojian = " ProjectID='"+ProjID+"' and BuildEd='" + (i + 2006) + "' and (Col3='新建' or Col3='扩建') and Col4='bian' and  ProgType='" + type + "' and BianInfo  like '35@%'";
                        }
                        //向表写入变电站台数 
                        double BDZsum1 = 0;
                        if (Services.BaseService.GetObject("SelectTZGSSUMnum5", tiaojian) != null)
                        {
                            BDZsum1 = (double)Services.BaseService.GetObject("SelectTZGSSUMnum5", tiaojian);
                        }
                        obj_sheet.SetValue(row + 0, i, BDZsum1);
                        //向表写入主变台数
                        double ZBsum1 = 0;
                        if (Services.BaseService.GetObject("SelectTZGSSUMnum1", tiaojian) != null)
                        {
                            ZBsum1 = (double)Services.BaseService.GetObject("SelectTZGSSUMnum1", tiaojian);
                        }
                        obj_sheet.SetValue(row + 1, i, ZBsum1);
                        //向表写入变电容量
                        double BDRLsum1 = 0;
                        if (Services.BaseService.GetObject("SelectTZGSSUMVolumn", tiaojian) != null)
                        {
                            BDRLsum1 = (double)Services.BaseService.GetObject("SelectTZGSSUMVolumn", tiaojian);
                        }
                        obj_sheet.SetValue(row + 2, i, BDRLsum1);
                        //向表写入无功补尝容量
                        double WGBCsum1 = 0;
                        if (Services.BaseService.GetObject("SelectTZGSSUMWGNum", tiaojian) != null)
                        {
                            WGBCsum1 = (double)Services.BaseService.GetObject("SelectTZGSSUMWGNum", tiaojian);
                        }
                        obj_sheet.SetValue(row + 3, i, WGBCsum1);
                        //写入投资金额
                        double TZJEsum = 0;
                        if (Services.BaseService.GetObject("SelectTZGSSUMAmount", tiaojian) != null)
                        {
                            TZJEsum = (double)Services.BaseService.GetObject("SelectTZGSSUMAmount", tiaojian);
                        }
                        obj_sheet.AddSpanCell(row, i     + 1, 4, 1);
                        obj_sheet.SetValue(row, i     + 1, TZJEsum);

                    }
                    if (obj_sheet.Cells[row, 2].Value.ToString() == "电源侧间隔数(个)")
                    {
                        if (dianya == "110（66）")
                        {
                            tiaojian = " ProjectID='"+ProjID+"' and BuildEd='" + (2010 + (i - 4) / 2) + "' and (Col3='新建' or Col3='扩建') and Col4='' and  ProgType='" + type + "' and (BianInfo like '110@%' or BianInfo  like '66@%')";
                            danjiaditojian = "Name='电源侧间隔' and ( S1='110'or S1='66')";

                        }
                        if (dianya == "35")
                        {
                            tiaojian = " ProjectID='"+ProjID+"' and BuildEd='" + (2010 + (i - 4) / 2) + "' and (Col3='新建' or Col3='扩建') and Col4='' and  ProgType='" + type + "' and BianInfo  like '35@%'";
                            danjiaditojian = "Name='电源侧间隔' and  S1='35'";
                        }
                        //向表写入电源侧间隔数(个)
                        int JGsum1 = 0;
                        if (Services.BaseService.GetObject("SelectTZGSSUMJGNum", tiaojian) != null)
                        {
                            JGsum1 = (int)Services.BaseService.GetObject("SelectTZGSSUMJGNum", tiaojian);
                        }
                        obj_sheet.SetValue(row + 0, i, JGsum1);
                        //电源侧间隔数(个)投资金额为个数*单价
                        //单价数据在PSP_Project_Sum表中，其中S1为电压等级Name为名称,Num为单价

                        double danjia = 0;
                        if (Services.BaseService.GetObject("SelectProject_Sum_NUM", danjiaditojian) != null)
                        {
                            danjia = (double)Services.BaseService.GetObject("SelectProject_Sum_NUM", danjiaditojian);
                        }
                        double JGjine = danjia * JGsum1;

                        obj_sheet.SetValue(row, i + 1, JGjine);

                    }
                    if (obj_sheet.Cells[row, 2].Value.ToString() == "线路")
                    {
                        if (dianya == "110（66）")
                        {
                            tiaojian = " ProjectID='"+ProjID+"' and BuildEd='" + (2010 + (i - 4) / 2) + "' and (Col3='新建' or Col3='扩建') and Col4='line' and  ProgType='" + type + "' and (LineInfo like '110@%' or LineInfo  like '66@%')";

                        }
                        if (dianya == "35")
                        {
                            tiaojian = " ProjectID='"+ProjID+"' and BuildEd='" + (2010 + (i - 4) / 2) + "' and (Col3='新建' or Col3='扩建') and Col4='line' and  ProgType='" + type + "' and LineInfo  like '35@%'";
                        }
                        //向表写入线路条数
                        double XLsum1 = 0;
                        if (Services.BaseService.GetObject("SelectTZGSSUMnum5", tiaojian) != null)
                        {
                            XLsum1 = (double)Services.BaseService.GetObject("SelectTZGSSUMnum5", tiaojian);
                        }
                        obj_sheet.SetValue(row + 0, i, XLsum1);
                        //向表写入架空线长
                        double JKlenth1 = 0;
                        if (Services.BaseService.GetObject("SelectTZGSSUMLength", tiaojian) != null)
                        {
                            JKlenth1 = (double)Services.BaseService.GetObject("SelectTZGSSUMLength", tiaojian);
                        }
                        obj_sheet.SetValue(row + 1, i, JKlenth1);
                        //向表写入电缆线长度
                        double DLXlenth1 = 0;
                        if (Services.BaseService.GetObject("SelectTZGSSUMLength2", tiaojian) != null)
                        {
                            DLXlenth1 = (double)Services.BaseService.GetObject("SelectTZGSSUMLength2", tiaojian);
                        }
                        obj_sheet.SetValue(row + 2, i, DLXlenth1);
                        //写入投资金额
                        double TZJEsum = 0;
                        if (Services.BaseService.GetObject("SelectTZGSSUMAmount", tiaojian) != null)
                        {
                            TZJEsum = (double)Services.BaseService.GetObject("SelectTZGSSUMAmount", tiaojian);
                        }
                        obj_sheet.AddSpanCell(row, i + 1, 3, 1);
                        obj_sheet.SetValue(row, i + 1, TZJEsum);
                    }
                    //列数据加1
                    i++;

                }
                //如果是“变电”每个对应四行内容，如果是“线路”每个对应三行内容，如果是”电源侧间隔数(个)“每个对应一行内容，
                //循环本身再加1，指向下一个内容
                if (obj_sheet.Cells[row, 2].Value.ToString() == "变电")
                {
                    row = row + 3;
                }
                else if (obj_sheet.Cells[row, 2].Value.ToString() == "线路")
                {
                    row = row + 2;
                }
            }
        }
        private void Build_Sheet12(FarPoint.Win.Spread.SheetView obj_sheet)
        {
            //19列
            string tiaojian = " and a.ProjectID='"+ProjID+"' and  cast(substring(a.BianInfo,1,charindex('@',a.BianInfo,0)-1) as int)>=35 and (a.Col3='新建' or a.Col3='扩建') and a.Col4='' order by a.DQ";
            IList<Ps_Table_TZGS> ptz = Services.BaseService.GetList<Ps_Table_TZGS>("SelectPs_Table_TZGS_GCQD_lgm", tiaojian);
            int itemcout = ptz.Count;
            //计算行数
            rowcount = 3 + itemcout; ;
            colcount = 19;
            title = "表1-2 铜陵市高压配电网新建扩建工程明细表";
            sheetname = "项目清册表1-2";
            fc.Sheet_RowCol_Title_Name(obj_sheet, rowcount, colcount, title, sheetname);
            fc.Sheet_GridandCenter(obj_sheet);
            //设定表格列宽度
            obj_sheet.Columns[0].Width = 50;
            obj_sheet.Columns[1].Width = 60;
            obj_sheet.Columns[2].Width = 70;
            obj_sheet.Columns[3].Width = 90;
            obj_sheet.Columns[4].Width = 50;
            obj_sheet.Columns[5].Width = 200;
            obj_sheet.Columns[7].Width = 200;

            obj_sheet.Columns[11].Width = 80;
            obj_sheet.Columns[14].Width = 70;
            obj_sheet.Columns[15].Width = 80;

            obj_sheet.Columns[18].Width = 120;
            //obj_sheet.Columns[3].Width = 60;
            //obj_sheet.Columns[4].Width = 110;
            //obj_sheet.Columns[10].Width = 85;

            obj_sheet.Rows[2].Height = 40;

            //向表中写入相关的标题
            obj_sheet.AddSpanCell(1, 0, 2, 1);
            obj_sheet.SetValue(1, 0, "序号");
            obj_sheet.AddSpanCell(1, 1, 2, 1);
            obj_sheet.SetValue(1, 1, "地市名称");
            obj_sheet.AddSpanCell(1, 2, 1, 2);
            obj_sheet.SetValue(1, 2, "供电区");
            obj_sheet.SetValue(2, 2, "名称");
            obj_sheet.SetValue(2, 3, "性质");
            obj_sheet.AddSpanCell(1, 4, 2, 1);
            obj_sheet.SetValue(1, 4, "电压等级(kV)");
            //obj_sheet.Cells[1, 4].CellType = cellType;
                  obj_sheet.AddSpanCell(1, 5, 2, 1);
            obj_sheet.SetValue(1, 5, "工程名称");
            //obj_sheet.Columns[5].CellType = cellType;
            obj_sheet.AddSpanCell(1, 6, 2, 1);
            obj_sheet.SetValue(1, 6, "建设类型");
            obj_sheet.AddSpanCell(1, 7, 2, 1);
            obj_sheet.SetValue(1, 7, "工程描述");
            obj_sheet.AddSpanCell(1, 8, 1, 3);
            obj_sheet.SetValue(1, 8, "线路工程");
            obj_sheet.SetValue(2, 8, "条数(条)");
            obj_sheet.SetValue(2, 9, "架空(km)");
            obj_sheet.SetValue(2, 10, "电缆(km)");
            obj_sheet.AddSpanCell(1, 11, 1, 4);
            obj_sheet.SetValue(1, 11, "变电工程");
            obj_sheet.SetValue(2, 11, "变电站座数（座）");
            obj_sheet.SetValue(2, 12, "台数(台)");
            obj_sheet.SetValue(2, 13, "容量(MVA))");
            obj_sheet.SetValue(2, 14, "无功补偿容量(Mvat)");
            obj_sheet.AddSpanCell(1, 15, 2, 1);
            obj_sheet.SetValue(1, 15, "电源侧间隔数(个)");
            obj_sheet.AddSpanCell(1, 16, 2, 1);
            obj_sheet.SetValue(1, 16, "投资(万元)");
            obj_sheet.AddSpanCell(1, 17, 2, 1);
            obj_sheet.SetValue(1, 17, "投运时间(年)");
            obj_sheet.AddSpanCell(1, 18, 2, 1);
            obj_sheet.SetValue(1, 18, "工程属性");
            //循环写入数据
            for (int row = 3; row < rowcount;  row++)
            {
                //写入序号
                obj_sheet.SetValue(row, 0, row - 2);
                obj_sheet.SetValue(row, 1, "铜陵");
                //根据工程和工程描述的长度来确定行高
                //每个字符宽度定义为6.45
                int hanggao = 1;
                int titlelength   = fc.Text_Lenght(ptz[row - 3].Title.ToString());
                int Col1length   = fc.Text_Lenght(ptz[row - 3].Col1.ToString());
                if (titlelength   > Col1length)
                {
                    hanggao = int.Parse(Math.Ceiling(double.Parse((titlelength   * 6.45).ToString( )) / double.Parse("200")).ToString());
                }
                else
                {
                    hanggao = int.Parse(Math.Ceiling(double.Parse((Col1length   * 6.45).ToString()) / double.Parse("200")).ToString());
                }
                //如果行数超过2，则每增加一行，行高增加15个像素（单行高为20）
                obj_sheet.Rows[row].Height = 20 + (hanggao - 1) * 15;

                if (ptz[row - 3].DQ == "市辖供电区")
                {
                    obj_sheet.SetValue(row,  2 , "铜陵市区");
                }
                else
                   {
                       obj_sheet.SetValue(row, 2, "铜陵县区");
                   }
                obj_sheet.SetValue(row, 3, ptz[row - 3].DQ);
                //BianInfo存放截断后的电压数据
                obj_sheet.SetValue(row, 4, ptz[row - 3].BianInfo);
                obj_sheet.SetValue(row, 5, ptz[row - 3].Title);
                obj_sheet.SetValue(row, 6, ptz[row - 3].Col3);
                //工程描述
                obj_sheet.SetValue(row, 7, ptz[row - 3].Col1);

                //线路工程 y1990存Num5 线路条数,y1991存length架空线长,y1992存length2电缆线长,
                obj_sheet.SetValue(row, 8, ptz[row - 3].y1990);
                obj_sheet.SetValue(row, 9, ptz[row - 3].y1991);
                obj_sheet.SetValue(row, 10, ptz[row - 3].y1992);

                //变电工程 y1993存num5 变电站个数,y1994存num1 主变台数,y1995存Volumn 变电容量，y1996存放WGNum无功补偿容量
                obj_sheet.SetValue(row, 11, ptz[row - 3].y1993);
                obj_sheet.SetValue(row, 12, ptz[row - 3].y1994);
                obj_sheet.SetValue(row, 13, ptz[row - 3].y1995);
                obj_sheet.SetValue(row, 14, ptz[row - 3].y1996);

                obj_sheet.SetValue(row, 15, ptz[row - 3].JGNum);
                obj_sheet.SetValue(row, 16, ptz[row - 3].Amount);
                obj_sheet.SetValue(row, 17, ptz[row - 3].BuildEd);
                obj_sheet.SetValue(row, 18, ptz[row - 3].ProgType);
            }
            fc.Sheet_Locked(obj_sheet);
        }
        private void Build_Sheet21(FarPoint.Win.Spread.SheetView obj_sheet)
        {
            //19行17列
            rowcount = 19;
            colcount = 17;
            title = "表2-1 铜陵市高压配电网改造工程规模及投资汇总表";
            sheetname = "项目清册表2-1";
            fc.Sheet_RowCol_Title_Name(obj_sheet, rowcount, colcount, title, sheetname);
            fc.Sheet_GridandCenter(obj_sheet);
            fc.Sheet_Referen_R1C1(obj_sheet);
            //设定表格列宽度
            obj_sheet.Columns[0].Width = 80;
            obj_sheet.Columns[1].Width = 70;
            obj_sheet.Columns[2].Width = 145;
            //obj_sheet.Columns[3].Width = 135;
            //obj_sheet.Columns[3].Width = 60;
            //obj_sheet.Columns[4].Width = 110;
            //obj_sheet.Columns[10].Width = 85;
            //向表中写入相关的标题
            obj_sheet.AddSpanCell(1, 0, 2, 1);
            obj_sheet.SetValue(1, 0, "电压等级（kV）");
            obj_sheet.AddSpanCell(1, 1, 2, 2);
            obj_sheet.SetValue(1, 1, "项   目");
            //循环写入年份
            for (int i = 3; i < 15; i++)
            {
                obj_sheet.AddSpanCell(1, i, 1, 2);
                obj_sheet.SetValue(1, i, (2010 + (i - 3) / 2) + "年");
                obj_sheet.SetValue(2, i, "规模");
                obj_sheet.SetValue(2, ++i, "投资(万元)");
                obj_sheet.Columns[i].Width = 70;
            }
            obj_sheet.AddSpanCell(1, 15, 1, 2);
            obj_sheet.SetValue(1, 15, "“十二五”合计");
            obj_sheet.SetValue(2, 15, "规模");
            obj_sheet.SetValue(2, 16, "投资(万元)");
            obj_sheet.Columns[16].Width = 70;
            //除表头部分开始行号
            //int startrow = 3;
            //变电站-线路类型所占行数
            //添加110(66)部分
            Sheet21_AddItems(obj_sheet, 3, "110(66)");
            //添加35部分
            Sheet21_AddItems(obj_sheet, 11, "35");
            Sheet21_RefrehData(obj_sheet);
            //写求和公式
            fc.Sheet_WriteFormula_MoreColsum_NoSpan(obj_sheet, 3, 5, 16, 5, 3, 15, 2);
            fc.Sheet_Locked(obj_sheet);
            Sheet21_SetCellType(obj_sheet);
        }
        private void Sheet21_SetCellType(FarPoint.Win.Spread.SheetView obj_sheet)
        {

            //人工输入部分的单元格设为可写并设格式
            for (int col = 3; col < 15; col++)
            {
                fc.Sheet_UnLockedandCellNumber(obj_sheet, 10, col);
                fc.Sheet_UnLockedandCellNumber(obj_sheet, 18, col);
            }

        }
        private void Sheet21_AddItems(FarPoint.Win.Spread.SheetView obj_sheet, int row, string dianya)
        {
            //
            obj_sheet.SetValue(row, 2, "变压器台数（台）");
            obj_sheet.SetValue(row + 1, 2, "断路器台数（台）");
            obj_sheet.AddSpanCell(row, 1, 2, 1);
            obj_sheet.SetValue(row, 1, "变电");
            obj_sheet.AddSpanCell(row, 16, 2, 1);

            obj_sheet.SetValue(row + 2, 2, "长度（km）");
            obj_sheet.SetValue(row + 3, 2, "其中：耐热导线（km）");
            obj_sheet.SetValue(row + 4, 2, "杆塔（基）");
            obj_sheet.AddSpanCell(row + 2, 1, 3, 1);
            obj_sheet.SetValue(row + 2, 1, "架空线路");


            obj_sheet.SetValue(row + 5, 2, "长度（km）");
            obj_sheet.SetValue(row + 6, 2, "沟道（km）");
            obj_sheet.AddSpanCell(row + 5, 1, 2, 1);
            obj_sheet.SetValue(row + 5, 1, "电缆线路");
            obj_sheet.AddSpanCell(row + 2, 16, 5, 1);

            obj_sheet.AddSpanCell(row + 7, 1, 1, 2);
            obj_sheet.SetValue(row + 7, 1, "其它");

            obj_sheet.AddSpanCell(row, 0, 8, 1);
            obj_sheet.SetValue(row, 0, dianya);

        }
        private void Sheet21_AddData(FarPoint.Win.Spread.SheetView obj_sheet, int rowstart, string dianya)
        {
            // 把电压等级做为一个输入参数，每个电压等级后面的项目执行一次
            string tiaojianbian = "";
            string tiaojianline = "";
            string biandianya = "";
            string linedianya = "";
            for (int i = 3; i < 15; i++)
            {
                //判断电压
                if (dianya == "110（66）")
                {
                    biandianya = "and (BianInfo like '110@%' or BianInfo  like '66@%')";
                    linedianya = "and (LineInfo like '110@%' or LineInfo  like '66@%')";
                }
                if (dianya == "35")
                {
                    biandianya = "and BianInfo like '35@%'";
                    linedianya = "and LineInfo like '35@%'";
                }
                tiaojianbian = "BuildEd='" + (2010 + (i - 3) / 2) + "' and Col3='改造' and Col4='bian'" + biandianya;

                //向表写入主变台数
                double ZBsum1 = 0;
                if (Services.BaseService.GetObject("SelectTZGSSUMnum1", tiaojianbian) != null)
                {
                    ZBsum1 = (double)Services.BaseService.GetObject("SelectTZGSSUMnum1", tiaojianbian);
                }
                obj_sheet.SetValue(rowstart, i, ZBsum1);
                //向表写入断路器台数
                double DLQsum1 = 0;
                if (Services.BaseService.GetObject("SelectTZGSSUMnum2", tiaojianbian) != null)
                {
                    DLQsum1 = (double)Services.BaseService.GetObject("SelectTZGSSUMnum2", tiaojianbian);
                }
                obj_sheet.SetValue(rowstart + 1, i, DLQsum1);

                //写入投资金额
                double BDZTZJEsum = 0;
                if (Services.BaseService.GetObject("SelectTZGSSUMAmount", tiaojianbian) != null)
                {
                    BDZTZJEsum = (double)Services.BaseService.GetObject("SelectTZGSSUMAmount", tiaojianbian);
                }
                obj_sheet.AddSpanCell(rowstart, i + 1, 2, 1);
                obj_sheet.SetValue(rowstart, i + 1, BDZTZJEsum);


                tiaojianline = "BuildEd='" + (2010 + (i - 3) / 2) + "' and Col3='改造' and Col4='line'" + linedianya;

                //向表写入架空线长
                double JKlenth1 = 0;
                if (Services.BaseService.GetObject("SelectTZGSSUMLength", tiaojianline) != null)
                {
                    JKlenth1 = (double)Services.BaseService.GetObject("SelectTZGSSUMLength", tiaojianline);
                }
                obj_sheet.SetValue(rowstart + 2, i, JKlenth1);
                //向表写入耐热导线长
                double NRlength = 0;
                if (Services.BaseService.GetObject("SelectTZGSSUMnum1", tiaojianline) != null)
                {
                    NRlength = (double)Services.BaseService.GetObject("SelectTZGSSUMnum1", tiaojianline);
                }
                obj_sheet.SetValue(rowstart + 3, i, NRlength);
                //向表写入杆塔（基）
                double GTJsum1 = 0;
                if (Services.BaseService.GetObject("SelectTZGSSUMnum2", tiaojianline) != null)
                {
                    GTJsum1 = (double)Services.BaseService.GetObject("SelectTZGSSUMnum2", tiaojianline);
                }
                obj_sheet.SetValue(rowstart + 4, i, GTJsum1);

                //向表写入电缆线长度
                double DLXlenth1 = 0;
                if (Services.BaseService.GetObject("SelectTZGSSUMLength2", tiaojianline) != null)
                {
                    DLXlenth1 = (double)Services.BaseService.GetObject("SelectTZGSSUMLength2", tiaojianline);
                }
                obj_sheet.SetValue(rowstart + 5, i, DLXlenth1);
                //向表写入沟道长度
                double GDlenth1 = 0;
                if (Services.BaseService.GetObject("SelectTZGSSUMnum6", tiaojianline) != null)
                {
                    GDlenth1 = (double)Services.BaseService.GetObject("SelectTZGSSUMnum6", tiaojianline);
                }
                obj_sheet.SetValue(rowstart + 6, i, GDlenth1);

                //写入投资金额
                double LINETZJEsum = 0;
                if (Services.BaseService.GetObject("SelectTZGSSUMAmount", tiaojianline) != null)
                {
                    LINETZJEsum = (double)Services.BaseService.GetObject("SelectTZGSSUMAmount", tiaojianline);
                }
                obj_sheet.AddSpanCell(rowstart + 2, i + 1, 5, 1);
                obj_sheet.SetValue(rowstart + 2, i + 1, LINETZJEsum);

                //列数据加1
                i++;

            }
        }
        private void Sheet21_RefrehData(FarPoint.Win.Spread.SheetView obj_sheet)
        {
            //项目清册表2-1为固定格式表，其中有一部分是人工输入部分
            //更新数据时保留人工输入部分，只用此方法更新其它的数据部分
            //写入数据
            Sheet21_AddData(obj_sheet, 3, "110(66)");
            Sheet21_AddData(obj_sheet, 11, "35");
        }
        private void Build_Sheet22(FarPoint.Win.Spread.SheetView obj_sheet)
        {
            //13列
            string tiaojian = " and  a.ProjectID='"+ProjID+"' and  cast(substring(a.BianInfo,1,charindex('@',a.BianInfo,0)-1) as int)>=35 and a.Col3='改造' and a.Col4='' order by a.DQ";
            IList<Ps_Table_TZGS> ptz = Services.BaseService.GetList<Ps_Table_TZGS>("SelectPs_Table_TZGS_GCQD_lgm", tiaojian);
            int itemcout = ptz.Count;
            //计算行数
            rowcount = 3 + itemcout; ;
            colcount = 13;
            title = "表2-2 铜陵市高压配电网变电站改造工程明细表";
            sheetname = "项目清册表2-2";
            fc.Sheet_RowCol_Title_Name(obj_sheet, rowcount, colcount, title, sheetname);
            fc.Sheet_GridandCenter(obj_sheet);
            //设定表格列宽度
            obj_sheet.Columns[0].Width = 50;
            obj_sheet.Columns[1].Width = 60;
            obj_sheet.Columns[2].Width = 70;
            obj_sheet.Columns[3].Width = 90;
            obj_sheet.Columns[4].Width = 50;
            obj_sheet.Columns[5].Width = 200;
            obj_sheet.Columns[7].Width = 200;

            obj_sheet.Columns[10].Width = 120;

            //向表中写入相关的标题
            obj_sheet.AddSpanCell(1, 0, 2, 1);
            obj_sheet.SetValue(1, 0, "序号");
            obj_sheet.AddSpanCell(1, 1, 2, 1);
            obj_sheet.SetValue(1, 1, "地市名称");
            obj_sheet.AddSpanCell(1, 2, 1, 2);
            obj_sheet.SetValue(1, 2, "供电区");
            obj_sheet.SetValue(2, 2, "名称");
            obj_sheet.SetValue(2, 3, "性质");
            obj_sheet.AddSpanCell(1, 4, 2, 1);
            obj_sheet.SetValue(1, 4, "电压等级(kV)");
            //obj_sheet.Cells[1, 4].CellType = cellType;
            obj_sheet.AddSpanCell(1, 5, 2, 1);
            obj_sheet.SetValue(1, 5, "项目名称");
            //obj_sheet.Columns[5].CellType = cellType;
            obj_sheet.AddSpanCell(1, 6, 2, 1);
            obj_sheet.SetValue(1, 6, "建设类型");
            obj_sheet.AddSpanCell(1, 7, 2, 1);
            obj_sheet.SetValue(1, 7, "工程描述");

            obj_sheet.AddSpanCell(1, 8, 2, 1);
            obj_sheet.SetValue(1, 8, "主变压器（台）");
            obj_sheet.AddSpanCell(1, 9, 2, 1);
            obj_sheet.SetValue(1, 9, "断路器（台）");
            obj_sheet.AddSpanCell(1, 10, 2, 1);
            obj_sheet.SetValue(1, 10, "其它（项）");
            obj_sheet.AddSpanCell(1, 11, 2, 1);
            obj_sheet.SetValue(1, 11, "投资(万元)");
            obj_sheet.AddSpanCell(1, 12, 2, 1);
            obj_sheet.SetValue(1, 12, "改造时间（年）");
            //锁定表格中的单元格
            fc.Sheet_Locked(obj_sheet);
            //循环写入数据
            for (int row = 3; row < rowcount; row++)
            {
                //写入序号
                obj_sheet.SetValue(row, 0, row - 2);
                obj_sheet.SetValue(row, 1, "铜陵");
                //根据工程和工程描述的长度来确定行高
                //每个字符宽度定义为6.45
                int hanggao = 1;
                int titlelength = fc.Text_Lenght(ptz[row - 3].Title.ToString());
                int Col1length = fc.Text_Lenght(ptz[row - 3].Col1.ToString());
                if (titlelength > Col1length)
                {
                    hanggao = int.Parse(Math.Ceiling(double.Parse((titlelength * 6.45).ToString()) / double.Parse("200")).ToString());
                }
                else
                {
                    hanggao = int.Parse(Math.Ceiling(double.Parse((Col1length * 6.45).ToString()) / double.Parse("200")).ToString());
                }
                //如果行数超过2，则每增加一行，行高增加15个像素（单行高为20）
                obj_sheet.Rows[row].Height = 20 + (hanggao - 1) * 15;


                if (ptz[row - 3].DQ == "市辖供电区")
                {
                    obj_sheet.SetValue(row, 2, "铜陵市区");
                }
                else
                {
                    obj_sheet.SetValue(row, 2, "铜陵县区");
                }
                obj_sheet.SetValue(row, 3, ptz[row - 3].DQ);
                //BianInfo存放截断后的电压数据
                obj_sheet.SetValue(row, 4, ptz[row - 3].BianInfo);
                obj_sheet.SetValue(row, 5, ptz[row - 3].Title);
                obj_sheet.SetValue(row, 6, ptz[row - 3].Col3);
                //工程描述
                obj_sheet.SetValue(row, 7, ptz[row - 3].Col1);
                //y1994存num1 主变台数,y1997存Num2断路器数 ,y1998存Amount 变电站投资金额
                obj_sheet.SetValue(row, 8, ptz[row - 3].y1994);
                obj_sheet.SetValue(row, 9, ptz[row - 3].y1997);
                //使手工输入部分可写
                obj_sheet.Cells[row, 10].Locked = false;

                obj_sheet.SetValue(row, 11, ptz[row - 3].y1998);
                obj_sheet.SetValue(row, 12, ptz[row - 3].BuildEd);

            }


        }
        private void Sheet22_SaveData(FarPoint.Win.Spread.SheetView obj_sheet)
        {
            //每个有手写的表格（不固定）都有两个方法，一个在更新前写入手写数据，一个在更新后写回手写数据
            //存表2-2中人工输入的其他项，并与项目名称形成关联 title中存项目名称，str1存其他项的内容

            //存放数据时先清空这个列表
            sd.Clear();
            int row = obj_sheet.RowCount;
            //循环读取表格中的两项内容，并写入sd中
            for (int i = 3; i < row;   i++)
            {
                sdata stest = new sdata();
                stest.title = obj_sheet.Cells[i, 5].Value.ToString();
                if (obj_sheet.Cells[i, 10].Value != null)
                {
                    stest.str1 = obj_sheet.Cells[i, 10].Value.ToString();
                }
                sd.Add(stest);
            }
        }
        private void Sheet22_WirteData(FarPoint.Win.Spread.SheetView obj_sheet)
        {
            //每个有手写的表格（不固定）都有两个方法，一个在更新前写入手写数据，一个在更新后写回手写数据
            //用于更新数据后写回手写部分
            int row = obj_sheet.RowCount;
            for (int i = 3; i < row; i++)
            {
                for (int j = 0; j < sd.Count; j++)
                {
                    //通过项目名称的比对来回写数据
                    if (obj_sheet.Cells[i, 5].Value.ToString() ==   sd[j].title)
                    {
                        obj_sheet.SetValue(i, 10, sd[j].str1);
                        //比对完与后删除这一项，有利于下次比对提高效率
                        sd.Remove(sd[j]);
                        break;
                    }
                }

            }
        }
        private void Build_Sheet23(FarPoint.Win.Spread.SheetView obj_sheet)
        {
            //17列
            string tiaojian = " and  a.ProjectID='"+ProjID+"' and  cast(substring(a.BianInfo,1,charindex('@',a.BianInfo,0)-1) as int)>=35 and a.Col3='改造' and a.Col4='' order by a.DQ";
            IList<Ps_Table_TZGS> ptz = Services.BaseService.GetList<Ps_Table_TZGS>("SelectPs_Table_TZGS_GCQD_lgm", tiaojian);
            int itemcout = ptz.Count;
            //计算行数
            rowcount = 3 + itemcout; ;
            colcount = 17;
            title = "表2-3 铜陵市高压配电网线路改造工程明细表";
            sheetname = "项目清册表2-3";
            fc.Sheet_RowCol_Title_Name(obj_sheet, rowcount, colcount, title, sheetname);
            fc.Sheet_GridandCenter(obj_sheet);
            //设定表格列宽度
            obj_sheet.Columns[0].Width = 50;
            obj_sheet.Columns[1].Width = 60;
            obj_sheet.Columns[2].Width = 70;
            obj_sheet.Columns[3].Width = 90;
            obj_sheet.Columns[4].Width = 50;
            obj_sheet.Columns[5].Width = 200;
            obj_sheet.Columns[7].Width = 200;
            obj_sheet.Columns[9].Width = 70;

            obj_sheet.Columns[11].Width = 120;
            obj_sheet.Columns[14].Width = 120;

            obj_sheet.Rows[2].Height = 40;

            //向表中写入相关的标题
            obj_sheet.AddSpanCell(1, 0, 2, 1);
            obj_sheet.SetValue(1, 0, "序号");
            obj_sheet.AddSpanCell(1, 1, 2, 1);
            obj_sheet.SetValue(1, 1, "地市名称");
            obj_sheet.AddSpanCell(1, 2, 1, 2);
            obj_sheet.SetValue(1, 2, "供电区");
            obj_sheet.SetValue(2, 2, "名称");
            obj_sheet.SetValue(2, 3, "性质");
            obj_sheet.AddSpanCell(1, 4, 2, 1);
            obj_sheet.SetValue(1, 4, "电压等级(kV)");
            //obj_sheet.Cells[1, 4].CellType = cellType;
            obj_sheet.AddSpanCell(1, 5, 2, 1);
            obj_sheet.SetValue(1, 5, "项目名称");
            //obj_sheet.Columns[5].CellType = cellType;
            obj_sheet.AddSpanCell(1, 6, 2, 1);
            obj_sheet.SetValue(1, 6, "建设类型");
            obj_sheet.AddSpanCell(1, 7, 2, 1);
            obj_sheet.SetValue(1, 7, "工程描述");

            obj_sheet.AddSpanCell(1, 8, 1, 4);
            obj_sheet.SetValue(1, 8, "架空线路");
            obj_sheet.SetValue(2, 8, "导线长度(km)");
            obj_sheet.SetValue(2, 9, "其中:耐热导线(km)");
            obj_sheet.SetValue(2, 10, "杆塔(基)");
            obj_sheet.SetValue(2, 11, "其它(项）");

            obj_sheet.AddSpanCell(1, 12, 1, 3);
            obj_sheet.SetValue(1, 12, "电缆线路");
            obj_sheet.SetValue(2, 12, "电缆长度(km)");
            obj_sheet.SetValue(2, 13, "沟道(km)");
            obj_sheet.SetValue(2, 14, "其它(项)");

            obj_sheet.AddSpanCell(1, 15, 2, 1);
            obj_sheet.SetValue(1, 15, "投资(万元)");
            obj_sheet.AddSpanCell(1, 16, 2, 1);
            obj_sheet.SetValue(1, 16, "改造时间（年）");
            //锁定表格中的单元格
            fc.Sheet_Locked(obj_sheet);
            //循环写入数据
            for (int row = 3; row < rowcount; row++)
            {
                //写入序号
                obj_sheet.SetValue(row, 0, row - 2);
                obj_sheet.SetValue(row, 1, "铜陵");
                //根据工程和工程描述的长度来确定行高
                //每个字符宽度定义为6.45
                int hanggao = 1;
                int titlelength = fc.Text_Lenght(ptz[row - 3].Title.ToString());
                int Col1length = fc.Text_Lenght(ptz[row - 3].Col1.ToString());
                if (titlelength > Col1length)
                {
                    hanggao = int.Parse(Math.Ceiling(double.Parse((titlelength * 6.45).ToString()) / double.Parse("200")).ToString());
                }
                else
                {
                    hanggao = int.Parse(Math.Ceiling(double.Parse((Col1length * 6.45).ToString()) / double.Parse("200")).ToString());
                }
                //如果行数超过2，则每增加一行，行高增加15个像素（单行高为20）
                obj_sheet.Rows[row].Height = 20 + (hanggao - 1) * 15;


                if (ptz[row - 3].DQ == "市辖供电区")
                {
                    obj_sheet.SetValue(row, 2, "铜陵市区");
                }
                else
                {
                    obj_sheet.SetValue(row, 2, "铜陵县区");
                }
                obj_sheet.SetValue(row, 3, ptz[row - 3].DQ);
                //BianInfo存放截断后的电压数据
                obj_sheet.SetValue(row, 4, ptz[row - 3].BianInfo);
                obj_sheet.SetValue(row, 5, ptz[row - 3].Title);
                obj_sheet.SetValue(row, 6, ptz[row - 3].Col3);
                //工程描述
                obj_sheet.SetValue(row, 7, ptz[row - 3].Col1);
                //y1991存length架空线长,y1992存length2电缆线长,
                //y2004存num1耐热导长,y2005存num2杆塔基数,y2006存Num6沟道长度,y2007存Amount线路投资金额

                obj_sheet.SetValue(row, 8, ptz[row - 3].y1991);
                obj_sheet.SetValue(row, 9, ptz[row - 3].y2004);
                obj_sheet.SetValue(row, 10, ptz[row - 3].y2005);
                //使手工输入部分可写
                obj_sheet.Cells[row, 11].Locked = false;

                obj_sheet.SetValue(row, 12, ptz[row - 3].y1992);
                obj_sheet.SetValue(row, 13, ptz[row - 3].y2006);

                //使手工输入部分可写
                obj_sheet.Cells[row, 14].Locked = false;

                obj_sheet.SetValue(row, 15, ptz[row - 3].y2007);
                obj_sheet.SetValue(row, 16, ptz[row - 3].BuildEd);


            }


        }
        private void Sheet23_SaveData(FarPoint.Win.Spread.SheetView obj_sheet)
        {
            //每个有手写的表格（不固定）都有两个方法，一个在更新前写入手写数据，一个在更新后写回手写数据
            //存表2-3中人工输入的两个其他项，并与项目名称形成关联 title中存项目名称，str1存其他项1的内容，str2存其他项2的内容

            //存放数据时先清空这个列表
            sd.Clear();
            int row = obj_sheet.RowCount;
            //循环读取表格中的三项内容，并写入sd中
            for (int i = 3; i < row; i++)
            {
                sdata stest = new sdata();
                stest.title = obj_sheet.Cells[i, 5].Value.ToString();
                if (obj_sheet.Cells[i, 11].Value != null)
                {
                    stest.str1 = obj_sheet.Cells[i, 11].Value.ToString();
                }
                if (obj_sheet.Cells[i, 14].Value != null)
                {
                    stest.str2 = obj_sheet.Cells[i, 14].Value.ToString();
                }
                sd.Add(stest);
            }
        }
        private void Sheet23_WirteData(FarPoint.Win.Spread.SheetView obj_sheet)
        {
            //每个有手写的表格（不固定）都有两个方法，一个在更新前写入手写数据，一个在更新后写回手写数据
            //用于更新数据后写回手写部分
            int row = obj_sheet.RowCount;
            for (int i = 3; i < row; i++)
            {
                for (int j = 0; j < sd.Count; j++)
                {
                    //通过项目名称的比对来回写数据
                    if (obj_sheet.Cells[i, 5].Value.ToString() == sd[j].title)
                    {
                        obj_sheet.SetValue(i, 11, sd[j].str1);
                        obj_sheet.SetValue(i, 14, sd[j].str1);
                        //比对完与后删除这一项，有利于下次比对提高效率
                        sd.Remove(sd[j]);
                        break;
                    }
                }

            }
        }
        private void Build_Sheet3(FarPoint.Win.Spread.SheetView obj_sheet)
        {
            //43行18列
            rowcount = 43;
            colcount = 18;
            title = "表3 铜陵市中压配电网新建工程规模及投资汇总表";
            sheetname = "项目清册表3";
            fc.Sheet_RowCol_Title_Name(obj_sheet, rowcount, colcount, title, sheetname);
            fc.Sheet_GridandCenter(obj_sheet);
            fc.Sheet_Referen_R1C1(obj_sheet);
            //设定表格列宽度
            obj_sheet.Columns[0].Width = 110;
            obj_sheet.Columns[1].Width = 60;
            obj_sheet.Columns[2].Width = 65;
            obj_sheet.Columns[3].Width = 135;
            //obj_sheet.Columns[3].Width = 60;
            //obj_sheet.Columns[4].Width = 110;
            //obj_sheet.Columns[10].Width = 85;
            //向表中写入相关的标题
            obj_sheet.AddSpanCell(1, 0, 2, 1);
            obj_sheet.SetValue(1, 0, "工程类型");
            obj_sheet.AddSpanCell(1, 1, 2, 1);
            obj_sheet.SetValue(1, 1, "电压等级（kV）");
            obj_sheet.AddSpanCell(1, 2, 2, 2);
            obj_sheet.SetValue(1, 2, "项   目");
            //循环写入年份
            for (int i = 4; i < 16; i++)
            {
                obj_sheet.AddSpanCell(1, i, 1, 2);
                obj_sheet.SetValue(1, i, (2010 + (i - 4) / 2) + "年");
                obj_sheet.SetValue(2, i, "规模");
                obj_sheet.SetValue(2, ++i, "投资(万元)");
                obj_sheet.Columns[i].Width = 70;
            }
            obj_sheet.AddSpanCell(1, 16, 1, 2);
            obj_sheet.SetValue(1, 16, "“十二五”合计");
            obj_sheet.SetValue(2, 16, "规模");
            obj_sheet.SetValue(2, 17, "投资(万元)");
            obj_sheet.Columns[17].Width = 70;
            //除表头部分开始行号
            int startrow = 3;
            //中压线路 或者 低压网配套 所占行数
            int length1 = 2;
            int j1 = 0;
            //中压配电 或者 中压开关 所占行数
            int length2 = 4;
            int j2 = 0;
            int oldstartrow = 0;
            //写入变电站配套送出部分
            oldstartrow = startrow + length1 * j1 + length2 * j2;
            Sheet3_AddItems(obj_sheet, startrow + length1 * j1 + length2 * j2, "中压线路");
            j1++;
            Sheet3_AddItems(obj_sheet, startrow + length1 * j1 + length2 * j2, "中压配电");
            j2++;
            Sheet3_AddItems(obj_sheet, startrow + length1 * j1 + length2 * j2, "中压开关");
            j2++;
            obj_sheet.AddSpanCell(oldstartrow, 1, length1 * j1 + length2 * j2 - oldstartrow + startrow, 1);
            obj_sheet.SetValue(oldstartrow, 1, "10");
            Sheet3_AddItems(obj_sheet, startrow + length1 * j1 + length2 * j2, "低压网配套");
            j1++;
            obj_sheet.AddSpanCell(oldstartrow, 0, length1 * j1 + length2 * j2 - oldstartrow + startrow, 1);
            obj_sheet.SetValue(oldstartrow, 0, "变电站配套送出");

            //配电网切改
            oldstartrow = startrow + length1 * j1 + length2 * j2;
            Sheet3_AddItems(obj_sheet, startrow + length1 * j1 + length2 * j2, "中压线路");
            j1++;
            Sheet3_AddItems(obj_sheet, startrow + length1 * j1 + length2 * j2, "中压开关");
            j2++;
            obj_sheet.AddSpanCell(oldstartrow, 1, length1 * j1 + length2 * j2 - oldstartrow + startrow, 1);
            obj_sheet.SetValue(oldstartrow, 1, "10");
            obj_sheet.AddSpanCell(oldstartrow, 0, length1 * j1 + length2 * j2 - oldstartrow + startrow, 1);
            obj_sheet.SetValue(oldstartrow, 0, "配电网切改");
            //架空线入地
            oldstartrow = startrow + length1 * j1 + length2 * j2;
            Sheet3_AddItems(obj_sheet, startrow + length1 * j1 + length2 * j2, "中压线路");
            j1++;
            Sheet3_AddItems(obj_sheet, startrow + length1 * j1 + length2 * j2, "中压配电");
            j2++;
            Sheet3_AddItems(obj_sheet, startrow + length1 * j1 + length2 * j2, "中压开关");
            j2++;
            obj_sheet.AddSpanCell(oldstartrow, 1, length1 * j1 + length2 * j2 - oldstartrow + startrow, 1);
            obj_sheet.SetValue(oldstartrow, 1, "10");
            obj_sheet.AddSpanCell(oldstartrow, 0, length1 * j1 + length2 * j2 - oldstartrow + startrow, 1);
            obj_sheet.SetValue(oldstartrow, 0, "架空线入地");

            //其他类别
            oldstartrow = startrow + length1 * j1 + length2 * j2;
            Sheet3_AddItems(obj_sheet, startrow + length1 * j1 + length2 * j2, "中压线路");
            j1++;
            Sheet3_AddItems(obj_sheet, startrow + length1 * j1 + length2 * j2, "中压配电");
            j2++;
            Sheet3_AddItems(obj_sheet, startrow + length1 * j1 + length2 * j2, "中压开关");
            j2++;
            obj_sheet.AddSpanCell(oldstartrow, 1, length1 * j1 + length2 * j2 - oldstartrow + startrow, 1);
            obj_sheet.SetValue(oldstartrow, 1, "10");
            Sheet3_AddItems(obj_sheet, startrow + length1 * j1 + length2 * j2, "低压网配套");
            j1++;
            obj_sheet.AddSpanCell(oldstartrow, 0, length1 * j1 + length2 * j2 - oldstartrow + startrow, 1);
            obj_sheet.SetValue(oldstartrow, 0, "其他类别");

            Sheet3_RefreshData(obj_sheet);

            //写求和公式
            fc.Sheet_WriteFormula_MoreColsum(obj_sheet, 3, 6, 38, 5, 3, 16, 2);
            //写入手工输入部分的求和公式
            string[] str ={ "", "", "", "", "", "" };
            for (int i = 6; i < 16; i++)
            {
                if (i % 2 == 0)
                {
                    str[0] += "," + "R14" + "C" + (i + 1);
                    str[1] += "," + "R15" + "C" + (i + 1);
                    str[3] += "," + "R42" + "C" + (i + 1);
                    str[4] += "," + "R43" + "C" + (i + 1);


                }
                else
                {
                    str[2] += "," + "R14" + "C" + (i + 1);
                    str[5] += "," + "R42" + "C" + (i + 1);
                }
            }
            str[0] = "SUM(" + str[0].Substring(1, str[0].Length - 1) + ")";
            str[1] = "SUM(" + str[1].Substring(1, str[1].Length - 1) + ")";
            str[2] = "SUM(" + str[2].Substring(1, str[2].Length - 1) + ")";
            str[3] = "SUM(" + str[3].Substring(1, str[3].Length - 1) + ")";
            str[4] = "SUM(" + str[4].Substring(1, str[4].Length - 1) + ")";
            str[5] = "SUM(" + str[5].Substring(1, str[5].Length - 1) + ")";
            obj_sheet.Cells[13, 16].Formula = str[0];
            obj_sheet.Cells[14, 16].Formula = str[1];
            obj_sheet.Cells[13, 17].Formula = str[2];
            obj_sheet.Cells[41, 16].Formula = str[3];
            obj_sheet.Cells[42, 16].Formula = str[4];
            obj_sheet.Cells[41, 17].Formula = str[5];

            //锁定表格
            fc.Sheet_Locked(obj_sheet);
            Sheet3_SetCellType(obj_sheet);

        }
        private void Sheet3_SetCellType(FarPoint.Win.Spread.SheetView obj_sheet)
        {
            //解开用户手工输入部分并设定格式
            for (int i = 4; i < 16; i++)
            {
                fc.Sheet_UnLockedandCellNumber(obj_sheet, 13, i);
                fc.Sheet_UnLockedandCellNumber(obj_sheet, 14, i);
                fc.Sheet_UnLockedandCellNumber(obj_sheet, 13, i+1);
                fc.Sheet_UnLockedandCellNumber(obj_sheet, 41, i);
                fc.Sheet_UnLockedandCellNumber(obj_sheet, 42, i);
                fc.Sheet_UnLockedandCellNumber(obj_sheet, 41, i+1);
            
                i++;
            }
        }
        private void Sheet3_AddItems(FarPoint.Win.Spread.SheetView obj_sheet, int row, string liebie)
        {
            //类别有四种，分别是“中压线路”，“中压配电”，“中压开关”和“低压网配套”
            if (liebie == "中压线路")
            {

                obj_sheet.SetValue(row, 3, "架空长度（km）");
                obj_sheet.SetValue(row + 1, 3, "电缆长度（km）");
                obj_sheet.AddSpanCell(row, 2, 2, 1);
                obj_sheet.SetValue(row, 2, liebie);
                obj_sheet.AddSpanCell(row, 17, 2, 1);
            }
            if (liebie == "中压配电")
            {

                obj_sheet.SetValue(row, 3, " 配电室（座）");
                obj_sheet.SetValue(row + 1, 3, "箱变（座）");
                obj_sheet.SetValue(row + 2, 3, "柱上变（座）");
                obj_sheet.SetValue(row + 3, 3, "无功补偿容量（Mvar）");
                obj_sheet.AddSpanCell(row, 2, 4, 1);
                obj_sheet.SetValue(row, 2, "中压配电");
                obj_sheet.AddSpanCell(row, 17, 4, 1);
            }
            if (liebie == "中压开关")
            {

                obj_sheet.SetValue(row, 3, " 开闭站（座）");
                obj_sheet.SetValue(row + 1, 3, "环网柜（座）");
                obj_sheet.SetValue(row + 2, 3, "柱上开关（台）");
                obj_sheet.SetValue(row + 3, 3, "电缆分支箱（座）");
                obj_sheet.AddSpanCell(row, 2, 4, 1);
                obj_sheet.SetValue(row, 2, "中压开关");
                obj_sheet.AddSpanCell(row, 17, 4, 1);
            }
            if (liebie == "低压网配套")
            {

                obj_sheet.SetValue(row, 3, "架空线路长度（km）");
                obj_sheet.SetValue(row + 1, 3, "电缆线路长度（km）");
                obj_sheet.AddSpanCell(row, 1, 2, 2);
                obj_sheet.SetValue(row, 1, liebie);
                obj_sheet.AddSpanCell(row, 17, 2, 1);
            }

        }
        private void Sheet3_AddData(FarPoint.Win.Spread.SheetView obj_sheet, int rowstart, int countrow)
        {
            //根据表3特点，把项目部分做为一个突破口，对应“中压线路”“中压配电”和“中压开关”三个基本部分
            //低压网配套部分不用记算
            //用行做外循环控制，列（年份）做内循环控制，取工程类型（合并单元格有空能值为空，调用Sheet_find_notnull方法），
            for (int row = rowstart; row < countrow; row++)
            {
                string tiaojian = "";
                //记录工程序类别
                string type = fc.Sheet_find_Rownotemptycell(obj_sheet, row, 0);
                for (int i = 4; i < 16; i++)
                {
                    //判断是否有数据，如果没有那么对应低压网配套
                    if (obj_sheet.Cells[row, 2].Value != null)
                    {
                        if (obj_sheet.Cells[row, 2].Value.ToString() == "中压线路")
                        {
                            tiaojian = " ProjectID='"+ProjID+"' and BuildEd='" + (2010 + (i - 4) / 2) + "' and Col3='新建'and Col4='pw-line' and  ProgType='" + type + "' and LineInfo  like '10@%'";
                            //向表写入架空线长
                            double JKlenth1 = 0;
                            if (Services.BaseService.GetObject("SelectTZGSSUMLength", tiaojian) != null)
                            {
                                JKlenth1 = (double)Services.BaseService.GetObject("SelectTZGSSUMLength", tiaojian);
                            }
                            obj_sheet.SetValue(row, i, JKlenth1);
                            //向表写入电缆线长度
                            double DLXlenth1 = 0;
                            if (Services.BaseService.GetObject("SelectTZGSSUMLength2", tiaojian) != null)
                            {
                                DLXlenth1 = (double)Services.BaseService.GetObject("SelectTZGSSUMLength2", tiaojian);
                            }
                            obj_sheet.SetValue(row + 1, i, DLXlenth1);
                            //写入投资金额
                            double LINETZJEsum = 0;
                            if (Services.BaseService.GetObject("SelectTZGSSUMAmount", tiaojian) != null)
                            {
                                LINETZJEsum = (double)Services.BaseService.GetObject("SelectTZGSSUMAmount", tiaojian);
                            }
                            obj_sheet.AddSpanCell(row, i + 1, 2, 1);
                            obj_sheet.SetValue(row, i + 1, LINETZJEsum);
                        }
                        if (obj_sheet.Cells[row, 2].Value.ToString() == "中压配电")
                        {
                            tiaojian = " ProjectID='"+ProjID+"' and BuildEd='" + (2010 + (i - 4) / 2) + "' and Col3='新建'and Col4='pw-pb' and  ProgType='" + type + "' and BianInfo  like '10@%'";
                            //向表写入配电室（座）
                            double PDSsum = 0;
                            if (Services.BaseService.GetObject("SelectTZGSSUMnum1", tiaojian) != null)
                            {
                                PDSsum = (double)Services.BaseService.GetObject("SelectTZGSSUMnum1", tiaojian);
                            }
                            obj_sheet.SetValue(row, i, PDSsum);
                            //向表写入箱变（座）
                            double XBsum = 0;
                            if (Services.BaseService.GetObject("SelectTZGSSUMnum3", tiaojian) != null)
                            {
                                XBsum = (double)Services.BaseService.GetObject("SelectTZGSSUMnum3", tiaojian);
                            }
                            obj_sheet.SetValue(row + 1, i, XBsum);
                            //向表写入柱上变（座）
                            double ZSBsum = 0;
                            if (Services.BaseService.GetObject("SelectTZGSSUMnum5", tiaojian) != null)
                            {
                                ZSBsum = (double)Services.BaseService.GetObject("SelectTZGSSUMnum5", tiaojian);
                            }
                            obj_sheet.SetValue(row + 2, i, ZSBsum);
                            //向表写入无功补偿容量（Mvar）
                            double WGBCsum = 0;
                            if (Services.BaseService.GetObject("SelectTZGSSUMWGNum", tiaojian) != null)
                            {
                                WGBCsum = (double)Services.BaseService.GetObject("SelectTZGSSUMWGNum", tiaojian);
                            }
                            obj_sheet.SetValue(row + 3, i, WGBCsum);

                            //写入投资金额
                            double BIANTZJEsum = 0;
                            if (Services.BaseService.GetObject("SelectTZGSSUMAmount", tiaojian) != null)
                            {
                                BIANTZJEsum = (double)Services.BaseService.GetObject("SelectTZGSSUMAmount", tiaojian);
                            }
                            obj_sheet.AddSpanCell(row, i + 1, 4, 1);
                            obj_sheet.SetValue(row, i + 1, BIANTZJEsum);
                        }
                        if (obj_sheet.Cells[row, 2].Value.ToString() == "中压开关")
                        {
                            tiaojian = " ProjectID='"+ProjID+"' and BuildEd='" + (2010 + (i - 4) / 2) + "' and Col3='新建'and Col4='pw-kg' and  ProgType='" + type + "' and BianInfo  like '10@%'";
                            //向表写入开闭站（座）
                            double KBZsum = 0;
                            if (Services.BaseService.GetObject("SelectTZGSSUMnum1", tiaojian) != null)
                            {
                                KBZsum = (double)Services.BaseService.GetObject("SelectTZGSSUMnum1", tiaojian);
                            }
                            obj_sheet.SetValue(row, i, KBZsum);
                            //向表写入环网柜（座）
                            double HWGsum = 0;
                            if (Services.BaseService.GetObject("SelectTZGSSUMnum2", tiaojian) != null)
                            {
                                HWGsum = (double)Services.BaseService.GetObject("SelectTZGSSUMnum2", tiaojian);
                            }
                            obj_sheet.SetValue(row + 1, i, HWGsum);
                            //向表写入柱上开关（台）
                            double ZSKGsum = 0;
                            if (Services.BaseService.GetObject("SelectTZGSSUMnum3", tiaojian) != null)
                            {
                                ZSKGsum = (double)Services.BaseService.GetObject("SelectTZGSSUMnum3", tiaojian);
                            }
                            obj_sheet.SetValue(row + 2, i, ZSKGsum);
                            //向表写入电缆分支箱（座）
                            double DLFZsum = 0;
                            if (Services.BaseService.GetObject("SelectTZGSSUMnum4", tiaojian) != null)
                            {
                                DLFZsum = (double)Services.BaseService.GetObject("SelectTZGSSUMnum4", tiaojian);
                            }
                            obj_sheet.SetValue(row + 3, i, DLFZsum);

                            //写入投资金额
                            double KGTZJEsum = 0;
                            if (Services.BaseService.GetObject("SelectTZGSSUMAmount", tiaojian) != null)
                            {
                                KGTZJEsum = (double)Services.BaseService.GetObject("SelectTZGSSUMAmount", tiaojian);
                            }
                            obj_sheet.AddSpanCell(row, i + 1, 4, 1);
                            obj_sheet.SetValue(row, i + 1, KGTZJEsum);
                        }

                    }
                    else
                    {
                        //低压网配套时将相应的投资金额部分合并
                        obj_sheet.AddSpanCell(row, i + 1, 2, 1);
                    }

                    //列数据加1
                    i++;

                }
                //如果是“中压线路”每个对应二行内容，如果是“中压配电”每个对应四行内容，如果是”中压开关“每个对应四行内容，如果是”null"表示低压网配套，每个对应二行内容，
                //循环本身再加1，指向下一个内容
                if (obj_sheet.Cells[row, 2].Value != null)
                {
                    if (obj_sheet.Cells[row, 2].Value.ToString() == "中压配电" || obj_sheet.Cells[row, 2].Value.ToString() == "中压开关")
                    {
                        row = row + 3;
                    }
                    else
                    {
                        row = row + 1;
                    }
                }
                else
                {
                    row = row + 1;
                }

            }
        }
        private void Sheet3_RefreshData(FarPoint.Win.Spread.SheetView obj_sheet)
        {
            //项目清册表3为固定格式表，其中有一部分是人工输入部分
            //更新数据时保留人工输入部分，只用此方法更新其它的数据部分
            //写入数据
            Sheet3_AddData(obj_sheet, 3, 43);

        }
        private void Build_Sheet4(FarPoint.Win.Spread.SheetView obj_sheet)
        {
            //17行17列
            rowcount = 17;
            colcount = 17;
            title = "表4 铜陵市中压配电网改造工程规模及投资汇总表";
            sheetname = "项目清册表4";
            fc.Sheet_RowCol_Title_Name(obj_sheet, rowcount, colcount, title, sheetname);
            fc.Sheet_GridandCenter(obj_sheet);
            fc.Sheet_Referen_R1C1(obj_sheet);
            //设定表格列宽度
            obj_sheet.Columns[0].Width = 95;
            obj_sheet.Columns[1].Width = 60;
            obj_sheet.Columns[2].Width = 110;
            //obj_sheet.Columns[3].Width = 135;
            //obj_sheet.Columns[3].Width = 60;
            //obj_sheet.Columns[4].Width = 110;
            //obj_sheet.Columns[10].Width = 85;
            //向表中写入相关的标题
            obj_sheet.AddSpanCell(1, 0, 2, 1);
            obj_sheet.SetValue(1, 0, "电压等级（kV）");
            obj_sheet.AddSpanCell(1, 1, 2, 2);
            obj_sheet.SetValue(1, 1, "项   目");
            //循环写入年份
            for (int i = 3; i < 15; i++)
            {
                obj_sheet.AddSpanCell(1, i, 1, 2);
                obj_sheet.SetValue(1, i, (2010 + (i - 3) / 2) + "年");
                obj_sheet.SetValue(2, i, "规模");
                obj_sheet.SetValue(2, ++i, "投资(万元)");
                obj_sheet.Columns[i].Width = 70;
            }
            obj_sheet.AddSpanCell(1, 15, 1, 2);
            obj_sheet.SetValue(1, 15, "“十二五”合计");
            obj_sheet.SetValue(2, 15, "规模");
            obj_sheet.SetValue(2, 16, "投资(万元)");
            obj_sheet.Columns[16].Width = 70;
            //添加左侧表头内容
            //第一列内容
            obj_sheet.AddSpanCell(3, 0, 12, 1);
            obj_sheet.SetValue(3, 0, "10");

            obj_sheet.AddSpanCell(15, 0, 2, 2);
            obj_sheet.SetValue(15, 0, "低压线路");

            //第二及第三列
            obj_sheet.AddSpanCell(3, 1, 2, 1);
            obj_sheet.SetValue(3, 1, "高损配变");
            obj_sheet.SetValue(3, 2, "台数（台）");
            obj_sheet.SetValue(4, 2, "容量（MVA）");

            obj_sheet.AddSpanCell(5, 1, 2, 1);
            obj_sheet.SetValue(5, 1, "无功补偿装置");
            obj_sheet.SetValue(5, 2, "台数（台）");
            obj_sheet.SetValue(6, 2, "容量（MVA）");

            obj_sheet.AddSpanCell(7, 1, 4, 1);
            obj_sheet.SetValue(7, 1, "开关类");
            obj_sheet.SetValue(7, 2, "断路器（台）");
            obj_sheet.SetValue(8, 2, "负荷开关（台）");
            obj_sheet.SetValue(9, 2, "环网柜（座）");
            obj_sheet.SetValue(10, 2, "电缆分支箱（座）");

            obj_sheet.AddSpanCell(11, 1, 2, 1);
            obj_sheet.SetValue(11, 1, "架空线路");
            obj_sheet.SetValue(11, 2, "长度（km）");
            obj_sheet.SetValue(12, 2, "杆塔（基）");

            obj_sheet.AddSpanCell(13, 1, 2, 1);
            obj_sheet.SetValue(13, 1, "电缆线路");
            obj_sheet.SetValue(13, 2, "长度（km）");
            obj_sheet.SetValue(14, 2, "沟道（km）");

            obj_sheet.SetValue(15, 2, "架空线路（km）");
            obj_sheet.SetValue(16, 2, "电缆线路（km）");

            //合并手工输入部分的投资金额单元格
            for (int i = 4; i < 17; i++)
            {
                obj_sheet.AddSpanCell(3, i, 2, 1);
                obj_sheet.AddSpanCell(4, i, 2, 1);
                obj_sheet.AddSpanCell(5, i, 2, 1);
                obj_sheet.AddSpanCell(6, i, 2, 1);
                obj_sheet.AddSpanCell(15, i, 2, 1);
                obj_sheet.AddSpanCell(16, i, 2, 1);
                i++;
            }
            //合并"十二五"自动部分的单元格
            obj_sheet.AddSpanCell(7, 16, 4, 1);
            obj_sheet.AddSpanCell(11, 16, 4, 1);
            //写入数据
            Sheet4_AddData(obj_sheet);
            //写入求和公式
            fc.Sheet_WriteFormula_MoreColsum(obj_sheet, 3, 5, 14, 5, 3, 15, 2);

            //写入手工输入部分的计算公式
            //数组长为9
            string[] str ={ "", "", "", "","","","","","" };
            for (int i = 5; i < 15; i++)
            {
                if (i % 2 != 0)
                {
                    str[0] += "," + "R4" + "C" + (i + 1);
                    str[1] += "," + "R5" + "C" + (i + 1);
                    str[3] += "," + "R6" + "C" + (i + 1);
                    str[4] += "," + "R7" + "C" + (i + 1);
                    str[6] += "," + "R16" + "C" + (i + 1);
                    str[7] += "," + "R17" + "C" + (i + 1);
                }
                else
                {
                    str[2] += "," + "R4" + "C" + (i + 1);
                    str[5] += "," + "R6" + "C" + (i + 1);
                    str[8] += "," + "R16" + "C" + (i + 1);
                }
            }
            str[0] = "SUM(" + str[0].Substring(1, str[0].Length - 1) + ")";
            str[1] = "SUM(" + str[1].Substring(1, str[1].Length - 1) + ")";
            str[2] = "SUM(" + str[2].Substring(1, str[2].Length - 1) + ")";
            str[3] = "SUM(" + str[3].Substring(1, str[3].Length - 1) + ")";
            str[4] = "SUM(" + str[4].Substring(1, str[4].Length - 1) + ")";
            str[5] = "SUM(" + str[5].Substring(1, str[5].Length - 1) + ")";
            str[6] = "SUM(" + str[6].Substring(1, str[6].Length - 1) + ")";
            str[7] = "SUM(" + str[7].Substring(1, str[7].Length - 1) + ")";
            str[8] = "SUM(" + str[8].Substring(1, str[8].Length - 1) + ")";

            obj_sheet.Cells[3, 15].Formula = str[0];
            obj_sheet.Cells[4, 15].Formula = str[1];
            obj_sheet.Cells[3, 16].Formula = str[2];
            obj_sheet.Cells[5, 15].Formula = str[3];
            obj_sheet.Cells[6, 15].Formula = str[4];
            obj_sheet.Cells[5, 16].Formula = str[5];
            obj_sheet.Cells[15, 15].Formula = str[6];
            obj_sheet.Cells[16, 15].Formula = str[7];
            obj_sheet.Cells[15, 16].Formula = str[8];

            fc.Sheet_Locked(obj_sheet);
            //解锁手工输入部分并设定格式
            Sheet4_SetCellType(obj_sheet);
        }
        private void Sheet4_AddData(FarPoint.Win.Spread.SheetView obj_sheet)
        {
            string kgtiaojian = "";
            string linetiaojian = "";
            for (int col = 3; col < 15;col++ )
            {
                kgtiaojian = " ProjectID='"+ProjID+"' and BuildEd='" + (2010 + (col - 3) / 2) + "' and Col3='改造' and Col4='pw-kg' and BianInfo like '10@%'";
                //写入断路器台数
                double DLQsum = 0;
                if (Services.BaseService.GetObject("SelectTZGSSUMnum5", kgtiaojian)!=null)
                {
                    DLQsum = (double)Services.BaseService.GetObject("SelectTZGSSUMnum5", kgtiaojian);
                }
                obj_sheet.SetValue(7, col, DLQsum);
                //写入负荷开关（台）
                double FHKGsum = 0;
                if (Services.BaseService.GetObject("SelectTZGSSUMnum6", kgtiaojian) != null)
                {
                    FHKGsum = (double)Services.BaseService.GetObject("SelectTZGSSUMnum6", kgtiaojian);
                }
                obj_sheet.SetValue(8, col, FHKGsum);
                //写入环网柜（座）
                double HWGsum = 0;
                if (Services.BaseService.GetObject("SelectTZGSSUMnum2", kgtiaojian) != null)
                {
                    HWGsum = (double)Services.BaseService.GetObject("SelectTZGSSUMnum2", kgtiaojian);
                }
                obj_sheet.SetValue(9, col, HWGsum);
                //写入电缆分支箱（座）
                double DLFZsum = 0;
                if (Services.BaseService.GetObject("SelectTZGSSUMnum4", kgtiaojian) != null)
                {
                    DLFZsum = (double)Services.BaseService.GetObject("SelectTZGSSUMnum4", kgtiaojian);
                }
                obj_sheet.SetValue(10, col, DLFZsum);
                //写入开关投资金额
                double KGTZJEsum = 0;
                if (Services.BaseService.GetObject("SelectTZGSSUMAmount", kgtiaojian) != null)
                {
                    KGTZJEsum = (double)Services.BaseService.GetObject("SelectTZGSSUMAmount", kgtiaojian);
                }
                obj_sheet.AddSpanCell(7, col + 1, 4, 1);
                obj_sheet.SetValue(7, col + 1, KGTZJEsum);

                linetiaojian = " ProjectID='"+ProjID+"' and BuildEd='" + (2010 + (col - 3) / 2) + "' and Col3='改造' and Col4='pw-line' and LineInfo like '10@%'";
                //写入架空线路 长度
                double JKXlength = 0;
                if (Services.BaseService.GetObject("SelectTZGSSUMLength", linetiaojian) != null)
                {
                    JKXlength = (double)Services.BaseService.GetObject("SelectTZGSSUMLength", linetiaojian);
                }
                obj_sheet.SetValue(11, col, JKXlength);
                //写入架空线路 杆塔（基）
                double GTJlength = 0;
                if (Services.BaseService.GetObject("SelectTZGSSUMnum2", linetiaojian) != null)
                {
                    GTJlength = (double)Services.BaseService.GetObject("SelectTZGSSUMnum2", linetiaojian);
                }
                obj_sheet.SetValue(12, col, GTJlength);
                //写入电缆线路 长度
                double DLXlength = 0;
                if (Services.BaseService.GetObject("SelectTZGSSUMLength2", linetiaojian) != null)
                {
                    DLXlength = (double)Services.BaseService.GetObject("SelectTZGSSUMLength2", linetiaojian);
                }
                obj_sheet.SetValue(13, col, DLXlength);
                //写入电缆线路 沟道（km）
                double DLGDlength = 0;
                if (Services.BaseService.GetObject("SelectTZGSSUMnum6", linetiaojian) != null)
                {
                    DLGDlength = (double)Services.BaseService.GetObject("SelectTZGSSUMnum6", linetiaojian);
                }
                obj_sheet.SetValue(14, col, DLGDlength);
                //写入线路投资金额
                double LineTZJEsum = 0;
                if (Services.BaseService.GetObject("SelectTZGSSUMAmount", linetiaojian) != null)
                {
                    LineTZJEsum = (double)Services.BaseService.GetObject("SelectTZGSSUMAmount", linetiaojian);
                }
                obj_sheet.AddSpanCell(11, col + 1, 4, 1);
                obj_sheet.SetValue(11, col + 1, LineTZJEsum);
                //列加一
                col++;
            }

        }
        private void Sheet4_RefreshData(FarPoint.Win.Spread.SheetView obj_sheet)
        {
            //项目清册表4为固定格式表，其中有一部分是人工输入部分
            //更新数据时保留人工输入部分，只用此方法更新其它的数据部分
            //写入数据
            Sheet4_AddData(obj_sheet);
          
        }
        private void Sheet4_SetCellType(FarPoint.Win.Spread.SheetView obj_sheet)
        {
            //解开用户手工输入部分并设定格式
            for (int i = 3; i < 15; i++)
            {
                fc.Sheet_UnLockedandCellNumber(obj_sheet, 3, i+1);
                fc.Sheet_UnLockedandCellNumber(obj_sheet, 5, i + 1);
                fc.Sheet_UnLockedandCellNumber(obj_sheet, 15, i);
                fc.Sheet_UnLockedandCellNumber(obj_sheet, 16, i);
                fc.Sheet_UnLockedandCellNumber(obj_sheet, 15, i+1);
                i++;
            }
        }
        private void Sheet4_GetDatafrom83(FarPoint.Win.Spread.SheetView obj_sheet)
        {
            //表4中的高损配变和无功补偿装置数据（不包括投资金额）从中低压规划表的8-3的全地区合计获取
            if (File.Exists(System.Windows.Forms.Application.StartupPath + "\\xls\\ZDYGH.xls"))
            {
                try
                {
                    // 定义要使用的Excel 组件接口
                    // 定义Application 对象,此对象表示整个Excel 程序
                    Microsoft.Office.Interop.Excel.Application excelApp = null;
                    // 定义Workbook对象,此对象代表工作薄
                    Microsoft.Office.Interop.Excel.Workbook workBook;
                    // 定义Worksheet 对象,此对象表示Execel 中的一张工作表
                    Microsoft.Office.Interop.Excel.Worksheet ws = null;
                   // Microsoft.Office.Interop.Excel.Range range = null;
                    excelApp = new Microsoft.Office.Interop.Excel.Application();
                    string filename = System.Windows.Forms.Application.StartupPath + "\\xls\\ZDYGH.xls";
                    workBook = excelApp.Workbooks.Open(filename, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);
                    //找到表8-3
                    ws = (Microsoft.Office.Interop.Excel.Worksheet)workBook.Worksheets[4];
                    //取消保护工作表
                    ws.Unprotect(Missing.Value);
                    
                    //用来帮助两个表格之间列号不匹配做转换
                    int turn = 3;
                    for (int col = 6; col < 12;col++ )
                    {

                        double a = double.Parse(ws.get_Range(ws.Cells[27, col], ws.Cells[27, col]).Value2.ToString());
                        obj_sheet.SetValue(3, col - turn, a);
                        double b = double.Parse(ws.get_Range(ws.Cells[28, col], ws.Cells[28, col]).Value2.ToString());
                        obj_sheet.SetValue(4, col - turn, b);
                        double c = double.Parse(ws.get_Range(ws.Cells[29, col], ws.Cells[29, col]).Value2.ToString());
                        obj_sheet.SetValue(5, col - turn, c);
                        double d = double.Parse(ws.get_Range(ws.Cells[30, col], ws.Cells[30, col]).Value2.ToString());
                        obj_sheet.SetValue(6, col - turn, d);
                        turn--;
                    }
                    //保护工作表
                    ws.Protect(Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);

                    //保存工作簿
                    workBook.Save();
                    //关闭工作簿
                    excelApp.Workbooks.Close();
                    
                }
                catch (System.Exception ew)
                {
                	
                }
            }
        }
        private void barBtnClose_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }
        private void barBtnDaochuExcel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
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
                    //以下是打开文件设表格自动换行

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
        private void barBtnRefreshData_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            
          
            WaitDialogForm newwait = new WaitDialogForm("", "正在更新数据, 请稍候...");
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
                      
            //清除表格原有数据，这样写入速度快
            fpSpread1.Sheets[0].RowCount = 0;
            fpSpread1.Sheets[0].ColumnCount = 0;
            Build_Sheet11(fpSpread1.Sheets[0]);

            fpSpread1.Sheets[1].RowCount = 0;
            fpSpread1.Sheets[1].ColumnCount = 0;
            Build_Sheet12(fpSpread1.Sheets[1]);

            //2-1中有手写数据 固定项 所以只更新其它的数据部分
            Sheet21_RefrehData(fpSpread1.Sheets[2]);

            //2-2动态有手写一项
            //先保存手写部分
            Sheet22_SaveData(fpSpread1.Sheets[3]);
            //清空表
            fpSpread1.Sheets[3].RowCount = 0;
            fpSpread1.Sheets[3].ColumnCount = 0;
            //重建表
            Build_Sheet22(fpSpread1.Sheets[3]);
            //回写手工数据
            Sheet22_WirteData(fpSpread1.Sheets[3]);

            //2-3动态有手写两项
            Sheet23_SaveData(fpSpread1.Sheets[4]);
            fpSpread1.Sheets[4].RowCount = 0;
            fpSpread1.Sheets[4].ColumnCount = 0;
            Build_Sheet23(fpSpread1.Sheets[4]);
            Sheet23_WirteData(fpSpread1.Sheets[4]);
            //3有手写，固定项所以只更新其它的数据部分
            Sheet3_RefreshData(fpSpread1.Sheets[5]);
            //4有手写，固定项所以只更新其它的数据部分
            Sheet4_RefreshData(fpSpread1.Sheets[6]);
            Sheet4_GetDatafrom83(fpSpread1.Sheets[6]);

            //移除空表
            fpSpread1.Sheets.Remove(activesheet);
            //还原当前表
            fpSpread1.ActiveSheet = obj_sheet;

            //设文本自动换行
            fc.Sheet_Colautoenter(fpSpread1);
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
            try
            {
                //保存excel文件
                fpSpread1.SaveExcel(System.Windows.Forms.Application.StartupPath + "\\xls\\XMQC.xls");
                //以下是打开文件设表格自动换行

                // 定义要使用的Excel 组件接口
                // 定义Application 对象,此对象表示整个Excel 程序
                Microsoft.Office.Interop.Excel.Application excelApp = null;
                // 定义Workbook对象,此对象代表工作薄
                Microsoft.Office.Interop.Excel.Workbook workBook;
                // 定义Worksheet 对象,此对象表示Execel 中的一张工作表
                Microsoft.Office.Interop.Excel.Worksheet ws = null;
                Microsoft.Office.Interop.Excel.Range range = null;
                excelApp = new Microsoft.Office.Interop.Excel.Application();
                string filename = System.Windows.Forms.Application.StartupPath + "\\xls\\XMQC.xls";
                workBook = excelApp.Workbooks.Open(filename, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);
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
                    //保护工作表
                    ws.Protect(Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);

                }
                //保存工作簿
                workBook.Save();
                //关闭工作簿
                excelApp.Workbooks.Close();
                wait.Close();
                MsgBox.Show("保存成功");
            }
            catch (System.Exception ee)
            {
                wait.Close();
                MsgBox.Show("保存错误！确定您安装有Office Excel,或者关闭所有Excel文件重试");
            }
            

        }

      

     
       
    }
}