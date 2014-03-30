using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net.ObjectRenderer;

namespace Hack.Common.Framework
{
    public class Log4NetObjectLogger : IObjectRenderer
    {
        public void RenderObject(RendererMap rendererMap, object obj, TextWriter writer)
        {
            var ex = obj as Exception;

            //if its not an exception, dump it. If it's an exception, log extended details.
            if (ex == null)
            {
                //by default log up to 10 levels deep.
                ObjectDumper.Write(obj, 10, writer);
            }
            else
            {
                while (ex != null)
                {
                    RenderException(ex, writer);
                    ex = ex.InnerException;
                }
            }
        }

        private void RenderException(Exception ex, TextWriter writer)
        {
            writer.WriteLine(string.Format("Type: {0}", ex.GetType().FullName));
            writer.WriteLine(string.Format("Message: {0}", ex.Message));
            writer.WriteLine(string.Format("Source: {0}", ex.Source));
            writer.WriteLine(string.Format("TargetSite: {0}", ex.TargetSite));
            RenderExceptionData(ex, writer);
            writer.WriteLine(string.Format("StackTrace: {0}", ex.StackTrace));
        }

        private void RenderExceptionData(Exception ex, TextWriter writer)
        {
            foreach (DictionaryEntry entry in ex.Data)
            {
                writer.WriteLine(string.Format("{0}: {1}", entry.Key, entry.Value));
            }
        }
    }

}
