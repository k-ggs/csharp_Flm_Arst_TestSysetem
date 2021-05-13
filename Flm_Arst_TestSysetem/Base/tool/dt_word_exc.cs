using Flm_Arst_TestSysetem.Model;
using Microsoft.Office.Core;
using Microsoft.Office.Interop.Word;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DataTable = System.Data.DataTable;

namespace Flm_Arst_TestSysetem.Base.tool
{
    public static class dt_word_exc
    {
        public static void ExportWord(DataTable dg)
        {
            try
            {
                if (dg.Rows.Count != 0)
                {
                    //建表格
                    object Nothing = System.Reflection.Missing.Value;
                    Microsoft.Office.Interop.Word._Application oWord;
                    Microsoft.Office.Interop.Word._Document oDoc;
                    oWord = new Microsoft.Office.Interop.Word.Application();
                    oWord.Visible = true;
                    oDoc = oWord.Documents.Add(ref Nothing, ref Nothing, ref Nothing, ref Nothing);
                    object start = 0;
                    object end = 0;
                    Microsoft.Office.Interop.Word.Range tableLocation = oDoc.Range(ref start, ref end);
                    oDoc.Tables.Add(tableLocation, dg.Rows.Count + 1, dg.Columns.Count, ref Nothing, ref Nothing);
                    Microsoft.Office.Interop.Word.Table newTable = oDoc.Tables[1];
                    newTable.Borders.OutsideLineStyle = Microsoft.Office.Interop.Word.WdLineStyle.wdLineStyleSingle;
                    newTable.Borders.InsideLineStyle = Microsoft.Office.Interop.Word.WdLineStyle.wdLineStyleSingle;
                    object beforeRow = newTable.Rows[1];
                    newTable.Rows.Add(ref beforeRow);

                    //赋值
                    for (int i = 0; i < dg.Columns.Count; i++)
                    {
                        newTable.Cell(1, i + 1).Range.Text = dg.Columns[i].ColumnName;
                    }
                    for (int i = 0; i < dg.Rows.Count; i++)
                    {
                        for (int j = 0; j < dg.Columns.Count; j++)
                        {
                            newTable.Cell(i + 2, j + 1).Range.Text = dg.Rows[i][j].ToString();
                        }
                    }

                    object fileName = "";
                    oDoc.SaveAs(fileName, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing,
                         ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing);
                    oWord.Documents.Open(fileName, Nothing, false, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing);
                }
            }
            catch
            { }
        }
        public static void ResversExportWord(DataTable dg)
        {
            try
            {
                if (dg.Rows.Count != 0)
                {
                    //建表格
                    object Nothing = System.Reflection.Missing.Value;
                    Microsoft.Office.Interop.Word._Application oWord;
                    Microsoft.Office.Interop.Word._Document oDoc;
                    oWord = new Microsoft.Office.Interop.Word.Application();
                    oWord.Visible = true;
                    oDoc = oWord.Documents.Add(ref Nothing, ref Nothing, ref Nothing, ref Nothing);
                    object start = 0;
                    object end = 0;
                    Microsoft.Office.Interop.Word.Range tableLocation = oDoc.Range(ref start, ref end);
                    oDoc.Tables.Add(tableLocation, dg.Rows.Count + 1, dg.Columns.Count, ref Nothing, ref Nothing);
                    Microsoft.Office.Interop.Word.Table newTable = oDoc.Tables[1];
                    newTable.Borders.OutsideLineStyle = Microsoft.Office.Interop.Word.WdLineStyle.wdLineStyleSingle;
                    newTable.Borders.InsideLineStyle = Microsoft.Office.Interop.Word.WdLineStyle.wdLineStyleSingle;
                    object beforeRow = newTable.Rows[1];
                    newTable.Rows.Add(ref beforeRow);
                    for (int i = 0; i < 5; i++)
                    {
                        newTable.Cell(i,1).Range.Text = dg.Columns[i].ColumnName;
                        newTable.Cell(i , 2).Range.Text = dg.Rows[0][i].ToString();

                    }
                        //赋值
                        for (int i = 5; i < dg.Columns.Count; i++)
                    {
                        newTable.Cell(5, i + 1-5).Range.Text = dg.Columns[i].ColumnName;
                    }
                    for (int i = 0; i < dg.Rows.Count; i++)
                    {
                        for (int j = 5; j < dg.Columns.Count; j++)
                        {
                            newTable.Cell(i + 2, j + 1-5).Range.Text = dg.Rows[i][j].ToString();
                        }
                    }






                    object fileName = "";
                    oDoc.SaveAs(fileName, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing,
                         ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing);
                    oWord.Documents.Open(fileName, Nothing, false, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing);
                }
            }
            catch
            { }
        }
        /// <summary>
        /// 导出datatable到Excel
        /// </summary>
        /// <param name="dg">需要中文列名的表</param>
        public static void ExportExcel(DataTable dg)
            {
                try
                {
                    if (dg.Rows.Count != 0)
                    {
                        Microsoft.Office.Interop.Excel.Application app = new Microsoft.Office.Interop.Excel.Application();
                        //Microsoft.Office.Interop.Excel.ApplicationClass ac = new Microsoft.Office.Interop.Excel.ApplicationClass();

                        Microsoft.Office.Interop.Excel.Workbook wb; //这里千万不能使用 new 来实例对象，不然会异常

                        Microsoft.Office.Interop.Excel.Worksheet ws;




                        wb = app.Workbooks.Add(System.Reflection.Missing.Value);  //创建工作簿（WorkBook：即Excel文件主体本身）
                        ws = (Microsoft.Office.Interop.Excel.Worksheet)wb.Worksheets.Add(System.Reflection.Missing.Value, System.Reflection.Missing.Value, System.Reflection.Missing.Value, System.Reflection.Missing.Value);        //创建工作表（Worksheet：工作表，即Excel里的子表sheet）

                        //设置表名
                        ws.Name = dg.TableName == "" ? "Table" : dg.TableName;
                        //赋值
                        for (int i = 0; i < dg.Columns.Count; i++)
                        {
                            ws.Cells[1, i + 1] = dg.Columns[i].ColumnName;
                        }
                        //将数据导入到工作表的单元格
                        for (int i = 0; i < dg.Rows.Count; i++)
                        {
                            for (int j = 0; j < dg.Columns.Count; j++)
                                ws.Cells[i + 2, j + 1] = dg.Rows[i][j].ToString();
                        }
                        string strPath = Environment.CurrentDirectory;
                        Microsoft.Win32.SaveFileDialog dialogOpenFile = new Microsoft.Win32.SaveFileDialog();
                        dialogOpenFile.DefaultExt = "xls";//默认扩展名
                        dialogOpenFile.AddExtension = true;//是否自动添加扩展名
                        dialogOpenFile.Filter = "*.xls|.xls";
                        dialogOpenFile.OverwritePrompt = true;//文件已存在是否提示覆盖
                        dialogOpenFile.FileName = ws.Name;//默认文件名
                        dialogOpenFile.CheckPathExists = true;//提示输入的文件名无效
                        dialogOpenFile.Title = "保存EXCEL";
                        //显示对话框
                        bool? b = dialogOpenFile.ShowDialog();
                        if (b == true)//点击保存
                        {
                            //保存到文件
                            wb.SaveAs(dialogOpenFile.FileName, System.Reflection.Missing.Value, System.Reflection.Missing.Value, System.Reflection.Missing.Value, System.Reflection.Missing.Value, System.Reflection.Missing.Value, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange, System.Reflection.Missing.Value, System.Reflection.Missing.Value, System.Reflection.Missing.Value, System.Reflection.Missing.Value, System.Reflection.Missing.Value);
                        }

                        //恢复系统路径-涉及不到的可以去掉
                        Environment.CurrentDirectory = strPath;

                        //关闭
                        wb.Close(System.Reflection.Missing.Value, System.Reflection.Missing.Value, System.Reflection.Missing.Value);

                    }
                }
                catch
                { }

            }



