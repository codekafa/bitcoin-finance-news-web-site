using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BTC.Business.Managers
{
    public class ImageManager
    {

        public string SaveProfileImageOrigin(HttpPostedFile file)
        {
            string newFileName = Guid.NewGuid().ToString().Replace("-", "");
            string ext = System.IO.Path.GetExtension(file.FileName);
            newFileName = newFileName + ".png";
            string savedBaseFilePath = Path.Combine(@"\Images\_userfiles\profile\origin\", newFileName);
            file.SaveAs(savedBaseFilePath);
            return savedBaseFilePath;
        }

    }
}
