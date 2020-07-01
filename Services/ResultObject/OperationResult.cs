using System;
using System.Collections.Generic;
using System.Text;

namespace Services.ResultObject
{
    public abstract class OperationResult  <T>
    {
        public virtual bool Success { get; set; }
        public string Code { get; set; }
    }
}
