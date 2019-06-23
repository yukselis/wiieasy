using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;

namespace Arwend.Json
{
    public class DynamicJsonArray : DynamicObject, IEnumerable
    {
        JArray array;

        public DynamicJsonArray(JArray array)
        {
            this.array = array;
        }

        public override bool TryGetIndex(GetIndexBinder binder, object[] indexes, out object result)
        {
            var i = (int)indexes[0];
            if (i < 0 || i > array.Count)
                result = null;
            else
            {
                var item = array[i];
                result = DynamicJsonHelper.GetDynamicValue(item);
            }
            return true;
        }

        public override bool TrySetIndex(SetIndexBinder binder, object[] indexes, object value)
        {
            var token = JToken.FromObject(value);
            var i = (int)indexes[0];
            if (i >= array.Count)
            {
                while (array.Count != i)
                    array.Add(null);
                array.Add(token);
            }
            else
                array[i] = token;
            return true;
        }

        public IEnumerator GetEnumerator()
        {
            return new DynamicJArrayEnumerator(array.AsEnumerable().GetEnumerator());
        }


        public class DynamicJArrayEnumerator : IEnumerator
        {
            IEnumerator<JToken> enumerator;

            public DynamicJArrayEnumerator(IEnumerator<JToken> enumerator)
            {
                this.enumerator = enumerator;
            }

            public object Current
            {
                get { return DynamicJsonHelper.GetDynamicValue(enumerator.Current); }
            }

            public bool MoveNext()
            {
                return enumerator.MoveNext();
            }

            public void Reset()
            {
                enumerator.Reset();
            }
        }
    }
}
