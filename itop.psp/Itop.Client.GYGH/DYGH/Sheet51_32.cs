using System;
using System.Collections.Generic;
using System.Text;
using Itop.Client.Common;
using System.Collections;
using Itop.Domain.Graphics;
namespace Itop.Client.GYGH.DYGH
{
    class Sheet51_32
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
        public void Build(FarPoint.Win.Spread.SheetView obj_sheet, string ProjID)
        {
            //计算市辖供电区条件 
            string SXtiaojianarea = " AreaID='" + ProjID + "' and (S10='水电' or S10='煤电') and S9!='' and CAST(S1 as int) between 10 and 110 and S3!='' and  S5='市辖供电区'  group by S9";
            //计算县级供电区条件
            string XJtiaojianarea = " AreaID='" + ProjID + "' and (S10='水电' or S10='煤电') and S9!='' and CAST(S1 as int) between 10 and 110 and S3!='' and  S5!='市辖供电区'  group by S9";
            //存放市辖供电区分区名
            IList<string> SXareaid_List = Services.BaseService.GetList<string>("SelectPSP_PowerSubstation_Info_GroupS9", SXtiaojianarea);
            //存放县级供电区分区名
            IList<string> XJareaid_List = Services.BaseService.GetList<string>("SelectPSP_PowerSubstation_Info_GroupS9", XJtiaojianarea);

            //计算市辖电区电厂个数条件 
            string SXtiaojian = " AreaID='" + ProjID + "' and (S10='水电' or S10='煤电') and S9!='' and CAST(S1 as int) between 10 and 110 and S3!='' and  S5='市辖供电区'";
            //计算县级供电区电厂个数条件
            string XJtiaojian = " AreaID='" + ProjID + "' and (S10='水电' or S10='煤电') and S9!='' and CAST(S1 as int) between 10 and 110 and S3!='' and  S5!='市辖供电区'";
            //市辖供电区电厂个数
            int SXsum = (int)Services.BaseService.GetObject("SelectPSP_PowerSubstation_InfoCountByObject", SXtiaojian);
            //县级供电区电厂个数
            int XJsum = (int)Services.BaseService.GetObject("SelectPSP_PowerSubstation_InfoCountByObject", XJtiaojian);
            //表标题行数
            int startrow = 3;

            //表格共10 行12 列
            rowcount = startrow+SXsum+XJsum;
            colcount = 12;
            //工作表第一行的标题
            title = "附表32  铜陵市接入110kV及以下配电网的电源规划方案";
            //工作表名
            sheetname = "表5-1 附表32";
            //设定工作表行列值及标题和表名
            fc.Sheet_RowCol_Title_Name(obj_sheet, rowcount, colcount, title, sheetname);
            //设定表格线
            fc.Sheet_GridandCenter(obj_sheet);
            //设定行列模式，以便写公式使用
            fc.Sheet_Referen_R1C1(obj_sheet);
            //设定表格列宽度
            obj_sheet.Columns[0].Width = 100;
            obj_sheet.Columns[1].Width = 110;
            obj_sheet.Columns[2].Width = 150;
            obj_sheet.Columns[3].Width = 60;
            obj_sheet.Columns[4].Width = 60;
            obj_sheet.Columns[5].Width = 60;
            obj_sheet.Columns[6].Width = 60;
            obj_sheet.Columns[7].Width = 60;
            obj_sheet.Columns[8].Width = 60;
            obj_sheet.Columns[9].Width = 60;
            obj_sheet.Columns[10].Width = 60;
            obj_sheet.Columns[11].Width = 60;
            //设定表格行高度
            obj_sheet.Rows[1].Height = 20;
            obj_sheet.Rows[2].Height = 20;
            //写标题行列内容
            Sheet_AddItem(obj_sheet, SXareaid_List, XJareaid_List);
            //写入数据
            Sheet_AddData(obj_sheet, ProjID, SXareaid_List, XJareaid_List);

            //锁定表格
            fc.Sheet_Locked(obj_sheet);

        }
        private void Sheet_AddItem(FarPoint.Win.Spread.SheetView obj_sheet, IList<string> SXareaid_List, IList<string> XJareaid_List)
        {
           //2行标题内容
            obj_sheet.AddSpanCell(1,0,2,1);
            obj_sheet.SetValue(1,0,"分区类型");
            obj_sheet.AddSpanCell(1,1,2,1);
            obj_sheet.SetValue(1,1,"分区名称");
            obj_sheet.AddSpanCell(1,2,2,1);
            obj_sheet.SetValue(1,2,"电厂名称");
            obj_sheet.AddSpanCell(1,3,2,1);
            obj_sheet.SetValue(1,3,"电厂类型");
            obj_sheet.AddSpanCell(1,4,2,1);
            obj_sheet.SetValue(1,4,"电压等级(kV)");
            obj_sheet.AddSpanCell(1,5,1,7);
            obj_sheet.SetValue(1,5,"装机容量(MW)");

            //3行标题内容
            obj_sheet.SetValue(2,5,"2010年");
            obj_sheet.SetValue(2,6,"2011年");
            obj_sheet.SetValue(2,7,"2012年");
            obj_sheet.SetValue(2,8,"2013年");
            obj_sheet.SetValue(2,9,"2014年");
            obj_sheet.SetValue(2,10,"2015年");
            obj_sheet.SetValue(2,11,"2020年");
        }
        private void Sheet_AddData(FarPoint.Win.Spread.SheetView obj_sheet, string ProjID, IList<string> SXareaid_List, IList<string> XJareaid_List)
        {
            int startrow = 3;
            string tiaojian = "";
            string areaname = "";
            int xjrow = 0;
            int nowrow=startrow;
            for (int i = 0; i < SXareaid_List.Count; i++)
            {
                areaname = SXareaid_List[i].ToString();
                tiaojian = " AreaID='" + ProjID + "' and (S10='水电' or S10='煤电')  and CAST(S1 as int) between 10 and 110 and S3!='' and  S5='市辖供电区' and  S9='" + areaname + "'";
                IList<PSP_PowerSubstation_Info> GDClist = Services.BaseService.GetList<PSP_PowerSubstation_Info>("SelectPSP_PowerSubstation_InfoListByWhere", tiaojian);
                obj_sheet.AddSpanCell(nowrow, 1, GDClist.Count, 1);
                obj_sheet.SetValue(nowrow, 1, areaname);
                for (int j = 0; j < GDClist.Count; j++)
                {
                    //电厂名称
                    obj_sheet.SetValue(nowrow,2,GDClist[j].Title);
                    //电厂类型
                    obj_sheet.SetValue(nowrow, 3, GDClist[j].S10);
                    //并网电压等级(kV)
                    obj_sheet.SetValue(nowrow, 4, GDClist[j].S1);
                    int year=0;
                    if (GDClist[j].S3!="")
	                {
                        year=int.Parse(GDClist[j].S3);
	                }
                    if (year!=0)
	                {
                        if (year<=2010)
	                    {
                            double ZJRL = 0;
                            if (GDClist[j].S2 != "")
                            {
                                ZJRL = double.Parse(GDClist[j].S2);
                            }
                            obj_sheet.SetValue(nowrow, 5, ZJRL);
	                    }
                        else if(2016<=year&&year<=2020)
	                    {
                            double ZJRL = 0;
                            if (GDClist[j].S2 != "")
                            {
                                ZJRL = double.Parse(GDClist[j].S2);
                            }
                            obj_sheet.SetValue(nowrow, 11, ZJRL);
	                    }
                        else
	                    {
                             double ZJRL = 0;
                            if (GDClist[j].S2 != "")
                            {
                                ZJRL = double.Parse(GDClist[j].S2);
                            }
                            obj_sheet.SetValue(nowrow, year-2005, ZJRL);
	                    }
	                }
                    nowrow += 1;
                    
                }
                
            }
            if (SXareaid_List.Count>0)
            {
                obj_sheet.AddSpanCell(startrow, 0, nowrow - startrow, 1);
                obj_sheet.SetValue(startrow, 0, "市辖供电区");
            }
            xjrow = nowrow; ;
            for (int i = 0; i < XJareaid_List.Count; i++)
            {
                areaname = XJareaid_List[i].ToString();
                tiaojian = " AreaID='" + ProjID + "' and (S10='水电' or S10='煤电')  and CAST(S1 as int) between 10 and 110 and S3!='' and  S5!='市辖供电区' and  S9='" + areaname + "'";
                IList<PSP_PowerSubstation_Info> GDClist = Services.BaseService.GetList<PSP_PowerSubstation_Info>("SelectPSP_PowerSubstation_InfoListByWhere", tiaojian);
                obj_sheet.AddSpanCell(nowrow, 1, GDClist.Count, 1);
                obj_sheet.SetValue(nowrow, 1, areaname);
                for (int j = 0; j < GDClist.Count; j++)
                {
                    //电厂名称
                    obj_sheet.SetValue(nowrow,2,GDClist[j].Title);
                    //电厂类型
                    obj_sheet.SetValue(nowrow, 3, GDClist[j].S10);
                    //并网电压等级(kV)
                    obj_sheet.SetValue(nowrow, 4, GDClist[j].S1);
                    int year=0;
                    if (GDClist[j].S3!="")
	                {
                        year=int.Parse(GDClist[j].S3);
	                }
                    if (year!=0)
	                {
                        if (year<=2010)
	                    {
                            double ZJRL = 0;
                            if (GDClist[j].S2 != "")
                            {
                                ZJRL = double.Parse(GDClist[j].S2);
                            }
                            obj_sheet.SetValue(nowrow, 5, ZJRL);
	                    }
                        else if(2016<=year&&year<=2020)
	                    {
                            double ZJRL = 0;
                            if (GDClist[j].S2 != "")
                            {
                                ZJRL = double.Parse(GDClist[j].S2);
                            }
                            obj_sheet.SetValue(nowrow, 11, ZJRL);
	                    }
                        else
	                    {
                             double ZJRL = 0;
                            if (GDClist[j].S2 != "")
                            {
                                ZJRL = double.Parse(GDClist[j].S2);
                            }
                            obj_sheet.SetValue(nowrow, year-2005, ZJRL);
	                    }
	                }
                    nowrow += 1;
                    
                }
                
            }
            if (XJareaid_List.Count>0)
            {
                obj_sheet.AddSpanCell(xjrow, 0, nowrow - xjrow , 1);
                obj_sheet.SetValue(xjrow, 0, "县级供电区");
            }
        }
    }
}