        public static void ExportExcel2(DataTable dg)
        {
                         
            Microsoft.Office.Interop.Word._Application wordApp;                   //Word应用程序变量 
             Microsoft.Office.Interop.Word._Document wordDoc;                  //Word文档变量

          
            wordApp = new Microsoft.Office.Interop.Word.Application(); //初始化

            wordApp.Visible = true;//使文档可见

          

            //由于使用的是COM库，因此有许多变量需要用Missing.Value代替
            Object Nothing = Missing.Value;
            wordDoc = wordApp.Documents.Add(ref Nothing, ref Nothing, ref Nothing, ref Nothing);

            #region 页面设置、页眉图片和文字设置，最后跳出页眉设置

            //页面设置
            wordDoc.PageSetup.PaperSize = Microsoft.Office.Interop.Word.WdPaperSize.wdPaperA4;//设置纸张样式为A4纸
            wordDoc.PageSetup.Orientation = Microsoft.Office.Interop.Word.WdOrientation.wdOrientPortrait;//排列方式为垂直方向
            wordDoc.PageSetup.TopMargin = 57.0f;
            wordDoc.PageSetup.BottomMargin = 57.0f;
            wordDoc.PageSetup.LeftMargin = 57.0f;
            wordDoc.PageSetup.RightMargin = 57.0f;
            wordDoc.PageSetup.HeaderDistance = 30.0f;//页眉位置

            //设置页眉
            wordApp.ActiveWindow.View.Type = Microsoft.Office.Interop.Word.WdViewType.wdPrintView;//普通视图（即页面视图）样式
            wordApp.ActiveWindow.View.SeekView = Microsoft.Office.Interop.Word.WdSeekView.wdSeekPrimaryHeader;//进入页眉设置，其中页眉边距在页面设置中已完成
            wordApp.Selection.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphRight;//页眉中的文字右对齐


          

            //去掉页眉的横线
            wordApp.ActiveWindow.ActivePane.Selection.ParagraphFormat.Borders[Microsoft.Office.Interop.Word.WdBorderType.wdBorderBottom].LineStyle = Microsoft.Office.Interop.Word.WdLineStyle.wdLineStyleNone;
            wordApp.ActiveWindow.ActivePane.Selection.Borders[Microsoft.Office.Interop.Word.WdBorderType.wdBorderBottom].Visible = false;
            wordApp.ActiveWindow.ActivePane.View.SeekView = Microsoft.Office.Interop.Word.WdSeekView.wdSeekMainDocument;//退出页眉设置
            #endregion

            #region 页码设置并添加页码

            //为当前页添加页码
            Microsoft.Office.Interop.Word.PageNumbers pns = wordApp.Selection.Sections[1].Headers[Microsoft.Office.Interop.Word.WdHeaderFooterIndex.wdHeaderFooterEvenPages].PageNumbers;//获取当前页的号码
            pns.NumberStyle = Microsoft.Office.Interop.Word.WdPageNumberStyle.wdPageNumberStyleNumberInDash;//设置页码的风格，是Dash形还是圆形的
            pns.HeadingLevelForChapter = 0;
            pns.IncludeChapterNumber = false;
            pns.RestartNumberingAtSection = false;
            pns.StartingNumber = 0; //开始页页码？
            object pagenmbetal = Microsoft.Office.Interop.Word.WdPageNumberAlignment.wdAlignPageNumberCenter;//将号码设置在中间
            object first = true;
            wordApp.Selection.Sections[1].Footers[Microsoft.Office.Interop.Word.WdHeaderFooterIndex.wdHeaderFooterEvenPages].PageNumbers.Add(ref pagenmbetal, ref first);

            #endregion

            #region 行间距与缩进、文本字体、字号、加粗、斜体、颜色、下划线、下划线颜色设置

            wordApp.Selection.ParagraphFormat.LineSpacing = 16f;//设置文档的行间距
            wordApp.Selection.ParagraphFormat.FirstLineIndent = 30;//首行缩进的长度
            object unite = Microsoft.Office.Interop.Word.WdUnits.wdStory;
            //赋值 


            wordApp.Selection.EndKey(ref unite, ref Nothing);//将光标移到文本末尾
           
            wordDoc.Paragraphs.Last.Range.Font.Bold = 1;
            wordDoc.Paragraphs.Last.Range.Font.Size = 20;
            wordDoc.Paragraphs.Last.Range.Font.Name = "宋体";
            wordDoc.Paragraphs.Last.Range.Text ="阻 火 器 检 测 报 告\n\n";
            wordApp.Selection.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
            for (int i = 0; i < 4; i++)
            {
                //写入文本
              
                wordApp.Selection.EndKey(ref unite, ref Nothing);//将光标移到文本末尾
                wordApp.Selection.ParagraphFormat.FirstLineIndent = 0;//取消首行缩进的长度
                wordDoc.Paragraphs.Last.Range.Font.Bold = 1;
                wordDoc.Paragraphs.Last.Range.Font.Size = 15;
                wordDoc.Paragraphs.Last.Range.Font.Name = "宋体";
                wordDoc.Paragraphs.Last.Range.Text =  dg.Columns[i].ColumnName+":  ";
              
               
                wordDoc.Paragraphs.Last.Range.Font.Bold = 0;
                wordDoc.Paragraphs.Last.Range.Font.Size = 10;
                wordDoc.Paragraphs.Last.Range.Font.Name = "宋体";
                wordDoc.Paragraphs.Last.Range.InsertAfter(dg.Rows[0][i].ToString()+"\n");
                wordApp.Selection.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphLeft;
            }


         
            #endregion


        

            #region 添加表格、填充数据、设置表格行列宽高、合并单元格、添加表头斜线、给单元格添加图片
            wordDoc.Content.InsertAfter("\n");//这一句与下一句的顺序不能颠倒，原因还没搞透
            wordApp.Selection.EndKey(ref unite, ref Nothing); //将光标移动到文档末尾
            wordApp.Selection.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphLeft;
            //object WdLine2 = MSWord.WdUnits.wdLine;//换一行;  
            //wordApp.Selection.MoveDown(ref WdLine2, 6, ref Nothing);//向下跨15行输入表格，这样表格就在文字下方了，不过这是非主流的方法

            //设置表格的行数和列数
            int tableRow = dg.Rows.Count + 1;
            int tableColumn = dg.Columns.Count-4;

            //定义一个Word中的表格对象
            Microsoft.Office.Interop.Word.Table table = wordDoc.Tables.Add(wordApp.Selection.Range,
            tableRow, tableColumn, ref Nothing, ref Nothing);

        //    #region
        //    wordDoc.Content.InsertAfter("\n");//这一句与下一句的顺序不能颠倒，原因还没搞透
        //    wordApp.Selection.EndKey(ref unite, ref Nothing); //将光标移动到文档末尾
        //    wordApp.Selection.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphLeft;
        //    Microsoft.Office.Interop.Word.Table table2 = wordDoc.Tables.Add(wordApp.Selection.Range,
        //tableRow, tableColumn, ref Nothing, ref Nothing);
        //    #endregion
            tableColumn = dg.Columns.Count;
            //默认创建的表格没有边框，这里修改其属性，使得创建的表格带有边框 
            table.Borders.Enable = 1;//这个值可以设置得很大，例如5、13等等

            //表格的索引是从1开始的。
            wordDoc.Tables[1].Cell(1, 1).Range.Text = "列\n行";
            for (int i = 1; i < tableRow; i++)
            {
                for (int j = 4; j < tableColumn-1; j++)
                {
                    if (i == 1)
                    {
                        table.Cell(1, j + 2-4).Range.Text = dg.Columns[j+1].ColumnName;//填充每列的标题
                    }
                    if (j == 5)
                    {
                        table.Cell(i + 1, 1).Range.Text = dg.Rows[i-1][4].ToString(); //填充每行的标题
                    }
                    table.Cell(i + 1, j + 2-4).Range.Text = dg.Rows[i-1][j+1].ToString();  //填充表格的各个小格子
                }
            }

            table.Cell(1, 2+1).Range.Text += "(m/s)";
            table.Cell(1, 3 + 1).Range.Text += "(m/s)";
            table.Cell(1, 4 + 1).Range.Text += "(m/s)";
            table.Cell(1, 5 + 1).Range.Text += "(m/s)";
            table.Cell(1, 6 + 1).Range.Text += "(m/s)";
            table.Cell(1, 7 + 1).Range.Text += "(m/s)";
            table.Cell(1, 8 + 1).Range.Text += "(m/s)";
            table.Cell(1, 9 + 1).Range.Text += "(KPa)";
            table.Cell(1, 10 + 1).Range.Text += "(KPa)";



            //设置table样式
            table.Rows.HeightRule = Microsoft.Office.Interop.Word.WdRowHeightRule.wdRowHeightAtLeast;//高度规则是：行高有最低值下限？
            table.Rows.Height = wordApp.CentimetersToPoints(float.Parse("0.8"));// 

            table.Range.Font.Size = 8F;
            table.Range.Font.Bold = 0;

            table.Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;//表格文本居中
            table.Range.Cells.VerticalAlignment = Microsoft.Office.Interop.Word.WdCellVerticalAlignment.wdCellAlignVerticalBottom;//文本垂直贴到底部
            //设置table边框样式
            table.Borders.OutsideLineStyle = Microsoft.Office.Interop.Word.WdLineStyle.wdLineStyleDouble;//表格外框是双线
            table.Borders.InsideLineStyle = Microsoft.Office.Interop.Word.WdLineStyle.wdLineStyleSingle;//表格内框是单线

            table.Rows[1].Range.Font.Bold = 1;//加粗
            table.Rows[1].Range.Font.Size = 10F;
            table.Cell(1, 1).Range.Font.Size = 10.5F;
            wordApp.Selection.Cells.Height = 30;//所有单元格的高度

            //除第一行外，其他行的行高都设置为20
            for (int i = 2; i <= tableRow; i++)
            {
                table.Rows[i].Height = 20;
            }

            //将表格左上角的单元格里的文字（“行” 和 “列”）居右
            table.Cell(1, 1).Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphRight;
            //将表格左上角的单元格里面下面的“列”字移到左边，相比上一行就是将ParagraphFormat改成了Paragraphs[2].Format
            table.Cell(1, 1).Range.Paragraphs[2].Format.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphLeft;

            table.Columns[1].Width = 25;//将第 1列宽度设置为50

            //将其他列的宽度都设置为75
            for (int i = 2; i <= tableColumn-6; i++)
            {
                table.Columns[i].Width = 45;
            }


            //添加表头斜线,并设置表头的样式
            table.Cell(1, 1).Borders[Microsoft.Office.Interop.Word.WdBorderType.wdBorderDiagonalDown].Visible = true;
            table.Cell(1, 1).Borders[Microsoft.Office.Interop.Word.WdBorderType.wdBorderDiagonalDown].Color = Microsoft.Office.Interop.Word.WdColor.wdColorBlack;
            table.Cell(1, 1).Borders[Microsoft.Office.Interop.Word.WdBorderType.wdBorderDiagonalDown].LineWidth = Microsoft.Office.Interop.Word.WdLineWidth.wdLineWidth150pt;

        

            #endregion

            wordApp.Selection.EndKey(ref unite, ref Nothing); //将光标移动到文档末尾

            wordDoc.Content.InsertAfter("\n");
          

         //   object fileName = "";
         //   wordDoc.SaveAs(fileName, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing,
          //       ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing);
         //   wordApp.Documents.Open(fileName, Nothing, false, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing);


        }


