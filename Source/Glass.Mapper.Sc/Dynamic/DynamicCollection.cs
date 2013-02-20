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

/*
   Copyright 2011 Michael Edwards
 
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Dynamic;
using System.Collections;
using System.Linq.Expressions;
using System.ComponentModel;
using System.Reflection;

namespace Glass.Mapper.Sc.Dynamic
{
    public class DynamicCollection<T> : DynamicObject, IEnumerable<T> where T:class
    {
        IList<T> _collection;

        public DynamicCollection()
        {
            _collection = new List<T>();
        }
        public DynamicCollection(IEnumerable<T> collection)
        {
            _collection = new List<T>(collection);
        }


        public void Add(T t)
        {
            _collection.Add(t);
        }


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

        public IEnumerator<T> GetEnumerator()
        {
            return _collection.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _collection.GetEnumerator();
        }

       
    }

}



