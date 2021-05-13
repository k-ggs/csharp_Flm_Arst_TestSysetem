using System;
using System.Collections.Generic;
using System.Data;
using Word = Microsoft.Office.Interop.Word;
using System.IO;

using System.Runtime.Remoting.Contexts;
using System.Windows;
using Flm_Arst_TestSysetem.Model;
using System.Reflection;

namespace Flm_Arst_TestSysetem.tool
{
    /// <summary>
    /// 使用替换模板进行到处word文件
    /// </summary>
    public class WordUtility
    {
        private object tempFile = null;
        private object saveFile = null;
        private static Word._Document wDoc = null; //word文档
        private static Word._Application wApp = null; //word进程
        private object missing = System.Reflection.Missing.Value;
        //public void LeadWord()
        //{
        //    #region 动态创建DataTable数据
        //    DataTable tblDatas = new DataTable("Datas");
        //    DataColumn dc = null;
        //    //赋值给dc，是便于对每一个datacolumn的操作
        //    dc = tblDatas.Columns.Add("ID", Type.GetType("System.Int32"));
        //    dc.AutoIncrement = true;//自动增加
        //    dc.AutoIncrementSeed = 1;//起始为1
        //    dc.AutoIncrementStep = 1;//步长为1
        //    dc.AllowDBNull = false;//
        //    dc = tblDatas.Columns.Add("Test_name", Type.GetType("System.String"));
        //    dc = tblDatas.Columns.Add("Test_users", Type.GetType("System.String"));
        //    dc = tblDatas.Columns.Add("Test_unit", Type.GetType("System.String"));
        //    dc = tblDatas.Columns.Add("Test_time", Type.GetType("System.String"));
        //    dc = tblDatas.Columns.Add("str2", Type.GetType("System.String"));
        //    dc = tblDatas.Columns.Add("str3", Type.GetType("System.String"));
        //    dc = tblDatas.Columns.Add("str4", Type.GetType("System.String"));
        //    dc = tblDatas.Columns.Add("str5", Type.GetType("System.String"));
        //    dc = tblDatas.Columns.Add("str6", Type.GetType("System.String"));
        //    dc = tblDatas.Columns.Add("remark", Type.GetType("System.String"));
        //    DataRow newRow;
        //    newRow = tblDatas.NewRow();
        //    newRow["name"] = "张三";
        //    newRow["sex"] = "男";
        //    newRow["age"] = "11";
        //    newRow["str1"] = "字符串1";
        //    newRow["str2"] = "字符串2";
        //    newRow["str3"] = "字符串3";
        //    newRow["str4"] = "字符串4";
        //    newRow["str5"] = "字符串5";
        //    newRow["str6"] = "字符串6";
        //    newRow["remark"] = "备注一下";
        //    tblDatas.Rows.Add(newRow);
        //    #endregion
        //    #region word要替换的表达式和表格字段的对应关系
        //    Dictionary<string, string> dic = new Dictionary<string, string>();
        //    dic.Add("$name$", "name");
        //    dic.Add("$sex$", "sex");
        //    dic.Add("$age$", "age");
        //    dic.Add("$str1$", "str1");
        //    dic.Add("$str2$", "str2");
        //    dic.Add("$str3$", "str3");
        //    dic.Add("$str4$", "str4");
        //    dic.Add("$str5$", "str5");
        //    dic.Add("$str6$", "str6");
        //    dic.Add("$remark$", "remark");
        //    #endregion
        //    string tempFile = "~/Content/Word/temp.doc";
        //    string saveFile = "~/Content/Word/1.doc";
        //    WordUtility w = new WordUtility(tempFile, saveFile);
        //    w.GenerateWord(tblDatas, dic, null);
        //    return Content("ok");
        //}
        public WordUtility(string tempFile, string saveFile)
        {
           // tempFile = "";
          // saveFile = System.Web.HttpContext.Current.Server.MapPath(saveFile);
            this.tempFile = tempFile;
            this.saveFile = saveFile;
        }

        /// <summary>
        /// 模版包含头部信息和表格，表格重复使用
        /// </summary>
        /// <param name="dt">重复表格的数据</param>
        /// <param name="expPairColumn">word中要替换的表达式和表格字段的对应关系</param>
        /// <param name="simpleExpPairValue">简单的非重复型数据</param>
        public bool GenerateWord(List<Model_word> dt3, Dictionary<string, string> expPairColumn, Dictionary<string, string> simpleExpPairValue)
        {
            DataTable dt =ToDataTable<Model_word>(dt3);
            if (!File.Exists(tempFile.ToString()))
            {

                return false;
            }
            try
            {
                wApp = new Word.Application();

                wApp.Visible = false;

                wDoc = wApp.Documents.Add(ref tempFile, ref missing, ref missing, ref missing);

                wDoc.Activate();// 当前文档置前

                bool isGenerate = false;

                if (simpleExpPairValue != null && simpleExpPairValue.Count > 0)
                    isGenerate = ReplaceAllRang(simpleExpPairValue);

                // 表格有重复
                if (dt != null && dt.Rows.Count > 0 && expPairColumn != null && expPairColumn.Count > 0)
                    isGenerate = GenerateTable(dt, expPairColumn);

                if (isGenerate)
                    wDoc.SaveAs(ref saveFile, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing,
                        ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing);

                DisposeWord();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public  DataTable ToDataTable<T>( List<T> items)
        {
            var tb = new DataTable(typeof(T).Name);

            PropertyInfo[] props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo prop in props)
            {
                Type t = GetCoreType(prop.PropertyType);
                tb.Columns.Add(prop.Name, t);
            }

            foreach (T item in items)
            {
                var values = new object[props.Length];

                for (int i = 0; i < props.Length; i++)
                {
                    values[i] = props[i].GetValue(item, null);
                }

                tb.Rows.Add(values);
            }

            return tb;
        }
        public  Type GetCoreType(Type t)
        {
            if (t != null && IsNullable(t))
            {
                if (!t.IsValueType)
                {
                    return t;
                }
                else
                {
                    return Nullable.GetUnderlyingType(t);
                }
            }
            else
            {
                return t;
            }
        }
        public  bool IsNullable(Type t)
        {
            return !t.IsValueType || (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>));
        }


