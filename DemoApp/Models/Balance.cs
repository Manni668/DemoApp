using System;
using System.ComponentModel.DataAnnotations;

namespace DemoApp.Models
{
    public class Balance
    {
        [Key]
        public int Id { get; set; }

        public Account userId { get; set; }

        /// <summary>
        /// 人民币余额
        /// </summary>
        public decimal CNY { get; set; }

        /// <summary>
        /// USD余额
        /// </summary>
        public decimal USD { get; set; }

        /// <summary>
        /// 泰铢余额
        /// </summary>
        public decimal THB { get; set; }
        /// <summary>
        /// 越南盾余额
        /// </summary>
        public decimal VND { get; set; }
    }
}
