using System;
using System.Collections.Generic;
using System.Text;

namespace CommonLayer
{
    public class ResponseModel<T>
    {
        public bool success { get; set; }

        public string message { get; set; }

        public T Data { get; set; }
    }
}
