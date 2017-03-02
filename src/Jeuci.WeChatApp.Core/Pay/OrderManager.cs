using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Logging;
using Jeuci.WeChatApp.Common.Tools;
using Jeuci.WeChatApp.Pay.Models;

namespace Jeuci.WeChatApp.Pay
{
    public class OrderManager : IOrderManager
    {
        private Timer timer = null;

        private IOrderExecutor _orderExecutor;
        public OrderManager(IOrderExecutor orderExecutor)
        {
            _orderExecutor = orderExecutor;
            timer = new Timer(1000 * 50);
           
        }

        public void Start()
        {
            timer.AutoReset = true;//设置是执行一次（false）还是一直执行(true)；
            timer.Enabled = true;//是否执行System.Timers.Timer.Elapsed事件；
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
            LogHelper.Logger.Info("启动定时检查订单服务");
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {

            try
            {
                _orderExecutor.UpdateServiceOrder();

            }
            catch (Exception exception)
            {
                LogHelper.Logger.Debug(exception.Message);
                timer.Start();
            }
        }
    }
}