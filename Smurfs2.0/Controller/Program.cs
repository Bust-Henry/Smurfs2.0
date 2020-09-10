using MingweiSamuel.Camille.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Smurfs2._0
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                LoLApi api = LoLApi.getInstance();
                Console.WriteLine(api.getFlexRank("4damantium", api.getRegion("euw")));
            }
            catch(ApiNotInitializableException e)
            {
                Console.WriteLine(e.toString());
            }
            catch(ApiInvalidRegionException e)
            {
                Console.WriteLine(e.toString());
            }
            catch(ApiCouldNotBeReachedException e)
            {
                Console.WriteLine(e.toString());
            }
            /*Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());*/
        }
    }
}
