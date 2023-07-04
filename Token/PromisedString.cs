using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TASI.Token
{
    internal class PromisedString
    {
        private Task<string>? stringTask;
        private CancellationTokenSource cancellationTokenSource;
        public Task<string>? StringTask 
        {
            get
            {
                return stringTask;
            }
        }
        private string? result;

        public string Result
        {
            get
            {
                if (StringTask == null)
                { //Has already been handled
                    return result;
                }
                if (!StringTask.IsCompleted)
                {
                    StringTask.Wait();
                }
                if (StringTask.IsFaulted)
                {
                    throw StringTask.Exception;
                }
                result = StringTask.Result;
                stringTask = null;
                return result;
            }
            set
            {
                if (stringTask != null)
                {
                    //As the value gets set now, the task is no longer needed
                    cancellationTokenSource.Cancel();
                    try
                    {
                        stringTask.Wait();
                    }
                    catch(Exception)
                    {

                    }
                    finally
                    {
                        stringTask = null;
                    }
                }
                result = value;
            }
        }

        public PromisedString(CancellationTokenSource ct, Func<string> stringTask)
        {
            cancellationTokenSource = ct;
            this.stringTask = new(stringTask, cancellationTokenSource.Token);
            this.stringTask.Start();
        }

    }
}
