using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zhaoxi.Communication;
using Flm_Arst_TestSysetem.DAL;
using Flm_Arst_TestSysetem.Model;
using System.Data;
using System.Windows;
using Flm_Arst_TestSysetem.Base;
using Flm_Arst_TestSysetem.ViewModel;
namespace Flm_Arst_TestSysetem.BLL
{
    public class IndustrialBLL
    {
        DataAccess da = new DataAccess();
        // 获取串口信息
        public DataResult<SerialInfo> InitSerialInfo()
        {
            DataResult<SerialInfo> result = new DataResult<SerialInfo>();

            try
            {
                SerialInfo si = new SerialInfo();
                si.PortName = ConfigurationManager.AppSettings["port"].ToString();
                si.BaudRate = int.Parse(ConfigurationManager.AppSettings["baud"].ToString());
                si.DataBit = int.Parse(ConfigurationManager.AppSettings["data_bit"].ToString());
                si.Parity = (Parity)Enum.Parse(typeof(Parity), ConfigurationManager.AppSettings["parity"].ToString(), true);
                si.StopBits = (StopBits)Enum.Parse(typeof(StopBits), ConfigurationManager.AppSettings["stop_bit"].ToString(), true);

                result.State = true;
                result.Data = si;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }

            return result;
            ;
        }

        public DataResult<List<StorageModel>> InitStorageArea()
        {
            DataResult<List<StorageModel>> result = new DataResult<List<StorageModel>>();

            try
            {
                var sa = da.GetStorageArea();

                result.State = true;
                result.Data = (from q in sa.AsEnumerable()
                               select new StorageModel
                               {
                                   id = q.Field<string>("id"),
                                   SlaveAddress = q.Field<Int32>("slave_add"),
                                   FuncCode = q.Field<string>("func_code"),
                                   StartAddress = int.Parse(q.Field<string>("start_reg")),
                                   Length = int.Parse(q.Field<string>("length"))
                               }).ToList();
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }

            return result;
        }

        public DataResult<List<DeviceModel>> InitDevices()
        {
            DataResult<List<DeviceModel>> result = new DataResult<List<DeviceModel>>();

            try
            {
                var devices = da.GetDevices();
                var monitorValues = da.GetMonitorValues();

                List<DeviceModel> deviceList = new List<DeviceModel>();
                foreach (var dr in devices.AsEnumerable())
                {
                    DeviceModel dModel = new DeviceModel();
                    deviceList.Add(dModel);

                    dModel.DeviceID = dr.Field<string>("d_id");
                    dModel.DeviceName = dr.Field<string>("d_name");
                    dModel.IsRunning = true;
                    dModel.IsWarning = false;

                    foreach (var mv in monitorValues.AsEnumerable().Where(m => m.Field<string>("d_id") == dModel.DeviceID))
                    {
                        MonitorValueModel mvm = new MonitorValueModel();
                        dModel.MonitorValueList.Add(mvm);

                        mvm.ValueId = mv.Field<string>("value_id");
                        mvm.ValueName = mv.Field<string>("value_name");
                        mvm.StorageAreaId = mv.Field<string>("s_area_id");
                        mvm.StartAddress = mv.Field<Int32>("address");
                        mvm.DataType = mv.Field<string>("data_type");
                        mvm.IsAlarm = mv.Field<Int32>("is_alarm") == 1;
                        mvm.ValueDesc = mv.Field<string>("description");
                        mvm.Unit = mv.Field<string>("unit");

                        // 警戒值
                        var column = mv.Field<string>("alarm_lolo");
                        mvm.LoLoAlarm = column == null ? 0.0 : double.Parse(column);
                        column = mv.Field<string>("alarm_low");
                        mvm.LowAlarm = column == null ? 0.0 : double.Parse(column);
                        column = mv.Field<string>("alarm_high");
                        mvm.HighAlarm = column == null ? 0.0 : double.Parse(column);
                        column = mv.Field<string>("alarm_hihi");
                        mvm.HiHiAlarm = column == null ? 0.0 : double.Parse(column);

                        mvm.ValueStateChanged = (state, msg, value_id) =>
                        {
                            try
                            {
                                Application.Current?.Dispatcher.Invoke(() =>
                                {
                                    var index = dModel.WarningMessageList.ToList().FindIndex(w => w.ValueId == value_id);
                                    if (index > -1)
                                        dModel.WarningMessageList.RemoveAt(index);

                                    if (state != Base.MonitorValueState.OK)
                                    {
                                        dModel.IsWarning = true;
                                        dModel.WarningMessageList.Add(new WarningMessageModel { ValueId = value_id, Message = msg });
                                    }
                                });

                                var ss = dModel.WarningMessageList.Count > 0;
                                if (dModel.IsWarning != ss)
                                {
                                    dModel.IsWarning = ss;
                                }
                            }
                            catch { }
                        };
                    }
                }

                result.State = true;
                result.Data = deviceList;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }
            return result;
        }

        public List<Model_zuhuoqi> model_Zuhuoqis;
        //  public List<Model_data> model_M_data;
        public Model_word Model_words;
        public Model_Result model_Result;
        /// <summary>
        /// 获取一组数据
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public List<Model_zuhuoqi> Get_Data(string name)
        {
            string sql = "select media_name,Create_time, Test_users,fr_record.Test_index,fr_record.row_id,fr_record.Clock_Rate,fr_record.senors,fr_record.Vals,fr_record.Test_name from fr_record  where Test_name='" + name + "'";
            model_Zuhuoqis = new List<Model_zuhuoqi>();
            DataTable dt = DataMysql.QuerySQL(sql);

            model_Zuhuoqis = (from q in dt.AsEnumerable()
                              select new Model_zuhuoqi
                              {
                                  Test_index = q.Field<Int32>("Test_index"),
                                  Clock_Rate = q.Field<Int32>("Clock_Rate"),
                                  senors = q.Field<String>("senors"),
                                  Vals = q.Field<string>("Vals").Split('#').ToList<string>(),
                                  Test_name = q.Field<String>("Test_name"),
                                  sval = q.Field<string>("Vals"),
                                  senor1 = q.Field<string>("Vals").Split('#')[0],
                                  senor2 = q.Field<string>("Vals").Split('#')[1],
                                  senor3 = q.Field<string>("Vals").Split('#')[2],
                                  senor4 = q.Field<string>("Vals").Split('#')[3],
                                  senor5 = q.Field<string>("Vals").Split('#')[4],
                                  senor6 = q.Field<string>("Vals").Split('#')[5],
                                  senor7 = q.Field<string>("Vals").Split('#')[6],
                                  senor8 = q.Field<string>("Vals").Split('#')[7],
                                  Psenor1 = q.Field<string>("Vals").Split('#')[8],
                                  Psenor2 = q.Field<string>("Vals").Split('#')[9],
                                  users_name = q.Field<String>("Test_users"),
                                  time = q.Field<String>("Create_time"),
                                  media_name = q.Field<string>("media_name")

                              }).ToList();
            return model_Zuhuoqis;


        }


