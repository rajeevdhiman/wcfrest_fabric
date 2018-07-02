using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Reader.ViewModels.Base
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        private Dictionary<string, object> _values = new Dictionary<string, object>();

        /// <summary>
        /// Sets the value of a property.
        /// </summary>
        /// <typeparam name="T">The type of the property value.</typeparam>
        /// <param name="propertySelector">Expression tree contains the property definition.</param>
        /// <param name="value">The property value.</param>
        protected void SetValue<T>(Expression<Func<T>> propertySelector, T value)
        {
            string propertyName = GetPropertyName(propertySelector);
            SetValue(propertyName, value);
        }

        /// <summary>
        /// Sets the value of a property.
        /// </summary>
        /// <typeparam name="T">The type of the property value.</typeparam>
        /// <param name="propertyName">The name of the property.</param>
        /// <param name="value">The property value.</param>
        protected void SetValue<T>(string propertyName, T value)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                throw new ArgumentException("ViewModelBase_GetValue_Invalid_PropertyName", propertyName);
            }

            if (_values == null)
            {
                _values = new Dictionary<string, object>();
            }

            _values[propertyName] = value;
            NotifyPropertyChanged(propertyName);
        }

        /// <summary>
        /// Gets the value of a property.
        /// </summary>
        /// <typeparam name="T">The type of the property value.</typeparam>
        /// <param name="propertySelector">Expression tree contains the property definition.</param>
        /// <returns>The value of the property or default value if not exist.</returns>
        protected T GetValue<T>(Expression<Func<T>> propertySelector)
        {
            string propertyName = GetPropertyName(propertySelector);
            return GetValue<T>(propertyName);
        }

        /// <summary>
        /// Gets the value of a property.
        /// </summary>
        /// <typeparam name="T">The type of the property value.</typeparam>
        /// <param name="propertyName">The name of the property.</param>
        /// <returns>The value of the property or default value if not exist.</returns>
        protected T GetValue<T>(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                throw new ArgumentNullException(nameof(propertyName), "ViewModelBase_GetValue_Invalid_PropertyName");
            }

            object value;

            if (_values == null)
            {
                _values = new Dictionary<string, object>();
            }

            if (!_values.TryGetValue(propertyName, out value))
            {
                value = default(T);
                _values.Add(propertyName, value);
            }

            return (T)value;
        }

       /// <summary>
        /// Raised when a property on this object has a new value.
        /// </summary>
        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises this object's PropertyChanged event.
        /// </summary>
        /// <param name="propertyName">The property that has a new value.</param>
        protected void NotifyPropertyChanged(string propertyName)
        {
            VerifyPropertyName(propertyName);

            PropertyChangedEventHandler handler = PropertyChanged;

            if (handler == null)
                return;

            var e = new PropertyChangedEventArgs(propertyName);
            handler(this, e);
        }

        protected void NotifyPropertyChanged<T>(Expression<Func<T>> propertySelector)
        {
            PropertyChangedEventHandler propertyChanged = PropertyChanged;

            if (propertyChanged == null)
                return;

            string propertyName = GetPropertyName(propertySelector);
            propertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        private static string GetPropertyName(LambdaExpression expression)
        {
            var memberExpression = expression.Body as MemberExpression;
            if (memberExpression == null)
            {
                throw new InvalidOperationException();
            }

            return memberExpression.Member.Name;
        }

        private object GetValue(string propertyName)
        {
            object value = null;
            if (_values != null && (!_values.TryGetValue(propertyName, out value)))
            {
                PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(GetType()).Find(propertyName, false);

                if (propertyDescriptor == null)
                    throw new ArgumentNullException(nameof(propertyName), "ViewModelBase_GetValue_Invalid_PropertyName");

                value = propertyDescriptor.GetValue(this);

                if (value != null)
                    _values.Add(propertyName, value);
            }

            return value;
        }

        #region Debugging

        /// <summary>
        /// Warns the developer if this object does not have
        /// a public property with the specified name. This
        /// method does not exist in a Release build.
        /// </summary>
        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        public void VerifyPropertyName(string propertyName)
        {
            if (TypeDescriptor.GetProperties(this)[propertyName] != null)
                return;

            string msg = "Invalid property name: " + propertyName;
            throw new ArgumentException(msg);
        }

        #endregion Debugging
    }
}