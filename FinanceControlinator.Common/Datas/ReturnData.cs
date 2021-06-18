using System.Collections.Generic;

namespace FinanceControlinator.Common.Datas
{
    public class ReturnData
    {
        public ReturnData(IEnumerable<object> data)
        {
            Data = data;
        }

        public IEnumerable<object> Data { get; set; }

    }
}