        public List<Model_data> Get_Data_bulk(string name)
        {
            string sql = "select  media_name,Create_time, Test_users,Test_index,fr_record.row_id,fr_record.Clock_Rate,fr_record.Test_name,Fire_sensor1,Fire_sensor2,Fire_sensor3,Fire_sensor4,Fire_sensor5,Fire_sensor6,Fire_sensor6,Fire_sensor7,Fire_sensor8,P_sensor1,P_sensor2 from fr_record  where Test_name='" + name + "'";
            //  model_Zuhuoqis = new List<Model_zuhuoqi>();
            DataTable dt = DataMysql.QuerySQL(sql);
            List<Model_data> lmd;
            lmd = (from q in dt.AsEnumerable()
                   select new Model_data
                   {
                       Test_index = q.Field<Int32>("Test_index"),
                       Clock_Rate = q.Field<Int32>("Clock_Rate"),
                       //senors = q.Field<String>("senors"),
                       //   Vals = q.Field<string>("Vals").Split('#').ToList<string>(),
                      
                       //sval = q.Field<string>("Vals"),
                       Fire_sensor1 = q.Field<double>("Fire_sensor1"),
                       Fire_sensor2 = q.Field<double>("Fire_sensor2"),
                       Fire_sensor3 = q.Field<double>("Fire_sensor3"),
                       Fire_sensor4 = q.Field<double>("Fire_sensor4"),
                       Fire_sensor5 = q.Field<double>("Fire_sensor5"),
                       Fire_sensor6 = q.Field<double>("Fire_sensor6"),
                       Fire_sensor7 = q.Field<double>("Fire_sensor7"),
                       Fire_sensor8 = q.Field<double>("Fire_sensor8"),
                       P_sensor1 = q.Field<double>("P_sensor1"),
                       P_sensor2 = q.Field<double>("P_sensor2"),
                      
                       Create_time = q.Field<String>("Create_time"),
                       media_name = q.Field<string>("media_name")

                   }).ToList();
            return lmd;


        }
        public DataTable dtGet_ResultData;
        public List<Model_word> Get_ResultData(string min_date, string max_date, string name)
        {
            string sql = "select fr_record_name 试验名称,Test_users 试验人员,Test_unit 检测单位,Test_time 检测时间,Row_id 行,media_name 介质,ROUND(sp1,2) 爆速1,ROUND(sp2,2) 爆速2,ROUND(sp3,2) 爆速3,ROUND(sp4,2) 爆速4,ROUND(sp5,2) 爆速5,ROUND(sp6,2) 爆速6,ROUND(sp7,2) 爆速7,ROUND(P1,2) 压力1,ROUND(P2,2) 压力2 from sys_result_record where 1=1";
            List<Model_word> Model_words = new List<Model_word>();

            string btw = " and  SUBSTR(REPLACE(fr_record_name,':',''),1,8) BETWEEN '" + min_date + "' and '" + max_date + "' ";
            string name_sql = "and fr_record_name='" + name + "' ";
            if (!string.IsNullOrEmpty(min_date) && !string.IsNullOrEmpty(max_date))
            {
                sql += btw;
            }
            if (!string.IsNullOrEmpty(name))
            {
                sql += name_sql;
            }

            dtGet_ResultData = DataMysql.QuerySQL(sql);

            Model_words = (from q in dtGet_ResultData.AsEnumerable()
                           select new Model_word
                           {
                               Test_name = q.Field<string>("试验名称"),
                               Test_unit = q.Field<string>("检测单位"),
                               Test_time = q.Field<String>("检测时间"),
                               Test_users = q.Field<string>("试验人员"),
                               Row_id = q.Field<int>("行"),
                               media_name = q.Field<string>("介质"),
                               sp1 = q.Field<double>("爆速1").ToString(),
                               sp2 = q.Field<double>("爆速2").ToString(),
                               sp3 = q.Field<double>("爆速3").ToString(),
                               sp4 = q.Field<double>("爆速4").ToString(),
                               sp5 = q.Field<double>("爆速5").ToString(),
                               sp6 = q.Field<double>("爆速6").ToString(),
                               sp7 = q.Field<double>("爆速7").ToString(),

                               P1 = q.Field<double>("压力1").ToString(),
                               P2 = q.Field<double>("压力2").ToString()


                           }).ToList();
            return Model_words;


        }





        public string unit = "xxxxxxxx";
        public float s1 = 200;
        public float s2 = 200;
        public float s3 = 200;
        public float s4 = 200;
        public float s5 = 200;
        public float s6 = 200;
        public float s7 = 200;
        public float s8 = 200;


