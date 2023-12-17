using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace MG.Posh.Extensions.Progress
{
    public interface IProgressKeeper
    {
        string Activity { get; set; }
        string CurrentItem { get; set; }
        int DecimalPlacesToRound { get; set; }
        bool Enabled { get; set; }
        int Id { get; }
        MidpointRounding Rounding { get; set; }
        string StatusFormat { get; set; }
        double TotalCount { get; }

        //int CalculatePercentComplete(int current);
        void Continue(double current, ref ProgressRecord record);
        void ContinueReverse(double current, ref ProgressRecord record);
        void Finish(ref ProgressRecord record);
        ProgressRecord GetRecord(double current);
        ProgressRecord GetRecord(double current, int parentId);
    }
}
