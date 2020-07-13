using System;
using System.Collections.Generic;
using System.Text;
using Helpers;

namespace Services.ResultObject
{
    public class SuccessOperationResult<T> : OperationResult<T>
    {
        public T Result { get; set; }
        public override bool Success => true;
        public override bool Equals(object obj)
        {   if (obj == null)
                return false;
            if(typeof(SuccessOperationResult<T>)!= obj.GetType())
            {
                return false;
            }
            var object2 = obj as SuccessOperationResult<T>;
           return GenericComparer.Compare(this, object2);
        }
    }
}
