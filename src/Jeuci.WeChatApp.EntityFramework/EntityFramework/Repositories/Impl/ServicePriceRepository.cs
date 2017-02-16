using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.EntityFramework;
using Jeuci.WeChatApp.Lottery.Models;
using Jeuci.WeChatApp.Repositories;

namespace Jeuci.WeChatApp.EntityFramework.Repositories.Impl
{
    public class ServicePriceRepository : WeChatAppRepositoryBase<ServerPrice>, IServicePriceRepository
    {
        public ServicePriceRepository(IDbContextProvider<WeChatAppDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public IList<ServerPrice> GetServerPrices(int sid, int userId)
        {
            SqlParameter[] paramsList = { new SqlParameter { ParameterName = "@UID", Value = userId }, new SqlParameter { ParameterName = "@SID", Value = sid } };
            using (System.Data.Common.DbCommand cmd = Context.Database.Connection.CreateCommand())
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "P_API_QueryServiceBuyOnline";
                cmd.Parameters.AddRange(paramsList);
                if (cmd.Connection.State != System.Data.ConnectionState.Open)
                    cmd.Connection.Open();
                IList<ServerPrice> list = new List<ServerPrice>();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ServerPrice item = new ServerPrice();
                        item.Id = Convert.ToInt32(reader["ID"]);
                        item.AgentPrice = Convert.ToDecimal(reader["AgentPrice"]);
                        item.Price = Convert.ToDecimal(reader["Price"]);
                        item.AuthDesc = reader["AuthDesc"].ToString().Contains("-") ? 
                            reader["AuthDesc"].ToString().Split('-')[1] : reader["AuthDesc"].ToString();
                        item.CanByOnline = Convert.ToInt32(reader["CanByOnline"]);
                        item.Description = reader["Description"].ToString();
                        item.ServiceId = Convert.ToInt32(reader["ServiceId"]);
                        item.State = Convert.ToInt32(reader["State"]);
                        item.AuthType = Convert.ToInt32(reader["AuthType"]);
                        item.BeforeDiscountPrice = ((int) (((double) item.Price/0.8)*0.01)*100 + 98);
                        list.Add(item);
                    }
                }
                return list;
            }
        }
    }
}