        public static void ExportExcel_multi(DataTable dg)
        {

            Microsoft.Office.Interop.Word._Application wordApp;                   //Word应用程序变量 
            Microsoft.Office.Interop.Word._Document wordDoc;                  //Word文档变量


            wordApp = new Microsoft.Office.Interop.Word.Application(); //初始化

            wordApp.Visible = true;//使文档可见



            //由于使用的是COM库，因此有许多变量需要用Missing.Value代替
            Object Nothing = Missing.Value;
            wordDoc = wordApp.Documents.Add(ref Nothing, ref Nothing, ref Nothing, ref Nothing);

            #region 页面设置、页眉图片和文字设置，最后跳出页眉设置

            //页面设置
            wordDoc.PageSetup.PaperSize = Microsoft.Office.Interop.Word.WdPaperSize.wdPaperA4;//设置纸张样式为A4纸
            wordDoc.PageSetup.Orientation = Microsoft.Office.Interop.Word.WdOrientation.wdOrientPortrait;//排列方式为垂直方向
            wordDoc.PageSetup.TopMargin = 57.0f;
            wordDoc.PageSetup.BottomMargin = 57.0f;
            wordDoc.PageSetup.LeftMargin = 57.0f;
            wordDoc.PageSetup.RightMargin = 57.0f;
            wordDoc.PageSetup.HeaderDistance = 30.0f;//页眉位置

            //设置页眉
            wordApp.ActiveWindow.View.Type = Microsoft.Office.Interop.Word.WdViewType.wdPrintView;//普通视图（即页面视图）样式
            wordApp.ActiveWindow.View.SeekView = Microsoft.Office.Interop.Word.WdSeekView.wdSeekPrimaryHeader;//进入页眉设置，其中页眉边距在页面设置中已完成
            wordApp.Selection.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphRight;//页眉中的文字右对齐




            //去掉页眉的横线
            wordApp.ActiveWindow.ActivePane.Selection.ParagraphFormat.Borders[Microsoft.Office.Interop.Word.WdBorderType.wdBorderBottom].LineStyle = Microsoft.Office.Interop.Word.WdLineStyle.wdLineStyleNone;
            wordApp.ActiveWindow.ActivePane.Selection.Borders[Microsoft.Office.Interop.Word.WdBorderType.wdBorderBottom].Visible = false;
            wordApp.ActiveWindow.ActivePane.View.SeekView = Microsoft.Office.Interop.Word.WdSeekView.wdSeekMainDocument;//退出页眉设置
            #endregion

            #region 页码设置并添加页码

            //为当前页添加页码
            Microsoft.Office.Interop.Word.PageNumbers pns = wordApp.Selection.Sections[1].Headers[Microsoft.Office.Interop.Word.WdHeaderFooterIndex.wdHeaderFooterEvenPages].PageNumbers;//获取当前页的号码
            pns.NumberStyle = Microsoft.Office.Interop.Word.WdPageNumberStyle.wdPageNumberStyleNumberInDash;//设置页码的风格，是Dash形还是圆形的
            pns.HeadingLevelForChapter = 0;
            pns.IncludeChapterNumber = false;
            pns.RestartNumberingAtSection = false;
            pns.StartingNumber = 0; //开始页页码？
            object pagenmbetal = Microsoft.Office.Interop.Word.WdPageNumberAlignment.wdAlignPageNumberCenter;//将号码设置在中间
            object first = true;
            wordApp.Selection.Sections[1].Footers[Microsoft.Office.Interop.Word.WdHeaderFooterIndex.wdHeaderFooterEvenPages].PageNumbers.Add(ref pagenmbetal, ref first);

            #endregion

            #region 行间距与缩进、文本字体、字号、加粗、斜体、颜色、下划线、下划线颜色设置

            wordApp.Selection.ParagraphFormat.LineSpacing = 16f;//设置文档的行间距
            wordApp.Selection.ParagraphFormat.FirstLineIndent = 30;//首行缩进的长度
            object unite = Microsoft.Office.Interop.Word.WdUnits.wdStory;
            //赋值 


            wordApp.Selection.EndKey(ref unite, ref Nothing);//将光标移到文本末尾

            wordDoc.Paragraphs.Last.Range.Font.Bold = 1;
            wordDoc.Paragraphs.Last.Range.Font.Size = 20;
            wordDoc.Paragraphs.Last.Range.Font.Name = "宋体";
            wordDoc.Paragraphs.Last.Range.Text = "阻 火 器 检 测 报 告\n\n";
            wordApp.Selection.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
            for (int p = 1; p < dg.Rows.Count + 1; p++)
            {
                #region
                for (int z = 0; z < 4; z++)
                {
                    //写入文本

                    wordApp.Selection.EndKey(ref unite, ref Nothing);//将光标移到文本末尾
                    wordApp.Selection.ParagraphFormat.FirstLineIndent = 0;//取消首行缩进的长度
                    wordDoc.Paragraphs.Last.Range.Font.Bold = 1;
                    wordDoc.Paragraphs.Last.Range.Font.Size = 15;
                    wordDoc.Paragraphs.Last.Range.Font.Name = "宋体";
                    wordDoc.Paragraphs.Last.Range.Text = dg.Columns[z].ColumnName + ":  ";


                    wordDoc.Paragraphs.Last.Range.Font.Bold = 0;
                    wordDoc.Paragraphs.Last.Range.Font.Size = 10;
                    wordDoc.Paragraphs.Last.Range.Font.Name = "宋体";
                    wordDoc.Paragraphs.Last.Range.InsertAfter(dg.Rows[p-1][z].ToString() + "\n");
                    wordApp.Selection.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphLeft;
                }



                #endregion




                #region 添加表格、填充数据、设置表格行列宽高、合并单元格、添加表头斜线、给单元格添加图片
                wordDoc.Content.InsertAfter("\n");//这一句与下一句的顺序不能颠倒，原因还没搞透
                wordApp.Selection.EndKey(ref unite, ref Nothing); //将光标移动到文档末尾
                wordApp.Selection.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphLeft;
                //object WdLine2 = MSWord.WdUnits.wdLine;//换一行;  
                //wordApp.Selection.MoveDown(ref WdLine2, 6, ref Nothing);//向下跨15行输入表格，这样表格就在文字下方了，不过这是非主流的方法

                //设置表格的行数和列数
                int tableRow = 2;
                int tableColumn = dg.Columns.Count - 4;

                //定义一个Word中的表格对象
                Microsoft.Office.Interop.Word.Table table = wordDoc.Tables.Add(wordApp.Selection.Range,
                tableRow, tableColumn, ref Nothing, ref Nothing);
                tableColumn = dg.Columns.Count;
                //默认创建的表格没有边框，这里修改其属性，使得创建的表格带有边框 
                table.Borders.Enable = 1;//这个值可以设置得很大，例如5、13等等

                //表格的索引是从1开始的。
                wordDoc.Tables[p].Cell(1, 1).Range.Text = "列\n行";
                for (int i = 1; i < tableRow; i++)
                {
                    for (int j = 4; j < tableColumn - 1; j++)
                    {
                        //   if (i == 1)
                        {
                            table.Cell(1, j + 2 - 4).Range.Text = dg.Columns[j + 1].ColumnName;//填充每列的标题
                        }
                        if (j == 5)
                        {
                            table.Cell(i + 1, 1).Range.Text = dg.Rows[i - 1][4].ToString(); //填充每行的标题
                        }
                        table.Cell(i + 1, j + 2 - 4).Range.Text = dg.Rows[p - 1][j + 1].ToString();  //填充表格的各个小格子
                    }
                }

                table.Cell(1, 2 + 1).Range.Text += "(m/s)";
                table.Cell(1, 3 + 1).Range.Text += "(m/s)";
                table.Cell(1, 4 + 1).Range.Text += "(m/s)";
                table.Cell(1, 5 + 1).Range.Text += "(m/s)";
                table.Cell(1, 6 + 1).Range.Text += "(m/s)";
                table.Cell(1, 7 + 1).Range.Text += "(m/s)";
                table.Cell(1, 8 + 1).Range.Text += "(m/s)";
                table.Cell(1, 9 + 1).Range.Text += "(KPa)";
                table.Cell(1, 10 + 1).Range.Text += "(KPa)";



                //设置table样式
                table.Rows.HeightRule = Microsoft.Office.Interop.Word.WdRowHeightRule.wdRowHeightAtLeast;//高度规则是：行高有最低值下限？
                table.Rows.Height = wordApp.CentimetersToPoints(float.Parse("0.8"));// 

                table.Range.Font.Size = 8F;
                table.Range.Font.Bold = 0;

                table.Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;//表格文本居中
                table.Range.Cells.VerticalAlignment = Microsoft.Office.Interop.Word.WdCellVerticalAlignment.wdCellAlignVerticalBottom;//文本垂直贴到底部
                                                                                                                                      //设置table边框样式
                table.Borders.OutsideLineStyle = Microsoft.Office.Interop.Word.WdLineStyle.wdLineStyleDouble;//表格外框是双线
                table.Borders.InsideLineStyle = Microsoft.Office.Interop.Word.WdLineStyle.wdLineStyleSingle;//表格内框是单线

                table.Rows[1].Range.Font.Bold = 1;//加粗
                table.Rows[1].Range.Font.Size = 10F;
                table.Cell(1, 1).Range.Font.Size = 10.5F;
                wordApp.Selection.Cells.Height = 30;//所有单元格的高度

                //除第一行外，其他行的行高都设置为20
                for (int i = 2; i <= tableRow; i++)
                {
                    table.Rows[i].Height = 20;
                }

                //将表格左上角的单元格里的文字（“行” 和 “列”）居右
                table.Cell(1, 1).Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphRight;
                //将表格左上角的单元格里面下面的“列”字移到左边，相比上一行就是将ParagraphFormat改成了Paragraphs[2].Format
                table.Cell(1, 1).Range.Paragraphs[2].Format.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphLeft;

                table.Columns[1].Width = 25;//将第 1列宽度设置为50

                //将其他列的宽度都设置为75
                for (int i = 2; i <= tableColumn - 6; i++)
                {
                    table.Columns[i].Width = 45;
                }


                //添加表头斜线,并设置表头的样式
                table.Cell(1, 1).Borders[Microsoft.Office.Interop.Word.WdBorderType.wdBorderDiagonalDown].Visible = true;
                table.Cell(1, 1).Borders[Microsoft.Office.Interop.Word.WdBorderType.wdBorderDiagonalDown].Color = Microsoft.Office.Interop.Word.WdColor.wdColorBlack;
                table.Cell(1, 1).Borders[Microsoft.Office.Interop.Word.WdBorderType.wdBorderDiagonalDown].LineWidth = Microsoft.Office.Interop.Word.WdLineWidth.wdLineWidth150pt;



                #endregion

                wordApp.Selection.EndKey(ref unite, ref Nothing); //将光标移动到文档末尾

                wordDoc.Content.InsertAfter("\n");

                #endregion
            }

        }

