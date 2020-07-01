using System;
using System.Collections.Generic;
using System.Text;

namespace Services.ResultObject
{
    public class SuccessOperationResult<T> : OperationResult<T>
    {
        public T Result { get; set; }
        public override bool Success => true;
    }
}
