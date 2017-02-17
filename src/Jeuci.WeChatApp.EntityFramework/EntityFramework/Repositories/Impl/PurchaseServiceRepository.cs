using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Abp.EntityFramework;
using Abp.Logging;
using Jeuci.WeChatApp.Common.Enums;
using Jeuci.WeChatApp.Lottery.Models;
using Jeuci.WeChatApp.Pay.Models;
using Jeuci.WeChatApp.Repositories;

namespace Jeuci.WeChatApp.EntityFramework.Repositories.Impl
{
    public class PurchaseServiceRepository : WeChatAppRepositoryBase<ServerPrice>, IPurchaseServiceRepository
    {
        public PurchaseServiceRepository(IDbContextProvider<WeChatAppDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public bool GeneratePayOrder(PayOrder payOrder)
        {
            var sqlParamsList = new List<SqlParameter>();
            var properties = payOrder.GetType().GetProperties();
            foreach (var pro in properties)
            {
                sqlParamsList.Add(new SqlParameter()
                {
                    ParameterName = string.Format("@{0}",pro.Name),
                    Value = pro.GetValue(payOrder, null)
                });
            }

            using (System.Data.Common.DbCommand cmd = Context.Database.Connection.CreateCommand())
            {
                try
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "P_PAY_NewPayOrder";
                    cmd.Parameters.AddRange(sqlParamsList.ToArray());
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();
                    cmd.ExecuteNonQuery();

                    return true;
                }
                catch (Exception e)
                {
                    LogHelper.Logger.Error(e.Message);
                    return false;
                }
            }
        }

        public int CompleteServiceOrder(CompleteServiceOrder payOrder)
        {
            var sqlParamsList = new List<SqlParameter>();
            var properties = payOrder.GetType().GetProperties();
            foreach (var pro in properties)
            {
                sqlParamsList.Add(new SqlParameter()
                {
                    ParameterName = string.Format("@{0}", pro.Name),
                    Value = pro.GetValue(payOrder, null)
                });
            }

            sqlParamsList.Add(new SqlParameter()
            {
                ParameterName = "@Ret",
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.Output,
            });

            using (System.Data.Common.DbCommand cmd = Context.Database.Connection.CreateCommand())
            {
                try
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "P_PAY_CompleteServiceOrder";
                    cmd.Parameters.AddRange(sqlParamsList.ToArray());
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();
                    cmd.ExecuteNonQuery();

                    return Convert.ToInt32(sqlParamsList[sqlParamsList.Count - 1].Value);
                }
                catch (Exception e)
                {
                    LogHelper.Logger.Error(e.Message);
                    return -1;
                }
            }

        }

        public int FailServiceOrder(UpdateServiceOrder order)
        {
            var sqlParamsList = new List<SqlParameter>();
            var properties = order.GetType().GetProperties();
            foreach (var pro in properties)
            {
                sqlParamsList.Add(new SqlParameter()
                {
                    ParameterName = string.Format("@{0}", pro.Name),
                    Value = pro.GetValue(order, null)
                });
            }

            sqlParamsList.Add(new SqlParameter()
            {
                ParameterName = "@Ret",
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.Output,
            });

            using (System.Data.Common.DbCommand cmd = Context.Database.Connection.CreateCommand())
            {
                try
                {
                    //
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "P_PAY_UpdatePayOrder";
                    cmd.Parameters.AddRange(sqlParamsList.ToArray());
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();
                    cmd.ExecuteNonQuery();

                    return Convert.ToInt32(sqlParamsList[sqlParamsList.Count - 1].Value);
                }
                catch (Exception e)
                {
                    LogHelper.Logger.Error(e.Message);
                    return -1;
                }
            }

        }

        public IList<UserPayOrderInfo> GetNeedQueryOrderList(PayMode mobileWeb)
        {
            using (System.Data.Common.DbCommand cmd = Context.Database.Connection.CreateCommand())
            {
                try
                {
                    var orderList = new List<UserPayOrderInfo>();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "P_PAY_QueryUnclosedPayOrderInfos";
                    cmd.Parameters.Add(new SqlParameter()
                    {
                        ParameterName = "@PayMode",
                        Value = (int)mobileWeb
                    });
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var order = new UserPayOrderInfo();
                            order.CreateTime = Convert.ToDateTime(reader["CreateTime"]);
                            order.PayAppID = reader["PayAppID"].ToString();
                            order.State = Convert.ToInt32(reader["State"]);
                            order.UId = Convert.ToInt32(reader["UID"]);
                            order.Id = reader["ID"].ToString();
                            orderList.Add(order);
                        }
                    }

                    return orderList;
                }
                catch (Exception ex)
                {
                    LogHelper.Logger.Error(ex.Message);
                    return null;
                }
            }
        }
    }
}