        public static void ExportExcel3(DataTable dg)
        {

            Microsoft.Office.Interop.Word._Application wordApp;                   //Word应用程序变量 
            Microsoft.Office.Interop.Word._Document wordDoc;                  //Word文档变量


            wordApp = new Microsoft.Office.Interop.Word.Application(); //初始化

            wordApp.Visible = true;//使文档可见



            //由于使用的是COM库，因此有许多变量需要用Missing.Value代替
            Object Nothing = Missing.Value;
            wordDoc = wordApp.Documents.Add(ref Nothing, ref Nothing, ref Nothing, ref Nothing);

            #region 页面设置、页眉图片和文字设置，最后跳出页眉设置

            //页面设置
            wordDoc.PageSetup.PaperSize = Microsoft.Office.Interop.Word.WdPaperSize.wdPaperA4;//设置纸张样式为A4纸
            wordDoc.PageSetup.Orientation = Microsoft.Office.Interop.Word.WdOrientation.wdOrientPortrait;//排列方式为垂直方向
            wordDoc.PageSetup.TopMargin = 57.0f;
            wordDoc.PageSetup.BottomMargin = 57.0f;
            wordDoc.PageSetup.LeftMargin = 57.0f;
            wordDoc.PageSetup.RightMargin = 57.0f;
            wordDoc.PageSetup.HeaderDistance = 30.0f;//页眉位置

            //设置页眉
            wordApp.ActiveWindow.View.Type = Microsoft.Office.Interop.Word.WdViewType.wdPrintView;//普通视图（即页面视图）样式
            wordApp.ActiveWindow.View.SeekView = Microsoft.Office.Interop.Word.WdSeekView.wdSeekPrimaryHeader;//进入页眉设置，其中页眉边距在页面设置中已完成
            wordApp.Selection.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphRight;//页眉中的文字右对齐




            //去掉页眉的横线
            wordApp.ActiveWindow.ActivePane.Selection.ParagraphFormat.Borders[Microsoft.Office.Interop.Word.WdBorderType.wdBorderBottom].LineStyle = Microsoft.Office.Interop.Word.WdLineStyle.wdLineStyleNone;
            wordApp.ActiveWindow.ActivePane.Selection.Borders[Microsoft.Office.Interop.Word.WdBorderType.wdBorderBottom].Visible = false;
            wordApp.ActiveWindow.ActivePane.View.SeekView = Microsoft.Office.Interop.Word.WdSeekView.wdSeekMainDocument;//退出页眉设置
            #endregion

            #region 页码设置并添加页码

            //为当前页添加页码
            Microsoft.Office.Interop.Word.PageNumbers pns = wordApp.Selection.Sections[1].Headers[Microsoft.Office.Interop.Word.WdHeaderFooterIndex.wdHeaderFooterEvenPages].PageNumbers;//获取当前页的号码
            pns.NumberStyle = Microsoft.Office.Interop.Word.WdPageNumberStyle.wdPageNumberStyleNumberInDash;//设置页码的风格，是Dash形还是圆形的
            pns.HeadingLevelForChapter = 0;
            pns.IncludeChapterNumber = false;
            pns.RestartNumberingAtSection = false;
            pns.StartingNumber = 0; //开始页页码？
            object pagenmbetal = Microsoft.Office.Interop.Word.WdPageNumberAlignment.wdAlignPageNumberCenter;//将号码设置在中间
            object first = true;
            wordApp.Selection.Sections[1].Footers[Microsoft.Office.Interop.Word.WdHeaderFooterIndex.wdHeaderFooterEvenPages].PageNumbers.Add(ref pagenmbetal, ref first);

            #endregion

            #region 行间距与缩进、文本字体、字号、加粗、斜体、颜色、下划线、下划线颜色设置

            wordApp.Selection.ParagraphFormat.LineSpacing = 16f;//设置文档的行间距
            wordApp.Selection.ParagraphFormat.FirstLineIndent = 30;//首行缩进的长度
            object unite = Microsoft.Office.Interop.Word.WdUnits.wdStory;
            //赋值 


            wordApp.Selection.EndKey(ref unite, ref Nothing);//将光标移到文本末尾

            wordDoc.Paragraphs.Last.Range.Font.Bold = 1;
            wordDoc.Paragraphs.Last.Range.Font.Size = 20;
            wordDoc.Paragraphs.Last.Range.Font.Name = "宋体";
            wordDoc.Paragraphs.Last.Range.Text = "阻 火 器 检 测 报 告\n\n";
            wordApp.Selection.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
            //for (int i = 0; i < 4; i++)
            //{
            //    //写入文本

            //    wordApp.Selection.EndKey(ref unite, ref Nothing);//将光标移到文本末尾
            //    wordApp.Selection.ParagraphFormat.FirstLineIndent = 0;//取消首行缩进的长度
            //    wordDoc.Paragraphs.Last.Range.Font.Bold = 1;
            //    wordDoc.Paragraphs.Last.Range.Font.Size = 15;
            //    wordDoc.Paragraphs.Last.Range.Font.Name = "宋体";
            //    wordDoc.Paragraphs.Last.Range.Text = dg.Columns[i].ColumnName + ":  ";


            //    wordDoc.Paragraphs.Last.Range.Font.Bold = 0;
            //    wordDoc.Paragraphs.Last.Range.Font.Size = 10;
            //    wordDoc.Paragraphs.Last.Range.Font.Name = "宋体";
            //    wordDoc.Paragraphs.Last.Range.InsertAfter(dg.Rows[0][i].ToString() + "\n");
            //    wordApp.Selection.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphLeft;
            //}



            #endregion




            #region 添加表格、填充数据、设置表格行列宽高、合并单元格、添加表头斜线、给单元格添加图片
            wordDoc.Content.InsertAfter("\n");//这一句与下一句的顺序不能颠倒，原因还没搞透
            wordApp.Selection.EndKey(ref unite, ref Nothing); //将光标移动到文档末尾
            wordApp.Selection.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphLeft;
            //object WdLine2 = MSWord.WdUnits.wdLine;//换一行;  
            //wordApp.Selection.MoveDown(ref WdLine2, 6, ref Nothing);//向下跨15行输入表格，这样表格就在文字下方了，不过这是非主流的方法

            //设置表格的行数和列数
            int tableRow = dg.Columns.Count;
            int tableColumn =   dg.Rows.Count;

            //定义一个Word中的表格对象
            Microsoft.Office.Interop.Word.Table table = wordDoc.Tables.Add(wordApp.Selection.Range,
            tableRow, tableColumn, ref Nothing, ref Nothing);
         //   tableColumn = dg.Columns.Count;
            //默认创建的表格没有边框，这里修改其属性，使得创建的表格带有边框 
            table.Borders.Enable = 1;//这个值可以设置得很大，例如5、13等等

            //表格的索引是从1开始的。
            wordDoc.Tables[1].Cell(1, 1).Range.Text = "列\n行";
            for (int i = 1; i < tableRow+1; i++)
            {
                for (int j = 1; j < tableColumn ; j++)
                {
                    if (j == 1)
                    {
                        table.Cell(i+ 1,1).Range.Text = dg.Columns[i-1].ColumnName;//填充每行的标题
                    }
                    if (i==1)
                    {
                        table.Cell(1,j+ 1).Range.Text = dg.Rows[j-1][4].ToString(); //填充每列的标题
                    }
                   
                    table.Cell(i + 1 ,j + 1).Range.Text = dg.Rows[j-1][i -1].ToString();  //填充表格的各个小格子
                }
            }

            table.Cell( 2 + 1,1).Range.Text += "(m/s)";
            table.Cell( 3 + 1,1).Range.Text += "(m/s)";
            table.Cell( 4 + 1,1).Range.Text += "(m/s)";
            table.Cell( 5 + 1,1).Range.Text += "(m/s)";
            table.Cell( 6 + 1,1).Range.Text += "(m/s)";
            table.Cell( 7 + 1,1).Range.Text += "(m/s)";
            table.Cell( 8 + 1,1).Range.Text += "(m/s)";
            table.Cell( 9 + 1,1).Range.Text += "(KPa)";
            table.Cell( 10 + 1,1).Range.Text +="(KPa)";



            //设置table样式
            table.Rows.HeightRule = Microsoft.Office.Interop.Word.WdRowHeightRule.wdRowHeightAtLeast;//高度规则是：行高有最低值下限？
            table.Rows.Height = wordApp.CentimetersToPoints(float.Parse("0.8"));// 

            table.Range.Font.Size = 8F;
            table.Range.Font.Bold = 0;

            table.Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;//表格文本居中
            table.Range.Cells.VerticalAlignment = Microsoft.Office.Interop.Word.WdCellVerticalAlignment.wdCellAlignVerticalBottom;//文本垂直贴到底部
            //设置table边框样式
            table.Borders.OutsideLineStyle = Microsoft.Office.Interop.Word.WdLineStyle.wdLineStyleDouble;//表格外框是双线
            table.Borders.InsideLineStyle = Microsoft.Office.Interop.Word.WdLineStyle.wdLineStyleSingle;//表格内框是单线

            table.Rows[1].Range.Font.Bold = 1;//加粗
            table.Rows[1].Range.Font.Size = 10F;
            table.Cell(1, 1).Range.Font.Size = 10.5F;
            wordApp.Selection.Cells.Height = 30;//所有单元格的高度

            //除第一行外，其他行的行高都设置为20
         //   for (int i = 2; i <= tableColumn; i++)
         //   {
           //     table.Columns[i]. = 20;
            //}

            //将表格左上角的单元格里的文字（“行” 和 “列”）居右
            table.Cell(1, 1).Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphRight;
            //将表格左上角的单元格里面下面的“列”字移到左边，相比上一行就是将ParagraphFormat改成了Paragraphs[2].Format
            table.Cell(1, 1).Range.Paragraphs[2].Format.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphLeft;

            table.Columns[1].Width = 25;//将第 1列宽度设置为50

            //将其他列的宽度都设置为75
            for (int i = 2; i <= tableColumn - 6; i++)
            {
                table.Columns[i].Width = 45;
            }


            //添加表头斜线,并设置表头的样式
            table.Cell(1, 1).Borders[Microsoft.Office.Interop.Word.WdBorderType.wdBorderDiagonalDown].Visible = true;
            table.Cell(1, 1).Borders[Microsoft.Office.Interop.Word.WdBorderType.wdBorderDiagonalDown].Color = Microsoft.Office.Interop.Word.WdColor.wdColorBlack;
            table.Cell(1, 1).Borders[Microsoft.Office.Interop.Word.WdBorderType.wdBorderDiagonalDown].LineWidth = Microsoft.Office.Interop.Word.WdLineWidth.wdLineWidth150pt;



            #endregion

            wordApp.Selection.EndKey(ref unite, ref Nothing); //将光标移动到文档末尾

            wordDoc.Content.InsertAfter("\n");


            //   object fileName = "";
            //   wordDoc.SaveAs(fileName, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing,
            //       ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing, ref Nothing);
            //   wordApp.Documents.Open(fileName, Nothing, false, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing);


        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="test_info"></param>
        /// <param name="dg1">阻爆轰试验数据</param>
        /// <param name="dg2">组爆燃试验数据</param>
        public static void ExportExcel_tag(DataTable test_info, DataTable dg1, DataTable dg2)
        {

            //**********************************************
            //来自博客http://blog.csdn.net/fujie724
            //**********************************************
            object oMissing = System.Reflection.Missing.Value;
            //创建一个Word应用程序实例
            Microsoft.Office.Interop.Word._Application oWord = new Microsoft.Office.Interop.Word.Application();
            //设置为不可见
            oWord.Visible = true;
            //模板文件地址，这里假设在X盘根目录
            //object oTemplate = "./word_temp/temp.doc";
            string  f = System.Environment.CurrentDirectory+@"\word_temp\temp.doc";
            object oTemplate = f;
            //以模板为基础生成文档
               Microsoft.Office.Interop.Word._Document oDoc = oWord.Documents.Add(ref oTemplate, ref oMissing, ref oMissing, ref oMissing);
         
         
           
            Table table = oDoc.Tables[1];

            //Table table = section[0] as Table;
            //声明书签数组
            object[] oBookMark = new object[9];
            //赋值书签名
            oBookMark[0] = "Product_No";
            oBookMark[1] = "Product_BitNo";
            oBookMark[2] = "GasGroup_No";
            oBookMark[3] = "Media_name";
            oBookMark[4] = "CBTB_G_C";

            oBookMark[5] = "Media_C";
            oBookMark[6] = "Product_Models";
            oBookMark[7] = "Product_Spcfts";
            oBookMark[8] = "Teststandard_No";
            //赋值任意数据到书签的位置
            oDoc.Bookmarks.get_Item(ref oBookMark[0]).Range.Text = test_info.Rows[0]["Product_No"].ToString();
            oDoc.Bookmarks.get_Item(ref oBookMark[1]).Range.Text = test_info.Rows[0]["Product_BitNo"].ToString();
            oDoc.Bookmarks.get_Item(ref oBookMark[2]).Range.Text = test_info.Rows[0]["GasGroup_No"].ToString();
            oDoc.Bookmarks.get_Item(ref oBookMark[3]).Range.Text = test_info.Rows[0]["Media_name"].ToString();
            oDoc.Bookmarks.get_Item(ref oBookMark[4]).Range.Text = test_info.Rows[0]["CBTB_G_C"].ToString();
            oDoc.Bookmarks.get_Item(ref oBookMark[5]).Range.Text = test_info.Rows[0]["Media_C"].ToString();
            oDoc.Bookmarks.get_Item(ref oBookMark[6]).Range.Text = test_info.Rows[0]["Product_Models"].ToString();
            oDoc.Bookmarks.get_Item(ref oBookMark[7]).Range.Text = test_info.Rows[0]["Product_Spcfts"].ToString();
            oDoc.Bookmarks.get_Item(ref oBookMark[8]).Range.Text = test_info.Rows[0]["Teststandard_No"].ToString();
            //弹出保存文件对话框，保存生成的Word


            #region 阻爆轰
            int Rs = dg1.Rows.Count >= 14 ? 14 : dg1.Rows.Count;
            if (Rs <= 7)
            {
                for (int i = 0; i < Rs; i++)
                {
                    table.Cell(8, 2 + i).Range.Text = dg1.Rows[i]["Initial_pressure"].ToString();
                    table.Cell(9, 2 + i).Range.Text = dg1.Rows[i]["explosion_pressure"].ToString();
                    table.Cell(10, 2 + i).Range.Text = dg1.Rows[i]["Flame_V"].ToString();
                    table.Cell(11, 2 + i).Range.Text = dg1.Rows[i]["Average_pressure"].ToString();
                    table.Cell(12, 2 + i).Range.Text = dg1.Rows[i]["Anti_Fire_flag"].ToString() == "0" ? "失败" : "成功";

                }


            }
            else
            {
                for (int i = 0; i < 7; i++)
                {
                    table.Cell(8, 2 + i).Range.Text = dg1.Rows[i]["Initial_pressure"].ToString();
                    table.Cell(9, 2 + i).Range.Text = dg1.Rows[i]["explosion_pressure"].ToString();
                    table.Cell(10, 2 + i).Range.Text = dg1.Rows[i]["Flame_V"].ToString();
                    table.Cell(11, 2 + i).Range.Text = dg1.Rows[i]["Average_pressure"].ToString();
                    table.Cell(12, 2 + i).Range.Text = dg1.Rows[i]["Anti_Fire_flag"].ToString() == "0" ? "失败" : "成功";

                }

                for (int i = 7; i < Rs; i++)
                {
                    table.Cell(15, 2 + i - 7).Range.Text = dg1.Rows[i]["Initial_pressure"].ToString();
                    table.Cell(16, 2 + i - 7).Range.Text = dg1.Rows[i]["explosion_pressure"].ToString();
                    table.Cell(17, 2 + i - 7).Range.Text = dg1.Rows[i]["Flame_V"].ToString();
                    table.Cell(18, 2 + i - 7).Range.Text = dg1.Rows[i]["Average_pressure"].ToString();
                    table.Cell(19, 2 + i - 7).Range.Text = dg1.Rows[i]["Anti_Fire_flag"].ToString() == "0" ? "失败" : "成功";

                }



            }
            #endregion


            #region 阻爆燃
             Rs = dg2.Rows.Count >= 14 ? 14 : dg2.Rows.Count;
            if (Rs <= 7)
            {
                for (int i = 0; i < Rs; i++)
                {
                    table.Cell(22, 2 + i).Range.Text =  dg2.Rows[i]["Initial_pressure"].ToString();
                    table.Cell(23, 2 + i).Range.Text =  dg2.Rows[i]["explosion_pressure"].ToString();
                    table.Cell(24, 2 + i).Range.Text = dg2.Rows[i]["Flame_V"].ToString();
                 //   table.Cell(25, 2 + i).Range.Text = dg2.Rows[i]["Average_pressure"].ToString();
                    table.Cell(25, 2 + i).Range.Text = dg2.Rows[i]["Anti_Fire_flag"].ToString() == "0" ? "失败" : "成功";

                }


            }
            else
            {
                for (int i = 0; i < 7; i++)
                {
                    table.Cell(22, 2 + i).Range.Text =  dg2.Rows[i]["Initial_pressure"].ToString();
                    table.Cell(23, 2 + i).Range.Text =  dg2.Rows[i]["explosion_pressure"].ToString();
                    table.Cell(24, 2 + i).Range.Text = dg2.Rows[i]["Flame_V"].ToString();
                  //  table.Cell(25, 2 + i).Range.Text = dg2.Rows[i]["Average_pressure"].ToString();
                    table.Cell(25, 2 + i).Range.Text = dg2.Rows[i]["Anti_Fire_flag"].ToString() == "0" ? "失败" : "成功";

                }

                for (int i = 7; i < Rs ; i++)
                {
                   table.Cell(28, 2 + i-7).Range.Text = dg2.Rows[i]["Initial_pressure"].ToString();
                   table.Cell(29, 2 + i-7).Range.Text = dg2.Rows[i]["explosion_pressure"].ToString();
                   table.Cell(30, 2 + i-7).Range.Text = dg2.Rows[i]["Flame_V"].ToString();
                  // table.Cell(30, 2 + i).Range.Text = dg2.Rows[i]["Average_pressure"].ToString();
                   table.Cell(31, 2 + i-7).Range.Text = dg2.Rows[i]["Anti_Fire_flag"].ToString() == "0" ? "失败" : "成功";

                }



            }
            #endregion



            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Word Document(*.doc)|*.doc";
            sfd.DefaultExt = "Word Document(*.doc)|*.doc";
            if (sfd.ShowDialog() == true)
            {
                object filename = sfd.FileName;

                oDoc.SaveAs(ref filename, ref oMissing, ref oMissing, ref oMissing,
                ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing,
                ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing,
                ref oMissing, ref oMissing);
                oDoc.Close(ref oMissing, ref oMissing, ref oMissing);
                //关闭word
                oWord.Quit(ref oMissing, ref oMissing, ref oMissing);
            }

        }
    }

}
    


