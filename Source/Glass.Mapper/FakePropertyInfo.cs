/*
   Copyright 2012 Michael Edwards
 
   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
 
*/ 
//-CRE-


using System;
using System.Reflection;

namespace Glass.Mapper
{
    /// <summary>
    /// This is used to fake the System.Reflection.PropertyInfo class when needed.
    /// </summary>
    public class FakePropertyInfo : PropertyInfo
    {

        Type _propertyType;
	    Type _declaringType;

        string _name;

	    /// <summary>
	    /// Initializes a new instance of the <see cref="FakePropertyInfo"/> class.
	    /// </summary>
	    /// <param name="propertyType">Type of the property.</param>
	    /// <param name="declaringType">Type that declares this property member</param>
        public FakePropertyInfo(Type propertyType, Type declaringType)
        {
            _propertyType = propertyType;
            _declaringType = declaringType;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="FakePropertyInfo"/> class.
        /// </summary>
        /// <param name="propertyType">Type of the property.</param>
		/// <param name="name">The name.</param>
		/// <param name="declaringType">Type that declares this property member</param>
        public FakePropertyInfo(Type propertyType, string name, Type declaringType)
        {
            _propertyType = propertyType;
             _declaringType = declaringType;
            _name = name;
        }

        /// <summary>
        /// Gets the attributes for this property.
        /// </summary>
        /// <value>The attributes.</value>
        /// <exception cref="System.NotImplementedException"></exception>
        /// <returns>Attributes of this property.</returns>
        public override PropertyAttributes Attributes
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Gets a value indicating whether the property can be read.
        /// </summary>
        /// <value><c>true</c> if this instance can read; otherwise, <c>false</c>.</value>
        /// <exception cref="System.NotImplementedException"></exception>
        /// <returns>true if this property can be read; otherwise, false.</returns>
        public override bool CanRead
        {
			get { return _declaringType.GetProperty(_name).CanRead; }
        }

        /// <summary>
        /// Gets a value indicating whether the property can be written to.
        /// </summary>
        /// <value><c>true</c> if this instance can write; otherwise, <c>false</c>.</value>
        /// <exception cref="System.NotImplementedException"></exception>
        /// <returns>true if this property can be written to; otherwise, false.</returns>
        public override bool CanWrite
        {
			get { return _declaringType.GetProperty(_name).CanWrite; }
        }

        /// <summary>
        /// Returns an array whose elements reflect the public and, if specified, non-public get, set, and other accessors of the property reflected by the current instance.
        /// </summary>
        /// <param name="nonPublic">Indicates whether non-public methods should be returned in the MethodInfo array. true if non-public methods are to be included; otherwise, false.</param>
        /// <returns>An array of <see cref="T:System.Reflection.MethodInfo" /> objects whose elements reflect the get, set, and other accessors of the property reflected by the current instance. If <paramref name="nonPublic" /> is true, this array contains public and non-public get, set, and other accessors. If <paramref name="nonPublic" /> is false, this array contains only public get, set, and other accessors. If no accessors with the specified visibility are found, this method returns an array with zero (0) elements.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public override MethodInfo[] GetAccessors(bool nonPublic)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// When overridden in a derived class, returns the public or non-public get accessor for this property.
        /// </summary>
        /// <param name="nonPublic">Indicates whether a non-public get accessor should be returned. true if a non-public accessor is to be returned; otherwise, false.</param>
        /// <returns>A MethodInfo object representing the get accessor for this property, if <paramref name="nonPublic" /> is true. Returns null if <paramref name="nonPublic" /> is false and the get accessor is non-public, or if <paramref name="nonPublic" /> is true but no get accessors exist.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public override MethodInfo GetGetMethod(bool nonPublic)
        {
			return _declaringType.GetProperty(_name).GetGetMethod(nonPublic);
        }

        /// <summary>
        /// When overridden in a derived class, returns an array of all the index parameters for the property.
        /// </summary>
        /// <returns>An array of type ParameterInfo containing the parameters for the indexes.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public override ParameterInfo[] GetIndexParameters()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// When overridden in a derived class, returns the set accessor for this property.
        /// </summary>
        /// <param name="nonPublic">Indicates whether the accessor should be returned if it is non-public. true if a non-public accessor is to be returned; otherwise, false.</param>
        /// <returns>Value Condition A <see cref="T:System.Reflection.MethodInfo" /> object representing the Set method for this property. The set accessor is public.-or- <paramref name="nonPublic" /> is true and the set accessor is non-public. null<paramref name="nonPublic" /> is true, but the property is read-only.-or- <paramref name="nonPublic" /> is false and the set accessor is non-public.-or- There is no set accessor.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public override MethodInfo GetSetMethod(bool nonPublic)
        {
	        return _declaringType.GetProperty(_name).GetSetMethod(nonPublic);
        }

        /// <summary>
        /// When overridden in a derived class, returns the value of a property having the specified binding, index, and <see cref="T:System.Globalization.CultureInfo" />.
        /// </summary>
        /// <param name="obj">The object whose property value will be returned.</param>
        /// <param name="invokeAttr">The invocation attribute. This must be a bit flag from BindingFlags : InvokeMethod, CreateInstance, Static, GetField, SetField, GetProperty, or SetProperty. A suitable invocation attribute must be specified. If a static member is to be invoked, the Static flag of BindingFlags must be set.</param>
        /// <param name="binder">An object that enables the binding, coercion of argument types, invocation of members, and retrieval of MemberInfo objects via reflection. If <paramref name="binder" /> is null, the default binder is used.</param>
        /// <param name="index">Optional index values for indexed properties. This value should be null for non-indexed properties.</param>
        /// <param name="culture">The CultureInfo object that represents the culture for which the resource is to be localized. Note that if the resource is not localized for this culture, the CultureInfo.Parent method will be called successively in search of a match. If this value is null, the CultureInfo is obtained from the CultureInfo.CurrentUICulture property.</param>
        /// <returns>The property value for <paramref name="obj" />.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public override object GetValue(object obj, BindingFlags invokeAttr, Binder binder, object[] index, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the type of this property.
        /// </summary>
        /// <value>The type of the property.</value>
        /// <returns>The type of this property.</returns>
        public override Type PropertyType
        {
            get { return _propertyType; }
        }

        /// <summary>
        /// When overridden in a derived class, sets the property value for the given object to the given value.
        /// </summary>
        /// <param name="obj">The object whose property value will be set.</param>
        /// <param name="value">The new value for this property.</param>
        /// <param name="invokeAttr">The invocation attribute. This must be a bit flag from <see cref="T:System.Reflection.BindingFlags" /> : InvokeMethod, CreateInstance, Static, GetField, SetField, GetProperty, or SetProperty. A suitable invocation attribute must be specified. If a static member is to be invoked, the Static flag of BindingFlags must be set.</param>
        /// <param name="binder">An object that enables the binding, coercion of argument types, invocation of members, and retrieval of <see cref="T:System.Reflection.MemberInfo" /> objects through reflection. If <paramref name="binder" /> is null, the default binder is used.</param>
        /// <param name="index">Optional index values for indexed properties. This value should be null for non-indexed properties.</param>
        /// <param name="culture">The <see cref="T:System.Globalization.CultureInfo" /> object that represents the culture for which the resource is to be localized. Note that if the resource is not localized for this culture, the CultureInfo.Parent method will be called successively in search of a match. If this value is null, the CultureInfo is obtained from the CultureInfo.CurrentUICulture property.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public override void SetValue(object obj, object value, BindingFlags invokeAttr, Binder binder, object[] index, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the class that declares this member.
        /// </summary>
        /// <value>The type of the declaring.</value>
        /// <exception cref="System.NotImplementedException"></exception>
        /// <returns>The Type object for the class that declares this member.</returns>
        public override Type DeclaringType
        {
            get { return _declaringType; }
        }

        /// <summary>
        /// When overridden in a derived class, returns an array of custom attributes identified by <see cref="T:System.Type" />.
        /// </summary>
        /// <param name="attributeType">The type of attribute to search for. Only attributes that are assignable to this type are returned.</param>
        /// <param name="inherit">Specifies whether to search this member's inheritance chain to find the attributes.</param>
        /// <returns>An array of custom attributes applied to this member, or an array with zero (0) elements if no attributes have been applied.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public override object[] GetCustomAttributes(Type attributeType, bool inherit)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// When overridden in a derived class, returns an array containing all the custom attributes.
        /// </summary>
        /// <param name="inherit">Specifies whether to search this member's inheritance chain to find the attributes.</param>
        /// <returns>An array that contains all the custom attributes, or an array with zero elements if no attributes are defined.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public override object[] GetCustomAttributes(bool inherit)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// When overridden in a derived class, indicates whether one or more instance of <paramref name="attributeType" /> is applied to this member.
        /// </summary>
        /// <param name="attributeType">The Type object to which the custom attributes are applied.</param>
        /// <param name="inherit">Specifies whether to search this member's inheritance chain to find the attributes.</param>
        /// <returns>true if one or more instance of <paramref name="attributeType" /> is applied to this member; otherwise false.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public override bool IsDefined(Type attributeType, bool inherit)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the name of the current member.
        /// </summary>
        /// <value>The name.</value>
        /// <returns>A <see cref="T:System.String" /> containing the name of this member.</returns>
        public override string Name
        {
            get { return _name; }
        }

        /// <summary>
        /// Gets the class object that was used to obtain this instance of MemberInfo.
        /// </summary>
        /// <value>The type of the reflected.</value>
        /// <returns>The Type object through which this MemberInfo object was obtained.</returns>
        public override Type ReflectedType
        {
            get { return typeof(FakePropertyInfo); }
        }
    }
}