        public Model_word Insert_result(string name)
        {
            List<Model_zuhuoqi> model_Zuhuoqiss = Get_Data(name);
            int ms1 = model_Zuhuoqiss.Where(a => a.senor1 == model_Zuhuoqiss.Max(m => m.senor1)).Select(m => m.Test_index).FirstOrDefault();
            int ms2 = model_Zuhuoqiss.Where(a => a.senor2 == model_Zuhuoqiss.Max(m => m.senor2)).Select(m => m.Test_index).FirstOrDefault();
            int ms3 = model_Zuhuoqiss.Where(a => a.senor3 == model_Zuhuoqiss.Max(m => m.senor3)).Select(m => m.Test_index).FirstOrDefault();
            int ms4 = model_Zuhuoqiss.Where(a => a.senor4 == model_Zuhuoqiss.Max(m => m.senor4)).Select(m => m.Test_index).FirstOrDefault();
            int ms5 = model_Zuhuoqiss.Where(a => a.senor5 == model_Zuhuoqiss.Max(m => m.senor5)).Select(m => m.Test_index).FirstOrDefault();
            int ms6 = model_Zuhuoqiss.Where(a => a.senor6 == model_Zuhuoqiss.Max(m => m.senor6)).Select(m => m.Test_index).FirstOrDefault();
            int ms7 = model_Zuhuoqiss.Where(a => a.senor7 == model_Zuhuoqiss.Max(m => m.senor7)).Select(m => m.Test_index).FirstOrDefault();
            int ms8 = model_Zuhuoqiss.Where(a => a.senor8 == model_Zuhuoqiss.Max(m => m.senor8)).Select(m => m.Test_index).FirstOrDefault();
            String P_7 = model_Zuhuoqiss.Max(m => m.Psenor1);
            String P_8 = model_Zuhuoqiss.Max(m => m.Psenor2);
            Model_words = (from q in model_Zuhuoqiss
                           select new Model_word
                           {
                               media_name = q.media_name,
                               Test_name = q.Test_name,
                               Test_time = q.time,
                               Test_unit = unit,
                               Test_users = q.users_name,
                               sp1 = (s1 / Math.Abs(ms2 - ms1) * 1 / q.Clock_Rate * 1000000).ToString(),
                               sp2 = (s2 / Math.Abs(ms3 - ms2) * 1 / q.Clock_Rate * 1000000).ToString(),
                               sp3 = (s3 / Math.Abs(ms4 - ms3) * 1 / q.Clock_Rate * 1000000).ToString(),
                               sp4 = (s4 / Math.Abs(ms5 - ms4) * 1 / q.Clock_Rate * 1000000).ToString(),
                               sp5 = (s5 / Math.Abs(ms6 - ms5) * 1 / q.Clock_Rate * 1000000).ToString(),
                               sp6 = (s6 / Math.Abs(ms7 - ms6) * 1 / q.Clock_Rate * 1000000).ToString(),
                               sp7 = (s7 / Math.Abs(ms8 - ms7) * 1 / q.Clock_Rate * 1000000).ToString(),

                               P1 = P_7,
                               P2 = P_8




                           }


                          ).FirstOrDefault();
            string sql = string.Format("insert into sys_result_record (fr_record_name,Test_users,Test_unit,Test_time,media_name,sp1,sp2,sp3,sp4,sp5,sp6,sp7,P1,P2,Create_time)" +
                " VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}')", Model_words.Test_name, Model_words.Test_users, Model_words.Test_unit, Model_words.Test_time,

                Model_words.media_name, Model_words.sp1,
                                           Model_words.sp2,
                                           Model_words.sp3,
                                            Model_words.sp4,
                                            Model_words.sp5,
                                            Model_words.sp6,
                                            Model_words.sp7,
                                            Model_words.P1,
                                             Model_words.P2,
                                             DateTime.Now.ToString("yyyyMMddHHmmss")

                                            );



            DataMysql.ExcuteSQL(sql);

            return Model_words;

        }



