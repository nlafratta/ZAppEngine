﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zirpl.Reflection
{
    internal sealed class Accessor : IAccessor
    {
        private Object _obj;
        private ITypeAccessor _typeAccessor;

        internal Accessor(Object obj, ITypeAccessor typeAccessor)
        {
            this._obj = obj;
            this._typeAccessor = typeAccessor;
        }
        public T Field<T>(string fieldName)
        {
            return this._typeAccessor.GetFieldValue<T>(this._obj, fieldName);
        }

        public T Property<T>(string propertyName)
        {
            return this._typeAccessor.GetPropertyValue<T>(this._obj, propertyName);
        }

        public void Field<T>(string fieldName, T value)
        {
            this._typeAccessor.SetFieldValue<T>(this._obj, fieldName, value);
        }

        public void Property<T>(string propertyName, T value)
        {
            this._typeAccessor.SetPropertyValue<T>(this._obj, propertyName, value);
        }

        public void Invoke(string methodName, params object[] parameters)
        {
            this._typeAccessor.InvokeMethod(this._obj, methodName, parameters);
        }

        public T Invoke<T>(string methodName, params object[] parameters)
        {
            return this._typeAccessor.InvokeMethod<T>(this._obj, methodName, parameters);
        }

        public bool HasSet(string propertyName)
        {
            return this._typeAccessor.HasPropertySetter(propertyName);
        }

        public bool HasSet(string propertyName, Type valueType)
        {
            return this._typeAccessor.HasPropertySetter(propertyName, valueType);
        }

        public bool HasSet<T>(string propertyName)
        {
            return this._typeAccessor.HasPropertySetter<T>(propertyName);
        }

        public bool HasGet(string propertyName)
        {
            return this._typeAccessor.HasPropertyGetter(propertyName);
        }

        public bool HasGet(string propertyName, Type valueType)
        {
            return this._typeAccessor.HasPropertyGetter(propertyName, valueType);
        }

        public bool HasGet<T>(string propertyName)
        {
            return this._typeAccessor.HasPropertyGetter<T>(propertyName);
        }

        public bool HasField(string fieldName)
        {
            return this._typeAccessor.HasField(fieldName);
        }

        public bool HasField(string fieldName, Type valueType)
        {
            return this._typeAccessor.HasField(fieldName, valueType);
        }

        public bool HasField<T>(string fieldName)
        {
            return this._typeAccessor.HasField<T>(fieldName);
        }

        public bool HasMethod(string methodName)
        {
            return this._typeAccessor.HasMethod(methodName);
        }

        public bool HasMethod<T>(string methodName)
        {
            return this._typeAccessor.HasMethod<T>(methodName);
        }
    }
}
