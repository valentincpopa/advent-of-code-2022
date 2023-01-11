namespace NoSpaceLeftOnDevice
{
    public class DeviceFileSystemInfo
    {
        public DeviceFileSystemInfo(string name, double size, DeviceDirectory parent)
        {
            Name = name;
            Size = size;
            Parent = parent;
        }

        public DeviceDirectory Parent { get; set; }
        public string Name { get; set; }
        public double Size { get; set; }
    }
}