using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolMgt.Model
{
    public partial class User
    {

        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrMsg
        {
            get => errMsg;
            set
            {
                Set(ref errMsg, value); IsError = !string.IsNullOrEmpty(value);
            }
        }

        /// <summary>
        /// 是否错误
        /// </summary>
        public bool IsError { get => isError; private set => Set(ref isError, value); }

        private string errMsg;
        private bool isError;
    }
}