        public bool GenerateWord(DataTable dt, Dictionary<string, string> expPairColumn, Dictionary<string, string> simpleExpPairValue)
        {
            if (!File.Exists(tempFile.ToString()))
            {

                return false;
            }
            try
            {
                wApp = new Word.Application();

                wApp.Visible = false;

                wDoc = wApp.Documents.Add(ref tempFile, ref missing, ref missing, ref missing);

                wDoc.Activate();// 当前文档置前

                bool isGenerate = false;

                if (simpleExpPairValue != null && simpleExpPairValue.Count > 0)
                    isGenerate = ReplaceAllRang(simpleExpPairValue);

                // 表格有重复
                if (dt != null && dt.Rows.Count > 0 && expPairColumn != null && expPairColumn.Count > 0)
                    isGenerate = GenerateTable(dt, expPairColumn);

                if (isGenerate)
                    wDoc.SaveAs(ref saveFile, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing,
                        ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing);

                DisposeWord();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }



        /// <summary>
        /// 单个替换 模版没有重复使用的表格
        /// </summary>
        /// <param name="dc">要替换的</param>
        public bool GenerateWord(Dictionary<string, string> dc)
        {
            List<Model_word> dcc = new List<Model_word>();
            dcc = null;
            return GenerateWord(dcc, null, dc);
        }


        private bool GenerateTable(DataTable dt, Dictionary<string, string> expPairColumn)
        {
            try
            {
                int tableNums = dt.Rows.Count;

                Word.Table tb = wDoc.Tables[1];

                tb.Range.Copy();

                Dictionary<string, object> dc = new Dictionary<string, object>();
                for (int i = 0; i < tableNums; i++)
                {
                    dc.Clear();

                    if (i == 0)
                    {
                        foreach (string key in expPairColumn.Keys)
                        {
                            string column = expPairColumn[key];
                            object value = null;
                            value = dt.Rows[i][column];
                            dc.Add(key, value);
                        }

                        ReplaceTableRang(wDoc.Tables[1], dc);
                        continue;
                    }

                    wDoc.Paragraphs.Last.Range.Paste();

                    foreach (string key in expPairColumn.Keys)
                    {
                        string column = expPairColumn[key];
                        object value = null;
                        value = dt.Rows[i][column];
                        dc.Add(key, value);
                    }

                    ReplaceTableRang(wDoc.Tables[1], dc);
                }


                return true;
            }
            catch (Exception ex)
            {
                DisposeWord();
                return false;
            }
        }
   
        private bool ReplaceTableRang(Word.Table table, Dictionary<string, object> dc)
        {
            try
            {
                object replaceArea = Word.WdReplace.wdReplaceAll;

                foreach (string item in dc.Keys)
                {
                    object replaceKey = item;
                    object replaceValue = dc[item];
                    table.Range.Find.Execute(ref replaceKey, ref missing, ref missing, ref missing,
                      ref missing, ref missing, ref missing, ref missing, ref missing,
                      ref replaceValue, ref replaceArea, ref missing, ref missing, ref missing,
                      ref missing);
                }
                return true;
            }
            catch (Exception ex)
            {
                DisposeWord();

                return false;
            }
        }

        private bool ReplaceAllRang(Dictionary<string, string> dc)
        {
            try
            {
                object replaceArea = Word.WdReplace.wdReplaceAll;

                foreach (string item in dc.Keys)
                {
                    object replaceKey = item;
                    object replaceValue = dc[item];
                    wApp.Selection.Find.Execute(ref replaceKey, ref missing, ref missing, ref missing,
                      ref missing, ref missing, ref missing, ref missing, ref missing,
                      ref replaceValue, ref replaceArea, ref missing, ref missing, ref missing,
                      ref missing);
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private void DisposeWord()
        {
            object saveOption = Word.WdSaveOptions.wdSaveChanges;

            wDoc.Close(ref saveOption, ref missing, ref missing);

            saveOption = Word.WdSaveOptions.wdDoNotSaveChanges;

            wApp.Quit(ref saveOption, ref missing, ref missing); //关闭Word进程
        }
    }
}