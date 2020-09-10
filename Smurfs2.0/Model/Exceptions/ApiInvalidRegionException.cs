using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smurfs2._0
{
    class ApiInvalidRegionException : Exception
    {
        //Attributes
        private string message = null;

        //Constructors
        public ApiInvalidRegionException(string message)
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
            return "ApiInvalidRegionException: " + this.getMessage();
        }
    }
}
