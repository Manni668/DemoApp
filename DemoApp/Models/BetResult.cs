using System;
namespace DemoApp.Models
{
    public class BetResult
    {

        /// <summary>
        /// 创建订单时间
        /// </summary>
        public DateTime Createtime { get; set; }

        /// <summary>
        /// 用户玩法
        /// </summary>
        public string PlayerBet { get; set; }

        /// <summary>
        /// 电脑玩法
        /// </summary>
        public string PcBet { get; set; }

        /// <summary>
        /// 用户投注币种(BTC、ETH、USDT)
        /// </summary>
        public string Coin { get; set; }

        /// <summary>
        /// 用户投注金额
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// 输赢金额
        /// </summary>
        public decimal ResultAmount { get; set; }

        /// <summary>
        ///订单号
        /// </summary>
        public string OrderNumber { get; set; }
        /// <summary>
        /// 输赢结果（0：平；1：赢；-1：输）
        /// </summary>
        public int Result { get; set; }
    }
}
