using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ToolMgt.BLL;
using ToolMgt.Common;
using ToolMgt.Model;

namespace ToolMgt.UI.ViewModel
{
    public class MainViewModel : ViewModelBase
    {

        private ICommand _testCmd;
        public ICommand TestCmd
        {
            get
            {
                if (_testCmd == null)
                {
                    _testCmd = new RelayCommand(Test, CanTest);
                }
                return _testCmd;
            }
        }

        private void Test(object obj)
        {

        }

        private bool CanTest(object obj)
        {
            return true;
        }
    }
}
