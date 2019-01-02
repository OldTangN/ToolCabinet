using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolMgt.Model
{
    public partial class LogInModel : ObservableObject
    {
        /// <summary>
        /// 用户名或卡号
        /// </summary>
        public string NameOrCard { get => nameOrCard; set => Set(ref nameOrCard, value); }

        /// 密码
        /// </summary>
        public string Pwd { get => pwd; set => Set(ref pwd, value); }

        /// <summary>
        /// 是否卡号登录
        /// </summary>
        public bool IsCard { get => isCard; set => Set(ref isCard, value); }

        private string nameOrCard = "";
        private string pwd = "";
        private bool isCard = false;
    }
}
