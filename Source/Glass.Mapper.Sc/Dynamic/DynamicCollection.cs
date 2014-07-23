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
using System.Collections.Generic;
using System.Linq;
using System.Dynamic;
using System.Collections;
using System.Reflection;

namespace Glass.Mapper.Sc.Dynamic
{
    /// <summary>
    /// Class DynamicCollection
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DynamicCollection<T> : DynamicObject, IEnumerable<T> where T:class
    {
        IList<T> _collection;

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicCollection{T}"/> class.
        /// </summary>
        public DynamicCollection()
        {
            _collection = new List<T>();
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicCollection{T}"/> class.
        /// </summary>
        /// <param name="collection">The collection.</param>
        public DynamicCollection(IEnumerable<T> collection)
        {
            _collection = new List<T>(collection);
        }


        /// <summary>
        /// Adds the specified t.
        /// </summary>
        /// <param name="t">The t.</param>
        public void Add(T t)
        {
            _collection.Add(t);
        }


        /// <summary>
        /// Provides the implementation for operations that invoke a member. Classes derived from the <see cref="T:System.Dynamic.DynamicObject" /> class can override this method to specify dynamic behavior for operations such as calling a method.
        /// </summary>
        /// <param name="binder">Provides information about the dynamic operation. The binder.Name property provides the name of the member on which the dynamic operation is performed. For example, for the statement sampleObject.SampleMethod(100), where sampleObject is an instance of the class derived from the <see cref="T:System.Dynamic.DynamicObject" /> class, binder.Name returns "SampleMethod". The binder.IgnoreCase property specifies whether the member name is case-sensitive.</param>
        /// <param name="args">The arguments that are passed to the object member during the invoke operation. For example, for the statement sampleObject.SampleMethod(100), where sampleObject is derived from the <see cref="T:System.Dynamic.DynamicObject" /> class, <paramref name="args" /> is equal to 100.</param>
        /// <param name="result">The result of the member invocation.</param>
        /// <returns>true if the operation is successful; otherwise, false. If this method returns false, the run-time binder of the language determines the behavior. (In most cases, a language-specific run-time exception is thrown.)</returns>
        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            string method = binder.Name;
            
            bool hasArg = args.Length == 1;

            result = null;
            
            switch (method)
            {
                case "First":
                    result = hasArg ? _collection.First((Func<T, bool>)args[0]) : _collection.First();
                    
                    break;
                case "Last":
                    result = hasArg ? _collection.Last((Func<T, bool>)args[0]) : _collection.Last();
                    break;
                case "FirstOrDefault":
                    result = hasArg ? _collection.FirstOrDefault((Func<T, bool>)args[0]) : _collection.FirstOrDefault();
                    break;
                case "LastOrDefault":
                    result = hasArg ? _collection.LastOrDefault((Func<T, bool>)args[0]) : _collection.LastOrDefault();
                    break;
                case "Count":
                    result = _collection.Count;
                    break;
                case "ElementAt":
                    result = _collection.ElementAt((int)args[0]);
                    break;
                case "Where":
                    var arrayWhere = _collection.Where((Func<T, bool>)args[0]).Select(x=> x as T); 
                    result = new DynamicCollection<T>(arrayWhere);
                    break;
                case "Any":
                    result = hasArg ? _collection.Any((Func<T, bool>)args[0]) : _collection.Any();
                    break;
                case "All":
                    result = _collection.All((Func<T, bool>)args[0]);
                    break;
                case "Select":
                    var type =  args[0].GetType();
                    var generic2 = type.GetGenericArguments()[1];

                    var enumGeneric = typeof(DynamicCollection<>);
                    var enumType = enumGeneric.MakeGenericType(generic2);
                    
                    var list = Activator.CreateInstance(enumType);

                    foreach (var item in _collection)
                    {
                      var newItem =  type.InvokeMember("Invoke", BindingFlags.DeclaredOnly |
                            BindingFlags.Public | BindingFlags.NonPublic |
                            BindingFlags.Instance | BindingFlags.InvokeMethod, null, args[0], new object[]{ item});

                      enumType.InvokeMember("Add", BindingFlags.DeclaredOnly |
                         BindingFlags.Public | BindingFlags.NonPublic |
                         BindingFlags.Instance | BindingFlags.InvokeMethod, null, list, new object[]{newItem});

                    }
                    result = list;
                    break;


            }

            return true;
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>A <see cref="T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate through the collection.</returns>
        public IEnumerator<T> GetEnumerator()
        {
            return _collection.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return _collection.GetEnumerator();
        }

       
    }

}




