using System;
using System.Collections.Generic;
using System.Text;

namespace ExchangeRateResolver.Core
{
    public abstract class Processor
    {
        protected Processor successor;

        public void SetSuccessor(Processor successor)
        {
            this.successor = successor;
        }

        public abstract void Process(object work);
    }
}