        public Model_Result Insert_result_bulk()
        {
            try
            {
                DataTable dt = DataMysql.QuerySQL("select max(Row_id) Row_id from sys_testres_record");
                List<Model_data> model_Zuhuoqiss = SystemMonitorViewModel.Highdatas;
                int ms1 = model_Zuhuoqiss.Where(a => a.Fire_sensor1 == model_Zuhuoqiss.Max(m => m.Fire_sensor1)).Select(m => m.Test_index).FirstOrDefault();
                int ms2 = model_Zuhuoqiss.Where(a => a.Fire_sensor2 == model_Zuhuoqiss.Max(m => m.Fire_sensor2)).Select(m => m.Test_index).FirstOrDefault();
                int ms3 = model_Zuhuoqiss.Where(a => a.Fire_sensor3 == model_Zuhuoqiss.Max(m => m.Fire_sensor3)).Select(m => m.Test_index).FirstOrDefault();
                int ms4 = model_Zuhuoqiss.Where(a => a.Fire_sensor4 == model_Zuhuoqiss.Max(m => m.Fire_sensor4)).Select(m => m.Test_index).FirstOrDefault();
                int ms5 = model_Zuhuoqiss.Where(a => a.Fire_sensor5 == model_Zuhuoqiss.Max(m => m.Fire_sensor5)).Select(m => m.Test_index).FirstOrDefault();
                int ms6 = model_Zuhuoqiss.Where(a => a.Fire_sensor6 == model_Zuhuoqiss.Max(m => m.Fire_sensor6)).Select(m => m.Test_index).FirstOrDefault();
                int ms7 = model_Zuhuoqiss.Where(a => a.Fire_sensor7 == model_Zuhuoqiss.Max(m => m.Fire_sensor7)).Select(m => m.Test_index).FirstOrDefault();
                int ms8 = model_Zuhuoqiss.Where(a => a.Fire_sensor8 == model_Zuhuoqiss.Max(m => m.Fire_sensor8)).Select(m => m.Test_index).FirstOrDefault();
                double P_7 = model_Zuhuoqiss.Max(m => m.P_sensor1);
                double P_8 = model_Zuhuoqiss.Max(m => m.P_sensor2);
                model_Result = (from q in model_Zuhuoqiss
                                select new Model_Result
                                {
                                    media_name = q.media_name,
                                 //   Test_name = q.Test_name,
                                    Test_time = q.Create_time,
                                  //  Test_unit = q.Test_unit,
                                  //  Test_users = q.Test_users,
                                    sp1 = (Globalvariable.systemMonitorViewModel.S1 * 0.001 / Math.Abs(ms2 - ms1) / q.Clock_Rate).ToString(),
                                    sp2 = (Globalvariable.systemMonitorViewModel.S2 * 0.001 / Math.Abs(ms3 - ms2) / q.Clock_Rate).ToString(),
                                    sp3 = (Globalvariable.systemMonitorViewModel.S3 * 0.001 / Math.Abs(ms4 - ms3) / q.Clock_Rate).ToString(),
                                    sp4 = (Globalvariable.systemMonitorViewModel.S4 * 0.001 / Math.Abs(ms5 - ms4) / q.Clock_Rate).ToString(),
                                    sp5 = (Globalvariable.systemMonitorViewModel.S5 * 0.001 / Math.Abs(ms6 - ms5) / q.Clock_Rate).ToString(),
                                    sp6 = (Globalvariable.systemMonitorViewModel.S6 * 0.001 / Math.Abs(ms7 - ms6) / q.Clock_Rate).ToString(),
                                    sp7 = (Globalvariable.systemMonitorViewModel.S7 * 0.001 / Math.Abs(ms8 - ms7) / q.Clock_Rate).ToString(),

                                    P1 = P_7.ToString(),
                                    P2 = P_8.ToString(),
                                    clock_rate = q.Clock_Rate,
                                    idxsp1 = ms1,
                                    idxsp2 = ms2,
                                    idxsp3 = ms3,
                                    idxsp4 = ms4,
                                    idxsp5 = ms5,
                                    idxsp6 = ms6,
                                    idxsp7 = ms7,
                                    idxsp8 = ms8,
                                    maxfs1 = model_Zuhuoqiss.Max(m => m.Fire_sensor1),
                                    maxfs2 = model_Zuhuoqiss.Max(m => m.Fire_sensor2),
                                    maxfs3 = model_Zuhuoqiss.Max(m => m.Fire_sensor3),
                                    maxfs4 = model_Zuhuoqiss.Max(m => m.Fire_sensor4),
                                    maxfs5 = model_Zuhuoqiss.Max(m => m.Fire_sensor5),
                                    maxfs6 = model_Zuhuoqiss.Max(m => m.Fire_sensor6),
                                    maxfs7 = model_Zuhuoqiss.Max(m => m.Fire_sensor7),
                                    maxfs8 = model_Zuhuoqiss.Max(m => m.Fire_sensor8),
                                    maxPs1 = model_Zuhuoqiss.Max(m => m.P_sensor1),
                                    maxPs2 = model_Zuhuoqiss.Max(m => m.P_sensor2),
                                    selectionlength = q.sectionLength









                                }


                              ).FirstOrDefault();
                string sql = string.Format("insert into sys_result_record (Test_index,Test_users,Test_unit,Test_time,media_name,sp1,sp2,sp3,sp4,sp5,sp6,sp7,P1,P2,Create_time,clock_rate,idxsp1,idxsp2,idxsp3,idxsp4,idxsp5,idxsp6,idxsp7,idxsp8,maxfs1,maxfs2,maxfs3,maxfs4,maxfs5,maxfs6,maxfs7,maxfs8,maxPs1,maxPs2,selectionlength)" +
                    " VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}','{19}','{20}','{21}','{22}','{23}','{24}','{25}','{26}','{27}','{28}','{29}','{30}','{31}','{32}','{33}','{34}')", model_Result.Test_name, model_Result.Test_users, model_Result.Test_unit, model_Result.Test_time,

                   dt.Rows[0]["Row_id"], model_Result.sp1,
                                               model_Result.sp2,
                                               model_Result.sp3,
                                                model_Result.sp4,
                                                model_Result.sp5,
                                                model_Result.sp6,
                                                model_Result.sp7,
                                                model_Result.P1,
                                                 model_Result.P2,

                                                 DateTime.Now.ToString("yyyyMMddHHmmss"),
                                                   model_Result.clock_rate,
                                   model_Result.idxsp1,
                                   model_Result.idxsp2,
                                   model_Result.idxsp3,
                                   model_Result.idxsp4,
                                   model_Result.idxsp5,
                                   model_Result.idxsp6,
                                   model_Result.idxsp7,
                                   model_Result.idxsp8,
                                    model_Result.maxfs1,
                                   model_Result.maxfs2,
                                   model_Result.maxfs3,
                                   model_Result.maxfs4,
                                  model_Result.maxfs5,
                                   model_Result.maxfs6,
                                  model_Result.maxfs7,
                                   model_Result.maxfs8,
                                   model_Result.maxPs1,
                                  model_Result.maxPs2,
                                   model_Result.selectionlength

                                                );



                DataMysql.ExcuteSQL(sql);
            }

            catch (Exception ex)
            {

            }

            return model_Result;

        }




        public List<Cm_box> Get_TestNams(string min_date, string max_date)

        {
            List<Cm_box> vs = new List<Cm_box>();
            try
            {
                if (min_date.Length < 1 || max_date.Length < 1)
                { return vs; }
                if (Convert.ToInt32(min_date) > Convert.ToInt32(max_date))
                {
                    return vs;
                }

                string sql = "select CONCAT(b.Product_No,'#',b.Product_BitNo) slt,a.Test_index Test_index from fr_record a,sys_testres_record b where a.Test_index=b.Row_id AND SUBSTR(REPLACE(a.Create_time,':',''),1,8) BETWEEN '" + min_date + "' and '" + max_date + "' group by b.Product_No,b.Product_BitNo  ";

                DataTable dt = DataMysql.QuerySQL(sql);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    vs.Add(new Cm_box { text = dt.Rows[i]["slt"].ToString(), idx = Convert.ToInt32(dt.Rows[i]["Test_index"]) });


                }

                return vs;
            }
            catch (Exception)
            {

                return vs;

            }


        }

        public void export_word()
        {

            List<Model_word> Lmodel_Words = new List<Model_word>();



        }

