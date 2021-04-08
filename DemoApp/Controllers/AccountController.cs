using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Threading.Tasks;
using DemoApp.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using DemoApp.Data;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace DemoApp.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : Controller
    {

        private IConfiguration _config;
        private readonly ILogger<AccountController> _logger;

        private readonly DemoDbContext _context;

        //Constructor
        public AccountController(IConfiguration config, DemoDbContext context, ILogger<AccountController> logger)
        {
            _config = config;
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// 生成Json web Token 
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        private string GenerateJSWToken(Login userInfo)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Issuer"],
              null,
              expires: DateTime.Now.AddMinutes(120),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        /// <summary>
        /// 用户登陆
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        private async Task<Login> AuthenticateUser(Login login)
        {
            //Login user = null;
            var user = _context.Accounts.Where(b => b.UserId == login.Id);
            // if (login.UserName == "Jay")
            // {
            //     user = new Login { UserName = "Jay", Id = 12345 };
            // }
            return (Login)user;


        }
        /// <summary>
        /// 使用Token登陆
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost(nameof(Login))]
        public async Task<IActionResult> Login([FromBody] Login data)
        {
            IActionResult response = Unauthorized();
            var user = await AuthenticateUser(data);
            if (data != null)
            {
                var tokenString = GenerateJSWToken(user);
                response = Ok(new { Token = tokenString, Message = " Login Success" });
            }

            return response;
        }

        /// <summary>
        /// 生成用户名与ID
        /// </summary>
        /// <returns></returns>
        [HttpPut("CreateAccount")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(string), 200)]
        public IActionResult CreateAccount()
        {
            char[] lowers = new char[] { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'j', 'k', 'm', 'n', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };

            //随机生成用户名 
            Random random = new Random();
            string name = "";
            for (int i = 0; i < 5; i++)
            {
                name += lowers[random.Next(0, lowers.Length)].ToString();
            }

            //生成用户ID
            int userId = random.Next(1, 99999);
            //检查是否id重复
            var exists = _context.Accounts.Any(o => o.UserId == userId);
            while (exists)
            {
                userId = random.Next(1, 99999);
                exists = _context.Accounts.Any(o => o.UserId == userId);
            }

            //保存到数据库
            var account = new Account
            {
                UserName = name,
                UserId = userId
            };
            _context.Accounts.Add(account);
            _context.SaveChanges();

            //用户默认余额
            var balance = new Balance
            {
                USD = 100,
                CNY = 1000,
                THB = 10000,
                VND = 100000
            };
            _context.Balances.Add(balance);

            return Ok(account);
        }
        /// <summary>
        /// 获取用户钱包
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAccountBalance")]
        [ProducesResponseType(typeof(Account), 200)]
        public async Task<IActionResult> GetAccountBalance([Required] int userId)
        {

            //通过id查找
            var coin = _context.Balances.Find(userId);
            // 返回所有coin信息
            Dictionary<string, decimal> dict = new Dictionary<string, decimal>()
                {
                    {"USD", coin.USD },
                    {"CNY", coin.CNY },
                    {"VND", coin.VND },
                    {"THB", coin.THB },
                };

            return Ok(dict);

        }
        /// <summary>
        /// 获取用户法币余额
        /// </summary>
        /// <returns>输入Coin的金额</returns>
        [HttpGet("GetCoinBalance")]
        [ProducesResponseType(typeof(Account), 200)]
        public async Task<IActionResult> GetCoinBalance([Required] int userId, Balance coin)
        {
            if (coin == null)
            {
                return BadRequest("please enter coin Name");
            }
            //通过id查找
            var balance = _context.Balances.Where(x => x.userId.UserId == userId);
            //查找对应的Coin
            var amount = balance.Where(o => o == coin).FirstOrDefault();

            return Ok(amount);

        }
        /// <summary>
        /// 投注
        /// </summary>
        /// <returns></returns>
        [HttpPut("PutBet")]
        [ProducesResponseType(typeof(BetResult), 200)]
        public async Task<IActionResult> PutBetAsync(PutBetModel model)
        {

            //输赢结果
            var result = 1;
            //输赢金额
            decimal amount = 0;

            Random random = new Random(DateTime.Now.Millisecond);
            var npc = random.Next(0, 3);
            //生成订单号
            var orderNumber = Guid.NewGuid().ToString();

            //记录投注结果
            //投注时间 - 提交的玩法 - 金额- 结果 - 随机订单号
            System.DateTime currentTime = DateTime.Now;

            var betResult = new BetResult()
            {
                Createtime = currentTime,
                PlayerBet = model.BetInput,
                PcBet = npc.ToString(),
                Amount = amount,
                ResultAmount = amount,
                OrderNumber = orderNumber,
                Coin = model.Coin,
                Result = result
            };

            //加入数据库
            //_context.betResult.Add(betResult);
            return Ok(betResult);
        }

    }
}