using System;
using System.Collections.Generic;
using System.Text;

namespace Services.ResultObject
{
    public class FailedOperationResult<T> : OperationResult<T>
    {
        public override bool Success => false;
        public string Message { get; set; }
    }
}
