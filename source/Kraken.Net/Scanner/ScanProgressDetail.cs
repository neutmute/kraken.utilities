using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kraken.Net
{

    public class ScanProgressDetailEventArgs : EventArgs
    {
        public ScanProgressDetail Item { get; set; }
    }

    public class ScanProgressDetail
    {
        public string Device { get; set; }

        public string Operation { get; set; }

        public string Status { get; set; }

        public bool IsCleanable { get; set; }

        public DateTime DateUpdated { get; set; }

        public object Tag { get; set; }

        public ScanProgressDetail()
        {
            DateUpdated = DateTime.Now;
        }

        public ScanProgressDetail(object device, string operation, string status = null, bool isCleanable = false): this()
        {
            Device = device.ToString();
            Status = status;
            Operation = operation;
            IsCleanable = isCleanable;
        }

        public override bool Equals(object obj)
        {
            ScanProgressDetail otherObject = obj as ScanProgressDetail;
            return otherObject != null && otherObject.Device == Device;
        }

        public override int GetHashCode()
        {
            return Device.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("Device={0}, Operation={1}, Status={2}", Device, Operation, Status);
        }
    }
}