        public List<string> Get_userNams()
        {

            List<string> vs = new List<string>();
            try
            {


                string sql = "select sys_user.User_name from sys_user";

                DataTable dt = DataMysql.QuerySQL(sql);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    vs.Add(dt.Rows[i]["User_name"].ToString());


                }

                return vs;
            }
            catch (Exception)
            {

                return vs;

            }

        }


        public List<string> Get_MediaNams()
        {

            List<string> vs = new List<string>();
            try
            {


                string sql = "select Media_name,Create_time from sys_media";

                DataTable dt = DataMysql.QuerySQL(sql);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    vs.Add(dt.Rows[i]["Media_name"].ToString());


                }

                return vs;
            }
            catch (Exception)
            {

                return vs;

            }

        }

        public DataTable Get_sLength()
        {
            DataTable dt = new DataTable();


            try
            {


                string sql = "select s1,s2,s3,s4,s5,s6,S7,clock_rate,selection,channelcount,start_channel from sys_config";

                dt = DataMysql.QuerySQL(sql);


                return dt;
            }
            catch (Exception)
            {

                return dt;

            }

        }

        public bool update_sLength(List<string> vs)
        {
            DataTable dt = new DataTable();


            try
            {

                if (vs.Count < 7)
                {
                    return false;
                }
                foreach (string s in vs)
                {
                    if (string.IsNullOrEmpty(s))
                    {
                        return false;
                    }
                }
                string sql = string.Format("update sys_config set s1='{0}',s2='{1}',s3='{2}',s4='{3}',s5='{4}',s6='{5}',S7='{6}'     ", vs[0], vs[1], vs[2], vs[3], vs[4], vs[5], vs[6]);

                if (DataMysql.ExcuteSQL(sql))
                {
                    return true;

                }
                else
                {
                    return false;

                }



            }
            catch (Exception)
            {

                return false;

            }

        }


        public bool update_pcie(int clock_rate, int selection)
        {
            DataTable dt = new DataTable();


            try
            {

                string sql = string.Format("update sys_config set clock_rate='{0}',selection='{1}'     ", clock_rate, selection);

                if (DataMysql.ExcuteSQL(sql))
                {
                    return true;

                }
                else
                {
                    return false;

                }



            }
            catch (Exception)
            {

                return false;

            }

        }

        public string Get_unit()
        {
            DataTable dt = new DataTable();


            try
            {


                string sql = "select unit_name from sys_unit limit 1";

                dt = DataMysql.QuerySQL(sql);


                return dt.Rows[0]["unit_name"].ToString();
            }
            catch (Exception)
            {

                return "";

            }

        }

        public bool update_Unit(string vs)
        {
            DataTable dt = new DataTable();


            try
            {



                if (string.IsNullOrEmpty(vs))
                {
                    return false;
                }

                string sql = string.Format("update sys_unit set unit_name='{0}'  ", vs);

                if (DataMysql.ExcuteSQL(sql))
                {
                    return true;

                }
                else
                {
                    return false;

                }



            }
            catch (Exception)
            {

                return false;

            }

        }

        public void add_user(string name)
        {
            string sql1 = "select sys_user.User_name from sys_user where User_name='" + name + "' ";

            string sql1_de = "delete from sys_user where User_name='" + name + "' ";


            string sql = "INSERT INTO sys_user (User_name,Create_time) VALUES('" + name + "','" + DateTime.Now.ToString("yyyyMMddHHmmss") + "')";
            try {


                DataTable dt = DataMysql.QuerySQL(sql1);
                if (dt.Rows.Count > 0)
                {
                    Globalvariable.systemMonitorViewModel.LogInfoAdd("用户添加 #", name + ":已存在!", LogType.Fault);
                    DataMysql.ExcuteSQL(sql1_de);
                    Globalvariable.systemMonitorViewModel.LogInfoAdd("用户添加 #", name + ":已删除!", LogType.Warn);
                    return;
                }


                bool b = DataMysql.ExcuteSQL(sql);
                if (b)
                { Globalvariable.systemMonitorViewModel.LogInfoAdd("用户添加 #", name + ":添加成功!", LogType.Info); }
                else { Globalvariable.systemMonitorViewModel.LogInfoAdd("用户添加 #", name + ":添加失败!", LogType.Fault); }
            }
            catch (Exception)
            {
                Globalvariable.systemMonitorViewModel.LogInfoAdd("用户添加 #", name + ":添加失败259!", LogType.Fault);
                return;
            }

        }


        public void add_Media(string name)
        {
            string sql1 = "select Media_name ,create_time from sys_media   where Media_name='" + name + "' ";

            string sql1_de = "delete from sys_media  where Media_name='" + name + "' ";


            string sql = "INSERT INTO sys_media (Media_name,create_time) VALUES('" + name + "','" + DateTime.Now.ToString("yyyyMMddHHmmss") + "')";
            try
            {


                DataTable dt = DataMysql.QuerySQL(sql1);
                if (dt.Rows.Count > 0)
                {
                    Globalvariable.systemMonitorViewModel.LogInfoAdd("介质添加 #", name + ":已存在!", LogType.Fault);
                    DataMysql.ExcuteSQL(sql1_de);
                    Globalvariable.systemMonitorViewModel.LogInfoAdd("介质添加 #", name + ":已删除!", LogType.Warn);
                    return;
                }


                bool b = DataMysql.ExcuteSQL(sql);
                if (b)
                { Globalvariable.systemMonitorViewModel.LogInfoAdd("介质添加 #", name + ":添加成功!", LogType.Info); }
                else { Globalvariable.systemMonitorViewModel.LogInfoAdd("介质添加 #", name + ":添加失败!", LogType.Fault); }
            }
            catch (Exception)
            {
                Globalvariable.systemMonitorViewModel.LogInfoAdd("介质添加 #", name + ":添加失败259!", LogType.Fault);
                return;
            }

        }

        #region 阻火器参数 
        public List<sys_product> Get_products_data(string Product_No = "", string BitNo = "")
        {
            string sql = string.Empty;
            if (Product_No == "" && BitNo == "")
            {
                sql = "select Row_id, GasGroup_No,Teststandard_No,Product_BitNo,Product_Models,Product_Spcfts,Product_No,Remake, Create_time from sys_product ";

            }
            else
            {

                sql = "select Row_id, GasGroup_No,Teststandard_No,Product_BitNo,Product_Models,Product_Spcfts,Product_No,Remake, Create_time from sys_product where Product_No='" + Product_No + "' and Product_BitNo='" + BitNo + "'";
            }
            List<sys_product> ls = new List<sys_product>();
            DataTable dt = DataMysql.QuerySQL(sql);

            ls = (from q in dt.AsEnumerable()
                  select new sys_product
                  { Row_id = q.Field<int>("Row_id"),
                      GasGroup_No = q.Field<string>("GasGroup_No"),
                      Teststandard_No = q.Field<string>("Teststandard_No"),
                      //senors = q.Field<String>("senors"),
                      //   Vals = q.Field<string>("Vals").Split('#').ToList<string>(),
                      Product_BitNo = q.Field<String>("Product_BitNo"),
                      //sval = q.Field<string>("Vals"),
                      Product_Models = q.Field<string>("Product_Models"),
                      Product_Spcfts = q.Field<string>("Product_Spcfts"),
                      Product_No = q.Field<string>("Product_No"),
                      Remake = q.Field<string>("Remake"),
                      Create_time = q.Field<string>("Create_time")


                  }).ToList();
            return ls;


        }
        public bool delete_products_data(int index)
        {
            string s_query = "select * from sys_product where Row_id=" + index + " ";
            DataTable dt = DataMysql.QuerySQL(s_query);
            if (dt == null)
            {
                return false;
            }
            if (dt.Rows.Count >= 1)
            {
                string s_delete = "delete from sys_product where Row_id=" + index + "";

                if (DataMysql.deleteSQL(s_delete) > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            else
            {
                return false;

            }


            //  DataMysql.ExcuteSQL()


        }

        public List<string> Get_products_BitNo(string Product_No)
        {
            string sql = "select Product_BitNo from sys_product  where Product_No='" + Product_No + "' GROUP BY Product_BitNo";

            List<string> ls = new List<string>();
            try
            {

                DataTable dt = DataMysql.QuerySQL(sql);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ls.Add(dt.Rows[i]["Product_BitNo"].ToString());


                }



                return ls;
            }
            catch (Exception)
            {
                return ls;
                throw;
            }



        }

        public List<string> Get_products_No()
        {
            string sql = "select Product_No from sys_product GROUP BY Product_No";

            List<string> ls = new List<string>();
            try
            {

                DataTable dt = DataMysql.QuerySQL(sql);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ls.Add(dt.Rows[i]["Product_No"].ToString());


                }



                return ls;
            }
            catch (Exception)
            {
                return ls;
                //  throw;
            }

        }


        public bool Insert_products_data(sys_product s_pt)
        {
            try {




                string insert_sql =string.Format( "insert into  sys_product (Product_No,Product_Models,Product_Spcfts,Product_BitNo,GasGroup_No,Teststandard_No,Remake,Create_time)VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}')", s_pt.Product_No, s_pt.Product_Models,
                    s_pt.Product_Spcfts, s_pt.Product_BitNo, s_pt.GasGroup_No,s_pt.Teststandard_No,s_pt.Remake,s_pt.Create_time);

             return   DataMysql.ExcuteSQL(insert_sql);
            }
            catch (Exception e)
            {
                return false;
              //  Globalvariable.
            }
        
        
        }

        #endregion


        #region 导出word
        /// <summary>
        /// 获取试验数据
        /// </summary>
        public DataTable get_GW(sys_testres_record sys)
        {
            DataTable dt=null;
            string sql =string.Format( "select frequency,Initial_pressure,explosion_pressure,Flame_V,Average_pressure,Anti_Fire_flag from sys_testres_record where " +
                "Product_No='{0}' and Product_BitNo='{1}' and Media_name='{2}' and CBTB_G_C='{3}' and Media_C='{4}' AND Test_Type_No = '{5}' ",
                  sys.Product_No,sys.Product_BitNo,sys.Media_name,sys.CBTB_G_C,sys.Media_C,sys.Test_Type_No);
            try
            {

                dt = DataMysql.QuerySQL(sql);

                return dt;
            }
            catch (Exception ex)
            {
                return dt;

            }
        
        
        }
        //select a.Product_No,a.Product_BitNo,a.GasGroup_No,b.Media_name,b.CBTB_G_C,b.Media_C,a.Product_Models,a.Product_Spcfts,a.Teststandard_No from sys_product a,sys_testres_record b where a.Product_No=b.Product_No and a.Product_BitNo=b.Product_BitNo and b.Product_No='F001' and b.Product_BitNo='0001' and b.Media_name='甲烷' and b.CBTB_G_C='0' and b.Media_C='0'
        // AND b.Test_Type_No='GZ-II' 
        public DataTable get_GW_param(sys_testres_record sys)
        {
            DataTable dt = null;
            string sql = string.Format("select a.Product_No,a.Product_BitNo,a.GasGroup_No,b.Media_name,b.CBTB_G_C,b.Media_C,a.Product_Models,a.Product_Spcfts,a.Teststandard_No from sys_product a,sys_testres_record b where" +
                " a.Product_No=b.Product_No and a.Product_BitNo=b.Product_BitNo and b.Product_No='{0}' and b.Product_BitNo='{1}' and b.Media_name='{2}' and b.CBTB_G_C='{3}' and b.Media_C='{4}' limit 1 ",
                  sys.Product_No, sys.Product_BitNo, sys.Media_name, sys.CBTB_G_C, sys.Media_C); 
            try
            {

                dt = DataMysql.QuerySQL(sql);

                return dt;
            }
            catch (Exception ex)
            {
                return dt;

            }


        }


        #endregion
        #region
        public List<sys_testres_record> get_testres_record(string Product_No = "", string BitNo = "", string media = "")
        {
            string sql = "";
            if (Product_No == "" && BitNo == "" && media == "")
            {
                sql = "select * from sys_testres_record ";
            }
            else
            {
                sql = "select * from sys_testres_record where Product_No='" + Product_No + "' and Product_BitNo='" + BitNo + "' and Media_name='" + media + "' order by frequency DESC";
            }
            List<sys_testres_record> ls = new List<sys_testres_record>();
            DataTable dt = DataMysql.QuerySQL(sql);

            ls = (from q in dt.AsEnumerable()
                  select new sys_testres_record
                  { Row_id = q.Field<int>("Row_id"),
                      Product_No = q.Field<string>("Product_No"),
                      Product_BitNo = q.Field<string>("Product_BitNo"),
                      //senors = q.Field<String>("senors"),
                      //   Vals = q.Field<string>("Vals").Split('#').ToList<string>(),
                      Test_Type_No = q.Field<String>("Test_Type_No"),
                      //sval = q.Field<string>("Vals"),
                      Media_name = q.Field<string>("Media_name"),
                      CBTB_G_C = q.Field<double>("CBTB_G_C"),
                      Media_C = q.Field<double>("Media_C"),
                      frequency = q.Field<int>("frequency").ToString(),
                      Initial_pressure = q.Field<double>("Initial_pressure"),
                      explosion_pressure = q.Field<double>("explosion_pressure"),
                      Flame_V = q.Field<double>("Flame_V"),
                      Average_pressure = q.Field<double>("Average_pressure"),
                      Create_time = q.Field<string>("Create_time")



                  }).ToList();
            return ls;


        }


        public List<Model_data> get_fr_record(int index)
        {

            string sql = "select idx,b.Test_Type_No, a.media_name,a.Create_time, a.Test_users,a.Test_index,a.row_id,a.Clock_Rate,a.Test_name,Fire_sensor1,Fire_sensor2,Fire_sensor3,Fire_sensor4,Fire_sensor5,Fire_sensor6,Fire_sensor6,Fire_sensor7,Fire_sensor8,P_sensor1,P_sensor2 from fr_record a,sys_testres_record b  where a.Test_index=b.Row_id AND a.Test_index='" + index + "'";
            //    model_Zuhuoqis = new List<Model_zuhuoqi>();
            DataTable dt = DataMysql.QuerySQL(sql);
            List<Model_data> lmd;
            lmd = (from q in dt.AsEnumerable()
                   select new Model_data
                   { idx = q.Field<Int32>("idx"),
                       Test_index = q.Field<Int32>("Test_index"),
                       Clock_Rate = q.Field<Int32>("Clock_Rate"),
                       Test_Type_No = q.Field<String>("Test_Type_No"),
                     

                       Fire_sensor1 = q.Field<double>("Fire_sensor1"),
                       Fire_sensor2 = q.Field<double>("Fire_sensor2"),
                       Fire_sensor3 = q.Field<double>("Fire_sensor3"),
                       Fire_sensor4 = q.Field<double>("Fire_sensor4"),
                       Fire_sensor5 = q.Field<double>("Fire_sensor5"),
                       Fire_sensor6 = q.Field<double>("Fire_sensor6"),
                       Fire_sensor7 = q.Field<double>("Fire_sensor7"),
                       Fire_sensor8 = q.Field<double>("Fire_sensor8"),
                       P_sensor1 = q.Field<double>("P_sensor1"),
                       P_sensor2 = q.Field<double>("P_sensor2"),
                     
                       Create_time = q.Field<String>("Create_time"),
                       media_name = q.Field<string>("media_name")

                   }).ToList();
            return lmd;


        }




        //public bool insert_test_record(sys_testres_record str)
        //{
        //    //insert sys_testres_record (sys_testres_record.Product_No,sys_testres_record.Product_BitNo, sys_testres_record.Media_name,sys_testres_record.Create_time,sys_testres_record.Test_Type_No,sys_testres_record. ) VALUES()
        //    string sql = string.Format("insert sys_testres_record (Product_No,Product_BitNo,Test_Type_No,Media_name,CBTB_G_C,Media_C,frequency,Initial_pressure,Create_time ) " +
        //           "VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}')", str.Product_No, str.Product_BitNo, str.Test_Type_No, str.Media_name, str.CBTB_G_C, str.Media_C, str.frequency, str.Initial_pressure, str.Create_time);
        //    try {
        //        if (DataMysql.ExcuteSQL(sql))
        //        { return true; }
        //        else
        //        { return false; }
        //    }
        //    catch (Exception ex)
        //    {
        //        return false;
        //    }
        //    return true;
        //}

          public bool insert_test_record(sys_testres_record str)
        {
            //insert sys_testres_record (sys_testres_record.Product_No,sys_testres_record.Product_BitNo, sys_testres_record.Media_name,sys_testres_record.Create_time,sys_testres_record.Test_Type_No,sys_testres_record. ) VALUES()
            string sql = string.Format("insert sys_testres_record (Product_No,Product_BitNo,Test_Type_No,Media_name,CBTB_G_C,Media_C,frequency,Initial_pressure,Create_time, Test_index) " +
                   "VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", str.Product_No, str.Product_BitNo, str.Test_Type_No, str.Media_name, str.CBTB_G_C, str.Media_C, str.frequency, str.Initial_pressure, str.Create_time,str.Test_index);
            try {
                if (DataMysql.ExcuteSQL(sql))
                { return true; }
                else
                { return false; }
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }



        public bool update_test_record(sys_testres_record str)
        {
            //insert sys_testres_record (sys_testres_record.Product_No,sys_testres_record.Product_BitNo, sys_testres_record.Media_name,sys_testres_record.Create_time,sys_testres_record.Test_Type_No,sys_testres_record. ) VALUES()
            string sql = string.Format("update  sys_testres_record  set explosion_pressure='{0}',flame_v='{1}',average_Pressure='{2}',anti_fire_flag='{3}' ,create_time='{4}' WHERE row_id='{5}'",
                                                                        str.explosion_pressure, str.Flame_V, str.Average_pressure, str.Anti_Fire_flag, str.Create_time, str.Row_id);
            try
            {
                if (DataMysql.ExcuteSQL(sql))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            //   return true;
        }



        public List<sys_test_type> Get_Test_type()
        {

            string sql = "select Row_id,Test_Type_No,Test_Type_Desc,Create_time from sys_test_type GROUP BY Test_Type_No";

            List<sys_test_type> ls = new List<sys_test_type>();
            try
            {

                DataTable dt = DataMysql.QuerySQL(sql);
                ls = (from q in dt.AsEnumerable()
                      select new sys_test_type
                      {
                          Row_id = q.Field<int>("Row_id"),
                          Test_Type_Desc = q.Field<string>("Test_Type_Desc"),

                          Test_Type_No = q.Field<string>("Test_Type_No"),


                          Create_time = q.Field<string>("Create_time")



                      }).ToList();



                return ls;
            }
            catch (Exception)
            {
                return ls;
                // throw;
            }
        }



        /// <summary>
        /// 获取相同产品 介质性质的试验次数
        /// </summary>
        /// <returns></returns>
        public int Get_test_frequency(string Product_No, string Product_BitNo, string Test_Type_No, string Media_name, string CBTB_G_C, string Media_C)
        {
            string text_idx_s = "select max(Test_index) MTest_index from sys_testres_record ";
            try
            {
                string sql = "SELECT MAX(frequency) frequency,Test_index FROM sys_testres_record WHERE Product_No = '" + Product_No + "'AND Product_BitNo = '" + Product_BitNo + "'AND Test_Type_No = '" + Test_Type_No + "'AND Media_name = '" + Media_name + "'AND CBTB_G_C = '" + CBTB_G_C + "'AND Media_C = '" + Media_C + "'";

              
                DataTable dt = DataMysql.QuerySQL(sql);
                if (dt == null)
                {
                    Globalvariable.systemMonitorViewModel.mTexidx = Convert.ToInt32(DataMysql.QuerySQL(text_idx_s).Rows[0]["MTest_index"]) + 1;

                    return 0; }
               

                    if (dt.Rows[0]["frequency"].ToString().Length <1)
                {
                    Globalvariable.systemMonitorViewModel.mTexidx = Convert.ToInt32(DataMysql.QuerySQL(text_idx_s).Rows[0]["MTest_index"]) + 1;

                    return 0; }
                if (dt.Rows.Count < 1)
                {


                    Globalvariable.systemMonitorViewModel.mTexidx = Convert.ToInt32(DataMysql.QuerySQL(text_idx_s).Rows[0]["MTest_index"]) + 1;

                    return 0;
                }
                else
                {
                    if (Convert.ToInt32(dt.Rows[0]["frequency"]) < 1)
                    { Globalvariable.systemMonitorViewModel.mTexidx = Convert.ToInt32(DataMysql.QuerySQL(text_idx_s).Rows[0]["MTest_index"]) + 1; }
                    else
                    { Globalvariable.systemMonitorViewModel.mTexidx = Convert.ToInt32(dt.Rows[0]["Test_index"]); }
                      
                }
                int frequency = (Convert.ToInt32(dt.Rows[0]["frequency"]));


                return frequency;
            }
            catch (Exception E)
            {
                Globalvariable.systemMonitorViewModel.mTexidx = DataMysql.QuerySQL(text_idx_s).Rows.Count>1? Convert.ToInt32(DataMysql.QuerySQL(text_idx_s).Rows[0]["MTest_index"]) + 1:1;
                Globalvariable.systemMonitorViewModel.LogInfoAdd("ErrorCode #", "Error" + E.ToString(), LogType.Fault);
                return 0;
            }

        }

        public int Get_test_maxRow_id()
        {
            try
            {
                string sql = "SELECT MAX(Row_id) Row_id  FROM sys_testres_record ";
                DataTable dt = DataMysql.QuerySQL(sql);
                if (dt.Rows.Count < 1)
                {
                    return 1;
                }
                int max_id = (Convert.ToInt32(dt.Rows[0]["Row_id"]) + 1);


                return max_id;
            }
            catch (Exception E)
            {
                Globalvariable.systemMonitorViewModel.LogInfoAdd("ErrorCode #", "Error" + E.ToString(), LogType.Fault);
                return 0;
            }

        }


        public List<sys_teststandard> Get_teststandard()
        {
            List<sys_teststandard> lists = new List<sys_teststandard>();
            try
            {
             
                string sql = "select Teststandard_No,Teststandard_Desc from sys_teststandard GROUP BY Teststandard_No";
                DataTable dt = DataMysql.QuerySQL(sql);
                lists=(from q in dt.AsEnumerable()
                       select new sys_teststandard
                       {
                           Teststandard_No = q.Field<string>("Teststandard_No"),
                           Teststandard_Desc = q.Field<string>("Teststandard_Desc")
                       }).ToList();


                return lists;
            }
            catch (Exception E)
            {
                Globalvariable.systemMonitorViewModel.LogInfoAdd("ErrorCode1193 #", "Error" + E.ToString(), LogType.Fault);
                return lists;
            }


        }

        public List<sys_gasgroup> Get_gas_group()
        {
            List<sys_gasgroup> lists = new List<sys_gasgroup>();
            try
            {

                string sql = "select GasGroup_No,GasGroup_Desc from sys_gasgroup GROUP BY GasGroup_No";
                DataTable dt = DataMysql.QuerySQL(sql);
                lists = (from q in dt.AsEnumerable()
                         select new sys_gasgroup
                         {
                             GasGroup_No = q.Field<string>("GasGroup_No"),
                             GasGroup_Desc = q.Field<string>("GasGroup_Desc")
                         }).ToList();


                return lists;
            }
            catch (Exception E)
            {
                Globalvariable.systemMonitorViewModel.LogInfoAdd("ErrorCode1193 #", "Error" + E.ToString(), LogType.Fault);
                return lists;
            }


        }




        public List<sys_testres_record> GET_TEST_GROUP()
        {
            List<sys_testres_record> LGt = new List<sys_testres_record>();

            String SQL = "select count(Product_No) ct,Product_No,Product_BitNo,Media_name,CBTB_G_C,Media_C ,Create_time from sys_testres_record GROUP BY Product_No,Product_BitNo,Media_name,CBTB_G_C,Media_C";
            try
            {
                DataTable dt = DataMysql.QuerySQL(SQL);
                LGt = (from q in dt.AsEnumerable()

                       select new sys_testres_record
                       {
                           Test_index=Convert.ToInt32( q.Field<Int64>("ct")),
                           Product_No =q.Field<string>("Product_No").ToString(),
                           Product_BitNo=q.Field<string>("Product_BitNo").ToString(),
                           Media_name=q.Field<string>("Media_name").ToString(),
                           CBTB_G_C=q.Field<double>("CBTB_G_C"),
                           Media_C= q.Field<double>("Media_C"),
                           Create_time=q.Field<string>("Create_time")
                       }
                ).ToList();

                return LGt;
            }
            catch (Exception ex)
            {
                return LGt;
                // throw;
            }


        }
        #endregion
    }
}
