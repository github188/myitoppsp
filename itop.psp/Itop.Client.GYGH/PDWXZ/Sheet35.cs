using System;
using System.Collections.Generic;
using System.Text;
using Itop.Client.Common;
namespace Itop.Client.GYGH.PDWXZ
{
    class Sheet35
    {

        fpcommon fc = new fpcommon();
        //工作表行数
        int rowcount = 0;
        //工作表列数据
        int colcount = 0;
        //工作表第一行的表名
        string title = "";
        //工作表标签名
        string sheetname = "";
        public void Build(FarPoint.Win.Spread.SheetView obj_sheet,int year,string ProjID,List<string[]> SxXjName)
        {
            //表格共9 行6 列
            rowcount = 9;
            colcount = 6;
            //工作表第一行的标题
            title = "表3‑5  " + year + "年铜陵市220kV或330kV电网规模";
            //工作表名
            sheetname = "表3-5";
            //设定工作表行列值及标题和表名
            fc.Sheet_RowCol_Title_Name(obj_sheet, rowcount, colcount, title, sheetname);
            //设定表格线
            fc.Sheet_GridandCenter(obj_sheet);
            //设定行列模式，以便写公式使用
            fc.Sheet_Referen_R1C1(obj_sheet);
            //设定表格列宽度
            obj_sheet.Columns[0].Width = 60;
            obj_sheet.Columns[1].Width = 110;
            obj_sheet.Columns[2].Width = 80;
            obj_sheet.Columns[3].Width = 80;
            obj_sheet.Columns[4].Width = 80;
            obj_sheet.Columns[5].Width = 80;

            //添加行列标题
            Sheet_AddItem(obj_sheet);
            //添加数据
            Sheet_AddData(obj_sheet,year,ProjID);
            //写求和公式
            fc.Sheet_WriteFormula_RowSum(obj_sheet, 6, 2, 2, 1, 8, 2, 4);

            //锁定表格
            fc.Sheet_Locked(obj_sheet);
        }
        private void Sheet_AddItem(FarPoint.Win.Spread.SheetView obj_sheet)
        {
            //写标题行内容
            //2行标题内容
            obj_sheet.AddSpanCell(1, 0, 2, 1);
            obj_sheet.SetValue(1, 0, "分类");
            obj_sheet.AddSpanCell(1, 1, 2, 1);
            obj_sheet.SetValue(1, 1, "项目");
            obj_sheet.AddSpanCell(1, 2, 1, 2);
            obj_sheet.SetValue(1, 2, "330kV");
            obj_sheet.AddSpanCell(1, 4, 1, 2);
            obj_sheet.SetValue(1, 4, "220kV");

            //3行标题内容
            obj_sheet.SetValue(2, 2, "公用");
            obj_sheet.SetValue(2, 3, "专用");
            obj_sheet.SetValue(2, 4, "公用");
            obj_sheet.SetValue(2, 5, "专用");
            //写标题列内容

            //1列标题内容
            obj_sheet.AddSpanCell(3, 0, 3, 1);
            obj_sheet.SetValue(3, 0, "变电");
            obj_sheet.AddSpanCell(6, 0, 3, 1);
            obj_sheet.SetValue(6, 0, "线路");

            //2列标题内容
            obj_sheet.SetValue(3, 1, "变电站座数（座）");
            obj_sheet.SetValue(4, 1, "主变台数（台）");
            obj_sheet.SetValue(5, 1, "变电容量（MVA）");
            obj_sheet.SetValue(6, 1, "架空（km）");
            obj_sheet.SetValue(7, 1, "电缆（km）");
            obj_sheet.SetValue(8, 1, "合计");
        }
        private void Sheet_AddData(FarPoint.Win.Spread.SheetView obj_sheet, int year, string ProjID)
        {
            //对于变电站信息，S2中存放的投产年份，有些包含了其他信息，所要不为空并截取前四位
            //S4存放的公用专用标识，L1存放电压，为int类型
            //对于线路信息，OperationYear中存放的投产年份, Type='05'表示线路信息
            //LineType2存放的公用专用标识,RateVolt存放电压,为float类型

            string BDtiaojian = "";
            string XLtiaojian = "";
            //公用专用标识
            string strgz = "";
            //电压标识
            string dianya = "";
            for (int col = 2; col <= 5; col++)
            {
                if (col == 2 || col == 4)
                {
                    strgz = "公用";
                }
                else
                {
                    strgz = "专用";
                }
                if (col == 2 || col == 3)
                {
                    dianya = "330";
                }
                else
                {
                    dianya = "220";
                }
                BDtiaojian = " S2!='' and CAST(substring(S2,1,4) as int)<=" + year + " and AreaID='" + ProjID + "' and S4='" + strgz + "' and L1=" + dianya;
                XLtiaojian = " year(cast(OperationYear as datetime))<=" + year + " and  Type='05' and ProjectID='" + ProjID + "' and LineType2='" + strgz + "'and RateVolt=" + dianya;
                           
                //变电站台数
                int BDsum = 0;
                if (Services.BaseService.GetObject("SelectPSP_Substation_InfoCountall", BDtiaojian) != null)
                {
                    BDsum = (int)Services.BaseService.GetObject("SelectPSP_Substation_InfoCountall", BDtiaojian);
                }
                obj_sheet.SetValue(3, col, BDsum);
                //主变台数（台）
                int ZBsum = 0;
                if (Services.BaseService.GetObject("SelectPSP_Substation_InfoSUML3", BDtiaojian) != null)
                {
                    ZBsum = (int)Services.BaseService.GetObject("SelectPSP_Substation_InfoSUML3", BDtiaojian);
                }
                obj_sheet.SetValue(4, col, ZBsum);
                //变电容量（MVA）
                double BDRLsum = 0;
                if (Services.BaseService.GetObject("SelectPSP_Substation_InfoSUML2", BDtiaojian) != null)
                {
                    BDRLsum = (double)Services.BaseService.GetObject("SelectPSP_Substation_InfoSUML2", BDtiaojian);
                }
                obj_sheet.SetValue(5, col, BDRLsum);

                //架空线长
                double JKXlength = 0;
                if (Services.BaseService.GetObject("SelectPSPDEV_SUMLineLength", XLtiaojian) != null)
                {
                    JKXlength = (double)Services.BaseService.GetObject("SelectPSPDEV_SUMLineLength", XLtiaojian);
                }
                obj_sheet.SetValue(6, col, JKXlength);

                //电缆线长
                double DLXlength = 0;
                if (Services.BaseService.GetObject("SelectPSPDEV_SUMLength2", XLtiaojian) != null)
                {
                    DLXlength = (double)Services.BaseService.GetObject("SelectPSPDEV_SUMLength2", XLtiaojian);
                }
                obj_sheet.SetValue(7, col, DLXlength);

            }
        }

    }
}
