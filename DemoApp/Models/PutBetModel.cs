using System;
using System.ComponentModel.DataAnnotations;

namespace DemoApp.Models
{
    public class PutBetModel
    {
        /// <summary>
        /// 用户投注金额
        /// </summary>
        [Required(ErrorMessage = "Please enter bet amount")]
        [Range(0.000001, double.MaxValue, ErrorMessage = "Bet Amount must greater than 0")]
        public decimal BetAmount { get; set; }

        /// <summary>
        /// 用户投注种类（0:石头、1:剪刀、2:布）
        /// </summary>
        [Required(ErrorMessage = "Please enter bet option")]
        public string BetInput { get; set; }

        /// <summary>
        /// 用户投注币种(BTC、ETH、USDT)
        /// </summary>
        [Required(ErrorMessage = "Please enter betting coin (BTC, ETH, USDT")]
        public string Coin { get; set; }
    }
}
