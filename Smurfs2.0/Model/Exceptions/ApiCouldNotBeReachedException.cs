using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smurfs2._0
{
    class ApiCouldNotBeReachedException : Exception
    {
        //Attributes
        private string message = null;

        //Constructors
        public ApiCouldNotBeReachedException()
        {
            this.message = "The Riot Games Api was not reached.Check if the Api key is valid and if the Riot Games Api is available at the moment.";
        }
        public ApiCouldNotBeReachedException(string message)
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
            return "ApiCouldNotBeReachedException: " + this.getMessage();
        }
    }
}
