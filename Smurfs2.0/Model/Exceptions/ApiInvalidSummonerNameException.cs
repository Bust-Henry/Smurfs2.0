using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smurfs2._0
{
    class ApiInvalidSummonerNameException : Exception
    {
        //Attributes
        private string message = null;

        //Constructors

        public ApiInvalidSummonerNameException(string message)
        {
            this.message = message;
        }

        //Selectors
        private void setMessage(string message)
        {
            this.message = message;
        }
        private string getMessage()
        {
            return this.message;
        }

        //Functions
        public string toString()
        {
            return "ApiInvalidSummonerNameException: " + this.getMessage();
        }
    }
}
