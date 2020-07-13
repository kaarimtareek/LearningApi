using System;
using System.Collections.Generic;
using System.Text;
using Helpers;

namespace Services.ResultObject
{
    public abstract class OperationResult  <T>
    {
        public virtual bool Success { get; set; }
        public string Code { get; set; }
        public override bool Equals(object obj)
        {
            if (obj is OperationResult<T>)
            {
                var newObj = obj as OperationResult<T>;
                if (newObj.Code != this.Code || newObj.Success != this.Success)
                {
                    return false;
                }
                return true;
            }
            return false;
        }
    }
}
