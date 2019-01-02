using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ToolMgt.Model
{
    public abstract class ObservableObject : INotifyPropertyChanged
    {
        #region 观察者模式

        /// <summary>
        /// 属性改变事件
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// 设置值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="field">字段</param>
        /// <param name="newValue">新值</param>
        /// <param name="propertyName">属性名</param>
       protected void Set<T>(ref T field, T newValue, [System.Runtime.CompilerServices.CallerMemberName] string propertyName = "")
        {
            if (!EqualityComparer<T>.Default.Equals(field, newValue))
            {
                field = newValue;
                RaisePropertyChanged(propertyName);
            }
        }

        /// <summary>
        /// 触发属性改变事件
        /// </summary>
        /// <param name="propertyName">属性名</param>
        void RaisePropertyChanged(string propertyName)
        {
           PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// 获取属性名
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyExpression">表达式 p=>p.XXX</param>
        /// <returns></returns>
        public string GetPropertyName<T>(Expression<Func<T>> propertyExpression)
        {
            if (propertyExpression == null)
            {
                throw new ArgumentNullException("propertyExpression");
            }

            var body = propertyExpression.Body as MemberExpression;

            if (body == null)
            {
                throw new ArgumentException("Invalid argument", "propertyExpression");
            }

            var property = body.Member as PropertyInfo;

            if (property == null)
            {
                throw new ArgumentException("Argument is not a property", "propertyExpression");
            }

            return property.Name;
        }

        #endregion
    }
}
