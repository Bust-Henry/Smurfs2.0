using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smurfs2._0.Model
{
    class LoLApi
    {

        //Attributes
        private string apiKey = null;

        //Constructors
        private LoLApi(string apiKey)
        {
            this.setApiKey(apiKey);
        }

        //Selectors
        public void setApiKey(string apiKey)
        {
            this.apiKey = apiKey;
        }
        private string getApiKey()
        {
            return this.apiKey;
        }

        //Functions
    }
}
