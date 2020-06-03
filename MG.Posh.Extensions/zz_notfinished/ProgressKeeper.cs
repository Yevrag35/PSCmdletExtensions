using System;
using System.Management.Automation;

namespace MG.Posh.Extensions.Progress
{
    public struct ProgressKeeper : IProgressKeeper
    {
        private const double ONE_HUNDRED = 100d;
        private string _statForm;

        public string Activity { get; set; }
        public string CurrentItem { get; set; }
        public int DecimalPlacesToRound { get; set; }
        public bool Enabled { get; set; }
        public int Id { get; private set; }
        public MidpointRounding Rounding { get; set; }
        public string StatusFormat
        {
            get => _statForm;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("StatusFormat cannot be a null, empty, or a whitespace string.");

                else if (!value.Contains(@"{") || !value.Contains(@"}"))
                    throw new ArgumentException("StatusFormat must be a formattable string.");

                else
                    _statForm = value;
            }
        }
        public double TotalCount { get; set; }

        public int CalculatePercentComplete(double current)
        {
            double num = Math.Round(current / this.TotalCount * ONE_HUNDRED, this.DecimalPlacesToRound, this.Rounding);
            return Convert.ToInt32(num);
        }

        public static ProgressKeeper Create(int id, string activity, string statusFormat, int total)
        {
            return Create(id, activity, statusFormat, total, 2);
        }
        public static ProgressKeeper Create(int id, string activity, string statusFormat, int total, int decimalPlacesToRound)
        {
            return new ProgressKeeper
            {
                Activity = activity,
                StatusFormat = statusFormat,
                DecimalPlacesToRound = decimalPlacesToRound,
                Enabled = true,
                Id = id,
                CurrentItem = string.Empty,
                Rounding = MidpointRounding.ToEven,
                TotalCount = total
            };
        }
        public static ProgressKeeper DefaultKeeper()
        {
            return new ProgressKeeper
            {
                Activity = "Executing",
                CurrentItem = "Item",
                DecimalPlacesToRound = 2,
                Enabled = true,
                Id = 0,
                Rounding = MidpointRounding.ToEven,
                _statForm = "Processing item {0}/{1}... {2}",
                TotalCount = 1
            };
        }

        public ProgressRecord GetRecord(double current)
        {
            string desc = !string.IsNullOrWhiteSpace(this.CurrentItem)
                ? string.Format(this.StatusFormat, current, this.TotalCount, this.CurrentItem)
                : string.Format(this.StatusFormat, current, this.TotalCount);

            var pr = new ProgressRecord(this.Id, this.Activity, desc)
            {
                RecordType = ProgressRecordType.Processing
            };
            pr.PercentComplete = this.CalculatePercentComplete(current);
            return pr;
        }
        public ProgressRecord GetRecord(double current, int parentId)
        {
            string desc = !string.IsNullOrWhiteSpace(this.CurrentItem)
                ? string.Format(this.StatusFormat, current, this.TotalCount, this.CurrentItem)
                : string.Format(this.StatusFormat, current, this.TotalCount);

            var pr = new ProgressRecord(this.Id, this.Activity, desc)
            {
                RecordType = ProgressRecordType.Processing,
                ParentActivityId = parentId
            };
            pr.PercentComplete = this.CalculatePercentComplete(current);
            return pr;
        }
        public void Continue(double current, ref ProgressRecord record)
        {
            record.PercentComplete = this.CalculatePercentComplete(current);
            record.StatusDescription = this.GetDescription(current);
        }
        public void ContinueReverse(double current, ref ProgressRecord record)
        {
            record.PercentComplete = this.CalculatePercentComplete((this.TotalCount - current));
            record.StatusDescription = this.GetOppositeDescription(current);
        }
        public void Finish(ref ProgressRecord record)
        {
            record.RecordType = ProgressRecordType.Completed;
        }

        private string GetDescription(double current)
        {
            return !string.IsNullOrWhiteSpace(this.CurrentItem)
                ? string.Format(this.StatusFormat, current, this.TotalCount, this.CurrentItem)
                : string.Format(this.StatusFormat, current, this.TotalCount);
        }
        private string GetOppositeDescription(double current)
        {
            return !string.IsNullOrWhiteSpace(this.CurrentItem)
                ? string.Format(this.StatusFormat, this.TotalCount - current, this.TotalCount, this.CurrentItem)
                : string.Format(this.StatusFormat, this.TotalCount - current, this.TotalCount);
        }
    }
}
