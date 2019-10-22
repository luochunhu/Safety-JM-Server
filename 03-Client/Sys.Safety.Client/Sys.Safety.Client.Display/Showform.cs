using Sys.Safety.ClientFramework.CBFCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sys.Safety.Client.Display
{
    
    public  class Showform
    {
        private  string shownum = "";
        public void formshow(Dictionary<string, string> param)
        {
            if (param != null && param.Count > 0)
            {
                RequestUtil.DoMainTabChange("实时主界面");
                shownum = param["MM"].ToString();
                StaticClass.formshow(shownum);
            }

        }
    }
}
