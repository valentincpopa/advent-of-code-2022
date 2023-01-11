using System.Collections.Generic;
using System.Linq;

namespace NoSpaceLeftOnDevice
{
    public class DeviceDirectory : DeviceFileSystemInfo
    {
        public DeviceDirectory(string name, DeviceDirectory parent) : base(name, 0, parent)
        {
            ChildItems = new();
        }

        public List<DeviceFileSystemInfo> ChildItems { get; set; }

        public double ComputeSize()
        {
            foreach (var childItem in ChildItems)
            {
                Size += childItem is DeviceDirectory directory
                    ? directory.ComputeSize()
                    : childItem.Size;
            }

            return Size;
        }

        public DeviceDirectory GetChildDirectory(string name)
        {
            return ChildItems
                .OfType<DeviceDirectory>()
                .First(x => x.Name == name);
        }
    }
}