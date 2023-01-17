namespace CathodeRayTube
{
    public class Instruction
    {
        public Instruction(int processingTime)
        {
            ProcessingTime = processingTime;
        }

        public Instruction(int processingTime, int value)
        {
            ProcessingTime = processingTime;
            Value = value;
        }

        public int ProcessingTime { get; set; }
        public int? Value { get; set; }
    }
}
